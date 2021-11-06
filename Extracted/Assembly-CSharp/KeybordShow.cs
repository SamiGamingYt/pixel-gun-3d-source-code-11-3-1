using System;
using UnityEngine;

// Token: 0x020007FD RID: 2045
public class KeybordShow : MonoBehaviour
{
	// Token: 0x06004A69 RID: 19049 RVA: 0x001A7020 File Offset: 0x001A5220
	private void Start()
	{
		this._Uil = this.CF.GetComponent<UILabel>();
		Vector3 position = this.CF.transform.position;
		this.CF.transform.position = new Vector3(posNGUI.getPosX(0f), posNGUI.getPosY(0f), position.z);
		this._Uil.lineWidth = Screen.width;
		this.mKeyboard = TouchScreenKeyboard.Open(string.Empty, TouchScreenKeyboardType.Default, false, false);
	}

	// Token: 0x06004A6A RID: 19050 RVA: 0x001A70A4 File Offset: 0x001A52A4
	private void Update()
	{
		if (this.mKeyboard != null)
		{
			string text = this.mKeyboard.text;
			if (this.mText != text)
			{
				this.mText = string.Empty;
				foreach (char c in text)
				{
					if (c != '\0')
					{
						this.mText += c;
					}
				}
				if (this.maxChars > 0 && this.mKeyboard.text.Length > this.maxChars)
				{
					this.mKeyboard.text = this.mKeyboard.text.Substring(0, this.maxChars);
				}
				if (this.mText != text)
				{
					this.mKeyboard.text = this.mText;
				}
				base.SendMessage("OnInputChanged", this, SendMessageOptions.DontRequireReceiver);
			}
			this.mKeyboard.active = true;
			if (this.mKeyboard.done)
			{
				this.mKeyboard.active = true;
				if (string.IsNullOrEmpty(this.mText))
				{
					this._Uil.text = this.mText + '\n' + this._Uil.text;
					this.mText = string.Empty;
				}
				if (!this.mKeybordHold)
				{
					this.mKeyboard.active = false;
					this.mKeyboard = null;
				}
			}
			else if (this.mKeyboard.wasCanceled)
			{
				this.mKeyboard.active = false;
				this.mKeyboard = null;
			}
		}
	}

	// Token: 0x06004A6B RID: 19051 RVA: 0x001A724C File Offset: 0x001A544C
	private void OnClick()
	{
		this.mKeyboard = TouchScreenKeyboard.Open(this.mText, TouchScreenKeyboardType.Default, false);
	}

	// Token: 0x04003715 RID: 14101
	private TouchScreenKeyboard mKeyboard;

	// Token: 0x04003716 RID: 14102
	public bool mKeybordHold = true;

	// Token: 0x04003717 RID: 14103
	public int maxChars = 20;

	// Token: 0x04003718 RID: 14104
	public GameObject CF;

	// Token: 0x04003719 RID: 14105
	public UILabel _Uil;

	// Token: 0x0400371A RID: 14106
	private string mText = string.Empty;
}
