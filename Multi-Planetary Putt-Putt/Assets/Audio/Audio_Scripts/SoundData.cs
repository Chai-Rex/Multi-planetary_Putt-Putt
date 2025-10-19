using System;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioSystem {

    [Serializable]

    public class SoundData {
        public AudioClip Clip;
        public AudioMixerGroup MixerGroup;
        public bool Loop;
        public bool PlayOnAwake;
        public bool FrequentSound;

        [HideInInspector] public float volume = 1f;
    }

}
