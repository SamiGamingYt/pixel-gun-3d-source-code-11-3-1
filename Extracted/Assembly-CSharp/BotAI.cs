using System;
using UnityEngine;

// Token: 0x0200057B RID: 1403
[RequireComponent(typeof(BotMovement))]
public class BotAI : MonoBehaviour
{
	// Token: 0x06003099 RID: 12441 RVA: 0x000FCF54 File Offset: 0x000FB154
	private void Awake()
	{
		if (Defs.isCOOP)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600309A RID: 12442 RVA: 0x000FCF68 File Offset: 0x000FB168
	private void Start()
	{
		this.Target = null;
		this._motor = base.GetComponent<BotMovement>();
		this._eh = base.GetComponent<BotHealth>();
		this.myTransform = base.transform;
		this._botTrigger = base.GetComponent<BotTrigger>();
	}

	// Token: 0x0600309B RID: 12443 RVA: 0x000FCFAC File Offset: 0x000FB1AC
	private void Update()
	{
		bool isLife = this._eh.getIsLife();
		if (!isLife)
		{
			if (!this.deaded)
			{
				base.SendMessage("Death");
				this._botTrigger.shouldDetectPlayer = false;
				this.deaded = true;
			}
		}
	}

	// Token: 0x0600309C RID: 12444 RVA: 0x000FCFFC File Offset: 0x000FB1FC
	public void SetTarget(Transform _tgt, bool agression)
	{
		this.Agression = agression;
		this.Target = _tgt;
		this._motor.SetTarget(this.Target, agression);
	}

	// Token: 0x040023AD RID: 9133
	private bool Agression;

	// Token: 0x040023AE RID: 9134
	private bool deaded;

	// Token: 0x040023AF RID: 9135
	public Transform Target;

	// Token: 0x040023B0 RID: 9136
	private Transform myTransform;

	// Token: 0x040023B1 RID: 9137
	private BotMovement _motor;

	// Token: 0x040023B2 RID: 9138
	private BotHealth _eh;

	// Token: 0x040023B3 RID: 9139
	private BotTrigger _botTrigger;

	// Token: 0x040023B4 RID: 9140
	public Transform homePoint;
}
