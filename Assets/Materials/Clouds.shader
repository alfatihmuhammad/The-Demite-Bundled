// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:False,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.1019608,fgcg:0.1333333,fgcb:0.1686275,fgca:1,fgde:0.05,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:True,fnfb:True;n:type:ShaderForge.SFN_Final,id:5079,x:32719,y:32712,varname:node_5079,prsc:2|emission-9622-RGB,alpha-4952-OUT;n:type:ShaderForge.SFN_Tex2d,id:9622,x:32276,y:32631,varname:node_9622,prsc:2,tex:37803055de3588346ab556d0e6cbe8c9,ntxv:0,isnm:False|UVIN-4942-OUT,TEX-7042-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:7042,x:31934,y:32859,ptovrint:False,ptlb:Texture,ptin:_Texture,varname:node_7042,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:True,tagnrm:False,tex:37803055de3588346ab556d0e6cbe8c9,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:434,x:32258,y:33026,varname:node_434,prsc:2,tex:37803055de3588346ab556d0e6cbe8c9,ntxv:0,isnm:False|TEX-7042-TEX;n:type:ShaderForge.SFN_Slider,id:1658,x:32148,y:33247,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:node_1658,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5,max:1;n:type:ShaderForge.SFN_Multiply,id:4952,x:32489,y:33060,varname:node_4952,prsc:2|A-434-A,B-1658-OUT;n:type:ShaderForge.SFN_TexCoord,id:4553,x:31177,y:32449,varname:node_4553,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:3349,x:31399,y:32519,varname:node_3349,prsc:2|A-4553-UVOUT,B-1407-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1407,x:31177,y:32649,ptovrint:False,ptlb:Tiling,ptin:_Tiling,varname:node_1407,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:6131,x:31006,y:32821,ptovrint:False,ptlb:Speed X,ptin:_SpeedX,varname:node_6131,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_ValueProperty,id:586,x:31006,y:32932,ptovrint:False,ptlb:Speed Y,ptin:_SpeedY,varname:node_586,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Append,id:3575,x:31200,y:32843,varname:node_3575,prsc:2|A-6131-OUT,B-586-OUT;n:type:ShaderForge.SFN_Time,id:3105,x:31200,y:33031,varname:node_3105,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9497,x:31420,y:32935,varname:node_9497,prsc:2|A-3575-OUT,B-3105-TSL;n:type:ShaderForge.SFN_Add,id:4942,x:31674,y:32655,varname:node_4942,prsc:2|A-3349-OUT,B-9497-OUT;proporder:7042-1658-1407-6131-586;pass:END;sub:END;*/

Shader "ShaderForge/Clouds" {
    Properties {
        [NoScaleOffset]_Texture ("Texture", 2D) = "white" {}
        _Opacity ("Opacity", Range(0, 1)) = 0.5
        _Tiling ("Tiling", Float ) = 1
        _SpeedX ("Speed X", Float ) = 1
        _SpeedY ("Speed Y", Float ) = 1
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            uniform float4 _TimeEditor;
            uniform sampler2D _Texture;
            uniform float _Opacity;
            uniform float _Tiling;
            uniform float _SpeedX;
            uniform float _SpeedY;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 node_3105 = _Time + _TimeEditor;
                float2 node_4942 = ((i.uv0*_Tiling)+(float2(_SpeedX,_SpeedY)*node_3105.r));
                float4 node_9622 = tex2D(_Texture,node_4942);
                float3 emissive = node_9622.rgb;
                float3 finalColor = emissive;
                float4 node_434 = tex2D(_Texture,i.uv0);
                return fixed4(finalColor,(node_434.a*_Opacity));
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
