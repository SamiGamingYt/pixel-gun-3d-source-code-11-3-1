using System;
using UnityEngine;

// Token: 0x020004CA RID: 1226
public class AudioSourseOnOff : MonoBehaviour
{
	// Token: 0x06002BB9 RID: 11193 RVA: 0x000E61A4 File Offset: 0x000E43A4
	private void Awake()
	{
		this.myAudioSources = base.GetComponents<AudioSource>();
		for (int i = 0; i < this.myAudioSources.Length; i++)
		{
			if (this.myAudioSources[i] != null)
			{
				this.myAudioSources[i].enabled = Defs.isSoundFX;
			}
		}
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x000E61FC File Offset: 0x000E43FC
	private void Update()
	{
		for (int i = 0; i < this.myAudioSources.Length; i++)
		{
			if (this.myAudioSources[i] != null && this.myAudioSources[i].enabled != Defs.isSoundFX)
			{
				this.myAudioSources[i].enabled = Defs.isSoundFX;
			}
		}
	}

	// Token: 0x040020AC RID: 8364
	private AudioSource[] myAudioSources;
}
