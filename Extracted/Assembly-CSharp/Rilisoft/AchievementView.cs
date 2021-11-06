using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200051C RID: 1308
	public class AchievementView : MonoBehaviour
	{
		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06002D8A RID: 11658 RVA: 0x000EF818 File Offset: 0x000EDA18
		// (remove) Token: 0x06002D8B RID: 11659 RVA: 0x000EF830 File Offset: 0x000EDA30
		public static event Action<AchievementView> OnClicked;

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06002D8C RID: 11660 RVA: 0x000EF848 File Offset: 0x000EDA48
		// (set) Token: 0x06002D8D RID: 11661 RVA: 0x000EF850 File Offset: 0x000EDA50
		public Achievement Achievement
		{
			get
			{
				return this._achievement;
			}
			set
			{
				if (this._achievement != null)
				{
					this._achievement.OnProgressChanged -= this.UpdateUI;
				}
				if (value == null)
				{
					Achievement.LogMsg("achievement is null");
					return;
				}
				this._achievement = value;
				this._textName.text = LocalizationStore.Get(this._achievement.Data.LKeyName);
				if (this._textName.text == this._achievement.Data.LKeyName)
				{
					this._textName.text = string.Format("[{0}]", this._achievement.GetType().Name.Replace("Achievement", string.Empty));
				}
				this.UpdateUI(true, true);
				this._achievement.OnProgressChanged += this.UpdateUI;
			}
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x000EF930 File Offset: 0x000EDB30
		public static Texture BackgroundTextureFor(Achievement ach)
		{
			string text;
			switch (ach.Type)
			{
			case AchievementType.Common:
				text = string.Format("Achievements/Achievment_base_common_{0}", ach.Stage);
				break;
			case AchievementType.Hidden:
				text = "Achievements/Achievment_base_hidden_1";
				break;
			case AchievementType.Openable:
				text = string.Format("Achievements/Achievment_base_openable_{0}", ach.Stage);
				break;
			default:
				text = string.Empty;
				break;
			}
			if (text.IsNullOrEmpty())
			{
				return null;
			}
			if (AchievementView._loadedBgTextures.ContainsKey(text))
			{
				return AchievementView._loadedBgTextures[text];
			}
			if (Debug.isDebugBuild)
			{
			}
			Texture texture = Resources.Load<Texture>(text);
			if (Debug.isDebugBuild)
			{
			}
			if (texture != null)
			{
				AchievementView._loadedBgTextures.Add(text, texture);
			}
			return texture;
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x000EFA08 File Offset: 0x000EDC08
		private void Awake()
		{
			LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.OnLocalize));
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000EFA1C File Offset: 0x000EDC1C
		private void OnLocalize()
		{
			this._textName.text = LocalizationStore.Get(this._achievement.Data.LKeyName);
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x000EFA4C File Offset: 0x000EDC4C
		private void UpdateUI(bool pointsChanged, bool stageChanged)
		{
			this._textureBackground.mainTexture = AchievementView.BackgroundTextureFor(this._achievement);
			this._textureBackground.fixedAspect = true;
			this._spriteIcon.spriteName = this._achievement.Data.Icon;
			if (this._achievement.IsCompleted)
			{
				this._spriteProgress.gameObject.SetActive(false);
				if (this._achievement.Type == AchievementType.Common)
				{
					this._textProgress.gameObject.SetActive(true);
					this._textProgress.text = string.Format("{0}", this._achievement.Points);
				}
				else
				{
					this._textProgress.gameObject.SetActive(false);
				}
			}
			else
			{
				this._textProgress.gameObject.SetActive(true);
				this._spriteProgress.gameObject.SetActive(true);
				this._textProgress.text = string.Format("{0}/{1}", this._achievement.Points, this._achievement.ToNextStagePointsTotal);
				this._spriteProgress.fillAmount = (float)this._achievement.Points / (float)this._achievement.ToNextStagePointsTotal;
			}
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000EFB94 File Offset: 0x000EDD94
		private void OnClick()
		{
			if (AchievementView.OnClicked != null)
			{
				AchievementView.OnClicked(this);
			}
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000EFBAC File Offset: 0x000EDDAC
		private void OnDestroy()
		{
			this._achievement.OnProgressChanged -= this.UpdateUI;
			LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.OnLocalize));
		}

		// Token: 0x0400220F RID: 8719
		[SerializeField]
		private UILabel _textName;

		// Token: 0x04002210 RID: 8720
		[SerializeField]
		private UITexture _textureBackground;

		// Token: 0x04002211 RID: 8721
		[SerializeField]
		private UISprite _spriteIcon;

		// Token: 0x04002212 RID: 8722
		[SerializeField]
		private UILabel _textProgress;

		// Token: 0x04002213 RID: 8723
		[SerializeField]
		private UISprite _spriteProgress;

		// Token: 0x04002214 RID: 8724
		private Achievement _achievement;

		// Token: 0x04002215 RID: 8725
		private static Dictionary<string, Texture> _loadedBgTextures = new Dictionary<string, Texture>();
	}
}
