using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour {


    [SerializeField] private float force = 1f;
    [SerializeField] private Indicator indicator;


    [SerializeField] private Rigidbody2D rigidbody2d;

    private Vector3 savePosition;

    private void Start() {
        savePosition = transform.position;

        GravityManager.attractees.Add(rigidbody2d);

        indicator.transform.position = transform.position;

        InputManager.Instance.InteractAction.started += InteractAction_started;
    }

    private void InteractAction_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        HitBallRoutine();
    }


    private async void HitBallRoutine() {
        indicator.gameObject.SetActive(false);
        rigidbody2d.AddForce(indicator.transform.up * force);
        await Awaitable.FixedUpdateAsync();
        while (rigidbody2d.linearVelocity.magnitude > .1f) {
            if (IsOutOfBounds()) {
                rigidbody2d.linearVelocity = Vector2.zero;
                await Awaitable.FixedUpdateAsync();
                transform.position = savePosition;
                indicator.gameObject.SetActive(true);
                indicator.transform.position = transform.position;
                return;
            }
            await Task.Yield();
        }
        rigidbody2d.linearVelocity = Vector2.zero;
        await Awaitable.FixedUpdateAsync();
        indicator.transform.position = transform.position;
        indicator.gameObject.SetActive(true);
        savePosition = transform.position;
    }


    private bool IsOutOfBounds(float padding = 0.5f) {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

        // Viewport coordinates: (0,0) is bottom-left, (1,1) is top-right
        // Add padding in viewport space (0.1 = 10% of screen)
        if (viewportPos.x < -padding || viewportPos.x > 1 + padding ||
            viewportPos.y < -padding || viewportPos.y > 1 + padding) {
            // Object is off screen
            Debug.Log("Object left the screen!");
            return true;
        }
        return false;
    }
}
