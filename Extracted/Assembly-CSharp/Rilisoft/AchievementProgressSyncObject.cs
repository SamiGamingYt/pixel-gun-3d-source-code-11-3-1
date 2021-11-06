using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft.MiniJson;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200051A RID: 1306
	[Serializable]
	public class AchievementProgressSyncObject
	{
		// Token: 0x06002D80 RID: 11648 RVA: 0x000EF4B8 File Offset: 0x000ED6B8
		public AchievementProgressSyncObject() : this(new List<AchievementProgressData>())
		{
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x000EF4C8 File Offset: 0x000ED6C8
		public AchievementProgressSyncObject(List<AchievementProgressData> progressData)
		{
			this._progressData = (progressData ?? new List<AchievementProgressData>());
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06002D82 RID: 11650 RVA: 0x000EF4E4 File Offset: 0x000ED6E4
		public List<AchievementProgressData> ProgressData
		{
			get
			{
				return this._progressData;
			}
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x000EF4EC File Offset: 0x000ED6EC
		public static AchievementProgressSyncObject FromJson(string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				return new AchievementProgressSyncObject();
			}
			Dictionary<string, object> dictionary = Json.Deserialize(json) as Dictionary<string, object>;
			if (dictionary == null)
			{
				return new AchievementProgressSyncObject();
			}
			object obj;
			if (!dictionary.TryGetValue("progressData", out obj))
			{
				return new AchievementProgressSyncObject();
			}
			List<object> list = obj as List<object>;
			if (list == null)
			{
				return new AchievementProgressSyncObject();
			}
			List<AchievementProgressData> list2 = new List<AchievementProgressData>();
			foreach (object obj2 in list)
			{
				Dictionary<string, object> dictionary2 = obj2 as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					try
					{
						AchievementProgressData item = AchievementProgressData.Create(dictionary2);
						list2.Add(item);
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
			return new AchievementProgressSyncObject(list2);
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x000EF5FC File Offset: 0x000ED7FC
		public static string ToJson(AchievementProgressSyncObject memento)
		{
			if (memento == null)
			{
				return string.Empty;
			}
			List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
			foreach (AchievementProgressData achievementProgressData in memento.ProgressData)
			{
				Dictionary<string, object> item = achievementProgressData.ObjectForSave();
				list.Add(item);
			}
			Dictionary<string, object> obj = new Dictionary<string, object>
			{
				{
					"progressData",
					list
				}
			};
			return Json.Serialize(obj);
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06002D85 RID: 11653 RVA: 0x000EF69C File Offset: 0x000ED89C
		internal bool Conflicted
		{
			get
			{
				return this._conflicted;
			}
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x000EF6A4 File Offset: 0x000ED8A4
		internal void SetConflicted()
		{
			this._conflicted = true;
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000EF6B0 File Offset: 0x000ED8B0
		public override bool Equals(object obj)
		{
			AchievementProgressSyncObject o = obj as AchievementProgressSyncObject;
			if (o == null)
			{
				return false;
			}
			if (this.ProgressData == null && o.ProgressData == null)
			{
				return true;
			}
			if ((this.ProgressData == null && o.ProgressData != null) || (this.ProgressData != null && o.ProgressData == null))
			{
				return false;
			}
			if (!this.ProgressData.All((AchievementProgressData d) => o.ProgressData.Contains(d)) || !o.ProgressData.All((AchievementProgressData k) => this.ProgressData.Contains(k)))
			{
				return false;
			}
			int num = 0;
			foreach (AchievementProgressData achievementProgressData in this.ProgressData)
			{
				AchievementProgressData obj2 = o.ProgressData[num];
				if (!achievementProgressData.Equals(obj2))
				{
					return false;
				}
				num++;
			}
			return true;
		}

		// Token: 0x04002208 RID: 8712
		[SerializeField]
		private readonly List<AchievementProgressData> _progressData;

		// Token: 0x04002209 RID: 8713
		private bool _conflicted;
	}
}
