using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000F RID: 15
public sealed class AdvancedEffects : MonoBehaviour
{
	// Token: 0x06000038 RID: 56 RVA: 0x00004060 File Offset: 0x00002260
	private void Awake()
	{
		this._photonView = base.GetComponent<PhotonView>();
		if (this.syncInLocal)
		{
			this._networkView = base.GetComponent<NetworkView>();
		}
		this.isMine = (!Defs.isMulti || this._photonView == null || ((!Defs.isInet) ? (this.syncInLocal && this._networkView.isMine) : this._photonView.isMine));
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000040EC File Offset: 0x000022EC
	public void SendAdvancedEffect(int effectIndex, float effectTime)
	{
		if (Defs.isMulti)
		{
			if (Defs.isInet)
			{
				this._photonView.RPC("AdvancedEffectRPC", PhotonTargets.Others, new object[]
				{
					effectIndex,
					effectTime
				});
			}
			else if (this.syncInLocal)
			{
				this._networkView.RPC("AdvancedEffectRPC", RPCMode.Others, new object[]
				{
					effectIndex,
					effectTime
				});
			}
		}
		this.AdvancedEffectRPC(effectIndex, effectTime);
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00004178 File Offset: 0x00002378
	[PunRPC]
	[RPC]
	public void AdvancedEffectRPC(int effectIndex, float effectTime)
	{
		for (int i = 0; i < this.playerEffects.Count; i++)
		{
			if (this.playerEffects[i].effect == (AdvancedEffects.AdvancedEffect)effectIndex)
			{
				this.playerEffects[i] = this.playerEffects[i].UpdateTime(effectTime);
				return;
			}
		}
		this.playerEffects.Add(new AdvancedEffects.ActiveAdvancedEffect((AdvancedEffects.AdvancedEffect)effectIndex, effectTime));
		this.ActivateAdvancedEffect((AdvancedEffects.AdvancedEffect)effectIndex);
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000041F8 File Offset: 0x000023F8
	private float GetCenterPosition()
	{
		if (base.transform.childCount > 0)
		{
			BoxCollider component = base.transform.GetChild(0).GetComponent<BoxCollider>();
			if (component != null)
			{
				return component.center.y;
			}
		}
		return 0f;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00004248 File Offset: 0x00002448
	private void ActivateAdvancedEffect(AdvancedEffects.AdvancedEffect effect)
	{
		if (effect == AdvancedEffects.AdvancedEffect.burning)
		{
			this.burningEffect = ParticleStacks.instance.fireStack.GetParticle();
			if (this.burningEffect != null)
			{
				this.burningEffect.transform.SetParent(base.transform, false);
				this.burningEffect.transform.localPosition = Vector3.up * this.GetCenterPosition();
			}
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000042C8 File Offset: 0x000024C8
	private void DeactivateAdvancedEffect(AdvancedEffects.AdvancedEffect effect)
	{
		if (effect == AdvancedEffects.AdvancedEffect.burning)
		{
			if (this.burningEffect != null && ParticleStacks.instance != null)
			{
				this.burningEffect.transform.parent = null;
				ParticleStacks.instance.fireStack.ReturnParticle(this.burningEffect);
				this.burningEffect = null;
			}
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00004338 File Offset: 0x00002538
	private void Update()
	{
		for (int i = 0; i < this.playerEffects.Count; i++)
		{
			if (this.playerEffects[i].lifeTime < Time.time)
			{
				this.DeactivateAdvancedEffect(this.playerEffects[i].effect);
				this.playerEffects.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x000043AC File Offset: 0x000025AC
	private void OnDestroy()
	{
		for (int i = 0; i < this.playerEffects.Count; i++)
		{
			this.DeactivateAdvancedEffect(this.playerEffects[i].effect);
			this.playerEffects.RemoveAt(i);
			i--;
		}
	}

	// Token: 0x04000049 RID: 73
	public bool syncInLocal;

	// Token: 0x0400004A RID: 74
	private bool isMine;

	// Token: 0x0400004B RID: 75
	private PhotonView _photonView;

	// Token: 0x0400004C RID: 76
	private NetworkView _networkView;

	// Token: 0x0400004D RID: 77
	private List<AdvancedEffects.ActiveAdvancedEffect> playerEffects = new List<AdvancedEffects.ActiveAdvancedEffect>(3);

	// Token: 0x0400004E RID: 78
	private GameObject burningEffect;

	// Token: 0x0400004F RID: 79
	private GameObject bleedingEffect;

	// Token: 0x02000010 RID: 16
	public enum AdvancedEffect
	{
		// Token: 0x04000051 RID: 81
		none,
		// Token: 0x04000052 RID: 82
		burning
	}

	// Token: 0x02000011 RID: 17
	public struct ActiveAdvancedEffect
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00004400 File Offset: 0x00002600
		public ActiveAdvancedEffect(AdvancedEffects.AdvancedEffect effect, float time)
		{
			this.effect = effect;
			this.lifeTime = Time.time + time;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004418 File Offset: 0x00002618
		public AdvancedEffects.ActiveAdvancedEffect UpdateTime(float time)
		{
			return new AdvancedEffects.ActiveAdvancedEffect(this.effect, time);
		}

		// Token: 0x04000053 RID: 83
		public AdvancedEffects.AdvancedEffect effect;

		// Token: 0x04000054 RID: 84
		public float lifeTime;
	}
}
