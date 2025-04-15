Shader "Unlit/CombineCameraShader"
{
    Properties
    {
        targetTexture0 ("Render Texture 1", 2D) = "black" {}
        targetTexture1 ("Render Texture 2", 2D) = "black" {}
        targetTexture2 ("Render Texture 3", 2D) = "black" {}
        targetTexture3 ("Render Texture 4", 2D) = "black" {}
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
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
            sampler2D targetTexture0;
            sampler2D targetTexture1;
            sampler2D targetTexture2;
            sampler2D targetTexture3;
            float4 targetTexture0_ST;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, targetTexture0);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(targetTexture0, i.uv);
                fixed4 col2 = tex2D(targetTexture1, i.uv);
                fixed4 col3 = tex2D(targetTexture2, i.uv);
                fixed4 col4 = tex2D(targetTexture3, i.uv);
                return col + col2 + col3 + col4;
            }
            ENDCG
        }
    }
}
