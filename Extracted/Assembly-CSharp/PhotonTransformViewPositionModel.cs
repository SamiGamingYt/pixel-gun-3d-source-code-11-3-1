using System;
using UnityEngine;

// Token: 0x0200042F RID: 1071
[Serializable]
public class PhotonTransformViewPositionModel
{
	// Token: 0x04001AD5 RID: 6869
	public bool SynchronizeEnabled;

	// Token: 0x04001AD6 RID: 6870
	public bool TeleportEnabled = true;

	// Token: 0x04001AD7 RID: 6871
	public float TeleportIfDistanceGreaterThan = 3f;

	// Token: 0x04001AD8 RID: 6872
	public PhotonTransformViewPositionModel.InterpolateOptions InterpolateOption = PhotonTransformViewPositionModel.InterpolateOptions.EstimatedSpeed;

	// Token: 0x04001AD9 RID: 6873
	public float InterpolateMoveTowardsSpeed = 1f;

	// Token: 0x04001ADA RID: 6874
	public float InterpolateLerpSpeed = 1f;

	// Token: 0x04001ADB RID: 6875
	public float InterpolateMoveTowardsAcceleration = 2f;

	// Token: 0x04001ADC RID: 6876
	public float InterpolateMoveTowardsDeceleration = 2f;

	// Token: 0x04001ADD RID: 6877
	public AnimationCurve InterpolateSpeedCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(-1f, 0f, 0f, float.PositiveInfinity),
		new Keyframe(0f, 1f, 0f, 0f),
		new Keyframe(1f, 1f, 0f, 1f),
		new Keyframe(4f, 4f, 1f, 0f)
	});

	// Token: 0x04001ADE RID: 6878
	public PhotonTransformViewPositionModel.ExtrapolateOptions ExtrapolateOption;

	// Token: 0x04001ADF RID: 6879
	public float ExtrapolateSpeed = 1f;

	// Token: 0x04001AE0 RID: 6880
	public bool ExtrapolateIncludingRoundTripTime = true;

	// Token: 0x04001AE1 RID: 6881
	public int ExtrapolateNumberOfStoredPositions = 1;

	// Token: 0x04001AE2 RID: 6882
	public bool DrawErrorGizmo = true;

	// Token: 0x02000430 RID: 1072
	public enum InterpolateOptions
	{
		// Token: 0x04001AE4 RID: 6884
		Disabled,
		// Token: 0x04001AE5 RID: 6885
		FixedSpeed,
		// Token: 0x04001AE6 RID: 6886
		EstimatedSpeed,
		// Token: 0x04001AE7 RID: 6887
		SynchronizeValues,
		// Token: 0x04001AE8 RID: 6888
		Lerp
	}

	// Token: 0x02000431 RID: 1073
	public enum ExtrapolateOptions
	{
		// Token: 0x04001AEA RID: 6890
		Disabled,
		// Token: 0x04001AEB RID: 6891
		SynchronizeValues,
		// Token: 0x04001AEC RID: 6892
		EstimateSpeedAndTurn,
		// Token: 0x04001AED RID: 6893
		FixedSpeed
	}
}
