using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000317 RID: 791
[AddComponentMenu("NGUI/Interaction/Typewriter Effect")]
[RequireComponent(typeof(UILabel))]
public class TypewriterEffect : MonoBehaviour
{
	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06001B84 RID: 7044 RVA: 0x00070AE0 File Offset: 0x0006ECE0
	public bool isActive
	{
		get
		{
			return this.mActive;
		}
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x00070AE8 File Offset: 0x0006ECE8
	public void ResetToBeginning()
	{
		this.Finish();
		this.mReset = true;
		this.mActive = true;
		this.mNextChar = 0f;
		this.mCurrentOffset = 0;
		this.Update();
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x00070B24 File Offset: 0x0006ED24
	public void Finish()
	{
		if (this.mActive)
		{
			this.mActive = false;
			if (!this.mReset)
			{
				this.mCurrentOffset = this.mFullText.Length;
				this.mFade.Clear();
				this.mLabel.text = this.mFullText;
			}
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
		}
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x00070BBC File Offset: 0x0006EDBC
	private void OnEnable()
	{
		this.mReset = true;
		this.mActive = true;
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x00070BCC File Offset: 0x0006EDCC
	private void OnDisable()
	{
		this.Finish();
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x00070BD4 File Offset: 0x0006EDD4
	private void Update()
	{
		if (!this.mActive)
		{
			return;
		}
		if (this.mReset)
		{
			this.mCurrentOffset = 0;
			this.mReset = false;
			this.mLabel = base.GetComponent<UILabel>();
			this.mFullText = this.mLabel.processedText;
			this.mFade.Clear();
			if (this.keepFullDimensions && this.scrollView != null)
			{
				this.scrollView.UpdatePosition();
			}
		}
		if (string.IsNullOrEmpty(this.mFullText))
		{
			return;
		}
		while (this.mCurrentOffset < this.mFullText.Length && this.mNextChar <= RealTime.time)
		{
			int num = this.mCurrentOffset;
			this.charsPerSecond = Mathf.Max(1, this.charsPerSecond);
			if (this.mLabel.supportEncoding)
			{
				while (NGUIText.ParseSymbol(this.mFullText, ref this.mCurrentOffset))
				{
				}
			}
			this.mCurrentOffset++;
			if (this.mCurrentOffset > this.mFullText.Length)
			{
				break;
			}
			float num2 = 1f / (float)this.charsPerSecond;
			char c = (num >= this.mFullText.Length) ? '\n' : this.mFullText[num];
			if (c == '\n')
			{
				num2 += this.delayOnNewLine;
			}
			else if (num + 1 == this.mFullText.Length || this.mFullText[num + 1] <= ' ')
			{
				if (c == '.')
				{
					if (num + 2 < this.mFullText.Length && this.mFullText[num + 1] == '.' && this.mFullText[num + 2] == '.')
					{
						num2 += this.delayOnPeriod * 3f;
						num += 2;
					}
					else
					{
						num2 += this.delayOnPeriod;
					}
				}
				else if (c == '!' || c == '?')
				{
					num2 += this.delayOnPeriod;
				}
			}
			if (this.mNextChar == 0f)
			{
				this.mNextChar = RealTime.time + num2;
			}
			else
			{
				this.mNextChar += num2;
			}
			if (this.fadeInTime != 0f)
			{
				TypewriterEffect.FadeEntry item = default(TypewriterEffect.FadeEntry);
				item.index = num;
				item.alpha = 0f;
				item.text = this.mFullText.Substring(num, this.mCurrentOffset - num);
				this.mFade.Add(item);
			}
			else
			{
				this.mLabel.text = ((!this.keepFullDimensions) ? this.mFullText.Substring(0, this.mCurrentOffset) : (this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset)));
				if (!this.keepFullDimensions && this.scrollView != null)
				{
					this.scrollView.UpdatePosition();
				}
			}
		}
		if (this.mCurrentOffset >= this.mFullText.Length)
		{
			this.mLabel.text = this.mFullText;
			TypewriterEffect.current = this;
			EventDelegate.Execute(this.onFinished);
			TypewriterEffect.current = null;
			this.mActive = false;
		}
		else if (this.mFade.size != 0)
		{
			int i = 0;
			while (i < this.mFade.size)
			{
				TypewriterEffect.FadeEntry value = this.mFade[i];
				value.alpha += RealTime.deltaTime / this.fadeInTime;
				if (value.alpha < 1f)
				{
					this.mFade[i] = value;
					i++;
				}
				else
				{
					this.mFade.RemoveAt(i);
				}
			}
			if (this.mFade.size == 0)
			{
				if (this.keepFullDimensions)
				{
					this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset) + "[00]" + this.mFullText.Substring(this.mCurrentOffset);
				}
				else
				{
					this.mLabel.text = this.mFullText.Substring(0, this.mCurrentOffset);
				}
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < this.mFade.size; j++)
				{
					TypewriterEffect.FadeEntry fadeEntry = this.mFade[j];
					if (j == 0)
					{
						stringBuilder.Append(this.mFullText.Substring(0, fadeEntry.index));
					}
					stringBuilder.Append('[');
					stringBuilder.Append(NGUIText.EncodeAlpha(fadeEntry.alpha));
					stringBuilder.Append(']');
					stringBuilder.Append(fadeEntry.text);
				}
				if (this.keepFullDimensions)
				{
					stringBuilder.Append("[00]");
					stringBuilder.Append(this.mFullText.Substring(this.mCurrentOffset));
				}
				this.mLabel.text = stringBuilder.ToString();
			}
		}
	}

	// Token: 0x04001095 RID: 4245
	public static TypewriterEffect current;

	// Token: 0x04001096 RID: 4246
	public int charsPerSecond = 20;

	// Token: 0x04001097 RID: 4247
	public float fadeInTime;

	// Token: 0x04001098 RID: 4248
	public float delayOnPeriod;

	// Token: 0x04001099 RID: 4249
	public float delayOnNewLine;

	// Token: 0x0400109A RID: 4250
	public UIScrollView scrollView;

	// Token: 0x0400109B RID: 4251
	public bool keepFullDimensions;

	// Token: 0x0400109C RID: 4252
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x0400109D RID: 4253
	private UILabel mLabel;

	// Token: 0x0400109E RID: 4254
	private string mFullText = string.Empty;

	// Token: 0x0400109F RID: 4255
	private int mCurrentOffset;

	// Token: 0x040010A0 RID: 4256
	private float mNextChar;

	// Token: 0x040010A1 RID: 4257
	private bool mReset = true;

	// Token: 0x040010A2 RID: 4258
	private bool mActive;

	// Token: 0x040010A3 RID: 4259
	private BetterList<TypewriterEffect.FadeEntry> mFade = new BetterList<TypewriterEffect.FadeEntry>();

	// Token: 0x02000318 RID: 792
	private struct FadeEntry
	{
		// Token: 0x040010A4 RID: 4260
		public int index;

		// Token: 0x040010A5 RID: 4261
		public string text;

		// Token: 0x040010A6 RID: 4262
		public float alpha;
	}
}
