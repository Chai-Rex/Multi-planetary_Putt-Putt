using System.Threading.Tasks;
using UnityEngine;

public class Indicator : MonoBehaviour {
    [Header("Rotation")]
    [SerializeField] private float rotationMaxSpeed = 100f;
    [SerializeField] private float rotationTime = 1f;
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Scale")]
    [SerializeField] private float scaleMaxSpeed = 1f;
    [SerializeField] private float scaleTime = 1f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 3f;

    private bool isMoving = false;
    private float rotationElapsedTime = 0f;
    private float scaleElapsedTime = 0f;

    private void OnEnable() {
        InputManager.Instance.MoveAction.started += MoveAction_started;
        InputManager.Instance.MoveAction.canceled += MoveAction_canceled;
    }

    private void OnDisable() {
        InputManager.Instance.MoveAction.started -= MoveAction_started;
        InputManager.Instance.MoveAction.canceled -= MoveAction_canceled;
        isMoving = false;
    }

    private void MoveAction_started(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (isMoving) return;
        isMoving = true;
        UpdateAsync();
    }

    private void MoveAction_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        isMoving = false;
        rotationElapsedTime = 0f;
        scaleElapsedTime = 0f;
    }

    private async void UpdateAsync() {
        while (isMoving && this != null) {
            Vector2 input = InputManager.Instance.MoveAction.ReadValue<Vector2>();

            // Handle rotation with curve
            if (input.x != 0) {
                rotationElapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(rotationElapsedTime / rotationTime);
                float curveValue = rotationCurve.Evaluate(t);
                float rotationAmount = -input.x * rotationMaxSpeed * curveValue * Time.deltaTime;
                transform.Rotate(0f, 0f, rotationAmount);
            } else {
                rotationElapsedTime = 0f;
            }

            // Handle scale with curve
            if (input.y != 0) {
                scaleElapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(scaleElapsedTime / scaleTime);
                float curveValue = scaleCurve.Evaluate(t);
                float scaleAmount = input.y * scaleMaxSpeed * curveValue * Time.deltaTime;

                Vector3 newScale = transform.localScale + new Vector3(0f, scaleAmount, 0f);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                transform.localScale = newScale;
            } else {
                scaleElapsedTime = 0f;
            }

            await Task.Yield();
        }
    }
}