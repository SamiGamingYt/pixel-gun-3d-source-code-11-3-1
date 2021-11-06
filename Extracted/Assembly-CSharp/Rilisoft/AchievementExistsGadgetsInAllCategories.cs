using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000510 RID: 1296
	public class AchievementExistsGadgetsInAllCategories : Achievement
	{
		// Token: 0x06002D20 RID: 11552 RVA: 0x000EDE8C File Offset: 0x000EC08C
		public AchievementExistsGadgetsInAllCategories(AchievementData data, AchievementProgressData progressData) : base(data, progressData)
		{
			if (base.IsCompleted)
			{
				return;
			}
			GadgetsInfo.OnGetGadget += this.GadgetsInfo_OnGetGadget;
			this.UpdateMe();
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000EDEBC File Offset: 0x000EC0BC
		private void GadgetsInfo_OnGetGadget(string obj)
		{
			if (base.IsCompleted)
			{
				return;
			}
			this.UpdateMe();
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000EDED0 File Offset: 0x000EC0D0
		private void UpdateMe()
		{
			List<GadgetInfo.GadgetCategory> existsCategories = new List<GadgetInfo.GadgetCategory>();
			RiliExtensions.ForEachEnum<GadgetInfo.GadgetCategory>(delegate(GadgetInfo.GadgetCategory val)
			{
				Dictionary<string, GadgetInfo> dictionary = GadgetsInfo.GadgetsOfCategory(val);
				foreach (KeyValuePair<string, GadgetInfo> keyValuePair in dictionary)
				{
					if (GadgetsInfo.IsBought(keyValuePair.Key))
					{
						existsCategories.Add(val);
						break;
					}
				}
			});
			if (existsCategories.Count == RiliExtensions.EnumLen<GadgetInfo.GadgetCategory>())
			{
				base.Gain(1);
			}
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x000EDF1C File Offset: 0x000EC11C
		public override void Dispose()
		{
			GadgetsInfo.OnGetGadget -= this.GadgetsInfo_OnGetGadget;
		}
	}
}
