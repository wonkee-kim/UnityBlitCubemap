using UnityEngine;

[ExecuteInEditMode]
public class CubemapBlend : MonoBehaviour
{
    [Range(0, 1)] public float blendAmount;

    [SerializeField] private Cubemap _cubemap1;
    [SerializeField] private Cubemap _cubemap2;
    [SerializeField] private ReflectionProbe _reflectionProbe;

    private RenderTexture _cubemapBlended;

    private bool _isInitialized = false;

    private void OnEnable()
    {
        Initialize();
    }

    private void OnValidate()
    {
        Initialize();
    }

    private void OnDisable()
    {
        if (_cubemapBlended != null)
        {
            _cubemapBlended.Release();
            _cubemapBlended = null;
        }
        _isInitialized = false;
    }

    private void Update()
    {
        ReflectionProbe.BlendCubemap(src: _cubemap1, dst: _cubemap2, blend: blendAmount, target: _cubemapBlended);
    }

    private void Initialize()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            int width = _cubemap1.width;
            _cubemapBlended = new RenderTexture(width, width, depth: 0, RenderTextureFormat.ARGBHalf);
            _cubemapBlended.dimension = UnityEngine.Rendering.TextureDimension.Cube;
            _cubemapBlended.useMipMap = true; // Smoothness is not working without this.
            _cubemapBlended.autoGenerateMips = true;
            _cubemapBlended.filterMode = FilterMode.Bilinear;
            _cubemapBlended.wrapMode = TextureWrapMode.Clamp;
            _cubemapBlended.Create();
            _reflectionProbe.customBakedTexture = _cubemapBlended;
        }
    }
}
