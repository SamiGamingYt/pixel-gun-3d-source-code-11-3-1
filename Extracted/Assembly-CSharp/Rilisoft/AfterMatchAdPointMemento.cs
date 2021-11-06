using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x02000528 RID: 1320
	[Serializable]
	internal sealed class AfterMatchAdPointMemento : AdPointMementoBase
	{
		// Token: 0x06002DF1 RID: 11761 RVA: 0x000F118C File Offset: 0x000EF38C
		public AfterMatchAdPointMemento(string id) : base(id)
		{
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06002DF2 RID: 11762 RVA: 0x000F1198 File Offset: 0x000EF398
		// (set) Token: 0x06002DF3 RID: 11763 RVA: 0x000F11A0 File Offset: 0x000EF3A0
		public bool EnabledForWinner { get; private set; }

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002DF4 RID: 11764 RVA: 0x000F11AC File Offset: 0x000EF3AC
		// (set) Token: 0x06002DF5 RID: 11765 RVA: 0x000F11B4 File Offset: 0x000EF3B4
		public bool EnabledForLoser { get; private set; }

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002DF6 RID: 11766 RVA: 0x000F11C0 File Offset: 0x000EF3C0
		// (set) Token: 0x06002DF7 RID: 11767 RVA: 0x000F11C8 File Offset: 0x000EF3C8
		public double MatchMinDurationInMinutes { get; private set; }

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000F11D4 File Offset: 0x000EF3D4
		internal bool? GetEnabledForWinnerOverride(string category)
		{
			return base.GetBooleanOverride("enabledForWinner", category);
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000F11E4 File Offset: 0x000EF3E4
		internal bool? GetEnabledForLoserOverride(string category)
		{
			return base.GetBooleanOverride("enabledForLoser", category);
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000F11F4 File Offset: 0x000EF3F4
		internal double? GetMatchMinDurationInMinutesOverride(string category)
		{
			return base.GetDoubleOverride("matchMinDurationMinutes", category);
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x000F1204 File Offset: 0x000EF404
		internal static AfterMatchAdPointMemento FromObject(string id, object obj)
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
			AfterMatchAdPointMemento afterMatchAdPointMemento = new AfterMatchAdPointMemento(id);
			afterMatchAdPointMemento.Reset(dictionary);
			bool? boolean = ParsingHelper.GetBoolean(dictionary, "enabledForWinner");
			if (boolean != null)
			{
				afterMatchAdPointMemento.EnabledForWinner = boolean.Value;
			}
			bool? boolean2 = ParsingHelper.GetBoolean(dictionary, "enabledForLoser");
			if (boolean2 != null)
			{
				afterMatchAdPointMemento.EnabledForLoser = boolean2.Value;
			}
			double? @double = ParsingHelper.GetDouble(dictionary, "matchMinDurationMinutes");
			if (@double != null)
			{
				afterMatchAdPointMemento.MatchMinDurationInMinutes = @double.Value;
			}
			return afterMatchAdPointMemento;
		}
	}
}
