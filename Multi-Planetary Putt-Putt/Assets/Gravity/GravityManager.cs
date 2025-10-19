using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.CullingGroup;
using static UnityEngine.GraphicsBuffer;

public class GravityManager : MonoBehaviour {

    public static GravityManager Instance { get; private set; }

    [SerializeField] private float g = 1f;
    static float G;

    public static List<Rigidbody2D> attractors = new List<Rigidbody2D>();
    public static List<Rigidbody2D> attractees = new List<Rigidbody2D>();


    private void Awake() {
        Instance = this;
    }

    void FixedUpdate() {
        G = g;

        SimulateGravities();

    }
    public static void SimulateGravities() {
        foreach (Rigidbody2D attractor in attractors) {
            foreach (Rigidbody2D attractee in attractees) {
                if (attractor != attractee)
                    AddGravityForce(attractor, attractee);
            }
        }
    }

    public static void AddGravityForce(Rigidbody2D attractor, Rigidbody2D target) {

        target.AddForce(GetGravityForce(attractor.position, attractor.mass, target.position, target.mass));
    }

    public static Vector3 GetGravityForceAtLocation(Vector2 location, float targetMass)
    {
        Vector2 totalForce = Vector2.zero;

        foreach(Rigidbody2D attractor in attractors)
        {
            totalForce += GetGravityForce(attractor.position, attractor.mass, location, targetMass);
        }

        return totalForce;
    }

    private static Vector2 GetGravityForce(Vector2 location1, float mass1, Vector2 location2, float mass2)
    {
        float massProduct = mass1 * mass2 * G;

        //float distance = Vector3.Distance(attractor.position,target.position.
        Vector2 difference = location1 - location2;
        float distance = difference.magnitude; // r = Mathf.Sqrt((x*x)+(y*y))

        //F = G * ((m1*m2)/r^2)
        float unScaledforceMagnitude = massProduct / distance;
        float forceMagnitude = G * unScaledforceMagnitude;

        Vector2 forceDirection = difference.normalized;

        Vector2 forceVector = forceDirection * forceMagnitude;

        return forceVector;
    }
}