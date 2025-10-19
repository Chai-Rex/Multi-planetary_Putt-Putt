using UnityEngine;

public class Atmosphere : MonoBehaviour {
    [SerializeField] private Renderer spriteRenderer;
    [SerializeField] private Color atmosphereColor = Color.green;
    [SerializeField] private Color planetColor = Color.red;
    [SerializeField] private Transform planetTransform;
    [SerializeField, Range(0f, 1f)] private float atmosphereStrength = 0.15f;

    private MaterialPropertyBlock _materialPropertyBlock;

    void Awake() {
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    void Start() {
        spriteRenderer.GetPropertyBlock(_materialPropertyBlock);

        float fullRadius = transform.localScale.x;
        float planetRadius = planetTransform.localScale.x;
        float planetRadiusNormalized = planetRadius / fullRadius * 0.5f;
        float atmospherThicknessNormalized = (fullRadius - planetRadius) / fullRadius * 0.5f;

        Debug.Log($"{gameObject.name}: planet {planetRadiusNormalized}");
        Debug.Log($"{gameObject.name}: atmo {atmospherThicknessNormalized}");


        _materialPropertyBlock.SetColor("_AtmosphereColor", atmosphereColor);
        _materialPropertyBlock.SetColor("_PlanetColor", planetColor);
        _materialPropertyBlock.SetFloat("_Radius", planetRadiusNormalized);
        _materialPropertyBlock.SetFloat("_Thickness", atmospherThicknessNormalized);
        _materialPropertyBlock.SetFloat("_Strength", atmosphereStrength);

        spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
    }



    private void OnTriggerEnter2D(Collider2D collision) {
        if (!CompareTag("Atmosphere")) return;
        Debug.Log("ATMO enter");
        collision.attachedRigidbody.linearDamping = 1.0f;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!CompareTag("Atmosphere")) return;
        Debug.Log("ATMO exit");
        collision.attachedRigidbody.linearDamping = 0.0f;
    }
}
