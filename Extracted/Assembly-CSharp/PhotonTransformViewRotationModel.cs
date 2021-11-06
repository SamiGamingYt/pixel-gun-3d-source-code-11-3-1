using System;

// Token: 0x02000433 RID: 1075
[Serializable]
public class PhotonTransformViewRotationModel
{
	// Token: 0x04001AF0 RID: 6896
	public bool SynchronizeEnabled;

	// Token: 0x04001AF1 RID: 6897
	public PhotonTransformViewRotationModel.InterpolateOptions InterpolateOption = PhotonTransformViewRotationModel.InterpolateOptions.RotateTowards;

	// Token: 0x04001AF2 RID: 6898
	public float InterpolateRotateTowardsSpeed = 180f;

	// Token: 0x04001AF3 RID: 6899
	public float InterpolateLerpSpeed = 5f;

	// Token: 0x02000434 RID: 1076
	public enum InterpolateOptions
	{
		// Token: 0x04001AF5 RID: 6901
		Disabled,
		// Token: 0x04001AF6 RID: 6902
		RotateTowards,
		// Token: 0x04001AF7 RID: 6903
		Lerp
	}
}
