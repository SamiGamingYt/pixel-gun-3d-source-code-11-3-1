using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000075 RID: 117
internal sealed class CoinsUpdater : MonoBehaviour
{
	// Token: 0x06000353 RID: 851 RVA: 0x0001C0A4 File Offset: 0x0001A2A4
	private void Start()
	{
		this.coinsLabel = base.GetComponent<UILabel>();
		CoinsMessage.CoinsLabelDisappeared += this._ReplaceMsgForTraining;
		string text = Storager.getInt("Coins", false).ToString();
		if (this.coinsLabel != null)
		{
			this.coinsLabel.text = text;
		}
	}

	// Token: 0x06000354 RID: 852 RVA: 0x0001C100 File Offset: 0x0001A300
	private void OnEnable()
	{
		BankController.onUpdateMoney += this.UpdateMoney;
		base.StartCoroutine(this.UpdateCoinsLabel());
	}

	// Token: 0x06000355 RID: 853 RVA: 0x0001C120 File Offset: 0x0001A320
	private void OnDisable()
	{
		BankController.onUpdateMoney -= this.UpdateMoney;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0001C134 File Offset: 0x0001A334
	private void _ReplaceMsgForTraining(bool isGems, int count)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			this._trainingMsg = CoinsUpdater.trainCoinsStub;
		}
	}

	// Token: 0x06000357 RID: 855 RVA: 0x0001C158 File Offset: 0x0001A358
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

	// Token: 0x06000358 RID: 856 RVA: 0x0001C174 File Offset: 0x0001A374
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
			string text = Storager.getInt("Coins", false).ToString();
			if (this.coinsLabel != null)
			{
				this.coinsLabel.text = text;
			}
		}
	}

	// Token: 0x06000359 RID: 857 RVA: 0x0001C21C File Offset: 0x0001A41C
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

	// Token: 0x0600035A RID: 858 RVA: 0x0001C240 File Offset: 0x0001A440
	private void OnDestroy()
	{
		CoinsMessage.CoinsLabelDisappeared -= this._ReplaceMsgForTraining;
		this._disposed = true;
	}

	// Token: 0x04000394 RID: 916
	public static readonly string trainCoinsStub = "999";

	// Token: 0x04000395 RID: 917
	private UILabel coinsLabel;

	// Token: 0x04000396 RID: 918
	private string _trainingMsg = "0";

	// Token: 0x04000397 RID: 919
	private bool _disposed;
}
