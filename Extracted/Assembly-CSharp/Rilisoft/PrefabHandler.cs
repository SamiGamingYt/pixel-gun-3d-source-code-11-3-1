using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000703 RID: 1795
	[Serializable]
	public class PrefabHandler
	{
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06003E60 RID: 15968 RVA: 0x0014E9C0 File Offset: 0x0014CBC0
		public string ResourcePath
		{
			get
			{
				return PrefabHandler.ToResourcePath(this.FullPath);
			}
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x0014E9D0 File Offset: 0x0014CBD0
		public static string ToResourcePath(string fullPath)
		{
			if (fullPath.IsNullOrEmpty())
			{
				return string.Empty;
			}
			List<string> list = fullPath.Split(new char[]
			{
				(!fullPath.Contains("/")) ? '\\' : '/'
			}).ToList<string>();
			if (list.Count > 0 && list[0].Contains("Assets"))
			{
				list.RemoveAt(0);
			}
			if (list.Count > 0 && list[0].Contains("Resources"))
			{
				list.RemoveAt(0);
			}
			string separator = Path.DirectorySeparatorChar.ToString();
			fullPath = string.Join(separator, list.ToArray());
			fullPath = Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileNameWithoutExtension(fullPath));
			return fullPath;
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06003E62 RID: 15970 RVA: 0x0014EAA0 File Offset: 0x0014CCA0
		public GameObject Prefab
		{
			get
			{
				if (this._prefab == null)
				{
					this._prefab = Resources.Load<GameObject>(this.ResourcePath);
				}
				return this._prefab;
			}
		}

		// Token: 0x04002E17 RID: 11799
		[SerializeField]
		public string FullPath;

		// Token: 0x04002E18 RID: 11800
		public GameObject _prefab;
	}
}
