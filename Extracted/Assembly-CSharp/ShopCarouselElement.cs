using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x020007AA RID: 1962
public class ShopCarouselElement : MonoBehaviour
{
	// Token: 0x060045EF RID: 17903 RVA: 0x00179FDC File Offset: 0x001781DC
	public void SetupPriceAndDiscount(string idToGetPriceFor, ShopNGUIController.CategoryNames category)
	{
		this.IdToGetPriceFor = idToGetPriceFor;
		this.Category = category;
		this.StartUpdatePriceAndDiscount();
	}

	// Token: 0x060045F0 RID: 17904 RVA: 0x00179FF4 File Offset: 0x001781F4
	private void OnEnable()
	{
		this.StartUpdatePriceAndDiscount();
		base.Invoke("RecalculatePriceAndDiscount", Time.unscaledDeltaTime);
	}

	// Token: 0x060045F1 RID: 17905 RVA: 0x0017A00C File Offset: 0x0017820C
	private void RecalculatePriceAndDiscountPositions(bool discount)
	{
		if (discount)
		{
			this.priceLabels[0].transform.localPosition = new Vector3(42f, 0f, 0f);
			this.frameDiscountAndPrice.width = 135;
		}
		else
		{
			this.frameDiscountAndPrice.width = 80;
			this.priceLabels[0].transform.localPosition = new Vector3(12f, 0f, 0f);
		}
	}

	// Token: 0x060045F2 RID: 17906 RVA: 0x0017A098 File Offset: 0x00178298
	private void StartUpdatePriceAndDiscount()
	{
		if (this.IdToGetPriceFor != null)
		{
			base.StartCoroutine(this.UpdatePriceAndDiscountCoroutine(this.IdToGetPriceFor, this.Category));
		}
	}

