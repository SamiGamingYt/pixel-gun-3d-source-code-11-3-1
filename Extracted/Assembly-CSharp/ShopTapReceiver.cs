using System;
using UnityEngine;

// Token: 0x020007B7 RID: 1975
public class ShopTapReceiver : MonoBehaviour
{
	// Token: 0x140000AE RID: 174
	// (add) Token: 0x06004797 RID: 18327 RVA: 0x0018BEFC File Offset: 0x0018A0FC
	// (remove) Token: 0x06004798 RID: 18328 RVA: 0x0018BF14 File Offset: 0x0018A114
	public static event Action ShopClicked;

	// Token: 0x06004799 RID: 18329 RVA: 0x0018BF2C File Offset: 0x0018A12C
	private void Start()
	{
		this.sp = base.GetComponentInChildren<UISprite>();
	}

	// Token: 0x0600479A RID: 18330 RVA: 0x0018BF3C File Offset: 0x0018A13C
	private void HandleStopBlinkShop()
	{
		base.GetComponentInChildren<UISprite>().spriteName = "green_btn";
		this.blinkShop = false;
	}

	// Token: 0x0600479B RID: 18331 RVA: 0x0018BF58 File Offset: 0x0018A158
	private void HandleStartBlinkShop()
	{
		this.blinkShop = true;
		this.lastTimeBlink = Time.realtimeSinceStartup;
	}

	// Token: 0x0600479C RID: 18332 RVA: 0x0018BF6C File Offset: 0x0018A16C
	private void Update()
	{
		if (this.blinkShop && Time.realtimeSinceStartup - this.lastTimeBlink > 0.16f)
		{
			this.lastTimeBlink = Time.realtimeSinceStartup;
			this.sp.spriteName = ((!this.sp.spriteName.Equals("green_btn")) ? "green_btn" : "green_btn_n");
		}
	}

	// Token: 0x0600479D RID: 18333 RVA: 0x0018BFDC File Offset: 0x0018A1DC
	public static void AddClickHndIfNotExist(Action handler)
	{
		if (ShopTapReceiver.ShopClicked == null || Array.IndexOf<Delegate>(ShopTapReceiver.ShopClicked.GetInvocationList(), handler) < 0)
		{
			ShopTapReceiver.ShopClicked = (Action)Delegate.Combine(ShopTapReceiver.ShopClicked, handler);
		}
	}

	// Token: 0x0600479E RID: 18334 RVA: 0x0018C014 File Offset: 0x0018A214
	private void OnClick()
	{
		if (LoadingInAfterGame.isShowLoading)
		{
			return;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		if (JoystickController.leftJoystick != null)
		{
			JoystickController.leftJoystick.Reset();
		}
		if (ShopTapReceiver.ShopClicked != null)
		{
			ShopTapReceiver.ShopClicked();
		}
		else
		{
			Debug.Log("ShopClicked == null");
		}
	}

	// Token: 0x0600479F RID: 18335 RVA: 0x0018C08C File Offset: 0x0018A28C
	private void OnDestroy()
	{
	}

	// Token: 0x040034CD RID: 13517
	private bool blinkShop;

	// Token: 0x040034CE RID: 13518
	private float lastTimeBlink;

	// Token: 0x040034CF RID: 13519
	private UISprite sp;
}
