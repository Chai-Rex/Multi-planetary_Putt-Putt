using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    protected Canvas _currentUI;

    [Header("Canvas")]
    [SerializeField] private Canvas settingsCanvas;

    private bool isInSettings = false;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        InputManager.Instance.MenuAction.started += MenuAction_Started;
    }

    private void OnDisable()
    {
        InputManager.Instance.MenuAction.started -= MenuAction_Started;
    }

    private void Start()
    {
        settingsCanvas.gameObject.SetActive(false);
    }

    public bool SetSettings()
    {
        if (settingsCanvas.gameObject.activeSelf)
        {
            ToggleCurrentUI();
            return false;
        }
        SwitchCurrentUI(settingsCanvas);
        return true;
    }

    private void MenuAction_Started(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isInSettings = !isInSettings;

        if (isInSettings) 
        { 
            Time.timeScale = 0.0f; 
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }


    public string GetMode()
    {
        if (_currentUI == settingsCanvas) return "Setting";
        return null;
    }


    public bool IsCurrentUIVisible()
    {
        return _currentUI.gameObject.activeSelf;
    }

    public bool ToggleCurrentUI()
    {

        bool isActive = _currentUI.gameObject.activeSelf;
        if (isActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        isActive = !isActive;
        _currentUI.gameObject.SetActive(isActive);
        return isActive;
    }

    public void SwitchCurrentUI(Canvas newCurrentUI)
    {
        _currentUI?.gameObject.SetActive(false);
        _currentUI = newCurrentUI;
        ToggleCurrentUI();
    }

    public void HideCurrentCanvas()
    {
        _currentUI.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool GetIsInSettings()
    {
        return isInSettings;
    }

}
