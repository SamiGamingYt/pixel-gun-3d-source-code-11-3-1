using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using UnityEngine;

// Token: 0x020007E5 RID: 2021
public class CharacterViewRotator : MonoBehaviour
{
	// Token: 0x06004927 RID: 18727 RVA: 0x00196D48 File Offset: 0x00194F48
	public void SetDefaultRotationFromCharacterView()
	{
		this._defaultLocalRotation = this.characterView.localRotation;
	}

	// Token: 0x06004928 RID: 18728 RVA: 0x00196D5C File Offset: 0x00194F5C
	private void Awake()
	{
		if (this.characterView != null)
		{
			this.SetDefaultRotationFromCharacterView();
		}
	}

	// Token: 0x06004929 RID: 18729 RVA: 0x00196D78 File Offset: 0x00194F78
	private void Start()
	{
		if (this.characterView == null)
		{
			return;
		}
		this.ReturnCharacterToDefaultOrientation();
	}

	// Token: 0x0600492A RID: 18730 RVA: 0x00196D94 File Offset: 0x00194F94
	private void OnEnable()
	{
		if (this.characterView == null)
		{
			return;
		}
		this.ReturnCharacterToDefaultOrientation();
	}

	// Token: 0x0600492B RID: 18731 RVA: 0x00196DB0 File Offset: 0x00194FB0
	private void Update()
	{
		if (this.characterView == null)
		{
			return;
		}
		if (Time.realtimeSinceStartup > this._toDefaultOrientationTime)
		{
			this.ReturnCharacterToDefaultOrientation();
		}
	}

	// Token: 0x0600492C RID: 18732 RVA: 0x00196DE8 File Offset: 0x00194FE8
	private void OnDragStart()
	{
		if (this.characterView == null)
		{
			return;
		}
		this._lastRotateTime = Time.realtimeSinceStartup;
	}

	// Token: 0x0600492D RID: 18733 RVA: 0x00196E08 File Offset: 0x00195008
	private void OnDrag(Vector2 delta)
	{
		if (this.characterView == null)
		{
			return;
		}
		if (HOTween.IsTweening(this.characterView))
		{
			return;
		}
		this.RefreshToDefaultOrientationTime();
		float num = -30f;
		this.characterView.Rotate(Vector3.up, delta.x * num * (Time.realtimeSinceStartup - this._lastRotateTime));
		this._lastRotateTime = Time.realtimeSinceStartup;
	}

	// Token: 0x0600492E RID: 18734 RVA: 0x00196E78 File Offset: 0x00195078
	private void OnScroll(float delta)
	{
		this.OnDrag(new Vector2(-delta * 20f, 0f));
	}

	// Token: 0x0600492F RID: 18735 RVA: 0x00196E94 File Offset: 0x00195094
	private void RefreshToDefaultOrientationTime()
	{
		this._toDefaultOrientationTime = Time.realtimeSinceStartup + ShopNGUIController.IdleTimeoutPers;
	}

	// Token: 0x06004930 RID: 18736 RVA: 0x00196EA8 File Offset: 0x001950A8
	private void ReturnCharacterToDefaultOrientation()
	{
		if (this.characterView == null)
		{
			return;
		}
		foreach (Tweener p_tweener in this.m_currentReturnTweeners)
		{
			HOTween.Kill(p_tweener);
		}
		this.m_currentReturnTweeners.Clear();
		this.RefreshToDefaultOrientationTime();
		TweenParms p_parms = new TweenParms().Prop("localRotation", new PlugQuaternion(this._defaultLocalRotation)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear).OnComplete(delegate()
		{
			this.RefreshToDefaultOrientationTime();
		});
		Tweener item = HOTween.To(this.characterView, 0.5f, p_parms);
		this.m_currentReturnTweeners.Add(item);
	}

	// Token: 0x0400365D RID: 13917
	public Transform characterView;

	// Token: 0x0400365E RID: 13918
	private Quaternion _defaultLocalRotation;

	// Token: 0x0400365F RID: 13919
	private float _toDefaultOrientationTime;

	// Token: 0x04003660 RID: 13920
	private float _lastRotateTime;

	// Token: 0x04003661 RID: 13921
	private List<Tweener> m_currentReturnTweeners = new List<Tweener>();
}
