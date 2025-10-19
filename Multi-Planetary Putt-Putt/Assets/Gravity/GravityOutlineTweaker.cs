using UnityEngine;

public class GravityOutlineTweaker : MonoBehaviour {

    [SerializeField] private Renderer spriteVisualRenderer;

    private MaterialPropertyBlock _materialPropertyBlock;

    void Awake() {
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    void Start() {
        _materialPropertyBlock.SetFloat("_Radius", transform.localScale.x * 0.5f);

        spriteVisualRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
