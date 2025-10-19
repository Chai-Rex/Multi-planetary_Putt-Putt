using System.Threading.Tasks;
using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField] private float force = 1f;
    public float LaunchForce {  get { return force; } }
    [SerializeField] private float outOfBoundsPadding = 0.1f;
    [SerializeField] private float stoppingVelocityThreshold = 0.1f;
    public float StopVelocity { get { return stoppingVelocityThreshold; } }
    [SerializeField] private Indicator indicator;
    [SerializeField] private Rigidbody2D rb;
    public Rigidbody2D RB {  get { return rb; } }

    private Vector3 _lastStablePosition;
    private bool _isHitRoutineRunning;

    private void Start() {
        InitializeBall();
        InputManager.Instance.InteractAction.started += OnInteract;
    }

    private void OnDestroy() {
        if (InputManager.Instance != null) {
            InputManager.Instance.InteractAction.started -= OnInteract;
        }
        GravityManager.attractees.Remove(rb);
        _isHitRoutineRunning = false;
    }

    private void InitializeBall() {
        rb.bodyType = RigidbodyType2D.Kinematic;
        _lastStablePosition = transform.position;
        GravityManager.attractees.Add(rb);
        indicator.TargetBall = this;
        UpdateIndicatorPosition();
    }

    private void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (!_isHitRoutineRunning) {
            HitBallAsync();
        }
    }

    private async void HitBallAsync() {
        _isHitRoutineRunning = true;

        LaunchBall();
        await Awaitable.FixedUpdateAsync();

        bool shouldReset = await WaitForBallToStop();

        if (shouldReset) {
            ResetBall();
        } else {
            StabilizeBall();
        }

        _isHitRoutineRunning = false;
    }

    private void LaunchBall() {
        rb.bodyType = RigidbodyType2D.Dynamic;
        indicator.gameObject.SetActive(false);
        //indicator.SetPredictionLineVisible(false);

        Vector2 launchDirection = indicator.transform.up * indicator.transform.localScale.y;
        rb.AddForce(launchDirection * force, ForceMode2D.Impulse);
    }

    private async Task<bool> WaitForBallToStop() {
        while (rb.linearVelocity.magnitude > stoppingVelocityThreshold) {
            if (IsOutOfBounds()) {
                return true; // Signal to reset
            }
            await Task.Yield();
        }
        return false; // Ball stopped naturally
    }

    private void ResetBall() {
        rb.linearVelocity = Vector2.zero;
        transform.position = _lastStablePosition;
        rb.bodyType = RigidbodyType2D.Kinematic;
        UpdateIndicatorPosition();
        indicator.gameObject.SetActive(true);
    }

    private void StabilizeBall() {
        rb.linearVelocity = Vector2.zero;
        _lastStablePosition = transform.position;
        rb.bodyType = RigidbodyType2D.Kinematic;
        UpdateIndicatorPosition();
        indicator.gameObject.SetActive(true);
    }

    private void UpdateIndicatorPosition() {
        indicator.transform.position = transform.position;
        indicator.SetPredictionLineVisible(true);
    }

    private bool IsOutOfBounds() {

        if (GravityManager.Instance.HasAttractors()) return false;      
        
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        return viewportPos.x < -outOfBoundsPadding || viewportPos.x > 1 + outOfBoundsPadding ||
               viewportPos.y < -outOfBoundsPadding || viewportPos.y > 1 + outOfBoundsPadding;
    }
}