using System;

// Token: 0x02000852 RID: 2130
public class RandomGenerator
{
	// Token: 0x06004D25 RID: 19749 RVA: 0x001BDA34 File Offset: 0x001BBC34
	public RandomGenerator(uint val)
	{
		this.SetSeed(val);
	}

	// Token: 0x06004D26 RID: 19750 RVA: 0x001BDA44 File Offset: 0x001BBC44
	public RandomGenerator()
	{
		this.SetSeed(RandomGenerator.counter++);
	}

	// Token: 0x06004D28 RID: 19752 RVA: 0x001BDA64 File Offset: 0x001BBC64
	public uint GenerateUint()
	{
		uint num = this.a ^ this.a << 11;
		this.a = this.b;
		this.b = this.c;
		this.c = this.d;
		return this.d = (this.d ^ this.d >> 19 ^ (num ^ num >> 8));
	}

	// Token: 0x06004D29 RID: 19753 RVA: 0x001BDAC8 File Offset: 0x001BBCC8
	public int Range(int max)
	{
		return (int)((ulong)this.GenerateUint() % (ulong)((long)max));
	}

	// Token: 0x06004D2A RID: 19754 RVA: 0x001BDAD8 File Offset: 0x001BBCD8
	public int Range(int min, int max)
	{
		return min + (int)((ulong)this.GenerateUint() % (ulong)((long)(max - min)));
	}

	// Token: 0x06004D2B RID: 19755 RVA: 0x001BDAEC File Offset: 0x001BBCEC
	public float GenerateFloat()
	{
		return 2.3283064E-10f * this.GenerateUint();
	}

	// Token: 0x06004D2C RID: 19756 RVA: 0x001BDAFC File Offset: 0x001BBCFC
	public float GenerateRangeFloat()
	{
		uint num = this.GenerateUint();
		return 4.656613E-10f * (float)num;
	}

	// Token: 0x06004D2D RID: 19757 RVA: 0x001BDB18 File Offset: 0x001BBD18
	public double GenerateDouble()
	{
		return 2.3283064370807974E-10 * this.GenerateUint();
	}

	// Token: 0x06004D2E RID: 19758 RVA: 0x001BDB2C File Offset: 0x001BBD2C
	public double GenerateRangeDouble()
	{
		uint num = this.GenerateUint();
		return 4.656612874161595E-10 * (double)num;
	}

	// Token: 0x06004D2F RID: 19759 RVA: 0x001BDB4C File Offset: 0x001BBD4C
	public void SetSeed(uint val)
	{
		this.a = val;
		this.b = (val ^ 1842502087U);
		this.c = (val >> 5 ^ 1357980759U);
		this.d = (val >> 7 ^ 273326509U);
		for (uint num = 0U; num < 4U; num += 1U)
		{
			this.a = this.GenerateUint();
		}
	}

	// Token: 0x04003B9D RID: 15261
	private const uint B = 1842502087U;

	// Token: 0x04003B9E RID: 15262
	private const uint C = 1357980759U;

	// Token: 0x04003B9F RID: 15263
	private const uint D = 273326509U;

	// Token: 0x04003BA0 RID: 15264
	private static uint counter;

	// Token: 0x04003BA1 RID: 15265
	private uint a;

	// Token: 0x04003BA2 RID: 15266
	private uint b;

	// Token: 0x04003BA3 RID: 15267
	private uint c;

	// Token: 0x04003BA4 RID: 15268
	private uint d;
}
