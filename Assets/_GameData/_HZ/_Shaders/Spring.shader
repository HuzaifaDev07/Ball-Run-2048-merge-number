Shader "Custom/UnlitGradientColorShaderWithTransition"
{
    Properties
    {
        _ColorStart ("Color Start", Color) = (1, 0, 0, 1)
        _ColorEnd ("Color End", Color) = (0, 0, 1, 1)
        _TransitionDuration ("Transition Duration", Range(0.1, 10)) = 1
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

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            float4 _ColorStart;
            float4 _ColorEnd;
            float _TransitionDuration;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                float t = saturate(_Time.y / _TransitionDuration);
                o.color = lerp(_ColorStart, _ColorEnd, t);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}
