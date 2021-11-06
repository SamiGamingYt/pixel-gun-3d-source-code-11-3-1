using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

// Token: 0x0200038F RID: 911
public abstract class UITweener : MonoBehaviour
{
	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x0600201C RID: 8220 RVA: 0x00093620 File Offset: 0x00091820
	public float amountPerDelta
	{
		get
		{
			if (this.mDuration != this.duration)
			{
				this.mDuration = this.duration;
				this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f) ? 1000f : (1f / this.duration)) * Mathf.Sign(this.mAmountPerDelta);
			}
			return this.mAmountPerDelta;
		}
	}

	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x0600201D RID: 8221 RVA: 0x00093690 File Offset: 0x00091890
	// (set) Token: 0x0600201E RID: 8222 RVA: 0x00093698 File Offset: 0x00091898
	public float tweenFactor
	{
		get
		{
			return this.mFactor;
		}
		set
		{
			this.mFactor = Mathf.Clamp01(value);
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x0600201F RID: 8223 RVA: 0x000936A8 File Offset: 0x000918A8
	public Direction direction
	{
		get
		{
			return (this.amountPerDelta >= 0f) ? Direction.Forward : Direction.Reverse;
		}
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x000936C4 File Offset: 0x000918C4
	private void Reset()
	{
		if (!this.mStarted)
		{
			this.SetStartToCurrentValue();
			this.SetEndToCurrentValue();
		}
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x000936E0 File Offset: 0x000918E0
	protected virtual void Start()
	{
		this.Update();
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000936E8 File Offset: 0x000918E8
	private void Update()
	{
		float num = (!this.ignoreTimeScale) ? Time.deltaTime : RealTime.deltaTime;
		float num2 = (!this.ignoreTimeScale) ? Time.time : RealTime.time;
		if (!this.mStarted)
		{
			this.mStarted = true;
			this.mStartTime = num2 + this.delay;
		}
		if (num2 < this.mStartTime)
		{
			return;
		}
		this.mFactor += this.amountPerDelta * num;
		if (this.style == UITweener.Style.Loop)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor -= Mathf.Floor(this.mFactor);
			}
		}
		else if (this.style == UITweener.Style.PingPong)
		{
			if (this.mFactor > 1f)
			{
				this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
			else if (this.mFactor < 0f)
			{
				this.mFactor = -this.mFactor;
				this.mFactor -= Mathf.Floor(this.mFactor);
				this.mAmountPerDelta = -this.mAmountPerDelta;
			}
		}
		if (this.style == UITweener.Style.Once && (this.duration == 0f || this.mFactor > 1f || this.mFactor < 0f))
		{
			this.mFactor = Mathf.Clamp01(this.mFactor);
			this.Sample(this.mFactor, true);
			base.enabled = false;
			if (UITweener.current != this)
			{
				UITweener uitweener = UITweener.current;
				UITweener.current = this;
				if (this.onFinished != null)
				{
					this.mTemp = this.onFinished;
					this.onFinished = new List<EventDelegate>();
					EventDelegate.Execute(this.mTemp);
					for (int i = 0; i < this.mTemp.Count; i++)
					{
						EventDelegate eventDelegate = this.mTemp[i];
						if (eventDelegate != null && !eventDelegate.oneShot)
						{
							EventDelegate.Add(this.onFinished, eventDelegate, eventDelegate.oneShot);
						}
					}
					this.mTemp = null;
				}
				if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
				{
					this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
				}
				UITweener.current = uitweener;
			}
		}
		else
		{
			this.Sample(this.mFactor, false);
		}
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x00093984 File Offset: 0x00091B84
	public void SetOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x00093994 File Offset: 0x00091B94
	public void SetOnFinished(EventDelegate del)
	{
		EventDelegate.Set(this.onFinished, del);
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x000939A4 File Offset: 0x00091BA4
	public void AddOnFinished(EventDelegate.Callback del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	// Token: 0x06002026 RID: 8230 RVA: 0x000939B4 File Offset: 0x00091BB4
	public void AddOnFinished(EventDelegate del)
	{
		EventDelegate.Add(this.onFinished, del);
	}

	// Token: 0x06002027 RID: 8231 RVA: 0x000939C4 File Offset: 0x00091BC4
	public void RemoveOnFinished(EventDelegate del)
	{
		if (this.onFinished != null)
		{
			this.onFinished.Remove(del);
		}
		if (this.mTemp != null)
		{
			this.mTemp.Remove(del);
		}
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x00093A04 File Offset: 0x00091C04
	private void OnDisable()
	{
		this.mStarted = false;
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x00093A10 File Offset: 0x00091C10
	public void Sample(float factor, bool isFinished)
	{
		float num = Mathf.Clamp01(factor);
		if (this.method == UITweener.Method.EaseIn)
		{
			num = 1f - Mathf.Sin(1.5707964f * (1f - num));
			if (this.steeperCurves)
			{
				num *= num;
			}
		}
		else if (this.method == UITweener.Method.EaseOut)
		{
			num = Mathf.Sin(1.5707964f * num);
			if (this.steeperCurves)
			{
				num = 1f - num;
				num = 1f - num * num;
			}
		}
		else if (this.method == UITweener.Method.EaseInOut)
		{
			num -= Mathf.Sin(num * 6.2831855f) / 6.2831855f;
			if (this.steeperCurves)
			{
				num = num * 2f - 1f;
				float num2 = Mathf.Sign(num);
				num = 1f - Mathf.Abs(num);
				num = 1f - num * num;
				num = num2 * num * 0.5f + 0.5f;
			}
		}
		else if (this.method == UITweener.Method.BounceIn)
		{
			num = this.BounceLogic(num);
		}
		else if (this.method == UITweener.Method.BounceOut)
		{
			num = 1f - this.BounceLogic(1f - num);
		}
		this.OnUpdate((this.animationCurve == null) ? num : this.animationCurve.Evaluate(num), isFinished);
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x00093B64 File Offset: 0x00091D64
	private float BounceLogic(float val)
	{
		if (val < 0.363636f)
		{
			val = 7.5685f * val * val;
		}
		else if (val < 0.727272f)
		{
			val = 7.5625f * (val -= 0.545454f) * val + 0.75f;
		}
		else if (val < 0.90909f)
		{
			val = 7.5625f * (val -= 0.818181f) * val + 0.9375f;
		}
		else
		{
			val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f;
		}
		return val;
	}

	// Token: 0x0600202B RID: 8235 RVA: 0x00093BFC File Offset: 0x00091DFC
	[Obsolete("Use PlayForward() instead")]
	public void Play()
	{
		this.Play(true);
	}

	// Token: 0x0600202C RID: 8236 RVA: 0x00093C08 File Offset: 0x00091E08
	public void PlayForward()
	{
		this.Play(true);
	}

	// Token: 0x0600202D RID: 8237 RVA: 0x00093C14 File Offset: 0x00091E14
	public void PlayReverse()
	{
		this.Play(false);
	}

	// Token: 0x0600202E RID: 8238 RVA: 0x00093C20 File Offset: 0x00091E20
	public void Play(bool forward)
	{
		this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		if (!forward)
		{
			this.mAmountPerDelta = -this.mAmountPerDelta;
		}
		base.enabled = true;
		this.Update();
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x00093C54 File Offset: 0x00091E54
	public void ResetToBeginning()
	{
		this.mStarted = false;
		this.mFactor = 0f;
		this.Sample(this.mFactor, false);
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x00093C78 File Offset: 0x00091E78
	public void Toggle()
	{
		if (this.mFactor > 0f)
		{
			this.mAmountPerDelta = -this.amountPerDelta;
		}
		else
		{
			this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
		}
		base.enabled = true;
	}

	// Token: 0x06002031 RID: 8241
	protected abstract void OnUpdate(float factor, bool isFinished);

	// Token: 0x06002032 RID: 8242 RVA: 0x00093CC0 File Offset: 0x00091EC0
	public static T Begin<T>(GameObject go, float duration) where T : UITweener
	{
		T t = go.GetComponent<T>();
		if (t != null && t.tweenGroup != 0)
		{
			t = (T)((object)null);
			T[] components = go.GetComponents<T>();
			int i = 0;
			int num = components.Length;
			while (i < num)
			{
				t = components[i];
				if (t != null && t.tweenGroup == 0)
				{
					break;
				}
				t = (T)((object)null);
				i++;
			}
		}
		if (t == null)
		{
			t = go.AddComponent<T>();
			if (t == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Unable to add ",
					typeof(T),
					" to ",
					NGUITools.GetHierarchy(go)
				}), go);
				return (T)((object)null);
			}
		}
		t.mStarted = false;
		t.duration = duration;
		t.mFactor = 0f;
		t.mAmountPerDelta = Mathf.Abs(t.amountPerDelta);
		t.style = UITweener.Style.Once;
		t.animationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 1f),
			new Keyframe(1f, 1f, 1f, 0f)
		});
		t.eventReceiver = null;
		t.callWhenFinished = null;
		t.enabled = true;
		return t;
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x00093E90 File Offset: 0x00092090
	public virtual void SetStartToCurrentValue()
	{
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x00093E94 File Offset: 0x00092094
	public virtual void SetEndToCurrentValue()
	{
	}

	// Token: 0x0400144C RID: 5196
	public static UITweener current;

	// Token: 0x0400144D RID: 5197
	[HideInInspector]
	public UITweener.Method method;

	// Token: 0x0400144E RID: 5198
	[HideInInspector]
	public UITweener.Style style;

	// Token: 0x0400144F RID: 5199
	[HideInInspector]
	public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, 0f, 1f),
		new Keyframe(1f, 1f, 1f, 0f)
	});

	// Token: 0x04001450 RID: 5200
	[HideInInspector]
	public bool ignoreTimeScale = true;

	// Token: 0x04001451 RID: 5201
	[HideInInspector]
	public float delay;

	// Token: 0x04001452 RID: 5202
	[HideInInspector]
	public float duration = 1f;

	// Token: 0x04001453 RID: 5203
	[HideInInspector]
	public bool steeperCurves;

	// Token: 0x04001454 RID: 5204
	[HideInInspector]
	public int tweenGroup;

	// Token: 0x04001455 RID: 5205
	[HideInInspector]
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	// Token: 0x04001456 RID: 5206
	[HideInInspector]
	public GameObject eventReceiver;

	// Token: 0x04001457 RID: 5207
	[HideInInspector]
	public string callWhenFinished;

	// Token: 0x04001458 RID: 5208
	private bool mStarted;

	// Token: 0x04001459 RID: 5209
	private float mStartTime;

	// Token: 0x0400145A RID: 5210
	private float mDuration;

	// Token: 0x0400145B RID: 5211
	private float mAmountPerDelta = 1000f;

	// Token: 0x0400145C RID: 5212
	private float mFactor;

	// Token: 0x0400145D RID: 5213
	private List<EventDelegate> mTemp;

	// Token: 0x02000390 RID: 912
	public enum Method
	{
		// Token: 0x0400145F RID: 5215
		Linear,
		// Token: 0x04001460 RID: 5216
		EaseIn,
		// Token: 0x04001461 RID: 5217
		EaseOut,
		// Token: 0x04001462 RID: 5218
		EaseInOut,
		// Token: 0x04001463 RID: 5219
		BounceIn,
		// Token: 0x04001464 RID: 5220
		BounceOut
	}

	// Token: 0x02000391 RID: 913
	public enum Style
	{
		// Token: 0x04001466 RID: 5222
		Once,
		// Token: 0x04001467 RID: 5223
		Loop,
		// Token: 0x04001468 RID: 5224
		PingPong
	}
}
