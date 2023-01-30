Shader "CubemapBlit" {
    Properties {
        _Cubemap1 ("Cubemap 1", CUBE) = "" {}
        _Cubemap2 ("Cubemap 2", CUBE) = "" {}
        _Blend ("Blend", Range(0, 1)) = 0.5
        _Face ("Face", Int) = 0
    }

    SubShader {
        Tags {"Queue"="Geometry" "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 uv : TEXCOORD0;
            };

            struct v2f {
                float3 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 cubeDir : TEXCOORD3;
            };

            samplerCUBE _Cubemap1;
            samplerCUBE _Cubemap2;

            float _Blend;
            half _Face;

            //Face bases, assuming +x, -x, +z, -z, +y, -y with origin at bottom-left.
            static const half3 CUBE_DIR_O[6] = { {1.0, -1.0,  1.0}, {-1.0, -1.0, -1.0}, {-1.0, 1.0,  1.0}, {-1.0, -1.0, -1.0}, {-1.0, -1.0, 1.0}, { 1.0, -1.0, -1.0} };
            static const half3 CUBE_DIR_U[6] = { {0.0,  0.0, -1.0}, { 0.0,  0.0,  1.0}, { 1.0, 0.0,  0.0}, { 1.0,  0.0,  0.0}, { 1.0,  0.0, 0.0}, {-1.0,  0.0,  0.0} };
            static const half3 CUBE_DIR_V[6] = { {0.0,  1.0,  0.0}, { 0.0,  1.0,  0.0}, { 0.0, 0.0, -1.0}, { 0.0,  0.0,  1.0}, { 0.0,  1.0, 0.0}, { 0.0,  1.0,  0.0} };

            v2f vert (appdata IN) {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.uv = IN.uv;

                //Map the input UV to the corresponding face basis.
                OUT.cubeDir = CUBE_DIR_O[_Face] + 2.0*IN.uv.x * CUBE_DIR_U[_Face] + 2.0*(1.0 - IN.uv.y) * CUBE_DIR_V[_Face];

                return OUT;
            }

            half4 frag (v2f i) : COLOR {
                half4 col1 = texCUBE(_Cubemap1, i.cubeDir);
                half4 col2 = texCUBE(_Cubemap2, i.cubeDir);
                float t = _Blend;
                t = t * t * (3.0 - 2.0 * t); // smoothstep
                return lerp(col1, col2, t);
            }
            ENDCG
        }
    }
}
