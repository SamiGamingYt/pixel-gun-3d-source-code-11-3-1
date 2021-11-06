using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000529 RID: 1321
	[Serializable]
	internal sealed class ReturnInConnectSceneAdPointMemento : AdPointMementoBase
	{
		// Token: 0x06002DFC RID: 11772 RVA: 0x000F12B8 File Offset: 0x000EF4B8
		public ReturnInConnectSceneAdPointMemento(string id) : base(id)
		{
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002DFD RID: 11773 RVA: 0x000F12C4 File Offset: 0x000EF4C4
		// (set) Token: 0x06002DFE RID: 11774 RVA: 0x000F12CC File Offset: 0x000EF4CC
		public double DelayInSeconds { get; private set; }

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06002DFF RID: 11775 RVA: 0x000F12D8 File Offset: 0x000EF4D8
		// (set) Token: 0x06002E00 RID: 11776 RVA: 0x000F12E0 File Offset: 0x000EF4E0
		public double MinInGameTimePerDayInMinutes { get; private set; }

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06002E01 RID: 11777 RVA: 0x000F12EC File Offset: 0x000EF4EC
		// (set) Token: 0x06002E02 RID: 11778 RVA: 0x000F12F4 File Offset: 0x000EF4F4
		public int ImpressionMaxCountPerDay { get; private set; }

		// Token: 0x06002E03 RID: 11779 RVA: 0x000F1300 File Offset: 0x000EF500
		public double GetFinalDelayInSeconds(string category)
		{
			double? delayInSecondsOverride = this.GetDelayInSecondsOverride(category);
			if (delayInSecondsOverride != null)
			{
				return delayInSecondsOverride.Value;
			}
			return this.DelayInSeconds;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000F1330 File Offset: 0x000EF530
		public double GetFinalMinInGameTimePerDayInMinutes(string category)
		{
			double? minInGameTimePerDayInMinutesOverride = this.GetMinInGameTimePerDayInMinutesOverride(category);
			if (minInGameTimePerDayInMinutesOverride != null)
			{
				return minInGameTimePerDayInMinutesOverride.Value;
			}
			return this.MinInGameTimePerDayInMinutes;
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000F1360 File Offset: 0x000EF560
		public int GetFinalImpressionMaxCountPerDay(string category)
		{
			int? impressionMaxCountPerDayOverride = this.GetImpressionMaxCountPerDayOverride(category);
			if (impressionMaxCountPerDayOverride != null)
			{
				return impressionMaxCountPerDayOverride.Value;
			}
			return this.ImpressionMaxCountPerDay;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000F1390 File Offset: 0x000EF590
		private double? GetDelayInSecondsOverride(string category)
		{
			return base.GetDoubleOverride("delaySeconds", category);
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000F13A0 File Offset: 0x000EF5A0
		private double? GetMinInGameTimePerDayInMinutesOverride(string category)
		{
			return base.GetDoubleOverride("minInGameTimePerDayMinutes", category);
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000F13B0 File Offset: 0x000EF5B0
		private int? GetImpressionMaxCountPerDayOverride(string category)
		{
			return base.GetInt32Override("impressionMaxCountPerDay", category);
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000F13C0 File Offset: 0x000EF5C0
		internal static ReturnInConnectSceneAdPointMemento FromObject(string id, object obj)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (obj == null)
			{
				return null;
			}
			Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
			if (dictionary == null)
			{
				return null;
			}
			ReturnInConnectSceneAdPointMemento returnInConnectSceneAdPointMemento = new ReturnInConnectSceneAdPointMemento(id);
			returnInConnectSceneAdPointMemento.Reset(dictionary);
			double? @double = ParsingHelper.GetDouble(dictionary, "delaySeconds");
			if (@double != null)
			{
				returnInConnectSceneAdPointMemento.DelayInSeconds = @double.Value;
			}
			double? double2 = ParsingHelper.GetDouble(dictionary, "minInGameTimePerDayMinutes");
			if (double2 != null)
			{
				returnInConnectSceneAdPointMemento.MinInGameTimePerDayInMinutes = double2.Value;
			}
			int? @int = ParsingHelper.GetInt32(dictionary, "impressionMaxCountPerDay");
			if (@int != null)
			{
				returnInConnectSceneAdPointMemento.ImpressionMaxCountPerDay = @int.Value;
			}
			return returnInConnectSceneAdPointMemento;
		}
	}
}
