using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class EggReadyIndicatorController : MonoBehaviour
{
	// Token: 0x06000515 RID: 1301 RVA: 0x00029A04 File Offset: 0x00027C04
	private void Update()
	{
		if (Time.realtimeSinceStartup - 0.5f < this._lastUpdateTime)
		{
			return;
		}
		bool flag = Singleton<EggsManager>.Instance.ReadyEggs().Count > 0;
		if (this.sprite != null && this.sprite.enabled != flag)
		{
			this.sprite.enabled = flag;
		}
		this._lastUpdateTime = Time.realtimeSinceStartup;
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06000516 RID: 1302 RVA: 0x00029A74 File Offset: 0x00027C74
	private UISprite sprite
	{
		get
		{
			if (this._sprite == null)
			{
				this._sprite = base.GetComponent<UISprite>();
			}
			return this._sprite;
		}
	}

	// Token: 0x0400058D RID: 1421
	private UISprite _sprite;

	// Token: 0x0400058E RID: 1422
	private float _lastUpdateTime = float.MinValue;
}
