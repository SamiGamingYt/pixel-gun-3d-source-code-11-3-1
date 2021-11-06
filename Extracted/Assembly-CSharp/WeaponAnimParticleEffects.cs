using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000786 RID: 1926
public class WeaponAnimParticleEffects : MonoBehaviour
{
	// Token: 0x17000B30 RID: 2864
	// (get) Token: 0x060043B6 RID: 17334 RVA: 0x0016A9C8 File Offset: 0x00168BC8
	private List<GameObject> _effectObjects
	{
		get
		{
			if (this._eo == null)
			{
				this._eo = new List<GameObject>();
				foreach (WeaponAnimEffectData weaponAnimEffectData in this.effects)
				{
					foreach (ParticleSystem particleSystem in weaponAnimEffectData.particleSystems)
					{
						this._eo.Add(particleSystem.gameObject);
					}
				}
			}
			return this._eo;
		}
	}

	// Token: 0x060043B7 RID: 17335 RVA: 0x0016AA48 File Offset: 0x00168C48
	private void Start()
	{
		if (!this._isInit)
		{
			foreach (WeaponAnimEffectData effectData in this.effects)
			{
				this.InitiAnimatonEventForEffect(effectData);
			}
			this._isInit = true;
		}
		this.ActivateDefaultEffect();
	}

	// Token: 0x060043B8 RID: 17336 RVA: 0x0016AA94 File Offset: 0x00168C94
	private void OnEnable()
	{
		this.ActivateDefaultEffect();
	}

	// Token: 0x060043B9 RID: 17337 RVA: 0x0016AA9C File Offset: 0x00168C9C
	public List<GameObject> GetListAnimEffects()
	{
		return this._effectObjects;
	}

	// Token: 0x060043BA RID: 17338 RVA: 0x0016AAA4 File Offset: 0x00168CA4
	private void InitiAnimatonEventForEffect(WeaponAnimEffectData effectData)
	{
		AnimationClip animationClip = base.GetComponent<Animation>().GetClip(effectData.animationName);
		if (animationClip == null)
		{
			return;
		}
		if (!animationClip.events.Any((AnimationEvent e) => e.stringParameter == effectData.animationName && e.functionName == "OnStartAnimEffects" && Math.Abs(e.time) < 0.001f))
		{
			AnimationEvent animationEvent = new AnimationEvent();
			animationEvent.stringParameter = effectData.animationName;
			animationEvent.functionName = "OnStartAnimEffects";
			animationEvent.time = 0f;
			animationClip.AddEvent(animationEvent);
		}
		if (!animationClip.events.Any((AnimationEvent e) => e.stringParameter == effectData.animationName && e.functionName == "OnAnimationFinished" && Math.Abs(e.time - animationClip.length) < 0.001f))
		{
			AnimationEvent animationEvent2 = new AnimationEvent();
			animationEvent2.stringParameter = effectData.animationName;
			animationEvent2.functionName = "OnAnimationFinished";
			animationEvent2.time = animationClip.length;
			animationClip.AddEvent(animationEvent2);
		}
		effectData.animationLength = ((!effectData.isLoop) ? animationClip.length : 0f);
	}

	// Token: 0x060043BB RID: 17339 RVA: 0x0016ABD8 File Offset: 0x00168DD8
	public void ActivateDefaultEffect()
	{
		WeaponAnimEffectData weaponAnimEffectData = this.effects.FirstOrDefault((WeaponAnimEffectData e) => e.animationName == "Default");
		if (weaponAnimEffectData != null)
		{
			this.SetActiveEffect(weaponAnimEffectData, true);
		}
	}

	// Token: 0x060043BC RID: 17340 RVA: 0x0016AC1C File Offset: 0x00168E1C
	public void DisableEffectForAnimation(string animName)
	{
		WeaponAnimEffectData effectData = this.GetEffectData(animName);
		if (effectData != null)
		{
			effectData.isPlaying = false;
			this.SetActiveEffect(effectData, false);
			this._currentEffect = null;
		}
	}

