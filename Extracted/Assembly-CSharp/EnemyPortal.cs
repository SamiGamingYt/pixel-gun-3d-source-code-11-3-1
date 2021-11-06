using System;
using UnityEngine;

// Token: 0x0200058E RID: 1422
public class EnemyPortal : MonoBehaviour
{
	// Token: 0x14000047 RID: 71
	// (add) Token: 0x0600318D RID: 12685 RVA: 0x00102278 File Offset: 0x00100478
	// (remove) Token: 0x0600318E RID: 12686 RVA: 0x00102294 File Offset: 0x00100494
	public event Action OnHided = delegate()
	{
	};

	// Token: 0x0600318F RID: 12687 RVA: 0x001022B0 File Offset: 0x001004B0
	public void OnAnimationOff()
	{
		this.ChangeVisibleState(false, this.OnHided);
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x001022C0 File Offset: 0x001004C0
	public void Show(Vector3 position)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(position, Vector3.down, out raycastHit))
		{
			Debug.DrawLine(position, raycastHit.point, Color.blue);
			base.transform.position = raycastHit.point;
		}
		this.ChangeVisibleState(true, null);
	}

	// Token: 0x06003191 RID: 12689 RVA: 0x0010230C File Offset: 0x0010050C
	private void ChangeVisibleState(bool state, Action onComplete = null)
	{
		base.gameObject.SetActive(state);
		if (onComplete != null)
		{
			onComplete();
		}
	}
}
