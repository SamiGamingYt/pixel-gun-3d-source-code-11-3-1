using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x02000779 RID: 1913
[RequireComponent(typeof(UIInput))]
public class UIInputFocusAtEnable : MonoBehaviour
{
	// Token: 0x06004335 RID: 17205 RVA: 0x00166FC0 File Offset: 0x001651C0
	private void Awake()
	{
		this._input = base.GetComponent<UIInput>();
		if (this._input == null)
		{
			Debug.LogError("input not found");
		}
	}

	// Token: 0x06004336 RID: 17206 RVA: 0x00166FEC File Offset: 0x001651EC
	private void OnEnable()
	{
		if (this._onlyOnce && this._alreadyTurned)
		{
			return;
		}
		base.StartCoroutine(this.SetSelected());
		this._alreadyTurned = true;
	}

	// Token: 0x06004337 RID: 17207 RVA: 0x0016701C File Offset: 0x0016521C
	private IEnumerator SetSelected()
	{
		this._input.isSelected = false;
		yield return null;
		while (!this._input.isSelected)
		{
			yield return new WaitForRealSeconds(0.3f);
			this._input.isSelected = true;
		}
		yield break;
	}

	// Token: 0x0400313B RID: 12603
	private const float FOCUS_DELAY = 0.3f;

	// Token: 0x0400313C RID: 12604
	[SerializeField]
	[Tooltip("Применить только один раз")]
	private bool _onlyOnce;

	// Token: 0x0400313D RID: 12605
	[SerializeField]
	[ReadOnly]
	private UIInput _input;

	// Token: 0x0400313E RID: 12606
	private bool _alreadyTurned;
}
