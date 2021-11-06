using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000571 RID: 1393
public class StickerPackScroll : MonoBehaviour
{
	// Token: 0x06003052 RID: 12370 RVA: 0x000FC2EC File Offset: 0x000FA4EC
	private void Awake()
	{
		this.listButton.Clear();
		this.listButton.AddRange(base.GetComponentsInChildren<BtnPackItem>(true));
	}

	// Token: 0x06003053 RID: 12371 RVA: 0x000FC30C File Offset: 0x000FA50C
	private void OnEnable()
	{
		this.UpdateListButton();
		StickersController.onBuyPack += this.UpdateListButton;
	}

	// Token: 0x06003054 RID: 12372 RVA: 0x000FC328 File Offset: 0x000FA528
	private void OnDisable()
	{
		StickersController.onBuyPack -= this.UpdateListButton;
	}

	// Token: 0x06003055 RID: 12373 RVA: 0x000FC33C File Offset: 0x000FA53C
	public void UpdateListButton()
	{
		base.StartCoroutine(this.crtUpdateListButton());
	}

	// Token: 0x06003056 RID: 12374 RVA: 0x000FC34C File Offset: 0x000FA54C
	private IEnumerator crtUpdateListButton()
	{
		if (this.sortScript == null)
		{
			this.sortScript = this.parentButton.GetComponent<UIGrid>();
		}
		this.listItemData = StickersController.GetAvaliablePack();
		BtnPackItem fistAvaliableBtn = null;
		for (int i = 0; i < this.listButton.Count; i++)
		{
			BtnPackItem curButtonItem = this.listButton[i];
			if (this.listItemData.Contains(curButtonItem.typePack))
			{
				curButtonItem.transform.parent = this.parentButton.transform;
				curButtonItem.gameObject.SetActive(true);
				if (fistAvaliableBtn == null)
				{
					fistAvaliableBtn = curButtonItem;
				}
			}
			else
			{
				curButtonItem.transform.parent = base.transform;
				curButtonItem.gameObject.SetActive(false);
			}
		}
		if (fistAvaliableBtn != null)
		{
			this.ShowPack(fistAvaliableBtn.typePack);
		}
		yield return null;
		this.Sort();
		yield break;
	}

	// Token: 0x06003057 RID: 12375 RVA: 0x000FC368 File Offset: 0x000FA568
	public void Sort()
	{
		if (this.sortScript != null)
		{
			this.parentButton.SetActive(false);
			this.parentButton.SetActive(true);
			this.sortScript.Reposition();
		}
	}

	// Token: 0x06003058 RID: 12376 RVA: 0x000FC3AC File Offset: 0x000FA5AC
	public void ShowPack(TypePackSticker val)
	{
		for (int i = 0; i < this.listButton.Count; i++)
		{
			BtnPackItem btnPackItem = this.listButton[i];
			if (btnPackItem.typePack == val)
			{
				btnPackItem.ShowPack();
				this.curShowPack = btnPackItem.typePack;
			}
			else
			{
				btnPackItem.HidePack();
			}
		}
	}

	// Token: 0x04002384 RID: 9092
	public List<TypePackSticker> listItemData = new List<TypePackSticker>();

	// Token: 0x04002385 RID: 9093
	public List<BtnPackItem> listButton = new List<BtnPackItem>();

	// Token: 0x04002386 RID: 9094
	public GameObject parentButton;

	// Token: 0x04002387 RID: 9095
	public TypePackSticker curShowPack;

	// Token: 0x04002388 RID: 9096
	private UIGrid sortScript;
}
