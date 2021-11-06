using System;
using Rilisoft;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200049D RID: 1181
[RequireComponent(typeof(UISprite))]
[RequireComponent(typeof(UICenterOnPanelComponent))]
public class ProfileCup : MonoBehaviour
{
	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x06002A44 RID: 10820 RVA: 0x000DFD74 File Offset: 0x000DDF74
	public UISprite Cup
	{
		get
		{
			UISprite result;
			if ((result = this._cup) == null)
			{
				result = (this._cup = base.GetComponent<UISprite>());
			}
			return result;
		}
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x000DFDA0 File Offset: 0x000DDFA0
	private void Start()
	{
		this._controller = base.gameObject.GetComponentInParents<LeaguesGUIController>();
		this._centerMonitor = base.GetComponent<UICenterOnPanelComponent>();
		this._centerMonitor.OnCentered.RemoveListener(new UnityAction(this.OnCentered));
		this._centerMonitor.OnCentered.AddListener(new UnityAction(this.OnCentered));
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x000DFE04 File Offset: 0x000DE004
	private void OnCentered()
	{
		this._controller.CupCentered(this);
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x000DFE14 File Offset: 0x000DE014
	private void OnEnable()
	{
		this.Outline.SetActive(this.League == RatingSystem.instance.currentLeague);
		this.Cup.spriteName = string.Format("{0} {1}", this.League, 3 - RatingSystem.instance.DivisionInLeague(this.League));
	}

	// Token: 0x04001F4F RID: 8015
	private UISprite _cup;

	// Token: 0x04001F50 RID: 8016
	[SerializeField]
	public RatingSystem.RatingLeague League;

	// Token: 0x04001F51 RID: 8017
	public GameObject Outline;

	// Token: 0x04001F52 RID: 8018
	private LeaguesGUIController _controller;

	// Token: 0x04001F53 RID: 8019
	private UICenterOnPanelComponent _centerMonitor;
}
