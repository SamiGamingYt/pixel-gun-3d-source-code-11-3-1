using System;
using System.IO;

namespace SimpleJSON
{
	// Token: 0x020002B8 RID: 696
	public class JSONData : JSONNode
	{
		// Token: 0x060015F8 RID: 5624 RVA: 0x00058E50 File Offset: 0x00057050
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00058E60 File Offset: 0x00057060
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00058E70 File Offset: 0x00057070
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x00058E80 File Offset: 0x00057080
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00058E90 File Offset: 0x00057090
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x060015FD RID: 5629 RVA: 0x00058EA0 File Offset: 0x000570A0
		// (set) Token: 0x060015FE RID: 5630 RVA: 0x00058EA8 File Offset: 0x000570A8
		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x00058EB4 File Offset: 0x000570B4
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00058ED0 File Offset: 0x000570D0
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x00058EEC File Offset: 0x000570EC
		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jsondata = new JSONData(string.Empty);
			jsondata.AsInt = this.AsInt;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(4);
				aWriter.Write(this.AsInt);
				return;
			}
			jsondata.AsFloat = this.AsFloat;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(7);
				aWriter.Write(this.AsFloat);
				return;
			}
			jsondata.AsDouble = this.AsDouble;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(5);
				aWriter.Write(this.AsDouble);
				return;
			}
			jsondata.AsBool = this.AsBool;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(6);
				aWriter.Write(this.AsBool);
				return;
			}
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x04000CEC RID: 3308
		private string m_Data;
	}
}
