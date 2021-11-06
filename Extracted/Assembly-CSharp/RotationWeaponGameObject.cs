using System;
using UnityEngine;

// Token: 0x020004D3 RID: 1235
public class RotationWeaponGameObject : MonoBehaviour
{
	// Token: 0x06002C38 RID: 11320 RVA: 0x000EAB5C File Offset: 0x000E8D5C
	private void Start()
	{
		this.thisTransform = base.transform;
		switch (this.axis)
		{
		case RotationWeaponGameObject.ConstraintAxis.X:
			this.rotateAround = Vector3.right;
			break;
		case RotationWeaponGameObject.ConstraintAxis.Y:
			this.rotateAround = Vector3.up;
			break;
		case RotationWeaponGameObject.ConstraintAxis.Z:
			this.rotateAround = Vector3.forward;
			break;
		}
		Quaternion lhs = Quaternion.AngleAxis((this.axis != RotationWeaponGameObject.ConstraintAxis.X) ? ((this.axis != RotationWeaponGameObject.ConstraintAxis.Y) ? this.thisTransform.localRotation.eulerAngles.z : this.thisTransform.localRotation.eulerAngles.y) : this.thisTransform.localRotation.eulerAngles.x, this.rotateAround);
		this.minQuaternion = lhs * Quaternion.AngleAxis(this.min, this.rotateAround);
		this.maxQuaternion = lhs * Quaternion.AngleAxis(this.max, this.rotateAround);
		this.range = this.max - this.min;
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x000EAC94 File Offset: 0x000E8E94
	private void SetActiveFalse()
	{
		base.enabled = false;
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x000EACA0 File Offset: 0x000E8EA0
	private void LateUpdate()
	{
		Quaternion localRotation = this.thisTransform.localRotation;
		Quaternion a = Quaternion.AngleAxis((this.axis != RotationWeaponGameObject.ConstraintAxis.X) ? ((this.axis != RotationWeaponGameObject.ConstraintAxis.Y) ? localRotation.eulerAngles.z : localRotation.eulerAngles.y) : localRotation.eulerAngles.x, this.rotateAround);
		float num = Quaternion.Angle(a, this.minQuaternion);
		float num2 = Quaternion.Angle(a, this.maxQuaternion);
		if (num <= this.range && num2 <= this.range)
		{
			this.playerGun.rotation = this.thisTransform.rotation;
			this.playerGun.Rotate(this.player_move_c.deltaAngle, 0f, 0f);
			this.mechGun.rotation = this.thisTransform.rotation;
			this.mechGun.Rotate(this.player_move_c.deltaAngle, 0f, 0f);
			return;
		}
		Vector3 eulerAngles = localRotation.eulerAngles;
		if (num > num2)
		{
			eulerAngles = new Vector3((this.axis != RotationWeaponGameObject.ConstraintAxis.X) ? eulerAngles.x : this.maxQuaternion.eulerAngles.x, (this.axis != RotationWeaponGameObject.ConstraintAxis.Y) ? eulerAngles.y : this.maxQuaternion.eulerAngles.y, (this.axis != RotationWeaponGameObject.ConstraintAxis.Z) ? eulerAngles.z : this.maxQuaternion.eulerAngles.z);
		}
		else
		{
			eulerAngles = new Vector3((this.axis != RotationWeaponGameObject.ConstraintAxis.X) ? eulerAngles.x : this.minQuaternion.eulerAngles.x, (this.axis != RotationWeaponGameObject.ConstraintAxis.Y) ? eulerAngles.y : this.minQuaternion.eulerAngles.y, (this.axis != RotationWeaponGameObject.ConstraintAxis.Z) ? eulerAngles.z : this.minQuaternion.eulerAngles.z);
		}
		this.thisTransform.localEulerAngles = eulerAngles;
		this.playerGun.rotation = this.thisTransform.rotation;
		this.playerGun.Rotate(this.player_move_c.deltaAngle, 0f, 0f);
		this.mechGun.rotation = this.thisTransform.rotation;
		this.mechGun.Rotate(this.player_move_c.deltaAngle, 0f, 0f);
	}

	// Token: 0x0400213C RID: 8508
	public RotationWeaponGameObject.ConstraintAxis axis;

	// Token: 0x0400213D RID: 8509
	public float min;

	// Token: 0x0400213E RID: 8510
	public float max;

	// Token: 0x0400213F RID: 8511
	public Transform playerGun;

	// Token: 0x04002140 RID: 8512
	public Transform mechGun;

	// Token: 0x04002141 RID: 8513
	public Player_move_c player_move_c;

	// Token: 0x04002142 RID: 8514
	private Transform thisTransform;

	// Token: 0x04002143 RID: 8515
	private Vector3 rotateAround;

	// Token: 0x04002144 RID: 8516
	private Quaternion minQuaternion;

	// Token: 0x04002145 RID: 8517
	private Quaternion maxQuaternion;

	// Token: 0x04002146 RID: 8518
	private float range;

	// Token: 0x020004D4 RID: 1236
	public enum ConstraintAxis
	{
		// Token: 0x04002148 RID: 8520
		X,
		// Token: 0x04002149 RID: 8521
		Y,
		// Token: 0x0400214A RID: 8522
		Z
	}
}
