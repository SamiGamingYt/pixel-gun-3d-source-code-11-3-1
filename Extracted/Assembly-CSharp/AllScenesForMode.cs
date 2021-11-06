using System;
using System.Collections.Generic;

// Token: 0x0200074B RID: 1867
[Serializable]
public class AllScenesForMode
{
	// Token: 0x06004190 RID: 16784 RVA: 0x0015D130 File Offset: 0x0015B330
	public void AddInfoScene(SceneInfo needInfo)
	{
		this.avaliableScenes.Add(needInfo);
	}

	// Token: 0x04002FF2 RID: 12274
	public TypeModeGame mode;

	// Token: 0x04002FF3 RID: 12275
	public List<SceneInfo> avaliableScenes = new List<SceneInfo>();
}
