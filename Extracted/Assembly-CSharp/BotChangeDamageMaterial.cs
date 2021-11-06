using System;
using UnityEngine;

// Token: 0x0200058A RID: 1418
public sealed class BotChangeDamageMaterial : MonoBehaviour
{
	// Token: 0x0600316E RID: 12654 RVA: 0x00101B64 File Offset: 0x000FFD64
	private void Start()
	{
		string name = base.transform.root.GetChild(0).name;
		Texture texture = null;
		if (name.Contains("Enemy"))
		{
			string text = name + "_Level" + CurrentCampaignGame.currentLevel;
			if (!(texture = (SkinsManagerPixlGun.sharedManager.skins[text] as Texture)))
			{
				Debug.Log("No skin: " + text);
			}
		}
		if (texture != null)
		{
			this._mainTexture = texture;
			this.ResetMainMaterial();
		}
		else
		{
			this._mainTexture = base.GetComponent<Renderer>().material.mainTexture;
		}
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x00101C18 File Offset: 0x000FFE18
	public void ShowDamageEffect(bool poison = false)
	{
		base.GetComponent<Renderer>().material.mainTexture = ((!poison) ? SkinsController.damageHitTexture : SkinsController.poisonHitTexture);
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x00101C4C File Offset: 0x000FFE4C
	public void ResetMainMaterial()
	{
		base.GetComponent<Renderer>().material.mainTexture = this._mainTexture;
	}

	// Token: 0x0400246F RID: 9327
	private Texture _mainTexture;
}
