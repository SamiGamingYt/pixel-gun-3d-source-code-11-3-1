using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006DB RID: 1755
	[Serializable]
	public class PetAnimation
	{
		// Token: 0x04002D03 RID: 11523
		[SerializeField]
		public PetAnimationType Type;

		// Token: 0x04002D04 RID: 11524
		[SerializeField]
		public string AnimationName;

		// Token: 0x04002D05 RID: 11525
		[SerializeField]
		public float SpeedModificator = 1f;

		// Token: 0x04002D06 RID: 11526
		[SerializeField]
		[ReadOnly]
		public float CurrentPlaySpeed;
	}
}
