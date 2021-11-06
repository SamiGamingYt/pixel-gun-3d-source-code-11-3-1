using System;
using UnityEngine;

// Token: 0x020007FC RID: 2044
public class KeybordHolder : MonoBehaviour
{
	// Token: 0x06004A67 RID: 19047 RVA: 0x001A6FC0 File Offset: 0x001A51C0
	private void OnClick()
	{
		if (this._tk.mKeybordHold)
		{
			this._tk.mKeybordHold = false;
		}
		else
		{
			this._tk.mKeybordHold = true;
		}
	}

	// Token: 0x04003714 RID: 14100
	public KeybordShow _tk;
}
