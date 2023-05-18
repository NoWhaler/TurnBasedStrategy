Shader "Custom/OutLineShader"
{
     Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.01
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}

        Pass {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float _OutlineThickness;
            float4 _OutlineColor;

            fixed4 frag () : SV_Target {
                return _OutlineColor;
            }
            ENDCG
        }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineThickness;

            fixed4 frag (v2f i) : SV_Target {
                // Compute the depth gradient at the pixel
                float2 ddx_uv = ddx(i.uv);
                float2 ddy_uv = ddy(i.uv);
                float depth_gradient = length(ddx(i.vertex.xy)) + length(ddy(i.vertex.xy));

                // Render the pixel normally if it's not on the edge of the object
                if (depth_gradient <= _OutlineThickness) {
                    fixed4 col = tex2D(_MainTex, i.uv);
                    return col;
                }

                // Discard the pixel if it's inside the object
                clip (depth_gradient - _OutlineThickness);

                // Otherwise, render the pixel as the outline color
                return _OutlineColor;
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
