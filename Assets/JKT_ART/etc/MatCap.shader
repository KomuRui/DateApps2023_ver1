Shader "Universal Render Pipeline/MatCap"
{
	Properties
	{
		[HDR] _MainTex ("Base (RGB)", 2D) = "white" {}
		[Normal] _BumpMap ("Normal Map", 2D) = "bump" {}
		_ReceiveShadows ("Receive Shadows", Float) = 1
		[MainTex] _MatCap ("MatCap (RGB)", 2D) = "white" {}
		[MATCAP_ACCURATE] _MatCapAccurate ("Accurate Calculation", Int) = 0
	}
	
	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
			"RenderPipeline"="UniversalPipeline"
		}
		
		Pass
		{
			Name "FORWARD"
			
			// Tags
			Tags
			{
				"LightMode" = "ForwardBase"
			}
			
			// Culling
			Cull Back
			
			// ZWrite
			ZWrite On
			
			// ZTest
			ZTest LEqual
			
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
				#pragma vertex vert
				#pragma exclude_renderers gles xbox360 ps3
				#pragma fragment frag
				#pragma multi_compile_fog
				#pragma shader_feature MATCAP_ACCURATE
				#include "UnityCG.cginc"
				
				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 tangent : TANGENT;
					float2 texcoord : TEXCOORD0;
				};
				
				struct v2f
				{
					float4 pos : POSITION;
					float2 uv : TEXCOORD0;
					float2 uv_bump : TEXCOORD1;
					
			#if MATCAP_ACCURATE
					float3 tSpace0 : TEXCOORD2;
					float3 tSpace1 : TEXCOORD3;
					float3 tSpace2 : TEXCOORD4;
					UNITY_FOG_COORDS(5)
			#else
					float3 c0 : TEXCOORD2;
					float3 c1 : TEXCOORD3;
					UNITY_FOG_COORDS(4)
			#endif
				};
				
				uniform float4 _MainTex_ST;
				uniform float4 _BumpMap_ST;
				
				v2f vert (appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.uv_bump = TRANSFORM_TEX(v.texcoord, _BumpMap);
					
			#if MATCAP_ACCURATE
					float3 worldNormal = UnityObjectToWorldNormal(v.normal);
					float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
					float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;
					o.tSpace0 = float3(worldTangent.x, worldBinormal.x, worldNormal.x);
					o.tSpace1 = float3(worldTangent.y, worldBinormal.y, worldNormal.y);
					o.tSpace2 = float3(worldTangent.z, worldBinormal.z, worldNormal.z);
			#else
					v.normal = normalize(v.normal);
					v.tangent = normalize(v.tangent);
					TANGENT_SPACE_ROTATION;
					o.c0 = mul(rotation, normalize(UNITY_MATRIX_IT_MV[0].xyz));
					o.c1 = mul(rotation, normalize(UNITY_MATRIX_IT_MV[1].xyz));
			#endif

					UNITY_TRANSFER_FOG(o, o.pos);

					return o;
				}
				
				uniform sampler2D _MainTex;
				uniform sampler2D _BumpMap;
				uniform sampler2D _MatCap;
				
				fixed4 frag (v2f i) : SV_Target
				{
					fixed4 tex = tex2D(_MainTex, i.uv);
					float3 normals = UnpackNormal(tex2D(_BumpMap, i.uv_bump));
					
			#if MATCAP_ACCURATE
					float3 worldNorm;
					worldNorm.x = dot(i.tSpace0.xyz, normals);
					worldNorm.y = dot(i.tSpace1.xyz, normals);
					worldNorm.z = dot(i.tSpace2.xyz, normals);
					worldNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
					float4 mc = (tex + (tex2D(_MatCap, worldNorm.xy * 0.5 + 0.5)*2.0) - 1.0);
			#else
					half2 capCoord = half2(dot(i.c0, normals), dot(i.c1, normals));
					float4 mc = (tex + (tex2D(_MatCap, capCoord*0.5 + 0.5)*2.0) - 1.0);
			#endif
					
					UNITY_APPLY_FOG(i.fogCoord, mc);

					return mc;
				}
			ENDCG
		}
	}
}