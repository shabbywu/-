using System;
using System.Collections.Generic;

namespace JSONClass;

public class SkillSeidJsonData66 : IJSONClass
{
	public static int SEIDID = 66;

	public static Dictionary<int, SkillSeidJsonData66> DataDict = new Dictionary<int, SkillSeidJsonData66>();

	public static List<SkillSeidJsonData66> DataList = new List<SkillSeidJsonData66>();

	public static Action OnInitFinishAction = OnInitFinish;

	public int skillid;

	public int value1;

	public static void InitDataDict()
	{
		foreach (JSONObject item in jsonData.instance.SkillSeidJsonData[66].list)
		{
			try
			{
				SkillSeidJsonData66 skillSeidJsonData = new SkillSeidJsonData66();
				skillSeidJsonData.skillid = item["skillid"].I;
				skillSeidJsonData.value1 = item["value1"].I;
				if (DataDict.ContainsKey(skillSeidJsonData.skillid))
				{
					PreloadManager.LogException($"!!!错误!!!向字典SkillSeidJsonData66.DataDict添加数据时出现重复的键，Key:{skillSeidJsonData.skillid}，已跳过，请检查配表");
					continue;
				}
				DataDict.Add(skillSeidJsonData.skillid, skillSeidJsonData);
				DataList.Add(skillSeidJsonData);
			}
			catch (Exception arg)
			{
				PreloadManager.LogException("!!!错误!!!向字典SkillSeidJsonData66.DataDict添加数据时出现异常，已跳过，请检查配表");
				PreloadManager.LogException($"异常信息:\n{arg}");
				PreloadManager.LogException($"数据序列化:\n{item}");
			}
		}
		if (OnInitFinishAction != null)
		{
			OnInitFinishAction();
		}
	}

	private static void OnInitFinish()
	{
	}
}
