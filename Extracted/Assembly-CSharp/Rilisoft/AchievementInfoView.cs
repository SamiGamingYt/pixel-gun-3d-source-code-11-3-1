using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000517 RID: 1303
	public class AchievementInfoView : MonoBehaviour
	{
		// Token: 0x06002D64 RID: 11620 RVA: 0x000EE6FC File Offset: 0x000EC8FC
		private void OnEnable()
		{
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
			}
			this._backSubscription = BackSystem.Instance.Register(new Action(this.Hide), base.GetType().Name);
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x000EE748 File Offset: 0x000EC948
		private void OnDisable()
		{
			if (this._backSubscription != null)
			{
				this._backSubscription.Dispose();
				this._backSubscription = null;
			}
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x000EE768 File Offset: 0x000EC968
		public void Show(Achievement ach)
		{
			base.gameObject.SetActive(true);
			this._textureAchievementsBg.mainTexture = AchievementView.BackgroundTextureFor(ach);
			this._spriteIcon.spriteName = ach.Data.Icon;
			this._textName.Text = LocalizationStore.Get(ach.Data.LKeyName);
			string text = string.Empty;
			if (ach.Type == AchievementType.Common || ach.Type == AchievementType.Openable)
			{
				if (ach.Stage < ach.MaxStage)
				{
					text = string.Format("{0}/{1}", ach.Points, ach.ToNextStagePointsTotal);
				}
				else
				{
					text = string.Format("[00ff00]{0}", ach.Points);
				}
			}
			if (ach.Type == AchievementType.Openable && ach.IsCompleted && ach.Data.Thresholds[0] == 1)
			{
				text = string.Empty;
			}
			this._labelDesc.text = ((!text.IsNullOrEmpty()) ? string.Format("{0}{1}{2}", LocalizationStore.Get(ach.Data.LKeyDesc), Environment.NewLine, text) : LocalizationStore.Get(ach.Data.LKeyDesc));
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x000EE8AC File Offset: 0x000ECAAC
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x040021F9 RID: 8697
		[SerializeField]
		private UITexture _textureAchievementsBg;

		// Token: 0x040021FA RID: 8698
		[SerializeField]
		private UISprite _spriteIcon;

		// Token: 0x040021FB RID: 8699
		[SerializeField]
		private TextGroup _textName;

		// Token: 0x040021FC RID: 8700
		[SerializeField]
		private UILabel _labelDesc;

		// Token: 0x040021FD RID: 8701
		private IDisposable _backSubscription;
	}
}
