Shader "Hidden/WorldEnd"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _WorldSize ("World Size", float) = 1.0
        [PerRendererData] _CameraPos("Camera Pos", Vector) = (0, 0, 0, 0)
        [PerRendererData] _CameraSize("Camera Size", Vector) = (0, 0, 0, 0)
        _SmoothSize("Smooth Size", float) = 5.0
        _Distortion("Distortion", float) = 4.0
        _LightFreq("LightFreq", float) = 1.0
        _LightThresh("LightThresh", Range(.0, 1.0)) = 0.9
        _LightSpeed("LightSpeed", float) = 1.0
        _LightSin("LightSin", Vector) = (0, 0, 0, 0)
        _LightColor("LightColor", Color) = (1, 1, 1, 0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
            
            sampler2D _MainTex;
            float _WorldSize;
            float2 _CameraPos;
            float2 _CameraSize;
            float _SmoothSize;
            float _Distortion;
            float _LightFreq;
            float _LightSpeed;
            float _LightThresh;
            float2 _LightSin;
            fixed4 _LightColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 cameraPos = _CameraPos + i.uv * _CameraSize;
                
                float minX = min(cameraPos.x, _WorldSize - cameraPos.x);
                float minY = min(cameraPos.y, _WorldSize - cameraPos.y);
                float minDis = min(minX, minY);
                minDis -= _SmoothSize * 0.5;

                if (minDis >= 0)
                {
                    return tex2D(_MainTex, i.uv);
                }
                
                //Smootihing & Distortion
                float strength = lerp(.0, 1.0, abs(minDis) / _SmoothSize);
                fixed2 warpedUV = (i.uv + (-(2.0 * i.uv - fixed2(1.0 ,1.0)) + (1.0 + sin(_Time.y * 0.25)) / 2.0) * strength);
                fixed4 col = tex2D(_MainTex, warpedUV);
                
                //Lights
                half2 lightUV = warpedUV + half2(_Time.y * 1.2534 + sin(strength * _LightSin.x * sin(_Time.y * 0.857)), _Time.y * 0.6345 + sin(strength * _LightSin.y * cos(_Time.y * 1.2264))) * _LightSpeed;
                float lightSin1 = max(sin(frac(lightUV.x * 0.637 + lightUV.y * 0.826) * _LightFreq * 1.75) - _LightThresh, .0) / (1.0 - _LightThresh);
                float lightSin2 = max(sin(frac(lightUV.x * 1.473 + lightUV.y * 0.86) * _LightFreq) - _LightThresh, .0) / (1.0 - _LightThresh);
                float lightSin = step(1, lightSin1 + lightSin2);
                col = lerp(col, _LightColor, lightSin);
                
                col = lerp(1.0 - col, .0, pow(strength, 4.0));
                return lerp(1.0 - col, col, strength);
            }
            ENDCG
        }
    }
}
