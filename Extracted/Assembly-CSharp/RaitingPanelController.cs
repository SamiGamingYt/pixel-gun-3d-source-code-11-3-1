using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004AE RID: 1198
public class RaitingPanelController : MonoBehaviour
{
	// Token: 0x17000773 RID: 1907
	// (get) Token: 0x06002B1C RID: 11036 RVA: 0x000E2DD8 File Offset: 0x000E0FD8
	private int maxRating
	{
		get
		{
			return RatingSystem.instance.MaxRatingInLeague(RatingSystem.instance.currentLeague);
		}
	}

	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x06002B1D RID: 11037 RVA: 0x000E2DF0 File Offset: 0x000E0FF0
	private int league
	{
		get
		{
			return (int)RatingSystem.instance.currentLeague;
		}
	}

	// Token: 0x06002B1E RID: 11038 RVA: 0x000E2DFC File Offset: 0x000E0FFC
	private void Start()
	{
		RatingSystem.OnRatingUpdate += this.OnRatingUpdated;
		if (this.league != 5)
		{
			this.raitingText = string.Format("{0}/{1}", RatingSystem.instance.currentRating, this.maxRating);
			this.oldRaiting.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)this.maxRating, 1f, 1f);
			this.newRating.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)this.maxRating, 1f, 1f);
		}
		else
		{
			this.raitingText = RatingSystem.instance.currentRating.ToString();
			this.oldRaiting.localScale = Vector3.one;
		}
		this.raitingLabel.text = this.raitingText;
	}

	// Token: 0x06002B1F RID: 11039 RVA: 0x000E2EE8 File Offset: 0x000E10E8
	private void OnEnable()
	{
		if (this.league != 5)
		{
			this.raitingText = string.Format("{0}/{1}", RatingSystem.instance.currentRating, this.maxRating);
			this.oldRaiting.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)this.maxRating, 1f, 1f);
			this.newRating.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)this.maxRating, 1f, 1f);
		}
		else
		{
			this.raitingText = RatingSystem.instance.currentRating.ToString();
			this.oldRaiting.localScale = Vector3.one;
		}
		this.raitingLabel.text = this.raitingText;
	}

	// Token: 0x06002B20 RID: 11040 RVA: 0x000E2FC4 File Offset: 0x000E11C4
	private void OnRatingUpdated()
	{
		if (this.league != 5)
		{
			this.raitingText = string.Format("{0}/{1}", RatingSystem.instance.currentRating, this.maxRating);
			this.newRating.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)this.maxRating, 1f, 1f);
			CoroutineRunner.Instance.StartCoroutine(this.AnimateRaitingPanel());
		}
		else
		{
			this.raitingText = RatingSystem.instance.currentRating.ToString();
			this.oldRaiting.localScale = Vector3.one;
		}
		this.raitingLabel.text = this.raitingText;
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x000E3084 File Offset: 0x000E1284
	private void OnDestroy()
	{
		RatingSystem.OnRatingUpdate -= this.OnRatingUpdated;
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x000E3098 File Offset: 0x000E1298
	private IEnumerator AnimateRaitingPanel()
	{
		for (int i = 0; i != 4; i++)
		{
			this.newRating.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.15f);
			this.newRating.gameObject.SetActive(true);
			yield return new WaitForSeconds(0.15f);
		}
		yield return new WaitForSeconds(0.15f);
		this.oldRaiting.localScale = new Vector3((float)RatingSystem.instance.currentRating / (float)this.maxRating, 1f, 1f);
		yield break;
	}

	// Token: 0x0400202B RID: 8235
	[SerializeField]
	private Transform oldRaiting;

	// Token: 0x0400202C RID: 8236
	[SerializeField]
	private Transform newRating;

	// Token: 0x0400202D RID: 8237
	[SerializeField]
	private UILabel raitingLabel;

	// Token: 0x0400202E RID: 8238
	private string raitingText;
}
