using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000353 RID: 851
[AddComponentMenu("NGUI/Interaction/Toggled Objects")]
public class UIToggledObjects : MonoBehaviour
{
	// Token: 0x06001D46 RID: 7494 RVA: 0x0007D360 File Offset: 0x0007B560
	private void Awake()
	{
		if (this.target != null)
		{
			if (this.activate.Count == 0 && this.deactivate.Count == 0)
			{
				if (this.inverse)
				{
					this.deactivate.Add(this.target);
				}
				else
				{
					this.activate.Add(this.target);
				}
			}
			else
			{
				this.target = null;
			}
		}
		UIToggle component = base.GetComponent<UIToggle>();
		EventDelegate.Add(component.onChange, new EventDelegate.Callback(this.Toggle));
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x0007D3FC File Offset: 0x0007B5FC
	public void Toggle()
	{
		bool value = UIToggle.current.value;
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				this.Set(this.activate[i], value);
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				this.Set(this.deactivate[j], !value);
			}
		}
	}

	// Token: 0x06001D48 RID: 7496 RVA: 0x0007D480 File Offset: 0x0007B680
	private void Set(GameObject go, bool state)
	{
		if (go != null)
		{
			NGUITools.SetActive(go, state);
		}
	}

	// Token: 0x04001284 RID: 4740
	public List<GameObject> activate;

	// Token: 0x04001285 RID: 4741
	public List<GameObject> deactivate;

	// Token: 0x04001286 RID: 4742
	[HideInInspector]
	[SerializeField]
	private GameObject target;

	// Token: 0x04001287 RID: 4743
	[HideInInspector]
	[SerializeField]
	private bool inverse;
}
