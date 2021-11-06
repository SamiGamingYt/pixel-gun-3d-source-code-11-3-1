using System;
using UnityEngine;

// Token: 0x020006B1 RID: 1713
public class MainMenuPersClickHandler : MonoBehaviour
{
	// Token: 0x06003BCE RID: 15310 RVA: 0x00136570 File Offset: 0x00134770
	private void OnMouseDown()
	{
		this._startPos = Input.mousePosition;
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x00136580 File Offset: 0x00134780
	private void OnMouseUp()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (Mathf.Abs(this._startPos.magnitude - mousePosition.magnitude) > this.DragDistance)
		{
			return;
		}
		if (MainMenuController.sharedController != null && MainMenuController.sharedController.mainPanel != null && !MainMenuController.sharedController.mainPanel.activeInHierarchy)
		{
			return;
		}
		if (UICamera.lastHit.collider != null)
		{
			return;
		}
		if (!TrainingController.TrainingCompleted)
		{
			return;
		}
		if (ProfileController.Instance != null)
		{
			ProfileController.Instance.SetStaticticTab(ProfileStatTabType.Multiplayer);
		}
		MainMenuController.sharedController.GoToProfile();
	}

	// Token: 0x04002C34 RID: 11316
	public float DragDistance = 20f;

	// Token: 0x04002C35 RID: 11317
	private Vector3 _startPos;
}
