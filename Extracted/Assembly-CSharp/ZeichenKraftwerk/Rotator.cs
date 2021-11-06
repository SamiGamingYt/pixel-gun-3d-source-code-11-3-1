using System;
using UnityEngine;

namespace ZeichenKraftwerk
{
	// Token: 0x02000891 RID: 2193
	public sealed class Rotator : MonoBehaviour
	{
		// Token: 0x06004EE2 RID: 20194 RVA: 0x001C99F4 File Offset: 0x001C7BF4
		public void Start()
		{
			this.myTransform = base.transform;
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x001C9A04 File Offset: 0x001C7C04
		private void Update()
		{
			this.myTransform.Rotate(this.eulersPerSecond * RealTime.deltaTime);
		}

		// Token: 0x04003D55 RID: 15701
		public Vector3 eulersPerSecond = new Vector3(0f, 0f, 1f);

		// Token: 0x04003D56 RID: 15702
		private Transform myTransform;
	}
}
