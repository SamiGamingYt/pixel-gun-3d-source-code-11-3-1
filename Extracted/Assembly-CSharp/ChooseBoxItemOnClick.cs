using System;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class ChooseBoxItemOnClick : MonoBehaviour
{
	// Token: 0x060002D2 RID: 722 RVA: 0x0001873C File Offset: 0x0001693C
	private void OnClick()
	{
		if (base.gameObject.name.Contains((ChooseBox.instance.nguiController.selectIndexMap + 1).ToString()))
		{
			ChooseBox.instance.StartNameBox(base.gameObject.name);
		}
		else
		{
			ButtonClickSound.TryPlayClick();
			MyCenterOnChild component = base.transform.parent.GetComponent<MyCenterOnChild>();
			if (component != null)
			{
				component.CenterOn(base.transform);
			}
		}
	}
}
