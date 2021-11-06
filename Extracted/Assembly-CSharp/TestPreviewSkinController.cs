using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000858 RID: 2136
internal sealed class TestPreviewSkinController : MonoBehaviour
{
	// Token: 0x06004D54 RID: 19796 RVA: 0x001BE834 File Offset: 0x001BCA34
	private IEnumerator Start()
	{
		yield return null;
		this.Skins();
		yield break;
	}

	// Token: 0x06004D55 RID: 19797 RVA: 0x001BE850 File Offset: 0x001BCA50
	private void Skins()
	{
		for (int i = 0; i < SkinsController.baseSkinsForPersInString.Length; i++)
		{
			Texture2D texture = SkinsController.TextureFromString(SkinsController.baseSkinsForPersInString[i], 64, 32);
			Texture2D texture2D = new Texture2D(16, 32, TextureFormat.ARGB32, false);
			for (int j = 0; j < 16; j++)
			{
				for (int k = 0; k < 32; k++)
				{
					texture2D.SetPixel(j, k, Color.clear);
				}
			}
			texture2D.SetPixels(4, 24, 8, 8, this.GetPixelsByRect(texture, new Rect(8f, 16f, 8f, 8f)));
			texture2D.SetPixels(4, 12, 8, 12, this.GetPixelsByRect(texture, new Rect(20f, 0f, 8f, 12f)));
			texture2D.SetPixels(0, 12, 4, 12, this.GetPixelsByRect(texture, new Rect(44f, 0f, 4f, 12f)));
			texture2D.SetPixels(12, 12, 4, 12, this.GetPixelsByRect(texture, new Rect(44f, 0f, 4f, 12f)));
			texture2D.SetPixels(4, 0, 4, 12, this.GetPixelsByRect(texture, new Rect(4f, 0f, 4f, 12f)));
			texture2D.SetPixels(8, 0, 4, 12, this.GetPixelsByRect(texture, new Rect(4f, 0f, 4f, 12f)));
			texture2D.anisoLevel = 1;
			texture2D.mipMapBias = -0.5f;
			texture2D.Apply();
			texture2D.filterMode = FilterMode.Point;
			texture2D.Apply();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.previewPrefab);
			gameObject.transform.parent = this.grid.transform;
			gameObject.transform.localPosition = new Vector3((float)(160 * i), 0f, 0f);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.GetComponent<SetTestSkinPreview>().texture.mainTexture = texture2D;
			gameObject.GetComponent<SetTestSkinPreview>().nameLabel.text = i.ToString();
			gameObject.GetComponent<SetTestSkinPreview>().keyLabel.text = ((!SkinsController.shopKeyFromNameSkin.ContainsKey(i.ToString())) ? string.Empty : SkinsController.shopKeyFromNameSkin[i.ToString()]);
			gameObject.name = i.ToString();
		}
		string text = string.Empty;
		string[] array = new string[]
		{
			"player3",
			"player4"
		};
		for (int l = 1; l <= 4; l++)
		{
			text = text + "\"" + SkinsController.StringFromTexture(Resources.Load("MultSkins_6_3/10.6.0_Skin" + l) as Texture2D) + "\",\n";
		}
		Debug.Log(text);
	}

	// Token: 0x06004D56 RID: 19798 RVA: 0x001BEB48 File Offset: 0x001BCD48
	private Color[] GetPixelsByRect(Texture2D texture, Rect rect)
	{
		return texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
	}

	// Token: 0x04003BBB RID: 15291
	public UIGrid grid;

	// Token: 0x04003BBC RID: 15292
	public GameObject previewPrefab;
}
