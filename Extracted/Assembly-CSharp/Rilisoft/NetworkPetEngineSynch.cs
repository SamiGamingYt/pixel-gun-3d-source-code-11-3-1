using System;
using Photon;
using UnityEngine;
using UnityEngine.Events;

namespace Rilisoft
{
	// Token: 0x020006ED RID: 1773
	public sealed class NetworkPetEngineSynch : Photon.MonoBehaviour
	{
		// Token: 0x06003DA8 RID: 15784 RVA: 0x0013FE48 File Offset: 0x0013E048
		private void Awake()
		{
			if (!Defs.isMulti)
			{
				base.enabled = false;
			}
			this.thisTransform = base.transform;
			this.correctPlayerPos = new Vector3(0f, -10000f, 0f);
			this.movementHistory = new NetworkPetEngineSynch.MovementHistoryEntry[this.historyLengh];
			for (int i = 0; i < this.historyLengh; i++)
			{
				this.movementHistory[i].timeStamp = 0.0;
			}
			this.myTime = 1.0;
			this.engine = base.GetComponent<PetEngine>();
			this.engine.OnStateChanged += this.Engine_OnStateChanged;
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x0013FF00 File Offset: 0x0013E100
		private void Start()
		{
			if ((Defs.isInet && base.photonView.isMine) || (!Defs.isInet && base.GetComponent<NetworkView>().isMine))
			{
				this.isMine = true;
			}
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x0013FF48 File Offset: 0x0013E148
		private void Engine_OnStateChanged(PetState currentState, PetState prevState)
		{
			if (currentState == PetState.death)
			{
				this.Call_IsVisible(false, true);
				return;
			}
			if (currentState == PetState.teleport)
			{
				this.Call_IsVisible(false, false);
			}
			if (prevState == PetState.teleport)
			{
				this.Call_IsVisible(true, true);
			}
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x0013FF84 File Offset: 0x0013E184
		private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				stream.SendNext(this.thisTransform.position);
				stream.SendNext(this.thisTransform.rotation.eulerAngles.y);
				stream.SendNext(PhotonNetwork.time);
				stream.SendNext((int)this.currentAnimation);
				stream.SendNext(this.isTeleported);
				this.isTeleported = false;
			}
			else if (!this.isFirstSnapshot)
			{
				this.correctPlayerPos = (Vector3)stream.ReceiveNext();
				this.correctPlayerRot = Quaternion.Euler(0f, (float)stream.ReceiveNext(), 0f);
				this.correctPlayerTime = (double)stream.ReceiveNext();
				int anim = (int)stream.ReceiveNext();
				this.isTeleported = (bool)stream.ReceiveNext();
				if (this.isTeleported || Mathf.Abs((float)this.myTime - (float)this.correctPlayerTime) > 1000f)
				{
					this.isHistoryClear = true;
					this.myTime = this.correctPlayerTime;
				}
				if (!this.isHided)
				{
					this.AddNewSnapshot(this.correctPlayerPos, this.correctPlayerRot, this.correctPlayerTime, anim, this.isTeleported);
				}
			}
			else
			{
				this.isFirstSnapshot = false;
			}
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x001400F4 File Offset: 0x0013E2F4
		private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
		{
			if (stream.isWriting)
			{
				Vector3 position = this.thisTransform.position;
				Quaternion rotation = this.thisTransform.rotation;
				stream.Serialize(ref position);
				stream.Serialize(ref rotation);
				float num = (float)Network.time;
				stream.Serialize(ref num);
				int num2 = (int)this.currentAnimation;
				stream.Serialize(ref num2);
				stream.Serialize(ref this.isTeleported);
				this.isTeleported = false;
			}
			else
			{
				Vector3 zero = Vector3.zero;
				Quaternion identity = Quaternion.identity;
				float num3 = 0f;
				stream.Serialize(ref zero);
				stream.Serialize(ref identity);
				this.correctPlayerPos = zero;
				this.correctPlayerRot = identity;
				stream.Serialize(ref num3);
				this.correctPlayerTime = (double)num3;
				int anim = 0;
				stream.Serialize(ref anim);
				stream.Serialize(ref this.isTeleported);
				if (this.isTeleported)
				{
					this.isHistoryClear = true;
					this.myTime = this.correctPlayerTime;
				}
				if (!this.isHided)
				{
					this.AddNewSnapshot(this.correctPlayerPos, this.correctPlayerRot, this.correctPlayerTime, anim, this.isTeleported);
				}
			}
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x00140210 File Offset: 0x0013E410
		private void Update()
		{
			if (!this.isMine)
			{
				if (!this.engine.IsAlive || (this.engine.Owner != null && this.engine.Owner.isKilled))
				{
					this.engine.IsImmortal = true;
				}
				if (this.isHided || !this.engine.IsAlive)
				{
					return;
				}
				if (!this.isHistoryClear)
				{
					double num;
					if (this.myTime + (double)Time.deltaTime < this.movementHistory[this.movementHistory.Length - 1].timeStamp)
					{
						num = this.myTime + (double)(Time.deltaTime * 1.5f);
					}
					else
					{
						num = this.myTime + (double)Time.deltaTime;
					}
					int num2 = 0;
					for (int i = 0; i < this.movementHistory.Length; i++)
					{
						if (this.movementHistory[i].timeStamp <= this.myTime)
						{
							break;
						}
						num2 = i;
					}
					if (num2 == 0)
					{
						this.isHistoryClear = true;
					}
					if ((this.movementHistory[num2].timeStamp - this.myTime > 4.0 && num2 > 0) || this.movementHistory[num2].teleported)
					{
						num2--;
						this.thisTransform.position = this.movementHistory[num2].playerPos;
						this.thisTransform.rotation = this.movementHistory[num2].playerRot;
						this.myTime = this.movementHistory[num2].timeStamp;
						this.engine.ThisTransform.position = this.engine.MovePosition;
						this.engine.ShowModel();
						this.engine.PlayShowEffect();
					}
					else
					{
						float t = (float)((num - this.myTime) / (this.movementHistory[num2].timeStamp - this.myTime));
						this.thisTransform.position = Vector3.Lerp(this.thisTransform.position, this.movementHistory[num2].playerPos, t);
						if (!Device.isPixelGunLow)
						{
							this.thisTransform.rotation = Quaternion.Lerp(this.thisTransform.rotation, this.movementHistory[num2].playerRot, t);
						}
						else
						{
							this.thisTransform.rotation = this.movementHistory[num2].playerRot;
						}
						this.myTime = num;
						PetAnimationType anim = this.movementHistory[num2].anim;
						if (anim != PetAnimationType.Attack)
						{
							if (anim != PetAnimationType.Dead)
							{
								this.engine.SetMovementAnimation();
								this.engine.SetMovementAnimSpeed();
							}
							else
							{
								this.engine.PlayAnimation(this.movementHistory[num2].anim);
								this.engine.PlaySound(this.engine.ClipDeath);
							}
						}
						else
						{
							this.engine.PlayAnimation(this.movementHistory[num2].anim);
							this.engine.PlaySound(this.engine.ClipAttack);
						}
						if (this.movementHistory[num2].teleported)
						{
							this.engine.ShowModel();
							this.engine.PlayShowEffect();
						}
					}
				}
				else if (!this.isHistoryClear || this.movementHistory[this.movementHistory.Length - 1].teleported)
				{
					this.thisTransform.position = this.movementHistory[this.movementHistory.Length - 1].playerPos;
					this.thisTransform.rotation = this.movementHistory[this.movementHistory.Length - 1].playerRot;
					this.myTime = this.movementHistory[this.movementHistory.Length - 1].timeStamp;
					this.engine.ShowModel();
					this.engine.PlayShowEffect();
				}
				if (this.engine.Owner != null && this.engine.Owner.isKilled)
				{
					if (!this._effectPlayed)
					{
						this._effectPlayed = true;
						this.engine.PlayShowEffect();
					}
					base.transform.position = Vector3.down * 10000f;
					return;
				}
				this._effectPlayed = false;
				if (this.engine.IsImmortal)
				{
					this.engine.BlinkImmortal();
				}
			}
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x001406C8 File Offset: 0x0013E8C8
		private void AddNewSnapshot(Vector3 playerPos, Quaternion playerRot, double timeStamp, int _anim, bool teleported)
		{
			for (int i = this.movementHistory.Length - 1; i > 0; i--)
			{
				this.movementHistory[i] = this.movementHistory[i - 1];
			}
			this.movementHistory[0].playerPos = playerPos;
			this.movementHistory[0].playerRot = playerRot;
			this.movementHistory[0].timeStamp = timeStamp;
			this.movementHistory[0].anim = (PetAnimationType)_anim;
			this.movementHistory[0].teleported = teleported;
			if (this.isHistoryClear && this.movementHistory[this.movementHistory.Length - 1].timeStamp > this.myTime)
			{
				this.isHistoryClear = false;
				this.myTime = this.movementHistory[this.movementHistory.Length - 1].timeStamp;
				if (!this.isFirstHistoryFull)
				{
					this.thisTransform.position = this.movementHistory[this.movementHistory.Length - 1].playerPos;
					this.thisTransform.rotation = this.movementHistory[this.movementHistory.Length - 1].playerRot;
					this.isFirstHistoryFull = true;
				}
			}
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x00140824 File Offset: 0x0013EA24
		private void Call_IsVisible(bool state, bool showHideEffect)
		{
			if (Defs.isMulti && this.isMine)
			{
				if (!state)
				{
					this.isHistoryClear = true;
				}
				else
				{
					this.isHistoryClear = true;
					this.isTeleported = true;
				}
				if (Defs.isInet)
				{
					base.photonView.RPC("IsVisible_RPC", PhotonTargets.Others, new object[]
					{
						state,
						showHideEffect
					});
				}
				else
				{
					base.GetComponent<NetworkView>().RPC("IsVisible_RPC", RPCMode.Others, new object[]
					{
						state,
						showHideEffect
					});
				}
			}
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x001408C8 File Offset: 0x0013EAC8
		[PunRPC]
		[RPC]
		private void IsVisible_RPC(bool state, bool showHideEffect)
		{
			if (this.isMine)
			{
				return;
			}
			if (!state)
			{
				this.isHided = true;
				if (showHideEffect)
				{
					this.engine.Animator.Play(this.engine.GetAnimationName(PetAnimationType.Dead));
					this.engine.PlaySound(this.engine.ClipDeath);
					this.engine.AnimationHandler.SubscribeTo(this.engine.GetAnimationName(PetAnimationType.Dead), AnimationHandler.AnimState.Finished, true, delegate
					{
						this.engine.HideModel();
						this.engine.Animator.Play(this.engine.GetAnimationName(PetAnimationType.Idle));
						this.engine.PlayHideEffect();
						this.engine.EffectHide.OnEffectCompleted.AddListener(new UnityAction(this.OnEffectHideCompleted));
					});
				}
				else
				{
					this.engine.PlayShowEffect();
					base.transform.position = Vector3.down * 10000f;
				}
			}
			else
			{
				this.isHided = false;
			}
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x00140988 File Offset: 0x0013EB88
		private void OnEffectHideCompleted()
		{
			this.engine.EffectHide.OnEffectCompleted.RemoveListener(new UnityAction(this.OnEffectHideCompleted));
			base.transform.position = Vector3.down * 10000f;
		}

		// Token: 0x04002D71 RID: 11633
		private NetworkPetEngineSynch.MovementHistoryEntry[] movementHistory;

		// Token: 0x04002D72 RID: 11634
		private Vector3 correctPlayerPos;

		// Token: 0x04002D73 RID: 11635
		private double correctPlayerTime;

		// Token: 0x04002D74 RID: 11636
		private Quaternion correctPlayerRot = Quaternion.identity;

		// Token: 0x04002D75 RID: 11637
		private Transform thisTransform;

		// Token: 0x04002D76 RID: 11638
		private double myTime;

		// Token: 0x04002D77 RID: 11639
		private int historyLengh = 8;

		// Token: 0x04002D78 RID: 11640
		private bool isHistoryClear = true;

		// Token: 0x04002D79 RID: 11641
		public PetAnimationType currentAnimation;

		// Token: 0x04002D7A RID: 11642
		private int myAnimOld;

		// Token: 0x04002D7B RID: 11643
		public bool isTeleported;

		// Token: 0x04002D7C RID: 11644
		private bool isFirstSnapshot = true;

		// Token: 0x04002D7D RID: 11645
		private bool isMine;

		// Token: 0x04002D7E RID: 11646
		private bool isFirstHistoryFull;

		// Token: 0x04002D7F RID: 11647
		private PetEngine engine;

		// Token: 0x04002D80 RID: 11648
		private bool _effectPlayed;

		// Token: 0x04002D81 RID: 11649
		private bool isHided;

		// Token: 0x020006EE RID: 1774
		private struct MovementHistoryEntry
		{
			// Token: 0x04002D82 RID: 11650
			public Vector3 playerPos;

			// Token: 0x04002D83 RID: 11651
			public Quaternion playerRot;

			// Token: 0x04002D84 RID: 11652
			public PetAnimationType anim;

			// Token: 0x04002D85 RID: 11653
			public double timeStamp;

			// Token: 0x04002D86 RID: 11654
			public bool teleported;
		}
	}
}
