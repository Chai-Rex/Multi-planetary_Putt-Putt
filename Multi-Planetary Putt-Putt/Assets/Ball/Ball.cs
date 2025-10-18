using UnityEngine;

public class Ball : MonoBehaviour {


    [SerializeField] private float force = 1f;
    [SerializeField] private Transform direction;


    [SerializeField] private Rigidbody2D rigidbody2d;



    private void Start() {
        GravityManager.attractees.Add(rigidbody2d);
        rigidbody2d.AddForce(direction.up * force);
    }



}
