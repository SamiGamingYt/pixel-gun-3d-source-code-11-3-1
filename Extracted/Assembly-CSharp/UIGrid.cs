using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000331 RID: 817
[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
	// Token: 0x170004CD RID: 1229
	// (set) Token: 0x06001C35 RID: 7221 RVA: 0x000752CC File Offset: 0x000734CC
	public bool repositionNow
	{
		set
		{
			if (value)
			{
				this.mReposition = true;
				base.enabled = true;
			}
		}
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000752E4 File Offset: 0x000734E4
	public List<Transform> GetChildList()
	{
		Transform transform = base.transform;
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!this.hideInactive || (child && NGUITools.GetActive(child.gameObject)))
			{
				list.Add(child);
			}
		}
		if (this.sorting != UIGrid.Sorting.None && this.arrangement != UIGrid.Arrangement.CellSnap)
		{
			if (this.sorting == UIGrid.Sorting.Alphabetic)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortByName));
			}
			else if (this.sorting == UIGrid.Sorting.Horizontal)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortHorizontal));
			}
			else if (this.sorting == UIGrid.Sorting.Vertical)
			{
				list.Sort(new Comparison<Transform>(UIGrid.SortVertical));
			}
			else if (this.onCustomSort != null)
			{
				list.Sort(this.onCustomSort);
			}
			else
			{
				this.Sort(list);
			}
		}
		return list;
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x000753F0 File Offset: 0x000735F0
	public Transform GetChild(int index)
	{
		List<Transform> childList = this.GetChildList();
		return (index >= childList.Count) ? null : childList[index];
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x00075420 File Offset: 0x00073620
	public int GetIndex(Transform trans)
	{
		return this.GetChildList().IndexOf(trans);
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x00075430 File Offset: 0x00073630
	public void AddChild(Transform trans)
	{
		this.AddChild(trans, true);
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x0007543C File Offset: 0x0007363C
	public void AddChild(Transform trans, bool sort)
	{
		if (trans != null)
		{
			trans.parent = base.transform;
			this.ResetPosition(this.GetChildList());
		}
	}

	// Token: 0x06001C3B RID: 7227 RVA: 0x00075470 File Offset: 0x00073670
	public bool RemoveChild(Transform t)
	{
		List<Transform> childList = this.GetChildList();
		if (childList.Remove(t))
		{
			this.ResetPosition(childList);
			return true;
		}
		return false;
	}

	// Token: 0x06001C3C RID: 7228 RVA: 0x0007549C File Offset: 0x0007369C
	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x000754B8 File Offset: 0x000736B8
	protected virtual void Start()
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		bool flag = this.animateSmoothly;
		this.animateSmoothly = false;
		this.Reposition();
		this.animateSmoothly = flag;
		base.enabled = false;
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x000754F8 File Offset: 0x000736F8
	protected virtual void Update()
	{
		this.Reposition();
		base.enabled = false;
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x00075508 File Offset: 0x00073708
	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x00075528 File Offset: 0x00073728
	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x0007553C File Offset: 0x0007373C
	public static int SortHorizontal(Transform a, Transform b)
	{
		return a.localPosition.x.CompareTo(b.localPosition.x);
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x0007556C File Offset: 0x0007376C
	public static int SortVertical(Transform a, Transform b)
	{
		return b.localPosition.y.CompareTo(a.localPosition.y);
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x0007559C File Offset: 0x0007379C
	protected virtual void Sort(List<Transform> list)
	{
	}

	// Token: 0x06001C44 RID: 7236 RVA: 0x000755A0 File Offset: 0x000737A0
	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(base.gameObject))
		{
			this.Init();
		}
		if (this.sorted)
		{
			this.sorted = false;
			if (this.sorting == UIGrid.Sorting.None)
			{
				this.sorting = UIGrid.Sorting.Alphabetic;
			}
			NGUITools.SetDirty(this);
		}
		List<Transform> childList = this.GetChildList();
		this.ResetPosition(childList);
		if (this.keepWithinPanel)
		{
			this.ConstrainWithinPanel();
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x00075638 File Offset: 0x00073838
	public void ConstrainWithinPanel()
	{
		if (this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(base.transform, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x00075688 File Offset: 0x00073888
	protected virtual void ResetPosition(List<Transform> list)
	{
		this.mReposition = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		Transform transform = base.transform;
		int i = 0;
		int count = list.Count;
		while (i < count)
		{
			Transform transform2 = list[i];
			Vector3 vector = transform2.localPosition;
			float z = vector.z;
			if (this.arrangement == UIGrid.Arrangement.CellSnap)
			{
				if (this.cellWidth > 0f)
				{
					vector.x = Mathf.Round(vector.x / this.cellWidth) * this.cellWidth;
				}
				if (this.cellHeight > 0f)
				{
					vector.y = Mathf.Round(vector.y / this.cellHeight) * this.cellHeight;
				}
			}
			else
			{
				vector = ((this.arrangement != UIGrid.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z));
			}
			if (this.animateSmoothly && Application.isPlaying && Vector3.SqrMagnitude(transform2.localPosition - vector) >= 0.0001f)
			{
				SpringPosition springPosition = SpringPosition.Begin(transform2.gameObject, vector, 15f);
				springPosition.updateScrollView = true;
				springPosition.ignoreTimeScale = true;
			}
			else
			{
				transform2.localPosition = vector;
			}
			num3 = Mathf.Max(num3, num);
			num4 = Mathf.Max(num4, num2);
			if (++num >= this.maxPerLine && this.maxPerLine > 0)
			{
				num = 0;
				num2++;
			}
			i++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			float num5;
			float num6;
			if (this.arrangement == UIGrid.Arrangement.Horizontal)
			{
				num5 = Mathf.Lerp(0f, (float)num3 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num4) * this.cellHeight, 0f, pivotOffset.y);
			}
			else
			{
				num5 = Mathf.Lerp(0f, (float)num4 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num3) * this.cellHeight, 0f, pivotOffset.y);
			}
			for (int j = 0; j < transform.childCount; j++)
			{
				Transform child = transform.GetChild(j);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					SpringPosition springPosition2 = component;
					springPosition2.target.x = springPosition2.target.x - num5;
					SpringPosition springPosition3 = component;
					springPosition3.target.y = springPosition3.target.y - num6;
				}
				else
				{
					Vector3 localPosition = child.localPosition;
					localPosition.x -= num5;
					localPosition.y -= num6;
					child.localPosition = localPosition;
				}
			}
		}
	}

	// Token: 0x04001159 RID: 4441
	public UIGrid.Arrangement arrangement;

	// Token: 0x0400115A RID: 4442
	public UIGrid.Sorting sorting;

	// Token: 0x0400115B RID: 4443
	public UIWidget.Pivot pivot;

	// Token: 0x0400115C RID: 4444
	public int maxPerLine;

	// Token: 0x0400115D RID: 4445
	public float cellWidth = 200f;

	// Token: 0x0400115E RID: 4446
	public float cellHeight = 200f;

	// Token: 0x0400115F RID: 4447
	public bool animateSmoothly;

	// Token: 0x04001160 RID: 4448
	public bool hideInactive;

	// Token: 0x04001161 RID: 4449
	public bool keepWithinPanel;

	// Token: 0x04001162 RID: 4450
	public UIGrid.OnReposition onReposition;

	// Token: 0x04001163 RID: 4451
	public Comparison<Transform> onCustomSort;

	// Token: 0x04001164 RID: 4452
	[SerializeField]
	[HideInInspector]
	private bool sorted;

	// Token: 0x04001165 RID: 4453
	protected bool mReposition;

	// Token: 0x04001166 RID: 4454
	protected UIPanel mPanel;

	// Token: 0x04001167 RID: 4455
	protected bool mInitDone;

	// Token: 0x02000332 RID: 818
	public enum Arrangement
	{
		// Token: 0x04001169 RID: 4457
		Horizontal,
		// Token: 0x0400116A RID: 4458
		Vertical,
		// Token: 0x0400116B RID: 4459
		CellSnap
	}

	// Token: 0x02000333 RID: 819
	public enum Sorting
	{
		// Token: 0x0400116D RID: 4461
		None,
		// Token: 0x0400116E RID: 4462
		Alphabetic,
		// Token: 0x0400116F RID: 4463
		Horizontal,
		// Token: 0x04001170 RID: 4464
		Vertical,
		// Token: 0x04001171 RID: 4465
		Custom
	}

	// Token: 0x020008F0 RID: 2288
	// (Invoke) Token: 0x06005060 RID: 20576
	public delegate void OnReposition();
}
