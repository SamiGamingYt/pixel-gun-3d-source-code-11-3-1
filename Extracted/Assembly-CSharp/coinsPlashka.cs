using System;
using UnityEngine;

// Token: 0x02000792 RID: 1938
public sealed class coinsPlashka : MonoBehaviour
{
	// Token: 0x17000BB8 RID: 3000
	// (get) Token: 0x06004583 RID: 17795 RVA: 0x001780F4 File Offset: 0x001762F4
	public static Rect symmetricRect
	{
		get
		{
			Rect result = new Rect(coinsPlashka.thisScript.rectLabelCoins.x, coinsPlashka.thisScript.rectButCoins.y, coinsPlashka.thisScript.rectButCoins.width, coinsPlashka.thisScript.rectButCoins.height);
			result.x = (float)Screen.width - result.x - result.width;
			return result;
		}
	}

	// Token: 0x06004584 RID: 17796 RVA: 0x00178164 File Offset: 0x00176364
	private void Awake()
	{
		coinsPlashka.thisScript = base.gameObject.GetComponent<coinsPlashka>();
		coinsPlashka.hidePlashka();
		this.tekKolCoins = Storager.getInt("Coins", false);
		this.lastTImeFetchedeychain = Time.realtimeSinceStartup;
		this.isHasKeyAchived500 = PlayerPrefs.HasKey("Achieved500");
		this.isHasKeyAchived1000 = PlayerPrefs.HasKey("Achieved1000");
	}

	// Token: 0x06004585 RID: 17797 RVA: 0x001781C4 File Offset: 0x001763C4
	public static void showPlashka()
	{
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = true;
		}
	}

	// Token: 0x06004586 RID: 17798 RVA: 0x001781E4 File Offset: 0x001763E4
	public static void hidePlashka()
	{
		if (coinsPlashka.thisScript != null)
		{
			coinsPlashka.thisScript.enabled = false;
		}
	}

	// Token: 0x06004587 RID: 17799 RVA: 0x00178204 File Offset: 0x00176404
	private void Update()
	{
		if (!Social.localUser.authenticated)
		{
			return;
		}
		if (Time.frameCount % 60 != 23)
		{
			return;
		}
		if (this.tekKolCoins >= 500 && !this.isHasKeyAchived500)
		{
			Social.ReportProgress("CgkIr8rGkPIJEAIQBA", 100.0, delegate(bool success)
			{
				Debug.Log(string.Format("Achievement Ekonomist completed: {0}", success));
			});
			PlayerPrefs.SetInt("Achieved500", 1);
			this.isHasKeyAchived500 = true;
		}
		if (this.tekKolCoins >= 1000 && !this.isHasKeyAchived1000)
		{
			Social.ReportProgress("CgkIr8rGkPIJEAIQBQ", 100.0, delegate(bool success)
			{
				Debug.Log(string.Format("Achievement Rich Man completed: {0}", success));
			});
			PlayerPrefs.SetInt("Achieved1000", 1);
			this.isHasKeyAchived1000 = true;
		}
	}

	// Token: 0x04003300 RID: 13056
	public static coinsPlashka thisScript;

	// Token: 0x04003301 RID: 13057
	public static bool hideButtonCoins;

	// Token: 0x04003302 RID: 13058
	private float kfSize = (float)Screen.height / 768f;

	// Token: 0x04003303 RID: 13059
	private bool isHasKeyAchived500;

	// Token: 0x04003304 RID: 13060
	private bool isHasKeyAchived1000;

	// Token: 0x04003305 RID: 13061
	public Rect rectButCoins;

	// Token: 0x04003306 RID: 13062
	public Rect rectLabelCoins;

	// Token: 0x04003307 RID: 13063
	private int tekKolCoins;

	// Token: 0x04003308 RID: 13064
	private float lastTImeFetchedeychain;

	// Token: 0x04003309 RID: 13065
	private Font f;
}
