using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005F1 RID: 1521
	public class NestClickHandler : MonoBehaviour
	{
		// Token: 0x06003420 RID: 13344 RVA: 0x0010DDC4 File Offset: 0x0010BFC4
		private void OnClick()
		{
			if (Nest.Instance != null)
			{
				Nest.Instance.Click();
				this.HideAndSaveHint();
			}
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x0010DDF4 File Offset: 0x0010BFF4
		private void Start()
		{
			if (HintController.instance != null && ExperienceController.sharedController.currentLevel >= 2 && Nest.Instance.NestCanShow() && PlayerPrefs.GetInt("NestHintShowed", 0) == 0)
			{
				this.hintShowed = true;
				HintController.instance.ShowHintByName("incubator", 0f);
				HintController.instance.ShowHintByName("incubator_2", 0f);
			}
		}

		// Token: 0x06003422 RID: 13346 RVA: 0x0010DE70 File Offset: 0x0010C070
		private void OnDisable()
		{
			this.HideAndSaveHint();
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x0010DE78 File Offset: 0x0010C078
		private void HideAndSaveHint()
		{
			if (this.hintShowed && HintController.instance != null)
			{
				this.hintShowed = false;
				HintController.instance.HideHintByName("incubator");
				HintController.instance.HideHintByName("incubator_2");
				PlayerPrefs.SetInt("NestHintShowed", 1);
			}
		}

		// Token: 0x04002655 RID: 9813
		private bool hintShowed;
	}
}
