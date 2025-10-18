using System;
using UnityEngine;

public class Hole : MonoBehaviour {
    public event Action OnBallCompleted;

    [SerializeField] private bool debugMode = false;

    private void OnTriggerStay2D(Collider2D other) {
        if (other.attachedRigidbody.bodyType == RigidbodyType2D.Kinematic) {
            if (debugMode) {
                Debug.Log($"Ball completed!");
            }
            OnBallCompleted?.Invoke();
        }
    }
}