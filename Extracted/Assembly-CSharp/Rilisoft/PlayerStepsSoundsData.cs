using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000562 RID: 1378
	[Serializable]
	public class PlayerStepsSoundsData
	{
		// Token: 0x06002FD1 RID: 12241 RVA: 0x000F9C60 File Offset: 0x000F7E60
		public void SetTo(SkinName data)
		{
			data.walkAudio = this.Walk;
			data.jumpAudio = this.Jump;
			data.jumpDownAudio = this.JumpDown;
			data.walkMech = this.WalkMech;
			data.walkMechBear = this.WalkMechBear;
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000F9CAC File Offset: 0x000F7EAC
		public bool IsSettedTo(SkinName data)
		{
			return data.walkAudio == this.Walk && data.jumpAudio == this.Jump && data.jumpDownAudio == this.JumpDown && data.walkMech == this.WalkMech && data.walkMechBear == this.WalkMechBear;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000F9D28 File Offset: 0x000F7F28
		public static PlayerStepsSoundsData Create(SkinName data)
		{
			return new PlayerStepsSoundsData
			{
				Walk = data.walkAudio,
				Jump = data.jumpAudio,
				JumpDown = data.jumpDownAudio,
				WalkMech = data.walkMech,
				WalkMechBear = data.walkMechBear
			};
		}

		// Token: 0x04002327 RID: 8999
		public AudioClip Walk;

		// Token: 0x04002328 RID: 9000
		public AudioClip Jump;

		// Token: 0x04002329 RID: 9001
		public AudioClip JumpDown;

		// Token: 0x0400232A RID: 9002
		public AudioClip WalkMech;

		// Token: 0x0400232B RID: 9003
		public AudioClip WalkMechBear;
	}
}
