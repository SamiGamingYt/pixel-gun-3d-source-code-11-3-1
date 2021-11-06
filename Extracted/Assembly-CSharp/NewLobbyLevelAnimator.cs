using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020003C5 RID: 965
public class NewLobbyLevelAnimator : MonoBehaviour
{
	// Token: 0x0600232E RID: 9006 RVA: 0x000AE77C File Offset: 0x000AC97C
	private void Awake()
	{
		foreach (GameObject gameObject in this.buttons)
		{
			UISprite component = gameObject.GetComponent<UISprite>();
			component.alpha = 0f;
		}
		foreach (GameObject gameObject2 in this.shines)
		{
			gameObject2.SetActive(false);
		}
		foreach (GameObject gameObject3 in this.tips)
		{
			gameObject3.SetActive(false);
		}
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x000AE8A0 File Offset: 0x000ACAA0
	public void OnMouseDown()
	{
		this._tapped = true;
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x000AE8AC File Offset: 0x000ACAAC
	private IEnumerator Start()
	{
		while (FriendsController.sharedController == null)
		{
			yield return null;
		}
		for (int i = 0; i < this.buttons.Count; i++)
		{
			bool condition = true;
			if (this.conditions[i] == NewLobbyLevelAnimator.CondtionsForShow.Premium)
			{
				condition = (Storager.getInt(Defs.PremiumEnabledFromServer, false) == 1);
			}
			else if (this.conditions[i] == NewLobbyLevelAnimator.CondtionsForShow.PromoOffers)
			{
				condition = (MainMenuController.sharedController != null && MainMenuController.sharedController.PromoOffersPanelShouldBeShown());
			}
			if (condition)
			{
				UISprite sprite = this.buttons[i].GetComponent<UISprite>();
				while (sprite.alpha < 1f)
				{
					sprite.alpha += 0.04f;
					yield return base.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(this.buttonAlphaTime / 25f));
				}
				this.shines[i].SetActive(true);
				yield return new WaitForSeconds(this.timeBetweenShineAndTip);
				this.tips[i].SetActive(true);
				float startTm = Time.realtimeSinceStartup;
				this._tapped = false;
				while (Time.realtimeSinceStartup - startTm < this.timeTipShown && !this._tapped)
				{
					yield return null;
				}
				this._tapped = false;
				this.tips[i].SetActive(false);
				this.shines[i].SetActive(false);
			}
		}
		Storager.setInt(Defs.ShownLobbyLevelSN, Storager.getInt(Defs.ShownLobbyLevelSN, false) + 1, false);
		try
		{
			string tutorialStepsLoggedString = "[]";
			if (Storager.hasKey("AppsFlyer.TutorialStepsLogged"))
			{
				tutorialStepsLoggedString = Storager.getString("AppsFlyer.TutorialStepsLogged", false);
				if (string.IsNullOrEmpty(tutorialStepsLoggedString))
				{
					tutorialStepsLoggedString = "[]";
				}
			}
			int shownLobbyLevel = Storager.getInt(Defs.ShownLobbyLevelSN, false);
			List<object> tutorialStepsLoggedListOfObjects = Json.Deserialize(tutorialStepsLoggedString) as List<object>;
			List<int> tutorialStepsLoggedList = (tutorialStepsLoggedListOfObjects == null) ? new List<int>(2) : tutorialStepsLoggedListOfObjects.Select(new Func<object, int>(Convert.ToInt32)).ToList<int>();
			if (!tutorialStepsLoggedList.Contains(shownLobbyLevel))
			{
				AnalyticsFacade.SendCustomEventToAppsFlyer("af_gui_tutorial_completion", new Dictionary<string, string>
				{
					{
						"step",
						shownLobbyLevel.ToString()
					}
				});
				tutorialStepsLoggedList.Add(shownLobbyLevel);
				Storager.setString("AppsFlyer.TutorialStepsLogged", Json.Serialize(tutorialStepsLoggedList), false);
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			Debug.LogWarning(ex.ToString());
		}
		yield return null;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001768 RID: 5992
	private const int numOFStepsWhenAppearingButton = 25;

	// Token: 0x04001769 RID: 5993
	public float buttonAlphaTime = 1f;

	// Token: 0x0400176A RID: 5994
	public float timeBetweenShineAndTip = 0.3f;

	// Token: 0x0400176B RID: 5995
	public float timeTipShown = 5f;

	// Token: 0x0400176C RID: 5996
	public List<GameObject> buttons;

	// Token: 0x0400176D RID: 5997
	public List<GameObject> tips;

	// Token: 0x0400176E RID: 5998
	public List<GameObject> shines;

	// Token: 0x0400176F RID: 5999
	public List<NewLobbyLevelAnimator.CondtionsForShow> conditions;

	// Token: 0x04001770 RID: 6000
	private bool _tapped;

	// Token: 0x020003C6 RID: 966
	public enum CondtionsForShow
	{
		// Token: 0x04001772 RID: 6002
		None,
		// Token: 0x04001773 RID: 6003
		PromoOffers,
		// Token: 0x04001774 RID: 6004
		Premium
	}
}
