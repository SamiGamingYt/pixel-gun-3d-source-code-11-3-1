using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000827 RID: 2087
	internal sealed class ProgressBounds
	{
		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x06004BEA RID: 19434 RVA: 0x001B5720 File Offset: 0x001B3920
		public float LowerBound
		{
			get
			{
				return this._lowerBound;
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x06004BEB RID: 19435 RVA: 0x001B5728 File Offset: 0x001B3928
		public float UpperBound
		{
			get
			{
				return this._upperBound;
			}
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x001B5730 File Offset: 0x001B3930
		public float Clamp(float progress)
		{
			return Mathf.Clamp(progress, this._lowerBound, this._upperBound);
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x001B5744 File Offset: 0x001B3944
		public float Lerp(float progress, float time)
		{
			return Mathf.Lerp(this.Clamp(progress), this.UpperBound, time);
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x001B575C File Offset: 0x001B395C
		public void SetBounds(float lowerBound, float upperBound)
		{
			lowerBound = Mathf.Clamp01(lowerBound);
			upperBound = Mathf.Clamp01(upperBound);
			if (lowerBound > upperBound)
			{
				throw new ArgumentException("Bounds are not ordered.");
			}
			this._lowerBound = lowerBound;
			this._upperBound = upperBound;
		}

		// Token: 0x04003B1B RID: 15131
		private float _lowerBound;

		// Token: 0x04003B1C RID: 15132
		private float _upperBound = 1f;
	}
}
