using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x0200052A RID: 1322
	[Serializable]
	internal sealed class FreeSpinPointMemento : AdPointMementoBase
	{
		// Token: 0x06002E0A RID: 11786 RVA: 0x000F1474 File Offset: 0x000EF674
		public FreeSpinPointMemento(string id) : base(id)
		{
			this.TimeoutBetweenShowInMinutes = FreeSpinPointMemento.DefaultTimeoutBetweenShowInMinutes;
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06002E0B RID: 11787 RVA: 0x000F1488 File Offset: 0x000EF688
		public static double DefaultTimeoutBetweenShowInMinutes
		{
			get
			{
				return 1440.0;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002E0C RID: 11788 RVA: 0x000F1494 File Offset: 0x000EF694
		// (set) Token: 0x06002E0D RID: 11789 RVA: 0x000F149C File Offset: 0x000EF69C
		public double TimeoutBetweenShowInMinutes { get; private set; }

		// Token: 0x06002E0E RID: 11790 RVA: 0x000F14A8 File Offset: 0x000EF6A8
		public double GetFinalTimeoutBetweenShowInMinutes(string category)
		{
			double? doubleOverride = base.GetDoubleOverride("timeoutBetweenShowMinutes", category);
			if (doubleOverride != null)
			{
				return doubleOverride.Value;
			}
			return this.TimeoutBetweenShowInMinutes;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000F14DC File Offset: 0x000EF6DC
		internal static FreeSpinPointMemento FromObject(string id, object obj)
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
			FreeSpinPointMemento freeSpinPointMemento = new FreeSpinPointMemento(id);
			freeSpinPointMemento.Reset(dictionary);
			double? @double = ParsingHelper.GetDouble(dictionary, "timeoutBetweenShowMinutes");
			if (@double != null)
			{
				freeSpinPointMemento.TimeoutBetweenShowInMinutes = @double.Value;
			}
			return freeSpinPointMemento;
		}
	}
}
