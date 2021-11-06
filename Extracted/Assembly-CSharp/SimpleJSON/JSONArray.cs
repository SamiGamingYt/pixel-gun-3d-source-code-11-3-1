using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SimpleJSON
{
	// Token: 0x020002B6 RID: 694
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x17000271 RID: 625
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
				}
				else
				{
					this.m_List[aIndex] = value;
				}
			}
		}

		// Token: 0x17000272 RID: 626
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.m_List.Add(value);
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060015E0 RID: 5600 RVA: 0x000586D4 File Offset: 0x000568D4
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x000586E4 File Offset: 0x000568E4
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x000586F4 File Offset: 0x000568F4
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode result = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return result;
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x00058738 File Offset: 0x00056938
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060015E4 RID: 5604 RVA: 0x00058748 File Offset: 0x00056948
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (JSONNode N in this.m_List)
				{
					yield return N;
				}
				yield break;
			}
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0005876C File Offset: 0x0005696C
		public IEnumerator GetEnumerator()
		{
			foreach (JSONNode N in this.m_List)
			{
				yield return N;
			}
			yield break;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00058788 File Offset: 0x00056988
		public override string ToString()
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text += jsonnode.ToString();
			}
			text += " ]";
			return text;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00058820 File Offset: 0x00056A20
		public override string ToString(string aPrefix)
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text += jsonnode.ToString(aPrefix + "   ");
			}
			text = text + "\n" + aPrefix + "]";
			return text;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x000588DC File Offset: 0x00056ADC
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x04000CEA RID: 3306
		private List<JSONNode> m_List = new List<JSONNode>();
	}
}
