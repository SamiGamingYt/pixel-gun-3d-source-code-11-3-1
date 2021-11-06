using System;
using UnityEngine;

// Token: 0x0200034D RID: 845
[AddComponentMenu("NGUI/Interaction/Sound Volume")]
[RequireComponent(typeof(UISlider))]
public class UISoundVolume : MonoBehaviour
{
	// Token: 0x06001D29 RID: 7465 RVA: 0x0007C3A8 File Offset: 0x0007A5A8
	private void Awake()
	{
		UISlider component = base.GetComponent<UISlider>();
		component.value = NGUITools.soundVolume;
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.OnChange));
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x0007C3E0 File Offset: 0x0007A5E0
	private void OnChange()
	{
		NGUITools.soundVolume = UIProgressBar.current.value;
	}
}
