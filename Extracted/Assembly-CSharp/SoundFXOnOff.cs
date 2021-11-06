using System;
using UnityEngine;

// Token: 0x020007D0 RID: 2000
public class SoundFXOnOff : MonoBehaviour
{
	// Token: 0x060048AA RID: 18602 RVA: 0x00193338 File Offset: 0x00191538
	private void Start()
	{
		this._isWeakdevice = Device.isWeakDevice;
		if (this._isWeakdevice && !Application.isEditor)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			this.soundFX = base.transform.GetChild(0).gameObject;
			if (Defs.isSoundFX)
			{
				this.soundFX.SetActive(true);
			}
		}
	}

	// Token: 0x060048AB RID: 18603 RVA: 0x001933A4 File Offset: 0x001915A4
	private void Update()
	{
		if (!this._isWeakdevice && this.soundFX.activeSelf != Defs.isSoundFX)
		{
			this.soundFX.SetActive(Defs.isSoundFX);
		}
	}

	// Token: 0x0400358F RID: 13711
	private GameObject soundFX;

	// Token: 0x04003590 RID: 13712
	private bool _isWeakdevice;
}
