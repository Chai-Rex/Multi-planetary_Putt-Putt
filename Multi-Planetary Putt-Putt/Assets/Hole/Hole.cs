using AudioSystem;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour {
    public event Action OnBallCompleted;

    [SerializeField] private bool debugMode = false;

    private void OnTriggerStay2D(Collider2D other) {
        if (other.attachedRigidbody.bodyType == RigidbodyType2D.Kinematic) {
            if (debugMode) {
                Debug.Log($"Ball completed!");
            }

            OnBallCompleted?.Invoke();

            Ball ball;
            if (ball = other.gameObject.GetComponent<Ball>())
            {
                ResultsManager.Instance.SetNumberOfPutts(ball.GetNumberOfPutts());
                ResultsManager.Instance.ShowResultsScreen(ResultsManager.Instance.GetCurrentLevel());
            }     
        }
    }
}