using System;

// Token: 0x02000436 RID: 1078
[Serializable]
public class PhotonTransformViewScaleModel
{
	// Token: 0x04001AFA RID: 6906
	public bool SynchronizeEnabled;

	// Token: 0x04001AFB RID: 6907
	public PhotonTransformViewScaleModel.InterpolateOptions InterpolateOption;

	// Token: 0x04001AFC RID: 6908
	public float InterpolateMoveTowardsSpeed = 1f;

	// Token: 0x04001AFD RID: 6909
	public float InterpolateLerpSpeed;

	// Token: 0x02000437 RID: 1079
	public enum InterpolateOptions
	{
		// Token: 0x04001AFF RID: 6911
		Disabled,
		// Token: 0x04001B00 RID: 6912
		MoveTowards,
		// Token: 0x04001B01 RID: 6913
		Lerp
	}
}
