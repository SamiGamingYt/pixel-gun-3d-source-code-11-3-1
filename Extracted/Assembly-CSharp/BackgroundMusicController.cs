using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
internal sealed class BackgroundMusicController : MonoBehaviour
{
	// Token: 0x060000E8 RID: 232 RVA: 0x000082FC File Offset: 0x000064FC
	private void Start()
	{
		MenuBackgroundMusic.sharedMusic.PlayMusic(base.GetComponent<AudioSource>());
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00008310 File Offset: 0x00006510
	public void Play()
	{
		MenuBackgroundMusic.sharedMusic.PlayMusic(base.GetComponent<AudioSource>());
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00008324 File Offset: 0x00006524
	public void Stop()
	{
		MenuBackgroundMusic.sharedMusic.StopMusic(base.GetComponent<AudioSource>());
	}
}
