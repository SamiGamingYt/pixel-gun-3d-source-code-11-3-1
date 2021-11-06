using System;
using UnityEngine;

// Token: 0x020005CA RID: 1482
internal sealed class ControlSizeSlider : MonoBehaviour
{
	// Token: 0x1400004C RID: 76
	// (add) Token: 0x06003323 RID: 13091 RVA: 0x00108C74 File Offset: 0x00106E74
	// (remove) Token: 0x06003324 RID: 13092 RVA: 0x00108C90 File Offset: 0x00106E90
	public event EventHandler<ControlSizeSlider.EnabledChangedEventArgs> EnabledChanged;

	// Token: 0x06003325 RID: 13093 RVA: 0x00108CAC File Offset: 0x00106EAC
	private void OnEnable()
	{
		EventHandler<ControlSizeSlider.EnabledChangedEventArgs> enabledChanged = this.EnabledChanged;
		if (enabledChanged != null)
		{
			enabledChanged(this.slider, new ControlSizeSlider.EnabledChangedEventArgs
			{
				Enabled = true
			});
		}
	}

	// Token: 0x06003326 RID: 13094 RVA: 0x00108CE0 File Offset: 0x00106EE0
	private void OnDisable()
	{
		EventHandler<ControlSizeSlider.EnabledChangedEventArgs> enabledChanged = this.EnabledChanged;
		if (enabledChanged != null)
		{
			enabledChanged(this.slider, new ControlSizeSlider.EnabledChangedEventArgs
			{
				Enabled = false
			});
		}
	}

	// Token: 0x04002597 RID: 9623
	public UISlider slider;

	// Token: 0x020005CB RID: 1483
	public class EnabledChangedEventArgs : EventArgs
	{
		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06003328 RID: 13096 RVA: 0x00108D1C File Offset: 0x00106F1C
		// (set) Token: 0x06003329 RID: 13097 RVA: 0x00108D24 File Offset: 0x00106F24
		public bool Enabled { get; set; }
	}
}
