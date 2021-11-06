using System;

// Token: 0x020006FF RID: 1791
public class PixelGunPlaySound : UIPlaySound
{
	// Token: 0x06003E3A RID: 15930 RVA: 0x0014DC24 File Offset: 0x0014BE24
	private void OnClick()
	{
		if (Defs.isSoundFX && base.canPlay && this.trigger == UIPlaySound.Trigger.OnClick)
		{
			NGUITools.PlaySound(this.audioClip, this.volume, this.pitch);
		}
	}
}
