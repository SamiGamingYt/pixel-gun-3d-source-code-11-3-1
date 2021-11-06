using System;
using UnityEngine;

// Token: 0x0200035D RID: 861
[Serializable]
public class BMSymbol
{
	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06001D77 RID: 7543 RVA: 0x0007EA94 File Offset: 0x0007CC94
	public int length
	{
		get
		{
			if (this.mLength == 0)
			{
				this.mLength = this.sequence.Length;
			}
			return this.mLength;
		}
	}

	// Token: 0x17000504 RID: 1284
	// (get) Token: 0x06001D78 RID: 7544 RVA: 0x0007EAC4 File Offset: 0x0007CCC4
	public int offsetX
	{
		get
		{
			return this.mOffsetX;
		}
	}

	// Token: 0x17000505 RID: 1285
	// (get) Token: 0x06001D79 RID: 7545 RVA: 0x0007EACC File Offset: 0x0007CCCC
	public int offsetY
	{
		get
		{
			return this.mOffsetY;
		}
	}

	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x06001D7A RID: 7546 RVA: 0x0007EAD4 File Offset: 0x0007CCD4
	public int width
	{
		get
		{
			return this.mWidth;
		}
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06001D7B RID: 7547 RVA: 0x0007EADC File Offset: 0x0007CCDC
	public int height
	{
		get
		{
			return this.mHeight;
		}
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06001D7C RID: 7548 RVA: 0x0007EAE4 File Offset: 0x0007CCE4
	public int advance
	{
		get
		{
			return this.mAdvance;
		}
	}

	// Token: 0x17000509 RID: 1289
	// (get) Token: 0x06001D7D RID: 7549 RVA: 0x0007EAEC File Offset: 0x0007CCEC
	public Rect uvRect
	{
		get
		{
			return this.mUV;
		}
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x0007EAF4 File Offset: 0x0007CCF4
	public void MarkAsChanged()
	{
		this.mIsValid = false;
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x0007EB00 File Offset: 0x0007CD00
	public bool Validate(UIAtlas atlas)
	{
		if (atlas == null)
		{
			return false;
		}
		if (!this.mIsValid)
		{
			if (string.IsNullOrEmpty(this.spriteName))
			{
				return false;
			}
			this.mSprite = ((!(atlas != null)) ? null : atlas.GetSprite(this.spriteName));
			if (this.mSprite != null)
			{
				Texture texture = atlas.texture;
				if (texture == null)
				{
					this.mSprite = null;
				}
				else
				{
					this.mUV = new Rect((float)this.mSprite.x, (float)this.mSprite.y, (float)this.mSprite.width, (float)this.mSprite.height);
					this.mUV = NGUIMath.ConvertToTexCoords(this.mUV, texture.width, texture.height);
					this.mOffsetX = this.mSprite.paddingLeft;
					this.mOffsetY = this.mSprite.paddingTop;
					this.mWidth = this.mSprite.width;
					this.mHeight = this.mSprite.height;
					this.mAdvance = this.mSprite.width + (this.mSprite.paddingLeft + this.mSprite.paddingRight);
					this.mIsValid = true;
				}
			}
		}
		return this.mSprite != null;
	}

	// Token: 0x040012C9 RID: 4809
	public string sequence;

	// Token: 0x040012CA RID: 4810
	public string spriteName;

	// Token: 0x040012CB RID: 4811
	private UISpriteData mSprite;

	// Token: 0x040012CC RID: 4812
	private bool mIsValid;

	// Token: 0x040012CD RID: 4813
	private int mLength;

	// Token: 0x040012CE RID: 4814
	private int mOffsetX;

	// Token: 0x040012CF RID: 4815
	private int mOffsetY;

	// Token: 0x040012D0 RID: 4816
	private int mWidth;

	// Token: 0x040012D1 RID: 4817
	private int mHeight;

	// Token: 0x040012D2 RID: 4818
	private int mAdvance;

	// Token: 0x040012D3 RID: 4819
	private Rect mUV;
}
