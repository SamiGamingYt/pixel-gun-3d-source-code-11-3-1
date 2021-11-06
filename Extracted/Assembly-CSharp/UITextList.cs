using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020003B8 RID: 952
[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x0600222A RID: 8746 RVA: 0x000A45CC File Offset: 0x000A27CC
	protected BetterList<UITextList.Paragraph> paragraphs
	{
		get
		{
			if (this.mParagraphs == null && !UITextList.mHistory.TryGetValue(base.name, out this.mParagraphs))
			{
				this.mParagraphs = new BetterList<UITextList.Paragraph>();
				UITextList.mHistory.Add(base.name, this.mParagraphs);
			}
			return this.mParagraphs;
		}
	}

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x0600222B RID: 8747 RVA: 0x000A4628 File Offset: 0x000A2828
	public bool isValid
	{
		get
		{
			return this.textLabel != null && this.textLabel.ambigiousFont != null;
		}
	}

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x0600222C RID: 8748 RVA: 0x000A4650 File Offset: 0x000A2850
	// (set) Token: 0x0600222D RID: 8749 RVA: 0x000A4658 File Offset: 0x000A2858
	public float scrollValue
	{
		get
		{
			return this.mScroll;
		}
		set
		{
			value = Mathf.Clamp01(value);
			if (this.isValid && this.mScroll != value)
			{
				if (this.scrollBar != null)
				{
					this.scrollBar.value = value;
				}
				else
				{
					this.mScroll = value;
					this.UpdateVisibleText();
				}
			}
		}
	}

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x0600222E RID: 8750 RVA: 0x000A46B4 File Offset: 0x000A28B4
	protected float lineHeight
	{
		get
		{
			return (!(this.textLabel != null)) ? 20f : ((float)this.textLabel.fontSize + this.textLabel.effectiveSpacingY);
		}
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x0600222F RID: 8751 RVA: 0x000A46EC File Offset: 0x000A28EC
	protected int scrollHeight
	{
		get
		{
			if (!this.isValid)
			{
				return 0;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			return Mathf.Max(0, this.mTotalLines - num);
		}
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x000A4730 File Offset: 0x000A2930
	public void Clear()
	{
		this.paragraphs.Clear();
		this.UpdateVisibleText();
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x000A4744 File Offset: 0x000A2944
	private void Start()
	{
		if (this.textLabel == null)
		{
			this.textLabel = base.GetComponentInChildren<UILabel>();
		}
		if (this.scrollBar != null)
		{
			EventDelegate.Add(this.scrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
		}
		this.textLabel.overflowMethod = UILabel.Overflow.ClampContent;
		if (this.style == UITextList.Style.Chat)
		{
			this.textLabel.pivot = UIWidget.Pivot.BottomLeft;
			this.scrollValue = 1f;
		}
		else
		{
			this.textLabel.pivot = UIWidget.Pivot.TopLeft;
			this.scrollValue = 0f;
		}
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x000A47E8 File Offset: 0x000A29E8
	private void Update()
	{
		if (this.isValid && (this.textLabel.width != this.mLastWidth || this.textLabel.height != this.mLastHeight))
		{
			this.Rebuild();
		}
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x000A4834 File Offset: 0x000A2A34
	public void OnScroll(float val)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			val *= this.lineHeight;
			this.scrollValue = this.mScroll - val / (float)scrollHeight;
		}
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000A486C File Offset: 0x000A2A6C
	public void OnDrag(Vector2 delta)
	{
		int scrollHeight = this.scrollHeight;
		if (scrollHeight != 0)
		{
			float num = delta.y / this.lineHeight;
			this.scrollValue = this.mScroll + num / (float)scrollHeight;
		}
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x000A48A8 File Offset: 0x000A2AA8
	private void OnScrollBar()
	{
		this.mScroll = UIProgressBar.current.value;
		this.UpdateVisibleText();
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x000A48C0 File Offset: 0x000A2AC0
	public void Add(string text)
	{
		this.Add(text, true);
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x000A48CC File Offset: 0x000A2ACC
	protected void Add(string text, bool updateVisible)
	{
		UITextList.Paragraph paragraph;
		if (this.paragraphs.size < this.paragraphHistory)
		{
			paragraph = new UITextList.Paragraph();
		}
		else
		{
			paragraph = this.mParagraphs[0];
			this.mParagraphs.RemoveAt(0);
		}
		paragraph.text = text;
		this.mParagraphs.Add(paragraph);
		this.Rebuild();
	}

	// Token: 0x06002238 RID: 8760 RVA: 0x000A4930 File Offset: 0x000A2B30
	protected void Rebuild()
	{
		if (this.isValid)
		{
			this.mLastWidth = this.textLabel.width;
			this.mLastHeight = this.textLabel.height;
			this.textLabel.UpdateNGUIText();
			NGUIText.rectHeight = 1000000;
			NGUIText.regionHeight = 1000000;
			this.mTotalLines = 0;
			for (int i = 0; i < this.paragraphs.size; i++)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[i];
				string text;
				NGUIText.WrapText(paragraph.text, out text, false, true, false);
				paragraph.lines = text.Split(new char[]
				{
					'\n'
				});
				this.mTotalLines += paragraph.lines.Length;
			}
			this.mTotalLines = 0;
			int j = 0;
			int size = this.mParagraphs.size;
			while (j < size)
			{
				this.mTotalLines += this.mParagraphs.buffer[j].lines.Length;
				j++;
			}
			if (this.scrollBar != null)
			{
				UIScrollBar uiscrollBar = this.scrollBar as UIScrollBar;
				if (uiscrollBar != null)
				{
					uiscrollBar.barSize = ((this.mTotalLines != 0) ? (1f - (float)this.scrollHeight / (float)this.mTotalLines) : 1f);
				}
			}
			this.UpdateVisibleText();
		}
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x000A4AA4 File Offset: 0x000A2CA4
	protected void UpdateVisibleText()
	{
		if (this.isValid)
		{
			if (this.mTotalLines == 0)
			{
				this.textLabel.text = string.Empty;
				return;
			}
			int num = Mathf.FloorToInt((float)this.textLabel.height / this.lineHeight);
			int num2 = Mathf.Max(0, this.mTotalLines - num);
			int num3 = Mathf.RoundToInt(this.mScroll * (float)num2);
			if (num3 < 0)
			{
				num3 = 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num4 = 0;
			int size = this.paragraphs.size;
			while (num > 0 && num4 < size)
			{
				UITextList.Paragraph paragraph = this.mParagraphs.buffer[num4];
				int num5 = 0;
				int num6 = paragraph.lines.Length;
				while (num > 0 && num5 < num6)
				{
					string value = paragraph.lines[num5];
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
						num--;
					}
					num5++;
				}
				num4++;
			}
			this.textLabel.text = stringBuilder.ToString();
		}
	}

	// Token: 0x04001637 RID: 5687
	public UILabel textLabel;

	// Token: 0x04001638 RID: 5688
	public UIProgressBar scrollBar;

	// Token: 0x04001639 RID: 5689
	public UITextList.Style style;

	// Token: 0x0400163A RID: 5690
	public int paragraphHistory = 100;

	// Token: 0x0400163B RID: 5691
	protected char[] mSeparator = new char[]
	{
		'\n'
	};

	// Token: 0x0400163C RID: 5692
	protected float mScroll;

	// Token: 0x0400163D RID: 5693
	protected int mTotalLines;

	// Token: 0x0400163E RID: 5694
	protected int mLastWidth;

	// Token: 0x0400163F RID: 5695
	protected int mLastHeight;

	// Token: 0x04001640 RID: 5696
	private BetterList<UITextList.Paragraph> mParagraphs;

	// Token: 0x04001641 RID: 5697
	private static Dictionary<string, BetterList<UITextList.Paragraph>> mHistory = new Dictionary<string, BetterList<UITextList.Paragraph>>();

	// Token: 0x020003B9 RID: 953
	public enum Style
	{
		// Token: 0x04001643 RID: 5699
		Text,
		// Token: 0x04001644 RID: 5700
		Chat
	}

	// Token: 0x020003BA RID: 954
	protected class Paragraph
	{
		// Token: 0x04001645 RID: 5701
		public string text;

		// Token: 0x04001646 RID: 5702
		public string[] lines;
	}
}
