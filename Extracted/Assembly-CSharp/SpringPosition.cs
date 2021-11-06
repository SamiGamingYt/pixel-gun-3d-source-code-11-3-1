using System;
using UnityEngine;

// Token: 0x02000383 RID: 899
[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : MonoBehaviour
{
	// Token: 0x06001F9F RID: 8095 RVA: 0x00091F20 File Offset: 0x00090120
	private void Start()
	{
		this.mTrans = base.transform;
		if (this.updateScrollView)
		{
			this.mSv = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x00091F58 File Offset: 0x00090158
	private void Update()
	{
		float deltaTime = (!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime;
		if (this.worldSpace)
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.position).sqrMagnitude * 0.001f;
			}
			this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.position).sqrMagnitude)
			{
				this.mTrans.position = this.target;
				this.NotifyListeners();
				base.enabled = false;
			}
		}
		else
		{
			if (this.mThreshold == 0f)
			{
				this.mThreshold = (this.target - this.mTrans.localPosition).sqrMagnitude * 1E-05f;
			}
			this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
			if (this.mThreshold >= (this.target - this.mTrans.localPosition).sqrMagnitude)
			{
				this.mTrans.localPosition = this.target;
				this.NotifyListeners();
				base.enabled = false;
			}
		}
		if (this.mSv != null)
		{
			this.mSv.UpdateScrollbars(true);
		}
	}

	// Token: 0x06001FA1 RID: 8097 RVA: 0x00092100 File Offset: 0x00090300
	private void NotifyListeners()
	{
		SpringPosition.current = this;
		if (this.onFinished != null)
		{
			this.onFinished();
		}
		if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
		{
			this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
		}
		SpringPosition.current = null;
	}

	// Token: 0x06001FA2 RID: 8098 RVA: 0x00092164 File Offset: 0x00090364
	public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPosition springPosition = go.GetComponent<SpringPosition>();
		if (springPosition == null)
		{
			springPosition = go.AddComponent<SpringPosition>();
		}
		springPosition.target = pos;
		springPosition.strength = strength;
		springPosition.onFinished = null;
		if (!springPosition.enabled)
		{
			springPosition.mThreshold = 0f;
			springPosition.enabled = true;
		}
		return springPosition;
	}

	// Token: 0x0400140B RID: 5131
	public static SpringPosition current;

	// Token: 0x0400140C RID: 5132
	public Vector3 target = Vector3.zero;

	// Token: 0x0400140D RID: 5133
	public float strength = 10f;

	// Token: 0x0400140E RID: 5134
	public bool worldSpace;

	// Token: 0x0400140F RID: 5135
	public bool ignoreTimeScale;

	// Token: 0x04001410 RID: 5136
	public bool updateScrollView;

	// Token: 0x04001411 RID: 5137
	public SpringPosition.OnFinished onFinished;

	// Token: 0x04001412 RID: 5138
	[SerializeField]
	[HideInInspector]
	private GameObject eventReceiver;

	// Token: 0x04001413 RID: 5139
	[SerializeField]
	[HideInInspector]
	public string callWhenFinished;

	// Token: 0x04001414 RID: 5140
	private Transform mTrans;

	// Token: 0x04001415 RID: 5141
	private float mThreshold;

	// Token: 0x04001416 RID: 5142
	private UIScrollView mSv;

	// Token: 0x02000903 RID: 2307
	// (Invoke) Token: 0x060050AC RID: 20652
	public delegate void OnFinished();
}
