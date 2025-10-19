using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_SettingsSaver : MonoBehaviour
{

    private SettingsSO _settings;

    [Header("Sliders")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;

    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";

    private void Start()
    {

        _settings = UI_SettingsManager.Instance.SO;

        // Initialize sliders from settings
        musicVolumeSlider.value = _settings.MusicVolume;
        effectsVolumeSlider.value = _settings.EffectsVolume;

        // Add listeners to update settings (but not save yet)
        musicVolumeSlider.onValueChanged.AddListener((float value) => { _settings.MusicVolume = value; mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20); });
        effectsVolumeSlider.onValueChanged.AddListener((float value) => { _settings.EffectsVolume = value; mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20); });

    }

    private void OnDestroy()
    {
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        effectsVolumeSlider.onValueChanged.RemoveAllListeners();
    }

    private void OnDisable()
    {
        UI_SettingsManager.Instance.SaveSettings();
    }
}

