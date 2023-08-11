using UnityEngine;

namespace UltimateSurvival.StandardAssets;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Ultimate Survival/Add/Image Effects/DOF")]
public class DOF : ImageEffectBase
{
	public enum Dof34QualitySetting
	{
		OnlyBackground = 1,
		BackgroundAndForeground
	}

	public enum DofResolution
	{
		High = 2,
		Medium,
		Low
	}

	public enum DofBlurriness
	{
		Low = 1,
		High = 2,
		VeryHigh = 4
	}

	public enum BokehDestination
	{
		Background = 1,
		Foreground,
		BackgroundAndForeground
	}

	private static int SMOOTH_DOWNSAMPLE_PASS = 6;

	private static float BOKEH_EXTRA_BLUR = 2f;

	public Dof34QualitySetting quality = Dof34QualitySetting.OnlyBackground;

	public DofResolution resolution = DofResolution.Low;

	public bool simpleTweakMode = true;

	public float focalPoint = 1f;

	public float smoothness = 0.5f;

	public float focalZDistance;

	public float focalZStartCurve = 1f;

	public float focalZEndCurve = 1f;

	private float focalStartCurve = 2f;

	private float focalEndCurve = 2f;

	private float focalDistance01 = 0.1f;

	public Transform objectFocus;

	public float focalSize;

	public DofBlurriness bluriness = DofBlurriness.High;

	public float maxBlurSpread = 1.75f;

	public float foregroundBlurExtrude = 1.15f;

	public Shader dofBlurShader;

	private Material dofBlurMaterial;

	public Shader dofShader;

	private Material dofMaterial;

	public bool visualize;

	public BokehDestination bokehDestination = BokehDestination.Background;

	private float widthOverHeight = 1.25f;

	private float oneOverBaseSize = 0.001953125f;

	public bool bokeh;

	public bool bokehSupport = true;

	public Shader bokehShader;

	public Texture2D bokehTexture;

	public float bokehScale = 2.4f;

	public float bokehIntensity = 0.15f;

	public float bokehThresholdContrast = 0.1f;

	public float bokehThresholdLuminance = 0.55f;

	public int bokehDownsample = 1;

	private Material bokehMaterial;

	private Camera _camera;

	private RenderTexture foregroundTexture;

	private RenderTexture mediumRezWorkTexture;

	private RenderTexture finalDefocus;

	private RenderTexture lowRezWorkTexture;

	private RenderTexture bokehSource;

	private RenderTexture bokehSource2;

	private void CreateMaterials()
	{
		dofBlurMaterial = CheckShaderAndCreateMaterial(dofBlurShader, dofBlurMaterial);
		dofMaterial = CheckShaderAndCreateMaterial(dofShader, dofMaterial);
		bokehSupport = bokehShader.isSupported;
		if (bokeh && bokehSupport && Object.op_Implicit((Object)(object)bokehShader))
		{
			bokehMaterial = CheckShaderAndCreateMaterial(bokehShader, bokehMaterial);
		}
	}

	public override bool CheckResources()
	{
		CheckSupport(needDepth: true);
		dofBlurMaterial = CheckShaderAndCreateMaterial(dofBlurShader, dofBlurMaterial);
		dofMaterial = CheckShaderAndCreateMaterial(dofShader, dofMaterial);
		bokehSupport = bokehShader.isSupported;
		if (bokeh && bokehSupport && Object.op_Implicit((Object)(object)bokehShader))
		{
			bokehMaterial = CheckShaderAndCreateMaterial(bokehShader, bokehMaterial);
		}
		if (!isSupported)
		{
			ReportAutoDisable();
		}
		return isSupported;
	}

	private void OnDisable()
	{
		MyQuads.Cleanup();
	}

