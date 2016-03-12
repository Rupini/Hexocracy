Shader "Hexocrocy/CustomColorShader" 
{

	Properties
	{
	    _TargetColor ("Target Color", Color) = (1,1,1,1)
		_CustomColor ("Custom Color", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader 
	{
		Pass
		{		
		    Name "ColorReplacement"
		    CGPROGRAM
		    
			#pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
		    
		    sampler2D _MainTex;
		    fixed4 _TargetColor;
		    fixed4 _CustomColor;
		    
		    struct v2f
            {
                float4  pos : SV_POSITION;
                float2  uv : TEXCOORD0;
            };
 		    
            v2f vert (appdata_tan v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord.xy;
                return o;
            }
	        
		    
		    float4 frag(v2f i) : COLOR
            {
            // SURFACE COLOUR
                fixed4 c = tex2D(_MainTex, i.uv);
       	    
            // RESULT
                float4 result;
		    
		    	if(c.r == _TargetColor.r && c.g == _TargetColor.g && c.b == _TargetColor.b)
		    	{
		    	   result.rgb = _CustomColor.rgb;
		    	   result.a = _CustomColor.a;
		        }
		    	else
		    	{
		    	   result.rgb = c.rgb;
		    	   result.a = c.a;
		    	}
		    
            //  result.rgb = tex2D(_ColorRamp, float2(greyscale, 0.5)).rgb;
            //  result.a = tex2D(_MainTex, i.uv).a;
                return result;
            }
		    
		    /*void surf (Input IN, inout SurfaceOutputStandard o) {
		    	// Albedo comes from a texture tinted by color
		    	fixed4 c = tex2D (_MainTex, IN.uv_MainTex);// * _CustomColor;
		    
		    	if(c.r == c.r)// && c.g == _TargetColor.g && c.b == _TargetColor.b)
		    	{
		    	  o.Albedo = _CustomColor.rgb;
		    	  o.Alpha = _CustomColor.a;
		    	}
		    	else
		    	{
		    	  o.Albedo = c.rgb;
		    	  o.Alpha = c.a;
		    	}
		    }*/
		    ENDCG
		}
	} 
	FallBack "Diffuse"
}
