using AudioSystem;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour {
    public event Action OnBallCompleted;

    [SerializeField] private bool debugMode = false;

    AudioSource source;

    private void Awake()
    {
        source = GetComponentInChildren<AudioSource>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.attachedRigidbody.bodyType == RigidbodyType2D.Kinematic) {
            if (debugMode) {
                Debug.Log($"Ball completed!");
            }

            if(source)
            {
                source.Play();
            }
            
            OnBallCompleted?.Invoke();

            //REMOVE
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}