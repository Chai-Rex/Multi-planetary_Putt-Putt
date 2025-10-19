using AudioSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialSoundSO", menuName = "Scriptable Objects/MaterialSoundSO")]
public class MaterialSoundSO : ScriptableObject {


    public float MaxImplusMagnitude = 30f;


    public SoundData OnCollisionSoundData;
    public SoundData OnBreakSoundData;
    
}
