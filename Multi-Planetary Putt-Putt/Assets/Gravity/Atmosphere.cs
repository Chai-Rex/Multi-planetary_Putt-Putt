using UnityEngine;

public class Atmosphere : MonoBehaviour {

    [SerializeField] private float linearDamping = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!CompareTag("Atmosphere")) return;
        collision.attachedRigidbody.linearDamping = 1.0f;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!CompareTag("Atmosphere")) return;
        collision.attachedRigidbody.linearDamping = 0.0f;
    }
}
