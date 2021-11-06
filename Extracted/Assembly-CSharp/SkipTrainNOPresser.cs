using System;
using UnityEngine;

// Token: 0x020007CE RID: 1998
[Obsolete]
public sealed class SkipTrainNOPresser : SkipTrainingButton
{
	// Token: 0x060048A5 RID: 18597 RVA: 0x0019327C File Offset: 0x0019147C
	protected override void OnClick()
	{
		base.gameObject.transform.parent.gameObject.SetActive(false);
		this.skipButton.SetActive(true);
		base.OnClick();
		GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
		if (gameObject && gameObject != null)
		{
			Transform child = gameObject.transform.GetChild(0);
			if (child && child != null)
			{
				child.gameObject.SetActive(true);
			}
		}
		TrainingController.CancelSkipTraining();
	}

	// Token: 0x0400358D RID: 13709
	public GameObject skipButton;
}
