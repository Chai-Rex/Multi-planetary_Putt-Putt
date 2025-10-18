using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HasGravity : MonoBehaviour {


    [Header("References")]
    [SerializeField] private Rigidbody2D rigidbody2d;

    private bool isFriction = false;

    void Start() {
        //GravityManager.attractors.Add(rigidbody2d);
    }

    private void OnDestroy() {
        GravityManager.attractors.Remove(rigidbody2d);

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GravityManager.attractors.Add(rigidbody2d);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GravityManager.attractors.Remove(rigidbody2d);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (isFriction) return;
        isFriction = true;
        FrictionAsync(collision.rigidbody);
    }

    private void OnCollisionExit(Collision collision) {
        isFriction = false;
    }

    private async void FrictionAsync(Rigidbody2D ballRigidbody2D) {
        while (isFriction) {

            ballRigidbody2D.linearDamping = rigidbody2d.sharedMaterial.friction;
            await Awaitable.FixedUpdateAsync();
        }
        ballRigidbody2D.linearDamping = 0;
    }
}