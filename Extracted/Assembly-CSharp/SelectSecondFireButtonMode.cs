using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000797 RID: 1943
public class SelectSecondFireButtonMode : MonoBehaviour
{
	// Token: 0x060045B9 RID: 17849 RVA: 0x00179190 File Offset: 0x00177390
	private void Start()
	{
		this.sniperModeButton.gameObject.GetComponent<ButtonHandler>().Clicked += this.HandleSniperClicked;
		this.onModeButton.gameObject.GetComponent<ButtonHandler>().Clicked += this.HandleOnClicked;
		this.offSniperModeButton.gameObject.GetComponent<ButtonHandler>().Clicked += this.HandleOffClicked;
		this.sniperModeButton.value = (Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.Sniper);
		this.onModeButton.value = (Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.On);
		this.offSniperModeButton.value = (Defs.gameSecondFireButtonMode == Defs.GameSecondFireButtonMode.Off);
	}

	// Token: 0x060045BA RID: 17850 RVA: 0x0017923C File Offset: 0x0017743C
	private void HandleSniperClicked(object sender, EventArgs e)
	{
		Defs.gameSecondFireButtonMode = Defs.GameSecondFireButtonMode.Sniper;
		PlayerPrefs.SetInt("GameSecondFireButtonMode", (int)Defs.gameSecondFireButtonMode);
	}

	// Token: 0x060045BB RID: 17851 RVA: 0x00179254 File Offset: 0x00177454
	private void HandleOnClicked(object sender, EventArgs e)
	{
		Defs.gameSecondFireButtonMode = Defs.GameSecondFireButtonMode.On;
		PlayerPrefs.SetInt("GameSecondFireButtonMode", (int)Defs.gameSecondFireButtonMode);
	}

	// Token: 0x060045BC RID: 17852 RVA: 0x0017926C File Offset: 0x0017746C
	private void HandleOffClicked(object sender, EventArgs e)
	{
		Defs.gameSecondFireButtonMode = Defs.GameSecondFireButtonMode.Off;
		PlayerPrefs.SetInt("GameSecondFireButtonMode", (int)Defs.gameSecondFireButtonMode);
	}

	// Token: 0x04003324 RID: 13092
	public UIToggle sniperModeButton;

	// Token: 0x04003325 RID: 13093
	public UIToggle onModeButton;

	// Token: 0x04003326 RID: 13094
	public UIToggle offSniperModeButton;
}
