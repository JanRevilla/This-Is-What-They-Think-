Shader "Tecnocampus/BillboardShader"
{

    Properties
    {
        _MainTex ("_MainTexture", 2D) = "white" {} // textures

        _UseUpVector("_UpVector", Float) = 1
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

            float3 _RightDirection;
            float3 _UpDirection;
            float _UseUpVector;

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
                 o.vertex=mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1.0));

                 float3 l_Position = float3(unity_ObjectToWorld[0].w, unity_ObjectToWorld[1].w, unity_ObjectToWorld[2].w); //Posicion de mundo Objeto
                 o.vertex = float4(l_Position.xyz+_RightDirection*(v.uv.x-0.5)*2.0+(_UseUpVector==1.0 ? _UpDirection: float3(0,1,0)) *(v.uv.y - 0.5)*2.0, 1.0);

                 //o.vertex = l_Position.xyz + _RightDirection + ()                 ------------------------ACABAR---------------------------


                 o.vertex=mul(UNITY_MATRIX_V, o.vertex);
                 o.vertex=mul(UNITY_MATRIX_P, o.vertex);
                 o.uv = v.uv;

                return o;
            }
            

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 l_Color = tex2D (_MainTex, i.uv);
                return l_Color;
            }
            ENDCG
        }
    }
}
