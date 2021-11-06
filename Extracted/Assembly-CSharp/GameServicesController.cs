using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x02000643 RID: 1603
internal sealed class GameServicesController : MonoBehaviour
{
	// Token: 0x17000914 RID: 2324
	// (get) Token: 0x06003749 RID: 14153 RVA: 0x0011D364 File Offset: 0x0011B564
	public static GameServicesController Instance
	{
		get
		{
			return GameServicesController._instance;
		}
	}

	// Token: 0x0600374A RID: 14154 RVA: 0x0011D36C File Offset: 0x0011B56C
	public void WaitAuthenticationAndIncrementBeginnerAchievement()
	{
		using (new StopwatchLogger("WaitAuthenticationAndIncrementBeginnerAchievement()"))
		{
			base.StartCoroutine(GameServicesController.WaitAndIncrementBeginnerAchievementCoroutine());
		}
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x0011D3C0 File Offset: 0x0011B5C0
	private static IEnumerator WaitAndIncrementBeginnerAchievementCoroutine()
	{
		int oldGamesStartedCount = PlayerPrefs.GetInt("GamesStartedCount", 0);
		int newGamesStartedCount = oldGamesStartedCount + 1;
		PlayerPrefs.SetInt("GamesStartedCount", newGamesStartedCount);
		float step = 20f;
		switch (BuildSettings.BuildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			Debug.Log("Social platform local user authenticated: " + Social.localUser.authenticated);
			while (!Social.localUser.authenticated)
			{
				yield return new WaitForSeconds(2f);
			}
			Debug.Log("Social platform local user authenticated: " + Social.localUser.userName + ",\t\tid: " + Social.localUser.id);
			Debug.Log(string.Format("Trying to report {0} achievement...", "beginner_id"));
			Social.ReportProgress("beginner_id", (double)((float)newGamesStartedCount * step), delegate(bool success)
			{
				Debug.Log(string.Format("Achievement {0} incremented: {1}", "beginner_id", success));
			});
			break;
		case RuntimePlatform.Android:
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				Debug.Log("Social platform local user authenticated: " + Social.localUser.authenticated);
				while (!Social.localUser.authenticated)
				{
					yield return new WaitForSeconds(2f);
				}
				Debug.Log("Social platform local user authenticated: " + Social.localUser.userName + ",\t\tid: " + Social.localUser.id);
				Debug.Log("Trying to increment Beginner achievement...");
				GpgFacade.Instance.IncrementAchievement("CgkIr8rGkPIJEAIQBg", 1, delegate(bool success)
				{
					Debug.Log("Achievement Beginner incremented: " + success);
				});
			}
			else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
			{
				WaitForSeconds waitForSeconds = new WaitForSeconds(2f);
				while (!AGSClient.IsServiceReady())
				{
					yield return waitForSeconds;
				}
				Debug.Log("Social platform local user authenticated: " + Social.localUser.authenticated);
				while (!Social.localUser.authenticated)
				{
					yield return waitForSeconds;
				}
				Debug.Log("Social platform local user authenticated: " + GameCircleSocial.Instance.localUser.userName + ",\t\tid: " + GameCircleSocial.Instance.localUser.id);
				Debug.Log("Trying to increment Beginner achievement...");
				AGSAchievementsClient.UpdateAchievementProgress("beginner_id", (float)newGamesStartedCount * step, 0);
			}
			break;
		}
		yield break;
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x0011D3D4 File Offset: 0x0011B5D4
	private void Awake()
	{
		if (GameServicesController._instance != null)
		{
			Debug.LogWarning(base.GetType().Name + " already exists.");
		}
		GameServicesController._instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x04002840 RID: 10304
	private static GameServicesController _instance;
}
