using System;
using UnityEngine;

// Token: 0x020007B9 RID: 1977
public class ShowNick : MonoBehaviour
{
	// Token: 0x060047A4 RID: 18340 RVA: 0x0018C148 File Offset: 0x0018A348
	private void Start()
	{
		this.koofHeight = (float)Screen.height / 768f;
		this.labelStyle.fontSize = Mathf.RoundToInt(20f * this.koofHeight);
	}

	// Token: 0x060047A5 RID: 18341 RVA: 0x0018C184 File Offset: 0x0018A384
	private void Update()
	{
	}

	// Token: 0x060047A6 RID: 18342 RVA: 0x0018C188 File Offset: 0x0018A388
	private void OnGUI()
	{
	}

	// Token: 0x040034D1 RID: 13521
	public string nick;

	// Token: 0x040034D2 RID: 13522
	public bool isShowNick;

	// Token: 0x040034D3 RID: 13523
	public GUIStyle labelStyle;

	// Token: 0x040034D4 RID: 13524
	private float koofHeight;
}
