using System;
using UnityEngine;

// Token: 0x02000679 RID: 1657
public class InnerWeaponPars : MonoBehaviour
{
	// Token: 0x060039A6 RID: 14758 RVA: 0x0012B238 File Offset: 0x00129438
	private void Awake()
	{
		this.FindArms();
	}

	// Token: 0x060039A7 RID: 14759 RVA: 0x0012B240 File Offset: 0x00129440
	private void FindArms()
	{
		if (this.renderArms != null)
		{
			return;
		}
		SkinnedMeshRenderer[] componentsInChildren = base.GetComponentsInChildren<SkinnedMeshRenderer>(true);
		if (componentsInChildren != null)
		{
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
			{
				if (skinnedMeshRenderer != null && skinnedMeshRenderer.gameObject != this.bonusPrefab)
				{
					this.renderArms = skinnedMeshRenderer;
					return;
				}
			}
		}
	}

	// Token: 0x060039A8 RID: 14760 RVA: 0x0012B2B0 File Offset: 0x001294B0
	public void SetMaterialForArms(Material shMat)
	{
		if (this.renderArms == null)
		{
			this.FindArms();
		}
		if (this.renderArms != null)
		{
			this.renderArms.sharedMaterial = shMat;
		}
	}

	// Token: 0x04002A5B RID: 10843
	public GameObject particlePoint;

	// Token: 0x04002A5C RID: 10844
	public Transform LeftArmorHand;

	// Token: 0x04002A5D RID: 10845
	public Transform RightArmorHand;

	// Token: 0x04002A5E RID: 10846
	public Transform grenatePoint;

	// Token: 0x04002A5F RID: 10847
	public AudioClip shoot;

	// Token: 0x04002A60 RID: 10848
	public AudioClip reload;

	// Token: 0x04002A61 RID: 10849
	public AudioClip empty;

	// Token: 0x04002A62 RID: 10850
	public AudioClip idle;

	// Token: 0x04002A63 RID: 10851
	public AudioClip zoomIn;

	// Token: 0x04002A64 RID: 10852
	public AudioClip zoomOut;

	// Token: 0x04002A65 RID: 10853
	public AudioClip charge;

	// Token: 0x04002A66 RID: 10854
	public GameObject bonusPrefab;

	// Token: 0x04002A67 RID: 10855
	public GameObject shockerEffect;

	// Token: 0x04002A68 RID: 10856
	public GameObject fakeGrenade;

	// Token: 0x04002A69 RID: 10857
	public GameObject animationObject;

	// Token: 0x04002A6A RID: 10858
	public Texture preview;

	// Token: 0x04002A6B RID: 10859
	public Texture2D aimTextureV;

	// Token: 0x04002A6C RID: 10860
	public Texture2D aimTextureH;

	// Token: 0x04002A6D RID: 10861
	private SkinnedMeshRenderer renderArms;
}
