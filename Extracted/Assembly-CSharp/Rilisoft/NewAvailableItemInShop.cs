using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006C9 RID: 1737
	public class NewAvailableItemInShop : MonoBehaviour
	{
		// Token: 0x06003C7B RID: 15483 RVA: 0x0013A1A0 File Offset: 0x001383A0
		private void OnClick()
		{
			LevelUpWithOffers componentInParent = base.GetComponentInParent<LevelUpWithOffers>();
			if (componentInParent != null)
			{
				ExpController.Instance.HandleNewAvailableItem(componentInParent.gameObject, this);
			}
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x0013A1D4 File Offset: 0x001383D4
		[ContextMenu("Set ref")]
		private void SetRef()
		{
			this.itemImage = base.GetComponentsInChildren<UITexture>()[1];
			this.itemName.Clear();
			this.itemName.AddRange(base.GetComponentsInChildren<UILabel>(true));
		}

		// Token: 0x04002CA6 RID: 11430
		public string _tag = string.Empty;

		// Token: 0x04002CA7 RID: 11431
		public ShopNGUIController.CategoryNames category;

		// Token: 0x04002CA8 RID: 11432
		public UITexture itemImage;

		// Token: 0x04002CA9 RID: 11433
		public List<UILabel> itemName;
	}
}
