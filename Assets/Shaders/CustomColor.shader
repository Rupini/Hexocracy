Shader "Hexocrocy/CustomColorShader" 
{

	Properties
	{
	    _TargetColor ("Target Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Mask ("Mask", 2D) = "white" {}
	}

	SubShader 
	{
		Pass
		{		
		    Name "ColorReplacement"
		    CGPROGRAM
		    
			#pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
		    
		    struct v2f
            {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
            };
 		    
			sampler2D _MainTex;
			sampler2D _Mask;
		    fixed4 _TargetColor;

            v2f vert (appdata_tan v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord.xy;
                return o;
            }
		    
		    float4 frag(v2f i) : COLOR
            {
                fixed4 baseColor = tex2D(_MainTex, i.uv);
				fixed4 m = tex2D(_Mask, i.uv);
		   
                return fixed4(lerp(baseColor.rgb, _TargetColor.rgb, m.r), baseColor.a );
            }
		    
		    ENDCG
		}
	} 

	FallBack "Diffuse"
}
