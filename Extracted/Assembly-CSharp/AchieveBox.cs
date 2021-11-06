using System;
using UnityEngine;

// Token: 0x02000672 RID: 1650
public class AchieveBox : MonoBehaviour
{
	// Token: 0x06003967 RID: 14695 RVA: 0x0012A6B0 File Offset: 0x001288B0
	private void Awake()
	{
		this.mySprite = base.GetComponent<UISprite>();
		this.hidePos = base.transform.localPosition;
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x0012A6D0 File Offset: 0x001288D0
	private void OnEnable()
	{
		if (!this._selfOpened)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			this._selfOpened = false;
		}
	}

	// Token: 0x06003969 RID: 14697 RVA: 0x0012A6F8 File Offset: 0x001288F8
	private void OnDisable()
	{
		base.transform.localPosition = this.hidePos;
		this.isOpened = false;
		this._selfOpened = false;
	}

	// Token: 0x0600396A RID: 14698 RVA: 0x0012A71C File Offset: 0x0012891C
	public void ShowBox()
	{
		this._selfOpened = true;
		base.gameObject.SetActive(true);
		this.toggled = true;
		this.posToMove = this.hidePos + Vector3.down * (float)this.mySprite.height;
	}

	// Token: 0x0600396B RID: 14699 RVA: 0x0012A76C File Offset: 0x0012896C
	public void HideBox()
	{
		this.toggled = true;
		this.posToMove = this.hidePos;
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x0012A784 File Offset: 0x00128984
	private void Update()
	{
		if (!this.toggled)
		{
			return;
		}
		if (!(base.transform.localPosition == this.posToMove))
		{
			base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, this.posToMove, this.speed * RealTime.deltaTime);
		}
		else
		{
			this.toggled = false;
			this.isOpened = !this.isOpened;
			if (!this.isOpened)
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x04002A25 RID: 10789
	private Vector3 posToMove;

	// Token: 0x04002A26 RID: 10790
	private Vector3 hidePos;

	// Token: 0x04002A27 RID: 10791
	private UISprite mySprite;

	// Token: 0x04002A28 RID: 10792
	[HideInInspector]
	public bool isOpened;

	// Token: 0x04002A29 RID: 10793
	private bool toggled;

	// Token: 0x04002A2A RID: 10794
	public float speed = 300f;

	// Token: 0x04002A2B RID: 10795
	private bool _selfOpened;
}
