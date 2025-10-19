using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_SettingsManager : MonoBehaviour
{

    public static UI_SettingsManager Instance;

    [SerializeField] private SettingsSO settings;
    public SettingsSO SO { get { return settings; } }

    private string SavePath => Path.Combine(Application.persistentDataPath, "settings.json");

    private void Awake()
    {
        Instance = this;

        if (!LoadSettings())
        {
            // If no save file exists, initialize defaults
            settings.MusicVolume = 0.5f;
            settings.EffectsVolume = 0.5f;
        }
    }

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(settings, true); // pretty print for debugging
        File.WriteAllText(SavePath, json);
#if UNITY_EDITOR
        Debug.Log($"Settings saved to {SavePath}");
#endif
    }

    /// <summary>
    /// Loads settings from disk. Returns true if successful.
    /// </summary>
    private bool LoadSettings()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            JsonUtility.FromJsonOverwrite(json, settings);
#if UNITY_EDITOR
            Debug.Log($"Settings loaded from {SavePath}");
#endif
            return true;
        }
        return false;
    }
}

