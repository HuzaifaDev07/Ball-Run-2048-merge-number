Shader "Custom/BounceShader"
{
    Properties
    {
        _BounceAmount ("Bounce Amount", Range(0.0, 1.0)) = 0.1
        _BounceSpeed ("Bounce Speed", Range(0.0, 10.0)) = 2.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        float _BounceAmount;
        float _BounceSpeed;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
        };

        void vert(inout appdata_full v)
        {
            float time = _Time.y * _BounceSpeed;
            v.vertex.y += _BounceAmount * sin(time + v.vertex.x);
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
