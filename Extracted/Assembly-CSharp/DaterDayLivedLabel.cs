using System;
using I2.Loc;
using UnityEngine;

// Token: 0x0200008F RID: 143
public class DaterDayLivedLabel : MonoBehaviour
{
	// Token: 0x06000426 RID: 1062 RVA: 0x00023B50 File Offset: 0x00021D50
	private void Awake()
	{
		this.myLabel = base.GetComponent<UILabel>();
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x00023B60 File Offset: 0x00021D60
	private void SetText()
	{
		this.myLabel.text = LocalizationStore.Get("Key_1615") + ": " + Storager.getInt("DaterDayLived", false);
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00023B94 File Offset: 0x00021D94
	private void OnEnable()
	{
		this.SetText();
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00023B9C File Offset: 0x00021D9C
	private void Start()
	{
		LocalizationStore.AddEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00023BB0 File Offset: 0x00021DB0
	private void OnDestroy()
	{
		LocalizationStore.DelEventCallAfterLocalize(new LocalizationManager.OnLocalizeCallback(this.HandleLocalizationChanged));
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00023BC4 File Offset: 0x00021DC4
	private void HandleLocalizationChanged()
	{
		this.SetText();
	}

	// Token: 0x040004B7 RID: 1207
	private UILabel myLabel;
}
