using System;
using UnityEngine;

// Token: 0x020002EA RID: 746
[ExecuteInEditMode]
public class Label3d : MonoBehaviour
{
	// Token: 0x06001A1E RID: 6686 RVA: 0x000697A0 File Offset: 0x000679A0
	private void Create3dText()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<Label3d>());
		gameObject.GetComponent<UILabel>().depth = gameObject.GetComponent<UILabel>().depth - 2;
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		UnityEngine.Object.DestroyImmediate(gameObject2.GetComponent<Label3d>());
		gameObject2.transform.parent = base.transform;
		gameObject2.GetComponent<UILabel>().depth = gameObject2.GetComponent<UILabel>().depth - 1;
		gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject2.transform.localPosition = new Vector3(0f, this.offset, 0f);
		gameObject2.GetComponent<UILabel>().color = this.shadedColor;
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
		base.gameObject.GetComponent<UILabel>().effectStyle = UILabel.Effect.None;
		base.gameObject.SetActive(false);
		this.DeleteScript();
	}

	// Token: 0x06001A1F RID: 6687 RVA: 0x000698E0 File Offset: 0x00067AE0
	private void Update()
	{
		if (!this.apply)
		{
			return;
		}
		this.apply = false;
		this.Create3dText();
	}

	// Token: 0x06001A20 RID: 6688 RVA: 0x000698FC File Offset: 0x00067AFC
	private void DeleteScript()
	{
		base.gameObject.SetActive(true);
		UnityEngine.Object.DestroyImmediate(base.gameObject.GetComponent<Label3d>());
	}

	// Token: 0x04000F3A RID: 3898
	public bool apply;

	// Token: 0x04000F3B RID: 3899
	public Color shadedColor = Color.gray;

	// Token: 0x04000F3C RID: 3900
	public float offset = -3f;
}
