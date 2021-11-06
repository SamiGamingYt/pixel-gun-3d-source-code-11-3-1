using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200066C RID: 1644
	public class InAppBonusLobbyController : MonoBehaviour
	{
		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06003932 RID: 14642 RVA: 0x00128678 File Offset: 0x00126878
		// (set) Token: 0x06003933 RID: 14643 RVA: 0x00128680 File Offset: 0x00126880
		public static InAppBonusLobbyController Instance { get; private set; }

		// Token: 0x06003934 RID: 14644 RVA: 0x00128688 File Offset: 0x00126888
		private void Awake()
		{
			InAppBonusLobbyController.Instance = this;
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x00128690 File Offset: 0x00126890
		private void Start()
		{
			this._scrollStartPos = this._scroll.transform.localPosition;
			this._gridStartPos = this._buttonsGrid.transform.localPosition;
		}

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06003936 RID: 14646 RVA: 0x001286CC File Offset: 0x001268CC
		// (set) Token: 0x06003937 RID: 14647 RVA: 0x001286D4 File Offset: 0x001268D4
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				if (value == this._enabled)
				{
					return;
				}
				if (value)
				{
					this._centerOnChild.CenterOn(null);
					this._scroll.transform.localPosition = Vector3.zero;
					this._buttonsGrid.transform.localPosition = Vector3.zero;
					this._scroll.gameObject.transform.localPosition = Vector2.zero;
					this._scroll.gameObject.GetComponent<UIPanel>().clipOffset = Vector2.zero;
					this._scrollViewsRoot.gameObject.transform.localPosition = Vector3.zero;
					this.UpdateViews();
					this._scroll.ResetPosition();
					this._scrollViewsRoot.WrapContent();
					this._scrollViewsRoot.SortBasedOnScrollMovement();
					this._buttonsGrid.Reposition();
				}
				else
				{
					this._scroll.transform.localPosition = Vector3.right * 10000f;
					this._buttonsGrid.transform.localPosition = Vector3.right * 10000f;
				}
				this._enabled = value;
			}
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x001287FC File Offset: 0x001269FC
		private void Update()
		{
			if (!this._enabled)
			{
				return;
			}
			this._rotateDelayLeft -= Time.deltaTime;
			if (this._rotateDelayLeft <= 0f)
			{
				this._rotateDelayLeft = this._rotateDelay;
				if (Mathf.Approximately(this._panelAcceleration, 0f))
				{
					this.RotateScrollToNext();
				}
			}
			this._panelAcceleration = Mathf.Abs(this._scroll.transform.localPosition.x - this._prevPanelPosX);
			this._prevPanelPosX = this._scroll.transform.localPosition.x;
			this._loadBonusesTimeLeft -= Time.deltaTime;
			if (this._loadBonusesTimeLeft <= 0f)
			{
				this._loadBonusesTimeLeft = 1f;
				this._cachedBonuses = BalanceController.GetCurrentInnapBonus();
			}
			this.UpdateViews();
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x001288E4 File Offset: 0x00126AE4
		private void OnDestroy()
		{
			InAppBonusLobbyController.Instance = null;
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x001288EC File Offset: 0x00126AEC
		private void UpdateViews()
		{
			List<string> list = new List<string>();
			if (this._cachedBonuses != null)
			{
				foreach (Dictionary<string, object> dictionary in this._cachedBonuses)
				{
					string text = Convert.ToString(dictionary["ID"]);
					if (!text.IsNullOrEmpty())
					{
						list.Add(text);
						this.SetScrollView(text, dictionary);
						this.SetButtonView(text, dictionary);
					}
				}
			}
			List<string> list2 = this._scrollViews.Keys.Except(list).ToList<string>();
			if (list2.Any<string>())
			{
				if (this._scrollViews.Any<KeyValuePair<string, InAppBonusLobbyScrollView>>())
				{
					foreach (string key in list2)
					{
						this._scrollViews[key].gameObject.transform.SetParent(null);
						UnityEngine.Object.Destroy(this._scrollViews[key].gameObject);
						this._scrollViews.Remove(key);
					}
					this._scroll.ResetPosition();
					this._scrollViewsRoot.WrapContent();
					this._scrollViewsRoot.SortBasedOnScrollMovement();
				}
				if (this._buttonsViews.Any<KeyValuePair<string, InAppBonusLobbyButtonView>>())
				{
					foreach (string key2 in list2)
					{
						this._buttonsViews[key2].gameObject.transform.SetParent(null);
						UnityEngine.Object.Destroy(this._buttonsViews[key2].gameObject);
						this._buttonsViews.Remove(key2);
					}
					this._buttonsGrid.Reposition();
				}
			}
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x00128B24 File Offset: 0x00126D24
		private void SetScrollView(string id, Dictionary<string, object> bonusData)
		{
			if (this._scrollViews.ContainsKey(id))
			{
				this._scrollViews[id].SetData(bonusData);
			}
			else
			{
				if (this._scrollViewPrefabHandler.Prefab == null)
				{
					return;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._scrollViewPrefabHandler.Prefab);
				if (gameObject == null)
				{
					return;
				}
				InAppBonusLobbyScrollView component = gameObject.GetComponent<InAppBonusLobbyScrollView>();
				if (component == null)
				{
					UnityEngine.Object.Destroy(gameObject);
					return;
				}
				component.SetData(bonusData);
				gameObject.transform.SetParent(this._scrollViewsRoot.gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				this._scrollViews.Add(id, component);
				this._scroll.ResetPosition();
				this._scrollViewsRoot.WrapContent();
				this._scrollViewsRoot.SortBasedOnScrollMovement();
			}
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x00128C28 File Offset: 0x00126E28
		private void SetButtonView(string id, Dictionary<string, object> bonusData)
		{
			if (this._buttonsViews.ContainsKey(id))
			{
				this._buttonsViews[id].SetData(bonusData);
			}
			else
			{
				if (this._buttonViewPrefabHandler.Prefab == null)
				{
					return;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this._buttonViewPrefabHandler.Prefab);
				if (gameObject == null)
				{
					return;
				}
				InAppBonusLobbyButtonView component = gameObject.GetComponent<InAppBonusLobbyButtonView>();
				if (component == null)
				{
					UnityEngine.Object.Destroy(gameObject);
					return;
				}
				component.SetData(bonusData);
				gameObject.transform.SetParent(this._buttonsGrid.gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				this._buttonsViews.Add(id, component);
				this._buttonsGrid.Reposition();
			}
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x00128D18 File Offset: 0x00126F18
		private void RotateScrollToNext()
		{
			if (this._scrollViews.Count == 1)
			{
				this._scroll.transform.localPosition = Vector3.zero;
				this._scroll.GetComponent<UIPanel>().clipOffset = Vector3.zero;
				return;
			}
			if (this._scrollViews.Count < 2)
			{
				return;
			}
			if (!this._centerOnChild.enabled)
			{
				return;
			}
			if (this._centerOnChild.centeredObject == null)
			{
				this._centerOnChild.CenterOn(this._scrollViews.First<KeyValuePair<string, InAppBonusLobbyScrollView>>().Value.gameObject.transform);
			}
			else
			{
				List<Transform> list = this._centerOnChild.centeredObject.transform.Neighbors(false);
				if (!list.Any<Transform>())
				{
					return;
				}
				if (list.Count == 1)
				{
					this._centerOnChild.CenterOn(list.First<Transform>());
				}
				else
				{
					List<Transform> source = (from n in list
					where n.localPosition.x > this._centerOnChild.centeredObject.transform.localPosition.x
					select n).ToList<Transform>();
					if (!source.Any<Transform>())
					{
						return;
					}
					Transform transform = (from t in source
					orderby t.transform.localPosition.x
					select t).First<Transform>();
					if (transform != null)
					{
						this._centerOnChild.CenterOn(transform);
					}
				}
			}
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x00128E78 File Offset: 0x00127078
		public void Click()
		{
			if (base.gameObject.activeSelf && MainMenuController.sharedController != null)
			{
				MainMenuController.sharedController.ShowBankWindow();
			}
		}

		// Token: 0x040029F4 RID: 10740
		[SerializeField]
		[Header("[ scroll settings ]")]
		private PrefabHandler _scrollViewPrefabHandler;

		// Token: 0x040029F5 RID: 10741
		[SerializeField]
		private UIWrapContent _scrollViewsRoot;

		// Token: 0x040029F6 RID: 10742
		[SerializeField]
		private UIScrollView _scroll;

		// Token: 0x040029F7 RID: 10743
		[SerializeField]
		private UICenterOnChild _centerOnChild;

		// Token: 0x040029F8 RID: 10744
		[SerializeField]
		private float _rotateDelay = 2f;

		// Token: 0x040029F9 RID: 10745
		[SerializeField]
		[Header("[ buttons grid settings ]")]
		private PrefabHandler _buttonViewPrefabHandler;

		// Token: 0x040029FA RID: 10746
		[SerializeField]
		private UIGrid _buttonsGrid;

		// Token: 0x040029FB RID: 10747
		private readonly Dictionary<string, InAppBonusLobbyScrollView> _scrollViews = new Dictionary<string, InAppBonusLobbyScrollView>();

		// Token: 0x040029FC RID: 10748
		private readonly Dictionary<string, InAppBonusLobbyButtonView> _buttonsViews = new Dictionary<string, InAppBonusLobbyButtonView>();

		// Token: 0x040029FD RID: 10749
		private Vector3 _scrollStartPos;

		// Token: 0x040029FE RID: 10750
		private Vector3 _gridStartPos;

		// Token: 0x040029FF RID: 10751
		private bool _enabled = true;

		// Token: 0x04002A00 RID: 10752
		private float _rotateDelayLeft;

		// Token: 0x04002A01 RID: 10753
		private float _prevPanelPosX = -1f;

		// Token: 0x04002A02 RID: 10754
		private float _panelAcceleration;

		// Token: 0x04002A03 RID: 10755
		private float _loadBonusesTimeLeft;

		// Token: 0x04002A04 RID: 10756
		private List<Dictionary<string, object>> _cachedBonuses = new List<Dictionary<string, object>>();
	}
}
