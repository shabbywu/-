using System;
using UnityEngine;
using UnityEngine.UI;

public class LTDescr
{
	public delegate Vector3 EaseTypeDelegate();

	public delegate void ActionMethodDelegate();

	public bool toggle;

	public bool useEstimatedTime;

	public bool useFrames;

	public bool useManualTime;

	public bool usesNormalDt;

	public bool hasInitiliazed;

	public bool hasExtraOnCompletes;

	public bool hasPhysics;

	public bool onCompleteOnRepeat;

	public bool onCompleteOnStart;

	public bool useRecursion;

	public float ratioPassed;

	public float passed;

	public float delay;

	public float time;

	public float speed;

	public float lastVal;

	private uint _id;

	public int loopCount;

	public uint counter = uint.MaxValue;

	public float direction;

	public float directionLast;

	public float overshoot;

	public float period;

	public float scale;

	public bool destroyOnComplete;

	public Transform trans;

	internal Vector3 fromInternal;

	internal Vector3 toInternal;

	internal Vector3 diff;

	internal Vector3 diffDiv2;

	public TweenAction type;

	private LeanTweenType easeType;

	public LeanTweenType loopType;

	public bool hasUpdateCallback;

	public EaseTypeDelegate easeMethod;

	public SpriteRenderer spriteRen;

	public RectTransform rectTransform;

	public Text uiText;

	public Image uiImage;

	public RawImage rawImage;

	public Sprite[] sprites;

	public LTDescrOptional _optional = new LTDescrOptional();

	public static float val;

	public static float dt;

	public static Vector3 newVect;

