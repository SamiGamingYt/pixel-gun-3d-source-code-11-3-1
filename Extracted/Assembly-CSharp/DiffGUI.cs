using System;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class DiffGUI : MonoBehaviour
{
	// Token: 0x06000447 RID: 1095 RVA: 0x00024848 File Offset: 0x00022A48
	private void Start()
	{
		this.buttons.fontSize = Mathf.RoundToInt(16f * Defs.Coef);
		this._curInd = PlayerPrefs.GetInt(Defs.DiffSett, 1);
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00024884 File Offset: 0x00022A84
	private void OnGUI()
	{
		GUI.depth = -100;
		Rect position = new Rect((float)Screen.width - (float)this.fon.width * Defs.Coef, (float)Screen.height - (float)this.fon.height * Defs.Coef, (float)this.fon.width * Defs.Coef, (float)this.fon.height * Defs.Coef);
		GUI.DrawTexture(position, this.fon);
		float num = (float)this.buttons.normal.background.width * Defs.Coef;
		float num2 = (float)this.buttons.normal.background.height * Defs.Coef;
		float num3 = 14f * Defs.Coef;
		Rect position2 = new Rect(position.x + position.width - num3 - num - Defs.BottomOffs * Defs.Coef, position.y + position.height - num3 - num2 * 3f - Defs.BottomOffs * Defs.Coef, num, num2 * 3f);
		int num4 = GUI.SelectionGrid(position2, this._curInd, new string[]
		{
			"Easy",
			"Medium",
			"Hard"
		}, 1, this.buttons);
		if (num4 != this._curInd)
		{
			ButtonClickSound.Instance.PlayClick();
			this._curInd = num4;
			PlayerPrefs.SetInt(Defs.DiffSett, this._curInd);
			Defs.diffGame = this._curInd;
		}
		Rect position3 = new Rect(position2.x - (float)this.instr[this._curInd].width * Defs.Coef, position.y, (float)this.instr[this._curInd].width * Defs.Coef, (float)this.instr[this._curInd].height * Defs.Coef);
		GUI.DrawTexture(position3, this.instr[this._curInd]);
	}

	// Token: 0x040004D2 RID: 1234
	public GUIStyle buttons;

	// Token: 0x040004D3 RID: 1235
	public Texture fon;

	// Token: 0x040004D4 RID: 1236
	public Texture[] instr = new Texture[0];

	// Token: 0x040004D5 RID: 1237
	private int _curInd = 1;
}
