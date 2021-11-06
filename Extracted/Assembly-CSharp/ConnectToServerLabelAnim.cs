using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class ConnectToServerLabelAnim : MonoBehaviour
{
	// Token: 0x060003DC RID: 988 RVA: 0x000220BC File Offset: 0x000202BC
	private void Start()
	{
		this.timer = this.maxTimer;
		this.startText = LocalizationStore.Key_0564;
		this.myLabel.text = this.startText;
	}

	// Token: 0x060003DD RID: 989 RVA: 0x000220F4 File Offset: 0x000202F4
	private void Update()
	{
		this.timer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			this.timer = this.maxTimer;
			this.stateLabel++;
			if (this.stateLabel > 3)
			{
				this.stateLabel = 0;
			}
			string text = string.Empty;
			for (int i = 0; i < this.stateLabel; i++)
			{
				text += ".";
			}
			this.myLabel.text = string.Format("{0} {1}", this.startText, text);
		}
	}

	// Token: 0x04000469 RID: 1129
	public UILabel myLabel;

	// Token: 0x0400046A RID: 1130
	public string startText;

	// Token: 0x0400046B RID: 1131
	private int stateLabel;

	// Token: 0x0400046C RID: 1132
	private float timer;

	// Token: 0x0400046D RID: 1133
	private float maxTimer = 1f;
}
