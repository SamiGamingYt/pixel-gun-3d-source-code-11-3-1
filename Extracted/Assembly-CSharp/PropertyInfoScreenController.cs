using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public class PropertyInfoScreenController : MonoBehaviour
{
	// Token: 0x06002B12 RID: 11026 RVA: 0x000E2C58 File Offset: 0x000E0E58
	public virtual void Show(bool isMelee)
	{
		base.gameObject.SetActive(true);
		((!isMelee) ? this.description : this.descriptionMelee).SetActive(true);
		((!isMelee) ? this.descriptionMelee : this.description).SetActive(false);
	}

	// Token: 0x06002B13 RID: 11027 RVA: 0x000E2CAC File Offset: 0x000E0EAC
	public virtual void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x000E2CBC File Offset: 0x000E0EBC
	private void OnEnable()
	{
		if (this._escapeSubscription != null)
		{
			this._escapeSubscription.Dispose();
		}
		this._escapeSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "Property Info");
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x000E2CF8 File Offset: 0x000E0EF8
	private void OnDisable()
	{
		if (this._escapeSubscription != null)
		{
			this._escapeSubscription.Dispose();
			this._escapeSubscription = null;
		}
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x000E2D18 File Offset: 0x000E0F18
	private void HandleEscape()
	{
		this.Hide();
	}

	// Token: 0x04002026 RID: 8230
	public GameObject description;

	// Token: 0x04002027 RID: 8231
	public GameObject descriptionMelee;

	// Token: 0x04002028 RID: 8232
	private IDisposable _escapeSubscription;
}
