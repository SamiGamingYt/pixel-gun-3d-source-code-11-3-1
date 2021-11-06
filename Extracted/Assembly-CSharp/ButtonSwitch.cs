using System;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class ButtonSwitch : MonoBehaviour
{
	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000211 RID: 529 RVA: 0x00013604 File Offset: 0x00011804
	// (remove) Token: 0x06000212 RID: 530 RVA: 0x00013620 File Offset: 0x00011820
	public event EventHandler Clicked;

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000213 RID: 531 RVA: 0x0001363C File Offset: 0x0001183C
	private Collider collider
	{
		get
		{
			return base.gameObject.GetComponent<Collider>();
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000214 RID: 532 RVA: 0x0001364C File Offset: 0x0001184C
	public bool HasClickedHandlers
	{
		get
		{
			return this.Clicked != null;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000215 RID: 533 RVA: 0x0001365C File Offset: 0x0001185C
	// (set) Token: 0x06000216 RID: 534 RVA: 0x00013688 File Offset: 0x00011888
	private bool isEnable
	{
		get
		{
			return !(this.collider == null) && this.collider.enabled;
		}
		set
		{
			if (this.collider != null)
			{
				this.collider.enabled = value;
			}
		}
	}

	// Token: 0x06000217 RID: 535 RVA: 0x000136A8 File Offset: 0x000118A8
	private void OnClick()
	{
		if (!this.isAutomatic)
		{
			return;
		}
		if (!this.isEnable)
		{
			return;
		}
		if (ButtonClickSound.Instance != null && !this.noSound)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.Switch(false);
		EventHandler clicked = this.Clicked;
		if (clicked != null)
		{
			clicked(this, EventArgs.Empty);
		}
	}

	// Token: 0x06000218 RID: 536 RVA: 0x00013714 File Offset: 0x00011914
	public void DoClick()
	{
		if (!this.isEnable)
		{
			return;
		}
		this.OnClick();
	}

	// Token: 0x06000219 RID: 537 RVA: 0x00013728 File Offset: 0x00011928
	public void Switch(bool isEnabled)
	{
		this.enabledGo.SetActive(isEnabled);
		this.disbledGo.SetActive(!isEnabled);
		this.isEnable = isEnabled;
		if (isEnabled)
		{
			if (this.tweenOn)
			{
				this.tweenOn.ResetToBeginning();
				this.tweenOn.PlayForward();
			}
		}
		else if (this.tweenOff)
		{
			this.tweenOff.ResetToBeginning();
			this.tweenOff.PlayForward();
		}
	}

	// Token: 0x0400023F RID: 575
	[SerializeField]
	[Header("GO to show when enabled")]
	private GameObject enabledGo;

	// Token: 0x04000240 RID: 576
	[SerializeField]
	[Header("GO to show when disabled")]
	private GameObject disbledGo;

	// Token: 0x04000241 RID: 577
	[Header("Does button should play click sound?")]
	[SerializeField]
	private bool noSound;

	// Token: 0x04000242 RID: 578
	[Header("Должна ли кнопка нажиматься сама или активироваться из кода")]
	[SerializeField]
	private bool isAutomatic = true;

	// Token: 0x04000243 RID: 579
	[SerializeField]
	private UITweener tweenOn;

	// Token: 0x04000244 RID: 580
	[SerializeField]
	private UITweener tweenOff;
}