	private void OnEnable()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		_camera = ((Component)this).GetComponent<Camera>();
		Camera camera = _camera;
		camera.depthTextureMode = (DepthTextureMode)(camera.depthTextureMode | 1);
	}

	private float FocalDistance01(float worldDist)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		return _camera.WorldToViewportPoint((worldDist - _camera.nearClipPlane) * ((Component)_camera).transform.forward + ((Component)_camera).transform.position).z / (_camera.farClipPlane - _camera.nearClipPlane);
	}

	private int GetDividerBasedOnQuality()
	{
		int result = 1;
		if (resolution == DofResolution.Medium)
		{
			result = 2;
		}
		else if (resolution == DofResolution.Low)
		{
			result = 2;
		}
		return result;
	}

	private int GetLowResolutionDividerBasedOnQuality(int baseDivider)
	{
		int num = baseDivider;
		if (resolution == DofResolution.High)
		{
			num *= 2;
		}
		if (resolution == DofResolution.Low)
		{
			num *= 2;
		}
		return num;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		if (!CheckResources())
		{
			Graphics.Blit((Texture)(object)source, destination);
			return;
		}
		if (smoothness < 0.1f)
		{
			smoothness = 0.1f;
		}
		bokeh = bokeh && bokehSupport;
		float num = (bokeh ? BOKEH_EXTRA_BLUR : 1f);
		bool flag = quality > Dof34QualitySetting.OnlyBackground;
		float num2 = focalSize / (_camera.farClipPlane - _camera.nearClipPlane);
		if (simpleTweakMode)
		{
			focalDistance01 = (Object.op_Implicit((Object)(object)objectFocus) ? (_camera.WorldToViewportPoint(objectFocus.position).z / _camera.farClipPlane) : FocalDistance01(focalPoint));
			focalStartCurve = focalDistance01 * smoothness;
			focalEndCurve = focalStartCurve;
			flag = flag && focalPoint > _camera.nearClipPlane + Mathf.Epsilon;
		}
		else
		{
			if (Object.op_Implicit((Object)(object)objectFocus))
			{
				Vector3 val = _camera.WorldToViewportPoint(objectFocus.position);
				val.z /= _camera.farClipPlane;
				focalDistance01 = val.z;
			}
			else
			{
				focalDistance01 = FocalDistance01(focalZDistance);
			}
			focalStartCurve = focalZStartCurve;
			focalEndCurve = focalZEndCurve;
			flag = flag && focalPoint > _camera.nearClipPlane + Mathf.Epsilon;
		}
		widthOverHeight = 1f * (float)((Texture)source).width / (1f * (float)((Texture)source).height);
		oneOverBaseSize = 0.001953125f;
		dofMaterial.SetFloat("_ForegroundBlurExtrude", foregroundBlurExtrude);
		dofMaterial.SetVector("_CurveParams", new Vector4(simpleTweakMode ? (1f / focalStartCurve) : focalStartCurve, simpleTweakMode ? (1f / focalEndCurve) : focalEndCurve, num2 * 0.5f, focalDistance01));
		dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)((Texture)source).width), 1f / (1f * (float)((Texture)source).height), 0f, 0f));
		int dividerBasedOnQuality = GetDividerBasedOnQuality();
		int lowResolutionDividerBasedOnQuality = GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality);
		AllocateTextures(flag, source, dividerBasedOnQuality, lowResolutionDividerBasedOnQuality);
		Graphics.Blit((Texture)(object)source, source, dofMaterial, 3);
		Downsample(source, mediumRezWorkTexture);
		Blur(mediumRezWorkTexture, mediumRezWorkTexture, DofBlurriness.Low, 4, maxBlurSpread);
		if (bokeh && (BokehDestination.Foreground & bokehDestination) != 0)
		{
			dofMaterial.SetVector("_Threshhold", new Vector4(bokehThresholdContrast, bokehThresholdLuminance, 0.95f, 0f));
			Graphics.Blit((Texture)(object)mediumRezWorkTexture, bokehSource2, dofMaterial, 11);
			Graphics.Blit((Texture)(object)mediumRezWorkTexture, lowRezWorkTexture);
			Blur(lowRezWorkTexture, lowRezWorkTexture, bluriness, 0, maxBlurSpread * num);
		}
		else
		{
			Downsample(mediumRezWorkTexture, lowRezWorkTexture);
			Blur(lowRezWorkTexture, lowRezWorkTexture, bluriness, 0, maxBlurSpread);
		}
		dofBlurMaterial.SetTexture("_TapLow", (Texture)(object)lowRezWorkTexture);
		dofBlurMaterial.SetTexture("_TapMedium", (Texture)(object)mediumRezWorkTexture);
		Graphics.Blit((Texture)null, finalDefocus, dofBlurMaterial, 3);
		if (bokeh && (BokehDestination.Foreground & bokehDestination) != 0)
		{
			AddBokeh(bokehSource2, bokehSource, finalDefocus);
		}
		dofMaterial.SetTexture("_TapLowBackground", (Texture)(object)finalDefocus);
		dofMaterial.SetTexture("_TapMedium", (Texture)(object)mediumRezWorkTexture);
		Graphics.Blit((Texture)(object)source, flag ? foregroundTexture : destination, dofMaterial, visualize ? 2 : 0);
		if (flag)
		{
			Graphics.Blit((Texture)(object)foregroundTexture, source, dofMaterial, 5);
			Downsample(source, mediumRezWorkTexture);
			BlurFg(mediumRezWorkTexture, mediumRezWorkTexture, DofBlurriness.Low, 2, maxBlurSpread);
			if (bokeh && (BokehDestination.Foreground & bokehDestination) != 0)
			{
				dofMaterial.SetVector("_Threshhold", new Vector4(bokehThresholdContrast * 0.5f, bokehThresholdLuminance, 0f, 0f));
				Graphics.Blit((Texture)(object)mediumRezWorkTexture, bokehSource2, dofMaterial, 11);
				Graphics.Blit((Texture)(object)mediumRezWorkTexture, lowRezWorkTexture);
				BlurFg(lowRezWorkTexture, lowRezWorkTexture, bluriness, 1, maxBlurSpread * num);
			}
			else
			{
				BlurFg(mediumRezWorkTexture, lowRezWorkTexture, bluriness, 1, maxBlurSpread);
			}
			Graphics.Blit((Texture)(object)lowRezWorkTexture, finalDefocus);
			dofMaterial.SetTexture("_TapLowForeground", (Texture)(object)finalDefocus);
			Graphics.Blit((Texture)(object)source, destination, dofMaterial, visualize ? 1 : 4);
			if (bokeh && (BokehDestination.Foreground & bokehDestination) != 0)
			{
				AddBokeh(bokehSource2, bokehSource, destination);
			}
		}
		ReleaseTextures();
	}

	private void Blur(RenderTexture from, RenderTexture to, DofBlurriness iterations, int blurPass, float spread)
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		RenderTexture temporary = RenderTexture.GetTemporary(((Texture)to).width, ((Texture)to).height);
		if (iterations > DofBlurriness.Low)
		{
			BlurHex(from, to, blurPass, spread, temporary);
			if (iterations > DofBlurriness.High)
			{
				dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * oneOverBaseSize, 0f, 0f));
				Graphics.Blit((Texture)(object)to, temporary, dofBlurMaterial, blurPass);
				dofBlurMaterial.SetVector("offsets", new Vector4(spread / widthOverHeight * oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit((Texture)(object)temporary, to, dofBlurMaterial, blurPass);
			}
		}
		else
		{
			dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * oneOverBaseSize, 0f, 0f));
			Graphics.Blit((Texture)(object)from, temporary, dofBlurMaterial, blurPass);
			dofBlurMaterial.SetVector("offsets", new Vector4(spread / widthOverHeight * oneOverBaseSize, 0f, 0f, 0f));
			Graphics.Blit((Texture)(object)temporary, to, dofBlurMaterial, blurPass);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}

	private void BlurFg(RenderTexture from, RenderTexture to, DofBlurriness iterations, int blurPass, float spread)
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		dofBlurMaterial.SetTexture("_TapHigh", (Texture)(object)from);
		RenderTexture temporary = RenderTexture.GetTemporary(((Texture)to).width, ((Texture)to).height);
		if (iterations > DofBlurriness.Low)
		{
			BlurHex(from, to, blurPass, spread, temporary);
			if (iterations > DofBlurriness.High)
			{
				dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * oneOverBaseSize, 0f, 0f));
				Graphics.Blit((Texture)(object)to, temporary, dofBlurMaterial, blurPass);
				dofBlurMaterial.SetVector("offsets", new Vector4(spread / widthOverHeight * oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit((Texture)(object)temporary, to, dofBlurMaterial, blurPass);
			}
		}
		else
		{
			dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * oneOverBaseSize, 0f, 0f));
			Graphics.Blit((Texture)(object)from, temporary, dofBlurMaterial, blurPass);
			dofBlurMaterial.SetVector("offsets", new Vector4(spread / widthOverHeight * oneOverBaseSize, 0f, 0f, 0f));
			Graphics.Blit((Texture)(object)temporary, to, dofBlurMaterial, blurPass);
		}
		RenderTexture.ReleaseTemporary(temporary);
	}

	private void BlurHex(RenderTexture from, RenderTexture to, int blurPass, float spread, RenderTexture tmp)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * oneOverBaseSize, 0f, 0f));
		Graphics.Blit((Texture)(object)from, tmp, dofBlurMaterial, blurPass);
		dofBlurMaterial.SetVector("offsets", new Vector4(spread / widthOverHeight * oneOverBaseSize, 0f, 0f, 0f));
		Graphics.Blit((Texture)(object)tmp, to, dofBlurMaterial, blurPass);
		dofBlurMaterial.SetVector("offsets", new Vector4(spread / widthOverHeight * oneOverBaseSize, spread * oneOverBaseSize, 0f, 0f));
		Graphics.Blit((Texture)(object)to, tmp, dofBlurMaterial, blurPass);
		dofBlurMaterial.SetVector("offsets", new Vector4(spread / widthOverHeight * oneOverBaseSize, (0f - spread) * oneOverBaseSize, 0f, 0f));
		Graphics.Blit((Texture)(object)tmp, to, dofBlurMaterial, blurPass);
	}

	private void Downsample(RenderTexture from, RenderTexture to)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)((Texture)to).width), 1f / (1f * (float)((Texture)to).height), 0f, 0f));
		Graphics.Blit((Texture)(object)from, to, dofMaterial, SMOOTH_DOWNSAMPLE_PASS);
	}

	private void AddBokeh(RenderTexture bokehInfo, RenderTexture tempTex, RenderTexture finalTarget)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)bokehMaterial))
		{
			return;
		}
		Mesh[] meshes = MyQuads.GetMeshes(((Texture)tempTex).width, ((Texture)tempTex).height);
		RenderTexture.active = tempTex;
		GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
		GL.PushMatrix();
		GL.LoadIdentity();
		((Texture)bokehInfo).filterMode = (FilterMode)0;
		float num = (float)((Texture)bokehInfo).width * 1f / ((float)((Texture)bokehInfo).height * 1f);
		float num2 = 2f / (1f * (float)((Texture)bokehInfo).width);
		num2 += bokehScale * maxBlurSpread * BOKEH_EXTRA_BLUR * oneOverBaseSize;
		bokehMaterial.SetTexture("_Source", (Texture)(object)bokehInfo);
		bokehMaterial.SetTexture("_MainTex", (Texture)(object)bokehTexture);
		bokehMaterial.SetVector("_ArScale", new Vector4(num2, num2 * num, 0.5f, 0.5f * num));
		bokehMaterial.SetFloat("_Intensity", bokehIntensity);
		bokehMaterial.SetPass(0);
		Mesh[] array = meshes;
		foreach (Mesh val in array)
		{
			if (Object.op_Implicit((Object)(object)val))
			{
				Graphics.DrawMeshNow(val, Matrix4x4.identity);
			}
		}
		GL.PopMatrix();
		Graphics.Blit((Texture)(object)tempTex, finalTarget, dofMaterial, 8);
		((Texture)bokehInfo).filterMode = (FilterMode)1;
	}

	private void ReleaseTextures()
	{
		if (Object.op_Implicit((Object)(object)foregroundTexture))
		{
			RenderTexture.ReleaseTemporary(foregroundTexture);
		}
		if (Object.op_Implicit((Object)(object)finalDefocus))
		{
			RenderTexture.ReleaseTemporary(finalDefocus);
		}
		if (Object.op_Implicit((Object)(object)mediumRezWorkTexture))
		{
			RenderTexture.ReleaseTemporary(mediumRezWorkTexture);
		}
		if (Object.op_Implicit((Object)(object)lowRezWorkTexture))
		{
			RenderTexture.ReleaseTemporary(lowRezWorkTexture);
		}
		if (Object.op_Implicit((Object)(object)bokehSource))
		{
			RenderTexture.ReleaseTemporary(bokehSource);
		}
		if (Object.op_Implicit((Object)(object)bokehSource2))
		{
			RenderTexture.ReleaseTemporary(bokehSource2);
		}
	}

	private void AllocateTextures(bool blurForeground, RenderTexture source, int divider, int lowTexDivider)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		foregroundTexture = null;
		if (blurForeground)
		{
			foregroundTexture = RenderTexture.GetTemporary(((Texture)source).width, ((Texture)source).height, 0);
		}
		mediumRezWorkTexture = RenderTexture.GetTemporary(((Texture)source).width / divider, ((Texture)source).height / divider, 0);
		finalDefocus = RenderTexture.GetTemporary(((Texture)source).width / divider, ((Texture)source).height / divider, 0);
		lowRezWorkTexture = RenderTexture.GetTemporary(((Texture)source).width / lowTexDivider, ((Texture)source).height / lowTexDivider, 0);
		bokehSource = null;
		bokehSource2 = null;
		if (bokeh)
		{
			bokehSource = RenderTexture.GetTemporary(((Texture)source).width / (lowTexDivider * bokehDownsample), ((Texture)source).height / (lowTexDivider * bokehDownsample), 0, (RenderTextureFormat)2);
			bokehSource2 = RenderTexture.GetTemporary(((Texture)source).width / (lowTexDivider * bokehDownsample), ((Texture)source).height / (lowTexDivider * bokehDownsample), 0, (RenderTextureFormat)2);
			((Texture)bokehSource).filterMode = (FilterMode)1;
			((Texture)bokehSource2).filterMode = (FilterMode)1;
			RenderTexture.active = bokehSource2;
			GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
		}
		((Texture)source).filterMode = (FilterMode)1;
		((Texture)finalDefocus).filterMode = (FilterMode)1;
		((Texture)mediumRezWorkTexture).filterMode = (FilterMode)1;
		((Texture)lowRezWorkTexture).filterMode = (FilterMode)1;
		if (Object.op_Implicit((Object)(object)foregroundTexture))
		{
			((Texture)foregroundTexture).filterMode = (FilterMode)1;
		}
	}
}
