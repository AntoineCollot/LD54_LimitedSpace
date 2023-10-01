// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/SpriteRAMHurt"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
		_LockedColor ("Locked Color", Color)=(1,1,1,1)
		_RAMState ("RAM State", Integer) = 0
		_HurtFlashDuration ("Hurt Flash Duration", Range(0,1)) = 0.2
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
            #pragma vertex SpriteVert2
            #pragma fragment SpriteRAMFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"

		
		int _RAMState;
		fixed4 _LockedColor;
		half _UnscaledTime;
		half _HitTime;
		half _HurtFlashDuration;
		
		v2f SpriteVert2(appdata_t IN)
		{
			v2f OUT;

			UNITY_SETUP_INSTANCE_ID (IN);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

			OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
			OUT.vertex = UnityObjectToClipPos(OUT.vertex);
			OUT.texcoord = IN.texcoord;
			OUT.color = IN.color * _Color * _RendererColor;
			#ifdef PIXELSNAP_ON
			OUT.vertex = UnityPixelSnap (OUT.vertex);
			#endif

			return OUT;
		}
		
		fixed4 SpriteRAMFrag(v2f IN) : SV_Target
		{
			fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
			c.rgb = c.rgb * step(_HitTime + _HurtFlashDuration,_Time.y) +  step(_Time.y,_HitTime + _HurtFlashDuration);
			c.rgb *= c.a;
			
			//Hover
			half t = (sin(_UnscaledTime * 8) + 2) * 0.5;
			fixed4 hovCol0 = fixed4(0.910063, 0.3254717,1,1);
			fixed4 hovCol1 = fixed4(0.9341931, 0.5,1,1);
			c = c * step(0.5,_RAMState-1) + lerp(hovCol0, hovCol1,t) * step(_RAMState-1,0.5);
			
			//Locked
			c = c * step(0.5,_RAMState) + _LockedColor * step(_RAMState,0.5);
			
			return c;
		}
		ENDCG
		}
	}
}
