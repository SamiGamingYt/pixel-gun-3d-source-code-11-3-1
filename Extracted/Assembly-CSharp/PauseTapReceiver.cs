using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020003DD RID: 989
public class PauseTapReceiver : MonoBehaviour
{
	// Token: 0x14000026 RID: 38
	// (add) Token: 0x060023AA RID: 9130 RVA: 0x000B17A8 File Offset: 0x000AF9A8
	// (remove) Token: 0x060023AB RID: 9131 RVA: 0x000B17C0 File Offset: 0x000AF9C0
	public static event Action PauseClicked;

	// Token: 0x060023AC RID: 9132 RVA: 0x000B17D8 File Offset: 0x000AF9D8
	private void OnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (SceneLoader.ActiveSceneName.Equals("Training") && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted)
		{
			return;
		}
		if (PauseTapReceiver.PauseClicked != null)
		{
			PauseTapReceiver.PauseClicked();
		}
	}
}
