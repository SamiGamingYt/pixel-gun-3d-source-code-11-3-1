using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020002A1 RID: 673
internal sealed class HintController : MonoBehaviour
{
	// Token: 0x17000263 RID: 611
	// (get) Token: 0x0600153F RID: 5439 RVA: 0x00054588 File Offset: 0x00052788
	public static HintController instance
	{
		get
		{
			if (HintController._instances.Any<HintController>())
			{
				return HintController._instances.Last<HintController>();
			}
			return null;
		}
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x000545A8 File Offset: 0x000527A8
	private void Awake()
	{
		HintController._instances.Add(this);
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x000545B8 File Offset: 0x000527B8
	private void OnDestroy()
	{
		HintController._instances.Remove(this);
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x000545C8 File Offset: 0x000527C8
	private void Start()
	{
		this.showNextEvent = new EventDelegate(this, "ShowNext");
		base.Invoke("StartShow", 0.5f);
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x000545EC File Offset: 0x000527EC
	private void OnEnable()
	{
		base.Invoke("StartShow", 0.5f);
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x00054600 File Offset: 0x00052800
	private bool CheckShowReason(HintController.HintItem hint)
	{
		HintController.ShowReason showReason = hint.showReason;
		return showReason == HintController.ShowReason.New || (showReason == HintController.ShowReason.TrainingStage && !TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == hint.trainingStage);
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x00054644 File Offset: 0x00052844
	public void ShowHintByName(string name, float time = 0f)
	{
		if (this.hints == null)
		{
			return;
		}
		for (int i = 0; i < this.hints.Length; i++)
		{
			if (this.hints[i].name == name && !this.hintToShow.Contains(this.hints[i]))
			{
				this.hintToShow.Add(this.hints[i]);
				if (time == 0f)
				{
					this.ShowHint();
				}
				else
				{
					base.Invoke("ShowHint", time);
				}
			}
		}
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x000546DC File Offset: 0x000528DC
	public void HideCurrentHintObjectLabel()
	{
		if (this.hintToShow.Count > 0)
		{
			this.hintObject.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x0005470C File Offset: 0x0005290C
	public void ShowCurrentHintObjectLabel()
	{
		if (this.hintToShow.Count > 0)
		{
			this.hintObject.gameObject.SetActive(true);
		}
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x0005473C File Offset: 0x0005293C
	public void HideHintByName(string name)
	{
		if (this.inShow != null && this.inShow.name == name)
		{
			this.ShowNext();
		}
		else
		{
			for (int i = 0; i < this.hintToShow.Count; i++)
			{
				if (this.hintToShow[i].name == name)
				{
					this.hintToShow.RemoveAt(i);
					break;
				}
			}
		}
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x000547C0 File Offset: 0x000529C0
	public void StartShow()
	{
		if (this.hints != null && this.hintToShow.Count == 0)
		{
			for (int i = 0; i < this.hints.Length; i++)
			{
				if (this.CheckShowReason(this.hints[i]))
				{
					this.hintToShow.Add(this.hints[i]);
				}
			}
		}
		this.ShowHint();
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x00054830 File Offset: 0x00052A30
	public void ShowNext()
	{
		if (this.hintToShow.Count == 0)
		{
			return;
		}
		if (this.hintToShow[0].hideReason == HintController.HideReason.ButtonClick)
		{
			this.hintToShow[0].target.GetComponent<UIButton>().onClick.Remove(this.showNextEvent);
		}
		if (this.hintToShow[0].buttonsToBlock != null && this.hintToShow[0].buttonsToBlock.Length > 0)
		{
			for (int i = 0; i < this.hintToShow[0].buttonsToBlock.Length; i++)
			{
				this.hintToShow[0].buttonsToBlock[i].isEnabled = this.hintToShow[0].buttonsState[i];
			}
		}
		if (this.hintToShow[0].objectsToHide != null && this.hintToShow[0].objectsToHide.Length > 0)
		{
			for (int j = 0; j < this.hintToShow[0].objectsToHide.Length; j++)
			{
				this.hintToShow[0].objectsToHide[j].SetActive(this.hintToShow[0].objActiveState[j]);
			}
		}
		if (this.hintToShow[0].enableColliders)
		{
			this.hintToShow[0].collidersObj.SetActive(false);
		}
		this.hintToShow.RemoveAt(0);
		this.hintObject.Hide();
		this.inShow = null;
		this.ShowHint();
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x000549DC File Offset: 0x00052BDC
	private void ShowHint()
	{
		if (this.hintToShow.Count > 0)
		{
			HintController.HintItem hintItem = this.hintToShow[0];
			if (this.inShow == hintItem)
			{
				return;
			}
			HintController.HideReason hideReason = hintItem.hideReason;
			if (hideReason == HintController.HideReason.ButtonClick)
			{
				hintItem.targetButton = hintItem.target.GetComponent<UIButton>();
				hintItem.targetButton.onClick.Add(this.showNextEvent);
			}
			if (hintItem.timeout > 0f)
			{
				base.Invoke("ShowNext", hintItem.timeout);
			}
			if (hintItem.buttonsToBlock != null && hintItem.buttonsToBlock.Length > 0)
			{
				hintItem.buttonsState = new bool[hintItem.buttonsToBlock.Length];
				for (int i = 0; i < hintItem.buttonsToBlock.Length; i++)
				{
					hintItem.buttonsState[i] = hintItem.buttonsToBlock[i].isEnabled;
					if (hintItem.buttonsToBlock[i] != hintItem.target.GetComponent<UIButton>())
					{
						hintItem.buttonsToBlock[i].isEnabled = false;
					}
				}
			}
			if (hintItem.objectsToHide != null && hintItem.objectsToHide.Length > 0)
			{
				hintItem.objActiveState = new bool[hintItem.objectsToHide.Length];
				for (int j = 0; j < hintItem.objectsToHide.Length; j++)
				{
					hintItem.objActiveState[j] = hintItem.objectsToHide[j].activeSelf;
					hintItem.objectsToHide[j].SetActive(false);
				}
			}
			if (hintItem.enableColliders)
			{
				hintItem.collidersObj.SetActive(true);
			}
			if (hintItem.indicateTarget)
			{
				if (hintItem.targetButton == null)
				{
					hintItem.targetButton = hintItem.target.GetComponent<UIButton>();
				}
				if (!string.IsNullOrEmpty(hintItem.indicatedSpriteName) && hintItem.targetButton != null)
				{
					hintItem.targetSprite = hintItem.targetButton.tweenTarget.GetComponent<UISprite>();
					hintItem.defaultSpriteName = hintItem.targetSprite.spriteName;
				}
				else
				{
					hintItem.targetSprites = hintItem.target.GetComponentsInChildren<UISprite>();
				}
			}
			this.hintObject.Show(hintItem);
			this.inShow = hintItem;
		}
	}

	// Token: 0x04000C68 RID: 3176
	public HintObject hintObject;

	// Token: 0x04000C69 RID: 3177
	public HintController.HintItem[] hints;

	// Token: 0x04000C6A RID: 3178
	private readonly List<HintController.HintItem> hintToShow = new List<HintController.HintItem>();

	// Token: 0x04000C6B RID: 3179
	private EventDelegate showNextEvent;

	// Token: 0x04000C6C RID: 3180
	private static readonly List<HintController> _instances = new List<HintController>();

	// Token: 0x04000C6D RID: 3181
	private HintController.HintItem inShow;

	// Token: 0x020002A2 RID: 674
	public enum HideReason
	{
		// Token: 0x04000C6F RID: 3183
		None,
		// Token: 0x04000C70 RID: 3184
		ButtonClick
	}

	// Token: 0x020002A3 RID: 675
	public enum ShowReason
	{
		// Token: 0x04000C72 RID: 3186
		New,
		// Token: 0x04000C73 RID: 3187
		PlayerDay,
		// Token: 0x04000C74 RID: 3188
		PlayerSession,
		// Token: 0x04000C75 RID: 3189
		level,
		// Token: 0x04000C76 RID: 3190
		TrainingStage,
		// Token: 0x04000C77 RID: 3191
		OpenByScript
	}

	// Token: 0x020002A4 RID: 676
	[Serializable]
	public class HintItem
	{
		// Token: 0x04000C78 RID: 3192
		public string name;

		// Token: 0x04000C79 RID: 3193
		public GameObject target;

		// Token: 0x04000C7A RID: 3194
		public string hintText;

		// Token: 0x04000C7B RID: 3195
		public Vector3 relativeHintPosition;

		// Token: 0x04000C7C RID: 3196
		public Vector3 relativeLabelPosition;

		// Token: 0x04000C7D RID: 3197
		public HintController.ShowReason showReason;

		// Token: 0x04000C7E RID: 3198
		public HintController.HideReason hideReason;

		// Token: 0x04000C7F RID: 3199
		public UIButton[] buttonsToBlock;

		// Token: 0x04000C80 RID: 3200
		public GameObject[] objectsToHide;

		// Token: 0x04000C81 RID: 3201
		[HideInInspector]
		public bool[] buttonsState;

		// Token: 0x04000C82 RID: 3202
		[HideInInspector]
		public bool[] objActiveState;

		// Token: 0x04000C83 RID: 3203
		[HideInInspector]
		public UIButton targetButton;

		// Token: 0x04000C84 RID: 3204
		[HideInInspector]
		public UISprite targetSprite;

		// Token: 0x04000C85 RID: 3205
		[HideInInspector]
		public UISprite[] targetSprites;

		// Token: 0x04000C86 RID: 3206
		public float timeout;

		// Token: 0x04000C87 RID: 3207
		public bool indicateTarget;

		// Token: 0x04000C88 RID: 3208
		public bool manualRotateArrow;

		// Token: 0x04000C89 RID: 3209
		public bool scaleTween;

		// Token: 0x04000C8A RID: 3210
		public bool showLabelByCode;

		// Token: 0x04000C8B RID: 3211
		public Vector3 manualArrowRotation;

		// Token: 0x04000C8C RID: 3212
		public string indicatedSpriteName;

		// Token: 0x04000C8D RID: 3213
		[HideInInspector]
		public string defaultSpriteName;

		// Token: 0x04000C8E RID: 3214
		public bool enableColliders;

		// Token: 0x04000C8F RID: 3215
		public GameObject collidersObj;

		// Token: 0x04000C90 RID: 3216
		public TrainingController.NewTrainingCompletedStage trainingStage;
	}
}
