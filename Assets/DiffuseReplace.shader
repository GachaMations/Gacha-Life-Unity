Shader "Custom/ColorRemap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (1,1,0,1) // Original starting color (Yellow)
        _Color2 ("Color 2", Color) = (1,0,0,1) // Original ending color (Red)
        _RemapColor1 ("Remap Color 1", Color) = (0,0,1,1) // New starting color (Blue)
        _RemapColor2 ("Remap Color 2", Color) = (0,1,0,1) // New ending color (Green)
        _BlendThreshold ("Blend Threshold", Range(0,1)) = 0.3 // Threshold for blending
    }
 
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
 
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            fixed4 _Color1;
            fixed4 _Color2;
            fixed4 _RemapColor1;
            fixed4 _RemapColor2;
            float _BlendThreshold;
 
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
 
            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 vertex : SV_POSITION;
            };
 
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
 
                // Check if the pixel is fully transparent or part of the outline
                if (texColor.a == 0 || (texColor.r < _BlendThreshold && texColor.g < _BlendThreshold && texColor.b < _BlendThreshold)) {
                    return texColor; // Keep transparent pixels and outline unchanged
                }
 
                // Calculate the original gradient based on the original colors
                float originalGradient = dot(texColor.rgb - _Color1.rgb, _Color2.rgb - _Color1.rgb) / dot(_Color2.rgb - _Color1.rgb, _Color2.rgb - _Color1.rgb);
                
                // Smooth transition between color 1 and color 2 using a blend threshold
                float blend = smoothstep(0, _BlendThreshold, originalGradient);
                
                // Interpolate between the remap colors based on the original gradient
                fixed3 remapColor = lerp(_RemapColor1.rgb, _RemapColor2.rgb, blend);
 
                return fixed4(remapColor, texColor.a);
            }
            ENDCG
        }
    }
}