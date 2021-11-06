using System;
using UnityEngine;

// Token: 0x020004EB RID: 1259
public class ScoresLabel : MonoBehaviour
{
	// Token: 0x06002C83 RID: 11395 RVA: 0x000EBFC4 File Offset: 0x000EA1C4
	private void Start()
	{
		this.isHunger = Defs.isHunger;
		base.gameObject.SetActive(Defs.IsSurvival || Defs.isCOOP || this.isHunger);
		this._label = base.GetComponent<UILabel>();
		this.scoreLocalize = ((!this.isHunger) ? LocalizationStore.Key_0190 : LocalizationStore.Key_0351);
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x000EC030 File Offset: 0x000EA230
	private void Update()
	{
		if (this.isHunger)
		{
			this._label.text = string.Format("{0}", (Initializer.players == null) ? 0 : (Initializer.players.Count - 1));
		}
		else
		{
			this._label.text = string.Format("{0}", GlobalGameController.Score);
		}
	}

	// Token: 0x04002190 RID: 8592
	private UILabel _label;

	// Token: 0x04002191 RID: 8593
	private bool isHunger;

	// Token: 0x04002192 RID: 8594
	private string scoreLocalize;
}
