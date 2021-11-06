using System;

// Token: 0x020003B5 RID: 949
[Serializable]
public class UISpriteData
{
	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x0600221B RID: 8731 RVA: 0x000A3A98 File Offset: 0x000A1C98
	public bool hasBorder
	{
		get
		{
			return (this.borderLeft | this.borderRight | this.borderTop | this.borderBottom) != 0;
		}
	}

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x0600221C RID: 8732 RVA: 0x000A3ABC File Offset: 0x000A1CBC
	public bool hasPadding
	{
		get
		{
			return (this.paddingLeft | this.paddingRight | this.paddingTop | this.paddingBottom) != 0;
		}
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x000A3AE0 File Offset: 0x000A1CE0
	public void SetRect(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x000A3B00 File Offset: 0x000A1D00
	public void SetPadding(int left, int bottom, int right, int top)
	{
		this.paddingLeft = left;
		this.paddingBottom = bottom;
		this.paddingRight = right;
		this.paddingTop = top;
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x000A3B20 File Offset: 0x000A1D20
	public void SetBorder(int left, int bottom, int right, int top)
	{
		this.borderLeft = left;
		this.borderBottom = bottom;
		this.borderRight = right;
		this.borderTop = top;
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x000A3B40 File Offset: 0x000A1D40
	public void CopyFrom(UISpriteData sd)
	{
		this.name = sd.name;
		this.x = sd.x;
		this.y = sd.y;
		this.width = sd.width;
		this.height = sd.height;
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
		this.paddingLeft = sd.paddingLeft;
		this.paddingRight = sd.paddingRight;
		this.paddingTop = sd.paddingTop;
		this.paddingBottom = sd.paddingBottom;
	}

	// Token: 0x06002221 RID: 8737 RVA: 0x000A3BEC File Offset: 0x000A1DEC
	public void CopyBorderFrom(UISpriteData sd)
	{
		this.borderLeft = sd.borderLeft;
		this.borderRight = sd.borderRight;
		this.borderTop = sd.borderTop;
		this.borderBottom = sd.borderBottom;
	}

	// Token: 0x04001612 RID: 5650
	public string name = "Sprite";

	// Token: 0x04001613 RID: 5651
	public int x;

	// Token: 0x04001614 RID: 5652
	public int y;

	// Token: 0x04001615 RID: 5653
	public int width;

	// Token: 0x04001616 RID: 5654
	public int height;

	// Token: 0x04001617 RID: 5655
	public int borderLeft;

	// Token: 0x04001618 RID: 5656
	public int borderRight;

	// Token: 0x04001619 RID: 5657
	public int borderTop;

	// Token: 0x0400161A RID: 5658
	public int borderBottom;

	// Token: 0x0400161B RID: 5659
	public int paddingLeft;

	// Token: 0x0400161C RID: 5660
	public int paddingRight;

	// Token: 0x0400161D RID: 5661
	public int paddingTop;

	// Token: 0x0400161E RID: 5662
	public int paddingBottom;
}
