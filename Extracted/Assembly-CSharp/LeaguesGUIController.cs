using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using UnityEngine;

// Token: 0x0200070A RID: 1802
public class LeaguesGUIController : MonoBehaviour
{
	// Token: 0x06003E98 RID: 16024 RVA: 0x0014F890 File Offset: 0x0014DA90
	private void OnEnable()
	{
		this._cups = base.GetComponentsInChildren<ProfileCup>(true).ToList<ProfileCup>();
		this._itemsView = base.GetComponentInChildren<LeagueItemsView>(true);
		this.Reposition();
	}

	// Token: 0x06003E99 RID: 16025 RVA: 0x0014F8B8 File Offset: 0x0014DAB8
	private void Reposition()
	{
		this._selectedCup = this._cups.FirstOrDefault((ProfileCup c) => c.League == RatingSystem.instance.currentLeague);
		base.StartCoroutine(this.PositionToCurrentLeagueCoroutine());
	}

	// Token: 0x06003E9A RID: 16026 RVA: 0x0014F8F8 File Offset: 0x0014DAF8
	private IEnumerator PositionToCurrentLeagueCoroutine()
	{
		yield return null;
		ProfileCup to = this._cups.FirstOrDefault((ProfileCup c) => c.League == RatingSystem.instance.currentLeague);
		this._centerOnChild.CenterOn(to.gameObject.transform);
		yield return null;
		this.SetInfoFromLeague(to.League);
		yield break;
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x0014F914 File Offset: 0x0014DB14
	public void CupCentered(ProfileCup cup)
	{
		this._selectedCup = cup;
		this.SetInfoFromLeague(cup.League);
		this._itemsView.Repaint(cup.League);
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x0014F948 File Offset: 0x0014DB48
	private void SetInfoFromLeague(RatingSystem.RatingLeague league)
	{
		string text = LocalizationStore.Get(this._leaguesLKeys[league]);
		this._lblLeagueName.text = text;
		this._lblLeagueNameOutline.text = text;
		if (league < RatingSystem.instance.currentLeague)
		{
			this._progressGO.SetActive(false);
			this._progressTextLabel.gameObject.SetActive(true);
			this._progressTextLabel.text = LocalizationStore.Get("Key_2173");
		}
		else if (league > RatingSystem.instance.currentLeague)
		{
			this._progressGO.SetActive(false);
			this._progressTextLabel.gameObject.SetActive(true);
			int num = RatingSystem.instance.MaxRatingInLeague(league - 1) - RatingSystem.instance.currentRating;
			this._progressTextLabel.text = string.Format(LocalizationStore.Get("Key_2172"), num);
		}
		else if (league == (RatingSystem.RatingLeague)RiliExtensions.EnumNumbers<RatingSystem.RatingLeague>().Max())
		{
			this._progressGO.SetActive(false);
			this._progressTextLabel.gameObject.SetActive(true);
			this._progressTextLabel.text = LocalizationStore.Get("Key_2249");
		}
		else
		{
			this._progressGO.SetActive(true);
			this._progressTextLabel.gameObject.SetActive(false);
			int num2 = RatingSystem.instance.MaxRatingInLeague(league);
			this._lblScore.text = string.Format("{0}/{1}", RatingSystem.instance.currentRating, num2);
			this._sprScoreBar.fillAmount = (float)RatingSystem.instance.currentRating / (float)num2;
		}
	}

	// Token: 0x04002E38 RID: 11832
	[SerializeField]
	private UICenterOnChild _centerOnChild;

	// Token: 0x04002E39 RID: 11833
	[SerializeField]
	private UILabel _lblLeagueName;

	// Token: 0x04002E3A RID: 11834
	[SerializeField]
	private UILabel _lblLeagueNameOutline;

	// Token: 0x04002E3B RID: 11835
	[SerializeField]
	private UILabel _lblScore;

	// Token: 0x04002E3C RID: 11836
	[SerializeField]
	private UISprite _sprScoreBar;

	// Token: 0x04002E3D RID: 11837
	[SerializeField]
	private GameObject _progressGO;

	// Token: 0x04002E3E RID: 11838
	[SerializeField]
	private UILabel _progressTextLabel;

	// Token: 0x04002E3F RID: 11839
	[SerializeField]
	[ReadOnly]
	private List<ProfileCup> _cups;

	// Token: 0x04002E40 RID: 11840
	[ReadOnly]
	[SerializeField]
	private LeagueItemsView _itemsView;

	// Token: 0x04002E41 RID: 11841
	private ProfileCup _selectedCup;

	// Token: 0x04002E42 RID: 11842
	private readonly Dictionary<RatingSystem.RatingLeague, string> _leaguesLKeys = new Dictionary<RatingSystem.RatingLeague, string>(6, RatingSystem.RatingLeagueComparer.Instance)
	{
		{
			RatingSystem.RatingLeague.Wood,
			"Key_1953"
		},
		{
			RatingSystem.RatingLeague.Steel,
			"Key_1954"
		},
		{
			RatingSystem.RatingLeague.Gold,
			"Key_1955"
		},
		{
			RatingSystem.RatingLeague.Crystal,
			"Key_1956"
		},
		{
			RatingSystem.RatingLeague.Ruby,
			"Key_1957"
		},
		{
			RatingSystem.RatingLeague.Adamant,
			"Key_1958"
		}
	};
}
