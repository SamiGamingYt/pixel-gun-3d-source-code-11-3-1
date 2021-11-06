using System;
using UnityEngine;

// Token: 0x020002D6 RID: 726
public class ItemPreviewInArmoryInfoScreen : MonoBehaviour
{
	// Token: 0x1400001C RID: 28
	// (add) Token: 0x0600196C RID: 6508 RVA: 0x00065320 File Offset: 0x00063520
	// (remove) Token: 0x0600196D RID: 6509 RVA: 0x0006533C File Offset: 0x0006353C
	public event Action<ItemPreviewInArmoryInfoScreen, ShopNGUIController.CategoryNames> OnSelect;

	// Token: 0x0600196E RID: 6510 RVA: 0x00065358 File Offset: 0x00063558
	public void SetSelected(bool _isSelected, bool isMomentumScale = false)
	{
		this.isSelected = _isSelected;
		if (isMomentumScale)
		{
			if (this.isSelected)
			{
				this.thisTranform.localScale = new Vector3(ItemPreviewInArmoryInfoScreen.maxScale, ItemPreviewInArmoryInfoScreen.maxScale, ItemPreviewInArmoryInfoScreen.maxScale);
			}
			else
			{
				this.thisTranform.localScale = new Vector3(ItemPreviewInArmoryInfoScreen.minScale, ItemPreviewInArmoryInfoScreen.minScale, ItemPreviewInArmoryInfoScreen.minScale);
			}
		}
		if (!this.isSelected)
		{
			this.model.localRotation = Quaternion.Euler(this.baseRotation);
		}
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x000653E4 File Offset: 0x000635E4
	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (!this.isSelected && this.OnSelect != null)
		{
			this.OnSelect(this, this.category);
		}
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x00065434 File Offset: 0x00063634
	private void OnDrag(Vector2 delta)
	{
		if (this.isSelected)
		{
			Vector3 eulerAngles = this.model.localRotation.eulerAngles;
			this.model.localRotation = Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y - delta.x, eulerAngles.z));
			this.timeFromRotate = 0f;
		}
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x000654A0 File Offset: 0x000636A0
	private void Awake()
	{
		this.thisTranform = base.transform;
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x000654B0 File Offset: 0x000636B0
	private void Update()
	{
		float num = Time.realtimeSinceStartup - this.oldTime;
		if (num > 0.5f)
		{
			num = 0f;
		}
		this.oldTime = Time.realtimeSinceStartup;
		if (this.timeFromRotate < 3f)
		{
			this.timeFromRotate += Time.unscaledDeltaTime;
			if (this.timeFromRotate >= 3f)
			{
				this.model.localRotation = Quaternion.Euler(this.baseRotation);
			}
		}
		float x = this.thisTranform.localScale.x;
		if (this.isSelected && x < ItemPreviewInArmoryInfoScreen.maxScale)
		{
			float num2 = x + num * 2f;
			this.thisTranform.localScale = new Vector3(num2, num2, num2);
		}
		if (!this.isSelected && x > ItemPreviewInArmoryInfoScreen.minScale)
		{
			float num3 = x - num * 2f;
			this.thisTranform.localScale = new Vector3(num3, num3, num3);
		}
	}

	// Token: 0x04000E8A RID: 3722
	private Transform thisTranform;

	// Token: 0x04000E8B RID: 3723
	private bool isSelected;

	// Token: 0x04000E8C RID: 3724
	public string id;

	// Token: 0x04000E8D RID: 3725
	public ShopNGUIController.CategoryNames category;

	// Token: 0x04000E8E RID: 3726
	public string headName;

	// Token: 0x04000E8F RID: 3727
	public int numUpgrade;

	// Token: 0x04000E90 RID: 3728
	public Transform model;

	// Token: 0x04000E91 RID: 3729
	public Vector3 baseRotation;

	// Token: 0x04000E92 RID: 3730
	public static readonly float maxScale = 1.34f;

	// Token: 0x04000E93 RID: 3731
	public static readonly float minScale = 0.8f;

	// Token: 0x04000E94 RID: 3732
	private float timeFromRotate = 1000f;

	// Token: 0x04000E95 RID: 3733
	private float oldTime;
}
