Shader "Custom/ImageFade"
{
    Properties{
        _MainTex("Texture", 2D) = "white" { }
        _FadeAmount("Fade Amount", Range(0, 1)) = 0.5
    }

        SubShader{
            Tags { "Queue" = "Overlay" }
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            sampler2D _MainTex;
            fixed _FadeAmount;

            struct Input {
                float2 uv_MainTex;
            };

            void surf(Input IN, inout SurfaceOutput o) {
                // Calculate transparency based on UV coordinates
                o.Alpha = smoothstep(_FadeAmount, 1.0, IN.uv_MainTex.x);

                // Sample texture
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

                // Apply transparency
                c.a *= o.Alpha;

                // Output final color
                o.Albedo = c.rgb;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
