using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002C1 RID: 705
	public class LanguagePopup : MonoBehaviour
	{
		// Token: 0x0600164C RID: 5708 RVA: 0x00059FD4 File Offset: 0x000581D4
		private void Start()
		{
			UIPopupList component = base.GetComponent<UIPopupList>();
			component.items = this.Source.GetLanguages();
			EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnValueChange));
			component.value = LocalizationManager.CurrentLanguage;
		}

		// Token: 0x0600164D RID: 5709 RVA: 0x0005A01C File Offset: 0x0005821C
		public void OnValueChange()
		{
			LocalizationStore.CurrentLanguage = UIPopupList.current.value;
		}

		// Token: 0x04000D08 RID: 3336
		public LanguageSource Source;
	}
}
