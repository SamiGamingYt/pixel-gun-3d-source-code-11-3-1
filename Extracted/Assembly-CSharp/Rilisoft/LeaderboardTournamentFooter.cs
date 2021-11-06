using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000687 RID: 1671
	public class LeaderboardTournamentFooter : MonoBehaviour
	{
		// Token: 0x06003A77 RID: 14967 RVA: 0x0012E344 File Offset: 0x0012C544
		private void OnEnable()
		{
			this._textLabel.text = string.Format(LocalizationStore.Get("Key_2813"), BalanceController.countPlaceAwardInCompetion);
			if (this._textLabel.text.Length > 47)
			{
				this._textLabel.overflowMethod = UILabel.Overflow.ShrinkContent;
				this._textLabel.width = 690;
			}
			if (BalanceController.competitionAward == null)
			{
				return;
			}
			this._countLabel.text = BalanceController.competitionAward.Price + "!";
			if (BalanceController.competitionAward.Currency == "Coins")
			{
				this._coinObject.SetActiveSafe(true);
				this._gemObject.SetActiveSafe(false);
			}
			else
			{
				this._coinObject.SetActiveSafe(false);
				this._gemObject.SetActiveSafe(true);
			}
		}

		// Token: 0x04002B06 RID: 11014
		[SerializeField]
		private UILabel _textLabel;

		// Token: 0x04002B07 RID: 11015
		[SerializeField]
		private UILabel _countLabel;

		// Token: 0x04002B08 RID: 11016
		[SerializeField]
		private GameObject _coinObject;

		// Token: 0x04002B09 RID: 11017
		[SerializeField]
		private GameObject _gemObject;
	}
}
