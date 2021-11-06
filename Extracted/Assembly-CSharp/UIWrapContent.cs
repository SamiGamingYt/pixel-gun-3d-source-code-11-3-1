﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000355 RID: 853
[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class UIWrapContent : MonoBehaviour
{
	// Token: 0x06001D4B RID: 7499 RVA: 0x0007D4CC File Offset: 0x0007B6CC
	protected virtual void Start()
	{
		this.SortBasedOnScrollMovement();
		this.WrapContent();
		if (this.mScroll != null)
		{
			this.mScroll.GetComponent<UIPanel>().onClipMove = new UIPanel.OnClippingMoved(this.OnMove);
		}
		this.mFirstTime = false;
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x0007D51C File Offset: 0x0007B71C
	protected virtual void OnMove(UIPanel panel)
	{
		this.WrapContent();
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x0007D524 File Offset: 0x0007B724
	[ContextMenu("Sort Based on Scroll Movement")]
	public virtual void SortBasedOnScrollMovement()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			this.mChildren.Add(this.mTrans.GetChild(i));
		}
		if (this.mHorizontal)
		{
			this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
		}
		else
		{
			this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortVertical));
		}
		this.ResetChildPositions();
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x0007D5C0 File Offset: 0x0007B7C0
	[ContextMenu("Sort Alphabetically")]
	public virtual void SortAlphabetically()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			this.mChildren.Add(this.mTrans.GetChild(i));
		}
		this.mChildren.Sort(new Comparison<Transform>(UIGrid.SortByName));
		this.ResetChildPositions();
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x0007D634 File Offset: 0x0007B834
	protected bool CacheScrollView()
	{
		this.mTrans = base.transform;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		this.mScroll = this.mPanel.GetComponent<UIScrollView>();
		if (this.mScroll == null)
		{
			return false;
		}
		if (this.mScroll.movement == UIScrollView.Movement.Horizontal)
		{
			this.mHorizontal = true;
		}
		else
		{
			if (this.mScroll.movement != UIScrollView.Movement.Vertical)
			{
				return false;
			}
			this.mHorizontal = false;
		}
		return true;
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x0007D6C0 File Offset: 0x0007B8C0
	protected virtual void ResetChildPositions()
	{
		int i = 0;
		int count = this.mChildren.Count;
		while (i < count)
		{
			Transform transform = this.mChildren[i];
			transform.localPosition = ((!this.mHorizontal) ? new Vector3(0f, (float)(-(float)i * this.itemSize), 0f) : new Vector3((float)(i * this.itemSize), 0f, 0f));
			this.UpdateItem(transform, i);
			i++;
		}
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x0007D748 File Offset: 0x0007B948
	public virtual void WrapContent()
	{
		if (this.mPanel == null)
		{
			return;
		}
		float num = (float)(this.itemSize * this.mChildren.Count) * 0.5f;
		Vector3[] worldCorners = this.mPanel.worldCorners;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector = worldCorners[i];
			vector = this.mTrans.InverseTransformPoint(vector);
			worldCorners[i] = vector;
		}
		Vector3 vector2 = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
		bool flag = true;
		float num2 = num * 2f;
		if (this.mHorizontal)
		{
			float num3 = worldCorners[0].x - (float)this.itemSize;
			float num4 = worldCorners[2].x + (float)this.itemSize;
			int j = 0;
			int count = this.mChildren.Count;
			while (j < count)
			{
				Transform transform = this.mChildren[j];
				if (!(transform == null))
				{
					float num5 = transform.localPosition.x - vector2.x;
					if (num5 < -num)
					{
						Vector3 localPosition = transform.localPosition;
						localPosition.x += num2;
						num5 = localPosition.x - vector2.x;
						int num6 = Mathf.RoundToInt(localPosition.x / (float)this.itemSize);
						if (this.minIndex == this.maxIndex || (this.minIndex <= num6 && num6 <= this.maxIndex))
						{
							transform.localPosition = localPosition;
							this.UpdateItem(transform, j);
						}
						else
						{
							flag = false;
						}
					}
					else if (num5 > num)
					{
						Vector3 localPosition2 = transform.localPosition;
						localPosition2.x -= num2;
						num5 = localPosition2.x - vector2.x;
						int num7 = Mathf.RoundToInt(localPosition2.x / (float)this.itemSize);
						if (this.minIndex == this.maxIndex || (this.minIndex <= num7 && num7 <= this.maxIndex))
						{
							transform.localPosition = localPosition2;
							this.UpdateItem(transform, j);
						}
						else
						{
							flag = false;
						}
					}
					else if (this.mFirstTime)
					{
						this.UpdateItem(transform, j);
					}
					if (this.cullContent)
					{
						num5 += this.mPanel.clipOffset.x - this.mTrans.localPosition.x;
						if (!UICamera.IsPressed(transform.gameObject))
						{
							NGUITools.SetActive(transform.gameObject, num5 > num3 && num5 < num4, false);
						}
					}
				}
				j++;
			}
		}
		else
		{
			float num8 = worldCorners[0].y - (float)this.itemSize;
			float num9 = worldCorners[2].y + (float)this.itemSize;
			int k = 0;
			int count2 = this.mChildren.Count;
			while (k < count2)
			{
				Transform transform2 = this.mChildren[k];
				float num10 = transform2.localPosition.y - vector2.y;
				if (num10 < -num)
				{
					Vector3 localPosition3 = transform2.localPosition;
					localPosition3.y += num2;
					num10 = localPosition3.y - vector2.y;
					int num11 = Mathf.RoundToInt(localPosition3.y / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num11 && num11 <= this.maxIndex))
					{
						transform2.localPosition = localPosition3;
						this.UpdateItem(transform2, k);
					}
					else
					{
						flag = false;
					}
				}
				else if (num10 > num)
				{
					Vector3 localPosition4 = transform2.localPosition;
					localPosition4.y -= num2;
					num10 = localPosition4.y - vector2.y;
					int num12 = Mathf.RoundToInt(localPosition4.y / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num12 && num12 <= this.maxIndex))
					{
						transform2.localPosition = localPosition4;
						this.UpdateItem(transform2, k);
					}
					else
					{
						flag = false;
					}
				}
				else if (this.mFirstTime)
				{
					this.UpdateItem(transform2, k);
				}
				if (this.cullContent)
				{
					num10 += this.mPanel.clipOffset.y - this.mTrans.localPosition.y;
					if (!UICamera.IsPressed(transform2.gameObject))
					{
						NGUITools.SetActive(transform2.gameObject, num10 > num8 && num10 < num9, false);
					}
				}
				k++;
			}
		}
		this.mScroll.restrictWithinPanel = !flag;
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x0007DC64 File Offset: 0x0007BE64
	private void OnValidate()
	{
		if (this.maxIndex < this.minIndex)
		{
			this.maxIndex = this.minIndex;
		}
		if (this.minIndex > this.maxIndex)
		{
			this.maxIndex = this.minIndex;
		}
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x0007DCAC File Offset: 0x0007BEAC
	protected virtual void UpdateItem(Transform item, int index)
	{
		if (this.onInitializeItem != null)
		{
			int realIndex = (this.mScroll.movement != UIScrollView.Movement.Vertical) ? Mathf.RoundToInt(item.localPosition.x / (float)this.itemSize) : Mathf.RoundToInt(item.localPosition.y / (float)this.itemSize);
			this.onInitializeItem(item.gameObject, index, realIndex);
		}
	}

	// Token: 0x04001288 RID: 4744
	public int itemSize = 100;

	// Token: 0x04001289 RID: 4745
	public bool cullContent = true;

	// Token: 0x0400128A RID: 4746
	public int minIndex;

	// Token: 0x0400128B RID: 4747
	public int maxIndex;

	// Token: 0x0400128C RID: 4748
	public UIWrapContent.OnInitializeItem onInitializeItem;

	// Token: 0x0400128D RID: 4749
	protected Transform mTrans;

	// Token: 0x0400128E RID: 4750
	protected UIPanel mPanel;

	// Token: 0x0400128F RID: 4751
	protected UIScrollView mScroll;

	// Token: 0x04001290 RID: 4752
	protected bool mHorizontal;

	// Token: 0x04001291 RID: 4753
	protected bool mFirstTime = true;

	// Token: 0x04001292 RID: 4754
	protected List<Transform> mChildren = new List<Transform>();

	// Token: 0x020008F6 RID: 2294
	// (Invoke) Token: 0x06005078 RID: 20600
	public delegate void OnInitializeItem(GameObject go, int wrapIndex, int realIndex);
}
