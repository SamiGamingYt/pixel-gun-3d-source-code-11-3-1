using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003B0 RID: 944
[AddComponentMenu("NGUI/UI/Root")]
[ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x060021E8 RID: 8680 RVA: 0x000A2848 File Offset: 0x000A0A48
	public UIRoot.Constraint constraint
	{
		get
		{
			if (this.fitWidth)
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.Fit;
				}
				return UIRoot.Constraint.FitWidth;
			}
			else
			{
				if (this.fitHeight)
				{
					return UIRoot.Constraint.FitHeight;
				}
				return UIRoot.Constraint.Fill;
			}
		}
	}

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x060021E9 RID: 8681 RVA: 0x000A2880 File Offset: 0x000A0A80
	public UIRoot.Scaling activeScaling
	{
		get
		{
			UIRoot.Scaling scaling = this.scalingStyle;
			if (scaling == UIRoot.Scaling.ConstrainedOnMobiles)
			{
				return UIRoot.Scaling.Constrained;
			}
			return scaling;
		}
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x060021EA RID: 8682 RVA: 0x000A28A0 File Offset: 0x000A0AA0
	public int activeHeight
	{
		get
		{
			if (this.activeScaling == UIRoot.Scaling.Flexible)
			{
				Vector2 screenSize = NGUITools.screenSize;
				float num = screenSize.x / screenSize.y;
				if (screenSize.y < (float)this.minimumHeight)
				{
					screenSize.y = (float)this.minimumHeight;
					screenSize.x = screenSize.y * num;
				}
				else if (screenSize.y > (float)this.maximumHeight)
				{
					screenSize.y = (float)this.maximumHeight;
					screenSize.x = screenSize.y * num;
				}
				int num2 = Mathf.RoundToInt((!this.shrinkPortraitUI || screenSize.y <= screenSize.x) ? screenSize.y : (screenSize.y / num));
				return (!this.adjustByDPI) ? num2 : NGUIMath.AdjustByDPI((float)num2);
			}
			UIRoot.Constraint constraint = this.constraint;
			if (constraint == UIRoot.Constraint.FitHeight)
			{
				return this.manualHeight;
			}
			Vector2 screenSize2 = NGUITools.screenSize;
			float num3 = screenSize2.x / screenSize2.y;
			float num4 = (float)this.manualWidth / (float)this.manualHeight;
			switch (constraint)
			{
			case UIRoot.Constraint.Fit:
				return (num4 <= num3) ? this.manualHeight : Mathf.RoundToInt((float)this.manualWidth / num3);
			case UIRoot.Constraint.Fill:
				return (num4 >= num3) ? this.manualHeight : Mathf.RoundToInt((float)this.manualWidth / num3);
			case UIRoot.Constraint.FitWidth:
				return Mathf.RoundToInt((float)this.manualWidth / num3);
			default:
				return this.manualHeight;
			}
		}
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x060021EB RID: 8683 RVA: 0x000A2A44 File Offset: 0x000A0C44
	public float pixelSizeAdjustment
	{
		get
		{
			int num = Mathf.RoundToInt(NGUITools.screenSize.y);
			return (num != -1) ? this.GetPixelSizeAdjustment(num) : 1f;
		}
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x000A2A7C File Offset: 0x000A0C7C
	public static float GetPixelSizeAdjustment(GameObject go)
	{
		UIRoot uiroot = NGUITools.FindInParents<UIRoot>(go);
		return (!(uiroot != null)) ? 1f : uiroot.pixelSizeAdjustment;
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x000A2AAC File Offset: 0x000A0CAC
	public float GetPixelSizeAdjustment(int height)
	{
		height = Mathf.Max(2, height);
		if (this.activeScaling == UIRoot.Scaling.Constrained)
		{
			return (float)this.activeHeight / (float)height;
		}
		if (height < this.minimumHeight)
		{
			return (float)this.minimumHeight / (float)height;
		}
		if (height > this.maximumHeight)
		{
			return (float)this.maximumHeight / (float)height;
		}
		return 1f;
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000A2B0C File Offset: 0x000A0D0C
	protected virtual void Awake()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x000A2B1C File Offset: 0x000A0D1C
	protected virtual void OnEnable()
	{
		UIRoot.list.Add(this);
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000A2B2C File Offset: 0x000A0D2C
	protected virtual void OnDisable()
	{
		UIRoot.list.Remove(this);
	}

	// Token: 0x060021F1 RID: 8689 RVA: 0x000A2B3C File Offset: 0x000A0D3C
	protected virtual void Start()
	{
		UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
		if (componentInChildren != null)
		{
			Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
			Camera component = componentInChildren.gameObject.GetComponent<Camera>();
			componentInChildren.enabled = false;
			if (component != null)
			{
				component.orthographicSize = 1f;
			}
		}
		else
		{
			this.UpdateScale(false);
		}
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000A2BA0 File Offset: 0x000A0DA0
	private void Update()
	{
		this.UpdateScale(true);
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x000A2BAC File Offset: 0x000A0DAC
	public void UpdateScale(bool updateAnchors = true)
	{
		if (this.mTrans != null)
		{
			float num = (float)this.activeHeight;
			if (num > 0f)
			{
				float num2 = 2f / num;
				Vector3 localScale = this.mTrans.localScale;
				if (Mathf.Abs(localScale.x - num2) > 1E-45f || Mathf.Abs(localScale.y - num2) > 1E-45f || Mathf.Abs(localScale.z - num2) > 1E-45f)
				{
					this.mTrans.localScale = new Vector3(num2, num2, num2);
					if (updateAnchors)
					{
						base.BroadcastMessage("UpdateAnchors");
					}
				}
			}
		}
	}

	// Token: 0x060021F4 RID: 8692 RVA: 0x000A2C60 File Offset: 0x000A0E60
	public static void Broadcast(string funcName)
	{
		int i = 0;
		int count = UIRoot.list.Count;
		while (i < count)
		{
			UIRoot uiroot = UIRoot.list[i];
			if (uiroot != null)
			{
				uiroot.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
			}
			i++;
		}
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000A2CAC File Offset: 0x000A0EAC
	public static void Broadcast(string funcName, object param)
	{
		if (param == null)
		{
			Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
		}
		else
		{
			int i = 0;
			int count = UIRoot.list.Count;
			while (i < count)
			{
				UIRoot uiroot = UIRoot.list[i];
				if (uiroot != null)
				{
					uiroot.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
				}
				i++;
			}
		}
	}

	// Token: 0x040015F0 RID: 5616
	public static List<UIRoot> list = new List<UIRoot>();

	// Token: 0x040015F1 RID: 5617
	public UIRoot.Scaling scalingStyle;

	// Token: 0x040015F2 RID: 5618
	public int manualWidth = 1280;

	// Token: 0x040015F3 RID: 5619
	public int manualHeight = 720;

	// Token: 0x040015F4 RID: 5620
	public int minimumHeight = 320;

	// Token: 0x040015F5 RID: 5621
	public int maximumHeight = 1536;

	// Token: 0x040015F6 RID: 5622
	public bool fitWidth;

	// Token: 0x040015F7 RID: 5623
	public bool fitHeight = true;

	// Token: 0x040015F8 RID: 5624
	public bool adjustByDPI;

	// Token: 0x040015F9 RID: 5625
	public bool shrinkPortraitUI;

	// Token: 0x040015FA RID: 5626
	private Transform mTrans;

	// Token: 0x020003B1 RID: 945
	public enum Scaling
	{
		// Token: 0x040015FC RID: 5628
		Flexible,
		// Token: 0x040015FD RID: 5629
		Constrained,
		// Token: 0x040015FE RID: 5630
		ConstrainedOnMobiles
	}

	// Token: 0x020003B2 RID: 946
	public enum Constraint
	{
		// Token: 0x04001600 RID: 5632
		Fit,
		// Token: 0x04001601 RID: 5633
		Fill,
		// Token: 0x04001602 RID: 5634
		FitWidth,
		// Token: 0x04001603 RID: 5635
		FitHeight
	}
}
