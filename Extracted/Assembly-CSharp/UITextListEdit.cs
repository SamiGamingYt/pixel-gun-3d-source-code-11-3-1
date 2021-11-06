using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000809 RID: 2057
[AddComponentMenu("NGUI/UI/Text List")]
public class UITextListEdit : MonoBehaviour
{
	// Token: 0x06004B2F RID: 19247 RVA: 0x001AC1A8 File Offset: 0x001AA3A8
	public void Clear()
	{
		this.mParagraphs.Clear();
		this.UpdateVisibleText();
	}

	// Token: 0x06004B30 RID: 19248 RVA: 0x001AC1BC File Offset: 0x001AA3BC
	public void Add(string text)
	{
		this.Add(text, true);
	}

	// Token: 0x06004B31 RID: 19249 RVA: 0x001AC1C8 File Offset: 0x001AA3C8
	protected void Add(string text, bool updateVisible)
	{
		UITextListEdit.Paragraph paragraph;
		if (this.mParagraphs.Count < this.maxEntries)
		{
			paragraph = new UITextListEdit.Paragraph();
		}
		else
		{
			paragraph = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		paragraph.text = text;
		this.mParagraphs.Add(paragraph);
		if (this.textLabel != null && this.textLabel.font != null)
		{
			this.mTotalLines = 0;
			int i = 0;
			int count = this.mParagraphs.Count;
			while (i < count)
			{
				this.mTotalLines += this.mParagraphs[i].lines.Length;
				i++;
			}
		}
		if (updateVisible)
		{
			this.UpdateVisibleText();
		}
	}

	// Token: 0x06004B32 RID: 19250 RVA: 0x001AC29C File Offset: 0x001AA49C
	private void Awake()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.textLabel != null)
		{
			this.textLabel.lineWidth = 0;
		}
		Collider component = base.GetComponent<Collider>();
		if (component != null)
		{
			if (this.maxHeight <= 0f)
			{
				this.maxHeight = component.bounds.size.y / base.transform.lossyScale.y;
			}
			if (this.maxWidth <= 0f)
			{
				this.maxWidth = component.bounds.size.x / base.transform.lossyScale.x;
			}
		}
	}

	// Token: 0x06004B33 RID: 19251 RVA: 0x001AC37C File Offset: 0x001AA57C
	public void OnSelect(bool selected)
	{
		this.mSelected = selected;
	}

	// Token: 0x06004B34 RID: 19252 RVA: 0x001AC388 File Offset: 0x001AA588
	protected void UpdateVisibleText()
	{
		if (this.textLabel != null)
		{
			UIFont font = this.textLabel.font;
			if (font != null)
			{
				int num = 0;
				int num2 = (this.maxHeight <= 0f) ? 100000 : Mathf.FloorToInt(this.maxHeight / this.textLabel.cachedTransform.localScale.y);
				int num3 = Mathf.RoundToInt(this.mScroll);
				if (num2 + num3 > this.mTotalLines)
				{
					num3 = Mathf.Max(0, this.mTotalLines - num2);
					this.mScroll = (float)num3;
				}
				if (this.style == UITextListEdit.Style.Chat)
				{
					num3 = Mathf.Max(0, this.mTotalLines - num2 - num3);
				}
				StringBuilder stringBuilder = new StringBuilder();
				int i = 0;
				int count = this.mParagraphs.Count;
				while (i < count)
				{
					UITextListEdit.Paragraph paragraph = this.mParagraphs[this.mParagraphs.Count - 1 - i];
					int j = 0;
					int num4 = paragraph.lines.Length;
					while (j < num4)
					{
						string value = paragraph.lines[j];
						if (num3 > 0)
						{
							num3--;
						}
						else
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.Append("\n");
							}
							stringBuilder.Append(value);
							num++;
							if (num >= num2)
							{
								break;
							}
						}
						j++;
					}
					if (num >= num2)
					{
						break;
					}
					i++;
				}
				this.textLabel.text = stringBuilder.ToString();
			}
		}
	}

	// Token: 0x06004B35 RID: 19253 RVA: 0x001AC528 File Offset: 0x001AA728
	public void OnScroll(float val)
	{
		if (this.mSelected && this.supportScrollWheel)
		{
			val *= ((this.style != UITextListEdit.Style.Chat) ? -10f : 10f);
			this.mScroll = Mathf.Max(0f, this.mScroll + val);
			this.UpdateVisibleText();
		}
	}

	// Token: 0x040037C1 RID: 14273
	public UITextListEdit.Style style;

	// Token: 0x040037C2 RID: 14274
	public UILabel textLabel;

	// Token: 0x040037C3 RID: 14275
	public float maxWidth;

	// Token: 0x040037C4 RID: 14276
	public float maxHeight;

	// Token: 0x040037C5 RID: 14277
	public int maxEntries = 50;

	// Token: 0x040037C6 RID: 14278
	public bool supportScrollWheel = true;

	// Token: 0x040037C7 RID: 14279
	protected char[] mSeparator = new char[]
	{
		'\n'
	};

	// Token: 0x040037C8 RID: 14280
	protected List<UITextListEdit.Paragraph> mParagraphs = new List<UITextListEdit.Paragraph>();

	// Token: 0x040037C9 RID: 14281
	protected float mScroll;

	// Token: 0x040037CA RID: 14282
	protected bool mSelected;

	// Token: 0x040037CB RID: 14283
	protected int mTotalLines;

	// Token: 0x0200080A RID: 2058
	public enum Style
	{
		// Token: 0x040037CD RID: 14285
		Text,
		// Token: 0x040037CE RID: 14286
		Chat
	}

	// Token: 0x0200080B RID: 2059
	protected class Paragraph
	{
		// Token: 0x040037CF RID: 14287
		public string text;

		// Token: 0x040037D0 RID: 14288
		public string[] lines;
	}
}
