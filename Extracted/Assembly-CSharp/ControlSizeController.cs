using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020005C9 RID: 1481
internal sealed class ControlSizeController : MonoBehaviour
{
	// Token: 0x06003312 RID: 13074 RVA: 0x00108580 File Offset: 0x00106780
	public void HandleSizeSliderChanged(UISlider slider)
	{
		if (this._currentSprite == null)
		{
			Debug.LogWarning("_currentSprite == null");
			return;
		}
		ControlSize component = this._currentSprite.GetComponent<ControlSize>();
		if (component == null)
		{
			Debug.LogWarning("cs == null");
			return;
		}
		this._currentSprite.width = Mathf.RoundToInt(Mathf.Lerp((float)component.minValue, (float)component.maxValue, slider.value));
	}

	// Token: 0x06003313 RID: 13075 RVA: 0x001085F8 File Offset: 0x001067F8
	private void HandleControlPressedDown(object sender, EventArgs e)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		Debug.Log("Pressed");
		GameObject gameObject = sender as GameObject;
		if (gameObject == null)
		{
			return;
		}
		UISprite component = gameObject.GetComponent<UISprite>();
		if (component == null)
		{
			return;
		}
		this.SetCurrentSprite(component, this.view.slider);
	}

	// Token: 0x06003314 RID: 13076 RVA: 0x0010865C File Offset: 0x0010685C
	public void HandleControlButton(UISprite sprite, UISlider slider)
	{
	}

	// Token: 0x06003315 RID: 13077 RVA: 0x00108660 File Offset: 0x00106860
	public void HandleSaveButton()
	{
		Debug.Log("[Save] Pressed.");
		this.SaveControlSize();
	}

	// Token: 0x06003316 RID: 13078 RVA: 0x00108674 File Offset: 0x00106874
	public void HandleDefaultButton()
	{
		Debug.Log("[Default] Pressed.");
		if (this.view == null || this.view.buttons == null)
		{
			Debug.LogWarning("view == null || view.buttons == null");
			return;
		}
		foreach (UISprite uisprite in this.view.buttons)
		{
			if (!(uisprite == null))
			{
				ControlSize component = uisprite.GetComponent<ControlSize>();
				if (!(component == null))
				{
					uisprite.width = component.defaultValue;
				}
			}
		}
	}

	// Token: 0x06003317 RID: 13079 RVA: 0x00108710 File Offset: 0x00106910
	public void HandleCancelButton()
	{
		Debug.Log("[Cancel] Pressed.");
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x0010871C File Offset: 0x0010691C
	public void LoadControlSize()
	{
		if (this.view == null || this.view.buttons == null)
		{
			Debug.LogWarning("view == null || view.buttons == null");
			return;
		}
		object obj = Json.Deserialize(PlayerPrefs.GetString("Controls.Size", "[]"));
		List<object> list = obj as List<object>;
		if (list == null)
		{
			list = new List<object>(this.view.buttons.Length);
			Debug.LogWarning(list.GetType().FullName);
		}
		int num = this.view.buttons.Length;
		for (int num2 = 0; num2 != num; num2++)
		{
			if (!(this.view.buttons[num2] == null))
			{
				int num3 = 0;
				if (num2 < list.Count)
				{
					num3 = Convert.ToInt32(list[num2]);
				}
				this.view.buttons[num2].width = ((num3 <= 0) ? this.view.buttons[num2].GetComponent<ControlSize>().defaultValue : num3);
			}
		}
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x0010882C File Offset: 0x00106A2C
	public void SaveControlSize()
	{
		if (this.view == null)
		{
			Debug.LogWarning("view == null");
			return;
		}
		Func<UISprite, int> selector = (UISprite s) => (!(s != null)) ? 0 : s.width;
		int[] obj = this.view.buttons.Select(selector).ToArray<int>();
		PlayerPrefs.SetString("Controls.Size", Json.Serialize(obj));
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x0010889C File Offset: 0x00106A9C
	private void Awake()
	{
		if (this.view != null && this.view.slider != null)
		{
			ControlSizeSlider component = this.view.slider.GetComponent<ControlSizeSlider>();
			if (component != null)
			{
				component.EnabledChanged += this.HandleEnabledChanged;
			}
		}
		ButtonJoystickAdjust.PressedDown = (EventHandler<EventArgs>)Delegate.Combine(ButtonJoystickAdjust.PressedDown, new EventHandler<EventArgs>(this.HandleControlPressedDown));
		PressDetector.PressedDown = (EventHandler<EventArgs>)Delegate.Combine(PressDetector.PressedDown, new EventHandler<EventArgs>(this.HandleControlPressedDown));
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x00108940 File Offset: 0x00106B40
	private void OnDestroy()
	{
		if (this.view != null && this.view.slider != null)
		{
			ControlSizeSlider component = this.view.slider.GetComponent<ControlSizeSlider>();
			if (component != null)
			{
				component.EnabledChanged -= this.HandleEnabledChanged;
			}
		}
		ButtonJoystickAdjust.PressedDown = (EventHandler<EventArgs>)Delegate.Remove(ButtonJoystickAdjust.PressedDown, new EventHandler<EventArgs>(this.HandleControlPressedDown));
		PressDetector.PressedDown = (EventHandler<EventArgs>)Delegate.Remove(PressDetector.PressedDown, new EventHandler<EventArgs>(this.HandleControlPressedDown));
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x001089E4 File Offset: 0x00106BE4
	private void HandleEnabledChanged(object sender, ControlSizeSlider.EnabledChangedEventArgs e)
	{
		if (e.Enabled)
		{
			this.LoadControlSize();
			this.SetCurrentSprite(this.view.buttons[0], this.view.slider);
		}
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x00108A18 File Offset: 0x00106C18
	public static void ChangeLeftHanded(bool isChecked, Action handler = null)
	{
		if (Application.isEditor)
		{
			Debug.Log("[Left Handed] button clicked: " + isChecked);
		}
		if (GlobalGameController.LeftHanded != isChecked)
		{
			GlobalGameController.LeftHanded = isChecked;
			PlayerPrefs.SetInt(Defs.LeftHandedSN, (!isChecked) ? 0 : 1);
			PlayerPrefs.Save();
			if (handler != null)
			{
				handler();
			}
			if (!isChecked && Application.isEditor)
			{
				Debug.Log("Left-handed Layout Enabled");
			}
		}
	}

	// Token: 0x0600331E RID: 13086 RVA: 0x00108A98 File Offset: 0x00106C98
	private void RefreshSlider(UISlider slider)
	{
		if (slider == null)
		{
			return;
		}
		if (this._currentSprite == null)
		{
			slider.value = 0f;
		}
		else
		{
			ControlSize component = this._currentSprite.GetComponent<ControlSize>();
			if (component == null)
			{
				slider.value = 0f;
			}
			else
			{
				slider.value = Mathf.InverseLerp((float)component.minValue, (float)component.maxValue, (float)this._currentSprite.width);
			}
		}
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x00108B20 File Offset: 0x00106D20
	private void SetCurrentSprite(UISprite sprite, UISlider slider)
	{
		this._currentSprite = sprite;
		foreach (UISprite uisprite in from b in this.view.buttons
		where b != null
		select b)
		{
			UISprite[] componentsInChildren = uisprite.gameObject.GetComponentsInChildren<UISprite>();
			if (componentsInChildren.Length != 0)
			{
				if (uisprite.gameObject == sprite.gameObject)
				{
					foreach (UISprite uisprite2 in componentsInChildren)
					{
						uisprite2.color = Color.red;
					}
				}
				else
				{
					foreach (UISprite uisprite3 in componentsInChildren)
					{
						uisprite3.color = Color.white;
					}
				}
			}
		}
		this.RefreshSlider(slider);
	}

	// Token: 0x04002592 RID: 9618
	public const string ControlsSizeKey = "Controls.Size";

	// Token: 0x04002593 RID: 9619
	public ControlSizeView view;

	// Token: 0x04002594 RID: 9620
	public UISprite _currentSprite;
}
