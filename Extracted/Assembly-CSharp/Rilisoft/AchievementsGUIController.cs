using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200051D RID: 1309
	public class AchievementsGUIController : MonoBehaviour
	{
		// Token: 0x06002D95 RID: 11669 RVA: 0x000EFBF8 File Offset: 0x000EDDF8
		private void Awake()
		{
			base.StartCoroutine(this.PopulateViews());
			AchievementView.OnClicked += this.AchievementView_OnClicked;
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000EFC18 File Offset: 0x000EDE18
		private IEnumerator PopulateViews()
		{
			while (!Singleton<AchievementsManager>.Instance.IsReady)
			{
				yield return null;
			}
			foreach (Achievement ach in Singleton<AchievementsManager>.Instance.AvailableAchiements)
			{
				this.CreateView(ach);
				this._grid.Reposition();
				this._scrollView.ResetPosition();
			}
			ObservableList<Achievement> availableAchiements = Singleton<AchievementsManager>.Instance.AvailableAchiements;
			availableAchiements.OnItemInserted = (Action<int, Achievement>)Delegate.Combine(availableAchiements.OnItemInserted, new Action<int, Achievement>(this.OnAchievementAdded));
			ObservableList<Achievement> availableAchiements2 = Singleton<AchievementsManager>.Instance.AvailableAchiements;
			availableAchiements2.OnItemRemoved = (Action<int, Achievement>)Delegate.Combine(availableAchiements2.OnItemRemoved, new Action<int, Achievement>(this.OnAchievementRemoved));
			yield break;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000EFC34 File Offset: 0x000EDE34
		private AchievementView CreateView(Achievement ach)
		{
			AchievementView achievementView = UnityEngine.Object.Instantiate<AchievementView>(this._viewPrefab);
			achievementView.Achievement = ach;
			this._views.Add(achievementView);
			achievementView.gameObject.transform.SetParent(this._grid.gameObject.transform);
			achievementView.gameObject.transform.localPosition = Vector3.zero;
			achievementView.gameObject.transform.localScale = Vector3.one;
			return achievementView;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000EFCAC File Offset: 0x000EDEAC
		private void OnAchievementAdded(int pos, Achievement ach)
		{
			AchievementView achievementView = this.CreateView(ach);
			achievementView.gameObject.transform.SetSiblingIndex(pos);
			this._grid.Reposition();
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000EFCE0 File Offset: 0x000EDEE0
		private void OnAchievementRemoved(int pos, Achievement ach)
		{
			AchievementView achievementView = this._views.FirstOrDefault((AchievementView v) => v.Achievement == ach);
			if (achievementView != null)
			{
				this._views.Remove(achievementView);
				achievementView.gameObject.transform.SetParent(base.gameObject.transform);
				achievementView.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(achievementView);
			}
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000EFD58 File Offset: 0x000EDF58
		private void AchievementView_OnClicked(AchievementView obj)
		{
			this._infoView.Show(obj.Achievement);
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000EFD6C File Offset: 0x000EDF6C
		private void OnDestroy()
		{
			AchievementView.OnClicked -= this.AchievementView_OnClicked;
		}

		// Token: 0x04002217 RID: 8727
		[SerializeField]
		private UIScrollView _scrollView;

		// Token: 0x04002218 RID: 8728
		[SerializeField]
		private UIGrid _grid;

		// Token: 0x04002219 RID: 8729
		[SerializeField]
		private AchievementView _viewPrefab;

		// Token: 0x0400221A RID: 8730
		[ReadOnly]
		[SerializeField]
		private List<AchievementView> _views = new List<AchievementView>();

		// Token: 0x0400221B RID: 8731
		[SerializeField]
		private AchievementInfoView _infoView;
	}
}
