using System;
using UnityEngine;

// Token: 0x020007A6 RID: 1958
public class SetScalePreviews : MonoBehaviour
{
	// Token: 0x060045E6 RID: 17894 RVA: 0x00179EB8 File Offset: 0x001780B8
	private void Start()
	{
		this.widthCell = ConnectSceneNGUIController.sharedController.widthCell;
	}

	// Token: 0x060045E7 RID: 17895 RVA: 0x00179ECC File Offset: 0x001780CC
	public void LateUpdate()
	{
		MapPreviewController[] componentsInChildren = base.transform.GetComponentsInChildren<MapPreviewController>();
		if (componentsInChildren != null)
		{
			float x = this.myScrollPanel.clipOffset.x;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				float num = 1f - Mathf.Abs(componentsInChildren[i].transform.localPosition.x - x) / this.widthCell * 0.1f;
				if (num <= 0f)
				{
					num = 0.1f;
				}
				componentsInChildren[i].transform.localScale = new Vector3(num, num, num);
			}
		}
	}

	// Token: 0x0400333C RID: 13116
	public UIPanel myScrollPanel;

	// Token: 0x0400333D RID: 13117
	private float widthCell;
}
