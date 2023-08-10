using UnityEngine;

namespace UltimateSurvival.StandardAssets;

[ExecuteInEditMode]
[AddComponentMenu("Ultimate Survival/Add/Image Effects/Color Correction (Curves, Saturation)")]
public class ColorCorrection : ImageEffectBase
{
	public enum ColorCorrectionMode
	{
		Simple,
		Advanced
	}

	public AnimationCurve redChannel = new AnimationCurve((Keyframe[])(object)new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	public AnimationCurve greenChannel = new AnimationCurve((Keyframe[])(object)new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	public AnimationCurve blueChannel = new AnimationCurve((Keyframe[])(object)new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	public bool useDepthCorrection;

	public AnimationCurve zCurve = new AnimationCurve((Keyframe[])(object)new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	public AnimationCurve depthRedChannel = new AnimationCurve((Keyframe[])(object)new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	public AnimationCurve depthGreenChannel = new AnimationCurve((Keyframe[])(object)new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	public AnimationCurve depthBlueChannel = new AnimationCurve((Keyframe[])(object)new Keyframe[2]
	{
		new Keyframe(0f, 0f),
		new Keyframe(1f, 1f)
	});

	private Material ccMaterial;

	private Material ccDepthMaterial;

	private Material selectiveCcMaterial;

	private Texture2D rgbChannelTex;

	private Texture2D rgbDepthChannelTex;

	private Texture2D zCurveTex;

	public float saturation = 1f;

	public bool selectiveCc;

	public Color selectiveFromColor = Color.white;

	public Color selectiveToColor = Color.white;

	public ColorCorrectionMode mode;

	public bool updateTextures = true;

	public Shader colorCorrectionCurvesShader;

	public Shader simpleColorCorrectionCurvesShader;

	public Shader colorCorrectionSelectiveShader;

	private bool updateTexturesOnStartup = true;

	private new void Start()
	{
		base.Start();
		updateTexturesOnStartup = true;
	}

	private void Awake()
	{
	}

	public override bool CheckResources()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		CheckSupport(mode == ColorCorrectionMode.Advanced);
		ccMaterial = CheckShaderAndCreateMaterial(simpleColorCorrectionCurvesShader, ccMaterial);
		ccDepthMaterial = CheckShaderAndCreateMaterial(colorCorrectionCurvesShader, ccDepthMaterial);
		selectiveCcMaterial = CheckShaderAndCreateMaterial(colorCorrectionSelectiveShader, selectiveCcMaterial);
		if (!Object.op_Implicit((Object)(object)rgbChannelTex))
		{
			rgbChannelTex = new Texture2D(256, 4, (TextureFormat)5, false, true);
		}
		if (!Object.op_Implicit((Object)(object)rgbDepthChannelTex))
		{
			rgbDepthChannelTex = new Texture2D(256, 4, (TextureFormat)5, false, true);
		}
		if (!Object.op_Implicit((Object)(object)zCurveTex))
		{
			zCurveTex = new Texture2D(256, 1, (TextureFormat)5, false, true);
		}
		((Object)rgbChannelTex).hideFlags = (HideFlags)52;
		((Object)rgbDepthChannelTex).hideFlags = (HideFlags)52;
		((Object)zCurveTex).hideFlags = (HideFlags)52;
		((Texture)rgbChannelTex).wrapMode = (TextureWrapMode)1;
		((Texture)rgbDepthChannelTex).wrapMode = (TextureWrapMode)1;
		((Texture)zCurveTex).wrapMode = (TextureWrapMode)1;
		if (!isSupported)
		{
			ReportAutoDisable();
		}
		return isSupported;
	}

	public void UpdateParameters()
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		CheckResources();
		if (redChannel != null && greenChannel != null && blueChannel != null)
		{
			for (float num = 0f; num <= 1f; num += 0.003921569f)
			{
				float num2 = Mathf.Clamp(redChannel.Evaluate(num), 0f, 1f);
				float num3 = Mathf.Clamp(greenChannel.Evaluate(num), 0f, 1f);
				float num4 = Mathf.Clamp(blueChannel.Evaluate(num), 0f, 1f);
				rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
				rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
				rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
				float num5 = Mathf.Clamp(zCurve.Evaluate(num), 0f, 1f);
				zCurveTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num5, num5, num5));
				num2 = Mathf.Clamp(depthRedChannel.Evaluate(num), 0f, 1f);
				num3 = Mathf.Clamp(depthGreenChannel.Evaluate(num), 0f, 1f);
				num4 = Mathf.Clamp(depthBlueChannel.Evaluate(num), 0f, 1f);
				rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
				rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
				rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
			}
			rgbChannelTex.Apply();
			rgbDepthChannelTex.Apply();
			zCurveTex.Apply();
		}
	}

	private void UpdateTextures()
	{
		UpdateParameters();
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		if (!CheckResources())
		{
			Graphics.Blit((Texture)(object)source, destination);
			return;
		}
		if (updateTexturesOnStartup)
		{
			UpdateParameters();
			updateTexturesOnStartup = false;
		}
		if (useDepthCorrection)
		{
			Camera component = ((Component)this).GetComponent<Camera>();
			component.depthTextureMode = (DepthTextureMode)(component.depthTextureMode | 1);
		}
		RenderTexture val = destination;
		if (selectiveCc)
		{
			val = RenderTexture.GetTemporary(((Texture)source).width, ((Texture)source).height);
		}
		if (useDepthCorrection)
		{
			ccDepthMaterial.SetTexture("_RgbTex", (Texture)(object)rgbChannelTex);
			ccDepthMaterial.SetTexture("_ZCurve", (Texture)(object)zCurveTex);
			ccDepthMaterial.SetTexture("_RgbDepthTex", (Texture)(object)rgbDepthChannelTex);
			ccDepthMaterial.SetFloat("_Saturation", saturation);
			Graphics.Blit((Texture)(object)source, val, ccDepthMaterial);
		}
		else
		{
			ccMaterial.SetTexture("_RgbTex", (Texture)(object)rgbChannelTex);
			ccMaterial.SetFloat("_Saturation", saturation);
			Graphics.Blit((Texture)(object)source, val, ccMaterial);
		}
		if (selectiveCc)
		{
			selectiveCcMaterial.SetColor("selColor", selectiveFromColor);
			selectiveCcMaterial.SetColor("targetColor", selectiveToColor);
			Graphics.Blit((Texture)(object)val, destination, selectiveCcMaterial);
			RenderTexture.ReleaseTemporary(val);
		}
	}
}
