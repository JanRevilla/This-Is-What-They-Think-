Shader "Tecnocampus/GaussianBlurShader"
{

    Properties
    {
        _MainTex ("_MainTexture", 2D) = "white" {} // textures
        _SampleDistance("_SampleDistance", Float) = 1
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
            float _SampleDistance;

            


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
            

            fixed4 frag (v2f IN) : SV_Target
            {
                //fixed4 l_Color = tex2D (_MainTex, IN.uv);
                fixed4 l_Color = float4(0, 0, 0, 0);

                float2 l_KernelBase[7] =
                {
	                { 0.0,  0.0 },
	                { 1.0,  0.0 },
	                { 0.5,  0.8660 },
	                { -0.5,  0.8660 },
	                { -1.0,  0.0 },
	                { -0.5, -0.8660 },
	                { 0.5, -0.8660 },
                };
            float l_KernelWeights[7] =
                {
	                0.38774,
	                0.06136,
	                0.24477 / 2.0,
	                0.24477 / 2.0,
	                0.06136,
	                0.24477 / 2.0,
	                0.24477 / 2.0,
                };

                float2 l_UVScaled = float2(1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y) * _SampleDistance;

                for (int i = 0; i < 7; i++)
                {
                    l_Color.xyz += tex2D(_MainTex, IN.uv + l_UVScaled * l_KernelBase[i]).xyz * l_KernelWeights[i];
                }

                return l_Color;
            }
            ENDCG
        }
    }
}
