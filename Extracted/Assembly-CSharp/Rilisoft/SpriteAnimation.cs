using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200075D RID: 1885
	internal sealed class SpriteAnimation : UISpriteAnimation
	{
		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x0600422C RID: 16940 RVA: 0x0015FC4C File Offset: 0x0015DE4C
		// (set) Token: 0x0600422D RID: 16941 RVA: 0x0015FC54 File Offset: 0x0015DE54
		public bool SnapPixels
		{
			get
			{
				return this.mSnap;
			}
			set
			{
				this.mSnap = value;
			}
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x0015FC60 File Offset: 0x0015DE60
		protected override void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (!base.isPlaying)
			{
				return;
			}
			if (base.frames < 2)
			{
				return;
			}
			if ((float)base.framesPerSecond <= 0f)
			{
				return;
			}
			int num = Mathf.FloorToInt(Time.realtimeSinceStartup * (float)base.framesPerSecond);
			this.mIndex = num % base.frames;
			this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
			if (this.mSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}
}
