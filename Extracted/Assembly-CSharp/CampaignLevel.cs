using System;
using UnityEngine;

// Token: 0x020005B2 RID: 1458
public sealed class CampaignLevel
{
	// Token: 0x06003268 RID: 12904 RVA: 0x00105A2C File Offset: 0x00103C2C
	public CampaignLevel(string sceneName, string keyForLevelMap, string pr = "in")
	{
		this._sceneName = sceneName;
		this._localizeKeyForLevelMap = keyForLevelMap;
		this.predlog = pr;
	}

	// Token: 0x06003269 RID: 12905 RVA: 0x00105A60 File Offset: 0x00103C60
	public CampaignLevel()
	{
		this._sceneName = string.Empty;
	}

	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x0600326A RID: 12906 RVA: 0x00105A8C File Offset: 0x00103C8C
	// (set) Token: 0x0600326B RID: 12907 RVA: 0x00105A94 File Offset: 0x00103C94
	public string sceneName
	{
		get
		{
			return this._sceneName;
		}
		set
		{
			this._sceneName = value;
		}
	}

	// Token: 0x1700086F RID: 2159
	// (get) Token: 0x0600326C RID: 12908 RVA: 0x00105AA0 File Offset: 0x00103CA0
	// (set) Token: 0x0600326D RID: 12909 RVA: 0x00105AA8 File Offset: 0x00103CA8
	public string localizeKeyForLevelMap
	{
		get
		{
			return this._localizeKeyForLevelMap;
		}
		set
		{
			this._localizeKeyForLevelMap = value;
		}
	}

	// Token: 0x17000870 RID: 2160
	// (get) Token: 0x0600326E RID: 12910 RVA: 0x00105AB4 File Offset: 0x00103CB4
	// (set) Token: 0x0600326F RID: 12911 RVA: 0x00105ABC File Offset: 0x00103CBC
	public string predlog { get; set; }

	// Token: 0x17000871 RID: 2161
	// (get) Token: 0x06003270 RID: 12912 RVA: 0x00105AC8 File Offset: 0x00103CC8
	// (set) Token: 0x06003271 RID: 12913 RVA: 0x00105AD0 File Offset: 0x00103CD0
	public Vector3 LocalPosition
	{
		get
		{
			return this._localPosition;
		}
		set
		{
			this._localPosition = value;
		}
	}

	// Token: 0x04002511 RID: 9489
	public float timeToComplete = 300f;

	// Token: 0x04002512 RID: 9490
	private Vector3 _localPosition = Vector3.forward;

	// Token: 0x04002513 RID: 9491
	private string _sceneName;

	// Token: 0x04002514 RID: 9492
	private string _localizeKeyForLevelMap;
}
