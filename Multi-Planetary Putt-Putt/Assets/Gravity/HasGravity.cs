using UnityEngine;

public class HasGravity : MonoBehaviour {


    [Header("References")]
    [SerializeField] private Rigidbody2D rigidbody2d;

    void Start() {
        //GravityManager.attractors.Add(rigidbody2d);
    }

    private void OnDestroy() {
        GravityManager.attractors.Remove(rigidbody2d);

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!CompareTag("Gravity")) return;
        Debug.Log("Gravity enter");
        GravityManager.attractors.Add(rigidbody2d);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!CompareTag("Gravity")) return;
        Debug.Log("Gravity exit");
        GravityManager.attractors.Remove(rigidbody2d);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        collision.rigidbody.linearDamping = rigidbody2d.sharedMaterial.friction;

    }

    private void OnCollisionExit(Collision collision) {
        collision.rigidbody.linearDamping = 0;
    }

}