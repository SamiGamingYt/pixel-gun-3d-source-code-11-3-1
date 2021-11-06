using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x02000311 RID: 785
public class MyUIInput : UIInput
{
	// Token: 0x06001B6A RID: 7018 RVA: 0x00070618 File Offset: 0x0006E818
	private void Awake()
	{
		this.hideInput = false;
	}

	// Token: 0x06001B6B RID: 7019 RVA: 0x00070624 File Offset: 0x0006E824
	protected override void OnSelect(bool isSelected)
	{
		if (isSelected)
		{
			base.OnSelectEvent();
		}
		else if (!this.hideInput)
		{
			base.OnDeselectEvent();
		}
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x00070654 File Offset: 0x0006E854
	public void DeselectInput()
	{
		this.OnDeselectEventCustom();
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x0007065C File Offset: 0x0006E85C
	protected void OnDeselectEventCustom()
	{
		if (this.mDoInit)
		{
			base.Init();
		}
		if (UIInput.mKeyboard != null)
		{
			UIInput.mKeyboard.active = false;
			UIInput.mKeyboard = null;
		}
		if (this.label != null)
		{
			this.mValue = base.value;
			if (string.IsNullOrEmpty(this.mValue))
			{
				this.label.text = this.mDefaultText;
				this.label.color = this.mDefaultColor;
			}
			else
			{
				this.label.text = this.mValue;
			}
			Input.imeCompositionMode = IMECompositionMode.Auto;
			this.label.alignment = this.mAlignment;
		}
		base.isSelected = false;
		UIInput.selection = null;
		base.UpdateLabel();
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x00070724 File Offset: 0x0006E924
	private new void Update()
	{
		if (Application.isEditor)
		{
			if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown("enter")) && this.onKeyboardInter != null)
			{
				this.onKeyboardInter();
			}
			if (Input.GetKeyDown(KeyCode.KeypadPlus) && this.onKeyboardVisible != null)
			{
				this.onKeyboardVisible();
			}
			if (Input.GetKeyDown(KeyCode.KeypadMinus))
			{
				if (this.onKeyboardHide != null)
				{
					this.onKeyboardHide();
				}
				this.DeselectInput();
			}
		}
		base.Update();
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x000707C8 File Offset: 0x0006E9C8
	public float GetKeyboardHeight()
	{
		return this.heightKeyboard;
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x000707D0 File Offset: 0x0006E9D0
	private void SetKeyboardHeight()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView", new object[0]);
			using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("android.graphics.Rect", new object[0]))
			{
				androidJavaObject.Call("getWindowVisibleDisplayFrame", new object[]
				{
					androidJavaObject2
				});
				this.heightKeyboard = (float)(Screen.height - androidJavaObject2.Call<int>("height", new object[0]));
			}
		}
	}

	// Token: 0x06001B71 RID: 7025 RVA: 0x000708B0 File Offset: 0x0006EAB0
	private void OnDestroy()
	{
		base.OnSelect(false);
		this.DeselectInput();
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x000708C0 File Offset: 0x0006EAC0
	private void OnEnable()
	{
		DeviceOrientationMonitor.OnOrientationChange += this.OnDeviceOrientationChanged;
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x000708D4 File Offset: 0x0006EAD4
	private void OnDisable()
	{
		DeviceOrientationMonitor.OnOrientationChange -= this.OnDeviceOrientationChanged;
		base.OnSelect(false);
		this.DeselectInput();
		base.Cleanup();
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x00070908 File Offset: 0x0006EB08
	private void OnDeviceOrientationChanged(DeviceOrientation ori)
	{
		if (base.isSelected)
		{
			base.StartCoroutine(this.ReSelect());
		}
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x00070924 File Offset: 0x0006EB24
	private IEnumerator ReSelect()
	{
		this.DeselectInput();
		yield return new WaitForRealSeconds(0.3f);
		base.isSelected = true;
		yield break;
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x00070940 File Offset: 0x0006EB40
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			this._selectAfterPause = base.isSelected;
			if (base.isSelected)
			{
				base.isSelected = false;
			}
		}
		else
		{
			if (this._selectAfterPause)
			{
				base.isSelected = true;
			}
			this._selectAfterPause = false;
		}
	}

	// Token: 0x0400108B RID: 4235
	[NonSerialized]
	public float heightKeyboard;

	// Token: 0x0400108C RID: 4236
	public Action onKeyboardInter;

	// Token: 0x0400108D RID: 4237
	public Action onKeyboardCancel;

	// Token: 0x0400108E RID: 4238
	public Action onKeyboardVisible;

	// Token: 0x0400108F RID: 4239
	public Action onKeyboardHide;

	// Token: 0x04001090 RID: 4240
	private bool isKeyboardVisible;

	// Token: 0x04001091 RID: 4241
	private float timerlog = 0.3f;

	// Token: 0x04001092 RID: 4242
	private bool _selectAfterPause;
}
