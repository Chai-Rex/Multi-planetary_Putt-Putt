using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    protected Canvas _currentUI;

    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainmenuButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;

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
        settingsButton.onClick.AddListener(() => SetSettings());
        mainmenuButton.onClick.AddListener(() => LevelManager.Instance.LoadMainMenu());
        retryButton.onClick.AddListener(() => LevelManager.Instance.ReloadCurrentLevel());
        quitButton.onClick.AddListener(() => Application.Quit());
        settingsCanvas.gameObject.SetActive(false);
    }

    public bool SetSettings()
    {
        if (settingsCanvas.gameObject.activeSelf)
        {
            ToggleCurrentUI();
            isInSettings = false;
            Time.timeScale = 1.0f;

            return false;
        }

        isInSettings = true;
        Time.timeScale = 0.0f;
        SwitchCurrentUI(settingsCanvas);
        return true;
    }

    private void MenuAction_Started(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!ResultsManager.Instance.GetIsInResults())
        {
            SetSettings();
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
