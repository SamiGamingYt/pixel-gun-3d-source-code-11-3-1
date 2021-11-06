using System;
using System.Collections.Generic;
using System.Globalization;

namespace Rilisoft
{
	// Token: 0x02000523 RID: 1315
	[Serializable]
	internal abstract class AdPointMementoBase
	{
		// Token: 0x06002DCF RID: 11727 RVA: 0x000F0B28 File Offset: 0x000EED28
		public AdPointMementoBase(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this._id = id;
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06002DD0 RID: 11728 RVA: 0x000F0B54 File Offset: 0x000EED54
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06002DD1 RID: 11729 RVA: 0x000F0B5C File Offset: 0x000EED5C
		// (set) Token: 0x06002DD2 RID: 11730 RVA: 0x000F0B64 File Offset: 0x000EED64
		public bool Enabled { get; private set; }

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06002DD3 RID: 11731 RVA: 0x000F0B70 File Offset: 0x000EED70
		public Dictionary<string, Dictionary<string, object>> Overrides
		{
			get
			{
				return this._overrides;
			}
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000F0B78 File Offset: 0x000EED78
		public int GetDisabledReasonCode(string category)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			bool? enabledOverride = this.GetEnabledOverride(category);
			if (enabledOverride != null)
			{
				if (!enabledOverride.Value)
				{
					return 10;
				}
			}
			else if (!this.Enabled)
			{
				return 20;
			}
			return 0;
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x000F0BD0 File Offset: 0x000EEDD0
		public string GetDisabledReason(string category)
		{
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			bool? enabledOverride = this.GetEnabledOverride(category);
			if (enabledOverride != null)
			{
				if (!enabledOverride.Value)
				{
					return string.Format(CultureInfo.InvariantCulture, "`{0}` explicitely disabled for category `{1}`.", new object[]
					{
						this.Id,
						category
					});
				}
			}
			else if (!this.Enabled)
			{
				return string.Format(CultureInfo.InvariantCulture, "`{0}` just disabled", new object[]
				{
					this.Id
				});
			}
			return string.Empty;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x000F0C68 File Offset: 0x000EEE68
		protected void Reset(Dictionary<string, object> dictionary)
		{
			if (dictionary == null)
			{
				throw new ArgumentNullException("dictionary");
			}
			this.Enabled = false;
			this._overrides.Clear();
			bool? boolean = ParsingHelper.GetBoolean(dictionary, "enabled");
			if (boolean != null)
			{
				this.Enabled = boolean.Value;
			}
			object obj;
			if (dictionary.TryGetValue("overrides", out obj))
			{
				Dictionary<string, object> dictionary2 = obj as Dictionary<string, object>;
				if (dictionary2 != null)
				{
					foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
					{
						Dictionary<string, object> dictionary3 = keyValuePair.Value as Dictionary<string, object>;
						if (dictionary3 != null)
						{
							this.Overrides[keyValuePair.Key] = dictionary3;
						}
					}
				}
			}
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000F0D64 File Offset: 0x000EEF64
		protected bool? GetBooleanOverride(string nodeKey, string category)
		{
			object nodeObjectOverride = this.GetNodeObjectOverride(nodeKey, category);
			if (nodeObjectOverride == null)
			{
				return null;
			}
			bool? result;
			try
			{
				result = new bool?(Convert.ToBoolean(nodeObjectOverride));
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x000F0DD4 File Offset: 0x000EEFD4
		protected int? GetInt32Override(string nodeKey, string category)
		{
			object nodeObjectOverride = this.GetNodeObjectOverride(nodeKey, category);
			if (nodeObjectOverride == null)
			{
				return null;
			}
			int? result;
			try
			{
				result = new int?(Convert.ToInt32(nodeObjectOverride));
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000F0E44 File Offset: 0x000EF044
		protected double? GetDoubleOverride(string nodeKey, string category)
		{
			object nodeObjectOverride = this.GetNodeObjectOverride(nodeKey, category);
			if (nodeObjectOverride == null)
			{
				return null;
			}
			double? result;
			try
			{
				result = new double?(Convert.ToDouble(nodeObjectOverride));
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000F0EB4 File Offset: 0x000EF0B4
		protected string GetStringOverride(string nodeKey, string category)
		{
			object nodeObjectOverride = this.GetNodeObjectOverride(nodeKey, category);
			if (nodeObjectOverride == null)
			{
				return null;
			}
			return nodeObjectOverride as string;
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000F0ED8 File Offset: 0x000EF0D8
		protected object GetNodeObjectOverride(string nodeKey, string category)
		{
			if (nodeKey == null)
			{
				throw new ArgumentNullException("nodeKey");
			}
			if (category == null)
			{
				throw new ArgumentNullException("category");
			}
			Dictionary<string, object> dictionary;
			if (!this.Overrides.TryGetValue(category, out dictionary))
			{
				return null;
			}
			object result;
			if (!dictionary.TryGetValue(nodeKey, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000F0F30 File Offset: 0x000EF130
		private bool? GetEnabledOverride(string category)
		{
			return this.GetBooleanOverride("enabled", category);
		}

		// Token: 0x04002234 RID: 8756
		private readonly string _id;

		// Token: 0x04002235 RID: 8757
		private readonly Dictionary<string, Dictionary<string, object>> _overrides = new Dictionary<string, Dictionary<string, object>>();
	}
}
