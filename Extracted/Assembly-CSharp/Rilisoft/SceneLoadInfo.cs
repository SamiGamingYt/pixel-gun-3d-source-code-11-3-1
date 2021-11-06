using System;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	// Token: 0x0200074E RID: 1870
	public struct SceneLoadInfo
	{
		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x0015D998 File Offset: 0x0015BB98
		// (set) Token: 0x060041B3 RID: 16819 RVA: 0x0015D9A0 File Offset: 0x0015BBA0
		public string SceneName { get; set; }

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x0015D9AC File Offset: 0x0015BBAC
		// (set) Token: 0x060041B5 RID: 16821 RVA: 0x0015D9B4 File Offset: 0x0015BBB4
		public LoadSceneMode LoadMode { get; set; }
	}
}
