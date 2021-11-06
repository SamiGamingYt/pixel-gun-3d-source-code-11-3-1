using System;
using UnityEngine;

// Token: 0x02000393 RID: 915
public class UI2DSpriteAnimation : MonoBehaviour
{
	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x06002046 RID: 8262 RVA: 0x00094824 File Offset: 0x00092A24
	public bool isPlaying
	{
		get
		{
			return base.enabled;
		}
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06002047 RID: 8263 RVA: 0x0009482C File Offset: 0x00092A2C
	// (set) Token: 0x06002048 RID: 8264 RVA: 0x00094834 File Offset: 0x00092A34
	public int framesPerSecond
	{
		get
		{
			return this.framerate;
		}
		set
		{
			this.framerate = value;
		}
	}

	// Token: 0x06002049 RID: 8265 RVA: 0x00094840 File Offset: 0x00092A40
	public void Play()
	{
		if (this.frames != null && this.frames.Length > 0)
		{
			if (!base.enabled && !this.loop)
			{
				int num = (this.framerate <= 0) ? (this.mIndex - 1) : (this.mIndex + 1);
				if (num < 0 || num >= this.frames.Length)
				{
					this.mIndex = ((this.framerate >= 0) ? 0 : (this.frames.Length - 1));
				}
			}
			base.enabled = true;
			this.UpdateSprite();
		}
	}

	// Token: 0x0600204A RID: 8266 RVA: 0x000948E4 File Offset: 0x00092AE4
	public void Pause()
	{
		base.enabled = false;
	}

	// Token: 0x0600204B RID: 8267 RVA: 0x000948F0 File Offset: 0x00092AF0
	public void ResetToBeginning()
	{
		this.mIndex = ((this.framerate >= 0) ? 0 : (this.frames.Length - 1));
		this.UpdateSprite();
	}

	// Token: 0x0600204C RID: 8268 RVA: 0x00094928 File Offset: 0x00092B28
	private void Start()
	{
		this.Play();
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x00094930 File Offset: 0x00092B30
	private void Update()
	{
		if (this.frames == null || this.frames.Length == 0)
		{
			base.enabled = false;
		}
		else if (this.framerate != 0)
		{
			float num = (!this.ignoreTimeScale) ? Time.time : RealTime.time;
			if (this.mUpdate < num)
			{
				this.mUpdate = num;
				int num2 = (this.framerate <= 0) ? (this.mIndex - 1) : (this.mIndex + 1);
				if (!this.loop && (num2 < 0 || num2 >= this.frames.Length))
				{
					base.enabled = false;
					return;
				}
				this.mIndex = NGUIMath.RepeatIndex(num2, this.frames.Length);
				this.UpdateSprite();
			}
		}
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x00094A00 File Offset: 0x00092C00
	private void UpdateSprite()
	{
		if (this.mUnitySprite == null && this.mNguiSprite == null)
		{
			this.mUnitySprite = base.GetComponent<SpriteRenderer>();
			this.mNguiSprite = base.GetComponent<UI2DSprite>();
			if (this.mUnitySprite == null && this.mNguiSprite == null)
			{
				base.enabled = false;
				return;
			}
		}
		float num = (!this.ignoreTimeScale) ? Time.time : RealTime.time;
		if (this.framerate != 0)
		{
			this.mUpdate = num + Mathf.Abs(1f / (float)this.framerate);
		}
		if (this.mUnitySprite != null)
		{
			this.mUnitySprite.sprite = this.frames[this.mIndex];
		}
		else if (this.mNguiSprite != null)
		{
			this.mNguiSprite.nextSprite = this.frames[this.mIndex];
		}
	}

	// Token: 0x04001471 RID: 5233
	[SerializeField]
	protected int framerate = 20;

	// Token: 0x04001472 RID: 5234
	public bool ignoreTimeScale = true;

	// Token: 0x04001473 RID: 5235
	public bool loop = true;

	// Token: 0x04001474 RID: 5236
	public Sprite[] frames;

	// Token: 0x04001475 RID: 5237
	private SpriteRenderer mUnitySprite;

	// Token: 0x04001476 RID: 5238
	private UI2DSprite mNguiSprite;

	// Token: 0x04001477 RID: 5239
	private int mIndex;

	// Token: 0x04001478 RID: 5240
	private float mUpdate;
}
