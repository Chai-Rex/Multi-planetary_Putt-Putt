using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ConverganceParticleConfigurator : MonoBehaviour {

    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private Rigidbody2D rigidbody2d;
    [SerializeField] private GravityOutlineTweaker outlineTweaker;
    [SerializeField] private float maxMass = 1000f;


    [Header("Ring Setup")]
    [Tooltip("Radius of the circle ring from which particles start.")]
    public float ringRadius = 1f;

    [Tooltip("Base size of particles at spawn.")]
    public float startSize = 0.1f;

    [Tooltip("Speed of particles moving toward the center.")]
    public float startSpeed = 1f;
    public float maxSpeed = 30f;

    private ParticleSystem _particleSystem;
    private ParticleSystem.MainModule main;
    private ParticleSystem.ShapeModule shape;

    private void Awake() {
        _particleSystem = GetComponent<ParticleSystem>();
        main = _particleSystem.main;
        shape = _particleSystem.shape;

        ringRadius = circleCollider.radius;
        startSpeed = rigidbody2d.mass / maxMass * -maxSpeed;

        outlineTweaker.transform.localScale = ringRadius * 2 * Vector3.one;

        UpdateParticleParams();

    }

    /// <summary>
    /// Updates the particle system based on the current ringRadius, startSize, and startSpeed.
    /// </summary>
    public void UpdateParticleParams() {
        if (_particleSystem == null)
            _particleSystem = GetComponent<ParticleSystem>();

        main = _particleSystem.main;
        shape = _particleSystem.shape;

        // Calculate lifetime = distance / speed
        float lifetime = ringRadius / Mathf.Max(Mathf.Abs(startSpeed), 0.001f);

        // Apply parameters
        main.startLifetime = lifetime;
        main.startSpeed = startSpeed;
        main.startSize = startSize;

        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = ringRadius;
    }

    /// <summary>
    /// Allows runtime updates by setting parameters directly and recalculating everything.
    /// </summary>
    public void SetParams(float radius, float size, float speed) {
        ringRadius = radius;
        startSize = size;
        startSpeed = speed;
        UpdateParticleParams();
    }
}
