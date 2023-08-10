using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
[AddComponentMenu("NGUI/UI/Localize")]
public class UILocalize : MonoBehaviour
{
	public string key;

	private bool mStarted;

	public string value
	{
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			UIWidget component = ((Component)this).GetComponent<UIWidget>();
			UILabel uILabel = component as UILabel;
			UISprite uISprite = component as UISprite;
			if ((Object)(object)uILabel != (Object)null)
			{
				UIInput uIInput = NGUITools.FindInParents<UIInput>(((Component)uILabel).gameObject);
				if ((Object)(object)uIInput != (Object)null && (Object)(object)uIInput.label == (Object)(object)uILabel)
				{
					uIInput.defaultText = value;
				}
				else
				{
					uILabel.text = value;
				}
			}
			else if ((Object)(object)uISprite != (Object)null)
			{
				uISprite.spriteName = value;
				uISprite.MakePixelPerfect();
			}
		}
	}

	private void OnEnable()
	{
		if (mStarted)
		{
			OnLocalize();
		}
	}

	private void Start()
	{
		mStarted = true;
		OnLocalize();
	}

	private void OnLocalize()
	{
		if (string.IsNullOrEmpty(key))
		{
			UILabel component = ((Component)this).GetComponent<UILabel>();
			if ((Object)(object)component != (Object)null)
			{
				key = component.text;
			}
		}
		if (!string.IsNullOrEmpty(key))
		{
			value = Localization.Get(key);
		}
	}
}
