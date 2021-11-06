using System;
using UnityEngine;

// Token: 0x020007D2 RID: 2002
public class RPG_Animation : MonoBehaviour
{
	// Token: 0x060048AE RID: 18606 RVA: 0x0019346C File Offset: 0x0019166C
	private void Awake()
	{
		RPG_Animation.instance = this;
	}

	// Token: 0x060048AF RID: 18607 RVA: 0x00193474 File Offset: 0x00191674
	private void Update()
	{
		this.SetCurrentState();
		this.StartAnimation();
	}

	// Token: 0x060048B0 RID: 18608 RVA: 0x00193484 File Offset: 0x00191684
	public void SetCurrentMoveDir(Vector3 playerDir)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (playerDir.z > 0f)
		{
			flag = true;
		}
		if (playerDir.z < 0f)
		{
			flag2 = true;
		}
		if (playerDir.x < 0f)
		{
			flag3 = true;
		}
		if (playerDir.x > 0f)
		{
			flag4 = true;
		}
		if (flag)
		{
			if (flag3)
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeForwardLeft;
			}
			else if (flag4)
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeForwardRight;
			}
			else
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.Forward;
			}
		}
		else if (flag2)
		{
			if (flag3)
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeBackLeft;
			}
			else if (flag4)
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeBackRight;
			}
			else
			{
				this.currentMoveDir = RPG_Animation.CharacterMoveDirection.Backward;
			}
		}
		else if (flag3)
		{
			this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeLeft;
		}
		else if (flag4)
		{
			this.currentMoveDir = RPG_Animation.CharacterMoveDirection.StrafeRight;
		}
		else
		{
			this.currentMoveDir = RPG_Animation.CharacterMoveDirection.None;
		}
	}

	// Token: 0x060048B1 RID: 18609 RVA: 0x0019357C File Offset: 0x0019177C
	public void SetCurrentState()
	{
		if (RPG_Controller.instance.characterController.isGrounded)
		{
			switch (this.currentMoveDir)
			{
			case RPG_Animation.CharacterMoveDirection.None:
				this.currentState = RPG_Animation.CharacterState.Idle;
				break;
			case RPG_Animation.CharacterMoveDirection.Forward:
				this.currentState = RPG_Animation.CharacterState.Walk;
				break;
			case RPG_Animation.CharacterMoveDirection.Backward:
				this.currentState = RPG_Animation.CharacterState.WalkBack;
				break;
			case RPG_Animation.CharacterMoveDirection.StrafeLeft:
				this.currentState = RPG_Animation.CharacterState.StrafeLeft;
				break;
			case RPG_Animation.CharacterMoveDirection.StrafeRight:
				this.currentState = RPG_Animation.CharacterState.StrafeRight;
				break;
			case RPG_Animation.CharacterMoveDirection.StrafeForwardLeft:
				this.currentState = RPG_Animation.CharacterState.Walk;
				break;
			case RPG_Animation.CharacterMoveDirection.StrafeForwardRight:
				this.currentState = RPG_Animation.CharacterState.Walk;
				break;
			case RPG_Animation.CharacterMoveDirection.StrafeBackLeft:
				this.currentState = RPG_Animation.CharacterState.WalkBack;
				break;
			case RPG_Animation.CharacterMoveDirection.StrafeBackRight:
				this.currentState = RPG_Animation.CharacterState.WalkBack;
				break;
			}
		}
	}

	// Token: 0x060048B2 RID: 18610 RVA: 0x00193640 File Offset: 0x00191840
	public void StartAnimation()
	{
		switch (this.currentState)
		{
		case RPG_Animation.CharacterState.Idle:
			this.Idle();
			break;
		case RPG_Animation.CharacterState.Walk:
			if (this.currentMoveDir == RPG_Animation.CharacterMoveDirection.StrafeForwardLeft)
			{
				this.StrafeForwardLeft();
			}
			else if (this.currentMoveDir == RPG_Animation.CharacterMoveDirection.StrafeForwardRight)
			{
				this.StrafeForwardRight();
			}
			else
			{
				this.Walk();
			}
			break;
		case RPG_Animation.CharacterState.WalkBack:
			if (this.currentMoveDir == RPG_Animation.CharacterMoveDirection.StrafeBackLeft)
			{
				this.StrafeBackLeft();
			}
			else if (this.currentMoveDir == RPG_Animation.CharacterMoveDirection.StrafeBackRight)
			{
				this.StrafeBackRight();
			}
			else
			{
				this.WalkBack();
			}
			break;
		case RPG_Animation.CharacterState.StrafeLeft:
			this.StrafeLeft();
			break;
		case RPG_Animation.CharacterState.StrafeRight:
			this.StrafeRight();
			break;
		}
	}

	// Token: 0x060048B3 RID: 18611 RVA: 0x00193708 File Offset: 0x00191908
	private void Idle()
	{
		base.GetComponent<Animation>().CrossFade("idle");
	}

	// Token: 0x060048B4 RID: 18612 RVA: 0x0019371C File Offset: 0x0019191C
	private void Walk()
	{
		base.GetComponent<Animation>().CrossFade("walk");
	}

	// Token: 0x060048B5 RID: 18613 RVA: 0x00193730 File Offset: 0x00191930
	private void StrafeForwardLeft()
	{
		base.GetComponent<Animation>().CrossFade("strafeforwardleft");
	}

	// Token: 0x060048B6 RID: 18614 RVA: 0x00193744 File Offset: 0x00191944
	private void StrafeForwardRight()
	{
		base.GetComponent<Animation>().CrossFade("strafeforwardright");
	}

	// Token: 0x060048B7 RID: 18615 RVA: 0x00193758 File Offset: 0x00191958
	private void WalkBack()
	{
		base.GetComponent<Animation>().CrossFade("walkback");
	}

	// Token: 0x060048B8 RID: 18616 RVA: 0x0019376C File Offset: 0x0019196C
	private void StrafeBackLeft()
	{
		base.GetComponent<Animation>().CrossFade("strafebackleft");
	}

	// Token: 0x060048B9 RID: 18617 RVA: 0x00193780 File Offset: 0x00191980
	private void StrafeBackRight()
	{
		base.GetComponent<Animation>().CrossFade("strafebackright");
	}

	// Token: 0x060048BA RID: 18618 RVA: 0x00193794 File Offset: 0x00191994
	private void StrafeLeft()
	{
		base.GetComponent<Animation>().CrossFade("strafeleft");
	}

	// Token: 0x060048BB RID: 18619 RVA: 0x001937A8 File Offset: 0x001919A8
	private void StrafeRight()
	{
		base.GetComponent<Animation>().CrossFade("straferight");
	}

	// Token: 0x060048BC RID: 18620 RVA: 0x001937BC File Offset: 0x001919BC
	public void Jump()
	{
		this.currentState = RPG_Animation.CharacterState.Jump;
		if (base.GetComponent<Animation>().IsPlaying("jump"))
		{
			base.GetComponent<Animation>().Stop("jump");
		}
		base.GetComponent<Animation>().CrossFade("jump");
	}

	// Token: 0x0400359E RID: 13726
	public static RPG_Animation instance;

	// Token: 0x0400359F RID: 13727
	public RPG_Animation.CharacterMoveDirection currentMoveDir;

	// Token: 0x040035A0 RID: 13728
	public RPG_Animation.CharacterState currentState;

	// Token: 0x020007D3 RID: 2003
	public enum CharacterMoveDirection
	{
		// Token: 0x040035A2 RID: 13730
		None,
		// Token: 0x040035A3 RID: 13731
		Forward,
		// Token: 0x040035A4 RID: 13732
		Backward,
		// Token: 0x040035A5 RID: 13733
		StrafeLeft,
		// Token: 0x040035A6 RID: 13734
		StrafeRight,
		// Token: 0x040035A7 RID: 13735
		StrafeForwardLeft,
		// Token: 0x040035A8 RID: 13736
		StrafeForwardRight,
		// Token: 0x040035A9 RID: 13737
		StrafeBackLeft,
		// Token: 0x040035AA RID: 13738
		StrafeBackRight
	}

	// Token: 0x020007D4 RID: 2004
	public enum CharacterState
	{
		// Token: 0x040035AC RID: 13740
		Idle,
		// Token: 0x040035AD RID: 13741
		Walk,
		// Token: 0x040035AE RID: 13742
		WalkBack,
		// Token: 0x040035AF RID: 13743
		StrafeLeft,
		// Token: 0x040035B0 RID: 13744
		StrafeRight,
		// Token: 0x040035B1 RID: 13745
		Jump
	}
}
