using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007FF RID: 2047
public class MainMenuHeroCamera : MonoBehaviour
{
	// Token: 0x140000B4 RID: 180
	// (add) Token: 0x06004A70 RID: 19056 RVA: 0x001A72F4 File Offset: 0x001A54F4
	// (remove) Token: 0x06004A71 RID: 19057 RVA: 0x001A730C File Offset: 0x001A550C
	public static event Action onEndOpenGift;

	// Token: 0x140000B5 RID: 181
	// (add) Token: 0x06004A72 RID: 19058 RVA: 0x001A7324 File Offset: 0x001A5524
	// (remove) Token: 0x06004A73 RID: 19059 RVA: 0x001A733C File Offset: 0x001A553C
	public static event Action onEndCloseGift;

	// Token: 0x06004A74 RID: 19060 RVA: 0x001A7354 File Offset: 0x001A5554
	public void Start()
	{
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		base.transform.rotation = Quaternion.Euler(new Vector3(eulerAngles.x, this.YawForMainMenu, eulerAngles.z));
	}

	// Token: 0x06004A75 RID: 19061 RVA: 0x001A73A0 File Offset: 0x001A55A0
	private void OnEnable()
	{
		ButOpenGift.onOpen += this.OnShowGift;
		GiftBannerWindow.onClose += this.OnCloseGift;
	}

	// Token: 0x06004A76 RID: 19062 RVA: 0x001A73D0 File Offset: 0x001A55D0
	private void OnDisable()
	{
		ButOpenGift.onOpen -= this.OnShowGift;
		GiftBannerWindow.onClose -= this.OnCloseGift;
	}

	// Token: 0x17000C39 RID: 3129
	// (get) Token: 0x06004A77 RID: 19063 RVA: 0x001A7400 File Offset: 0x001A5600
	private float YawForMainMenu
	{
		get
		{
			return (!MenuLeaderboardsView.IsNeedShow) ? 6f : 0f;
		}
	}

	// Token: 0x17000C3A RID: 3130
	// (get) Token: 0x06004A78 RID: 19064 RVA: 0x001A741C File Offset: 0x001A561C
	// (set) Token: 0x06004A79 RID: 19065 RVA: 0x001A7424 File Offset: 0x001A5624
	public bool IsAnimPlaying { get; private set; }

	// Token: 0x06004A7A RID: 19066 RVA: 0x001A7430 File Offset: 0x001A5630
	public void OnCloseGift()
	{
		base.GetComponent<Animation>()[this.animGotcha].speed = -1f;
		base.GetComponent<Animation>()[this.animGotcha].time = base.GetComponent<Animation>()[this.animGotcha].length;
		base.GetComponent<Animation>().Play(this.animGotcha);
		base.StartCoroutine(this.WaitAnimEnd(this.animGotcha));
	}

	// Token: 0x06004A7B RID: 19067 RVA: 0x001A74AC File Offset: 0x001A56AC
	public void OnShowGift()
	{
		base.GetComponent<Animation>()[this.animGotcha].speed = 1f;
		base.GetComponent<Animation>()[this.animGotcha].time = 0f;
		base.GetComponent<Animation>().Play(this.animGotcha);
		base.StartCoroutine(this.WaitAnimEnd(this.animGotcha));
	}

	// Token: 0x06004A7C RID: 19068 RVA: 0x001A7514 File Offset: 0x001A5714
	private void EndAnimation(string nameAnim)
	{
		if (nameAnim != null)
		{
			if (MainMenuHeroCamera.<>f__switch$map14 == null)
			{
				MainMenuHeroCamera.<>f__switch$map14 = new Dictionary<string, int>(1)
				{
					{
						"MainMenuOpenGotcha",
						0
					}
				};
			}
			int num;
			if (MainMenuHeroCamera.<>f__switch$map14.TryGetValue(nameAnim, out num))
			{
				if (num == 0)
				{
					if (base.GetComponent<Animation>()[this.animGotcha].speed > 0f)
					{
						if (MainMenuHeroCamera.onEndOpenGift != null)
						{
							MainMenuHeroCamera.onEndOpenGift();
						}
					}
					else if (MainMenuHeroCamera.onEndCloseGift != null)
					{
						MainMenuHeroCamera.onEndCloseGift();
					}
				}
			}
		}
	}

