using System;
using System.Text;
using UnityEngine;

// Token: 0x020005E2 RID: 1506
public class EditorShopItem : MonoBehaviour
{
	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x060033A4 RID: 13220 RVA: 0x0010B5C8 File Offset: 0x001097C8
	// (set) Token: 0x060033A3 RID: 13219 RVA: 0x0010B5BC File Offset: 0x001097BC
	public string prefabName { get; private set; }

	// Token: 0x060033A5 RID: 13221 RVA: 0x0010B5D0 File Offset: 0x001097D0
	public void SetData(EditorShopItemData data)
	{
		this._itemData = data;
		StringBuilder stringBuilder = new StringBuilder();
		string byDefault = LocalizationStore.GetByDefault(data.localizeKey);
		stringBuilder.AppendLine(string.Format("Name: {0}", byDefault));
		if (!string.IsNullOrEmpty(data.prefabName))
		{
			this.prefabName = data.prefabName;
			stringBuilder.AppendLine(string.Format("Prefab: {0}", data.prefabName));
		}
		stringBuilder.Append(string.Format("Tag: {0}", data.tag));
		this.itemName.text = stringBuilder.ToString();
		this.topCheckbox.value = data.isTop;
		this.newCheckbox.value = data.isNew;
		this.discountInput.label.text = data.discount.ToString();
	}

	// Token: 0x060033A6 RID: 13222 RVA: 0x0010B6A0 File Offset: 0x001098A0
	public void SetTopState()
	{
		this._itemData.isTop = this.topCheckbox.value;
	}

	// Token: 0x060033A7 RID: 13223 RVA: 0x0010B6B8 File Offset: 0x001098B8
	public void SetNewState()
	{
		this._itemData.isNew = this.newCheckbox.value;
	}

	// Token: 0x060033A8 RID: 13224 RVA: 0x0010B6D0 File Offset: 0x001098D0
	public void SetDiscount()
	{
		int.TryParse(this.discountInput.label.text, out this._itemData.discount);
	}

	// Token: 0x040025F2 RID: 9714
	public UILabel itemName;

	// Token: 0x040025F3 RID: 9715
	public UIToggle topCheckbox;

	// Token: 0x040025F4 RID: 9716
	public UIToggle newCheckbox;

	// Token: 0x040025F5 RID: 9717
	public UIInput discountInput;

	// Token: 0x040025F6 RID: 9718
	private EditorShopItemData _itemData;
}
