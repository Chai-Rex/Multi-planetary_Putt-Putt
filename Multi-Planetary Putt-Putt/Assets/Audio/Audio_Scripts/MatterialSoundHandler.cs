using AudioSystem;
using UnityEngine;

public class MatterialSoundHandler : MonoBehaviour {


    [SerializeField] private MaterialSoundSO soundData;


    private void OnCollisionEnter(Collision collision) {

        foreach (var contact in collision.contacts) {

            soundData.OnCollisionSoundData.volume = Mathf.Clamp01(contact.impulse.magnitude / soundData.MaxImplusMagnitude);

            SoundManager.Instance.CreateSound()
                .WithSoundData(soundData.OnCollisionSoundData)
                .WithRandomPitch()
                .WithPosition(contact.point)
                .Play();
        }

    }



    public void BreakOff() {

        SoundManager.Instance.CreateSound()
            .WithSoundData(soundData.OnBreakSoundData)
            .WithRandomPitch()
            .WithPosition(transform.position)
            .Play();
    }


}
