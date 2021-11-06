using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000573 RID: 1395
public class StickersController : MonoBehaviour
{
	// Token: 0x14000042 RID: 66
	// (add) Token: 0x0600305B RID: 12379 RVA: 0x000FC434 File Offset: 0x000FA634
	// (remove) Token: 0x0600305C RID: 12380 RVA: 0x000FC44C File Offset: 0x000FA64C
	public static event Action onBuyPack;

	// Token: 0x0600305D RID: 12381 RVA: 0x000FC464 File Offset: 0x000FA664
	public static string KeyForBuyPack(TypePackSticker needPack)
	{
		switch (needPack)
		{
		case TypePackSticker.classic:
			return StickersController.ClassicSmileKey;
		case TypePackSticker.christmas:
			return StickersController.ChristmasSmileKey;
		case TypePackSticker.easter:
			return StickersController.EasterSmileKey;
		default:
			return null;
		}
	}

	// Token: 0x0600305E RID: 12382 RVA: 0x000FC4A0 File Offset: 0x000FA6A0
	public static ItemPrice GetPricePack(TypePackSticker needPack)
	{
		return VirtualCurrencyHelper.Price(StickersController.KeyForBuyPack(needPack));
	}

	// Token: 0x0600305F RID: 12383 RVA: 0x000FC4B0 File Offset: 0x000FA6B0
	public static bool IsBuyPack(TypePackSticker needPack)
	{
		return Storager.getInt(StickersController.KeyForBuyPack(needPack), true) == 1;
	}

	// Token: 0x06003060 RID: 12384 RVA: 0x000FC4C4 File Offset: 0x000FA6C4
	public static void BuyStickersPack(TypePackSticker buyPack)
	{
		Storager.setInt(StickersController.KeyForBuyPack(buyPack), 1, true);
	}

	// Token: 0x06003061 RID: 12385 RVA: 0x000FC4D4 File Offset: 0x000FA6D4
	public static bool IsBuyAnyPack()
	{
		foreach (object obj in Enum.GetValues(typeof(TypePackSticker)))
		{
			if ((int)obj != 0)
			{
				if (StickersController.IsBuyPack((TypePackSticker)((int)obj)))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003062 RID: 12386 RVA: 0x000FC56C File Offset: 0x000FA76C
	public static bool IsBuyAllPack()
	{
		foreach (object obj in Enum.GetValues(typeof(TypePackSticker)))
		{
			if ((int)obj != 0 && (int)obj != 3)
			{
				if (!StickersController.IsBuyPack((TypePackSticker)((int)obj)))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06003063 RID: 12387 RVA: 0x000FC610 File Offset: 0x000FA810
	public static List<TypePackSticker> GetAvaliablePack()
	{
		List<TypePackSticker> list = new List<TypePackSticker>();
		foreach (object obj in Enum.GetValues(typeof(TypePackSticker)))
		{
			if ((int)obj != 0)
			{
				if (StickersController.IsBuyPack((TypePackSticker)((int)obj)))
				{
					list.Add((TypePackSticker)((int)obj));
				}
			}
		}
		return list;
	}

	// Token: 0x06003064 RID: 12388 RVA: 0x000FC6B0 File Offset: 0x000FA8B0
	public static void EventPackBuy()
	{
		if (StickersController.onBuyPack != null)
		{
			StickersController.onBuyPack();
		}
	}

	// Token: 0x0400238E RID: 9102
	public static string ClassicSmileKey = "SmileKey";

	// Token: 0x0400238F RID: 9103
	public static string ChristmasSmileKey = "ChristmasSmileKey";

	// Token: 0x04002390 RID: 9104
	public static string EasterSmileKey = "EasterSmileKey";
}
