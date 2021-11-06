using System;
using UnityEngine;

// Token: 0x02000823 RID: 2083
public class SunRotation : MonoBehaviour
{
	// Token: 0x06004BBB RID: 19387 RVA: 0x001B405C File Offset: 0x001B225C
	private void LateUpdate()
	{
		if (TimeGameController.sharedController != null && PhotonNetwork.room != null && !string.IsNullOrEmpty(ConnectSceneNGUIController.maxKillProperty) && PhotonNetwork.room.customProperties.ContainsKey(ConnectSceneNGUIController.maxKillProperty))
		{
			int num = -1;
			int.TryParse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty].ToString(), out num);
			if (num < 0)
			{
				return;
			}
			this.matchTime = (float)num * 60f;
			if ((float)TimeGameController.sharedController.timerToEndMatch < this.matchTime)
			{
				this.matchTimeDelta = this.matchTime - (float)TimeGameController.sharedController.timerToEndMatch;
				if (Camera.main)
				{
					this.sun.LookAt(Camera.main.transform);
				}
				Quaternion localRotation = default(Quaternion);
				localRotation.eulerAngles = new Vector3(this.xRotation.Evaluate(this.matchTimeDelta / this.matchTime), 0f, 0f);
				base.transform.localRotation = localRotation;
				localRotation.eulerAngles = new Vector3(0f, this.yRotation.Evaluate(this.matchTimeDelta / this.matchTime), 0f);
				this.yAxis.localRotation = localRotation;
			}
		}
	}

	// Token: 0x04003ADB RID: 15067
	public AnimationCurve xRotation;

	// Token: 0x04003ADC RID: 15068
	public AnimationCurve yRotation;

	// Token: 0x04003ADD RID: 15069
	public Transform sun;

	// Token: 0x04003ADE RID: 15070
	public Transform yAxis;

	// Token: 0x04003ADF RID: 15071
	private float matchTime;

	// Token: 0x04003AE0 RID: 15072
	private float matchTimeDelta;
}
