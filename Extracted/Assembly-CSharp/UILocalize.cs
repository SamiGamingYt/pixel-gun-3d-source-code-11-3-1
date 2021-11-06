using System;
using UnityEngine;

// Token: 0x020003AC RID: 940
[RequireComponent(typeof(UIWidget))]
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Localize")]
public class UILocalize : MonoBehaviour
{
	// Token: 0x170005FB RID: 1531
	// (set) Token: 0x06002193 RID: 8595 RVA: 0x0009FA18 File Offset: 0x0009DC18
	public string value
	{
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				UIWidget component = base.GetComponent<UIWidget>();
				UILabel uilabel = component as UILabel;
				UISprite uisprite = component as UISprite;
				if (uilabel != null)
				{
					UIInput uiinput = NGUITools.FindInParents<UIInput>(uilabel.gameObject);
					if (uiinput != null && uiinput.label == uilabel)
					{
						uiinput.defaultText = value;
					}
					else
					{
						uilabel.text = value;
					}
				}
				else if (uisprite != null)
				{
					UIButton uibutton = NGUITools.FindInParents<UIButton>(uisprite.gameObject);
					if (uibutton != null && uibutton.tweenTarget == uisprite.gameObject)
					{
						uibutton.normalSprite = value;
					}
					uisprite.spriteName = value;
					uisprite.MakePixelPerfect();
				}
			}
		}
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x0009FAE4 File Offset: 0x0009DCE4
	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnLocalize();
		}
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x0009FAF8 File Offset: 0x0009DCF8
	private void Start()
	{
		this.mStarted = true;
		this.OnLocalize();
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x0009FB08 File Offset: 0x0009DD08
	private void OnLocalize()
	{
		if (string.IsNullOrEmpty(this.key))
		{
			UILabel component = base.GetComponent<UILabel>();
			if (component != null)
			{
				this.key = component.text;
			}
		}
		if (!string.IsNullOrEmpty(this.key))
		{
			this.value = Localization.Get(this.key);
		}
	}

	// Token: 0x040015C0 RID: 5568
	public string key;

	// Token: 0x040015C1 RID: 5569
	private bool mStarted;
}
