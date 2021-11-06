using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000158 RID: 344
internal sealed class GemsUpdater : MonoBehaviour
{
	// Token: 0x06000B57 RID: 2903 RVA: 0x00040338 File Offset: 0x0003E538
	private void Start()
	{
		this.coinsLabel = base.GetComponent<UILabel>();
		CoinsMessage.CoinsLabelDisappeared += this._ReplaceMsgForTraining;
		string text = Storager.getInt("GemsCurrency", false).ToString();
		if (this.coinsLabel != null)
		{
			this.coinsLabel.text = text;
		}
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x00040394 File Offset: 0x0003E594
	private void OnEnable()
	{
		BankController.onUpdateMoney += this.UpdateMoney;
		base.StartCoroutine(this.UpdateCoinsLabel());
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x000403B4 File Offset: 0x0003E5B4
	private void OnDisable()
	{
		BankController.onUpdateMoney -= this.UpdateMoney;
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x000403C8 File Offset: 0x0003E5C8
	private void _ReplaceMsgForTraining(bool isGems, int count)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this._trainingMsg = GemsUpdater.trainCoinsStub;
		}
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x000403EC File Offset: 0x0003E5EC
	private IEnumerator UpdateCoinsLabel()
	{
		while (!this._disposed)
		{
			if (!BankController.canShowIndication)
			{
				yield return null;
			}
			else
			{
				this.UpdateMoney();
				yield return base.StartCoroutine(this.MyWaitForSeconds(1f));
			}
		}
		yield break;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00040408 File Offset: 0x0003E608
	private void UpdateMoney()
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			if (this.coinsLabel != null)
			{
				if (ShopNGUIController.sharedShop != null && ShopNGUIController.GuiActive)
				{
					this.coinsLabel.text = "999";
				}
				else
				{
					this.coinsLabel.text = this._trainingMsg;
				}
			}
		}
		else
		{
			string text = Storager.getInt("GemsCurrency", false).ToString();
			if (this.coinsLabel != null)
			{
				this.coinsLabel.text = text;
			}
		}
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x000404B0 File Offset: 0x0003E6B0
	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
		yield break;
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x000404D4 File Offset: 0x0003E6D4
	private void OnDestroy()
	{
		CoinsMessage.CoinsLabelDisappeared -= this._ReplaceMsgForTraining;
		this._disposed = true;
	}

	// Token: 0x0400090B RID: 2315
	public static readonly string trainCoinsStub = "999";

	// Token: 0x0400090C RID: 2316
	private UILabel coinsLabel;

	// Token: 0x0400090D RID: 2317
	private string _trainingMsg = "0";

	// Token: 0x0400090E RID: 2318
	private bool _disposed;
}
