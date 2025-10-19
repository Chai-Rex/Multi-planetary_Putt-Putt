using UnityEngine;

[CreateAssetMenu(fileName = "SettingsSO", menuName = "Scriptable Objects/SettingsSO")]
public class SettingsSO : ScriptableObject
{


    [Range(0, 1)] public float MusicVolume;
    [Range(0, 1)] public float EffectsVolume;
}
