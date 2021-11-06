using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public class RemoveNoviceArmorController : MonoBehaviour
{
	// Token: 0x06002B93 RID: 11155 RVA: 0x000E56E4 File Offset: 0x000E38E4
	private void Awake()
	{
		this._escapeSubscription = BackSystem.Instance.Register(delegate
		{
			this.Hide();
		}, "RemoveNoviceArmorController");
		Storager.setInt("Training.ShouldRemoveNoviceArmorInShopKey", 0, false);
		ShopNGUIController.UnequipCurrentWearInCategory(ShopNGUIController.CategoryNames.ArmorCategory, false);
		ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.ArmorCategory, "Armor_Army_1", 1, false, 0, null, null, true, false, false);
	}

	// Token: 0x06002B94 RID: 11156 RVA: 0x000E5738 File Offset: 0x000E3938
	public void Hide()
	{
		base.transform.parent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x000E5754 File Offset: 0x000E3954
	private void OnDestroy()
	{
		this._escapeSubscription.Dispose();
	}

	// Token: 0x0400208C RID: 8332
	private IDisposable _escapeSubscription;
}
