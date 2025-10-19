using UnityEngine;

public class Atmosphere : MonoBehaviour {

    [SerializeField] private float linearDamping = 1.0f;
    public float LinearDrag { get { return linearDamping; } }

    private CircleCollider2D circleCollider;
    public float Radius
    {
        get
        {
            if (circleCollider != null)
            {
                return circleCollider.radius;
            }
            return 0f;
        }
    }

    void Start()
    {
        AtmosphereManager.atmosphereObjects.Add(this);

        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!CompareTag("Atmosphere")) return;
        if(collision.tag == "Ball")
        {
            collision.attachedRigidbody.linearDamping = linearDamping;
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!CompareTag("Atmosphere")) return;
        if (collision.tag == "Ball")
        {
            collision.attachedRigidbody.linearDamping =  linearDamping;
        }
        
    }
}