	// Token: 0x060043BD RID: 17341 RVA: 0x0016AC50 File Offset: 0x00168E50
	private void SetActiveEffect(WeaponAnimEffectData effectData, bool active)
	{
		if (effectData == null || effectData.particleSystems == null)
		{
			return;
		}
		if (active && effectData.blockAtPlay && effectData.isPlaying)
		{
			return;
		}
		foreach (ParticleSystem particleSystem in effectData.particleSystems)
		{
			if (active)
			{
				particleSystem.gameObject.SetActive(true);
				if (effectData.EmitCount < 0)
				{
					particleSystem.Play();
				}
				else
				{
					particleSystem.Emit(effectData.EmitCount);
				}
			}
			else if (effectData.EmitCount < 0)
			{
				particleSystem.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060043BE RID: 17342 RVA: 0x0016AD00 File Offset: 0x00168F00
	private WeaponAnimEffectData GetEffectData(string animationName)
	{
		if (this.effects == null)
		{
			return null;
		}
		int num = this.effects.Length;
		for (int i = 0; i < num; i++)
		{
			WeaponAnimEffectData weaponAnimEffectData = this.effects[i];
			if (weaponAnimEffectData != null)
			{
				if (weaponAnimEffectData.animationName == animationName)
				{
					return weaponAnimEffectData;
				}
			}
		}
		return null;
	}

	// Token: 0x060043BF RID: 17343 RVA: 0x0016AD60 File Offset: 0x00168F60
	private bool CheckSkipStartEffectForAnimation(string animationName)
	{
		if (this._currentEffect == null)
		{
			return false;
		}
		if (this._currentEffect.isLoop)
		{
			return this._lastStartedAnimationName == animationName;
		}
		WeaponAnimEffectData effectData = this.GetEffectData(animationName);
		if (effectData == null)
		{
			return false;
		}
		if (effectData != null && !effectData.isLoop)
		{
			base.CancelInvoke("ChangeEffectAfterStopAnimation");
			return false;
		}
		return !this._isCanStopNotLoopEffect;
	}

	// Token: 0x060043C0 RID: 17344 RVA: 0x0016ADD0 File Offset: 0x00168FD0
	private void OnStartAnimEffects(string animationName)
	{
		if (this.CheckSkipStartEffectForAnimation(animationName))
		{
			return;
		}
		this._lastStartedAnimationName = animationName;
		WeaponAnimEffectData effectData = this.GetEffectData(animationName);
		if (effectData == null)
		{
			return;
		}
		bool flag = false;
		if (this._currentEffect != null)
		{
			flag = (this._currentEffect.particleSystems.SequenceEqual(effectData.particleSystems) && this._currentEffect.isLoop && effectData.isLoop);
			if (!flag)
			{
				this.SetActiveEffect(this._currentEffect, false);
			}
		}
		this._currentEffect = effectData;
		if (effectData == null)
		{
			return;
		}
		if (!flag)
		{
			this.SetActiveEffect(effectData, true);
			effectData.isPlaying = true;
		}
		if (!effectData.isLoop)
		{
			this._isCanStopNotLoopEffect = false;
			base.Invoke("ChangeEffectAfterStopAnimation", effectData.animationLength);
		}
	}

	// Token: 0x060043C1 RID: 17345 RVA: 0x0016AEA0 File Offset: 0x001690A0
	private void OnAnimationFinished(string animationName)
	{
		WeaponAnimEffectData effectData = this.GetEffectData(animationName);
		this._lastFinishedAnimationName = animationName;
		if (effectData != null)
		{
			effectData.isPlaying = false;
		}
	}

	// Token: 0x060043C2 RID: 17346 RVA: 0x0016AECC File Offset: 0x001690CC
	private string GetNamePlayingAnimation()
	{
		if (base.GetComponent<Animation>() == null)
		{
			return string.Empty;
		}
		foreach (object obj in base.GetComponent<Animation>())
		{
			AnimationState animationState = (AnimationState)obj;
			if (base.GetComponent<Animation>().IsPlaying(animationState.name))
			{
				return animationState.name;
			}
		}
		return string.Empty;
	}

	// Token: 0x060043C3 RID: 17347 RVA: 0x0016AF74 File Offset: 0x00169174
	public void ChangeEffectAfterStopAnimation()
	{
		this._isCanStopNotLoopEffect = true;
		string namePlayingAnimation = this.GetNamePlayingAnimation();
		if (!string.IsNullOrEmpty(namePlayingAnimation))
		{
			this.OnStartAnimEffects(namePlayingAnimation);
		}
	}

	// Token: 0x0400316F RID: 12655
	public WeaponAnimEffectData[] effects;

	// Token: 0x04003170 RID: 12656
	private List<GameObject> _eo;

	// Token: 0x04003171 RID: 12657
	[ReadOnly]
	[SerializeField]
	private WeaponAnimEffectData _currentEffect;

	// Token: 0x04003172 RID: 12658
	[ReadOnly]
	[SerializeField]
	private string _lastStartedAnimationName;

	// Token: 0x04003173 RID: 12659
	[SerializeField]
	[ReadOnly]
	private string _lastFinishedAnimationName;

	// Token: 0x04003174 RID: 12660
	private bool _isInit;

	// Token: 0x04003175 RID: 12661
	private bool _isCanStopNotLoopEffect;
}