	// Token: 0x06004A7D RID: 19069 RVA: 0x001A75BC File Offset: 0x001A57BC
	private IEnumerator WaitAnimEnd(string nameAnim)
	{
		while (base.GetComponent<Animation>().IsPlaying(nameAnim))
		{
			yield return null;
		}
		this.EndAnimation(nameAnim);
		yield break;
	}

	// Token: 0x06004A7E RID: 19070 RVA: 0x001A75E8 File Offset: 0x001A57E8
	public void OnMainMenuOpenSocial()
	{
		this.PlayAnim(14.5f);
		if (MenuLeaderboardsController.sharedController != null && MenuLeaderboardsController.sharedController.menuLeaderboardsView != null)
		{
			MenuLeaderboardsController.sharedController.menuLeaderboardsView.Show(false, false);
		}
	}

	// Token: 0x06004A7F RID: 19071 RVA: 0x001A7638 File Offset: 0x001A5838
	public void OnMainMenuOpenOptions()
	{
		this.PlayAnim(23.5f);
		if (MenuLeaderboardsController.sharedController != null && MenuLeaderboardsController.sharedController.menuLeaderboardsView != null)
		{
			MenuLeaderboardsController.sharedController.menuLeaderboardsView.Show(false, false);
		}
	}

	// Token: 0x06004A80 RID: 19072 RVA: 0x001A7688 File Offset: 0x001A5888
	public void OnMainMenuCloseOptions()
	{
		this.PlayAnim(this.YawForMainMenu);
		if (MenuLeaderboardsController.sharedController != null && MenuLeaderboardsController.sharedController.menuLeaderboardsView != null && MenuLeaderboardsView.IsNeedShow)
		{
			MenuLeaderboardsController.sharedController.menuLeaderboardsView.Show(true, true);
		}
	}

	// Token: 0x06004A81 RID: 19073 RVA: 0x001A76E4 File Offset: 0x001A58E4
	public void OnMainMenuOpenLeaderboards()
	{
		this.PlayAnim(0f);
	}

	// Token: 0x06004A82 RID: 19074 RVA: 0x001A76F4 File Offset: 0x001A58F4
	public void OnMainMenuCloseLeaderboards()
	{
		this.PlayAnim(6f);
	}

	// Token: 0x06004A83 RID: 19075 RVA: 0x001A7704 File Offset: 0x001A5904
	private void PlayAnim(float endYaw)
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.PlayAnimCoroutine(endYaw));
	}

	// Token: 0x06004A84 RID: 19076 RVA: 0x001A771C File Offset: 0x001A591C
	private IEnumerator PlayAnimCoroutine(float endYaw)
	{
		this.IsAnimPlaying = true;
		Transform heroCameraTransform = base.transform;
		Quaternion heroCameraRotation = heroCameraTransform.rotation;
		float startTime = Time.realtimeSinceStartup;
		Quaternion startRotation = heroCameraRotation;
		Quaternion endRotation = Quaternion.Euler(heroCameraRotation.eulerAngles.x, endYaw, heroCameraRotation.eulerAngles.z);
		while (Time.realtimeSinceStartup - startTime <= 1f)
		{
			float deltaFromStart = Time.realtimeSinceStartup - startTime;
			if (deltaFromStart <= 0.1f)
			{
				yield return null;
			}
			heroCameraTransform.rotation = Quaternion.Lerp(startRotation, endRotation, (deltaFromStart - 0.1f) / 0.9f);
			yield return null;
		}
		heroCameraTransform.rotation = endRotation;
		this.IsAnimPlaying = false;
		yield break;
	}

	// Token: 0x06004A85 RID: 19077 RVA: 0x001A7748 File Offset: 0x001A5948
	public void OnOpenSingleModePanel()
	{
		base.StopAllCoroutines();
		this.moveMenuAnimator.enabled = false;
	}

	// Token: 0x06004A86 RID: 19078 RVA: 0x001A775C File Offset: 0x001A595C
	public void OnCloseSingleModePanel()
	{
		this.moveMenuAnimator.enabled = true;
		this.PlayAnim(this.YawForMainMenu);
	}

	// Token: 0x0400371D RID: 14109
	public Animator moveMenuAnimator;

	// Token: 0x0400371E RID: 14110
	private string animGotcha = "MainMenuOpenGotcha";

	// Token: 0x0400371F RID: 14111
	private string animNormal = "MainMenuCloseOptions";
}
