using System;
using System.Linq;
using UnityEngine;

// Token: 0x0200058F RID: 1423
public class EnemyPortalStackController : MonoBehaviour
{
	// Token: 0x06003194 RID: 12692 RVA: 0x00102334 File Offset: 0x00100534
	private void Awake()
	{
		EnemyPortalStackController.sharedController = this;
	}

	// Token: 0x06003195 RID: 12693 RVA: 0x0010233C File Offset: 0x0010053C
	private void Start()
	{
		if (Device.isPixelGunLow)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003196 RID: 12694 RVA: 0x00102354 File Offset: 0x00100554
	public EnemyPortal GetPortal()
	{
		if (this._portals == null || !this._portals.Any<EnemyPortal>())
		{
			this.SetPortals();
		}
		this.currentIndex++;
		if (this.currentIndex >= this._portals.Length)
		{
			this.currentIndex = 0;
		}
		return this._portals[this.currentIndex];
	}

	// Token: 0x06003197 RID: 12695 RVA: 0x001023B8 File Offset: 0x001005B8
	private void SetPortals()
	{
		this._portals = base.GetComponentsInChildren<EnemyPortal>(true);
		foreach (EnemyPortal enemyPortal in this._portals)
		{
			if (enemyPortal.gameObject.activeInHierarchy)
			{
				enemyPortal.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x04002487 RID: 9351
	public static EnemyPortalStackController sharedController;

	// Token: 0x04002488 RID: 9352
	[SerializeField]
	[ReadOnly]
	private EnemyPortal[] _portals;

	// Token: 0x04002489 RID: 9353
	private int currentIndex;
}
