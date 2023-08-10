using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Motion Blur (Color Accumulation)")]
[RequireComponent(typeof(Camera))]
public class MotionBlur : ImageEffectBase
{
	public float blurAmount = 0.8f;

	public bool extraBlur;

	private RenderTexture accumTexture;

	protected override void Start()
	{
		if (!SystemInfo.supportsRenderTextures)
		{
			((Behaviour)this).enabled = false;
		}
		else
		{
			base.Start();
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		Object.DestroyImmediate((Object)(object)accumTexture);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		if ((Object)(object)accumTexture == (Object)null || ((Texture)accumTexture).width != ((Texture)source).width || ((Texture)accumTexture).height != ((Texture)source).height)
		{
			Object.DestroyImmediate((Object)(object)accumTexture);
			accumTexture = new RenderTexture(((Texture)source).width, ((Texture)source).height, 0);
			((Object)accumTexture).hideFlags = (HideFlags)61;
			Graphics.Blit((Texture)(object)source, accumTexture);
		}
		if (extraBlur)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(((Texture)source).width / 4, ((Texture)source).height / 4, 0);
			Graphics.Blit((Texture)(object)accumTexture, temporary);
			Graphics.Blit((Texture)(object)temporary, accumTexture);
			RenderTexture.ReleaseTemporary(temporary);
		}
		blurAmount = Mathf.Clamp(blurAmount, 0f, 0.92f);
		base.material.SetTexture("_MainTex", (Texture)(object)accumTexture);
		base.material.SetFloat("_AccumOrig", 1f - blurAmount);
		Graphics.Blit((Texture)(object)source, accumTexture, base.material);
		Graphics.Blit((Texture)(object)accumTexture, destination);
	}
}
