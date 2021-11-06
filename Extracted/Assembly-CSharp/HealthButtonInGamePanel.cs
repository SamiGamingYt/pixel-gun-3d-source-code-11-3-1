using System;
using UnityEngine;

// Token: 0x02000297 RID: 663
public sealed class HealthButtonInGamePanel : MonoBehaviour
{
	// Token: 0x06001511 RID: 5393 RVA: 0x00053664 File Offset: 0x00051864
	private void Start()
	{
		this.priceLabel.text = Defs.healthInGamePanelPrice.ToString();
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x0005367C File Offset: 0x0005187C
	private void Update()
	{
		this.UpdateState(true);
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x00053688 File Offset: 0x00051888
	private void UpdateState(bool isDelta = true)
	{
		if (this.inGameGui.playerMoveC == null)
		{
			return;
		}
		bool flag = this.inGameGui.playerMoveC.CurHealth == this.inGameGui.playerMoveC.MaxHealth;
		if (this.fullLabel.activeSelf != flag)
		{
			this.fullLabel.SetActive(flag);
		}
		this.myButton.isEnabled = !flag;
		if (this.priceLabel.gameObject.activeSelf != !flag)
		{
			this.priceLabel.gameObject.SetActive(!flag);
		}
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x00053728 File Offset: 0x00051928
	private void OnEnable()
	{
		this.UpdateState(false);
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x00053734 File Offset: 0x00051934
	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			if (this.inGameGui.playerMoveC != null)
			{
				this.inGameGui.playerMoveC.CurHealth = this.inGameGui.playerMoveC.MaxHealth;
			}
			return;
		}
		if (this.inGameGui.playerMoveC.CurHealth <= 0f)
		{
			return;
		}
		ShopNGUIController.TryToBuy(this.inGameGui.gameObject, new ItemPrice(Defs.healthInGamePanelPrice, "Coins"), delegate
		{
			if (this.inGameGui.playerMoveC != null)
			{
				this.inGameGui.playerMoveC.CurHealth = this.inGameGui.playerMoveC.MaxHealth;
				this.inGameGui.playerMoveC.ShowBonuseParticle(Player_move_c.TypeBonuses.Health);
				this.inGameGui.playerMoveC.timeBuyHealth = Time.time;
			}
		}, new Action(JoystickController.leftJoystick.Reset), null, null, null, null);
	}

	// Token: 0x04000C4F RID: 3151
	public GameObject fullLabel;

	// Token: 0x04000C50 RID: 3152
	public UIButton myButton;

	// Token: 0x04000C51 RID: 3153
	public UILabel priceLabel;

	// Token: 0x04000C52 RID: 3154
	public InGameGUI inGameGui;
}
