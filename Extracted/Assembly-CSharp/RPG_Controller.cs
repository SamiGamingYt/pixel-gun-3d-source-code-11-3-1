using System;
using UnityEngine;

// Token: 0x020007D8 RID: 2008
public class RPG_Controller : MonoBehaviour
{
	// Token: 0x060048D1 RID: 18641 RVA: 0x0019479C File Offset: 0x0019299C
	private void Awake()
	{
		RPG_Controller.instance = this;
		this.characterController = (base.GetComponent("CharacterController") as CharacterController);
		RPG_Camera.CameraSetup();
	}

	// Token: 0x060048D2 RID: 18642 RVA: 0x001947C0 File Offset: 0x001929C0
	private void Update()
	{
		if (Camera.main == null)
		{
			return;
		}
		if (this.characterController == null)
		{
			Debug.Log("Error: No Character Controller component found! Please add one to the GameObject which has this script attached.");
			return;
		}
		this.GetInput();
		this.StartMotor();
	}

	// Token: 0x060048D3 RID: 18643 RVA: 0x001947FC File Offset: 0x001929FC
	private void GetInput()
	{
		float d = 0f;
		float d2 = 0f;
		if (Input.GetButton("Horizontal Strafe"))
		{
			d = ((Input.GetAxis("Horizontal Strafe") >= 0f) ? ((Input.GetAxis("Horizontal Strafe") <= 0f) ? 0f : 1f) : -1f);
		}
		if (Input.GetButton("Vertical"))
		{
			d2 = ((Input.GetAxis("Vertical") >= 0f) ? ((Input.GetAxis("Vertical") <= 0f) ? 0f : 1f) : -1f);
		}
		if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
		{
			d2 = 1f;
		}
		this.playerDir = d * Vector3.right + d2 * Vector3.forward;
		if (RPG_Animation.instance != null)
		{
			RPG_Animation.instance.SetCurrentMoveDir(this.playerDir);
		}
		if (this.characterController.isGrounded)
		{
			this.playerDirWorld = base.transform.TransformDirection(this.playerDir);
			if (Mathf.Abs(this.playerDir.x) + Mathf.Abs(this.playerDir.z) > 1f)
			{
				this.playerDirWorld.Normalize();
			}
			this.playerDirWorld *= this.walkSpeed;
			this.playerDirWorld.y = this.fallingThreshold;
			if (Input.GetButtonDown("Jump"))
			{
				this.playerDirWorld.y = this.jumpHeight;
				if (RPG_Animation.instance != null)
				{
					RPG_Animation.instance.Jump();
				}
			}
		}
		this.rotation.y = Input.GetAxis("Horizontal") * this.turnSpeed;
	}

	// Token: 0x060048D4 RID: 18644 RVA: 0x001949F0 File Offset: 0x00192BF0
	private void StartMotor()
	{
		this.playerDirWorld.y = this.playerDirWorld.y - this.gravity * Time.deltaTime;
		this.characterController.Move(this.playerDirWorld * Time.deltaTime);
		base.transform.Rotate(this.rotation);
		if (!Input.GetMouseButton(0))
		{
			RPG_Camera.instance.RotateWithCharacter();
		}
	}

	// Token: 0x040035E3 RID: 13795
	public static RPG_Controller instance;

	// Token: 0x040035E4 RID: 13796
	public CharacterController characterController;

	// Token: 0x040035E5 RID: 13797
	public float walkSpeed = 10f;

	// Token: 0x040035E6 RID: 13798
	public float turnSpeed = 2.5f;

	// Token: 0x040035E7 RID: 13799
	public float jumpHeight = 10f;

	// Token: 0x040035E8 RID: 13800
	public float gravity = 20f;

	// Token: 0x040035E9 RID: 13801
	public float fallingThreshold = -6f;

	// Token: 0x040035EA RID: 13802
	private Vector3 playerDir;

	// Token: 0x040035EB RID: 13803
	private Vector3 playerDirWorld;

	// Token: 0x040035EC RID: 13804
	private Vector3 rotation = Vector3.zero;
}
