using System;
using UnityEngine;

// Token: 0x02000154 RID: 340
public class GadgetButton : MonoBehaviour
{
	// Token: 0x06000B4C RID: 2892 RVA: 0x000401E8 File Offset: 0x0003E3E8
	public void OpenGadgetPanel(bool isOpen)
	{
		this.gadgetAnimator.SetBool("IsOpen", isOpen);
		JoystickController.rightJoystick.gadgetPanelVisible = isOpen;
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x00040208 File Offset: 0x0003E408
	public void OnPanelOpen()
	{
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000B4E RID: 2894 RVA: 0x0004020C File Offset: 0x0003E40C
	public UISprite cachedSprite
	{
		get
		{
			if (this._cachedSprite == null)
			{
				this._cachedSprite = base.GetComponent<UISprite>();
			}
			return this._cachedSprite;
		}
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x00040234 File Offset: 0x0003E434
	private void Update()
	{
		float num = (float)this.cachedSprite.width / 90f;
		this.ContainerForScale.localScale = new Vector3(num, num, num);
	}

	// Token: 0x040008DA RID: 2266
	public const int DefaultCircleSize = 90;

	// Token: 0x040008DB RID: 2267
	public const int DefaultSpaceBetweenCircles = 20;

	// Token: 0x040008DC RID: 2268
	public GameObject thirdAvailableGadgetCell;

	// Token: 0x040008DD RID: 2269
	public GameObject thirdAvailableGadgetFrame;

	// Token: 0x040008DE RID: 2270
	public GameObject yazichok;

	// Token: 0x040008DF RID: 2271
	[Header("Duration Sprites")]
	public UISprite duration;

	// Token: 0x040008E0 RID: 2272
	public UISprite duration1;

	// Token: 0x040008E1 RID: 2273
	public UISprite duration2;

	// Token: 0x040008E2 RID: 2274
	[Header("Cooldown Sprites")]
	public UISprite cooldown;

	// Token: 0x040008E3 RID: 2275
	public UISprite cooldown1;

	// Token: 0x040008E4 RID: 2276
	public UISprite cooldown2;

	// Token: 0x040008E5 RID: 2277
	[Header("Cooldown Sprites")]
	public GameObject cooldownEnds;

	// Token: 0x040008E6 RID: 2278
	public GameObject cooldown1Ends;

	// Token: 0x040008E7 RID: 2279
	public GameObject cooldown2Ends;

	// Token: 0x040008E8 RID: 2280
	[Header("Count Labels")]
	public UILabel count;

	// Token: 0x040008E9 RID: 2281
	public UILabel count1;

	// Token: 0x040008EA RID: 2282
	public UILabel count2;

	// Token: 0x040008EB RID: 2283
	[Header("Gadget Icons")]
	public UITexture gadgetIcon;

	// Token: 0x040008EC RID: 2284
	public UITexture gadgetIcon1;

	// Token: 0x040008ED RID: 2285
	public UITexture gadgetIcon2;

	// Token: 0x040008EE RID: 2286
	[Header("Containers With Count Labels")]
	public GameObject counter;

	// Token: 0x040008EF RID: 2287
	public GameObject counter1;

	// Token: 0x040008F0 RID: 2288
	public GameObject counter2;

	// Token: 0x040008F1 RID: 2289
	public Animator gadgetAnimator;

	// Token: 0x040008F2 RID: 2290
	public bool isOpen;

	// Token: 0x040008F3 RID: 2291
	public Transform ContainerForScale;

	// Token: 0x040008F4 RID: 2292
	private UISprite _cachedSprite;
}