	public Vector3 from
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return fromInternal;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			fromInternal = value;
		}
	}

	public Vector3 to
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return toInternal;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			toInternal = value;
		}
	}

	public ActionMethodDelegate easeInternal { get; set; }

	public ActionMethodDelegate initInternal { get; set; }

	public int uniqueId => (int)(_id | (counter << 16));

	public int id => uniqueId;

	public LTDescrOptional optional
	{
		get
		{
			return _optional;
		}
		set
		{
			_optional = optional;
		}
	}

	public override string ToString()
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		return string.Concat(((Object)(object)trans != (Object)null) ? ("name:" + ((Object)((Component)trans).gameObject).name) : "gameObject:null", " toggle:", toggle.ToString(), " passed:", passed, " time:", time, " delay:", delay, " direction:", direction, " from:", from, " to:", to, " diff:", diff, " type:", type, " ease:", easeType, " useEstimatedTime:", useEstimatedTime.ToString(), " id:", id, " hasInitiliazed:", hasInitiliazed.ToString());
	}

	[Obsolete("Use 'LeanTween.cancel( id )' instead")]
	public LTDescr cancel(GameObject gameObject)
	{
		if ((Object)(object)gameObject == (Object)(object)((Component)trans).gameObject)
		{
			LeanTween.removeTween((int)_id, uniqueId);
		}
		return this;
	}

	public void reset()
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		toggle = (useRecursion = (usesNormalDt = true));
		trans = null;
		spriteRen = null;
		passed = (delay = (lastVal = 0f));
		hasUpdateCallback = (useEstimatedTime = (useFrames = (hasInitiliazed = (onCompleteOnRepeat = (destroyOnComplete = (onCompleteOnStart = (useManualTime = (hasExtraOnCompletes = false))))))));
		easeType = LeanTweenType.linear;
		loopType = LeanTweenType.once;
		loopCount = 0;
		direction = (directionLast = (overshoot = (scale = 1f)));
		period = 0.3f;
		speed = -1f;
		easeMethod = easeLinear;
		Vector3 val = (to = Vector3.zero);
		from = val;
		_optional.reset();
	}

	public LTDescr setMoveX()
	{
		type = TweenAction.MOVE_X;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.position.x;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.position = new Vector3(easeMethod().x, trans.position.y, trans.position.z);
		};
		return this;
	}

	public LTDescr setMoveY()
	{
		type = TweenAction.MOVE_Y;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.position.y;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.position = new Vector3(trans.position.x, easeMethod().x, trans.position.z);
		};
		return this;
	}

	public LTDescr setMoveZ()
	{
		type = TweenAction.MOVE_Z;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.position.z;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.position = new Vector3(trans.position.x, trans.position.y, easeMethod().x);
		};
		return this;
	}

	public LTDescr setMoveLocalX()
	{
		type = TweenAction.MOVE_LOCAL_X;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.localPosition.x;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.localPosition = new Vector3(easeMethod().x, trans.localPosition.y, trans.localPosition.z);
		};
		return this;
	}

	public LTDescr setMoveLocalY()
	{
		type = TweenAction.MOVE_LOCAL_Y;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.localPosition.y;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.localPosition = new Vector3(trans.localPosition.x, easeMethod().x, trans.localPosition.z);
		};
		return this;
	}

	public LTDescr setMoveLocalZ()
	{
		type = TweenAction.MOVE_LOCAL_Z;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.localPosition.z;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y, easeMethod().x);
		};
		return this;
	}

	private void initFromInternal()
	{
		fromInternal.x = 0f;
	}

	public LTDescr setMoveCurved()
	{
		type = TweenAction.MOVE_CURVED;
		initInternal = initFromInternal;
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			val = newVect.x;
			if (_optional.path.orientToPath)
			{
				if (_optional.path.orientToPath2d)
				{
					_optional.path.place2d(trans, val);
				}
				else
				{
					_optional.path.place(trans, val);
				}
			}
			else
			{
				trans.position = _optional.path.point(val);
			}
		};
		return this;
	}

	public LTDescr setMoveCurvedLocal()
	{
		type = TweenAction.MOVE_CURVED_LOCAL;
		initInternal = initFromInternal;
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			val = newVect.x;
			if (_optional.path.orientToPath)
			{
				if (_optional.path.orientToPath2d)
				{
					_optional.path.placeLocal2d(trans, val);
				}
				else
				{
					_optional.path.placeLocal(trans, val);
				}
			}
			else
			{
				trans.localPosition = _optional.path.point(val);
			}
		};
		return this;
	}

	public LTDescr setMoveSpline()
	{
		type = TweenAction.MOVE_SPLINE;
		initInternal = initFromInternal;
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			val = newVect.x;
			if (_optional.spline.orientToPath)
			{
				if (_optional.spline.orientToPath2d)
				{
					_optional.spline.place2d(trans, val);
				}
				else
				{
					_optional.spline.place(trans, val);
				}
			}
			else
			{
				trans.position = _optional.spline.point(val);
			}
		};
		return this;
	}

	public LTDescr setMoveSplineLocal()
	{
		type = TweenAction.MOVE_SPLINE_LOCAL;
		initInternal = initFromInternal;
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			val = newVect.x;
			if (_optional.spline.orientToPath)
			{
				if (_optional.spline.orientToPath2d)
				{
					_optional.spline.placeLocal2d(trans, val);
				}
				else
				{
					_optional.spline.placeLocal(trans, val);
				}
			}
			else
			{
				trans.localPosition = _optional.spline.point(val);
			}
		};
		return this;
	}

	public LTDescr setScaleX()
	{
		type = TweenAction.SCALE_X;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.localScale.x;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.localScale = new Vector3(easeMethod().x, trans.localScale.y, trans.localScale.z);
		};
		return this;
	}

	public LTDescr setScaleY()
	{
		type = TweenAction.SCALE_Y;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.localScale.y;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.localScale = new Vector3(trans.localScale.x, easeMethod().x, trans.localScale.z);
		};
		return this;
	}

	public LTDescr setScaleZ()
	{
		type = TweenAction.SCALE_Z;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.localScale.z;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.localScale = new Vector3(trans.localScale.x, trans.localScale.y, easeMethod().x);
		};
		return this;
	}

	public LTDescr setRotateX()
	{
		type = TweenAction.ROTATE_X;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.eulerAngles.x;
			toInternal.x = LeanTween.closestRot(fromInternal.x, toInternal.x);
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.eulerAngles = new Vector3(easeMethod().x, trans.eulerAngles.y, trans.eulerAngles.z);
		};
		return this;
	}

	public LTDescr setRotateY()
	{
		type = TweenAction.ROTATE_Y;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.eulerAngles.y;
			toInternal.x = LeanTween.closestRot(fromInternal.x, toInternal.x);
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.eulerAngles = new Vector3(trans.eulerAngles.x, easeMethod().x, trans.eulerAngles.z);
		};
		return this;
	}

	public LTDescr setRotateZ()
	{
		type = TweenAction.ROTATE_Z;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = trans.eulerAngles.z;
			toInternal.x = LeanTween.closestRot(fromInternal.x, toInternal.x);
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			trans.eulerAngles = new Vector3(trans.eulerAngles.x, trans.eulerAngles.y, easeMethod().x);
		};
		return this;
	}

	public LTDescr setRotateAround()
	{
		type = TweenAction.ROTATE_AROUND;
		initInternal = delegate
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = 0f;
			_optional.origRotation = trans.rotation;
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			LTDescr.val = newVect.x;
			Vector3 localPosition = trans.localPosition;
			Vector3 val = trans.TransformPoint(_optional.point);
			trans.RotateAround(val, _optional.axis, 0f - _optional.lastVal);
			Vector3 val2 = localPosition - trans.localPosition;
			trans.localPosition = localPosition - val2;
			trans.rotation = _optional.origRotation;
			val = trans.TransformPoint(_optional.point);
			trans.RotateAround(val, _optional.axis, LTDescr.val);
			_optional.lastVal = LTDescr.val;
		};
		return this;
	}

	public LTDescr setRotateAroundLocal()
	{
		type = TweenAction.ROTATE_AROUND_LOCAL;
		initInternal = delegate
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = 0f;
			_optional.origRotation = trans.localRotation;
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			LTDescr.val = newVect.x;
			Vector3 localPosition = trans.localPosition;
			trans.RotateAround(trans.TransformPoint(_optional.point), trans.TransformDirection(_optional.axis), 0f - _optional.lastVal);
			Vector3 val = localPosition - trans.localPosition;
			trans.localPosition = localPosition - val;
			trans.localRotation = _optional.origRotation;
			Vector3 val2 = trans.TransformPoint(_optional.point);
			trans.RotateAround(val2, trans.TransformDirection(_optional.axis), LTDescr.val);
			_optional.lastVal = LTDescr.val;
		};
		return this;
	}

	public LTDescr setAlpha()
	{
		type = TweenAction.ALPHA;
		initInternal = delegate
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			SpriteRenderer component = ((Component)trans).GetComponent<SpriteRenderer>();
			if ((Object)(object)component != (Object)null)
			{
				fromInternal.x = component.color.a;
			}
			else if ((Object)(object)((Component)trans).GetComponent<Renderer>() != (Object)null && ((Component)trans).GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				fromInternal.x = ((Component)trans).GetComponent<Renderer>().material.color.a;
			}
			else if ((Object)(object)((Component)trans).GetComponent<Renderer>() != (Object)null && ((Component)trans).GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				Color color = ((Component)trans).GetComponent<Renderer>().material.GetColor("_TintColor");
				fromInternal.x = color.a;
			}
			else if (trans.childCount > 0)
			{
				foreach (Transform tran in trans)
				{
					Transform val = tran;
					if ((Object)(object)((Component)val).gameObject.GetComponent<Renderer>() != (Object)null)
					{
						Color color2 = ((Component)val).gameObject.GetComponent<Renderer>().material.color;
						fromInternal.x = color2.a;
						break;
					}
				}
			}
			easeInternal = delegate
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				LTDescr.val = easeMethod().x;
				if ((Object)(object)spriteRen != (Object)null)
				{
					spriteRen.color = new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, LTDescr.val);
					alphaRecursiveSprite(trans, LTDescr.val);
				}
				else
				{
					alphaRecursive(trans, LTDescr.val, useRecursion);
				}
			};
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			val = newVect.x;
			if ((Object)(object)spriteRen != (Object)null)
			{
				spriteRen.color = new Color(spriteRen.color.r, spriteRen.color.g, spriteRen.color.b, val);
				alphaRecursiveSprite(trans, val);
			}
			else
			{
				alphaRecursive(trans, val, useRecursion);
			}
		};
		return this;
	}

	public LTDescr setTextAlpha()
	{
		type = TweenAction.TEXT_ALPHA;
		initInternal = delegate
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			uiText = ((Component)trans).GetComponent<Text>();
			fromInternal.x = (((Object)(object)uiText != (Object)null) ? ((Graphic)uiText).color.a : 1f);
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			textAlphaRecursive(trans, easeMethod().x, useRecursion);
		};
		return this;
	}

	public LTDescr setAlphaVertex()
	{
		type = TweenAction.ALPHA_VERTEX;
		initInternal = delegate
		{
			fromInternal.x = (int)((Component)trans).GetComponent<MeshFilter>().mesh.colors32[0].a;
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			LTDescr.val = newVect.x;
			Mesh mesh = ((Component)trans).GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			Color32[] array = (Color32[])(object)new Color32[vertices.Length];
			if (array.Length == 0)
			{
				Color32 val = default(Color32);
				((Color32)(ref val))._002Ector(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)0);
				array = (Color32[])(object)new Color32[mesh.vertices.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = val;
				}
				mesh.colors32 = array;
			}
			Color32 val2 = mesh.colors32[0];
			val2 = Color32.op_Implicit(new Color((float)(int)val2.r, (float)(int)val2.g, (float)(int)val2.b, LTDescr.val));
			for (int j = 0; j < vertices.Length; j++)
			{
				array[j] = val2;
			}
			mesh.colors32 = array;
		};
		return this;
	}

	public LTDescr setColor()
	{
		type = TweenAction.COLOR;
		initInternal = delegate
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			SpriteRenderer component = ((Component)trans).GetComponent<SpriteRenderer>();
			if ((Object)(object)component != (Object)null)
			{
				setFromColor(component.color);
			}
			else if ((Object)(object)((Component)trans).GetComponent<Renderer>() != (Object)null && ((Component)trans).GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				Color color = ((Component)trans).GetComponent<Renderer>().material.color;
				setFromColor(color);
			}
			else if ((Object)(object)((Component)trans).GetComponent<Renderer>() != (Object)null && ((Component)trans).GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				Color color2 = ((Component)trans).GetComponent<Renderer>().material.GetColor("_TintColor");
				setFromColor(color2);
			}
			else if (trans.childCount > 0)
			{
				foreach (Transform tran in trans)
				{
					Transform val2 = tran;
					if ((Object)(object)((Component)val2).gameObject.GetComponent<Renderer>() != (Object)null)
					{
						Color color3 = ((Component)val2).gameObject.GetComponent<Renderer>().material.color;
						setFromColor(color3);
						break;
					}
				}
			}
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			LTDescr.val = newVect.x;
			Color val = tweenColor(this, LTDescr.val);
			if ((Object)(object)spriteRen != (Object)null)
			{
				spriteRen.color = val;
				colorRecursiveSprite(trans, val);
			}
			else if (type == TweenAction.COLOR)
			{
				colorRecursive(trans, val, useRecursion);
			}
			if (dt != 0f && _optional.onUpdateColor != null)
			{
				_optional.onUpdateColor(val);
			}
			else if (dt != 0f && _optional.onUpdateColorObject != null)
			{
				_optional.onUpdateColorObject(val, _optional.onUpdateParam);
			}
		};
		return this;
	}

	public LTDescr setCallbackColor()
	{
		type = TweenAction.CALLBACK_COLOR;
		initInternal = delegate
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			diff = new Vector3(1f, 0f, 0f);
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			LTDescr.val = newVect.x;
			Color val = tweenColor(this, LTDescr.val);
			if ((Object)(object)spriteRen != (Object)null)
			{
				spriteRen.color = val;
				colorRecursiveSprite(trans, val);
			}
			else if (type == TweenAction.COLOR)
			{
				colorRecursive(trans, val, useRecursion);
			}
			if (dt != 0f && _optional.onUpdateColor != null)
			{
				_optional.onUpdateColor(val);
			}
			else if (dt != 0f && _optional.onUpdateColorObject != null)
			{
				_optional.onUpdateColorObject(val, _optional.onUpdateParam);
			}
		};
		return this;
	}

	public LTDescr setTextColor()
	{
		type = TweenAction.TEXT_COLOR;
		initInternal = delegate
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			uiText = ((Component)trans).GetComponent<Text>();
			setFromColor(((Object)(object)uiText != (Object)null) ? ((Graphic)uiText).color : Color.white);
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			LTDescr.val = newVect.x;
			Color val = tweenColor(this, LTDescr.val);
			((Graphic)uiText).color = val;
			if (dt != 0f && _optional.onUpdateColor != null)
			{
				_optional.onUpdateColor(val);
			}
			if (useRecursion && trans.childCount > 0)
			{
				textColorRecursive(trans, val);
			}
		};
		return this;
	}

	public LTDescr setCanvasAlpha()
	{
		type = TweenAction.CANVAS_ALPHA;
		initInternal = delegate
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			uiImage = ((Component)trans).GetComponent<Image>();
			if ((Object)(object)uiImage != (Object)null)
			{
				fromInternal.x = ((Graphic)uiImage).color.a;
			}
			else
			{
				rawImage = ((Component)trans).GetComponent<RawImage>();
				if ((Object)(object)rawImage != (Object)null)
				{
					fromInternal.x = ((Graphic)rawImage).color.a;
				}
				else
				{
					fromInternal.x = 1f;
				}
			}
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			val = newVect.x;
			if ((Object)(object)uiImage != (Object)null)
			{
				Color color = ((Graphic)uiImage).color;
				color.a = val;
				((Graphic)uiImage).color = color;
			}
			else if ((Object)(object)rawImage != (Object)null)
			{
				Color color2 = ((Graphic)rawImage).color;
				color2.a = val;
				((Graphic)rawImage).color = color2;
			}
			if (useRecursion)
			{
				alphaRecursive(rectTransform, val);
				textAlphaChildrenRecursive((Transform)(object)rectTransform, val);
			}
		};
		return this;
	}

	public LTDescr setCanvasGroupAlpha()
	{
		type = TweenAction.CANVASGROUP_ALPHA;
		initInternal = delegate
		{
			fromInternal.x = ((Component)trans).GetComponent<CanvasGroup>().alpha;
		};
		easeInternal = delegate
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			((Component)trans).GetComponent<CanvasGroup>().alpha = easeMethod().x;
		};
		return this;
	}

	public LTDescr setCanvasColor()
	{
		type = TweenAction.CANVAS_COLOR;
		initInternal = delegate
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			uiImage = ((Component)trans).GetComponent<Image>();
			if ((Object)(object)uiImage == (Object)null)
			{
				rawImage = ((Component)trans).GetComponent<RawImage>();
				setFromColor(((Object)(object)rawImage != (Object)null) ? ((Graphic)rawImage).color : Color.white);
			}
			else
			{
				setFromColor(((Graphic)uiImage).color);
			}
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			LTDescr.val = newVect.x;
			Color val = tweenColor(this, LTDescr.val);
			if ((Object)(object)uiImage != (Object)null)
			{
				((Graphic)uiImage).color = val;
			}
			else if ((Object)(object)rawImage != (Object)null)
			{
				((Graphic)rawImage).color = val;
			}
			if (dt != 0f && _optional.onUpdateColor != null)
			{
				_optional.onUpdateColor(val);
			}
			if (useRecursion)
			{
				colorRecursive(rectTransform, val);
			}
		};
		return this;
	}

	public LTDescr setCanvasMoveX()
	{
		type = TweenAction.CANVAS_MOVE_X;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = rectTransform.anchoredPosition3D.x;
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Vector3 anchoredPosition3D = rectTransform.anchoredPosition3D;
			rectTransform.anchoredPosition3D = new Vector3(easeMethod().x, anchoredPosition3D.y, anchoredPosition3D.z);
		};
		return this;
	}

	public LTDescr setCanvasMoveY()
	{
		type = TweenAction.CANVAS_MOVE_Y;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = rectTransform.anchoredPosition3D.y;
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Vector3 anchoredPosition3D = rectTransform.anchoredPosition3D;
			rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D.x, easeMethod().x, anchoredPosition3D.z);
		};
		return this;
	}

	public LTDescr setCanvasMoveZ()
	{
		type = TweenAction.CANVAS_MOVE_Z;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal.x = rectTransform.anchoredPosition3D.z;
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Vector3 anchoredPosition3D = rectTransform.anchoredPosition3D;
			rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D.x, anchoredPosition3D.y, easeMethod().x);
		};
		return this;
	}

	private void initCanvasRotateAround()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		lastVal = 0f;
		fromInternal.x = 0f;
		_optional.origRotation = ((Transform)rectTransform).rotation;
	}

	public LTDescr setCanvasRotateAround()
	{
		type = TweenAction.CANVAS_ROTATEAROUND;
		initInternal = initCanvasRotateAround;
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			LTDescr.val = newVect.x;
			RectTransform val = rectTransform;
			Vector3 localPosition = ((Transform)val).localPosition;
			((Transform)val).RotateAround(((Transform)val).TransformPoint(_optional.point), _optional.axis, 0f - LTDescr.val);
			Vector3 val2 = localPosition - ((Transform)val).localPosition;
			((Transform)val).localPosition = localPosition - val2;
			((Transform)val).rotation = _optional.origRotation;
			((Transform)val).RotateAround(((Transform)val).TransformPoint(_optional.point), _optional.axis, LTDescr.val);
		};
		return this;
	}

	public LTDescr setCanvasRotateAroundLocal()
	{
		type = TweenAction.CANVAS_ROTATEAROUND_LOCAL;
		initInternal = initCanvasRotateAround;
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			LTDescr.val = newVect.x;
			RectTransform val = rectTransform;
			Vector3 localPosition = ((Transform)val).localPosition;
			((Transform)val).RotateAround(((Transform)val).TransformPoint(_optional.point), ((Transform)val).TransformDirection(_optional.axis), 0f - LTDescr.val);
			Vector3 val2 = localPosition - ((Transform)val).localPosition;
			((Transform)val).localPosition = localPosition - val2;
			((Transform)val).rotation = _optional.origRotation;
			((Transform)val).RotateAround(((Transform)val).TransformPoint(_optional.point), ((Transform)val).TransformDirection(_optional.axis), LTDescr.val);
		};
		return this;
	}

	public LTDescr setCanvasPlaySprite()
	{
		type = TweenAction.CANVAS_PLAYSPRITE;
		initInternal = delegate
		{
			uiImage = ((Component)trans).GetComponent<Image>();
			fromInternal.x = 0f;
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			val = newVect.x;
			int num = (int)Mathf.Round(val);
			uiImage.sprite = sprites[num];
		};
		return this;
	}

	public LTDescr setCanvasMove()
	{
		type = TweenAction.CANVAS_MOVE;
		initInternal = delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			fromInternal = rectTransform.anchoredPosition3D;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			rectTransform.anchoredPosition3D = easeMethod();
		};
		return this;
	}

	public LTDescr setCanvasScale()
	{
		type = TweenAction.CANVAS_SCALE;
		initInternal = delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			from = ((Transform)rectTransform).localScale;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			((Transform)rectTransform).localScale = easeMethod();
		};
		return this;
	}

	public LTDescr setCanvasSizeDelta()
	{
		type = TweenAction.CANVAS_SIZEDELTA;
		initInternal = delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			from = Vector2.op_Implicit(rectTransform.sizeDelta);
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			rectTransform.sizeDelta = Vector2.op_Implicit(easeMethod());
		};
		return this;
	}

	private void callback()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		newVect = easeMethod();
		val = newVect.x;
	}

	public LTDescr setCallback()
	{
		type = TweenAction.CALLBACK;
		initInternal = delegate
		{
		};
		easeInternal = callback;
		return this;
	}

	public LTDescr setValue3()
	{
		type = TweenAction.VALUE3;
		initInternal = delegate
		{
		};
		easeInternal = callback;
		return this;
	}

	public LTDescr setMove()
	{
		type = TweenAction.MOVE;
		initInternal = delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			from = trans.position;
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			trans.position = newVect;
		};
		return this;
	}

	public LTDescr setMoveLocal()
	{
		type = TweenAction.MOVE_LOCAL;
		initInternal = delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			from = trans.localPosition;
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			trans.localPosition = newVect;
		};
		return this;
	}

	public LTDescr setMoveToTransform()
	{
		type = TweenAction.MOVE_TO_TRANSFORM;
		initInternal = delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			from = trans.position;
		};
		easeInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			to = _optional.toTrans.position;
			diff = to - from;
			diffDiv2 = diff * 0.5f;
			newVect = easeMethod();
			trans.position = newVect;
		};
		return this;
	}

	public LTDescr setRotate()
	{
		type = TweenAction.ROTATE;
		initInternal = delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			from = trans.eulerAngles;
			to = new Vector3(LeanTween.closestRot(fromInternal.x, toInternal.x), LeanTween.closestRot(from.y, to.y), LeanTween.closestRot(from.z, to.z));
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			trans.eulerAngles = newVect;
		};
		return this;
	}

	public LTDescr setRotateLocal()
	{
		type = TweenAction.ROTATE_LOCAL;
		initInternal = delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			from = trans.localEulerAngles;
			to = new Vector3(LeanTween.closestRot(fromInternal.x, toInternal.x), LeanTween.closestRot(from.y, to.y), LeanTween.closestRot(from.z, to.z));
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			trans.localEulerAngles = newVect;
		};
		return this;
	}

	public LTDescr setScale()
	{
		type = TweenAction.SCALE;
		initInternal = delegate
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			from = trans.localScale;
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			newVect = easeMethod();
			trans.localScale = newVect;
		};
		return this;
	}

	public LTDescr setGUIMove()
	{
		type = TweenAction.GUI_MOVE;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Rect rect2 = _optional.ltRect.rect;
			float x2 = ((Rect)(ref rect2)).x;
			rect2 = _optional.ltRect.rect;
			from = new Vector3(x2, ((Rect)(ref rect2)).y, 0f);
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = easeMethod();
			LTRect ltRect = _optional.ltRect;
			float x = val.x;
			float y = val.y;
			Rect rect = _optional.ltRect.rect;
			float width = ((Rect)(ref rect)).width;
			rect = _optional.ltRect.rect;
			ltRect.rect = new Rect(x, y, width, ((Rect)(ref rect)).height);
		};
		return this;
	}

	public LTDescr setGUIMoveMargin()
	{
		type = TweenAction.GUI_MOVE_MARGIN;
		initInternal = delegate
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			from = Vector2.op_Implicit(new Vector2(_optional.ltRect.margin.x, _optional.ltRect.margin.y));
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = easeMethod();
			_optional.ltRect.margin = new Vector2(val.x, val.y);
		};
		return this;
	}

	public LTDescr setGUIScale()
	{
		type = TweenAction.GUI_SCALE;
		initInternal = delegate
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			Rect rect2 = _optional.ltRect.rect;
			float width = ((Rect)(ref rect2)).width;
			rect2 = _optional.ltRect.rect;
			from = new Vector3(width, ((Rect)(ref rect2)).height, 0f);
		};
		easeInternal = delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = easeMethod();
			LTRect ltRect = _optional.ltRect;
			Rect rect = _optional.ltRect.rect;
			float x = ((Rect)(ref rect)).x;
			rect = _optional.ltRect.rect;
			ltRect.rect = new Rect(x, ((Rect)(ref rect)).y, val.x, val.y);
		};
		return this;
	}

	public LTDescr setGUIAlpha()
	{
		type = TweenAction.GUI_ALPHA;
		initInternal = delegate
		{
			fromInternal.x = _optional.ltRect.alpha;
		};
		easeInternal = delegate
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			_optional.ltRect.alpha = easeMethod().x;
		};
		return this;
	}

	public LTDescr setGUIRotate()
	{
		type = TweenAction.GUI_ROTATE;
		initInternal = delegate
		{
			if (!_optional.ltRect.rotateEnabled)
			{
				_optional.ltRect.rotateEnabled = true;
				_optional.ltRect.resetForRotation();
			}
			fromInternal.x = _optional.ltRect.rotation;
		};
		easeInternal = delegate
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			_optional.ltRect.rotation = easeMethod().x;
		};
		return this;
	}

	public LTDescr setDelayedSound()
	{
		type = TweenAction.DELAYED_SOUND;
		initInternal = delegate
		{
			hasExtraOnCompletes = true;
		};
		easeInternal = callback;
		return this;
	}

	private void init()
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		hasInitiliazed = true;
		usesNormalDt = !useEstimatedTime && !useManualTime && !useFrames;
		if (useFrames)
		{
			optional.initFrameCount = Time.frameCount;
		}
		if (time <= 0f)
		{
			time = Mathf.Epsilon;
		}
		initInternal();
		diff = to - from;
		diffDiv2 = diff * 0.5f;
		if (_optional.onStart != null)
		{
			_optional.onStart();
		}
		if (onCompleteOnStart)
		{
			callOnCompletes();
		}
		if (speed >= 0f)
		{
			initSpeed();
		}
	}

	private void initSpeed()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (type == TweenAction.MOVE_CURVED || type == TweenAction.MOVE_CURVED_LOCAL)
		{
			time = _optional.path.distance / speed;
			return;
		}
		if (type == TweenAction.MOVE_SPLINE || type == TweenAction.MOVE_SPLINE_LOCAL)
		{
			time = _optional.spline.distance / speed;
			return;
		}
		Vector3 val = to - from;
		time = ((Vector3)(ref val)).magnitude / speed;
	}

	public LTDescr updateNow()
	{
		updateInternal();
		return this;
	}

	public bool updateInternal()
	{
		float num = direction;
		if (usesNormalDt)
		{
			dt = LeanTween.dtActual;
		}
		else if (useEstimatedTime)
		{
			dt = LeanTween.dtEstimated;
		}
		else if (useFrames)
		{
			dt = ((optional.initFrameCount != 0) ? 1 : 0);
			optional.initFrameCount = Time.frameCount;
		}
		else if (useManualTime)
		{
			dt = LeanTween.dtManual;
		}
		if (delay <= 0f && num != 0f)
		{
			if ((Object)(object)trans == (Object)null)
			{
				return true;
			}
			if (!hasInitiliazed)
			{
				init();
			}
			dt *= num;
			passed += dt;
			passed = Mathf.Clamp(passed, 0f, time);
			ratioPassed = passed / time;
			easeInternal();
			if (hasUpdateCallback)
			{
				_optional.callOnUpdate(val, ratioPassed);
			}
			if ((num > 0f) ? (passed >= time) : (passed <= 0f))
			{
				loopCount--;
				if (loopType == LeanTweenType.pingPong)
				{
					direction = 0f - num;
				}
				else
				{
					passed = Mathf.Epsilon;
				}
				int num2;
				if (loopCount != 0)
				{
					num2 = ((loopType == LeanTweenType.once) ? 1 : 0);
					if (num2 == 0 && onCompleteOnRepeat && hasExtraOnCompletes)
					{
						callOnCompletes();
					}
				}
				else
				{
					num2 = 1;
				}
				return (byte)num2 != 0;
			}
		}
		else
		{
			delay -= dt;
		}
		return false;
	}

	public void callOnCompletes()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		if (type == TweenAction.GUI_ROTATE)
		{
			_optional.ltRect.rotateFinished = true;
		}
		if (type == TweenAction.DELAYED_SOUND)
		{
			AudioSource.PlayClipAtPoint((AudioClip)_optional.onCompleteParam, to, from.x);
		}
		if (_optional.onComplete != null)
		{
			_optional.onComplete();
		}
		else if (_optional.onCompleteObject != null)
		{
			_optional.onCompleteObject(_optional.onCompleteParam);
		}
	}

	public LTDescr setFromColor(Color col)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		from = new Vector3(0f, col.a, 0f);
		diff = new Vector3(1f, 0f, 0f);
		_optional.axis = new Vector3(col.r, col.g, col.b);
		return this;
	}

	private static void alphaRecursive(Transform transform, float val, bool useRecursion = true)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		Renderer component = ((Component)transform).gameObject.GetComponent<Renderer>();
		if ((Object)(object)component != (Object)null)
		{
			Material[] materials = component.materials;
			foreach (Material val2 in materials)
			{
				if (val2.HasProperty("_Color"))
				{
					val2.color = new Color(val2.color.r, val2.color.g, val2.color.b, val);
				}
				else if (val2.HasProperty("_TintColor"))
				{
					Color color = val2.GetColor("_TintColor");
					val2.SetColor("_TintColor", new Color(color.r, color.g, color.b, val));
				}
			}
		}
		if (!useRecursion || transform.childCount <= 0)
		{
			return;
		}
		foreach (Transform item in transform)
		{
			alphaRecursive(item, val, useRecursion: true);
		}
	}

	private static void colorRecursive(Transform transform, Color toColor, bool useRecursion = true)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		Renderer component = ((Component)transform).gameObject.GetComponent<Renderer>();
		if ((Object)(object)component != (Object)null)
		{
			Material[] materials = component.materials;
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].color = toColor;
			}
		}
		if (!useRecursion || transform.childCount <= 0)
		{
			return;
		}
		foreach (Transform item in transform)
		{
			colorRecursive(item, toColor, useRecursion: true);
		}
	}

	private static void alphaRecursive(RectTransform rectTransform, float val, int recursiveLevel = 0)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if (((Transform)rectTransform).childCount <= 0)
		{
			return;
		}
		foreach (RectTransform item in (Transform)rectTransform)
		{
			RectTransform val2 = item;
			MaskableGraphic component = (MaskableGraphic)(object)((Component)val2).GetComponent<Image>();
			if ((Object)(object)component != (Object)null)
			{
				Color color = ((Graphic)component).color;
				color.a = val;
				((Graphic)component).color = color;
			}
			else
			{
				component = (MaskableGraphic)(object)((Component)val2).GetComponent<RawImage>();
				if ((Object)(object)component != (Object)null)
				{
					Color color2 = ((Graphic)component).color;
					color2.a = val;
					((Graphic)component).color = color2;
				}
			}
			alphaRecursive(val2, val, recursiveLevel + 1);
		}
	}

	private static void alphaRecursiveSprite(Transform transform, float val)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (transform.childCount <= 0)
		{
			return;
		}
		foreach (Transform item in transform)
		{
			SpriteRenderer component = ((Component)item).GetComponent<SpriteRenderer>();
			if ((Object)(object)component != (Object)null)
			{
				component.color = new Color(component.color.r, component.color.g, component.color.b, val);
			}
			alphaRecursiveSprite(item, val);
		}
	}

	private static void colorRecursiveSprite(Transform transform, Color toColor)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (transform.childCount <= 0)
		{
			return;
		}
		foreach (Transform item in transform)
		{
			SpriteRenderer component = ((Component)transform).gameObject.GetComponent<SpriteRenderer>();
			if ((Object)(object)component != (Object)null)
			{
				component.color = toColor;
			}
			colorRecursiveSprite(item, toColor);
		}
	}

	private static void colorRecursive(RectTransform rectTransform, Color toColor)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		if (((Transform)rectTransform).childCount <= 0)
		{
			return;
		}
		foreach (RectTransform item in (Transform)rectTransform)
		{
			RectTransform val = item;
			MaskableGraphic component = (MaskableGraphic)(object)((Component)val).GetComponent<Image>();
			if ((Object)(object)component != (Object)null)
			{
				((Graphic)component).color = toColor;
			}
			else
			{
				component = (MaskableGraphic)(object)((Component)val).GetComponent<RawImage>();
				if ((Object)(object)component != (Object)null)
				{
					((Graphic)component).color = toColor;
				}
			}
			colorRecursive(val, toColor);
		}
	}

	private static void textAlphaChildrenRecursive(Transform trans, float val, bool useRecursion = true)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (!useRecursion || trans.childCount <= 0)
		{
			return;
		}
		foreach (Transform tran in trans)
		{
			Text component = ((Component)tran).GetComponent<Text>();
			if ((Object)(object)component != (Object)null)
			{
				Color color = ((Graphic)component).color;
				color.a = val;
				((Graphic)component).color = color;
			}
			textAlphaChildrenRecursive(tran, val);
		}
	}

	private static void textAlphaRecursive(Transform trans, float val, bool useRecursion = true)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		Text component = ((Component)trans).GetComponent<Text>();
		if ((Object)(object)component != (Object)null)
		{
			Color color = ((Graphic)component).color;
			color.a = val;
			((Graphic)component).color = color;
		}
		if (!useRecursion || trans.childCount <= 0)
		{
			return;
		}
		foreach (Transform tran in trans)
		{
			textAlphaRecursive(tran, val);
		}
	}

	private static void textColorRecursive(Transform trans, Color toColor)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (trans.childCount <= 0)
		{
			return;
		}
		foreach (Transform tran in trans)
		{
			Text component = ((Component)tran).GetComponent<Text>();
			if ((Object)(object)component != (Object)null)
			{
				((Graphic)component).color = toColor;
			}
			textColorRecursive(tran, toColor);
		}
	}

	private static Color tweenColor(LTDescr tween, float val)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val2 = tween._optional.point - tween._optional.axis;
		float num = tween.to.y - tween.from.y;
		return new Color(tween._optional.axis.x + val2.x * val, tween._optional.axis.y + val2.y * val, tween._optional.axis.z + val2.z * val, tween.from.y + num * val);
	}

	public LTDescr pause()
	{
		if (direction != 0f)
		{
			directionLast = direction;
			direction = 0f;
		}
		return this;
	}

	public LTDescr resume()
	{
		direction = directionLast;
		return this;
	}

	public LTDescr setAxis(Vector3 axis)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_optional.axis = axis;
		return this;
	}

	public LTDescr setDelay(float delay)
	{
		this.delay = delay;
		return this;
	}

	public LTDescr setEase(LeanTweenType easeType)
	{
		switch (easeType)
		{
		case LeanTweenType.linear:
			setEaseLinear();
			break;
		case LeanTweenType.easeOutQuad:
			setEaseOutQuad();
			break;
		case LeanTweenType.easeInQuad:
			setEaseInQuad();
			break;
		case LeanTweenType.easeInOutQuad:
			setEaseInOutQuad();
			break;
		case LeanTweenType.easeInCubic:
			setEaseInCubic();
			break;
		case LeanTweenType.easeOutCubic:
			setEaseOutCubic();
			break;
		case LeanTweenType.easeInOutCubic:
			setEaseInOutCubic();
			break;
		case LeanTweenType.easeInQuart:
			setEaseInQuart();
			break;
		case LeanTweenType.easeOutQuart:
			setEaseOutQuart();
			break;
		case LeanTweenType.easeInOutQuart:
			setEaseInOutQuart();
			break;
		case LeanTweenType.easeInQuint:
			setEaseInQuint();
			break;
		case LeanTweenType.easeOutQuint:
			setEaseOutQuint();
			break;
		case LeanTweenType.easeInOutQuint:
			setEaseInOutQuint();
			break;
		case LeanTweenType.easeInSine:
			setEaseInSine();
			break;
		case LeanTweenType.easeOutSine:
			setEaseOutSine();
			break;
		case LeanTweenType.easeInOutSine:
			setEaseInOutSine();
			break;
		case LeanTweenType.easeInExpo:
			setEaseInExpo();
			break;
		case LeanTweenType.easeOutExpo:
			setEaseOutExpo();
			break;
		case LeanTweenType.easeInOutExpo:
			setEaseInOutExpo();
			break;
		case LeanTweenType.easeInCirc:
			setEaseInCirc();
			break;
		case LeanTweenType.easeOutCirc:
			setEaseOutCirc();
			break;
		case LeanTweenType.easeInOutCirc:
			setEaseInOutCirc();
			break;
		case LeanTweenType.easeInBounce:
			setEaseInBounce();
			break;
		case LeanTweenType.easeOutBounce:
			setEaseOutBounce();
			break;
		case LeanTweenType.easeInOutBounce:
			setEaseInOutBounce();
			break;
		case LeanTweenType.easeInBack:
			setEaseInBack();
			break;
		case LeanTweenType.easeOutBack:
			setEaseOutBack();
			break;
		case LeanTweenType.easeInOutBack:
			setEaseInOutBack();
			break;
		case LeanTweenType.easeInElastic:
			setEaseInElastic();
			break;
		case LeanTweenType.easeOutElastic:
			setEaseOutElastic();
			break;
		case LeanTweenType.easeInOutElastic:
			setEaseInOutElastic();
			break;
		case LeanTweenType.punch:
			setEasePunch();
			break;
		case LeanTweenType.easeShake:
			setEaseShake();
			break;
		case LeanTweenType.easeSpring:
			setEaseSpring();
			break;
		default:
			setEaseLinear();
			break;
		}
		return this;
	}

	public LTDescr setEaseLinear()
	{
		easeType = LeanTweenType.linear;
		easeMethod = easeLinear;
		return this;
	}

	public LTDescr setEaseSpring()
	{
		easeType = LeanTweenType.easeSpring;
		easeMethod = easeSpring;
		return this;
	}

	public LTDescr setEaseInQuad()
	{
		easeType = LeanTweenType.easeInQuad;
		easeMethod = easeInQuad;
		return this;
	}

	public LTDescr setEaseOutQuad()
	{
		easeType = LeanTweenType.easeOutQuad;
		easeMethod = easeOutQuad;
		return this;
	}

	public LTDescr setEaseInOutQuad()
	{
		easeType = LeanTweenType.easeInOutQuad;
		easeMethod = easeInOutQuad;
		return this;
	}

	public LTDescr setEaseInCubic()
	{
		easeType = LeanTweenType.easeInCubic;
		easeMethod = easeInCubic;
		return this;
	}

	public LTDescr setEaseOutCubic()
	{
		easeType = LeanTweenType.easeOutCubic;
		easeMethod = easeOutCubic;
		return this;
	}

	public LTDescr setEaseInOutCubic()
	{
		easeType = LeanTweenType.easeInOutCubic;
		easeMethod = easeInOutCubic;
		return this;
	}

	public LTDescr setEaseInQuart()
	{
		easeType = LeanTweenType.easeInQuart;
		easeMethod = easeInQuart;
		return this;
	}

	public LTDescr setEaseOutQuart()
	{
		easeType = LeanTweenType.easeOutQuart;
		easeMethod = easeOutQuart;
		return this;
	}

	public LTDescr setEaseInOutQuart()
	{
		easeType = LeanTweenType.easeInOutQuart;
		easeMethod = easeInOutQuart;
		return this;
	}

	public LTDescr setEaseInQuint()
	{
		easeType = LeanTweenType.easeInQuint;
		easeMethod = easeInQuint;
		return this;
	}

	public LTDescr setEaseOutQuint()
	{
		easeType = LeanTweenType.easeOutQuint;
		easeMethod = easeOutQuint;
		return this;
	}

	public LTDescr setEaseInOutQuint()
	{
		easeType = LeanTweenType.easeInOutQuint;
		easeMethod = easeInOutQuint;
		return this;
	}

	public LTDescr setEaseInSine()
	{
		easeType = LeanTweenType.easeInSine;
		easeMethod = easeInSine;
		return this;
	}

	public LTDescr setEaseOutSine()
	{
		easeType = LeanTweenType.easeOutSine;
		easeMethod = easeOutSine;
		return this;
	}

	public LTDescr setEaseInOutSine()
	{
		easeType = LeanTweenType.easeInOutSine;
		easeMethod = easeInOutSine;
		return this;
	}

	public LTDescr setEaseInExpo()
	{
		easeType = LeanTweenType.easeInExpo;
		easeMethod = easeInExpo;
		return this;
	}

	public LTDescr setEaseOutExpo()
	{
		easeType = LeanTweenType.easeOutExpo;
		easeMethod = easeOutExpo;
		return this;
	}

	public LTDescr setEaseInOutExpo()
	{
		easeType = LeanTweenType.easeInOutExpo;
		easeMethod = easeInOutExpo;
		return this;
	}

	public LTDescr setEaseInCirc()
	{
		easeType = LeanTweenType.easeInCirc;
		easeMethod = easeInCirc;
		return this;
	}

	public LTDescr setEaseOutCirc()
	{
		easeType = LeanTweenType.easeOutCirc;
		easeMethod = easeOutCirc;
		return this;
	}

	public LTDescr setEaseInOutCirc()
	{
		easeType = LeanTweenType.easeInOutCirc;
		easeMethod = easeInOutCirc;
		return this;
	}

	public LTDescr setEaseInBounce()
	{
		easeType = LeanTweenType.easeInBounce;
		easeMethod = easeInBounce;
		return this;
	}

	public LTDescr setEaseOutBounce()
	{
		easeType = LeanTweenType.easeOutBounce;
		easeMethod = easeOutBounce;
		return this;
	}

	public LTDescr setEaseInOutBounce()
	{
		easeType = LeanTweenType.easeInOutBounce;
		easeMethod = easeInOutBounce;
		return this;
	}

	public LTDescr setEaseInBack()
	{
		easeType = LeanTweenType.easeInBack;
		easeMethod = easeInBack;
		return this;
	}

	public LTDescr setEaseOutBack()
	{
		easeType = LeanTweenType.easeOutBack;
		easeMethod = easeOutBack;
		return this;
	}

	public LTDescr setEaseInOutBack()
	{
		easeType = LeanTweenType.easeInOutBack;
		easeMethod = easeInOutBack;
		return this;
	}

	public LTDescr setEaseInElastic()
	{
		easeType = LeanTweenType.easeInElastic;
		easeMethod = easeInElastic;
		return this;
	}

	public LTDescr setEaseOutElastic()
	{
		easeType = LeanTweenType.easeOutElastic;
		easeMethod = easeOutElastic;
		return this;
	}

	public LTDescr setEaseInOutElastic()
	{
		easeType = LeanTweenType.easeInOutElastic;
		easeMethod = easeInOutElastic;
		return this;
	}

	public LTDescr setEasePunch()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		_optional.animationCurve = LeanTween.punch;
		toInternal.x = from.x + to.x;
		easeMethod = tweenOnCurve;
		return this;
	}

	public LTDescr setEaseShake()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		_optional.animationCurve = LeanTween.shake;
		toInternal.x = from.x + to.x;
		easeMethod = tweenOnCurve;
		return this;
	}

	private Vector3 tweenOnCurve()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(from.x + diff.x * _optional.animationCurve.Evaluate(ratioPassed), from.y + diff.y * _optional.animationCurve.Evaluate(ratioPassed), from.z + diff.z * _optional.animationCurve.Evaluate(ratioPassed));
	}

	private Vector3 easeInOutQuad()
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed * 2f;
		if (val < 1f)
		{
			val *= val;
			return new Vector3(diffDiv2.x * val + from.x, diffDiv2.y * val + from.y, diffDiv2.z * val + from.z);
		}
		val = (1f - val) * (val - 3f) + 1f;
		return new Vector3(diffDiv2.x * val + from.x, diffDiv2.y * val + from.y, diffDiv2.z * val + from.z);
	}

	private Vector3 easeInQuad()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed * ratioPassed;
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeOutQuad()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed;
		val = (0f - val) * (val - 2f);
		return diff * val + from;
	}

	private Vector3 easeLinear()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed;
		return new Vector3(from.x + diff.x * val, from.y + diff.y * val, from.z + diff.z * val);
	}

	private Vector3 easeSpring()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		val = Mathf.Clamp01(ratioPassed);
		val = (Mathf.Sin(val * (float)Math.PI * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + 1.2f * (1f - val));
		return from + diff * val;
	}

	private Vector3 easeInCubic()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed * ratioPassed * ratioPassed;
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeOutCubic()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed - 1f;
		val = val * val * val + 1f;
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeInOutCubic()
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed * 2f;
		if (val < 1f)
		{
			val = val * val * val;
			return new Vector3(diffDiv2.x * val + from.x, diffDiv2.y * val + from.y, diffDiv2.z * val + from.z);
		}
		val -= 2f;
		val = val * val * val + 2f;
		return new Vector3(diffDiv2.x * val + from.x, diffDiv2.y * val + from.y, diffDiv2.z * val + from.z);
	}

	private Vector3 easeInQuart()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed * ratioPassed * ratioPassed * ratioPassed;
		return diff * val + from;
	}

	private Vector3 easeOutQuart()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed - 1f;
		val = 0f - (val * val * val * val - 1f);
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeInOutQuart()
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed * 2f;
		if (val < 1f)
		{
			val = val * val * val * val;
			return new Vector3(diffDiv2.x * val + from.x, diffDiv2.y * val + from.y, diffDiv2.z * val + from.z);
		}
		val -= 2f;
		return -diffDiv2 * (val * val * val * val - 2f) + from;
	}

	private Vector3 easeInQuint()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed;
		val = val * val * val * val * val;
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeOutQuint()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed - 1f;
		val = val * val * val * val * val + 1f;
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeInOutQuint()
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed * 2f;
		if (val < 1f)
		{
			val = val * val * val * val * val;
			return new Vector3(diffDiv2.x * val + from.x, diffDiv2.y * val + from.y, diffDiv2.z * val + from.z);
		}
		val -= 2f;
		val = val * val * val * val * val + 2f;
		return new Vector3(diffDiv2.x * val + from.x, diffDiv2.y * val + from.y, diffDiv2.z * val + from.z);
	}

	private Vector3 easeInSine()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		val = 0f - Mathf.Cos(ratioPassed * LeanTween.PI_DIV2);
		return new Vector3(diff.x * val + diff.x + from.x, diff.y * val + diff.y + from.y, diff.z * val + diff.z + from.z);
	}

	private Vector3 easeOutSine()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		val = Mathf.Sin(ratioPassed * LeanTween.PI_DIV2);
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeInOutSine()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		val = 0f - (Mathf.Cos((float)Math.PI * ratioPassed) - 1f);
		return new Vector3(diffDiv2.x * val + from.x, diffDiv2.y * val + from.y, diffDiv2.z * val + from.z);
	}

	private Vector3 easeInExpo()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		val = Mathf.Pow(2f, 10f * (ratioPassed - 1f));
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeOutExpo()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		val = 0f - Mathf.Pow(2f, -10f * ratioPassed) + 1f;
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeInOutExpo()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed * 2f;
		if (val < 1f)
		{
			return diffDiv2 * Mathf.Pow(2f, 10f * (val - 1f)) + from;
		}
		val -= 1f;
		return diffDiv2 * (0f - Mathf.Pow(2f, -10f * val) + 2f) + from;
	}

	private Vector3 easeInCirc()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		val = 0f - (Mathf.Sqrt(1f - ratioPassed * ratioPassed) - 1f);
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeOutCirc()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed - 1f;
		val = Mathf.Sqrt(1f - val * val);
		return new Vector3(diff.x * val + from.x, diff.y * val + from.y, diff.z * val + from.z);
	}

	private Vector3 easeInOutCirc()
	{
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed * 2f;
		if (val < 1f)
		{
			val = 0f - (Mathf.Sqrt(1f - val * val) - 1f);
			return new Vector3(diffDiv2.x * val + from.x, diffDiv2.y * val + from.y, diffDiv2.z * val + from.z);
		}
		val -= 2f;
		val = Mathf.Sqrt(1f - val * val) + 1f;
		return new Vector3(diffDiv2.x * val + from.x, diffDiv2.y * val + from.y, diffDiv2.z * val + from.z);
	}

	private Vector3 easeInBounce()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed;
		val = 1f - val;
		return new Vector3(diff.x - LeanTween.easeOutBounce(0f, diff.x, val) + from.x, diff.y - LeanTween.easeOutBounce(0f, diff.y, val) + from.y, diff.z - LeanTween.easeOutBounce(0f, diff.z, val) + from.z);
	}

	private Vector3 easeOutBounce()
	{
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed;
		float num2;
		float num;
		if (val < (num = 1f - 1.75f * overshoot / 2.75f))
		{
			val = 1f / num / num * val * val;
		}
		else if (val < (num2 = 1f - 0.75f * overshoot / 2.75f))
		{
			val -= (num + num2) / 2f;
			val = 7.5625f * val * val + 1f - 0.25f * overshoot * overshoot;
		}
		else if (val < (num = 1f - 0.25f * overshoot / 2.75f))
		{
			val -= (num + num2) / 2f;
			val = 7.5625f * val * val + 1f - 0.0625f * overshoot * overshoot;
		}
		else
		{
			val -= (num + 1f) / 2f;
			val = 7.5625f * val * val + 1f - 1f / 64f * overshoot * overshoot;
		}
		return diff * val + from;
	}

	private Vector3 easeInOutBounce()
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed * 2f;
		if (val < 1f)
		{
			return new Vector3(LeanTween.easeInBounce(0f, diff.x, val) * 0.5f + from.x, LeanTween.easeInBounce(0f, diff.y, val) * 0.5f + from.y, LeanTween.easeInBounce(0f, diff.z, val) * 0.5f + from.z);
		}
		val -= 1f;
		return new Vector3(LeanTween.easeOutBounce(0f, diff.x, val) * 0.5f + diffDiv2.x + from.x, LeanTween.easeOutBounce(0f, diff.y, val) * 0.5f + diffDiv2.y + from.y, LeanTween.easeOutBounce(0f, diff.z, val) * 0.5f + diffDiv2.z + from.z);
	}

	private Vector3 easeInBack()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		val = ratioPassed;
		val /= 1f;
		float num = 1.70158f * overshoot;
		return diff * val * val * ((num + 1f) * val - num) + from;
	}

	private Vector3 easeOutBack()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		float num = 1.70158f * overshoot;
		val = ratioPassed / 1f - 1f;
		val = val * val * ((num + 1f) * val + num) + 1f;
		return diff * val + from;
	}

	private Vector3 easeInOutBack()
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		float num = 1.70158f * overshoot;
		val = ratioPassed * 2f;
		if (val < 1f)
		{
			num *= 1.525f * overshoot;
			return diffDiv2 * (val * val * ((num + 1f) * val - num)) + from;
		}
		val -= 2f;
		num *= 1.525f * overshoot;
		val = val * val * ((num + 1f) * val + num) + 2f;
		return diffDiv2 * val + from;
	}

	private Vector3 easeInElastic()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(LeanTween.easeInElastic(from.x, to.x, ratioPassed, overshoot, period), LeanTween.easeInElastic(from.y, to.y, ratioPassed, overshoot, period), LeanTween.easeInElastic(from.z, to.z, ratioPassed, overshoot, period));
	}

	private Vector3 easeOutElastic()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(LeanTween.easeOutElastic(from.x, to.x, ratioPassed, overshoot, period), LeanTween.easeOutElastic(from.y, to.y, ratioPassed, overshoot, period), LeanTween.easeOutElastic(from.z, to.z, ratioPassed, overshoot, period));
	}

	private Vector3 easeInOutElastic()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(LeanTween.easeInOutElastic(from.x, to.x, ratioPassed, overshoot, period), LeanTween.easeInOutElastic(from.y, to.y, ratioPassed, overshoot, period), LeanTween.easeInOutElastic(from.z, to.z, ratioPassed, overshoot, period));
	}

	public LTDescr setOvershoot(float overshoot)
	{
		this.overshoot = overshoot;
		return this;
	}

	public LTDescr setPeriod(float period)
	{
		this.period = period;
		return this;
	}

	public LTDescr setScale(float scale)
	{
		this.scale = scale;
		return this;
	}

	public LTDescr setEase(AnimationCurve easeCurve)
	{
		_optional.animationCurve = easeCurve;
		easeMethod = tweenOnCurve;
		easeType = LeanTweenType.animationCurve;
		return this;
	}

	public LTDescr setTo(Vector3 to)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (hasInitiliazed)
		{
			this.to = to;
			diff = to - from;
		}
		else
		{
			this.to = to;
		}
		return this;
	}

	public LTDescr setTo(Transform to)
	{
		_optional.toTrans = to;
		return this;
	}

	public LTDescr setFrom(Vector3 from)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)trans))
		{
			init();
		}
		this.from = from;
		diff = to - this.from;
		diffDiv2 = diff * 0.5f;
		return this;
	}

	public LTDescr setFrom(float from)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return setFrom(new Vector3(from, 0f, 0f));
	}

	public LTDescr setDiff(Vector3 diff)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		this.diff = diff;
		return this;
	}

	public LTDescr setHasInitialized(bool has)
	{
		hasInitiliazed = has;
		return this;
	}

	public LTDescr setId(uint id, uint global_counter)
	{
		_id = id;
		counter = global_counter;
		return this;
	}

	public LTDescr setPassed(float passed)
	{
		this.passed = passed;
		return this;
	}

	public LTDescr setTime(float time)
	{
		float num = passed / this.time;
		passed = time * num;
		this.time = time;
		return this;
	}

	public LTDescr setSpeed(float speed)
	{
		this.speed = speed;
		if (hasInitiliazed)
		{
			initSpeed();
		}
		return this;
	}

	public LTDescr setRepeat(int repeat)
	{
		loopCount = repeat;
		if ((repeat > 1 && loopType == LeanTweenType.once) || (repeat < 0 && loopType == LeanTweenType.once))
		{
			loopType = LeanTweenType.clamp;
		}
		if (type == TweenAction.CALLBACK || type == TweenAction.CALLBACK_COLOR)
		{
			setOnCompleteOnRepeat(isOn: true);
		}
		return this;
	}

	public LTDescr setLoopType(LeanTweenType loopType)
	{
		this.loopType = loopType;
		return this;
	}

	public LTDescr setUseEstimatedTime(bool useEstimatedTime)
	{
		this.useEstimatedTime = useEstimatedTime;
		usesNormalDt = false;
		return this;
	}

	public LTDescr setIgnoreTimeScale(bool useUnScaledTime)
	{
		useEstimatedTime = useUnScaledTime;
		usesNormalDt = false;
		return this;
	}

	public LTDescr setUseFrames(bool useFrames)
	{
		this.useFrames = useFrames;
		usesNormalDt = false;
		return this;
	}

	public LTDescr setUseManualTime(bool useManualTime)
	{
		this.useManualTime = useManualTime;
		usesNormalDt = false;
		return this;
	}

	public LTDescr setLoopCount(int loopCount)
	{
		loopType = LeanTweenType.clamp;
		this.loopCount = loopCount;
		return this;
	}

	public LTDescr setLoopOnce()
	{
		loopType = LeanTweenType.once;
		return this;
	}

	public LTDescr setLoopClamp()
	{
		loopType = LeanTweenType.clamp;
		if (loopCount == 0)
		{
			loopCount = -1;
		}
		return this;
	}

	public LTDescr setLoopClamp(int loops)
	{
		loopCount = loops;
		return this;
	}

	public LTDescr setLoopPingPong()
	{
		loopType = LeanTweenType.pingPong;
		if (loopCount == 0)
		{
			loopCount = -1;
		}
		return this;
	}

	public LTDescr setLoopPingPong(int loops)
	{
		loopType = LeanTweenType.pingPong;
		loopCount = ((loops == -1) ? loops : (loops * 2));
		return this;
	}

	public LTDescr setOnComplete(Action onComplete)
	{
		_optional.onComplete = onComplete;
		hasExtraOnCompletes = true;
		return this;
	}

	public LTDescr setOnComplete(Action<object> onComplete)
	{
		_optional.onCompleteObject = onComplete;
		hasExtraOnCompletes = true;
		return this;
	}

	public LTDescr setOnComplete(Action<object> onComplete, object onCompleteParam)
	{
		_optional.onCompleteObject = onComplete;
		hasExtraOnCompletes = true;
		if (onCompleteParam != null)
		{
			_optional.onCompleteParam = onCompleteParam;
		}
		return this;
	}

	public LTDescr setOnCompleteParam(object onCompleteParam)
	{
		_optional.onCompleteParam = onCompleteParam;
		hasExtraOnCompletes = true;
		return this;
	}

	public LTDescr setOnUpdate(Action<float> onUpdate)
	{
		_optional.onUpdateFloat = onUpdate;
		hasUpdateCallback = true;
		return this;
	}

	public LTDescr setOnUpdateRatio(Action<float, float> onUpdate)
	{
		_optional.onUpdateFloatRatio = onUpdate;
		hasUpdateCallback = true;
		return this;
	}

	public LTDescr setOnUpdateObject(Action<float, object> onUpdate)
	{
		_optional.onUpdateFloatObject = onUpdate;
		hasUpdateCallback = true;
		return this;
	}

	public LTDescr setOnUpdateVector2(Action<Vector2> onUpdate)
	{
		_optional.onUpdateVector2 = onUpdate;
		hasUpdateCallback = true;
		return this;
	}

	public LTDescr setOnUpdateVector3(Action<Vector3> onUpdate)
	{
		_optional.onUpdateVector3 = onUpdate;
		hasUpdateCallback = true;
		return this;
	}

	public LTDescr setOnUpdateColor(Action<Color> onUpdate)
	{
		_optional.onUpdateColor = onUpdate;
		hasUpdateCallback = true;
		return this;
	}

	public LTDescr setOnUpdateColor(Action<Color, object> onUpdate)
	{
		_optional.onUpdateColorObject = onUpdate;
		hasUpdateCallback = true;
		return this;
	}

	public LTDescr setOnUpdate(Action<Color> onUpdate)
	{
		_optional.onUpdateColor = onUpdate;
		hasUpdateCallback = true;
		return this;
	}

	public LTDescr setOnUpdate(Action<Color, object> onUpdate)
	{
		_optional.onUpdateColorObject = onUpdate;
		hasUpdateCallback = true;
		return this;
	}

	public LTDescr setOnUpdate(Action<float, object> onUpdate, object onUpdateParam = null)
	{
		_optional.onUpdateFloatObject = onUpdate;
		hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			_optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	public LTDescr setOnUpdate(Action<Vector3, object> onUpdate, object onUpdateParam = null)
	{
		_optional.onUpdateVector3Object = onUpdate;
		hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			_optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	public LTDescr setOnUpdate(Action<Vector2> onUpdate, object onUpdateParam = null)
	{
		_optional.onUpdateVector2 = onUpdate;
		hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			_optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	public LTDescr setOnUpdate(Action<Vector3> onUpdate, object onUpdateParam = null)
	{
		_optional.onUpdateVector3 = onUpdate;
		hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			_optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	public LTDescr setOnUpdateParam(object onUpdateParam)
	{
		_optional.onUpdateParam = onUpdateParam;
		return this;
	}

	public LTDescr setOrientToPath(bool doesOrient)
	{
		if (type == TweenAction.MOVE_CURVED || type == TweenAction.MOVE_CURVED_LOCAL)
		{
			if (_optional.path == null)
			{
				_optional.path = new LTBezierPath();
			}
			_optional.path.orientToPath = doesOrient;
		}
		else
		{
			_optional.spline.orientToPath = doesOrient;
		}
		return this;
	}

	public LTDescr setOrientToPath2d(bool doesOrient2d)
	{
		setOrientToPath(doesOrient2d);
		if (type == TweenAction.MOVE_CURVED || type == TweenAction.MOVE_CURVED_LOCAL)
		{
			_optional.path.orientToPath2d = doesOrient2d;
		}
		else
		{
			_optional.spline.orientToPath2d = doesOrient2d;
		}
		return this;
	}

	public LTDescr setRect(LTRect rect)
	{
		_optional.ltRect = rect;
		return this;
	}

	public LTDescr setRect(Rect rect)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_optional.ltRect = new LTRect(rect);
		return this;
	}

	public LTDescr setPath(LTBezierPath path)
	{
		_optional.path = path;
		return this;
	}

	public LTDescr setPoint(Vector3 point)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_optional.point = point;
		return this;
	}

	public LTDescr setDestroyOnComplete(bool doesDestroy)
	{
		destroyOnComplete = doesDestroy;
		return this;
	}

	public LTDescr setAudio(object audio)
	{
		_optional.onCompleteParam = audio;
		return this;
	}

	public LTDescr setOnCompleteOnRepeat(bool isOn)
	{
		onCompleteOnRepeat = isOn;
		return this;
	}

	public LTDescr setOnCompleteOnStart(bool isOn)
	{
		onCompleteOnStart = isOn;
		return this;
	}

	public LTDescr setRect(RectTransform rect)
	{
		rectTransform = rect;
		return this;
	}

	public LTDescr setSprites(Sprite[] sprites)
	{
		this.sprites = sprites;
		return this;
	}

	public LTDescr setFrameRate(float frameRate)
	{
		time = (float)sprites.Length / frameRate;
		return this;
	}

	public LTDescr setOnStart(Action onStart)
	{
		_optional.onStart = onStart;
		return this;
	}

	public LTDescr setDirection(float direction)
	{
		if (this.direction != -1f && this.direction != 1f)
		{
			Debug.LogWarning((object)("You have passed an incorrect direction of '" + direction + "', direction must be -1f or 1f"));
			return this;
		}
		if (this.direction != direction)
		{
			if (hasInitiliazed)
			{
				this.direction = direction;
			}
			else if (_optional.path != null)
			{
				_optional.path = new LTBezierPath(LTUtility.reverse(_optional.path.pts));
			}
			else if (_optional.spline != null)
			{
				_optional.spline = new LTSpline(LTUtility.reverse(_optional.spline.pts));
			}
		}
		return this;
	}

	public LTDescr setRecursive(bool useRecursion)
	{
		this.useRecursion = useRecursion;
		return this;
	}
}
