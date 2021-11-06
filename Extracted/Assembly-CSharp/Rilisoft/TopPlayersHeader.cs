using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200068F RID: 1679
	public class TopPlayersHeader : MonoBehaviour
	{
		// Token: 0x06003AC2 RID: 15042 RVA: 0x0012FCE4 File Offset: 0x0012DEE4
		private void OnEnable()
		{
			if (this._label == null)
			{
				return;
			}
			int ourTier = ExpController.GetOurTier();
			if (ExpController.LevelsForTiers.Length - 1 >= ourTier)
			{
				this._startLevel = ExpController.LevelsForTiers[ourTier];
				if (ExpController.LevelsForTiers.Length - 1 >= ourTier + 1)
				{
					this._endLevel = ExpController.LevelsForTiers[ourTier + 1] - 1;
					this._label.text = string.Format("{0}: {1} - {2}", LocalizationStore.Get("Key_2835"), this._startLevel, this._endLevel);
				}
				else
				{
					this._label.text = string.Format("{0}: {1} + ", LocalizationStore.Get("Key_2835"), this._startLevel);
				}
			}
		}

		// Token: 0x04002B62 RID: 11106
		[SerializeField]
		private UILabel _label;

		// Token: 0x04002B63 RID: 11107
		private int _startLevel = -1;

		// Token: 0x04002B64 RID: 11108
		private int _endLevel = -1;
	}
}
