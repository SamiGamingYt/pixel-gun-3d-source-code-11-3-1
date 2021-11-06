using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020005E7 RID: 1511
public static class EggHatchingConditionFormatter
{
	// Token: 0x060033B8 RID: 13240 RVA: 0x0010BE9C File Offset: 0x0010A09C
	public static string TextForConditionOfEgg(Egg egg)
	{
		if (egg == null || egg.Data == null)
		{
			Debug.LogError("TextForConditionOfEgg: egg == null || egg.Data == null || egg.Data.HatchedTypes.Count == 0");
			return string.Empty;
		}
		try
		{
			switch (egg.Data.HatchedType)
			{
			case EggHatchedType.Time:
				if (egg.IncubationTimeLeft != null)
				{
					long value = egg.IncubationTimeLeft.Value;
					return (value < 86400L) ? RiliExtensions.GetTimeString(value, ":") : string.Format("{0} {1}", LocalizationStore.Get("Key_1125"), RiliExtensions.GetTimeStringDays(value));
				}
				return LocalizationStore.Get("Key_2566");
			case EggHatchedType.League:
				return RatingSystem.instance.RatingNeededForLeague(egg.Data.League).ToString();
			case EggHatchedType.Wins:
				return string.Format(LocalizationStore.Get("Key_2511"), egg.WinsLeft);
			case EggHatchedType.Rating:
				return string.Format(LocalizationStore.Get("Key_2732"), egg.RatingLeft);
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in TextForConditionOfEgg: {0}", new object[]
			{
				ex
			});
		}
		Debug.LogError("TextForConditionOfEgg: end of method reached");
		return string.Empty;
	}
}
