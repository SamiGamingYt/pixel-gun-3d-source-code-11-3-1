using System;
using System.Collections.Generic;

// Token: 0x0200035C RID: 860
[Serializable]
public class BMGlyph
{
	// Token: 0x06001D73 RID: 7539 RVA: 0x0007E8D8 File Offset: 0x0007CAD8
	public int GetKerning(int previousChar)
	{
		if (this.kerning != null && previousChar != 0)
		{
			int i = 0;
			int count = this.kerning.Count;
			while (i < count)
			{
				if (this.kerning[i] == previousChar)
				{
					return this.kerning[i + 1];
				}
				i += 2;
			}
		}
		return 0;
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x0007E938 File Offset: 0x0007CB38
	public void SetKerning(int previousChar, int amount)
	{
		if (this.kerning == null)
		{
			this.kerning = new List<int>();
		}
		for (int i = 0; i < this.kerning.Count; i += 2)
		{
			if (this.kerning[i] == previousChar)
			{
				this.kerning[i + 1] = amount;
				return;
			}
		}
		this.kerning.Add(previousChar);
		this.kerning.Add(amount);
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x0007E9B4 File Offset: 0x0007CBB4
	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		int num = this.x + this.width;
		int num2 = this.y + this.height;
		if (this.x < xMin)
		{
			int num3 = xMin - this.x;
			this.x += num3;
			this.width -= num3;
			this.offsetX += num3;
		}
		if (this.y < yMin)
		{
			int num4 = yMin - this.y;
			this.y += num4;
			this.height -= num4;
			this.offsetY += num4;
		}
		if (num > xMax)
		{
			this.width -= num - xMax;
		}
		if (num2 > yMax)
		{
			this.height -= num2 - yMax;
		}
	}

	// Token: 0x040012BF RID: 4799
	public int index;

	// Token: 0x040012C0 RID: 4800
	public int x;

	// Token: 0x040012C1 RID: 4801
	public int y;

	// Token: 0x040012C2 RID: 4802
	public int width;

	// Token: 0x040012C3 RID: 4803
	public int height;

	// Token: 0x040012C4 RID: 4804
	public int offsetX;

	// Token: 0x040012C5 RID: 4805
	public int offsetY;

	// Token: 0x040012C6 RID: 4806
	public int advance;

	// Token: 0x040012C7 RID: 4807
	public int channel;

	// Token: 0x040012C8 RID: 4808
	public List<int> kerning;
}
