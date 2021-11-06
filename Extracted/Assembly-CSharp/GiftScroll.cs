using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200064E RID: 1614
public class GiftScroll : MonoBehaviour
{
	// Token: 0x06003810 RID: 14352 RVA: 0x00121BE4 File Offset: 0x0011FDE4
	private void Awake()
	{
		if (this.exampleBut)
		{
			this.exampleBut.gameObject.SetActive(false);
		}
		this.scView = base.GetComponentInParent<UIScrollView>();
	}

	// Token: 0x06003811 RID: 14353 RVA: 0x00121C20 File Offset: 0x0011FE20
	private void OnEnable()
	{
		GiftController.OnChangeSlots += this.UpdateListButton;
		this.UpdateListButton();
	}

	// Token: 0x06003812 RID: 14354 RVA: 0x00121C3C File Offset: 0x0011FE3C
	private void OnDisable()
	{
		GiftController.OnChangeSlots -= this.UpdateListButton;
	}

	// Token: 0x06003813 RID: 14355 RVA: 0x00121C50 File Offset: 0x0011FE50
	public void UpdateListButton()
	{
		if (GiftScroll.canReCreateSlots && base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.crtUpdateListButton());
		}
	}

	// Token: 0x06003814 RID: 14356 RVA: 0x00121C84 File Offset: 0x0011FE84
	private IEnumerator crtUpdateListButton()
	{
		while (GiftBannerWindow.instance == null)
		{
			yield return null;
		}
		if (this.wrapScript == null)
		{
			this.wrapScript = this.parentButton.GetComponent<UIWrapContent>();
		}
		this.listItemData = GiftController.Instance.Slots;
		this.SetButtonCount(this.listItemData.Count);
		for (int i = 0; i < this.listButton.Count; i++)
		{
			GiftHUDItem curButtonRoom = this.listButton[i];
			curButtonRoom.gameObject.name = i.ToString() + "_" + this.listItemData[i].gift.Id;
			curButtonRoom.SetInfoButton(this.listItemData[i]);
		}
		this.Sort();
		yield break;
	}

	// Token: 0x06003815 RID: 14357 RVA: 0x00121CA0 File Offset: 0x0011FEA0
	public void Sort()
	{
		if (GiftScroll.canReCreateSlots)
		{
			base.StartCoroutine(this.CrtSort());
		}
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x00121CBC File Offset: 0x0011FEBC
	private IEnumerator CrtSort()
	{
		yield return null;
		GiftBannerWindow.instance.UpdateSizeScroll();
		this.scView.ResetPosition();
		if (this.wrapScript != null)
		{
			this.wrapScript.SortAlphabetically();
		}
		if (this.wrapScript != null)
		{
			this.wrapScript.WrapContent();
		}
		this.scView.restrictWithinPanel = true;
		yield return null;
		this.scView.disableDragIfFits = false;
		this.listButton[0].InCenter(false, 1);
		yield break;
	}

	// Token: 0x06003817 RID: 14359 RVA: 0x00121CD8 File Offset: 0x0011FED8
	private void SetButtonCount(int needCount)
	{
		if (this.listButton.Count < needCount)
		{
			for (int i = this.listButton.Count; i < needCount; i++)
			{
				GiftHUDItem item = this.CreateButton();
				this.listButton.Add(item);
			}
		}
		else if (this.listButton.Count > needCount)
		{
			int num = this.listButton.Count - needCount;
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = this.listButton[this.listButton.Count - 1].gameObject;
				this.listButton[this.listButton.Count - 1] = null;
				this.listButton.RemoveAt(this.listButton.Count - 1);
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}

	// Token: 0x06003818 RID: 14360 RVA: 0x00121DB4 File Offset: 0x0011FFB4
	private GiftHUDItem CreateButton()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(this.exampleBut.gameObject, Vector3.zero, Quaternion.identity) as GameObject;
		gameObject.SetActive(true);
		GiftHUDItem component = gameObject.GetComponent<GiftHUDItem>();
		gameObject.transform.parent = this.parentButton.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return component;
	}

	// Token: 0x06003819 RID: 14361 RVA: 0x00121E28 File Offset: 0x00120028
	public void AnimScrollGift(int num)
	{
		if (this.listButton.Count > num)
		{
			this.listButton[num].InCenter(true, this.listButton.Count);
		}
	}

	// Token: 0x0600381A RID: 14362 RVA: 0x00121E64 File Offset: 0x00120064
	public void SetCanDraggable(bool val)
	{
		if (this.scrollAreaCollider)
		{
			this.scrollAreaCollider.enabled = val;
		}
		for (int i = 0; i < this.listButton.Count; i++)
		{
			this.listButton[i].colliderForDrag.enabled = val;
		}
	}

	// Token: 0x0600381B RID: 14363 RVA: 0x00121EC0 File Offset: 0x001200C0
	[ContextMenu("Sort gift")]
	private void TestSortGift()
	{
		this.Sort();
	}

	// Token: 0x0600381C RID: 14364 RVA: 0x00121EC8 File Offset: 0x001200C8
	[ContextMenu("Center main gift")]
	private void TestCenterGift()
	{
		if (this.listButton.Count > 6)
		{
			this.listButton[0].InCenter(false, 1);
		}
	}

	// Token: 0x040028E4 RID: 10468
	private List<SlotInfo> listItemData = new List<SlotInfo>();

	// Token: 0x040028E5 RID: 10469
	public List<GiftHUDItem> listButton = new List<GiftHUDItem>();

	// Token: 0x040028E6 RID: 10470
	public GiftHUDItem exampleBut;

	// Token: 0x040028E7 RID: 10471
	public GameObject parentButton;

	// Token: 0x040028E8 RID: 10472
	public UIWrapContent wrapScript;

	// Token: 0x040028E9 RID: 10473
	public UIScrollView scView;

	// Token: 0x040028EA RID: 10474
	public BoxCollider scrollAreaCollider;

	// Token: 0x040028EB RID: 10475
	public static bool canReCreateSlots = true;
}
