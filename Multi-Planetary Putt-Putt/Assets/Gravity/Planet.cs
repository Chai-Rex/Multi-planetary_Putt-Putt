using UnityEngine;

public class Planet : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Rigidbody2D rigidbody2d;

    private void OnCollisionEnter2D(Collision2D collision) {
        collision.rigidbody.linearDamping = rigidbody2d.sharedMaterial.friction;
    }
    private void OnCollisionExit(Collision collision) {
        collision.rigidbody.linearDamping = 0;
    }
}
