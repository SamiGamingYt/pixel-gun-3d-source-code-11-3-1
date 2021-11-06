using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000864 RID: 2148
	public class ToggleGroupHalper : MonoBehaviour
	{
		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x06004D98 RID: 19864 RVA: 0x001C0CF8 File Offset: 0x001BEEF8
		// (remove) Token: 0x06004D99 RID: 19865 RVA: 0x001C0D14 File Offset: 0x001BEF14
		public event Action<string, bool> OnSelectedToggleChanged;

		// Token: 0x06004D9A RID: 19866 RVA: 0x001C0D30 File Offset: 0x001BEF30
		private void OnEnable()
		{
			if (this._toggleGroup == 0)
			{
				Debug.LogError("toggle group not setted");
				return;
			}
			foreach (KeyValuePair<UIToggle, EventDelegate> keyValuePair in this._subscribers)
			{
				if (keyValuePair.Key != null && keyValuePair.Value != null)
				{
					keyValuePair.Key.onChange.Remove(keyValuePair.Value);
				}
			}
			this._subscribers.Clear();
			List<UIToggle> toggles = this.GetToggles();
			if (toggles.Count < 1)
			{
				Debug.LogError("toggles not found");
				return;
			}
			foreach (UIToggle uitoggle in this.GetToggles())
			{
				if (!this._subscribers.ContainsKey(uitoggle))
				{
					string name = uitoggle.name.ToString();
					EventDelegate eventDelegate = new EventDelegate(delegate()
					{
						this.OnToggleStateChanged(name);
					});
					this._subscribers.Add(uitoggle, eventDelegate);
					uitoggle.onChange.Add(eventDelegate);
				}
			}
		}

		// Token: 0x06004D9B RID: 19867 RVA: 0x001C0EB8 File Offset: 0x001BF0B8
		public List<UIToggle> GetToggles()
		{
			return (from t in base.gameObject.GetComponentsInChildren<UIToggle>()
			where t.@group == this._toggleGroup
			select t).ToList<UIToggle>();
		}

		// Token: 0x06004D9C RID: 19868 RVA: 0x001C0EE8 File Offset: 0x001BF0E8
		public UIToggle GetSelectedToggle()
		{
			return this.GetToggles().FirstOrDefault((UIToggle t) => t.value);
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x001C0F20 File Offset: 0x001BF120
		private void OnToggleStateChanged(string name)
		{
			if (this.OnSelectedToggleChanged != null)
			{
				UIToggle uitoggle = this.GetToggles().FirstOrDefault((UIToggle t) => t.gameObject.name == name);
				if (uitoggle != null)
				{
					this.OnSelectedToggleChanged(name, uitoggle.value);
				}
			}
		}

		// Token: 0x06004D9E RID: 19870 RVA: 0x001C0F80 File Offset: 0x001BF180
		public void SelectToggle(string goName)
		{
			UIToggle uitoggle = this.GetToggles().FirstOrDefault((UIToggle t) => t.gameObject.name == goName);
			if (uitoggle != null && !uitoggle.value)
			{
				uitoggle.value = true;
			}
		}

		// Token: 0x04003C0B RID: 15371
		[SerializeField]
		private int _toggleGroup;

		// Token: 0x04003C0C RID: 15372
		private Dictionary<UIToggle, EventDelegate> _subscribers = new Dictionary<UIToggle, EventDelegate>();
	}
}
