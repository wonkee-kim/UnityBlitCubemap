using UnityEngine;

[ExecuteInEditMode]
public class CubemapBlit : MonoBehaviour
{
    [Range(0, 1)] public float blendAmount;

    [SerializeField] private Cubemap _cubemap1;
    [SerializeField] private Cubemap _cubemap2;
    [SerializeField] private ReflectionProbe _reflectionProbe;

    [SerializeField] private Shader _blenderShader;
    private Material _blendMaterial;
    private RenderTexture _cubemapBlended;

    private bool _isInitialized = false;

    private CubemapFace[] _faces = new CubemapFace[]
    {
        CubemapFace.PositiveX,
        CubemapFace.NegativeX,
        CubemapFace.PositiveY,
        CubemapFace.NegativeY,
        CubemapFace.PositiveZ,
        CubemapFace.NegativeZ,
    };

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
        if (_blendMaterial != null)
        {
            DestroyImmediate(_blendMaterial);
            _blendMaterial = null;
        }
        if (_cubemapBlended != null)
        {
            _cubemapBlended.Release();
            _cubemapBlended = null;
        }
        _isInitialized = false;
    }

    private void Update()
    {
        BlitCubemaps();
    }

    private void Initialize()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;

            _blendMaterial = new Material(_blenderShader);

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

    private void BlitCubemaps()
    {
        _blendMaterial.SetTexture("_Cubemap1", _cubemap1);
        _blendMaterial.SetTexture("_Cubemap2", _cubemap2);
        _blendMaterial.SetFloat("_Blend", blendAmount);
        for (int i = 0; i < 6; i++)
        {
            Graphics.SetRenderTarget(_cubemapBlended, mipLevel: 0, _faces[i], depthSlice: 0);
            _blendMaterial.SetInt("_Face", (int)_faces[i]);
            Graphics.Blit(null, _blendMaterial);
        }
    }
}
