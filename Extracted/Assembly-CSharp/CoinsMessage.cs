using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020005C2 RID: 1474
public sealed class CoinsMessage : MonoBehaviour
{
	// Token: 0x1400004B RID: 75
	// (add) Token: 0x060032F3 RID: 13043 RVA: 0x00107CD4 File Offset: 0x00105ED4
	// (remove) Token: 0x060032F4 RID: 13044 RVA: 0x00107CEC File Offset: 0x00105EEC
	public static event CoinsMessage.CoinsLabelDisappearedDelegate CoinsLabelDisappeared;

	// Token: 0x060032F5 RID: 13045 RVA: 0x00107D04 File Offset: 0x00105F04
	public static void FireCoinsAddedEvent(bool isGems = false, int count = 2)
	{
		if (CoinsMessage.CoinsLabelDisappeared != null)
		{
			CoinsMessage.CoinsLabelDisappeared(isGems, count);
		}
	}

	// Token: 0x060032F6 RID: 13046 RVA: 0x00107D1C File Offset: 0x00105F1C
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.coinsToShow = Storager.getInt(Defs.EarnedCoins, false);
		Storager.setInt(Defs.EarnedCoins, 0, false);
		if (this.coinsToShow > 1)
		{
			this.plashka = Resources.Load<Texture>(ResPath.Combine("CoinsIndicationSystem", "got_prize"));
		}
		else
		{
			this.plashka = Resources.Load<Texture>(ResPath.Combine("CoinsIndicationSystem", "got_coin"));
		}
		this.startTime = (double)Time.realtimeSinceStartup;
	}

	// Token: 0x060032F7 RID: 13047 RVA: 0x00107DA4 File Offset: 0x00105FA4
	private void Remove()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002571 RID: 9585
	public GUIStyle labelStyle;

	// Token: 0x04002572 RID: 9586
	public Rect rect = Tools.SuccessMessageRect();

	// Token: 0x04002573 RID: 9587
	public string message = "Purchases restored";

	// Token: 0x04002574 RID: 9588
	public Texture texture;

	// Token: 0x04002575 RID: 9589
	public int depth = -2;

	// Token: 0x04002576 RID: 9590
	public bool singleMessage;

	// Token: 0x04002577 RID: 9591
	public Texture youveGotCoin;

	// Token: 0x04002578 RID: 9592
	public Texture passNextLevels;

	// Token: 0x04002579 RID: 9593
	private int coinsToShow;

	// Token: 0x0400257A RID: 9594
	private int coinsForNextLevels;

	// Token: 0x0400257B RID: 9595
	private double startTime;

	// Token: 0x0400257C RID: 9596
	private float _time = 2f;

	// Token: 0x0400257D RID: 9597
	public Texture plashka;

	// Token: 0x02000921 RID: 2337
	// (Invoke) Token: 0x06005124 RID: 20772
	public delegate void CoinsLabelDisappearedDelegate(bool isGems, int count);
}
