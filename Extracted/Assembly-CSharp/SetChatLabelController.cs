using System;
using UnityEngine;

// Token: 0x0200079C RID: 1948
public class SetChatLabelController : MonoBehaviour
{
	// Token: 0x060045C7 RID: 17863 RVA: 0x0017938C File Offset: 0x0017758C
	public void SetChatLabelText(Player_move_c.SystemMessage message)
	{
		this.SetChatLabelText(message.nick1, message.message2, message.nick2, message.message, message.textColor);
	}

	// Token: 0x060045C8 RID: 17864 RVA: 0x001793B8 File Offset: 0x001775B8
	public void SetChatLabelText(string _nameKiller, string _reasonSpriteName2, string _nameKilled, string _reasonSpriteName, Color color)
	{
		this.killerLabel.text = _nameKiller;
		this.killerLabel.color = color;
		int num = this.killerLabel.width;
		if (this.reasonSprite != null && !string.IsNullOrEmpty(_reasonSpriteName))
		{
			if (!this.reasonSprite.gameObject.activeSelf)
			{
				this.reasonSprite.gameObject.SetActive(true);
			}
			this.reasonSprite.transform.localPosition = new Vector3((float)num + 33f, 0f, 0f);
			num += 66;
			this.reasonSprite.spriteName = this.SubstituteWeaponImageIfNeeded(_reasonSpriteName);
		}
		else if (this.reasonSprite != null && this.reasonSprite.gameObject.activeSelf)
		{
			this.reasonSprite.gameObject.SetActive(false);
		}
		if (string.IsNullOrEmpty(_reasonSpriteName2))
		{
			if (this.reasonSprite2.gameObject.activeSelf)
			{
				this.reasonSprite2.gameObject.SetActive(false);
			}
		}
		else
		{
			if (!this.reasonSprite2.gameObject.activeSelf)
			{
				this.reasonSprite2.gameObject.SetActive(true);
			}
			this.reasonSprite2.transform.localPosition = new Vector3((float)num + 23f, 0f, 0f);
			num += 46;
			this.reasonSprite2.spriteName = this.SubstituteWeaponImageIfNeeded(_reasonSpriteName2);
		}
		if (string.IsNullOrEmpty(_nameKilled))
		{
			if (this.killedLabel.gameObject.activeSelf)
			{
				this.killedLabel.gameObject.SetActive(false);
			}
		}
		else
		{
			if (!this.killedLabel.gameObject.activeSelf)
			{
				this.killedLabel.gameObject.SetActive(true);
			}
			this.killedLabel.transform.localPosition = new Vector3((float)num, 0f, 0f);
			this.killedLabel.text = _nameKilled;
			this.killedLabel.color = color;
		}
	}

	// Token: 0x060045C9 RID: 17865 RVA: 0x001795D8 File Offset: 0x001777D8
	private string SubstituteWeaponImageIfNeeded(string source)
	{
		if (source == null)
		{
			return null;
		}
		ItemRecord byPrefabName = ItemDb.GetByPrefabName(source);
		if (byPrefabName != null && byPrefabName.UseImagesFromFirstUpgrade && byPrefabName.Tag != null)
		{
			string text = WeaponUpgrades.TagOfFirstUpgrade(byPrefabName.Tag);
			if (text != null && !text.Equals(byPrefabName.Tag))
			{
				ItemRecord byTag = ItemDb.GetByTag(text);
				if (byTag != null && byTag.PrefabName != null)
				{
					return byTag.PrefabName;
				}
			}
		}
		return source;
	}

	// Token: 0x04003328 RID: 13096
	public UILabel killerLabel;

	// Token: 0x04003329 RID: 13097
	public UILabel killedLabel;

	// Token: 0x0400332A RID: 13098
	public UISprite reasonSprite;

	// Token: 0x0400332B RID: 13099
	public UISprite reasonSprite2;
}
