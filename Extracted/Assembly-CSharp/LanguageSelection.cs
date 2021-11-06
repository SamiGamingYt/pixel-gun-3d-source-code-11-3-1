using System;
using UnityEngine;

// Token: 0x02000316 RID: 790
[AddComponentMenu("NGUI/Interaction/Language Selection")]
[RequireComponent(typeof(UIPopupList))]
public class LanguageSelection : MonoBehaviour
{
	// Token: 0x06001B7F RID: 7039 RVA: 0x000709C4 File Offset: 0x0006EBC4
	private void Awake()
	{
		this.mList = base.GetComponent<UIPopupList>();
		this.Refresh();
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x000709D8 File Offset: 0x0006EBD8
	private void Start()
	{
		EventDelegate.Add(this.mList.onChange, delegate()
		{
			Localization.language = UIPopupList.current.value;
		});
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x00070A14 File Offset: 0x0006EC14
	public void Refresh()
	{
		if (this.mList != null && Localization.knownLanguages != null)
		{
			this.mList.Clear();
			int i = 0;
			int num = Localization.knownLanguages.Length;
			while (i < num)
			{
				this.mList.items.Add(Localization.knownLanguages[i]);
				i++;
			}
			this.mList.value = Localization.language;
		}
	}

	// Token: 0x04001093 RID: 4243
	private UIPopupList mList;
}
