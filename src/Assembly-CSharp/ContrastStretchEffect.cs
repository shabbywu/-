using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Contrast Stretch")]
public class ContrastStretchEffect : MonoBehaviour
{
	public float adaptationSpeed = 0.02f;

	public float limitMinimum = 0.2f;

	public float limitMaximum = 0.6f;

	private RenderTexture[] adaptRenderTex = (RenderTexture[])(object)new RenderTexture[2];

	private int curAdaptIndex;

	public Shader shaderLum;

	private Material m_materialLum;

	public Shader shaderReduce;

	private Material m_materialReduce;

	public Shader shaderAdapt;

	private Material m_materialAdapt;

	public Shader shaderApply;

	private Material m_materialApply;

	protected Material materialLum
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			if ((Object)(object)m_materialLum == (Object)null)
			{
				m_materialLum = new Material(shaderLum);
				((Object)m_materialLum).hideFlags = (HideFlags)61;
			}
			return m_materialLum;
		}
	}

	protected Material materialReduce
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			if ((Object)(object)m_materialReduce == (Object)null)
			{
				m_materialReduce = new Material(shaderReduce);
				((Object)m_materialReduce).hideFlags = (HideFlags)61;
			}
			return m_materialReduce;
		}
	}

	protected Material materialAdapt
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			if ((Object)(object)m_materialAdapt == (Object)null)
			{
				m_materialAdapt = new Material(shaderAdapt);
				((Object)m_materialAdapt).hideFlags = (HideFlags)61;
			}
			return m_materialAdapt;
		}
	}

	protected Material materialApply
	{
		get
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			if ((Object)(object)m_materialApply == (Object)null)
			{
				m_materialApply = new Material(shaderApply);
				((Object)m_materialApply).hideFlags = (HideFlags)61;
			}
			return m_materialApply;
		}
	}

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects)
		{
			((Behaviour)this).enabled = false;
		}
		else if (!shaderAdapt.isSupported || !shaderApply.isSupported || !shaderLum.isSupported || !shaderReduce.isSupported)
		{
			((Behaviour)this).enabled = false;
		}
	}

	private void OnEnable()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		for (int i = 0; i < 2; i++)
		{
			if (!Object.op_Implicit((Object)(object)adaptRenderTex[i]))
			{
				adaptRenderTex[i] = new RenderTexture(1, 1, 32);
				((Object)adaptRenderTex[i]).hideFlags = (HideFlags)61;
			}
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < 2; i++)
		{
			Object.DestroyImmediate((Object)(object)adaptRenderTex[i]);
			adaptRenderTex[i] = null;
		}
		if (Object.op_Implicit((Object)(object)m_materialLum))
		{
			Object.DestroyImmediate((Object)(object)m_materialLum);
		}
		if (Object.op_Implicit((Object)(object)m_materialReduce))
		{
			Object.DestroyImmediate((Object)(object)m_materialReduce);
		}
		if (Object.op_Implicit((Object)(object)m_materialAdapt))
		{
			Object.DestroyImmediate((Object)(object)m_materialAdapt);
		}
		if (Object.op_Implicit((Object)(object)m_materialApply))
		{
			Object.DestroyImmediate((Object)(object)m_materialApply);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture val = RenderTexture.GetTemporary(((Texture)source).width / 1, ((Texture)source).height / 1);
		Graphics.Blit((Texture)(object)source, val, materialLum);
		while (((Texture)val).width > 1 || ((Texture)val).height > 1)
		{
			int num = ((Texture)val).width / 2;
			if (num < 1)
			{
				num = 1;
			}
			int num2 = ((Texture)val).height / 2;
			if (num2 < 1)
			{
				num2 = 1;
			}
			RenderTexture temporary = RenderTexture.GetTemporary(num, num2);
			Graphics.Blit((Texture)(object)val, temporary, materialReduce);
			RenderTexture.ReleaseTemporary(val);
			val = temporary;
		}
		CalculateAdaptation((Texture)(object)val);
		materialApply.SetTexture("_AdaptTex", (Texture)(object)adaptRenderTex[curAdaptIndex]);
		Graphics.Blit((Texture)(object)source, destination, materialApply);
		RenderTexture.ReleaseTemporary(val);
	}

	private void CalculateAdaptation(Texture curTexture)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		int num = curAdaptIndex;
		curAdaptIndex = (curAdaptIndex + 1) % 2;
		float num2 = 1f - Mathf.Pow(1f - adaptationSpeed, 30f * Time.deltaTime);
		num2 = Mathf.Clamp(num2, 0.01f, 1f);
		materialAdapt.SetTexture("_CurTex", curTexture);
		materialAdapt.SetVector("_AdaptParams", new Vector4(num2, limitMinimum, limitMaximum, 0f));
		Graphics.Blit((Texture)(object)adaptRenderTex[num], adaptRenderTex[curAdaptIndex], materialAdapt);
	}
}
