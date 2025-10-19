using UnityEngine;

public class Gravity : MonoBehaviour {


    [Header("References")]
    [SerializeField] private Rigidbody2D rigidbody2d;

    private void OnDestroy() {
        GravityManager.attractors.Remove(rigidbody2d);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!CompareTag("Gravity")) return;
        GravityManager.attractors.Add(rigidbody2d);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!CompareTag("Gravity")) return;
        GravityManager.attractors.Remove(rigidbody2d);
    }



}