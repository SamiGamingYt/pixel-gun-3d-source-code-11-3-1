using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002FA RID: 762
public sealed class LoadingNGUIController : MonoBehaviour
{
	// Token: 0x170004AE RID: 1198
	// (set) Token: 0x06001A6D RID: 6765 RVA: 0x0006AD0C File Offset: 0x00068F0C
	public string SceneToLoad
	{
		set
		{
			this.sceneToLoad = value;
		}
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x0006AD18 File Offset: 0x00068F18
	public void Init()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("PromoForLoadings");
		if (textAsset == null)
		{
			return;
		}
		string text = textAsset.text;
		if (text == null)
		{
			return;
		}
		string[] array = text.Split(new char[]
		{
			'\r',
			'\n'
		});
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		foreach (string text2 in array)
		{
			string[] array3 = text2.Split(new char[]
			{
				'\t'
			});
			if (array3.Length >= 2)
			{
				if (array3[0] != null && this.sceneToLoad != null && array3[0].Equals(this.sceneToLoad))
				{
					List<string> list = new List<string>();
					for (int j = 1; j < array3.Length; j++)
					{
						if (array3[j] != null && array3[j].Equals("armor"))
						{
							list.AddRange(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0]);
						}
						else if (array3[j] == null || !array3[j].Equals("hat"))
						{
							for (int k = 0; k < WeaponManager.sharedManager.weaponsInGame.Length; k++)
							{
								if (WeaponManager.sharedManager.weaponsInGame[k].name.Equals(array3[j]))
								{
									array3[j] = ItemDb.GetByPrefabName(WeaponManager.sharedManager.weaponsInGame[k].name).Tag;
									array3[j] = (PromoActionsGUIController.FilterForLoadings(array3[j], list) ?? string.Empty);
									break;
								}
							}
							if (!string.IsNullOrEmpty(array3[j]))
							{
								list.Add(array3[j]);
							}
						}
					}
					List<string> list2 = PromoActionsGUIController.FilterPurchases(list, true, true, false, true);
					foreach (string item in list2)
					{
						list.Remove(item);
					}
					if (dictionary.ContainsKey(array3[0]))
					{
						dictionary[array3[0]] = list;
					}
					else
					{
						dictionary.Add(array3[0], list);
					}
				}
			}
		}
		if (this.sceneToLoad != null && dictionary.ContainsKey(this.sceneToLoad ?? string.Empty))
		{
			List<string> list3 = dictionary[this.sceneToLoad ?? string.Empty];
			if (list3 != null)
			{
				for (int l = 0; l < list3.Count; l++)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("PromoItemForLoading") as GameObject);
					gameObject.transform.parent = this.gunsPoint;
					gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
					gameObject.transform.localPosition = new Vector3(-256f * (float)list3.Count / 2f + 128f + (float)l * 256f, 0f, 0f);
					PromoItemForLoading component = gameObject.GetComponent<PromoItemForLoading>();
					int itemCategory = ItemDb.GetItemCategory(list3[l]);
					Texture itemIcon = ItemDb.GetItemIcon(list3[l], (ShopNGUIController.CategoryNames)itemCategory, null, false);
					foreach (UITexture uitexture in component.texture)
					{
						if (itemIcon != null)
						{
							uitexture.mainTexture = itemIcon;
						}
					}
					foreach (UILabel uilabel in component.label)
					{
						uilabel.text = ItemDb.GetItemName(list3[l], (ShopNGUIController.CategoryNames)itemCategory).Trim().Replace(" -", "-");
					}
				}
			}
		}
		this.recommendedForThisMap.gameObject.SetActive(this.sceneToLoad != null && dictionary.ContainsKey(this.sceneToLoad) && dictionary[this.sceneToLoad] != null && dictionary[this.sceneToLoad].Count > 0);
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.sceneToLoad);
		foreach (UILabel uilabel2 in this.levelNameLabels)
		{
			if (infoScene != null && !string.IsNullOrEmpty(this.sceneToLoad))
			{
				uilabel2.gameObject.SetActive(true);
				string text3 = infoScene.TranslatePreviewName;
				text3 = text3.Replace("\n", " ");
				text3 = text3.Replace("\r", " ");
				text3 = text3.ToUpper();
				uilabel2.text = text3;
			}
			else
			{
				uilabel2.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x0006B254 File Offset: 0x00069454
	public void SetEnabledMapName(bool enabled)
	{
		for (int i = 0; i < this.levelNameLabels.Length; i++)
		{
			this.levelNameLabels[i].gameObject.SetActive(enabled);
		}
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x0006B290 File Offset: 0x00069490
	public void SetEnabledGunsScroll(bool enabled)
	{
		if (this.recommendedForThisMap != null)
		{
			this.recommendedForThisMap.gameObject.SetActive(enabled);
		}
		if (this.gunsPoint != null)
		{
			this.gunsPoint.gameObject.SetActive(enabled);
		}
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x0006B2E4 File Offset: 0x000694E4
	private void OnDestroy()
	{
		this.loadingNGUITexture = null;
	}

	// Token: 0x04000F84 RID: 3972
	private string sceneToLoad = string.Empty;

	// Token: 0x04000F85 RID: 3973
	public UITexture loadingNGUITexture;

	// Token: 0x04000F86 RID: 3974
	public UILabel[] levelNameLabels;

	// Token: 0x04000F87 RID: 3975
	public UILabel recommendedForThisMap;

	// Token: 0x04000F88 RID: 3976
	public Transform gunsPoint;
}
