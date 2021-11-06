using System;
using UnityEngine;

// Token: 0x02000298 RID: 664
public class HeartEffect : MonoBehaviour
{
	// Token: 0x06001518 RID: 5400 RVA: 0x000538C4 File Offset: 0x00051AC4
	public string GetMechHealthSpriteName(int index)
	{
		if (WeaponManager.sharedManager.myPlayerMoveC.currentMech == null)
		{
			return string.Empty;
		}
		return WeaponManager.sharedManager.myPlayerMoveC.currentMech.healthSpriteName[index];
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x00053908 File Offset: 0x00051B08
	public void Animate(int index, HeartEffect.IndicatorType type)
	{
		this.heartIndex = index;
		if (this.heartIndex < 1)
		{
			this.heartIndex = 0;
			this.ShowHide(false);
		}
		else
		{
			if (this.heartIndex == 1)
			{
				this.ShowHide(true);
			}
			switch (type)
			{
			case HeartEffect.IndicatorType.Hearts:
				this.spriteName = "heart" + Mathf.Min(3, this.heartIndex).ToString();
				break;
			case HeartEffect.IndicatorType.Armor:
				this.spriteName = this.armSpriteName[this.heartIndex - 1];
				break;
			case HeartEffect.IndicatorType.Mech:
				this.spriteName = this.GetMechHealthSpriteName(this.heartIndex - 1);
				break;
			}
			this.ChangeSpriteEffect(this.spriteName);
		}
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x000539D0 File Offset: 0x00051BD0
	public void SetIndex(int index, HeartEffect.IndicatorType type)
	{
		this.heartIndex = index;
		this.activeEffect = false;
		this.targetScale = Vector3.one;
		base.transform.localScale = Vector3.one;
		if (this.heartIndex < 1)
		{
			this.heartIndex = 0;
			base.gameObject.SetActive(false);
			this.activeHeart = false;
			switch (type)
			{
			case HeartEffect.IndicatorType.Hearts:
				this.mySprite.spriteName = "heart1";
				break;
			case HeartEffect.IndicatorType.Armor:
				this.mySprite.spriteName = this.armSpriteName[0];
				break;
			case HeartEffect.IndicatorType.Mech:
				this.mySprite.spriteName = this.GetMechHealthSpriteName(0);
				break;
			}
		}
		else
		{
			this.activeHeart = true;
			base.gameObject.SetActive(true);
			switch (type)
			{
			case HeartEffect.IndicatorType.Hearts:
				this.mySprite.spriteName = "heart" + Mathf.Min(3, this.heartIndex).ToString();
				break;
			case HeartEffect.IndicatorType.Armor:
				this.mySprite.spriteName = this.armSpriteName[this.heartIndex - 1];
				break;
			case HeartEffect.IndicatorType.Mech:
				this.mySprite.spriteName = this.GetMechHealthSpriteName(this.heartIndex - 1);
				break;
			}
		}
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x00053B28 File Offset: 0x00051D28
	private void ShowHide(bool show)
	{
		this.activeHeart = show;
		this.activeEffect = true;
		if (show)
		{
			base.gameObject.SetActive(true);
			this.targetScale = Vector3.one;
		}
		else
		{
			this.targetScale = Vector3.one * 0.001f;
		}
	}

	// Token: 0x0600151C RID: 5404 RVA: 0x00053B7C File Offset: 0x00051D7C
	private void ChangeSpriteEffect(string newSprite)
	{
		this.spriteName = newSprite;
		this.activeEffect = true;
		this.activeHeart = true;
		this.targetScale = Vector3.one * 1.7f;
		base.gameObject.SetActive(true);
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x00053BC0 File Offset: 0x00051DC0
	private void Awake()
	{
		this.mySprite = base.GetComponent<UISprite>();
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x00053BD0 File Offset: 0x00051DD0
	private void Update()
	{
		if (this.activeEffect)
		{
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, this.targetScale, 7f * Time.deltaTime);
			if (base.transform.localScale == this.targetScale)
			{
				if (!this.activeHeart || base.transform.localScale == Vector3.one)
				{
					this.activeEffect = false;
					base.gameObject.SetActive(this.activeHeart);
				}
				else
				{
					this.mySprite.spriteName = this.spriteName;
					this.targetScale = Vector3.one;
				}
			}
		}
	}

	// Token: 0x04000C53 RID: 3155
	private UISprite mySprite;

	// Token: 0x04000C54 RID: 3156
	private bool activeEffect;

	// Token: 0x04000C55 RID: 3157
	private bool activeHeart;

	// Token: 0x04000C56 RID: 3158
	private Vector3 targetScale;

	// Token: 0x04000C57 RID: 3159
	private string spriteName;

	// Token: 0x04000C58 RID: 3160
	public int heartIndex;

	// Token: 0x04000C59 RID: 3161
	private readonly string[] armSpriteName = new string[]
	{
		"wood_armor",
		"armor",
		"gold_armor",
		"crystal_armor",
		"red_armor",
		"adamant_armor",
		"adamant_armor"
	};

	// Token: 0x02000299 RID: 665
	public enum IndicatorType
	{
		// Token: 0x04000C5B RID: 3163
		Hearts,
		// Token: 0x04000C5C RID: 3164
		Armor,
		// Token: 0x04000C5D RID: 3165
		Mech
	}
}
