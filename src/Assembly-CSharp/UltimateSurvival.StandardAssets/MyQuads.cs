using UnityEngine;

namespace UltimateSurvival.StandardAssets;

internal class MyQuads
{
	private static Mesh[] meshes;

	private static int currentQuads;

	private static bool HasMeshes()
	{
		if (meshes == null)
		{
			return false;
		}
		Mesh[] array = meshes;
		foreach (Mesh val in array)
		{
			if ((Object)null == (Object)(object)val)
			{
				return false;
			}
		}
		return true;
	}

	public static void Cleanup()
	{
		if (meshes == null)
		{
			return;
		}
		for (int i = 0; i < meshes.Length; i++)
		{
			if ((Object)null != (Object)(object)meshes[i])
			{
				Object.DestroyImmediate((Object)(object)meshes[i]);
				meshes[i] = null;
			}
		}
		meshes = null;
	}

	public static Mesh[] GetMeshes(int totalWidth, int totalHeight)
	{
		if (HasMeshes() && currentQuads == totalWidth * totalHeight)
		{
			return meshes;
		}
		int num = 10833;
		int num2 = (currentQuads = totalWidth * totalHeight);
		meshes = (Mesh[])(object)new Mesh[Mathf.CeilToInt(1f * (float)num2 / (1f * (float)num))];
		int num3 = 0;
		int num4 = 0;
		for (num3 = 0; num3 < num2; num3 += num)
		{
			int triCount = Mathf.FloorToInt((float)Mathf.Clamp(num2 - num3, 0, num));
			meshes[num4] = GetMesh(triCount, num3, totalWidth, totalHeight);
			num4++;
		}
		return meshes;
	}

	private static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		Mesh val = new Mesh();
		((Object)val).hideFlags = (HideFlags)52;
		Vector3[] array = (Vector3[])(object)new Vector3[triCount * 4];
		Vector2[] array2 = (Vector2[])(object)new Vector2[triCount * 4];
		Vector2[] array3 = (Vector2[])(object)new Vector2[triCount * 4];
		int[] array4 = new int[triCount * 6];
		Vector3 val2 = default(Vector3);
		for (int i = 0; i < triCount; i++)
		{
			int num = i * 4;
			int num2 = i * 6;
			int num3 = triOffset + i;
			float num4 = Mathf.Floor((float)(num3 % totalWidth)) / (float)totalWidth;
			float num5 = Mathf.Floor((float)(num3 / totalWidth)) / (float)totalHeight;
			((Vector3)(ref val2))._002Ector(num4 * 2f - 1f, num5 * 2f - 1f, 1f);
			array[num] = val2;
			array[num + 1] = val2;
			array[num + 2] = val2;
			array[num + 3] = val2;
			array2[num] = new Vector2(0f, 0f);
			array2[num + 1] = new Vector2(1f, 0f);
			array2[num + 2] = new Vector2(0f, 1f);
			array2[num + 3] = new Vector2(1f, 1f);
			array3[num] = new Vector2(num4, num5);
			array3[num + 1] = new Vector2(num4, num5);
			array3[num + 2] = new Vector2(num4, num5);
			array3[num + 3] = new Vector2(num4, num5);
			array4[num2] = num;
			array4[num2 + 1] = num + 1;
			array4[num2 + 2] = num + 2;
			array4[num2 + 3] = num + 1;
			array4[num2 + 4] = num + 2;
			array4[num2 + 5] = num + 3;
		}
		val.vertices = array;
		val.triangles = array4;
		val.uv = array2;
		val.uv2 = array3;
		return val;
	}
}
