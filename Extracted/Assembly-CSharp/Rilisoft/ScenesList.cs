using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000750 RID: 1872
	[Serializable]
	public class ScenesList : ScriptableObject
	{
		// Token: 0x0400300B RID: 12299
		[ReadOnly]
		[SerializeField]
		public List<ExistsSceneInfo> Infos = new List<ExistsSceneInfo>();
	}
}
