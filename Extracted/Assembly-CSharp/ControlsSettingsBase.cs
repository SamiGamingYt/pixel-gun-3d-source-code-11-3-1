using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000083 RID: 131
public class ControlsSettingsBase : MonoBehaviour
{
	// Token: 0x14000009 RID: 9
	// (add) Token: 0x060003F5 RID: 1013 RVA: 0x0002269C File Offset: 0x0002089C
	// (remove) Token: 0x060003F6 RID: 1014 RVA: 0x000226B4 File Offset: 0x000208B4
	public static event Action ControlsChanged;

	// Token: 0x060003F7 RID: 1015 RVA: 0x000226CC File Offset: 0x000208CC
	protected void HandleControlsClicked()
	{
		if (TrainingController.TrainingCompletedFlagForLogging != null)
		{
			TrainingController.TrainingCompletedFlagForLogging = null;
		}
		if (GlobalGameController.LeftHanded)
		{
			this.BottomRight.localPosition = new Vector3(0f, 0f, 0f);
			this.TopRight.localPosition = new Vector3(-512f, 450f, 0f);
			this.TopLeft.localPosition = new Vector3(0f, 380f, 0f);
			this.BottomLeft.localPosition = new Vector3(300f, 0f, 0f);
			this.BottomLeftControlsAnchor.side = UIAnchor.Side.BottomLeft;
			this.BottomRightControlsAnchor.side = UIAnchor.Side.BottomRight;
		}
		else
		{
			this.BottomRight.localPosition = new Vector3(512f, 0f, 0f);
			this.TopRight.localPosition = new Vector3(0f, 450f, 0f);
			this.TopLeft.localPosition = new Vector3(-300f, 380f, 0f);
			this.BottomLeft.localPosition = new Vector3(0f, 0f, 0f);
			this.BottomLeftControlsAnchor.side = UIAnchor.Side.BottomRight;
			this.BottomRightControlsAnchor.side = UIAnchor.Side.BottomLeft;
		}
		this.SetControlsCoords();
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x00022838 File Offset: 0x00020A38
	private void SetControlsCoords()
	{
		float num = (!GlobalGameController.LeftHanded) ? -1f : 1f;
		Vector3[] array = Load.LoadVector3Array(ControlsSettingsBase.JoystickSett);
		if (array == null || array.Length < 7)
		{
			Defs.InitCoordsIphone();
			this.zoomButton.transform.localPosition = new Vector3((float)Defs.ZoomButtonX * num, (float)Defs.ZoomButtonY, this.zoomButton.transform.localPosition.z);
			this.reloadButton.transform.localPosition = new Vector3((float)Defs.ReloadButtonX * num, (float)Defs.ReloadButtonY, this.reloadButton.transform.localPosition.z);
			this.jumpButton.transform.localPosition = new Vector3((float)Defs.JumpButtonX * num, (float)Defs.JumpButtonY, this.jumpButton.transform.localPosition.z);
			this.fireButton.transform.localPosition = new Vector3((float)Defs.FireButtonX * num, (float)Defs.FireButtonY, this.fireButton.transform.localPosition.z);
			this.grenadeButton.transform.localPosition = new Vector3((float)Defs.GrenadeX * num, (float)Defs.GrenadeY, this.grenadeButton.transform.localPosition.z);
			this.joystick.transform.localPosition = new Vector3((float)Defs.JoyStickX * num, (float)Defs.JoyStickY, this.joystick.transform.localPosition.z);
			this.fireButtonInJoystick.transform.localPosition = new Vector3((float)Defs.FireButton2X * num, (float)Defs.FireButton2Y, this.fireButtonInJoystick.transform.localPosition.z);
		}
		else
		{
			for (int i = 0; i < array.Length; i++)
			{
				Vector3[] array2 = array;
				int num2 = i;
				array2[num2].x = array2[num2].x * num;
			}
			this.zoomButton.transform.localPosition = array[0];
			this.reloadButton.transform.localPosition = array[1];
			this.jumpButton.transform.localPosition = array[2];
			this.fireButton.transform.localPosition = array[3];
			this.joystick.transform.localPosition = array[4];
			this.grenadeButton.transform.localPosition = array[5];
			this.fireButtonInJoystick.transform.localPosition = array[6];
		}
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x00022B14 File Offset: 0x00020D14
	protected void OnEnable()
	{
		if (ExperienceController.sharedController != null && !ShopNGUIController.GuiActive)
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
		if (ExpController.Instance != null)
		{
			ExpController.Instance.InterfaceEnabled = false;
		}
		this.SetControlsCoords();
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x00022B68 File Offset: 0x00020D68
	protected virtual void HandleSavePosJoystikClicked(object sender, EventArgs e)
	{
		float num = (float)((!GlobalGameController.LeftHanded) ? -1 : 1);
		Save.SaveVector3Array(ControlsSettingsBase.JoystickSett, new Vector3[]
		{
			new Vector3(this.zoomButton.transform.localPosition.x * num, this.zoomButton.transform.localPosition.y, this.zoomButton.transform.localPosition.z),
			new Vector3(this.reloadButton.transform.localPosition.x * num, this.reloadButton.transform.localPosition.y, this.reloadButton.transform.localPosition.z),
			new Vector3(this.jumpButton.transform.localPosition.x * num, this.jumpButton.transform.localPosition.y, this.jumpButton.transform.localPosition.z),
			new Vector3(this.fireButton.transform.localPosition.x * num, this.fireButton.transform.localPosition.y, this.fireButton.transform.localPosition.z),
			new Vector3(this.joystick.transform.localPosition.x * num, this.joystick.transform.localPosition.y, this.joystick.transform.localPosition.z),
			new Vector3(this.grenadeButton.transform.localPosition.x * num, this.grenadeButton.transform.localPosition.y, this.grenadeButton.transform.localPosition.z),
			new Vector3(this.fireButtonInJoystick.transform.localPosition.x * num, this.fireButtonInJoystick.transform.localPosition.y, this.fireButtonInJoystick.transform.localPosition.z)
		});
		this.SettingsJoysticksPanel.SetActive(false);
		this.settingsPanel.SetActive(true);
		ExperienceController.sharedController.isShowRanks = false;
		Action controlsChanged = ControlsSettingsBase.ControlsChanged;
		if (controlsChanged != null)
		{
			ControlsSettingsBase.ControlsChanged();
		}
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x00022E64 File Offset: 0x00021064
	private void HandleDefaultPosJoystikClicked(object sender, EventArgs e)
	{
		float num = (float)((!GlobalGameController.LeftHanded) ? -1 : 1);
		Defs.InitCoordsIphone();
		this.zoomButton.transform.localPosition = new Vector3((float)Defs.ZoomButtonX * num, (float)Defs.ZoomButtonY, this.zoomButton.transform.localPosition.z);
		this.reloadButton.transform.localPosition = new Vector3((float)Defs.ReloadButtonX * num, (float)Defs.ReloadButtonY, this.reloadButton.transform.localPosition.z);
		this.jumpButton.transform.localPosition = new Vector3((float)Defs.JumpButtonX * num, (float)Defs.JumpButtonY, this.jumpButton.transform.localPosition.z);
		this.fireButton.transform.localPosition = new Vector3((float)Defs.FireButtonX * num, (float)Defs.FireButtonY, this.fireButton.transform.localPosition.z);
		this.joystick.transform.localPosition = new Vector3((float)Defs.JoyStickX * num, (float)Defs.JoyStickY, this.joystick.transform.localPosition.z);
		this.grenadeButton.transform.localPosition = new Vector3((float)Defs.GrenadeX * num, (float)Defs.GrenadeY, this.grenadeButton.transform.localPosition.z);
		this.fireButtonInJoystick.transform.localPosition = new Vector3((float)Defs.FireButton2X * num, (float)Defs.FireButton2Y, this.fireButtonInJoystick.transform.localPosition.z);
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0002302C File Offset: 0x0002122C
	protected virtual void HandleCancelPosJoystikClicked(object sender, EventArgs e)
	{
		this._isCancellationRequested = true;
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x00023038 File Offset: 0x00021238
	protected void Start()
	{
		if (this.savePosJoystikButton != null)
		{
			ButtonHandler component = this.savePosJoystikButton.GetComponent<ButtonHandler>();
			if (component != null)
			{
				component.Clicked += this.HandleSavePosJoystikClicked;
			}
		}
		if (this.defaultPosJoystikButton != null)
		{
			ButtonHandler component2 = this.defaultPosJoystikButton.GetComponent<ButtonHandler>();
			if (component2 != null)
			{
				component2.Clicked += this.HandleDefaultPosJoystikClicked;
			}
		}
		if (this.cancelPosJoystikButton != null)
		{
			ButtonHandler component3 = this.cancelPosJoystikButton.GetComponent<ButtonHandler>();
			if (component3 != null)
			{
				component3.Clicked += this.HandleCancelPosJoystikClicked;
			}
		}
	}

	// Token: 0x0400047C RID: 1148
	public GameObject settingsPanel;

	// Token: 0x0400047D RID: 1149
	public GameObject savePosJoystikButton;

	// Token: 0x0400047E RID: 1150
	public GameObject defaultPosJoystikButton;

	// Token: 0x0400047F RID: 1151
	public GameObject cancelPosJoystikButton;

	// Token: 0x04000480 RID: 1152
	public GameObject SettingsJoysticksPanel;

	// Token: 0x04000481 RID: 1153
	public GameObject zoomButton;

	// Token: 0x04000482 RID: 1154
	public GameObject reloadButton;

	// Token: 0x04000483 RID: 1155
	public GameObject jumpButton;

	// Token: 0x04000484 RID: 1156
	public GameObject fireButton;

	// Token: 0x04000485 RID: 1157
	public GameObject joystick;

	// Token: 0x04000486 RID: 1158
	public GameObject grenadeButton;

	// Token: 0x04000487 RID: 1159
	public GameObject fireButtonInJoystick;

	// Token: 0x04000488 RID: 1160
	public UIAnchor BottomLeftControlsAnchor;

	// Token: 0x04000489 RID: 1161
	public UIAnchor BottomRightControlsAnchor;

	// Token: 0x0400048A RID: 1162
	public Transform BottomLeft;

	// Token: 0x0400048B RID: 1163
	public Transform TopLeft;

	// Token: 0x0400048C RID: 1164
	public Transform BottomRight;

	// Token: 0x0400048D RID: 1165
	public Transform TopRight;

	// Token: 0x0400048E RID: 1166
	public static string JoystickSett = "JoystickSettSettSett";

	// Token: 0x0400048F RID: 1167
	public bool _isCancellationRequested;
}
