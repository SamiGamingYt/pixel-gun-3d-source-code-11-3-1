using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006E8 RID: 1768
	public class AnimationHandler : MonoBehaviour
	{
		// Token: 0x14000074 RID: 116
		// (add) Token: 0x06003D96 RID: 15766 RVA: 0x0013F8A4 File Offset: 0x0013DAA4
		// (remove) Token: 0x06003D97 RID: 15767 RVA: 0x0013F8C0 File Offset: 0x0013DAC0
		public event Action<string, AnimationHandler.AnimState> OnAnimationEvent;

		// Token: 0x06003D98 RID: 15768 RVA: 0x0013F8DC File Offset: 0x0013DADC
		public void SubscribeTo(string animationName, AnimationHandler.AnimState toState, bool callOnce, Action callback)
		{
			this._subscribers.Add(new AnimationHandler.Subscriber
			{
				AnimationName = animationName,
				ToState = toState,
				CallOnce = callOnce,
				Callback = callback
			});
		}

		// Token: 0x06003D99 RID: 15769 RVA: 0x0013F918 File Offset: 0x0013DB18
		public void Unsubscribe(string animationName, AnimationHandler.AnimState toState, Action callback)
		{
			AnimationHandler.Subscriber[] array = (from s in this._subscribers
			where s.AnimationName == animationName && s.ToState == toState && s.Callback == callback
			select s).ToArray<AnimationHandler.Subscriber>();
			foreach (AnimationHandler.Subscriber item in array)
			{
				this._subscribers.Remove(item);
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x0013F98C File Offset: 0x0013DB8C
		public bool AnimationIsExists(string animName)
		{
			return !animName.IsNullOrEmpty() && this.clips.Any((AnimationClip c) => c.name == animName);
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x0013F9D0 File Offset: 0x0013DBD0
		private void Awake()
		{
			this.clips.Clear();
			Animation component = base.GetComponent<Animation>();
			if (component != null)
			{
				foreach (object obj in component)
				{
					AnimationState animationState = (AnimationState)obj;
					this.clips.Add(animationState.clip);
				}
			}
			else
			{
				Animator component2 = base.GetComponent<Animator>();
				if (component2 != null)
				{
					this.clips = component2.runtimeAnimatorController.animationClips.ToList<AnimationClip>();
				}
			}
			if (!this.clips.Any<AnimationClip>())
			{
				Debug.LogError("animations not found");
				return;
			}
			foreach (AnimationClip animationClip in this.clips)
			{
				bool flag = false;
				bool flag2 = false;
				foreach (AnimationEvent animationEvent in animationClip.events)
				{
					if (animationEvent.time == 0f && animationEvent.functionName == AnimationHandler.AnimState.Started.ToString())
					{
						flag = true;
					}
					if (animationEvent.time == 0f && animationEvent.functionName == AnimationHandler.AnimState.Finished.ToString())
					{
						flag2 = true;
					}
				}
				if (!flag)
				{
					AnimationEvent evt = new AnimationEvent
					{
						time = 0f,
						functionName = AnimationHandler.AnimState.Started.ToString(),
						stringParameter = animationClip.name
					};
					animationClip.AddEvent(evt);
				}
				if (!flag2)
				{
					AnimationEvent evt2 = new AnimationEvent
					{
						time = animationClip.length,
						functionName = AnimationHandler.AnimState.Finished.ToString(),
						stringParameter = animationClip.name
					};
					animationClip.AddEvent(evt2);
				}
			}
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x0013FC24 File Offset: 0x0013DE24
		private void Started(string animationName)
		{
			this.InvokeSubscribers(animationName, AnimationHandler.AnimState.Started);
			if (this.OnAnimationEvent != null)
			{
				this.OnAnimationEvent(animationName, AnimationHandler.AnimState.Started);
			}
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x0013FC54 File Offset: 0x0013DE54
		private void Finished(string animationName)
		{
			this.InvokeSubscribers(animationName, AnimationHandler.AnimState.Finished);
			if (this.OnAnimationEvent != null)
			{
				this.OnAnimationEvent(animationName, AnimationHandler.AnimState.Finished);
			}
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x0013FC84 File Offset: 0x0013DE84
		private void CustomCall(string animName_param)
		{
			string[] array = animName_param.Split(new char[]
			{
				'_'
			});
			if (array.Length != 2)
			{
				return;
			}
			AnimationHandler.AnimState? animState = array[1].ToEnum(null);
			if (animState == null)
			{
				return;
			}
			string text = array[0];
			this.InvokeSubscribers(text, animState.Value);
			if (this.OnAnimationEvent != null)
			{
				this.OnAnimationEvent(text, animState.Value);
			}
		}

		// Token: 0x06003D9F RID: 15775 RVA: 0x0013FCFC File Offset: 0x0013DEFC
		private void InvokeSubscribers(string animationName, AnimationHandler.AnimState state)
		{
			for (int i = this._subscribers.Count - 1; i >= 0; i--)
			{
				if (this._subscribers.Count - 1 >= i)
				{
					AnimationHandler.Subscriber subscriber = this._subscribers[i];
					if (subscriber.AnimationName == animationName && subscriber.ToState == state)
					{
						if (subscriber.Callback != null)
						{
							subscriber.Callback();
						}
						if (subscriber.CallOnce || subscriber.Callback == null)
						{
							this._toRemove.Add(subscriber);
						}
					}
					foreach (AnimationHandler.Subscriber item in this._toRemove)
					{
						this._subscribers.Remove(item);
					}
					this._toRemove.Clear();
				}
			}
		}

		// Token: 0x04002D65 RID: 11621
		private List<AnimationClip> clips = new List<AnimationClip>();

		// Token: 0x04002D66 RID: 11622
		private List<AnimationHandler.Subscriber> _subscribers = new List<AnimationHandler.Subscriber>();

		// Token: 0x04002D67 RID: 11623
		private List<AnimationHandler.Subscriber> _toRemove = new List<AnimationHandler.Subscriber>();

		// Token: 0x020006E9 RID: 1769
		public enum AnimState
		{
			// Token: 0x04002D6A RID: 11626
			Started,
			// Token: 0x04002D6B RID: 11627
			Finished,
			// Token: 0x04002D6C RID: 11628
			Custom1
		}

		// Token: 0x020006EA RID: 1770
		private class Subscriber
		{
			// Token: 0x04002D6D RID: 11629
			public string AnimationName;

			// Token: 0x04002D6E RID: 11630
			public AnimationHandler.AnimState ToState;

			// Token: 0x04002D6F RID: 11631
			public bool CallOnce;

			// Token: 0x04002D70 RID: 11632
			public Action Callback;
		}
	}
}
