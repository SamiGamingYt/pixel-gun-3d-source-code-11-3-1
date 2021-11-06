using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000540 RID: 1344
	internal sealed class InterstitialCounter
	{
		// Token: 0x06002EC9 RID: 11977 RVA: 0x000F49E0 File Offset: 0x000F2BE0
		private InterstitialCounter()
		{
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06002ECB RID: 11979 RVA: 0x000F49F4 File Offset: 0x000F2BF4
		public static InterstitialCounter Instance
		{
			get
			{
				return InterstitialCounter.s_instance;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06002ECC RID: 11980 RVA: 0x000F49FC File Offset: 0x000F2BFC
		public int RealInterstitialCount
		{
			get
			{
				return this._realInterstitialCount;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06002ECD RID: 11981 RVA: 0x000F4A04 File Offset: 0x000F2C04
		public int FakeInterstitialCount
		{
			get
			{
				return this._fakeInterstitialCount;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06002ECE RID: 11982 RVA: 0x000F4A0C File Offset: 0x000F2C0C
		public int TotalInterstitialCount
		{
			get
			{
				return this._realInterstitialCount + this._fakeInterstitialCount;
			}
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x000F4A1C File Offset: 0x000F2C1C
		public void Reset()
		{
			this._realInterstitialCount = 0;
			this._fakeInterstitialCount = 0;
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x000F4A2C File Offset: 0x000F2C2C
		public void IncrementRealInterstitialCount()
		{
			this._realInterstitialCount++;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log(this.ToString());
			}
		}

		// Token: 0x06002ED1 RID: 11985 RVA: 0x000F4A54 File Offset: 0x000F2C54
		public void IncrementFakeInterstitialCount()
		{
			this._fakeInterstitialCount++;
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log(this.ToString());
			}
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x000F4A7C File Offset: 0x000F2C7C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "realInterstitialCount: {0}, fakeInterstitialCount: {1}", new object[]
			{
				this.RealInterstitialCount,
				this.FakeInterstitialCount
			});
		}

		// Token: 0x04002298 RID: 8856
		private int _realInterstitialCount;

		// Token: 0x04002299 RID: 8857
		private int _fakeInterstitialCount;

		// Token: 0x0400229A RID: 8858
		private static readonly InterstitialCounter s_instance = new InterstitialCounter();
	}
}
