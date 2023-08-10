using UnityEngine;

namespace UltimateSurvival.StandardAssets;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Ultimate Survival/Add/Image Effects/Antialiasing")]
public class AA : ImageEffectBase
{
	public AAMode mode = AAMode.FXAA3Console;

	public bool showGeneratedNormals;

	public float offsetScale = 0.2f;

	public float blurRadius = 18f;

	public float edgeThresholdMin = 0.05f;

	public float edgeThreshold = 0.2f;

	public float edgeSharpness = 4f;

	public bool dlaaSharp;

	public Shader ssaaShader;

	private Material ssaa;

	public Shader dlaaShader;

	private Material dlaa;

	public Shader nfaaShader;

	private Material nfaa;

	public Shader shaderFXAAPreset2;

	private Material materialFXAAPreset2;

	public Shader shaderFXAAPreset3;

	private Material materialFXAAPreset3;

	public Shader shaderFXAAII;

	private Material materialFXAAII;

	public Shader shaderFXAAIII;

	private Material materialFXAAIII;

	public Material CurrentAAMaterial()
	{
		Material val = null;
		return (Material)(mode switch
		{
			AAMode.FXAA3Console => materialFXAAIII, 
			AAMode.FXAA2 => materialFXAAII, 
			AAMode.FXAA1PresetA => materialFXAAPreset2, 
			AAMode.FXAA1PresetB => materialFXAAPreset3, 
			AAMode.NFAA => nfaa, 
			AAMode.SSAA => ssaa, 
			AAMode.DLAA => dlaa, 
			_ => null, 
		});
	}

	public override bool CheckResources()
	{
		CheckSupport(needDepth: false);
		materialFXAAPreset2 = CreateMaterial(shaderFXAAPreset2, materialFXAAPreset2);
		materialFXAAPreset3 = CreateMaterial(shaderFXAAPreset3, materialFXAAPreset3);
		materialFXAAII = CreateMaterial(shaderFXAAII, materialFXAAII);
		materialFXAAIII = CreateMaterial(shaderFXAAIII, materialFXAAIII);
		nfaa = CreateMaterial(nfaaShader, nfaa);
		ssaa = CreateMaterial(ssaaShader, ssaa);
		dlaa = CreateMaterial(dlaaShader, dlaa);
		if (!ssaaShader.isSupported)
		{
			NotSupported();
			ReportAutoDisable();
		}
		return isSupported;
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (!CheckResources())
		{
			Graphics.Blit((Texture)(object)source, destination);
		}
		else if (mode == AAMode.FXAA3Console && (Object)(object)materialFXAAIII != (Object)null)
		{
			materialFXAAIII.SetFloat("_EdgeThresholdMin", edgeThresholdMin);
			materialFXAAIII.SetFloat("_EdgeThreshold", edgeThreshold);
			materialFXAAIII.SetFloat("_EdgeSharpness", edgeSharpness);
			Graphics.Blit((Texture)(object)source, destination, materialFXAAIII);
		}
		else if (mode == AAMode.FXAA1PresetB && (Object)(object)materialFXAAPreset3 != (Object)null)
		{
			Graphics.Blit((Texture)(object)source, destination, materialFXAAPreset3);
		}
		else if (mode == AAMode.FXAA1PresetA && (Object)(object)materialFXAAPreset2 != (Object)null)
		{
			((Texture)source).anisoLevel = 4;
			Graphics.Blit((Texture)(object)source, destination, materialFXAAPreset2);
			((Texture)source).anisoLevel = 0;
		}
		else if (mode == AAMode.FXAA2 && (Object)(object)materialFXAAII != (Object)null)
		{
			Graphics.Blit((Texture)(object)source, destination, materialFXAAII);
		}
		else if (mode == AAMode.SSAA && (Object)(object)ssaa != (Object)null)
		{
			Graphics.Blit((Texture)(object)source, destination, ssaa);
		}
		else if (mode == AAMode.DLAA && (Object)(object)dlaa != (Object)null)
		{
			((Texture)source).anisoLevel = 0;
			RenderTexture temporary = RenderTexture.GetTemporary(((Texture)source).width, ((Texture)source).height);
			Graphics.Blit((Texture)(object)source, temporary, dlaa, 0);
			Graphics.Blit((Texture)(object)temporary, destination, dlaa, (!dlaaSharp) ? 1 : 2);
			RenderTexture.ReleaseTemporary(temporary);
		}
		else if (mode == AAMode.NFAA && (Object)(object)nfaa != (Object)null)
		{
			((Texture)source).anisoLevel = 0;
			nfaa.SetFloat("_OffsetScale", offsetScale);
			nfaa.SetFloat("_BlurRadius", blurRadius);
			Graphics.Blit((Texture)(object)source, destination, nfaa, showGeneratedNormals ? 1 : 0);
		}
		else
		{
			Graphics.Blit((Texture)(object)source, destination);
		}
	}
}
