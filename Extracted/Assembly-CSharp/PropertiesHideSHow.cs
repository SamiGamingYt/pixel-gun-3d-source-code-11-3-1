using System;
using UnityEngine;

// Token: 0x020004AB RID: 1195
public class PropertiesHideSHow : MonoBehaviour
{
	// Token: 0x06002B0B RID: 11019 RVA: 0x000E2A24 File Offset: 0x000E0C24
	private void Update()
	{
		if (this.isAnimated)
		{
			if (this.animTimer > 0f)
			{
				this.animTimer -= Time.unscaledDeltaTime;
				if (this.isHidden)
				{
					this.target.localPosition = Vector3.Lerp(this.HiddenPosition, this.startPosition, this.animTimer / this.animTime);
				}
				else
				{
					this.target.localPosition = Vector3.Lerp(this.startPosition, this.HiddenPosition, this.animTimer / this.animTime);
				}
			}
			else
			{
				this.isAnimated = false;
			}
		}
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x000E2ACC File Offset: 0x000E0CCC
	public void OnHideClick()
	{
		this.isHidden = !this.isHidden;
		this.isAnimated = true;
		this.animTimer = this.animTime;
		this.AdjustUI();
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x000E2B04 File Offset: 0x000E0D04
	public void SetState(bool isShown)
	{
		this.isHidden = !isShown;
		this.isAnimated = false;
		if (isShown)
		{
			this.target.localPosition = this.startPosition;
		}
		else
		{
			this.target.localPosition = this.HiddenPosition;
		}
		this.AdjustUI();
	}

	// Token: 0x17000770 RID: 1904
	// (get) Token: 0x06002B0E RID: 11022 RVA: 0x000E2B58 File Offset: 0x000E0D58
	private Vector3 HiddenPosition
	{
		get
		{
			return new Vector3(this.startPosition.x, -300f - this.yAxysDelta, this.startPosition.z);
		}
	}

	// Token: 0x06002B0F RID: 11023 RVA: 0x000E2B94 File Offset: 0x000E0D94
	private void AdjustUI()
	{
		if (this.isHidden)
		{
			this.text.text = "SHOW";
			this.arrow.eulerAngles = new Vector3(0f, 0f, -90f);
		}
		else
		{
			this.text.text = "HIDE";
			this.arrow.eulerAngles = new Vector3(0f, 0f, 90f);
		}
	}

	// Token: 0x17000771 RID: 1905
	// (get) Token: 0x06002B10 RID: 11024 RVA: 0x000E2C10 File Offset: 0x000E0E10
	private Vector3 startPosition
	{
		get
		{
			if (!this._startPositionInitialized)
			{
				this._startPositionInitialized = true;
				this._startPosition = new Vector3(0f, -300f, 0f);
			}
			return this._startPosition;
		}
	}

	// Token: 0x0400201B RID: 8219
	private const float shownPositionY = -300f;

	// Token: 0x0400201C RID: 8220
	public bool isHidden;

	// Token: 0x0400201D RID: 8221
	public float yAxysDelta;

	// Token: 0x0400201E RID: 8222
	public float animTime;

	// Token: 0x0400201F RID: 8223
	public Transform target;

	// Token: 0x04002020 RID: 8224
	public Transform arrow;

	// Token: 0x04002021 RID: 8225
	public UILabel text;

	// Token: 0x04002022 RID: 8226
	private float animTimer;

	// Token: 0x04002023 RID: 8227
	private Vector3 _startPosition;

	// Token: 0x04002024 RID: 8228
	private bool _startPositionInitialized;

	// Token: 0x04002025 RID: 8229
	private bool isAnimated;
}
