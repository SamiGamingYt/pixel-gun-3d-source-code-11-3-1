using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000159 RID: 345
public class GeneralBannerWindow : MonoBehaviour
{
	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000B60 RID: 2912 RVA: 0x000404F8 File Offset: 0x0003E6F8
	// (set) Token: 0x06000B61 RID: 2913 RVA: 0x00040500 File Offset: 0x0003E700
	public Action OnCloseCustomAction { get; set; }

	// Token: 0x06000B62 RID: 2914 RVA: 0x0004050C File Offset: 0x0003E70C
	public virtual void HandleClose()
	{
		this.DestroyScreen();
		Action onCloseCustomAction = this.OnCloseCustomAction;
		if (onCloseCustomAction != null)
		{
			onCloseCustomAction();
		}
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00040534 File Offset: 0x0003E734
	private void DestroyScreen()
	{
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x00040550 File Offset: 0x0003E750
	protected virtual void RegisterEscapeHandler()
	{
		this._escapeSubscription = BackSystem.Instance.Register(delegate
		{
			this.OnHardwareBackPressed();
		}, string.Format("{0} Controller", base.gameObject.name));
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x00040590 File Offset: 0x0003E790
	protected virtual void UnregisterEscapeHandler()
	{
		this._escapeSubscription.Dispose();
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x000405A0 File Offset: 0x0003E7A0
	protected virtual void OnHardwareBackPressed()
	{
		this.HandleClose();
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x000405A8 File Offset: 0x0003E7A8
	private void Start()
	{
		this.RegisterEscapeHandler();
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x000405B0 File Offset: 0x0003E7B0
	private void OnDestroy()
	{
		this.UnregisterEscapeHandler();
	}

	// Token: 0x0400090F RID: 2319
	private IDisposable _escapeSubscription;
}
