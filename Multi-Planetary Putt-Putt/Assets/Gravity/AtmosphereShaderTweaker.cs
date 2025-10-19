using UnityEngine;

public class AtmosphereShaderTweaker : MonoBehaviour {

    [SerializeField] private Renderer spriteVisualRenderer;
    [SerializeField] private Color atmosphereColor = Color.green;
    [SerializeField] private Color planetColor = Color.red;
    [SerializeField] private Transform planetTransform;
    [SerializeField] private CircleCollider2D atmosphereCollider;
    [SerializeField, Range(0f, 1f)] private float atmosphereStrength = 0.15f;

    private MaterialPropertyBlock _materialPropertyBlock;

    void Awake() {
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    void Start() {
        spriteVisualRenderer.GetPropertyBlock(_materialPropertyBlock);

        float fullRadius = atmosphereCollider.radius;
        float planetRadius = planetTransform.localScale.x * 0.5f;
        float planetRadiusNormalized = planetRadius / fullRadius * 0.5f;
        float atmospherThicknessNormalized = (fullRadius - planetRadius) / fullRadius * 0.5f;


        spriteVisualRenderer.gameObject.transform.localScale = fullRadius * 2 * Vector3.one;
        //Debug.Log($"{gameObject.name}: planet {planetRadiusNormalized}");
        //Debug.Log($"{gameObject.name}: atmo {atmospherThicknessNormalized}");


        _materialPropertyBlock.SetColor("_AtmosphereColor", atmosphereColor);
        _materialPropertyBlock.SetColor("_PlanetColor", planetColor);
        _materialPropertyBlock.SetFloat("_Radius", planetRadiusNormalized);
        _materialPropertyBlock.SetFloat("_Thickness", atmospherThicknessNormalized);
        _materialPropertyBlock.SetFloat("_Strength", atmosphereStrength);
        _materialPropertyBlock.SetFloat("_RingRadius", fullRadius);

        spriteVisualRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
