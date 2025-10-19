using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioSystem {
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour {

        public SoundData Data { get; private set; }
        private AudioSource audioSource;
        Coroutine playingCoroutine;

        private void Awake() {
            audioSource = gameObject.GetComponent<AudioSource>();

        }
        
        public void Play() {
            if (playingCoroutine != null) StopCoroutine(playingCoroutine);

            audioSource.Play();
            playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        public void Stop() {
            if (playingCoroutine != null) StopCoroutine(playingCoroutine);
            StopCoroutine(playingCoroutine);
            playingCoroutine = null;
        }

        private IEnumerator WaitForSoundToEnd() {
            yield return new WaitWhile(() => audioSource.isPlaying);
            SoundManager.Instance.ReturnToPool(this);
        }

        public void Initialize(SoundData data) {
            Data = data;
            audioSource.clip = data.Clip;
            audioSource.outputAudioMixerGroup = data.MixerGroup;
            audioSource.loop = data.Loop;
            audioSource.playOnAwake = data.PlayOnAwake;

            audioSource.volume = data.volume;
        }

        public void WithRandomPitch(float min = -0.07f, float max = 0.07f) {
            audioSource.pitch += Random.Range(min, max);
        }
    }
}

