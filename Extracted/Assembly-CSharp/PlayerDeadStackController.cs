using System;
using UnityEngine;

// Token: 0x0200047D RID: 1149
public class PlayerDeadStackController : MonoBehaviour
{
	// Token: 0x060027FD RID: 10237 RVA: 0x000C7D24 File Offset: 0x000C5F24
	private void Start()
	{
		PlayerDeadStackController.sharedController = this;
		Transform transform = base.transform;
		transform.position = Vector3.zero;
		this.playerDeads = new PlayerDeadController[10];
		for (int i = 0; i < this.playerDeads.Length; i++)
		{
			GameObject gameObject;
			if (Device.isPixelGunLow)
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerDeadObjectLow);
			}
			else
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.playerDeadObject);
			}
			gameObject.transform.parent = base.transform;
			this.playerDeads[i] = gameObject.GetComponent<PlayerDeadController>();
		}
		UnityEngine.Object.Destroy(this.playerDeadObjectLow);
		UnityEngine.Object.Destroy(this.playerDeadObject);
	}

	// Token: 0x060027FE RID: 10238 RVA: 0x000C7DCC File Offset: 0x000C5FCC
	public PlayerDeadController GetCurrentParticle(bool _isUseMine)
	{
		bool flag = true;
		for (;;)
		{
			this.currentIndexHole++;
			if (this.currentIndexHole >= this.playerDeads.Length)
			{
				if (!flag)
				{
					break;
				}
				this.currentIndexHole = 0;
				flag = false;
			}
			if (!this.playerDeads[this.currentIndexHole].isUseMine || _isUseMine)
			{
				goto IL_51;
			}
		}
		return null;
		IL_51:
		return this.playerDeads[this.currentIndexHole];
	}

	// Token: 0x060027FF RID: 10239 RVA: 0x000C7E38 File Offset: 0x000C6038
	private void OnDestroy()
	{
		PlayerDeadStackController.sharedController = null;
	}

	// Token: 0x04001C45 RID: 7237
	public static PlayerDeadStackController sharedController;

	// Token: 0x04001C46 RID: 7238
	public PlayerDeadController[] playerDeads;

	// Token: 0x04001C47 RID: 7239
	public GameObject playerDeadObject;

	// Token: 0x04001C48 RID: 7240
	public GameObject playerDeadObjectLow;

	// Token: 0x04001C49 RID: 7241
	private int currentIndexHole;
}
