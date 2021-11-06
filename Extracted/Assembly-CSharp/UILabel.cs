using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003A8 RID: 936
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Label")]
public class UILabel : UIWidget
{
	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06002122 RID: 8482 RVA: 0x0009D4F8 File Offset: 0x0009B6F8
	public int finalFontSize
	{
		get
		{
			if (this.trueTypeFont)
			{
				return Mathf.RoundToInt(this.mScale * (float)this.mFinalFontSize);
			}
			return Mathf.RoundToInt((float)this.mFinalFontSize * this.mScale);
		}
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06002123 RID: 8483 RVA: 0x0009D534 File Offset: 0x0009B734
	// (set) Token: 0x06002124 RID: 8484 RVA: 0x0009D53C File Offset: 0x0009B73C
	private bool shouldBeProcessed
	{
		get
		{
			return this.mShouldBeProcessed;
		}
		set
		{
			if (value)
			{
				this.mChanged = true;
				this.mShouldBeProcessed = true;
			}
			else
			{
				this.mShouldBeProcessed = false;
			}
		}
	}

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06002125 RID: 8485 RVA: 0x0009D56C File Offset: 0x0009B76C
	public override bool isAnchoredHorizontally
	{
		get
		{
			return base.isAnchoredHorizontally || this.mOverflow == UILabel.Overflow.ResizeFreely;
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06002126 RID: 8486 RVA: 0x0009D588 File Offset: 0x0009B788
	public override bool isAnchoredVertically
	{
		get
		{
			return base.isAnchoredVertically || this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight;
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06002127 RID: 8487 RVA: 0x0009D5B0 File Offset: 0x0009B7B0
	// (set) Token: 0x06002128 RID: 8488 RVA: 0x0009D610 File Offset: 0x0009B810
	public override Material material
	{
		get
		{
			if (this.mMaterial != null)
			{
				return this.mMaterial;
			}
			if (this.mFont != null)
			{
				return this.mFont.material;
			}
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont.material;
			}
			return null;
		}
		set
		{
			if (this.mMaterial != value)
			{
				base.RemoveFromPanel();
				this.mMaterial = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06002129 RID: 8489 RVA: 0x0009D644 File Offset: 0x0009B844
	// (set) Token: 0x0600212A RID: 8490 RVA: 0x0009D64C File Offset: 0x0009B84C
	[Obsolete("Use UILabel.bitmapFont instead")]
	public UIFont font
	{
		get
		{
			return this.bitmapFont;
		}
		set
		{
			this.bitmapFont = value;
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x0600212B RID: 8491 RVA: 0x0009D658 File Offset: 0x0009B858
	// (set) Token: 0x0600212C RID: 8492 RVA: 0x0009D660 File Offset: 0x0009B860
	public UIFont bitmapFont
	{
		get
		{
			return this.mFont;
		}
		set
		{
			if (this.mFont != value)
			{
				base.RemoveFromPanel();
				this.mFont = value;
				this.mTrueTypeFont = null;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x0600212D RID: 8493 RVA: 0x0009D690 File Offset: 0x0009B890
	// (set) Token: 0x0600212E RID: 8494 RVA: 0x0009D6D8 File Offset: 0x0009B8D8
	public Font trueTypeFont
	{
		get
		{
			if (this.mTrueTypeFont != null)
			{
				return this.mTrueTypeFont;
			}
			return (!(this.mFont != null)) ? null : this.mFont.dynamicFont;
		}
		set
		{
			if (this.mTrueTypeFont != value)
			{
				this.SetActiveFont(null);
				base.RemoveFromPanel();
				this.mTrueTypeFont = value;
				this.shouldBeProcessed = true;
				this.mFont = null;
				this.SetActiveFont(value);
				this.ProcessAndRequest();
				if (this.mActiveTTF != null)
				{
					base.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x0600212F RID: 8495 RVA: 0x0009D73C File Offset: 0x0009B93C
	// (set) Token: 0x06002130 RID: 8496 RVA: 0x0009D754 File Offset: 0x0009B954
	public UnityEngine.Object ambigiousFont
	{
		get
		{
			return this.mFont ?? this.mTrueTypeFont;
		}
		set
		{
			UIFont uifont = value as UIFont;
			if (uifont != null)
			{
				this.bitmapFont = uifont;
			}
			else
			{
				this.trueTypeFont = (value as Font);
			}
		}
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x06002131 RID: 8497 RVA: 0x0009D78C File Offset: 0x0009B98C
	// (set) Token: 0x06002132 RID: 8498 RVA: 0x0009D794 File Offset: 0x0009B994
	public string text
	{
		get
		{
			return this.mText;
		}
		set
		{
			if (this.mText == value)
			{
				return;
			}
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(this.mText))
				{
					this.mText = string.Empty;
					this.MarkAsChanged();
					this.ProcessAndRequest();
				}
			}
			else if (this.mText != value)
			{
				this.mText = value;
				this.MarkAsChanged();
				this.ProcessAndRequest();
			}
			if (this.autoResizeBoxCollider)
			{
				base.ResizeCollider();
			}
		}
	}

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x06002133 RID: 8499 RVA: 0x0009D820 File Offset: 0x0009BA20
	public int defaultFontSize
	{
		get
		{
			return (!(this.trueTypeFont != null)) ? ((!(this.mFont != null)) ? 16 : this.mFont.defaultSize) : this.mFontSize;
		}
	}

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x06002134 RID: 8500 RVA: 0x0009D86C File Offset: 0x0009BA6C
	// (set) Token: 0x06002135 RID: 8501 RVA: 0x0009D874 File Offset: 0x0009BA74
	public int fontSize
	{
		get
		{
			return this.mFontSize;
		}
		set
		{
			value = Mathf.Clamp(value, 0, 256);
			if (this.mFontSize != value)
			{
				this.mFontSize = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x06002136 RID: 8502 RVA: 0x0009D8B0 File Offset: 0x0009BAB0
	// (set) Token: 0x06002137 RID: 8503 RVA: 0x0009D8B8 File Offset: 0x0009BAB8
	public FontStyle fontStyle
	{
		get
		{
			return this.mFontStyle;
		}
		set
		{
			if (this.mFontStyle != value)
			{
				this.mFontStyle = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06002138 RID: 8504 RVA: 0x0009D8E8 File Offset: 0x0009BAE8
	// (set) Token: 0x06002139 RID: 8505 RVA: 0x0009D8F0 File Offset: 0x0009BAF0
	public NGUIText.Alignment alignment
	{
		get
		{
			return this.mAlignment;
		}
		set
		{
			if (this.mAlignment != value)
			{
				this.mAlignment = value;
				this.shouldBeProcessed = true;
				this.ProcessAndRequest();
			}
		}
	}

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x0600213A RID: 8506 RVA: 0x0009D920 File Offset: 0x0009BB20
	// (set) Token: 0x0600213B RID: 8507 RVA: 0x0009D928 File Offset: 0x0009BB28
	public bool applyGradient
	{
		get
		{
			return this.mApplyGradient;
		}
		set
		{
			if (this.mApplyGradient != value)
			{
				this.mApplyGradient = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x0600213C RID: 8508 RVA: 0x0009D944 File Offset: 0x0009BB44
	// (set) Token: 0x0600213D RID: 8509 RVA: 0x0009D94C File Offset: 0x0009BB4C
	public Color gradientTop
	{
		get
		{
			return this.mGradientTop;
		}
		set
		{
			if (this.mGradientTop != value)
			{
				this.mGradientTop = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x0600213E RID: 8510 RVA: 0x0009D978 File Offset: 0x0009BB78
	// (set) Token: 0x0600213F RID: 8511 RVA: 0x0009D980 File Offset: 0x0009BB80
	public Color gradientBottom
	{
		get
		{
			return this.mGradientBottom;
		}
		set
		{
			if (this.mGradientBottom != value)
			{
				this.mGradientBottom = value;
				if (this.mApplyGradient)
				{
					this.MarkAsChanged();
				}
			}
		}
	}

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x06002140 RID: 8512 RVA: 0x0009D9AC File Offset: 0x0009BBAC
	// (set) Token: 0x06002141 RID: 8513 RVA: 0x0009D9B4 File Offset: 0x0009BBB4
	public int spacingX
	{
		get
		{
			return this.mSpacingX;
		}
		set
		{
			if (this.mSpacingX != value)
			{
				this.mSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x06002142 RID: 8514 RVA: 0x0009D9D0 File Offset: 0x0009BBD0
	// (set) Token: 0x06002143 RID: 8515 RVA: 0x0009D9D8 File Offset: 0x0009BBD8
	public int spacingY
	{
		get
		{
			return this.mSpacingY;
		}
		set
		{
			if (this.mSpacingY != value)
			{
				this.mSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x06002144 RID: 8516 RVA: 0x0009D9F4 File Offset: 0x0009BBF4
	// (set) Token: 0x06002145 RID: 8517 RVA: 0x0009D9FC File Offset: 0x0009BBFC
	public bool useFloatSpacing
	{
		get
		{
			return this.mUseFloatSpacing;
		}
		set
		{
			if (this.mUseFloatSpacing != value)
			{
				this.mUseFloatSpacing = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x06002146 RID: 8518 RVA: 0x0009DA18 File Offset: 0x0009BC18
	// (set) Token: 0x06002147 RID: 8519 RVA: 0x0009DA20 File Offset: 0x0009BC20
	public float floatSpacingX
	{
		get
		{
			return this.mFloatSpacingX;
		}
		set
		{
			if (!Mathf.Approximately(this.mFloatSpacingX, value))
			{
				this.mFloatSpacingX = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x06002148 RID: 8520 RVA: 0x0009DA40 File Offset: 0x0009BC40
	// (set) Token: 0x06002149 RID: 8521 RVA: 0x0009DA48 File Offset: 0x0009BC48
	public float floatSpacingY
	{
		get
		{
			return this.mFloatSpacingY;
		}
		set
		{
			if (!Mathf.Approximately(this.mFloatSpacingY, value))
			{
				this.mFloatSpacingY = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x0600214A RID: 8522 RVA: 0x0009DA68 File Offset: 0x0009BC68
	public float effectiveSpacingY
	{
		get
		{
			return (!this.mUseFloatSpacing) ? ((float)this.mSpacingY) : this.mFloatSpacingY;
		}
	}

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x0600214B RID: 8523 RVA: 0x0009DA88 File Offset: 0x0009BC88
	public float effectiveSpacingX
	{
		get
		{
			return (!this.mUseFloatSpacing) ? ((float)this.mSpacingX) : this.mFloatSpacingX;
		}
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x0600214C RID: 8524 RVA: 0x0009DAA8 File Offset: 0x0009BCA8
	// (set) Token: 0x0600214D RID: 8525 RVA: 0x0009DAB0 File Offset: 0x0009BCB0
	public bool overflowEllipsis
	{
		get
		{
			return this.mOverflowEllipsis;
		}
		set
		{
			if (this.mOverflowEllipsis != value)
			{
				this.mOverflowEllipsis = value;
				this.MarkAsChanged();
			}
		}
	}

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x0600214E RID: 8526 RVA: 0x0009DACC File Offset: 0x0009BCCC
	private bool keepCrisp
	{
		get
		{
			return this.trueTypeFont != null && this.keepCrispWhenShrunk != UILabel.Crispness.Never && this.keepCrispWhenShrunk == UILabel.Crispness.Always;
		}
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x0600214F RID: 8527 RVA: 0x0009DAF8 File Offset: 0x0009BCF8
	// (set) Token: 0x06002150 RID: 8528 RVA: 0x0009DB00 File Offset: 0x0009BD00
	public bool supportEncoding
	{
		get
		{
			return this.mEncoding;
		}
		set
		{
			if (this.mEncoding != value)
			{
				this.mEncoding = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x06002151 RID: 8529 RVA: 0x0009DB1C File Offset: 0x0009BD1C
	// (set) Token: 0x06002152 RID: 8530 RVA: 0x0009DB24 File Offset: 0x0009BD24
	public NGUIText.SymbolStyle symbolStyle
	{
		get
		{
			return this.mSymbols;
		}
		set
		{
			if (this.mSymbols != value)
			{
				this.mSymbols = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x06002153 RID: 8531 RVA: 0x0009DB40 File Offset: 0x0009BD40
	// (set) Token: 0x06002154 RID: 8532 RVA: 0x0009DB48 File Offset: 0x0009BD48
	public UILabel.Overflow overflowMethod
	{
		get
		{
			return this.mOverflow;
		}
		set
		{
			if (this.mOverflow != value)
			{
				this.mOverflow = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x06002155 RID: 8533 RVA: 0x0009DB64 File Offset: 0x0009BD64
	// (set) Token: 0x06002156 RID: 8534 RVA: 0x0009DB6C File Offset: 0x0009BD6C
	[Obsolete("Use 'width' instead")]
	public int lineWidth
	{
		get
		{
			return base.width;
		}
		set
		{
			base.width = value;
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x06002157 RID: 8535 RVA: 0x0009DB78 File Offset: 0x0009BD78
	// (set) Token: 0x06002158 RID: 8536 RVA: 0x0009DB80 File Offset: 0x0009BD80
	[Obsolete("Use 'height' instead")]
	public int lineHeight
	{
		get
		{
			return base.height;
		}
		set
		{
			base.height = value;
		}
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x06002159 RID: 8537 RVA: 0x0009DB8C File Offset: 0x0009BD8C
	// (set) Token: 0x0600215A RID: 8538 RVA: 0x0009DB9C File Offset: 0x0009BD9C
	public bool multiLine
	{
		get
		{
			return this.mMaxLineCount != 1;
		}
		set
		{
			if (this.mMaxLineCount != 1 != value)
			{
				this.mMaxLineCount = ((!value) ? 1 : 0);
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x0600215B RID: 8539 RVA: 0x0009DBD8 File Offset: 0x0009BDD8
	public override Vector3[] localCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.localCorners;
		}
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x0600215C RID: 8540 RVA: 0x0009DBF4 File Offset: 0x0009BDF4
	public override Vector3[] worldCorners
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.worldCorners;
		}
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x0600215D RID: 8541 RVA: 0x0009DC10 File Offset: 0x0009BE10
	public override Vector4 drawingDimensions
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.drawingDimensions;
		}
	}

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x0600215E RID: 8542 RVA: 0x0009DC2C File Offset: 0x0009BE2C
	// (set) Token: 0x0600215F RID: 8543 RVA: 0x0009DC34 File Offset: 0x0009BE34
	public int maxLineCount
	{
		get
		{
			return this.mMaxLineCount;
		}
		set
		{
			if (this.mMaxLineCount != value)
			{
				this.mMaxLineCount = Mathf.Max(value, 0);
				this.shouldBeProcessed = true;
				if (this.overflowMethod == UILabel.Overflow.ShrinkContent)
				{
					this.MakePixelPerfect();
				}
			}
		}
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x06002160 RID: 8544 RVA: 0x0009DC68 File Offset: 0x0009BE68
	// (set) Token: 0x06002161 RID: 8545 RVA: 0x0009DC70 File Offset: 0x0009BE70
	public UILabel.Effect effectStyle
	{
		get
		{
			return this.mEffectStyle;
		}
		set
		{
			if (this.mEffectStyle != value)
			{
				this.mEffectStyle = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x06002162 RID: 8546 RVA: 0x0009DC8C File Offset: 0x0009BE8C
	// (set) Token: 0x06002163 RID: 8547 RVA: 0x0009DC94 File Offset: 0x0009BE94
	public Color effectColor
	{
		get
		{
			return this.mEffectColor;
		}
		set
		{
			if (this.mEffectColor != value)
			{
				this.mEffectColor = value;
				if (this.mEffectStyle != UILabel.Effect.None)
				{
					this.shouldBeProcessed = true;
				}
			}
		}
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x06002164 RID: 8548 RVA: 0x0009DCCC File Offset: 0x0009BECC
	// (set) Token: 0x06002165 RID: 8549 RVA: 0x0009DCD4 File Offset: 0x0009BED4
	public Vector2 effectDistance
	{
		get
		{
			return this.mEffectDistance;
		}
		set
		{
			if (this.mEffectDistance != value)
			{
				this.mEffectDistance = value;
				this.shouldBeProcessed = true;
			}
		}
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x06002166 RID: 8550 RVA: 0x0009DCF8 File Offset: 0x0009BEF8
	// (set) Token: 0x06002167 RID: 8551 RVA: 0x0009DD04 File Offset: 0x0009BF04
	[Obsolete("Use 'overflowMethod == UILabel.Overflow.ShrinkContent' instead")]
	public bool shrinkToFit
	{
		get
		{
			return this.mOverflow == UILabel.Overflow.ShrinkContent;
		}
		set
		{
			if (value)
			{
				this.overflowMethod = UILabel.Overflow.ShrinkContent;
			}
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x06002168 RID: 8552 RVA: 0x0009DD14 File Offset: 0x0009BF14
	public string processedText
	{
		get
		{
			if (this.mLastWidth != this.mWidth || this.mLastHeight != this.mHeight)
			{
				this.mLastWidth = this.mWidth;
				this.mLastHeight = this.mHeight;
				this.mShouldBeProcessed = true;
			}
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return this.mProcessedText;
		}
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x06002169 RID: 8553 RVA: 0x0009DD7C File Offset: 0x0009BF7C
	public Vector2 printedSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return this.mCalculatedSize;
		}
	}

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x0600216A RID: 8554 RVA: 0x0009DD98 File Offset: 0x0009BF98
	public override Vector2 localSize
	{
		get
		{
			if (this.shouldBeProcessed)
			{
				this.ProcessText();
			}
			return base.localSize;
		}
	}

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x0600216B RID: 8555 RVA: 0x0009DDB4 File Offset: 0x0009BFB4
	private bool isValid
	{
		get
		{
			return this.mFont != null || this.mTrueTypeFont != null;
		}
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x0009DDE4 File Offset: 0x0009BFE4
	protected override void OnInit()
	{
		base.OnInit();
		UILabel.mList.Add(this);
		this.SetActiveFont(this.trueTypeFont);
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x0009DE10 File Offset: 0x0009C010
	protected override void OnDisable()
	{
		this.SetActiveFont(null);
		UILabel.mList.Remove(this);
		base.OnDisable();
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x0009DE2C File Offset: 0x0009C02C
	protected void SetActiveFont(Font fnt)
	{
		if (this.mActiveTTF != fnt)
		{
			Font font = this.mActiveTTF;
			int num;
			if (font != null && UILabel.mFontUsage.TryGetValue(font, out num))
			{
				num = Mathf.Max(0, --num);
				if (num == 0)
				{
					UILabel.mFontUsage.Remove(font);
				}
				else
				{
					UILabel.mFontUsage[font] = num;
				}
			}
			this.mActiveTTF = fnt;
			if (fnt != null)
			{
				int num2 = 0;
				UILabel.mFontUsage[fnt] = num2 + 1;
			}
		}
	}

	// Token: 0x0600216F RID: 8559 RVA: 0x0009DEC8 File Offset: 0x0009C0C8
	private static void OnFontChanged(Font font)
	{
		for (int i = 0; i < UILabel.mList.size; i++)
		{
			UILabel uilabel = UILabel.mList[i];
			if (uilabel != null)
			{
				Font trueTypeFont = uilabel.trueTypeFont;
				if (trueTypeFont == font)
				{
					trueTypeFont.RequestCharactersInTexture(uilabel.mText, uilabel.mFinalFontSize, uilabel.mFontStyle);
					uilabel.MarkAsChanged();
					if (uilabel.panel == null)
					{
						uilabel.CreatePanel();
					}
					if (UILabel.mTempDrawcalls == null)
					{
						UILabel.mTempDrawcalls = new List<UIDrawCall>();
					}
					if (uilabel.drawCall != null && !UILabel.mTempDrawcalls.Contains(uilabel.drawCall))
					{
						UILabel.mTempDrawcalls.Add(uilabel.drawCall);
					}
				}
			}
		}
		if (UILabel.mTempDrawcalls != null)
		{
			int j = 0;
			int count = UILabel.mTempDrawcalls.Count;
			while (j < count)
			{
				UIDrawCall uidrawCall = UILabel.mTempDrawcalls[j];
				if (uidrawCall.panel != null)
				{
					uidrawCall.panel.FillDrawCall(uidrawCall);
				}
				j++;
			}
			UILabel.mTempDrawcalls.Clear();
		}
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x0009DFFC File Offset: 0x0009C1FC
	public override Vector3[] GetSides(Transform relativeTo)
	{
		if (this.shouldBeProcessed)
		{
			this.ProcessText();
		}
		return base.GetSides(relativeTo);
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x0009E018 File Offset: 0x0009C218
	protected override void UpgradeFrom265()
	{
		this.ProcessText(true, true);
		if (this.mShrinkToFit)
		{
			this.overflowMethod = UILabel.Overflow.ShrinkContent;
			this.mMaxLineCount = 0;
		}
		if (this.mMaxLineWidth != 0)
		{
			base.width = this.mMaxLineWidth;
			this.overflowMethod = ((this.mMaxLineCount <= 0) ? UILabel.Overflow.ShrinkContent : UILabel.Overflow.ResizeHeight);
		}
		else
		{
			this.overflowMethod = UILabel.Overflow.ResizeFreely;
		}
		if (this.mMaxLineHeight != 0)
		{
			base.height = this.mMaxLineHeight;
		}
		if (this.mFont != null)
		{
			int defaultSize = this.mFont.defaultSize;
			if (base.height < defaultSize)
			{
				base.height = defaultSize;
			}
			this.fontSize = defaultSize;
		}
		this.mMaxLineWidth = 0;
		this.mMaxLineHeight = 0;
		this.mShrinkToFit = false;
		NGUITools.UpdateWidgetCollider(base.gameObject, true);
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x0009E0F4 File Offset: 0x0009C2F4
	protected override void OnAnchor()
	{
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			if (base.isFullyAnchored)
			{
				this.mOverflow = UILabel.Overflow.ShrinkContent;
			}
		}
		else if (this.mOverflow == UILabel.Overflow.ResizeHeight && this.topAnchor.target != null && this.bottomAnchor.target != null)
		{
			this.mOverflow = UILabel.Overflow.ShrinkContent;
		}
		base.OnAnchor();
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x0009E16C File Offset: 0x0009C36C
	private void ProcessAndRequest()
	{
		if (this.ambigiousFont != null)
		{
			this.ProcessText();
		}
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x0009E188 File Offset: 0x0009C388
	protected override void OnEnable()
	{
		base.OnEnable();
		if (!UILabel.mTexRebuildAdded)
		{
			UILabel.mTexRebuildAdded = true;
			Font.textureRebuilt += UILabel.OnFontChanged;
		}
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x0009E1B4 File Offset: 0x0009C3B4
	protected override void OnStart()
	{
		base.OnStart();
		if (this.mLineWidth > 0f)
		{
			this.mMaxLineWidth = Mathf.RoundToInt(this.mLineWidth);
			this.mLineWidth = 0f;
		}
		if (!this.mMultiline)
		{
			this.mMaxLineCount = 1;
			this.mMultiline = true;
		}
		this.mPremultiply = (this.material != null && this.material.shader != null && this.material.shader.name.Contains("Premultiplied"));
		this.ProcessAndRequest();
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x0009E25C File Offset: 0x0009C45C
	public override void MarkAsChanged()
	{
		this.shouldBeProcessed = true;
		base.MarkAsChanged();
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x0009E26C File Offset: 0x0009C46C
	public void ProcessText()
	{
		this.ProcessText(false, true);
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x0009E278 File Offset: 0x0009C478
	private void ProcessText(bool legacyMode, bool full)
	{
		if (!this.isValid)
		{
			return;
		}
		this.mChanged = true;
		this.shouldBeProcessed = false;
		float num = this.mDrawRegion.z - this.mDrawRegion.x;
		float num2 = this.mDrawRegion.w - this.mDrawRegion.y;
		NGUIText.rectWidth = ((!legacyMode) ? base.width : ((this.mMaxLineWidth == 0) ? 1000000 : this.mMaxLineWidth));
		NGUIText.rectHeight = ((!legacyMode) ? base.height : ((this.mMaxLineHeight == 0) ? 1000000 : this.mMaxLineHeight));
		NGUIText.regionWidth = ((num == 1f) ? NGUIText.rectWidth : Mathf.RoundToInt((float)NGUIText.rectWidth * num));
		NGUIText.regionHeight = ((num2 == 1f) ? NGUIText.rectHeight : Mathf.RoundToInt((float)NGUIText.rectHeight * num2));
		this.mFinalFontSize = Mathf.Abs((!legacyMode) ? this.defaultFontSize : Mathf.RoundToInt(base.cachedTransform.localScale.x));
		this.mScale = 1f;
		if (NGUIText.regionWidth < 1 || NGUIText.regionHeight < 0)
		{
			this.mProcessedText = string.Empty;
			return;
		}
		bool flag = this.trueTypeFont != null;
		if (flag && this.keepCrisp)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				this.mDensity = ((!(root != null)) ? 1f : root.pixelSizeAdjustment);
			}
		}
		else
		{
			this.mDensity = 1f;
		}
		if (full)
		{
			this.UpdateNGUIText();
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely)
		{
			NGUIText.rectWidth = 1000000;
			NGUIText.regionWidth = 1000000;
		}
		if (this.mOverflow == UILabel.Overflow.ResizeFreely || this.mOverflow == UILabel.Overflow.ResizeHeight)
		{
			NGUIText.rectHeight = 1000000;
			NGUIText.regionHeight = 1000000;
		}
		if (this.mFinalFontSize > 0)
		{
			bool keepCrisp = this.keepCrisp;
			for (int i = this.mFinalFontSize; i > 0; i--)
			{
				if (keepCrisp)
				{
					this.mFinalFontSize = i;
					NGUIText.fontSize = this.mFinalFontSize;
				}
				else
				{
					this.mScale = (float)i / (float)this.mFinalFontSize;
					NGUIText.fontScale = ((!flag) ? ((float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale) : this.mScale);
				}
				NGUIText.Update(false);
				bool flag2 = NGUIText.WrapText(this.mText, out this.mProcessedText, true, false, this.mOverflowEllipsis && this.mOverflow == UILabel.Overflow.ClampContent);
				if (this.mOverflow != UILabel.Overflow.ShrinkContent || flag2)
				{
					if (this.mOverflow == UILabel.Overflow.ResizeFreely)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						this.mWidth = Mathf.Max(this.minWidth, Mathf.RoundToInt(this.mCalculatedSize.x));
						if (num != 1f)
						{
							this.mWidth = Mathf.RoundToInt((float)this.mWidth / num);
						}
						this.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (num2 != 1f)
						{
							this.mHeight = Mathf.RoundToInt((float)this.mHeight / num2);
						}
						if ((this.mWidth & 1) == 1)
						{
							this.mWidth++;
						}
						if ((this.mHeight & 1) == 1)
						{
							this.mHeight++;
						}
					}
					else if (this.mOverflow == UILabel.Overflow.ResizeHeight)
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
						this.mHeight = Mathf.Max(this.minHeight, Mathf.RoundToInt(this.mCalculatedSize.y));
						if (num2 != 1f)
						{
							this.mHeight = Mathf.RoundToInt((float)this.mHeight / num2);
						}
						if ((this.mHeight & 1) == 1)
						{
							this.mHeight++;
						}
					}
					else
					{
						this.mCalculatedSize = NGUIText.CalculatePrintedSize(this.mProcessedText);
					}
					if (legacyMode)
					{
						base.width = Mathf.RoundToInt(this.mCalculatedSize.x);
						base.height = Mathf.RoundToInt(this.mCalculatedSize.y);
						base.cachedTransform.localScale = Vector3.one;
					}
					break;
				}
				if (--i <= 1)
				{
					break;
				}
			}
		}
		else
		{
			base.cachedTransform.localScale = Vector3.one;
			this.mProcessedText = string.Empty;
			this.mScale = 1f;
		}
		if (full)
		{
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x0009E774 File Offset: 0x0009C974
	public override void MakePixelPerfect()
	{
		if (this.ambigiousFont != null)
		{
			Vector3 localPosition = base.cachedTransform.localPosition;
			localPosition.x = (float)Mathf.RoundToInt(localPosition.x);
			localPosition.y = (float)Mathf.RoundToInt(localPosition.y);
			localPosition.z = (float)Mathf.RoundToInt(localPosition.z);
			base.cachedTransform.localPosition = localPosition;
			base.cachedTransform.localScale = Vector3.one;
			if (this.mOverflow == UILabel.Overflow.ResizeFreely)
			{
				this.AssumeNaturalSize();
			}
			else
			{
				int width = base.width;
				int height = base.height;
				UILabel.Overflow overflow = this.mOverflow;
				if (overflow != UILabel.Overflow.ResizeHeight)
				{
					this.mWidth = 100000;
				}
				this.mHeight = 100000;
				this.mOverflow = UILabel.Overflow.ShrinkContent;
				this.ProcessText(false, true);
				this.mOverflow = overflow;
				int num = Mathf.RoundToInt(this.mCalculatedSize.x);
				int num2 = Mathf.RoundToInt(this.mCalculatedSize.y);
				num = Mathf.Max(num, base.minWidth);
				num2 = Mathf.Max(num2, base.minHeight);
				if ((num & 1) == 1)
				{
					num++;
				}
				if ((num2 & 1) == 1)
				{
					num2++;
				}
				this.mWidth = Mathf.Max(width, num);
				this.mHeight = Mathf.Max(height, num2);
				this.MarkAsChanged();
			}
		}
		else
		{
			base.MakePixelPerfect();
		}
	}

	// Token: 0x0600217A RID: 8570 RVA: 0x0009E8E4 File Offset: 0x0009CAE4
	public void AssumeNaturalSize()
	{
		if (this.ambigiousFont != null)
		{
			this.mWidth = 100000;
			this.mHeight = 100000;
			this.ProcessText(false, true);
			this.mWidth = Mathf.RoundToInt(this.mCalculatedSize.x);
			this.mHeight = Mathf.RoundToInt(this.mCalculatedSize.y);
			if ((this.mWidth & 1) == 1)
			{
				this.mWidth++;
			}
			if ((this.mHeight & 1) == 1)
			{
				this.mHeight++;
			}
			this.MarkAsChanged();
		}
	}

	// Token: 0x0600217B RID: 8571 RVA: 0x0009E98C File Offset: 0x0009CB8C
	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector3 worldPos)
	{
		return this.GetCharacterIndexAtPosition(worldPos, false);
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x0009E998 File Offset: 0x0009CB98
	[Obsolete("Use UILabel.GetCharacterAtPosition instead")]
	public int GetCharacterIndex(Vector2 localPos)
	{
		return this.GetCharacterIndexAtPosition(localPos, false);
	}

	// Token: 0x0600217D RID: 8573 RVA: 0x0009E9A4 File Offset: 0x0009CBA4
	public int GetCharacterIndexAtPosition(Vector3 worldPos, bool precise)
	{
		Vector2 localPos = base.cachedTransform.InverseTransformPoint(worldPos);
		return this.GetCharacterIndexAtPosition(localPos, precise);
	}

	// Token: 0x0600217E RID: 8574 RVA: 0x0009E9CC File Offset: 0x0009CBCC
	public int GetCharacterIndexAtPosition(Vector2 localPos, bool precise)
	{
		if (this.isValid)
		{
			string processedText = this.processedText;
			if (string.IsNullOrEmpty(processedText))
			{
				return 0;
			}
			this.UpdateNGUIText();
			if (precise)
			{
				NGUIText.PrintExactCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			else
			{
				NGUIText.PrintApproximateCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			}
			if (UILabel.mTempVerts.size > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int result = (!precise) ? NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos) : NGUIText.GetExactCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, localPos);
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
				NGUIText.bitmapFont = null;
				NGUIText.dynamicFont = null;
				return result;
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
		}
		return 0;
	}

	// Token: 0x0600217F RID: 8575 RVA: 0x0009EAA4 File Offset: 0x0009CCA4
	public string GetWordAtPosition(Vector3 worldPos)
	{
		int characterIndexAtPosition = this.GetCharacterIndexAtPosition(worldPos, true);
		return this.GetWordAtCharacterIndex(characterIndexAtPosition);
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x0009EAC4 File Offset: 0x0009CCC4
	public string GetWordAtPosition(Vector2 localPos)
	{
		int characterIndexAtPosition = this.GetCharacterIndexAtPosition(localPos, true);
		return this.GetWordAtCharacterIndex(characterIndexAtPosition);
	}

	// Token: 0x06002181 RID: 8577 RVA: 0x0009EAE4 File Offset: 0x0009CCE4
	public string GetWordAtCharacterIndex(int characterIndex)
	{
		if (characterIndex != -1 && characterIndex < this.mText.Length)
		{
			int num = this.mText.LastIndexOfAny(new char[]
			{
				' ',
				'\n'
			}, characterIndex) + 1;
			int num2 = this.mText.IndexOfAny(new char[]
			{
				' ',
				'\n',
				',',
				'.'
			}, characterIndex);
			if (num2 == -1)
			{
				num2 = this.mText.Length;
			}
			if (num != num2)
			{
				int num3 = num2 - num;
				if (num3 > 0)
				{
					string text = this.mText.Substring(num, num3);
					return NGUIText.StripSymbols(text);
				}
			}
		}
		return null;
	}

	// Token: 0x06002182 RID: 8578 RVA: 0x0009EB84 File Offset: 0x0009CD84
	public string GetUrlAtPosition(Vector3 worldPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(worldPos, true));
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x0009EB94 File Offset: 0x0009CD94
	public string GetUrlAtPosition(Vector2 localPos)
	{
		return this.GetUrlAtCharacterIndex(this.GetCharacterIndexAtPosition(localPos, true));
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x0009EBA4 File Offset: 0x0009CDA4
	public string GetUrlAtCharacterIndex(int characterIndex)
	{
		if (characterIndex != -1 && characterIndex < this.mText.Length - 6)
		{
			int num;
			if (this.mText[characterIndex] == '[' && this.mText[characterIndex + 1] == 'u' && this.mText[characterIndex + 2] == 'r' && this.mText[characterIndex + 3] == 'l' && this.mText[characterIndex + 4] == '=')
			{
				num = characterIndex;
			}
			else
			{
				num = this.mText.LastIndexOf("[url=", characterIndex);
			}
			if (num == -1)
			{
				return null;
			}
			num += 5;
			int num2 = this.mText.IndexOf("]", num);
			if (num2 == -1)
			{
				return null;
			}
			int num3 = this.mText.IndexOf("[/url]", num2);
			if (num3 == -1 || characterIndex <= num3)
			{
				return this.mText.Substring(num, num2 - num);
			}
		}
		return null;
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x0009ECA4 File Offset: 0x0009CEA4
	public int GetCharacterIndex(int currentIndex, KeyCode key)
	{
		if (this.isValid)
		{
			string processedText = this.processedText;
			if (string.IsNullOrEmpty(processedText))
			{
				return 0;
			}
			int defaultFontSize = this.defaultFontSize;
			this.UpdateNGUIText();
			NGUIText.PrintApproximateCharacterPositions(processedText, UILabel.mTempVerts, UILabel.mTempIndices);
			if (UILabel.mTempVerts.size > 0)
			{
				this.ApplyOffset(UILabel.mTempVerts, 0);
				int i = 0;
				while (i < UILabel.mTempIndices.size)
				{
					if (UILabel.mTempIndices[i] == currentIndex)
					{
						Vector2 pos = UILabel.mTempVerts[i];
						if (key == KeyCode.UpArrow)
						{
							pos.y += (float)defaultFontSize + this.effectiveSpacingY;
						}
						else if (key == KeyCode.DownArrow)
						{
							pos.y -= (float)defaultFontSize + this.effectiveSpacingY;
						}
						else if (key == KeyCode.Home)
						{
							pos.x -= 1000f;
						}
						else if (key == KeyCode.End)
						{
							pos.x += 1000f;
						}
						int approximateCharacterIndex = NGUIText.GetApproximateCharacterIndex(UILabel.mTempVerts, UILabel.mTempIndices, pos);
						if (approximateCharacterIndex == currentIndex)
						{
							break;
						}
						UILabel.mTempVerts.Clear();
						UILabel.mTempIndices.Clear();
						return approximateCharacterIndex;
					}
					else
					{
						i++;
					}
				}
				UILabel.mTempVerts.Clear();
				UILabel.mTempIndices.Clear();
			}
			NGUIText.bitmapFont = null;
			NGUIText.dynamicFont = null;
			if (key == KeyCode.UpArrow || key == KeyCode.Home)
			{
				return 0;
			}
			if (key == KeyCode.DownArrow || key == KeyCode.End)
			{
				return processedText.Length;
			}
		}
		return currentIndex;
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x0009EE64 File Offset: 0x0009D064
	public void PrintOverlay(int start, int end, UIGeometry caret, UIGeometry highlight, Color caretColor, Color highlightColor)
	{
		if (caret != null)
		{
			caret.Clear();
		}
		if (highlight != null)
		{
			highlight.Clear();
		}
		if (!this.isValid)
		{
			return;
		}
		string processedText = this.processedText;
		this.UpdateNGUIText();
		int size = caret.verts.size;
		Vector2 item = new Vector2(0.5f, 0.5f);
		float finalAlpha = this.finalAlpha;
		if (highlight != null && start != end)
		{
			int size2 = highlight.verts.size;
			NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, highlight.verts);
			if (highlight.verts.size > size2)
			{
				this.ApplyOffset(highlight.verts, size2);
				Color32 item2 = new Color(highlightColor.r, highlightColor.g, highlightColor.b, highlightColor.a * finalAlpha);
				for (int i = size2; i < highlight.verts.size; i++)
				{
					highlight.uvs.Add(item);
					highlight.cols.Add(item2);
				}
			}
		}
		else
		{
			NGUIText.PrintCaretAndSelection(processedText, start, end, caret.verts, null);
		}
		this.ApplyOffset(caret.verts, size);
		Color32 item3 = new Color(caretColor.r, caretColor.g, caretColor.b, caretColor.a * finalAlpha);
		for (int j = size; j < caret.verts.size; j++)
		{
			caret.uvs.Add(item);
			caret.cols.Add(item3);
		}
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
	}

	// Token: 0x06002187 RID: 8583 RVA: 0x0009F014 File Offset: 0x0009D214
	public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		if (!this.isValid)
		{
			return;
		}
		int num = verts.size;
		Color color = base.color;
		color.a = this.finalAlpha;
		if (this.mFont != null && this.mFont.premultipliedAlphaShader)
		{
			color = NGUITools.ApplyPMA(color);
		}
		if (QualitySettings.activeColorSpace == ColorSpace.Linear)
		{
			color.r = Mathf.GammaToLinearSpace(color.r);
			color.g = Mathf.GammaToLinearSpace(color.g);
			color.b = Mathf.GammaToLinearSpace(color.b);
			color.a = Mathf.GammaToLinearSpace(color.a);
		}
		string processedText = this.processedText;
		int size = verts.size;
		this.UpdateNGUIText();
		NGUIText.tint = color;
		NGUIText.Print(processedText, verts, uvs, cols);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		Vector2 vector = this.ApplyOffset(verts, size);
		if (this.mFont != null && this.mFont.packedFontShader)
		{
			return;
		}
		if (this.effectStyle != UILabel.Effect.None)
		{
			int size2 = verts.size;
			vector.x = this.mEffectDistance.x;
			vector.y = this.mEffectDistance.y;
			this.ApplyShadow(verts, uvs, cols, num, size2, vector.x, -vector.y);
			if (this.effectStyle == UILabel.Effect.Outline || this.effectStyle == UILabel.Effect.Outline8)
			{
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, -vector.x, vector.y);
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, vector.x, vector.y);
				num = size2;
				size2 = verts.size;
				this.ApplyShadow(verts, uvs, cols, num, size2, -vector.x, -vector.y);
				if (vector.y >= 2.5f || vector.x >= 2.5f || vector.y <= -2.5f || vector.x <= -2.5f || this.fontSize < 32 || base.height <= 32 || this.effectStyle == UILabel.Effect.Outline8)
				{
					num = size2;
					size2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, size2, -vector.x, 0f);
					num = size2;
					size2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, size2, vector.x, 0f);
					num = size2;
					size2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, size2, 0f, vector.y);
					num = size2;
					size2 = verts.size;
					this.ApplyShadow(verts, uvs, cols, num, size2, 0f, -vector.y);
				}
			}
		}
		if (this.onPostFill != null)
		{
			this.onPostFill(this, num, verts, uvs, cols);
		}
	}

	// Token: 0x06002188 RID: 8584 RVA: 0x0009F314 File Offset: 0x0009D514
	public Vector2 ApplyOffset(BetterList<Vector3> verts, int start)
	{
		Vector2 pivotOffset = base.pivotOffset;
		float num = Mathf.Lerp(0f, (float)(-(float)this.mWidth), pivotOffset.x);
		float num2 = Mathf.Lerp((float)this.mHeight, 0f, pivotOffset.y) + Mathf.Lerp(this.mCalculatedSize.y - (float)this.mHeight, 0f, pivotOffset.y);
		num = Mathf.Round(num);
		num2 = Mathf.Round(num2);
		for (int i = start; i < verts.size; i++)
		{
			Vector3[] buffer = verts.buffer;
			int num3 = i;
			buffer[num3].x = buffer[num3].x + num;
			Vector3[] buffer2 = verts.buffer;
			int num4 = i;
			buffer2[num4].y = buffer2[num4].y + num2;
		}
		return new Vector2(num, num2);
	}

	// Token: 0x06002189 RID: 8585 RVA: 0x0009F3E0 File Offset: 0x0009D5E0
	public void ApplyShadow(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, int start, int end, float x, float y)
	{
		Color color = this.mEffectColor;
		color.a *= this.finalAlpha;
		Color32 color2 = (!(this.bitmapFont != null) || !this.bitmapFont.premultipliedAlphaShader) ? color : NGUITools.ApplyPMA(color);
		for (int i = start; i < end; i++)
		{
			verts.Add(verts.buffer[i]);
			uvs.Add(uvs.buffer[i]);
			cols.Add(cols.buffer[i]);
			Vector3 vector = verts.buffer[i];
			vector.x += x;
			vector.y += y;
			verts.buffer[i] = vector;
			Color32 color3 = cols.buffer[i];
			if (color3.a == 255)
			{
				cols.buffer[i] = color2;
			}
			else
			{
				Color color4 = color;
				color4.a = (float)color3.a / 255f * color.a;
				cols.buffer[i] = ((!(this.bitmapFont != null) || !this.bitmapFont.premultipliedAlphaShader) ? color4 : NGUITools.ApplyPMA(color4));
			}
		}
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x0009F578 File Offset: 0x0009D778
	public int CalculateOffsetToFit(string text)
	{
		this.UpdateNGUIText();
		NGUIText.encoding = false;
		NGUIText.symbolStyle = NGUIText.SymbolStyle.None;
		int result = NGUIText.CalculateOffsetToFit(text);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return result;
	}

	// Token: 0x0600218B RID: 8587 RVA: 0x0009F5AC File Offset: 0x0009D7AC
	public void SetCurrentProgress()
	{
		if (UIProgressBar.current != null)
		{
			this.text = UIProgressBar.current.value.ToString("F");
		}
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x0009F5E8 File Offset: 0x0009D7E8
	public void SetCurrentPercent()
	{
		if (UIProgressBar.current != null)
		{
			this.text = Mathf.RoundToInt(UIProgressBar.current.value * 100f) + "%";
		}
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x0009F630 File Offset: 0x0009D830
	public void SetCurrentSelection()
	{
		if (UIPopupList.current != null)
		{
			this.text = ((!UIPopupList.current.isLocalized) ? UIPopupList.current.value : Localization.Get(UIPopupList.current.value));
		}
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x0009F680 File Offset: 0x0009D880
	public bool Wrap(string text, out string final)
	{
		return this.Wrap(text, out final, 1000000);
	}

	// Token: 0x0600218F RID: 8591 RVA: 0x0009F690 File Offset: 0x0009D890
	public bool Wrap(string text, out string final, int height)
	{
		this.UpdateNGUIText();
		NGUIText.rectHeight = height;
		NGUIText.regionHeight = height;
		bool result = NGUIText.WrapText(text, out final, false);
		NGUIText.bitmapFont = null;
		NGUIText.dynamicFont = null;
		return result;
	}

	// Token: 0x06002190 RID: 8592 RVA: 0x0009F6C8 File Offset: 0x0009D8C8
	public void UpdateNGUIText()
	{
		Font trueTypeFont = this.trueTypeFont;
		bool flag = trueTypeFont != null;
		NGUIText.fontSize = this.mFinalFontSize;
		NGUIText.fontStyle = this.mFontStyle;
		NGUIText.rectWidth = this.mWidth;
		NGUIText.rectHeight = this.mHeight;
		NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
		NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		NGUIText.gradient = (this.mApplyGradient && (this.mFont == null || !this.mFont.packedFontShader));
		NGUIText.gradientTop = this.mGradientTop;
		NGUIText.gradientBottom = this.mGradientBottom;
		NGUIText.encoding = this.mEncoding;
		NGUIText.premultiply = this.mPremultiply;
		NGUIText.symbolStyle = this.mSymbols;
		NGUIText.maxLines = this.mMaxLineCount;
		NGUIText.spacingX = this.effectiveSpacingX;
		NGUIText.spacingY = this.effectiveSpacingY;
		NGUIText.fontScale = ((!flag) ? ((float)this.mFontSize / (float)this.mFont.defaultSize * this.mScale) : this.mScale);
		if (this.mFont != null)
		{
			NGUIText.bitmapFont = this.mFont;
			for (;;)
			{
				UIFont replacement = NGUIText.bitmapFont.replacement;
				if (replacement == null)
				{
					break;
				}
				NGUIText.bitmapFont = replacement;
			}
			if (NGUIText.bitmapFont.isDynamic)
			{
				NGUIText.dynamicFont = NGUIText.bitmapFont.dynamicFont;
				NGUIText.bitmapFont = null;
			}
			else
			{
				NGUIText.dynamicFont = null;
			}
		}
		else
		{
			NGUIText.dynamicFont = trueTypeFont;
			NGUIText.bitmapFont = null;
		}
		if (flag && this.keepCrisp)
		{
			UIRoot root = base.root;
			if (root != null)
			{
				NGUIText.pixelDensity = ((!(root != null)) ? 1f : root.pixelSizeAdjustment);
			}
		}
		else
		{
			NGUIText.pixelDensity = 1f;
		}
		if (this.mDensity != NGUIText.pixelDensity)
		{
			this.ProcessText(false, false);
			NGUIText.rectWidth = this.mWidth;
			NGUIText.rectHeight = this.mHeight;
			NGUIText.regionWidth = Mathf.RoundToInt((float)this.mWidth * (this.mDrawRegion.z - this.mDrawRegion.x));
			NGUIText.regionHeight = Mathf.RoundToInt((float)this.mHeight * (this.mDrawRegion.w - this.mDrawRegion.y));
		}
		if (this.alignment == NGUIText.Alignment.Automatic)
		{
			UIWidget.Pivot pivot = base.pivot;
			if (pivot == UIWidget.Pivot.Left || pivot == UIWidget.Pivot.TopLeft || pivot == UIWidget.Pivot.BottomLeft)
			{
				NGUIText.alignment = NGUIText.Alignment.Left;
			}
			else if (pivot == UIWidget.Pivot.Right || pivot == UIWidget.Pivot.TopRight || pivot == UIWidget.Pivot.BottomRight)
			{
				NGUIText.alignment = NGUIText.Alignment.Right;
			}
			else
			{
				NGUIText.alignment = NGUIText.Alignment.Center;
			}
		}
		else
		{
			NGUIText.alignment = this.alignment;
		}
		NGUIText.Update();
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x0009F9F0 File Offset: 0x0009DBF0
	private void OnApplicationPause(bool paused)
	{
		if (!paused && this.mTrueTypeFont != null)
		{
			this.Invalidate(false);
		}
	}

	// Token: 0x04001585 RID: 5509
	public UILabel.Crispness keepCrispWhenShrunk = UILabel.Crispness.OnDesktop;

	// Token: 0x04001586 RID: 5510
	[SerializeField]
	[HideInInspector]
	private Font mTrueTypeFont;

	// Token: 0x04001587 RID: 5511
	[HideInInspector]
	[SerializeField]
	private UIFont mFont;

	// Token: 0x04001588 RID: 5512
	[SerializeField]
	[Multiline(6)]
	[HideInInspector]
	private string mText = string.Empty;

	// Token: 0x04001589 RID: 5513
	[SerializeField]
	[HideInInspector]
	private int mFontSize = 16;

	// Token: 0x0400158A RID: 5514
	[SerializeField]
	[HideInInspector]
	private FontStyle mFontStyle;

	// Token: 0x0400158B RID: 5515
	[HideInInspector]
	[SerializeField]
	private NGUIText.Alignment mAlignment;

	// Token: 0x0400158C RID: 5516
	[HideInInspector]
	[SerializeField]
	private bool mEncoding = true;

	// Token: 0x0400158D RID: 5517
	[HideInInspector]
	[SerializeField]
	private int mMaxLineCount;

	// Token: 0x0400158E RID: 5518
	[HideInInspector]
	[SerializeField]
	private UILabel.Effect mEffectStyle;

	// Token: 0x0400158F RID: 5519
	[SerializeField]
	[HideInInspector]
	private Color mEffectColor = Color.black;

	// Token: 0x04001590 RID: 5520
	[SerializeField]
	[HideInInspector]
	private NGUIText.SymbolStyle mSymbols = NGUIText.SymbolStyle.Normal;

	// Token: 0x04001591 RID: 5521
	[SerializeField]
	[HideInInspector]
	private Vector2 mEffectDistance = Vector2.one;

	// Token: 0x04001592 RID: 5522
	[SerializeField]
	[HideInInspector]
	private UILabel.Overflow mOverflow;

	// Token: 0x04001593 RID: 5523
	[SerializeField]
	[HideInInspector]
	private Material mMaterial;

	// Token: 0x04001594 RID: 5524
	[HideInInspector]
	[SerializeField]
	private bool mApplyGradient;

	// Token: 0x04001595 RID: 5525
	[SerializeField]
	[HideInInspector]
	private Color mGradientTop = Color.white;

	// Token: 0x04001596 RID: 5526
	[HideInInspector]
	[SerializeField]
	private Color mGradientBottom = new Color(0.7f, 0.7f, 0.7f);

	// Token: 0x04001597 RID: 5527
	[HideInInspector]
	[SerializeField]
	private int mSpacingX;

	// Token: 0x04001598 RID: 5528
	[SerializeField]
	[HideInInspector]
	private int mSpacingY;

	// Token: 0x04001599 RID: 5529
	[SerializeField]
	[HideInInspector]
	private bool mUseFloatSpacing;

	// Token: 0x0400159A RID: 5530
	[SerializeField]
	[HideInInspector]
	private float mFloatSpacingX;

	// Token: 0x0400159B RID: 5531
	[SerializeField]
	[HideInInspector]
	private float mFloatSpacingY;

	// Token: 0x0400159C RID: 5532
	[SerializeField]
	[HideInInspector]
	private bool mOverflowEllipsis;

	// Token: 0x0400159D RID: 5533
	[SerializeField]
	[HideInInspector]
	private bool mShrinkToFit;

	// Token: 0x0400159E RID: 5534
	[SerializeField]
	[HideInInspector]
	private int mMaxLineWidth;

	// Token: 0x0400159F RID: 5535
	[SerializeField]
	[HideInInspector]
	private int mMaxLineHeight;

	// Token: 0x040015A0 RID: 5536
	[HideInInspector]
	[SerializeField]
	private float mLineWidth;

	// Token: 0x040015A1 RID: 5537
	[SerializeField]
	[HideInInspector]
	private bool mMultiline = true;

	// Token: 0x040015A2 RID: 5538
	[NonSerialized]
	private Font mActiveTTF;

	// Token: 0x040015A3 RID: 5539
	[NonSerialized]
	private float mDensity = 1f;

	// Token: 0x040015A4 RID: 5540
	[NonSerialized]
	private bool mShouldBeProcessed = true;

	// Token: 0x040015A5 RID: 5541
	[NonSerialized]
	private string mProcessedText;

	// Token: 0x040015A6 RID: 5542
	[NonSerialized]
	private bool mPremultiply;

	// Token: 0x040015A7 RID: 5543
	[NonSerialized]
	private Vector2 mCalculatedSize = Vector2.zero;

	// Token: 0x040015A8 RID: 5544
	[NonSerialized]
	private float mScale = 1f;

	// Token: 0x040015A9 RID: 5545
	[NonSerialized]
	private int mFinalFontSize;

	// Token: 0x040015AA RID: 5546
	[NonSerialized]
	private int mLastWidth;

	// Token: 0x040015AB RID: 5547
	[NonSerialized]
	private int mLastHeight;

	// Token: 0x040015AC RID: 5548
	private static BetterList<UILabel> mList = new BetterList<UILabel>();

	// Token: 0x040015AD RID: 5549
	private static Dictionary<Font, int> mFontUsage = new Dictionary<Font, int>();

	// Token: 0x040015AE RID: 5550
	private static List<UIDrawCall> mTempDrawcalls;

	// Token: 0x040015AF RID: 5551
	private static bool mTexRebuildAdded = false;

	// Token: 0x040015B0 RID: 5552
	private static BetterList<Vector3> mTempVerts = new BetterList<Vector3>();

	// Token: 0x040015B1 RID: 5553
	private static BetterList<int> mTempIndices = new BetterList<int>();

	// Token: 0x020003A9 RID: 937
	public enum Effect
	{
		// Token: 0x040015B3 RID: 5555
		None,
		// Token: 0x040015B4 RID: 5556
		Shadow,
		// Token: 0x040015B5 RID: 5557
		Outline,
		// Token: 0x040015B6 RID: 5558
		Outline8
	}

	// Token: 0x020003AA RID: 938
	public enum Overflow
	{
		// Token: 0x040015B8 RID: 5560
		ShrinkContent,
		// Token: 0x040015B9 RID: 5561
		ClampContent,
		// Token: 0x040015BA RID: 5562
		ResizeFreely,
		// Token: 0x040015BB RID: 5563
		ResizeHeight
	}

	// Token: 0x020003AB RID: 939
	public enum Crispness
	{
		// Token: 0x040015BD RID: 5565
		Never,
		// Token: 0x040015BE RID: 5566
		OnDesktop,
		// Token: 0x040015BF RID: 5567
		Always
	}
}
