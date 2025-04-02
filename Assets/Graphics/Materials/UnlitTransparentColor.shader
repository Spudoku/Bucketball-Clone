// Credit: ChatGPT
// allows color selection and alpha
// for an unlit, transparent shader
Shader "Custom/UnlitTransparentColor"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1) // RGBA (last value is transparency)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha // Enable transparency blending
        ZWrite Off // Fix sorting issues
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            fixed4 _Color; // The color property

            v2f vert (appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return _Color; // Use the color with transparency
            }
            ENDCG
        }
    }
}
