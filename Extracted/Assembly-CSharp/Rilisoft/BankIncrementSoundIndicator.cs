using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200065C RID: 1628
	public class BankIncrementSoundIndicator : MonoBehaviour
	{
		// Token: 0x060038CC RID: 14540 RVA: 0x00125CFC File Offset: 0x00123EFC
		private void OnEnable()
		{
			CoinsMessage.CoinsLabelDisappeared += this.OnCurrencyGetted;
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x00125D10 File Offset: 0x00123F10
		private void OnDisable()
		{
			CoinsMessage.CoinsLabelDisappeared -= this.OnCurrencyGetted;
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x00125D24 File Offset: 0x00123F24
		private void OnCurrencyGetted(bool isGems, int count)
		{
			float delay = (Defs.isMulti || Defs.IsSurvival || !TrainingController.TrainingCompleted) ? this.PlayDelay : 0f;
			base.StartCoroutine(this.PlaySounds(isGems, count < 2, delay));
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x00125D74 File Offset: 0x00123F74
		private IEnumerator PlaySounds(bool isGems, bool oneCoin, float delay)
		{
			if (SceneLoader.ActiveSceneName.Equals("LevelComplete"))
			{
				yield break;
			}
			if (!Defs.isSoundFX)
			{
				yield break;
			}
			yield return new WaitForRealSeconds(delay);
			AudioClip clip = null;
			if (isGems)
			{
				clip = ((!oneCoin || !(this.ClipCoinAdded != null)) ? this.ClipGemsAdded : this.ClipGemAdded);
			}
			else
			{
				clip = ((!oneCoin || !(this.ClipCoinAdded != null)) ? this.ClipCoinsAdded : this.ClipCoinAdded);
			}
			NGUITools.PlaySound(clip);
			yield break;
		}

		// Token: 0x0400298E RID: 10638
		public float PlayDelay = 0.1f;

		// Token: 0x0400298F RID: 10639
		public AudioClip ClipCoinAdded;

		// Token: 0x04002990 RID: 10640
		public AudioClip ClipCoinsAdded;

		// Token: 0x04002991 RID: 10641
		public AudioClip ClipGemAdded;

		// Token: 0x04002992 RID: 10642
		public AudioClip ClipGemsAdded;
	}
}
