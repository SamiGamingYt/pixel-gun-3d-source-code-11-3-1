using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000352 RID: 850
[AddComponentMenu("NGUI/Interaction/Toggled Components")]
[RequireComponent(typeof(UIToggle))]
[ExecuteInEditMode]
public class UIToggledComponents : MonoBehaviour
{
	// Token: 0x06001D43 RID: 7491 RVA: 0x0007D22C File Offset: 0x0007B42C
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

	// Token: 0x06001D44 RID: 7492 RVA: 0x0007D2C8 File Offset: 0x0007B4C8
	public void Toggle()
	{
		if (base.enabled)
		{
			for (int i = 0; i < this.activate.Count; i++)
			{
				MonoBehaviour monoBehaviour = this.activate[i];
				monoBehaviour.enabled = UIToggle.current.value;
			}
			for (int j = 0; j < this.deactivate.Count; j++)
			{
				MonoBehaviour monoBehaviour2 = this.deactivate[j];
				monoBehaviour2.enabled = !UIToggle.current.value;
			}
		}
	}

	// Token: 0x04001280 RID: 4736
	public List<MonoBehaviour> activate;

	// Token: 0x04001281 RID: 4737
	public List<MonoBehaviour> deactivate;

	// Token: 0x04001282 RID: 4738
	[HideInInspector]
	[SerializeField]
	private MonoBehaviour target;

	// Token: 0x04001283 RID: 4739
	[SerializeField]
	[HideInInspector]
	private bool inverse;
}
