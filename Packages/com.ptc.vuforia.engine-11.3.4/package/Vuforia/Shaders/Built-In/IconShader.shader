Shader "Vuforia/Built-In/IconShader"
{
    Properties
    {
        _IconTexture ("Icon Texture", 2D) = "white" {}
        _IconColor ("Icon Color", Color) = (1,1,1,1)
        _BackgroundTexture ("Background Texture", 2D) = "white" {}
        _BackgroundColor ("Background Color", Color) = (1,1,1,1)
        _IconSize ("Icon Size", Float) = 0.66
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        
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
                float4 vertex : SV_POSITION;
            };

            sampler2D _IconTexture;
            float4 _IconColor;

            sampler2D _BackgroundTexture;
            float4 _BackgroundColor;
            float _IconSize;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            float inRange(float v, float min, float max)
            {
                return min <= v && v <= max;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 backgroundColor = tex2D(_BackgroundTexture, i.uv) * _BackgroundColor;
                // Scale the icon uvs fom the center (0.5, 0.5) and clamp the value so that the
                // icon is not repeated
                float2 iconUVs = (i.uv - 0.5) / _IconSize + 0.5f;
                fixed4 iconColor = tex2D(_IconTexture, iconUVs) * _IconColor;
                
                float iconAlpha = iconColor.a * inRange(iconUVs.x, 0, 1) * inRange(iconUVs.y, 0, 1);
                // Blend the icon color onto the background color only using the icon's alpha channel
                fixed3 color = iconColor.rgb * iconAlpha + backgroundColor.rgb * (1.0f - iconAlpha);

                return fixed4(color, backgroundColor.a);
            }
            ENDCG
        }
    }
}
