Shader "Tecnocampus/VignettingShader"
{

    Properties
    {
        _MainTex ("_MainTexture", 2D) = "white" {} // textures
        _VignettingColor("_VignettingColor", Color) = (1.0, 0, 0, 1.0)
        _Vignetting("_Vignetting", Range(0.0, 1.0)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST; //Hay 4 valores en el tilling de la imagen.

            float4 _VignettingColor;
            float _Vignetting;

            struct appdata
            {
                float4 vertex : POSITION; //float4 vector de 4 (0, 0, 0, 0)
                float2 uv : TEXCOORD0;

            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;

            };

            v2f vert (appdata v)
            {
                v2f o;
                 //o.vertex = UnityObjectToClipPos(v.vertex);
                 //o.vertex=mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz, 1.0));
                 //o.vertex = mul(float4(v.vertex.xyz, 1.0), transpose(UNITY_MATRIX_MVP));


                 o.vertex=mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));
                 o.vertex=mul(UNITY_MATRIX_V, o.vertex);
                 o.vertex=mul(UNITY_MATRIX_P, o.vertex);
                 //o.uv = TRANSFORM_TEX(v.uv, _MainTex); VERSION AUTOMATIZADA DE UNITY
                 o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                return o;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 l_Color = tex2D (_MainTex, i.uv);

                float l_StartVignetting = _Vignetting;
                float l_EndVignetting = l_StartVignetting+0.5*_Vignetting;
                float l_Length = length(i.uv - float2(0.5, 0.5));
                float l_VignettingPct=saturate((l_Length - l_StartVignetting) / (l_EndVignetting - l_StartVignetting));


                return float4(lerp(l_Color.xyz, _VignettingColor.xyz, l_VignettingPct), 1.0);
            }
            ENDCG
        }
    }
}
