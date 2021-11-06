using System;
using UnityEngine;

// Token: 0x020007BA RID: 1978
public class ShowNoJoinConnectFromRanks : MonoBehaviour
{
	// Token: 0x060047A8 RID: 18344 RVA: 0x0018C194 File Offset: 0x0018A394
	private void Start()
	{
		ShowNoJoinConnectFromRanks.sharedController = this;
	}

	// Token: 0x060047A9 RID: 18345 RVA: 0x0018C19C File Offset: 0x0018A39C
	private void Update()
	{
		if (this.showTimer > 0f)
		{
			this.showTimer -= Time.deltaTime;
			if (this.showTimer <= 0f)
			{
				this.panelMessage.SetActive(false);
			}
		}
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x0018C1E8 File Offset: 0x0018A3E8
	public void resetShow(int rank)
	{
		this.label.text = "Reach rank " + rank + "  to play this mode!";
		this.panelMessage.SetActive(true);
		this.showTimer = 3f;
	}

	// Token: 0x060047AB RID: 18347 RVA: 0x0018C224 File Offset: 0x0018A424
	private void OnDestroy()
	{
		ShowNoJoinConnectFromRanks.sharedController = null;
	}

	// Token: 0x040034D5 RID: 13525
	public float showTimer;

	// Token: 0x040034D6 RID: 13526
	public UILabel label;

	// Token: 0x040034D7 RID: 13527
	public GameObject panelMessage;

	// Token: 0x040034D8 RID: 13528
	public static ShowNoJoinConnectFromRanks sharedController;
}
