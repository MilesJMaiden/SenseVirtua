Shader "Custom/DiffuseMixShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _MixColor ("Mix Color", Color) = (0,1,0,1)
        _MixFactor ("Mix Factor", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            fixed4 _BaseColor;
            fixed4 _MixColor;
            float _MixFactor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Normalize the normal vector
                float3 normal = normalize(i.normal);

                // Lambertian diffuse lighting
                float3 lightDir = normalize(float3(0.0, 1.0, 0.5));
                float diff = max(dot(normal, lightDir), 0.0);

                // Calculate the base color contribution
                fixed4 baseColor = _BaseColor * diff;

                // Color ramp effect (simplified to linear interpolation)
                float rampPos = saturate(baseColor.g);  // Using green channel for ramp position
                fixed4 rampColor = lerp(_BaseColor, _MixColor, rampPos);

                // Mix the base color with the ramp color
                fixed4 finalColor = lerp(baseColor, rampColor, _MixFactor);

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
