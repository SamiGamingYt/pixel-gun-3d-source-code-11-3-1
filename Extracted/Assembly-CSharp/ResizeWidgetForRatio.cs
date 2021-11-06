using System;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
public class ResizeWidgetForRatio : MonoBehaviour
{
	// Token: 0x06002BAF RID: 11183 RVA: 0x000E5E7C File Offset: 0x000E407C
	private void Start()
	{
		if (this.widgets != null)
		{
			if ((double)((float)Screen.width / (float)Screen.height) > 1.5)
			{
				for (int i = 0; i < this.widgets.Length; i++)
				{
					this.widgets[i].width = Mathf.RoundToInt(this.sizeFor16x9.x);
					this.widgets[i].height = Mathf.RoundToInt(this.sizeFor16x9.y);
				}
				if (this.isLabels)
				{
					for (int j = 0; j < this.widgets.Length; j++)
					{
						if (!(this.widgets[j].GetComponent<UILabel>() == null))
						{
							this.widgets[j].GetComponent<UILabel>().fontSize = this.fontSizeFor16x9;
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < this.widgets.Length; k++)
				{
					this.widgets[k].width = Mathf.RoundToInt(this.sizeFor4x3.x);
					this.widgets[k].height = Mathf.RoundToInt(this.sizeFor4x3.y);
				}
				if (this.isLabels)
				{
					for (int l = 0; l < this.widgets.Length; l++)
					{
						if (!(this.widgets[l].GetComponent<UILabel>() == null))
						{
							this.widgets[l].GetComponent<UILabel>().fontSize = this.fontSizeFor4x3;
						}
					}
				}
			}
		}
	}

	// Token: 0x040020A2 RID: 8354
	[SerializeField]
	private UIWidget[] widgets;

	// Token: 0x040020A3 RID: 8355
	[SerializeField]
	private Vector2 sizeFor4x3;

	// Token: 0x040020A4 RID: 8356
	[SerializeField]
	private Vector2 sizeFor16x9;

	// Token: 0x040020A5 RID: 8357
	public bool isLabels;

	// Token: 0x040020A6 RID: 8358
	[SerializeField]
	private int fontSizeFor4x3;

	// Token: 0x040020A7 RID: 8359
	[SerializeField]
	private int fontSizeFor16x9;
}
