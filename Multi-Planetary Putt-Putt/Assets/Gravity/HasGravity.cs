using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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
        GravityManager.attractors.Add(rigidbody2d);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GravityManager.attractors.Remove(rigidbody2d);
    }
}