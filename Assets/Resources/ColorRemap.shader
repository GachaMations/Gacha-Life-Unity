//THIS SUCKS. I'LL TRY AND FIX IT, BUT IDK HOW TO WRITE SHADERS. I USED CHATGPT
//I NEED TO ADD MORE COLORS TO THE LIST (why lunime) AND MAKE IT NOT AFFECT UNRELATED COLORS
Shader "Custom/ColorRemap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Color 1", Color) = (1,1,0,1)
        _Color2 ("Color 2", Color) = (1,0,0,1)
        _RemapColor1 ("Remap Color 1", Color) = (0,0,1,1)
        _RemapColor2 ("Remap Color 2", Color) = (0,1,0,1)
        _BlendThreshold ("Blend Threshold", Range(0,1)) = 0.3
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
                if (texColor.a < 0.4) {
                    return fixed4(0,0,0,0);
                }
                if (texColor.r < _BlendThreshold && texColor.g < _BlendThreshold && texColor.b < _BlendThreshold) {
                    return texColor;
                }
                float originalGradient = dot(texColor.rgb - _Color1.rgb, _Color2.rgb - _Color1.rgb) / dot(_Color2.rgb - _Color1.rgb, _Color2.rgb - _Color1.rgb);
                float blend = smoothstep(0, _BlendThreshold, originalGradient);
                fixed3 remapColor = lerp(_RemapColor1.rgb, _RemapColor2.rgb, blend);
                return fixed4(remapColor, texColor.a);
            }
            ENDCG
        }
    }
}