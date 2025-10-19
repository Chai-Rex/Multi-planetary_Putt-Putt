using UnityEngine;

public class Gravity : MonoBehaviour {


    [Header("References")]
    [SerializeField] private Rigidbody2D rigidbody2d;
    public Rigidbody2D RB { get {  return rigidbody2d; } }

    private CircleCollider2D circleCollider;
    public float Radius {
        get {
            if (circleCollider != null)
            {
                return circleCollider.radius * transform.localScale.x;
            }
            return 0f;
        } 
    }

    void Awake() {
        GravityManager.gravityObjects.Add(this);

        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnDestroy() {
        GravityManager.attractors.Remove(rigidbody2d);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!CompareTag("Gravity")) return;
        if (collision.tag == "Ball")
        {
            Debug.Log("adding");
            GravityManager.attractors.Add(rigidbody2d);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!CompareTag("Gravity")) return;
        if (collision.tag == "Ball")
        {
            Debug.Log("removing");
            GravityManager.attractors.Remove(rigidbody2d);
        }
    }



}