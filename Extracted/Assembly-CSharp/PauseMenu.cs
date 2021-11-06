using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004DB RID: 1243
public class PauseMenu : MonoBehaviour
{
	// Token: 0x06002C53 RID: 11347 RVA: 0x000EB378 File Offset: 0x000E9578
	private void Awake()
	{
		this.m_MenuToggle = base.GetComponent<Toggle>();
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x000EB388 File Offset: 0x000E9588
	private void MenuOn()
	{
		this.m_TimeScaleRef = Time.timeScale;
		Time.timeScale = 0f;
		this.m_VolumeRef = AudioListener.volume;
		AudioListener.volume = 0f;
		this.m_Paused = true;
	}

	// Token: 0x06002C55 RID: 11349 RVA: 0x000EB3BC File Offset: 0x000E95BC
	public void MenuOff()
	{
		Time.timeScale = this.m_TimeScaleRef;
		AudioListener.volume = this.m_VolumeRef;
		this.m_Paused = false;
	}

	// Token: 0x06002C56 RID: 11350 RVA: 0x000EB3DC File Offset: 0x000E95DC
	public void OnMenuStatusChange()
	{
		if (this.m_MenuToggle.isOn && !this.m_Paused)
		{
			this.MenuOn();
		}
		else if (!this.m_MenuToggle.isOn && this.m_Paused)
		{
			this.MenuOff();
		}
	}

	// Token: 0x04002159 RID: 8537
	private Toggle m_MenuToggle;

	// Token: 0x0400215A RID: 8538
	private float m_TimeScaleRef = 1f;

	// Token: 0x0400215B RID: 8539
	private float m_VolumeRef = 1f;

	// Token: 0x0400215C RID: 8540
	private bool m_Paused;
}
