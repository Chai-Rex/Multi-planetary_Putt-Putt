using AudioSystem;
using UnityEngine;
using UnityEngine.Audio;

public class EmitOnCollision : MonoBehaviour
{
    SoundEmitter emitter;
    AudioSource source;

    public float cd = 0.5f;
    private float lastPlayedTime = 0;

    public float minMagnitude = 0;

    private void Awake()
    {
        emitter = GetComponentInChildren<SoundEmitter>();
        source = GetComponentInChildren<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            if (emitter && Time.timeSinceLevelLoad - lastPlayedTime >= cd && minMagnitude < collision.relativeVelocity.magnitude)
            {
                lastPlayedTime = Time.timeSinceLevelLoad;
                if(source)
                {
                    source.Play();
                }
               
            }
        }
    }
}