	// Token: 0x060045F3 RID: 17907 RVA: 0x0017A0CC File Offset: 0x001782CC
	private IEnumerator UpdatePriceAndDiscountCoroutine(string idToGetPriceFor, ShopNGUIController.CategoryNames category)
	{
		do
		{
			this.UpdatePriceAndDiscount(idToGetPriceFor, category);
			yield return CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.WaitForSeconds(0.5f));
		}
		while (!(this == null) && !(base.gameObject == null));
		yield break;
	}

	// Token: 0x060045F4 RID: 17908 RVA: 0x0017A104 File Offset: 0x00178304
	private void UpdatePriceAndDiscount(string idToGetPriceFor, ShopNGUIController.CategoryNames category)
	{
		string a = WeaponManager.LastBoughtTag(idToGetPriceFor, null);
		string text = WeaponManager.FirstUnboughtTag(idToGetPriceFor);
		bool flag = idToGetPriceFor == text && a != text;
		if (this.priceAndDiscountContainer.activeSelf != flag)
		{
			this.priceAndDiscountContainer.SetActive(flag);
		}
		if (flag)
		{
			ItemPrice itemPrice = ShopNGUIController.GetItemPrice(text, category, false, true, false);
			bool flag2;
			int num = ShopNGUIController.DiscountFor(text, out flag2);
			foreach (UILabel uilabel in this.priceLabels)
			{
				uilabel.text = itemPrice.Price.ToString();
			}
			bool flag3 = num > 0;
			foreach (UILabel uilabel2 in this.discountLabels)
			{
				uilabel2.gameObject.SetActiveSafeSelf(flag3);
				if (flag3)
				{
					uilabel2.text = "-" + num.ToString() + "%";
				}
			}
			GameObject gameObject = (!(itemPrice.Currency == "GemsCurrency")) ? this.coin : this.gem;
			if (!gameObject.activeSelf)
			{
				gameObject.SetActive(true);
			}
			GameObject gameObject2 = (!(itemPrice.Currency == "Coins")) ? this.coin : this.gem;
			if (gameObject2.activeSelf)
			{
				gameObject2.SetActive(false);
			}
			this.RecalculatePriceAndDiscountPositions(flag3);
		}
	}

	// Token: 0x060045F5 RID: 17909 RVA: 0x0017A2E4 File Offset: 0x001784E4
	public void SetQuantity()
	{
		this.quantity.text = Storager.getInt(GearManager.HolderQuantityForID(this.itemID), false).ToString() + ((this.itemID == null || !GearManager.HolderQuantityForID(this.itemID).Equals(GearManager.Grenade)) ? string.Empty : ("/" + GearManager.MaxCountForGear(GearManager.HolderQuantityForID(this.itemID))));
	}

	// Token: 0x060045F6 RID: 17910 RVA: 0x0017A368 File Offset: 0x00178568
	private void Awake()
	{
		this.arrnoInitialPos = new Vector3(70.05f, -0.00016f, -120f);
	}

	// Token: 0x060045F7 RID: 17911 RVA: 0x0017A384 File Offset: 0x00178584
	private void Start()
	{
		if (Array.IndexOf<string>(PotionsController.potions, this.itemID) >= 0)
		{
			this.quantity.gameObject.SetActive(true);
			this.HandlePotionActivated(this.itemID);
		}
		PotionsController.PotionActivated += this.HandlePotionActivated;
	}

	// Token: 0x060045F8 RID: 17912 RVA: 0x0017A3D8 File Offset: 0x001785D8
	private void HandlePotionActivated(string obj)
	{
		if (this.itemID != null && obj != null && this.itemID.Equals(obj))
		{
			this.quantity.text = Storager.getInt(GearManager.HolderQuantityForID(this.itemID), false).ToString() + ((this.itemID == null || !GearManager.HolderQuantityForID(this.itemID).Equals(GearManager.Grenade)) ? string.Empty : ("/" + GearManager.MaxCountForGear(GearManager.HolderQuantityForID(this.itemID))));
		}
	}

	// Token: 0x060045F9 RID: 17913 RVA: 0x0017A480 File Offset: 0x00178680
	public void SetPos(float scaleCoef, float offset)
	{
		if (this.model != null)
		{
			this.model.localScale = this.baseScale * scaleCoef;
			this.model.localPosition = new Vector3(0f, 0f, -55f);
		}
		if (this.arrow != null)
		{
			if (scaleCoef <= 0f)
			{
				this.arrow.localScale = Vector3.zero;
			}
			else
			{
				this.arrow.localScale = Vector3.one;
			}
			this.arrow.localPosition = new Vector3(this.arrnoInitialPos.x * scaleCoef * 0.7f, this.arrnoInitialPos.y * scaleCoef, -300f);
		}
		if (this.locked != null)
		{
			this.locked.transform.localScale = new Vector3(1f, 1f, 1f) * scaleCoef;
			this.locked.transform.localPosition = new Vector3(0f, 0f, -300f);
		}
		if (this.frameDiscountAndPrice != null)
		{
			if (scaleCoef <= 0f)
			{
				this.frameDiscountAndPrice.transform.localScale = Vector3.zero;
			}
			else
			{
				this.frameDiscountAndPrice.transform.localScale = Vector3.one * 0.75f;
			}
			this.frameDiscountAndPrice.transform.localPosition = new Vector3(0f, -70f * (scaleCoef * 1.4f), -300f);
		}
	}

	// Token: 0x060045FA RID: 17914 RVA: 0x0017A62C File Offset: 0x0017882C
	private void OnDestroy()
	{
		PotionsController.PotionActivated -= this.HandlePotionActivated;
	}

	// Token: 0x17000BC0 RID: 3008
	// (get) Token: 0x060045FB RID: 17915 RVA: 0x0017A640 File Offset: 0x00178840
	// (set) Token: 0x060045FC RID: 17916 RVA: 0x0017A648 File Offset: 0x00178848
	private string IdToGetPriceFor { get; set; }

	// Token: 0x17000BC1 RID: 3009
	// (get) Token: 0x060045FD RID: 17917 RVA: 0x0017A654 File Offset: 0x00178854
	// (set) Token: 0x060045FE RID: 17918 RVA: 0x0017A65C File Offset: 0x0017885C
	private ShopNGUIController.CategoryNames Category { get; set; }

	// Token: 0x04003344 RID: 13124
	public GameObject priceAndDiscountContainer;

	// Token: 0x04003345 RID: 13125
	public GameObject gem;

	// Token: 0x04003346 RID: 13126
	public GameObject coin;

	// Token: 0x04003347 RID: 13127
	public List<UILabel> priceLabels;

	// Token: 0x04003348 RID: 13128
	public List<UILabel> discountLabels;

	// Token: 0x04003349 RID: 13129
	public UISprite frameDiscountAndPrice;

	// Token: 0x0400334A RID: 13130
	public GameObject locked;

	// Token: 0x0400334B RID: 13131
	public Transform arrow;

	// Token: 0x0400334C RID: 13132
	public UILabel topSeller;

	// Token: 0x0400334D RID: 13133
	public UILabel quantity;

	// Token: 0x0400334E RID: 13134
	public UILabel newnew;

	// Token: 0x0400334F RID: 13135
	public bool showTS;

	// Token: 0x04003350 RID: 13136
	public bool showNew;

	// Token: 0x04003351 RID: 13137
	public bool showQuantity;

	// Token: 0x04003352 RID: 13138
	public string prefabPath;

	// Token: 0x04003353 RID: 13139
	public Vector3 baseScale;

	// Token: 0x04003354 RID: 13140
	public Vector3 ourPosition;

	// Token: 0x04003355 RID: 13141
	public string itemID;

	// Token: 0x04003356 RID: 13142
	public string readableName;

	// Token: 0x04003357 RID: 13143
	public Transform model;

	// Token: 0x04003358 RID: 13144
	private float lastTimeUpdated;

	// Token: 0x04003359 RID: 13145
	public Vector3 arrnoInitialPos;
}
