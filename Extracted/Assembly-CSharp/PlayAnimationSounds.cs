using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
public class PlayAnimationSounds : MonoBehaviour
{
	// Token: 0x060027DB RID: 10203 RVA: 0x000C7028 File Offset: 0x000C5228
	private void PlayAnimSound(int number)
	{
		this.aSource.pitch = 1f;
		if (Defs.isSoundFX)
		{
			this.aSource.loop = false;
			this.aSource.clip = this.sounds[number];
			this.aSource.Play();
		}
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x000C707C File Offset: 0x000C527C
	private void PlayAnimSoundPithced(int number)
	{
		this.aSource.pitch = UnityEngine.Random.Range(0.6f, 1.2f);
		if (Defs.isSoundFX)
		{
			this.aSource.loop = false;
			this.aSource.clip = this.sounds[number];
			this.aSource.Play();
		}
	}

	// Token: 0x04001C1C RID: 7196
	public AudioSource aSource;

	// Token: 0x04001C1D RID: 7197
	public AudioClip[] sounds;
}
