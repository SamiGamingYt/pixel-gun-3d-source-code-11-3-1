using System;
using I2.Loc;
using UnityEngine;

// Token: 0x020002C5 RID: 709
[ExecuteInEditMode]
public class LocalizeRili : MonoBehaviour
{
	// Token: 0x060016C4 RID: 5828 RVA: 0x0005B710 File Offset: 0x00059910
	private void Start()
	{
		if (base.gameObject.GetComponent<UILabel>() != null)
		{
			this.labels = new GameObject[]
			{
				base.gameObject
			};
		}
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x0005B748 File Offset: 0x00059948
	private void Update()
	{
		if (!this.execute)
		{
			return;
		}
		if (this.labels == null)
		{
			return;
		}
		foreach (GameObject gameObject in this.labels)
		{
			while (gameObject.GetComponent<Localize>() != null)
			{
				Localize component = gameObject.GetComponent<Localize>();
				UnityEngine.Object.DestroyImmediate(component);
			}
			Localize localize = gameObject.gameObject.AddComponent<Localize>();
			localize.SetTerm("Key_04B_03", "Key_04B_03");
			if (this.term != string.Empty)
			{
				Localize localize2 = gameObject.gameObject.AddComponent<Localize>();
				localize2.SetTerm(this.term, this.term);
			}
		}
		this.execute = false;
		if (this.selfDestroy)
		{
			UnityEngine.Object.DestroyImmediate(this);
		}
	}

	// Token: 0x04000D2C RID: 3372
	public GameObject[] labels;

	// Token: 0x04000D2D RID: 3373
	public string term;

	// Token: 0x04000D2E RID: 3374
	public bool execute;

	// Token: 0x04000D2F RID: 3375
	[Header("delete after execute?")]
	public bool selfDestroy;
}
