using System;
using UnityEngine;

// Token: 0x020003CF RID: 975
public class NickLabelStack : MonoBehaviour
{
	// Token: 0x06002361 RID: 9057 RVA: 0x000B04A0 File Offset: 0x000AE6A0
	private void Awake()
	{
		NickLabelStack.sharedStack = this;
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x000B04A8 File Offset: 0x000AE6A8
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		base.transform.localPosition = Vector3.zero;
		Transform transform = base.transform.GetChild(0).transform;
		base.transform.position = Vector3.zero;
		this.lables = new NickLabelController[this.lengthStack];
		this.lables[0] = transform.GetChild(0).GetComponent<NickLabelController>();
		while (transform.childCount < this.lengthStack)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(transform.GetChild(0).gameObject);
			Transform transform2 = gameObject.transform;
			transform2.parent = transform;
			transform2.localPosition = Vector3.zero;
			transform2.localScale = new Vector3(1f, 1f, 1f);
			transform2.rotation = Quaternion.identity;
			this.lables[transform.childCount - 1] = gameObject.GetComponent<NickLabelController>();
		}
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x000B0594 File Offset: 0x000AE794
	public NickLabelController GetNextCurrentLabel()
	{
		base.transform.localPosition = Vector3.zero;
		bool flag = true;
		for (;;)
		{
			this.currentIndexLabel++;
			if (this.currentIndexLabel >= this.lables.Length)
			{
				if (!flag)
				{
					break;
				}
				this.currentIndexLabel = 0;
				flag = false;
			}
			if (!(this.lables[this.currentIndexLabel].target != null))
			{
				goto Block_3;
			}
		}
		return null;
		Block_3:
		this.lables[this.currentIndexLabel].currentType = NickLabelController.TypeNickLabel.None;
		return this.lables[this.currentIndexLabel];
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x000B0624 File Offset: 0x000AE824
	public NickLabelController GetCurrentLabel()
	{
		return this.lables[this.currentIndexLabel];
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x000B0634 File Offset: 0x000AE834
	private void OnDestroy()
	{
		NickLabelStack.sharedStack = null;
	}

	// Token: 0x040017DD RID: 6109
	public static NickLabelStack sharedStack;

	// Token: 0x040017DE RID: 6110
	public int lengthStack = 30;

	// Token: 0x040017DF RID: 6111
	public NickLabelController[] lables;

	// Token: 0x040017E0 RID: 6112
	private int currentIndexLabel;
}
