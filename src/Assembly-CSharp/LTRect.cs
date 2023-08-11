using System;
using UnityEngine;

[Serializable]
public class LTRect
{
	public Rect _rect;

	public float alpha = 1f;

	public float rotation;

	public Vector2 pivot;

	public Vector2 margin;

	public Rect relativeRect = new Rect(0f, 0f, float.PositiveInfinity, float.PositiveInfinity);

	public bool rotateEnabled;

	[HideInInspector]
	public bool rotateFinished;

	public bool alphaEnabled;

	public string labelStr;

	public LTGUI.Element_Type type;

	public GUIStyle style;

	public bool useColor;

	public Color color = Color.white;

	public bool fontScaleToFit;

	public bool useSimpleScale;

	public bool sizeByHeight;

	public Texture texture;

	private int _id = -1;

	[HideInInspector]
	public int counter;

	public static bool colorTouched;

	public bool hasInitiliazed => _id != -1;

	public int id => _id | (counter << 16);

	public float x
	{
		get
		{
			return ((Rect)(ref _rect)).x;
		}
		set
		{
			((Rect)(ref _rect)).x = value;
		}
	}

	public float y
	{
		get
		{
			return ((Rect)(ref _rect)).y;
		}
		set
		{
			((Rect)(ref _rect)).y = value;
		}
	}

	public float width
	{
		get
		{
			return ((Rect)(ref _rect)).width;
		}
		set
		{
			((Rect)(ref _rect)).width = value;
		}
	}

	public float height
	{
		get
		{
			return ((Rect)(ref _rect)).height;
		}
		set
		{
			((Rect)(ref _rect)).height = value;
		}
	}

	public Rect rect
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			if (colorTouched)
			{
				colorTouched = false;
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 1f);
			}
			if (rotateEnabled)
			{
				if (rotateFinished)
				{
					rotateFinished = false;
					rotateEnabled = false;
					pivot = Vector2.zero;
				}
				else
				{
					GUIUtility.RotateAroundPivot(rotation, pivot);
				}
			}
			if (alphaEnabled)
			{
				GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
				colorTouched = true;
			}
			if (fontScaleToFit)
			{
				if (useSimpleScale)
				{
					style.fontSize = (int)(((Rect)(ref _rect)).height * ((Rect)(ref relativeRect)).height);
				}
				else
				{
					style.fontSize = (int)((Rect)(ref _rect)).height;
				}
			}
			return _rect;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_rect = value;
		}
	}

	public LTRect()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		reset();
		rotateEnabled = (alphaEnabled = true);
		_rect = new Rect(0f, 0f, 1f, 1f);
	}

	public LTRect(Rect rect)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		_rect = rect;
		reset();
	}

	public LTRect(float x, float y, float width, float height)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		_rect = new Rect(x, y, width, height);
		alpha = 1f;
		rotation = 0f;
		rotateEnabled = (alphaEnabled = false);
	}

	public LTRect(float x, float y, float width, float height, float alpha)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		_rect = new Rect(x, y, width, height);
		this.alpha = alpha;
		rotation = 0f;
		rotateEnabled = (alphaEnabled = false);
	}

	public LTRect(float x, float y, float width, float height, float alpha, float rotation)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		_rect = new Rect(x, y, width, height);
		this.alpha = alpha;
		this.rotation = rotation;
		rotateEnabled = (alphaEnabled = false);
		if (rotation != 0f)
		{
			rotateEnabled = true;
			resetForRotation();
		}
	}

	public void setId(int id, int counter)
	{
		_id = id;
		this.counter = counter;
	}

	public void reset()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		alpha = 1f;
		rotation = 0f;
		rotateEnabled = (alphaEnabled = false);
		margin = Vector2.zero;
		sizeByHeight = false;
		useColor = false;
	}

	public void resetForRotation()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		Matrix4x4 matrix = GUI.matrix;
		float num = ((Matrix4x4)(ref matrix))[0, 0];
		matrix = GUI.matrix;
		float num2 = ((Matrix4x4)(ref matrix))[1, 1];
		matrix = GUI.matrix;
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(num, num2, ((Matrix4x4)(ref matrix))[2, 2]);
		if (pivot == Vector2.zero)
		{
			float num3 = (((Rect)(ref _rect)).x + ((Rect)(ref _rect)).width * 0.5f) * val.x;
			matrix = GUI.matrix;
			float num4 = num3 + ((Matrix4x4)(ref matrix))[0, 3];
			float num5 = (((Rect)(ref _rect)).y + ((Rect)(ref _rect)).height * 0.5f) * val.y;
			matrix = GUI.matrix;
			pivot = new Vector2(num4, num5 + ((Matrix4x4)(ref matrix))[1, 3]);
		}
	}

	public LTRect setStyle(GUIStyle style)
	{
		this.style = style;
		return this;
	}

	public LTRect setFontScaleToFit(bool fontScaleToFit)
	{
		this.fontScaleToFit = fontScaleToFit;
		return this;
	}

	public LTRect setColor(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		this.color = color;
		useColor = true;
		return this;
	}

	public LTRect setAlpha(float alpha)
	{
		this.alpha = alpha;
		return this;
	}

	public LTRect setLabel(string str)
	{
		labelStr = str;
		return this;
	}

	public LTRect setUseSimpleScale(bool useSimpleScale, Rect relativeRect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		this.useSimpleScale = useSimpleScale;
		this.relativeRect = relativeRect;
		return this;
	}

	public LTRect setUseSimpleScale(bool useSimpleScale)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		this.useSimpleScale = useSimpleScale;
		relativeRect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		return this;
	}

	public LTRect setSizeByHeight(bool sizeByHeight)
	{
		this.sizeByHeight = sizeByHeight;
		return this;
	}

	public override string ToString()
	{
		return "x:" + ((Rect)(ref _rect)).x + " y:" + ((Rect)(ref _rect)).y + " width:" + ((Rect)(ref _rect)).width + " height:" + ((Rect)(ref _rect)).height;
	}
}
