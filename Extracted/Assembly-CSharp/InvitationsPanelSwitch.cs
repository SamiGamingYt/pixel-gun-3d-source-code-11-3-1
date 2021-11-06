using System;
using UnityEngine;

// Token: 0x020002D4 RID: 724
public class InvitationsPanelSwitch : MonoBehaviour
{
	// Token: 0x06001965 RID: 6501 RVA: 0x00065110 File Offset: 0x00063310
	private void OnPress(bool isPress)
	{
		Debug.Log("press " + isPress);
		if (isPress)
		{
			base.GetComponent<UISprite>().spriteName = "trans_btn_n";
		}
		else
		{
			base.GetComponent<UISprite>().spriteName = "trans_btn";
		}
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x00065160 File Offset: 0x00063360
	private void OnClick()
	{
		Debug.Log("OnClick");
		if (this.left)
		{
			this.leftPanel.SetActive(true);
			this.MiddlePanel.SetActive(false);
			this.rightPanel.SetActive(false);
		}
		else if (this.Middle)
		{
			this.leftPanel.SetActive(false);
			this.MiddlePanel.SetActive(true);
			this.rightPanel.SetActive(false);
		}
		else
		{
			this.leftPanel.SetActive(false);
			this.MiddlePanel.SetActive(false);
			this.rightPanel.SetActive(true);
		}
		base.GetComponent<UIButton>().enabled = false;
		this.butt.GetComponent<UILabel>().gameObject.SetActive(false);
		this.chekmark.SetActive(true);
		base.GetComponent<UISprite>().spriteName = "trans_btn_n";
		foreach (GameObject gameObject in this.anotherButtons)
		{
			gameObject.SetActive(true);
		}
		foreach (GameObject gameObject2 in this.anotherToggles)
		{
			gameObject2.GetComponent<UIButton>().enabled = true;
			gameObject2.GetComponent<UISprite>().spriteName = "trans_btn";
		}
		foreach (GameObject gameObject3 in this.anotherChekmarks)
		{
			gameObject3.SetActive(false);
		}
		ButtonClickSound.Instance.PlayClick();
	}

	// Token: 0x04000E7F RID: 3711
	public bool left = true;

	// Token: 0x04000E80 RID: 3712
	public bool Middle;

	// Token: 0x04000E81 RID: 3713
	public GameObject leftPanel;

	// Token: 0x04000E82 RID: 3714
	public GameObject MiddlePanel;

	// Token: 0x04000E83 RID: 3715
	public GameObject rightPanel;

	// Token: 0x04000E84 RID: 3716
	public GameObject[] anotherButtons;

	// Token: 0x04000E85 RID: 3717
	public GameObject[] anotherChekmarks;

	// Token: 0x04000E86 RID: 3718
	public GameObject chekmark;

	// Token: 0x04000E87 RID: 3719
	public GameObject butt;

	// Token: 0x04000E88 RID: 3720
	public GameObject[] anotherToggles;
}
