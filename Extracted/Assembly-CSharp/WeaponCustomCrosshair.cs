using System;
using System.Linq;
using UnityEngine;

// Token: 0x0200080E RID: 2062
public class WeaponCustomCrosshair : MonoBehaviour
{
	// Token: 0x17000C47 RID: 3143
	// (get) Token: 0x06004B3C RID: 19260 RVA: 0x001ADC8C File Offset: 0x001ABE8C
	// (set) Token: 0x06004B3D RID: 19261 RVA: 0x001ADC94 File Offset: 0x001ABE94
	public int DataId
	{
		get
		{
			return this._dataId;
		}
		set
		{
			this._dataId = value;
			this._data = null;
		}
	}

	// Token: 0x17000C48 RID: 3144
	// (get) Token: 0x06004B3E RID: 19262 RVA: 0x001ADCA4 File Offset: 0x001ABEA4
	public CrosshairData Data
	{
		get
		{
			if (this._data != null)
			{
				return this._data;
			}
			if (WeaponCustomCrosshair.So == null)
			{
				return this._data = new CrosshairData();
			}
			this._data = WeaponCustomCrosshair.So.Crosshairs.FirstOrDefault((CrosshairData c) => c.ID == this.DataId);
			CrosshairData result;
			if ((result = this._data) == null)
			{
				result = (this._data = new CrosshairData());
			}
			return result;
		}
	}

	// Token: 0x17000C49 RID: 3145
	// (get) Token: 0x06004B3F RID: 19263 RVA: 0x001ADD20 File Offset: 0x001ABF20
	public static CrosshairsSo So
	{
		get
		{
			CrosshairsSo result;
			if ((result = WeaponCustomCrosshair._so) == null)
			{
				result = (WeaponCustomCrosshair._so = Resources.Load<CrosshairsSo>("Common/crosshairs"));
			}
			return result;
		}
	}

	// Token: 0x040037D2 RID: 14290
	public const string ASSET_PATH = "Common/crosshairs";

	// Token: 0x040037D3 RID: 14291
	[SerializeField]
	[ReadOnly]
	private int _dataId;

	// Token: 0x040037D4 RID: 14292
	private CrosshairData _data;

	// Token: 0x040037D5 RID: 14293
	private static CrosshairsSo _so;
}
