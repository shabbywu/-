using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Screen Space Ambient Occlusion")]
public class SSAOEffectDepthCutoff : MonoBehaviour
{
	public enum SSAOSamples
	{
		Low,
		Medium,
		High
	}

	public float m_Radius = 0.4f;

	public SSAOSamples m_SampleCount = SSAOSamples.Medium;

	public float m_OcclusionIntensity = 1.5f;

	public int m_Blur = 2;

	public int m_Downsampling = 2;

	public float m_OcclusionAttenuation = 1f;

	public float m_MinZ = 0.01f;

	public float m_DepthCutoff = 50f;

	public Shader m_SSAOShader;

	private Material m_SSAOMaterial;

	public Texture2D m_RandomTexture;

	private bool m_Supported;

	private static Material CreateMaterial(Shader shader)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (!Object.op_Implicit((Object)(object)shader))
		{
			return null;
		}
		return new Material(shader)
		{
			hideFlags = (HideFlags)61
		};
	}

	private static void DestroyMaterial(Material mat)
	{
		if (Object.op_Implicit((Object)(object)mat))
		{
			Object.DestroyImmediate((Object)(object)mat);
			mat = null;
		}
	}

	private void OnDisable()
	{
		DestroyMaterial(m_SSAOMaterial);
	}

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects || !SystemInfo.SupportsRenderTextureFormat((RenderTextureFormat)1))
		{
			m_Supported = false;
			((Behaviour)this).enabled = false;
			return;
		}
		CreateMaterials();
		if (!Object.op_Implicit((Object)(object)m_SSAOMaterial) || m_SSAOMaterial.passCount != 5)
		{
			m_Supported = false;
			((Behaviour)this).enabled = false;
		}
		else
		{
			m_Supported = true;
		}
	}

	private void OnEnable()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Camera component = ((Component)this).GetComponent<Camera>();
		component.depthTextureMode = (DepthTextureMode)(component.depthTextureMode | 2);
	}

	private void CreateMaterials()
	{
		if (!Object.op_Implicit((Object)(object)m_SSAOMaterial) && m_SSAOShader.isSupported)
		{
			m_SSAOMaterial = CreateMaterial(m_SSAOShader);
			m_SSAOMaterial.SetTexture("_RandomTexture", (Texture)(object)m_RandomTexture);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		if (!m_Supported || !m_SSAOShader.isSupported)
		{
			((Behaviour)this).enabled = false;
			return;
		}
		CreateMaterials();
		m_Downsampling = Mathf.Clamp(m_Downsampling, 1, 6);
		m_Radius = Mathf.Clamp(m_Radius, 0.05f, 1f);
		m_MinZ = Mathf.Clamp(m_MinZ, 1E-05f, 0.5f);
		m_OcclusionIntensity = Mathf.Clamp(m_OcclusionIntensity, 0.5f, 4f);
		m_OcclusionAttenuation = Mathf.Clamp(m_OcclusionAttenuation, 0.2f, 2f);
		m_Blur = Mathf.Clamp(m_Blur, 0, 4);
		RenderTexture val = RenderTexture.GetTemporary(((Texture)source).width / m_Downsampling, ((Texture)source).height / m_Downsampling, 0);
		float fieldOfView = ((Component)this).GetComponent<Camera>().fieldOfView;
		float farClipPlane = ((Component)this).GetComponent<Camera>().farClipPlane;
		float num = Mathf.Tan(fieldOfView * ((float)Math.PI / 180f) * 0.5f) * farClipPlane;
		float num2 = num * ((Component)this).GetComponent<Camera>().aspect;
		m_SSAOMaterial.SetVector("_FarCorner", Vector4.op_Implicit(new Vector3(num2, num, farClipPlane)));
		int num3;
		int num4;
		if (Object.op_Implicit((Object)(object)m_RandomTexture))
		{
			num3 = ((Texture)m_RandomTexture).width;
			num4 = ((Texture)m_RandomTexture).height;
		}
		else
		{
			num3 = 1;
			num4 = 1;
		}
		m_SSAOMaterial.SetVector("_NoiseScale", Vector4.op_Implicit(new Vector3((float)((Texture)val).width / (float)num3, (float)((Texture)val).height / (float)num4, 0f)));
		m_SSAOMaterial.SetVector("_Params", new Vector4(m_Radius, m_MinZ, 1f / m_OcclusionAttenuation, m_OcclusionIntensity));
		m_SSAOMaterial.SetFloat("_DepthCutoff", m_DepthCutoff);
		bool num5 = m_Blur > 0;
		Graphics.Blit((Texture)(object)(num5 ? null : source), val, m_SSAOMaterial, (int)m_SampleCount);
		if (num5)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(((Texture)source).width, ((Texture)source).height, 0);
			m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4((float)m_Blur / (float)((Texture)source).width, 0f, 0f, 0f));
			m_SSAOMaterial.SetTexture("_SSAO", (Texture)(object)val);
			Graphics.Blit((Texture)null, temporary, m_SSAOMaterial, 3);
			RenderTexture.ReleaseTemporary(val);
			RenderTexture temporary2 = RenderTexture.GetTemporary(((Texture)source).width, ((Texture)source).height, 0);
			m_SSAOMaterial.SetVector("_TexelOffsetScale", new Vector4(0f, (float)m_Blur / (float)((Texture)source).height, 0f, 0f));
			m_SSAOMaterial.SetTexture("_SSAO", (Texture)(object)temporary);
			Graphics.Blit((Texture)(object)source, temporary2, m_SSAOMaterial, 3);
			RenderTexture.ReleaseTemporary(temporary);
			val = temporary2;
		}
		m_SSAOMaterial.SetTexture("_SSAO", (Texture)(object)val);
		Graphics.Blit((Texture)(object)source, destination, m_SSAOMaterial, 4);
		RenderTexture.ReleaseTemporary(val);
	}
}
