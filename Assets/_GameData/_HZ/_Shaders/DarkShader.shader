Shader "Custom/LeftBottomDarkPolar" {
    Properties {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _DarkColor ("Dark Color", Color) = (0, 0, 0, 1)
        _DarkPosition ("Dark Position (Angle, Radius)", Vector) = (0, 0.5, 0, 0)  // Angle in radians, Radius normalized
        _DarkAmount ("Dark Amount", Range(0, 1)) = 0.5

        // Add new properties for reflection and specular
        _ReflectionStrength ("Reflection Strength", Range(0, 1)) = 0.5
        _SpecularColor ("Specular Color", Color) = (1, 1, 1, 1)
        _SpecularPower ("Specular Power", Range(1, 128)) = 16
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata_t {
                float4 vertex : POSITION;
            };
            
            struct v2f {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1; // Add world position for reflection
            };
            
            float4 _MainColor;
            float4 _DarkColor;
            float2 _DarkPosition;
            float _DarkAmount;
            
            // New properties
            float _ReflectionStrength;
            float4 _SpecularColor;
            float _SpecularPower;
            
            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy * 0.5 + 0.5; // Adjust UV to fit left bottom half
                
                // Calculate world position for reflection
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                // Convert polar coordinates to Cartesian
                float angle = _DarkPosition.x;
                float radius = _DarkPosition.y;
                float2 darkCenter = float2(radius * cos(angle), radius * sin(angle));
                
                // Calculate distance from current pixel to dark center
                float distance = length(i.uv - darkCenter);
                
                // Calculate reflection
                float3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
                float3 reflection = reflect(viewDir, float3(0, 1, 0));
                reflection.y = abs(reflection.y); // Invert Y reflection coordinate
                
                // Calculate specular highlight
                float3 normal = float3(0, 1, 0); // Assuming a flat surface
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfVector = normalize(lightDir + viewDir);
                float specular = pow(max(dot(normal, halfVector), 0.0), _SpecularPower);
                float4 specularColor = _SpecularColor * specular;
                
                // Combine reflection, specular, and original color
                fixed4 col = lerp(_MainColor, _DarkColor, smoothstep(_DarkAmount, 0, distance));
                //col.rgb = lerp(col.rgb, _ReflectionStrength * texCUBE(_Cube, reflection).rgb, _ReflectionStrength);
                col.rgb += specularColor.rgb;
                
                return col;
            }
            
            ENDCG
        }
    }
}
