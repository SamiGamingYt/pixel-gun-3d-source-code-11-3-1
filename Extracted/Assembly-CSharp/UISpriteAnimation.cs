using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003B4 RID: 948
[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation")]
public class UISpriteAnimation : MonoBehaviour
{
	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x0600220C RID: 8716 RVA: 0x000A37BC File Offset: 0x000A19BC
	public int frames
	{
		get
		{
			return this.mSpriteNames.Count;
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x0600220D RID: 8717 RVA: 0x000A37CC File Offset: 0x000A19CC
	// (set) Token: 0x0600220E RID: 8718 RVA: 0x000A37D4 File Offset: 0x000A19D4
	public int framesPerSecond
	{
		get
		{
			return this.mFPS;
		}
		set
		{
			this.mFPS = value;
		}
	}

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x0600220F RID: 8719 RVA: 0x000A37E0 File Offset: 0x000A19E0
	// (set) Token: 0x06002210 RID: 8720 RVA: 0x000A37E8 File Offset: 0x000A19E8
	public string namePrefix
	{
		get
		{
			return this.mPrefix;
		}
		set
		{
			if (this.mPrefix != value)
			{
				this.mPrefix = value;
				this.RebuildSpriteList();
			}
		}
	}

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x06002211 RID: 8721 RVA: 0x000A3808 File Offset: 0x000A1A08
	// (set) Token: 0x06002212 RID: 8722 RVA: 0x000A3810 File Offset: 0x000A1A10
	public bool loop
	{
		get
		{
			return this.mLoop;
		}
		set
		{
			this.mLoop = value;
		}
	}

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x06002213 RID: 8723 RVA: 0x000A381C File Offset: 0x000A1A1C
	public bool isPlaying
	{
		get
		{
			return this.mActive;
		}
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x000A3824 File Offset: 0x000A1A24
	protected virtual void Start()
	{
		this.RebuildSpriteList();
	}

	// Token: 0x06002215 RID: 8725 RVA: 0x000A382C File Offset: 0x000A1A2C
	protected virtual void Update()
	{
		if (this.mActive && this.mSpriteNames.Count > 1 && Application.isPlaying && this.mFPS > 0)
		{
			this.mDelta += RealTime.deltaTime;
			float num = 1f / (float)this.mFPS;
			if (num < this.mDelta)
			{
				this.mDelta = ((num <= 0f) ? 0f : (this.mDelta - num));
				if (++this.mIndex >= this.mSpriteNames.Count)
				{
					this.mIndex = 0;
					this.mActive = this.mLoop;
				}
				if (this.mActive)
				{
					this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
					if (this.mSnap)
					{
						this.mSprite.MakePixelPerfect();
					}
				}
			}
		}
	}

	// Token: 0x06002216 RID: 8726 RVA: 0x000A392C File Offset: 0x000A1B2C
	public void RebuildSpriteList()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			List<UISpriteData> spriteList = this.mSprite.atlas.spriteList;
			int i = 0;
			int count = spriteList.Count;
			while (i < count)
			{
				UISpriteData uispriteData = spriteList[i];
				if (string.IsNullOrEmpty(this.mPrefix) || uispriteData.name.StartsWith(this.mPrefix))
				{
					this.mSpriteNames.Add(uispriteData.name);
				}
				i++;
			}
			this.mSpriteNames.Sort();
		}
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x000A39FC File Offset: 0x000A1BFC
	public void Play()
	{
		this.mActive = true;
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x000A3A08 File Offset: 0x000A1C08
	public void Pause()
	{
		this.mActive = false;
	}

	// Token: 0x06002219 RID: 8729 RVA: 0x000A3A14 File Offset: 0x000A1C14
	public void ResetToBeginning()
	{
		this.mActive = true;
		this.mIndex = 0;
		if (this.mSprite != null && this.mSpriteNames.Count > 0)
		{
			this.mSprite.spriteName = this.mSpriteNames[this.mIndex];
			if (this.mSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	// Token: 0x04001609 RID: 5641
	[HideInInspector]
	[SerializeField]
	protected int mFPS = 30;

	// Token: 0x0400160A RID: 5642
	[HideInInspector]
	[SerializeField]
	protected string mPrefix = string.Empty;

	// Token: 0x0400160B RID: 5643
	[HideInInspector]
	[SerializeField]
	protected bool mLoop = true;

	// Token: 0x0400160C RID: 5644
	[SerializeField]
	[HideInInspector]
	protected bool mSnap = true;

	// Token: 0x0400160D RID: 5645
	protected UISprite mSprite;

	// Token: 0x0400160E RID: 5646
	protected float mDelta;

	// Token: 0x0400160F RID: 5647
	protected int mIndex;

	// Token: 0x04001610 RID: 5648
	protected bool mActive = true;

	// Token: 0x04001611 RID: 5649
	protected List<string> mSpriteNames = new List<string>();
}
