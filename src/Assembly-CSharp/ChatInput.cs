using UnityEngine;

[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]
public class ChatInput : MonoBehaviour
{
	public UITextList textList;

	public bool fillWithDummyData;

	private UIInput mInput;

	private void Start()
	{
		mInput = ((Component)this).GetComponent<UIInput>();
		mInput.label.maxLineCount = 1;
		if (fillWithDummyData && (Object)(object)textList != (Object)null)
		{
			for (int i = 0; i < 30; i++)
			{
				textList.Add(((i % 2 == 0) ? "[FFFFFF]" : "[AAAAAA]") + "This is an example paragraph for the text list, testing line " + i + "[-]");
			}
		}
	}

	public void OnSubmit()
	{
		if ((Object)(object)textList != (Object)null)
		{
			string text = NGUIText.StripSymbols(mInput.value);
			if (!string.IsNullOrEmpty(text))
			{
				textList.Add(text);
				mInput.value = "";
				mInput.isSelected = false;
			}
		}
	}
}
