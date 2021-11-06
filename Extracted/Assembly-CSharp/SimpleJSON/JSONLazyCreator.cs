using System;

namespace SimpleJSON
{
	// Token: 0x020002B9 RID: 697
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x06001602 RID: 5634 RVA: 0x00058FF0 File Offset: 0x000571F0
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x00059008 File Offset: 0x00057208
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x00059020 File Offset: 0x00057220
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x1700027A RID: 634
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray
				{
					value
				});
			}
		}

		// Token: 0x1700027B RID: 635
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONClass
				{
					{
						aKey,
						value
					}
				});
			}
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x000590B4 File Offset: 0x000572B4
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray
			{
				aItem
			});
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x000590D8 File Offset: 0x000572D8
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass
			{
				{
					aKey,
					aItem
				}
			});
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x000590FC File Offset: 0x000572FC
		public override bool Equals(object obj)
		{
			return obj == null || object.ReferenceEquals(this, obj);
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x00059110 File Offset: 0x00057310
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00059118 File Offset: 0x00057318
		public override string ToString()
		{
			return string.Empty;
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00059120 File Offset: 0x00057320
		public override string ToString(string aPrefix)
		{
			return string.Empty;
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600160F RID: 5647 RVA: 0x00059128 File Offset: 0x00057328
		// (set) Token: 0x06001610 RID: 5648 RVA: 0x00059144 File Offset: 0x00057344
		public override int AsInt
		{
			get
			{
				JSONData aVal = new JSONData(0);
				this.Set(aVal);
				return 0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001611 RID: 5649 RVA: 0x00059160 File Offset: 0x00057360
		// (set) Token: 0x06001612 RID: 5650 RVA: 0x00059184 File Offset: 0x00057384
		public override float AsFloat
		{
			get
			{
				JSONData aVal = new JSONData(0f);
				this.Set(aVal);
				return 0f;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06001613 RID: 5651 RVA: 0x000591A0 File Offset: 0x000573A0
		// (set) Token: 0x06001614 RID: 5652 RVA: 0x000591CC File Offset: 0x000573CC
		public override double AsDouble
		{
			get
			{
				JSONData aVal = new JSONData(0.0);
				this.Set(aVal);
				return 0.0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06001615 RID: 5653 RVA: 0x000591E8 File Offset: 0x000573E8
		// (set) Token: 0x06001616 RID: 5654 RVA: 0x00059204 File Offset: 0x00057404
		public override bool AsBool
		{
			get
			{
				JSONData aVal = new JSONData(false);
				this.Set(aVal);
				return false;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001617 RID: 5655 RVA: 0x00059220 File Offset: 0x00057420
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001618 RID: 5656 RVA: 0x0005923C File Offset: 0x0005743C
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x00059258 File Offset: 0x00057458
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || object.ReferenceEquals(a, b);
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0005926C File Offset: 0x0005746C
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x04000CED RID: 3309
		private JSONNode m_Node;

		// Token: 0x04000CEE RID: 3310
		private string m_Key;
	}
}
