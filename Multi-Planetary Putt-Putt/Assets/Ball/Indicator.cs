using System.Threading.Tasks;
using UnityEngine;

public class Indicator : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float scaleSpeed = 100f;
    private bool isMoving = false;

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
        RotateAsync();
    }

    private void MoveAction_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        isMoving = false;
    }

    private async void RotateAsync() {
        while (isMoving) {
            Vector2 input = InputManager.Instance.MoveAction.ReadValue<Vector2>();

            if (input.x != 0) {
                transform.Rotate(0f, 0f, -input.x * rotationSpeed * Time.deltaTime);
            }
            if (input.y != 0) {
                transform.localScale += new Vector3(0f, input.y * scaleSpeed * Time.deltaTime, 0f);
            }

            await Task.Yield();
        }
    }
}
