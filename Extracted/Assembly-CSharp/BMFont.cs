using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200035B RID: 859
[Serializable]
public class BMFont
{
	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x06001D61 RID: 7521 RVA: 0x0007E720 File Offset: 0x0007C920
	public bool isValid
	{
		get
		{
			return this.mSaved.Count > 0;
		}
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06001D62 RID: 7522 RVA: 0x0007E730 File Offset: 0x0007C930
	// (set) Token: 0x06001D63 RID: 7523 RVA: 0x0007E738 File Offset: 0x0007C938
	public int charSize
	{
		get
		{
			return this.mSize;
		}
		set
		{
			this.mSize = value;
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06001D64 RID: 7524 RVA: 0x0007E744 File Offset: 0x0007C944
	// (set) Token: 0x06001D65 RID: 7525 RVA: 0x0007E74C File Offset: 0x0007C94C
	public int baseOffset
	{
		get
		{
			return this.mBase;
		}
		set
		{
			this.mBase = value;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06001D66 RID: 7526 RVA: 0x0007E758 File Offset: 0x0007C958
	// (set) Token: 0x06001D67 RID: 7527 RVA: 0x0007E760 File Offset: 0x0007C960
	public int texWidth
	{
		get
		{
			return this.mWidth;
		}
		set
		{
			this.mWidth = value;
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06001D68 RID: 7528 RVA: 0x0007E76C File Offset: 0x0007C96C
	// (set) Token: 0x06001D69 RID: 7529 RVA: 0x0007E774 File Offset: 0x0007C974
	public int texHeight
	{
		get
		{
			return this.mHeight;
		}
		set
		{
			this.mHeight = value;
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06001D6A RID: 7530 RVA: 0x0007E780 File Offset: 0x0007C980
	public int glyphCount
	{
		get
		{
			return (!this.isValid) ? 0 : this.mSaved.Count;
		}
	}

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06001D6B RID: 7531 RVA: 0x0007E7A0 File Offset: 0x0007C9A0
	// (set) Token: 0x06001D6C RID: 7532 RVA: 0x0007E7A8 File Offset: 0x0007C9A8
	public string spriteName
	{
		get
		{
			return this.mSpriteName;
		}
		set
		{
			this.mSpriteName = value;
		}
	}

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06001D6D RID: 7533 RVA: 0x0007E7B4 File Offset: 0x0007C9B4
	public List<BMGlyph> glyphs
	{
		get
		{
			return this.mSaved;
		}
	}

	// Token: 0x06001D6E RID: 7534 RVA: 0x0007E7BC File Offset: 0x0007C9BC
	public BMGlyph GetGlyph(int index, bool createIfMissing)
	{
		BMGlyph bmglyph = null;
		if (this.mDict.Count == 0)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bmglyph2 = this.mSaved[i];
				this.mDict.Add(bmglyph2.index, bmglyph2);
				i++;
			}
		}
		if (!this.mDict.TryGetValue(index, out bmglyph) && createIfMissing)
		{
			bmglyph = new BMGlyph();
			bmglyph.index = index;
			this.mSaved.Add(bmglyph);
			this.mDict.Add(index, bmglyph);
		}
		return bmglyph;
	}

	// Token: 0x06001D6F RID: 7535 RVA: 0x0007E858 File Offset: 0x0007CA58
	public BMGlyph GetGlyph(int index)
	{
		return this.GetGlyph(index, false);
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x0007E864 File Offset: 0x0007CA64
	public void Clear()
	{
		this.mDict.Clear();
		this.mSaved.Clear();
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x0007E87C File Offset: 0x0007CA7C
	public void Trim(int xMin, int yMin, int xMax, int yMax)
	{
		if (this.isValid)
		{
			int i = 0;
			int count = this.mSaved.Count;
			while (i < count)
			{
				BMGlyph bmglyph = this.mSaved[i];
				if (bmglyph != null)
				{
					bmglyph.Trim(xMin, yMin, xMax, yMax);
				}
				i++;
			}
		}
	}

	// Token: 0x040012B8 RID: 4792
	[HideInInspector]
	[SerializeField]
	private int mSize = 16;

	// Token: 0x040012B9 RID: 4793
	[HideInInspector]
	[SerializeField]
	private int mBase;

	// Token: 0x040012BA RID: 4794
	[SerializeField]
	[HideInInspector]
	private int mWidth;

	// Token: 0x040012BB RID: 4795
	[HideInInspector]
	[SerializeField]
	private int mHeight;

	// Token: 0x040012BC RID: 4796
	[HideInInspector]
	[SerializeField]
	private string mSpriteName;

	// Token: 0x040012BD RID: 4797
	[SerializeField]
	[HideInInspector]
	private List<BMGlyph> mSaved = new List<BMGlyph>();

	// Token: 0x040012BE RID: 4798
	private Dictionary<int, BMGlyph> mDict = new Dictionary<int, BMGlyph>();
}
