Shader "Custom/MaskObject"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Overlay" }
        Pass
        {
            Stencil
            {
                Ref 1
                Comp equal    // ���ٽ� ���� 1�� �κи� ���
                Pass replace  // ���ٽ� ���� ����
            }
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                return float4(0, 0, 0, 0); // �����ϰ� ����
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
