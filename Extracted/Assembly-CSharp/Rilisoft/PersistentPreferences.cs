using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020003DF RID: 991
	internal sealed class PersistentPreferences : Preferences
	{
		// Token: 0x060023B6 RID: 9142 RVA: 0x000B1F24 File Offset: 0x000B0124
		public PersistentPreferences()
		{
			try
			{
				this._doc = XDocument.Load(PersistentPreferences._path);
			}
			catch (Exception)
			{
				this._doc = new XDocument(new object[]
				{
					new XElement("Preferences")
				});
				this._doc.Save(PersistentPreferences._path);
			}
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x000B1FBC File Offset: 0x000B01BC
		protected override void AddCore(string key, string value)
		{
			XElement xelement = this._doc.Root.Elements("Preference").FirstOrDefault((XElement e) => e.Element("Key") != null && e.Element("Key").Value.Equals(key));
			if (xelement != null)
			{
				xelement.Remove();
			}
			XElement content = new XElement("Preference", new object[]
			{
				new XElement("Key", key),
				new XElement("Value", value)
			});
			this._doc.Root.Add(content);
			this._doc.Save(PersistentPreferences._path);
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000B2070 File Offset: 0x000B0270
		protected override bool ContainsKeyCore(string key)
		{
			return this._doc.Root.Elements("Preference").Any((XElement e) => e.Element("Key") != null && e.Element("Key").Value.Equals(key));
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000B20B8 File Offset: 0x000B02B8
		protected override void CopyToCore(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x000B20C0 File Offset: 0x000B02C0
		protected override bool RemoveCore(string key)
		{
			XElement xelement = this._doc.Root.Elements("Preference").FirstOrDefault((XElement e) => e.Element("Key") != null && e.Element("Key").Value.Equals(key));
			if (xelement != null)
			{
				xelement.Remove();
				this._doc.Save(PersistentPreferences._path);
				return true;
			}
			return false;
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x000B2128 File Offset: 0x000B0328
		protected override bool TryGetValueCore(string key, out string value)
		{
			XElement xelement = this._doc.Root.Elements("Preference").FirstOrDefault((XElement e) => e.Element("Key") != null && e.Element("Key").Value.Equals(key));
			if (xelement != null)
			{
				XElement xelement2 = xelement.Element("Value");
				if (xelement2 != null)
				{
					value = xelement2.Value;
					return true;
				}
			}
			value = null;
			return false;
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x000B219C File Offset: 0x000B039C
		public override void Save()
		{
			this._doc.Save(PersistentPreferences._path);
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060023BE RID: 9150 RVA: 0x000B21B0 File Offset: 0x000B03B0
		public override ICollection<string> Keys
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060023BF RID: 9151 RVA: 0x000B21B8 File Offset: 0x000B03B8
		public override ICollection<string> Values
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x000B21C0 File Offset: 0x000B03C0
		public override void Clear()
		{
			this._doc.Root.RemoveNodes();
			this._doc.Save(PersistentPreferences._path);
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060023C1 RID: 9153 RVA: 0x000B21F0 File Offset: 0x000B03F0
		public override int Count
		{
			get
			{
				return this._doc.Root.Elements().Count<XElement>();
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060023C2 RID: 9154 RVA: 0x000B2208 File Offset: 0x000B0408
		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000B220C File Offset: 0x000B040C
		public override IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x060023C4 RID: 9156 RVA: 0x000B2214 File Offset: 0x000B0414
		internal static string Path
		{
			get
			{
				return PersistentPreferences._path;
			}
		}

		// Token: 0x0400182F RID: 6191
		private const string KeyElement = "Key";

		// Token: 0x04001830 RID: 6192
		private const string PreferenceElement = "Preference";

		// Token: 0x04001831 RID: 6193
		private const string RootElement = "Preferences";

		// Token: 0x04001832 RID: 6194
		private const string ValueElement = "Value";

		// Token: 0x04001833 RID: 6195
		private readonly XDocument _doc;

		// Token: 0x04001834 RID: 6196
		private static readonly string _path = System.IO.Path.Combine(Application.persistentDataPath, "com.P3D.Pixlgun.Settings.xml");
	}
}
