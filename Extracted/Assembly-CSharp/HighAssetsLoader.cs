using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x020002A0 RID: 672
internal sealed class HighAssetsLoader : MonoBehaviour
{
	// Token: 0x06001538 RID: 5432 RVA: 0x00054290 File Offset: 0x00052490
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x000542A0 File Offset: 0x000524A0
	private void OnLevelWasLoaded(int lev)
	{
		if (Device.isWeakDevice || (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 && !Application.isEditor))
		{
			return;
		}
		string path = ResPath.Combine(ResPath.Combine(HighAssetsLoader.LightmapsFolder, HighAssetsLoader.HighFolder), Application.loadedLevelName);
		string path2 = ResPath.Combine(ResPath.Combine(HighAssetsLoader.AtlasFolder, HighAssetsLoader.HighFolder), Application.loadedLevelName);
		Texture2D[] array = Resources.LoadAll<Texture2D>(path);
		if (array != null && array.Length > 0)
		{
			List<Texture2D> list = new List<Texture2D>();
			foreach (Texture2D item in array)
			{
				list.Add(item);
			}
			list.Sort((Texture2D lightmap1, Texture2D lightmap2) => lightmap1.name.CompareTo(lightmap2.name));
			LightmapData lightmapData = new LightmapData();
			lightmapData.lightmapFar = list[0];
			LightmapSettings.lightmaps = new List<LightmapData>
			{
				lightmapData
			}.ToArray();
		}
		Texture2D[] array3 = Resources.LoadAll<Texture2D>(path2);
		string value = Application.loadedLevelName + "_Atlas";
		if (array3 != null && array3.Length > 0)
		{
			UnityEngine.Object[] array4 = UnityEngine.Object.FindObjectsOfType(typeof(Renderer));
			List<Material> list2 = new List<Material>();
			foreach (Renderer renderer in array4)
			{
				if (renderer != null && renderer.sharedMaterial != null && renderer.sharedMaterial.name != null && renderer.sharedMaterial.name.Contains(value) && !list2.Contains(renderer.sharedMaterial))
				{
					list2.Add(renderer.sharedMaterial);
				}
			}
			List<Texture2D> list3 = new List<Texture2D>();
			foreach (Texture2D item2 in array3)
			{
				list3.Add(item2);
			}
			list2.Sort((Material m1, Material m2) => m1.name.CompareTo(m2.name));
			list3.Sort((Texture2D a1, Texture2D a2) => a1.name.CompareTo(a2.name));
			for (int l = 0; l < Mathf.Min(list2.Count, list3.Count); l++)
			{
				list2[l].mainTexture = list3[l];
			}
		}
	}

	// Token: 0x04000C62 RID: 3170
	public static readonly string LightmapsFolder = "Lightmap";

	// Token: 0x04000C63 RID: 3171
	public static readonly string HighFolder = "High";

	// Token: 0x04000C64 RID: 3172
	public static readonly string AtlasFolder = "Atlas";
}
