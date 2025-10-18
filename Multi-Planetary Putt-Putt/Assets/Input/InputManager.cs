using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour {

    public static InputManager Instance = null;

    public event UnityAction<string> OnControlsChanged;

    // In game
    public InputAction MoveAction { get; private set; }
    public InputAction InteractAction { get; private set; }
    public InputAction MenuAction { get; private set; }


    private InputActionAsset _inputActionAsset;
    public InputActionAsset PlayerInputActionAsset { get { return _inputActionAsset; } }
    private PlayerInput _playerInput;
    private Gamepad _gamepad;

    private void Awake() {

        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        _playerInput = GetComponent<PlayerInput>();

        _playerInput.onControlsChanged += PlayerInput_onControlsChanged;

        _inputActionAsset = _playerInput.actions;

        GetInputActions();
    }

    private void OnDestroy() {
        _playerInput.onControlsChanged -= PlayerInput_onControlsChanged;
    }

    private void PlayerInput_onControlsChanged(PlayerInput obj) {
        if (_playerInput.currentControlScheme == "Gamepad") _gamepad = Gamepad.current;

        OnControlsChanged?.Invoke(_playerInput.currentControlScheme);
    }

    private void GetInputActions() {
        MoveAction = _inputActionAsset.FindAction("Move");
        InteractAction = _inputActionAsset.FindAction("Interact");
        MenuAction = _inputActionAsset.FindAction("Menu");

        MoveAction.Enable();
        InteractAction.Enable();
        MenuAction.Enable();
    }

    #region Rumble
    public void SetRumble(float lowFrequency, float highFrequency) {

        if (_playerInput.currentControlScheme != "Gamepad") return;
        _gamepad.SetMotorSpeeds(lowFrequency, highFrequency);
    }

    public void StopRumble() {

        if (_playerInput.currentControlScheme != "Gamepad") return;
        _gamepad.SetMotorSpeeds(0f, 0f);
    }

    public void RumbleDuration(AnimationCurve lowFrequencyCurve, AnimationCurve highFrequencyCurve, float duration) {

        if (_playerInput.currentControlScheme != "Gamepad") return;
        StopAllCoroutines();
        StartCoroutine(RumbleRoutine(lowFrequencyCurve, highFrequencyCurve, duration, _gamepad));
    }

    private IEnumerator RumbleRoutine(AnimationCurve lowFrequencyCurve, AnimationCurve highFrequencyCurve, float duration, Gamepad gamepad) {

        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float percentCompleted = elapsedTime / duration;
            _gamepad.SetMotorSpeeds(lowFrequencyCurve.Evaluate(percentCompleted), highFrequencyCurve.Evaluate(percentCompleted));
            yield return null;
        }

        gamepad.SetMotorSpeeds(0f, 0f);
    }
    #endregion


}
