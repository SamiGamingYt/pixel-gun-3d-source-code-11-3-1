using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000530 RID: 1328
	[Serializable]
	internal sealed class PlayerStateMemento
	{
		// Token: 0x06002E48 RID: 11848 RVA: 0x000F2374 File Offset: 0x000F0574
		public PlayerStateMemento(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this._id = id;
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000F2394 File Offset: 0x000F0594
		internal static PlayerStateMemento FromDictionary(string id, Dictionary<string, object> dictionary)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			PlayerStateMemento playerStateMemento = new PlayerStateMemento(id);
			object obj;
			if (dictionary.TryGetValue("minDay", out obj))
			{
				try
				{
					playerStateMemento.MinDay = Convert.ToInt32(obj, NumberFormatInfo.InvariantInfo);
				}
				catch (Exception ex)
				{
					Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as int. {2}", new object[]
					{
						"minDay",
						obj,
						ex.Message
					});
				}
			}
			object obj2;
			if (dictionary.TryGetValue("maxDay", out obj2))
			{
				try
				{
					playerStateMemento.MaxDay = Convert.ToInt32(obj2, NumberFormatInfo.InvariantInfo);
				}
				catch (Exception ex2)
				{
					Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as int. {2}", new object[]
					{
						"maxDay",
						obj2,
						ex2.Message
					});
				}
			}
			object obj3;
			if (dictionary.TryGetValue("minInGameMinutes", out obj3))
			{
				try
				{
					playerStateMemento.MinInGameMinutes = new int?(Convert.ToInt32(obj3, NumberFormatInfo.InvariantInfo));
				}
				catch (Exception ex3)
				{
					Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as int. {2}", new object[]
					{
						"minInGameMinutes",
						obj3,
						ex3.Message
					});
				}
			}
			object obj4;
			if (dictionary.TryGetValue("maxInGameMinutes", out obj4))
			{
				try
				{
					playerStateMemento.MaxInGameMinutes = new int?(Convert.ToInt32(obj4, NumberFormatInfo.InvariantInfo));
				}
				catch (Exception ex4)
				{
					Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as int. {2}", new object[]
					{
						"maxInGameMinutes",
						obj4,
						ex4.Message
					});
				}
			}
			object obj5;
			if (dictionary.TryGetValue("paying", out obj5))
			{
				try
				{
					playerStateMemento.IsPaying = Convert.ToBoolean(obj5);
				}
				catch (Exception ex5)
				{
					Debug.LogWarningFormat("Failed to interpret `\"{0}\": {1}` as boolean. {2}", new object[]
					{
						"paying",
						obj5,
						ex5.Message
					});
				}
			}
			return playerStateMemento;
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002E4A RID: 11850 RVA: 0x000F2604 File Offset: 0x000F0804
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002E4B RID: 11851 RVA: 0x000F260C File Offset: 0x000F080C
		// (set) Token: 0x06002E4C RID: 11852 RVA: 0x000F2614 File Offset: 0x000F0814
		public int MinDay { get; private set; }

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06002E4D RID: 11853 RVA: 0x000F2620 File Offset: 0x000F0820
		// (set) Token: 0x06002E4E RID: 11854 RVA: 0x000F2628 File Offset: 0x000F0828
		public int MaxDay { get; private set; }

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002E4F RID: 11855 RVA: 0x000F2634 File Offset: 0x000F0834
		// (set) Token: 0x06002E50 RID: 11856 RVA: 0x000F263C File Offset: 0x000F083C
		public int? MinInGameMinutes { get; private set; }

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002E51 RID: 11857 RVA: 0x000F2648 File Offset: 0x000F0848
		// (set) Token: 0x06002E52 RID: 11858 RVA: 0x000F2650 File Offset: 0x000F0850
		public int? MaxInGameMinutes { get; private set; }

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002E53 RID: 11859 RVA: 0x000F265C File Offset: 0x000F085C
		// (set) Token: 0x06002E54 RID: 11860 RVA: 0x000F2664 File Offset: 0x000F0864
		public bool IsPaying { get; private set; }

		// Token: 0x0400225B RID: 8795
		private readonly string _id;
	}
}
