using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class AnimationCoroutineRunner : MonoBehaviour
{
	// Token: 0x0600005D RID: 93 RVA: 0x000049C4 File Offset: 0x00002BC4
	public void StartPlay(Animation animation, string clipName, bool useTimeScale, Action onComplete)
	{
		base.StartCoroutine(this.Play(animation, clipName, useTimeScale, onComplete));
	}

	// Token: 0x0600005E RID: 94 RVA: 0x000049D8 File Offset: 0x00002BD8
	public IEnumerator Play(Animation animation, string clipName, bool useTimeScale, Action onComplete)
	{
		if (!useTimeScale)
		{
			AnimationState _currState = animation[clipName];
			bool isPlaying = true;
			float _progressTime = 0f;
			float _timeAtLastFrame = 0f;
			float _timeAtCurrentFrame = 0f;
			float deltaTime = 0f;
			animation.Play(clipName);
			_timeAtLastFrame = Time.realtimeSinceStartup;
			while (isPlaying)
			{
				try
				{
					_timeAtCurrentFrame = Time.realtimeSinceStartup;
					deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
					_timeAtLastFrame = _timeAtCurrentFrame;
					_progressTime += deltaTime;
					_currState.normalizedTime = _progressTime / _currState.length;
					animation.Sample();
					if (_progressTime >= _currState.length)
					{
						if (_currState.wrapMode != WrapMode.Loop)
						{
							isPlaying = false;
						}
						else
						{
							_progressTime = 0f;
						}
					}
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogErrorFormat("Exception in AnimationCoroutineRunner Play: {0}", new object[]
					{
						e
					});
				}
				yield return new WaitForEndOfFrame();
				if (this == null || base.gameObject == null)
				{
					yield break;
				}
			}
			yield return null;
			if (onComplete != null)
			{
				onComplete();
			}
		}
		else
		{
			animation.Play(clipName);
		}
		yield break;
	}
}
