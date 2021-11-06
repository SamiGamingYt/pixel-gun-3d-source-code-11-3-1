using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000439 RID: 1081
public class CullArea : MonoBehaviour
{
	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06002686 RID: 9862 RVA: 0x000C0E20 File Offset: 0x000BF020
	// (set) Token: 0x06002687 RID: 9863 RVA: 0x000C0E28 File Offset: 0x000BF028
	public int CellCount { get; private set; }

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06002688 RID: 9864 RVA: 0x000C0E34 File Offset: 0x000BF034
	// (set) Token: 0x06002689 RID: 9865 RVA: 0x000C0E3C File Offset: 0x000BF03C
	public CellTree CellTree { get; private set; }

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x0600268A RID: 9866 RVA: 0x000C0E48 File Offset: 0x000BF048
	// (set) Token: 0x0600268B RID: 9867 RVA: 0x000C0E50 File Offset: 0x000BF050
	public Dictionary<int, GameObject> Map { get; private set; }

	// Token: 0x0600268C RID: 9868 RVA: 0x000C0E5C File Offset: 0x000BF05C
	private void Awake()
	{
		this.idCounter = this.FIRST_GROUP_ID;
		this.CreateCellHierarchy();
	}

	// Token: 0x0600268D RID: 9869 RVA: 0x000C0E70 File Offset: 0x000BF070
	public void OnDrawGizmos()
	{
		this.idCounter = this.FIRST_GROUP_ID;
		if (this.RecreateCellHierarchy)
		{
			this.CreateCellHierarchy();
		}
		this.DrawCells();
	}

	// Token: 0x0600268E RID: 9870 RVA: 0x000C0E98 File Offset: 0x000BF098
	private void CreateCellHierarchy()
	{
		if (!this.IsCellCountAllowed())
		{
			if (Debug.isDebugBuild)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"There are too many cells created by your subdivision options. Maximum allowed number of cells is ",
					250 - this.FIRST_GROUP_ID,
					". Current number of cells is ",
					this.CellCount,
					"."
				}));
				return;
			}
			Application.Quit();
		}
		CellTreeNode cellTreeNode = new CellTreeNode(this.idCounter++, CellTreeNode.ENodeType.Root, null);
		if (this.YIsUpAxis)
		{
			this.Center = new Vector2(base.transform.position.x, base.transform.position.y);
			this.Size = new Vector2(base.transform.localScale.x, base.transform.localScale.y);
			cellTreeNode.Center = new Vector3(this.Center.x, this.Center.y, 0f);
			cellTreeNode.Size = new Vector3(this.Size.x, this.Size.y, 0f);
			cellTreeNode.TopLeft = new Vector3(this.Center.x - this.Size.x / 2f, this.Center.y - this.Size.y / 2f, 0f);
			cellTreeNode.BottomRight = new Vector3(this.Center.x + this.Size.x / 2f, this.Center.y + this.Size.y / 2f, 0f);
		}
		else
		{
			this.Center = new Vector2(base.transform.position.x, base.transform.position.z);
			this.Size = new Vector2(base.transform.localScale.x, base.transform.localScale.z);
			cellTreeNode.Center = new Vector3(this.Center.x, 0f, this.Center.y);
			cellTreeNode.Size = new Vector3(this.Size.x, 0f, this.Size.y);
			cellTreeNode.TopLeft = new Vector3(this.Center.x - this.Size.x / 2f, 0f, this.Center.y - this.Size.y / 2f);
			cellTreeNode.BottomRight = new Vector3(this.Center.x + this.Size.x / 2f, 0f, this.Center.y + this.Size.y / 2f);
		}
		this.CreateChildCells(cellTreeNode, 1);
		this.CellTree = new CellTree(cellTreeNode);
		this.RecreateCellHierarchy = false;
	}

	// Token: 0x0600268F RID: 9871 RVA: 0x000C11D8 File Offset: 0x000BF3D8
	private void CreateChildCells(CellTreeNode parent, int cellLevelInHierarchy)
	{
		if (cellLevelInHierarchy > this.NumberOfSubdivisions)
		{
			return;
		}
		int num = (int)this.Subdivisions[cellLevelInHierarchy - 1].x;
		int num2 = (int)this.Subdivisions[cellLevelInHierarchy - 1].y;
		float num3 = parent.Center.x - parent.Size.x / 2f;
		float num4 = parent.Size.x / (float)num;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				float num5 = num3 + (float)i * num4 + num4 / 2f;
				CellTreeNode cellTreeNode = new CellTreeNode(this.idCounter++, (this.NumberOfSubdivisions != cellLevelInHierarchy) ? CellTreeNode.ENodeType.Node : CellTreeNode.ENodeType.Leaf, parent);
				if (this.YIsUpAxis)
				{
					float num6 = parent.Center.y - parent.Size.y / 2f;
					float num7 = parent.Size.y / (float)num2;
					float num8 = num6 + (float)j * num7 + num7 / 2f;
					cellTreeNode.Center = new Vector3(num5, num8, 0f);
					cellTreeNode.Size = new Vector3(num4, num7, 0f);
					cellTreeNode.TopLeft = new Vector3(num5 - num4 / 2f, num8 - num7 / 2f, 0f);
					cellTreeNode.BottomRight = new Vector3(num5 + num4 / 2f, num8 + num7 / 2f, 0f);
				}
				else
				{
					float num9 = parent.Center.z - parent.Size.z / 2f;
					float num10 = parent.Size.z / (float)num2;
					float num11 = num9 + (float)j * num10 + num10 / 2f;
					cellTreeNode.Center = new Vector3(num5, 0f, num11);
					cellTreeNode.Size = new Vector3(num4, 0f, num10);
					cellTreeNode.TopLeft = new Vector3(num5 - num4 / 2f, 0f, num11 - num10 / 2f);
					cellTreeNode.BottomRight = new Vector3(num5 + num4 / 2f, 0f, num11 + num10 / 2f);
				}
				parent.AddChild(cellTreeNode);
				this.CreateChildCells(cellTreeNode, cellLevelInHierarchy + 1);
			}
		}
	}

	// Token: 0x06002690 RID: 9872 RVA: 0x000C1448 File Offset: 0x000BF648
	private void DrawCells()
	{
		if (this.CellTree != null && this.CellTree.RootNode != null)
		{
			this.CellTree.RootNode.Draw();
		}
		else
		{
			this.RecreateCellHierarchy = true;
		}
	}

	// Token: 0x06002691 RID: 9873 RVA: 0x000C148C File Offset: 0x000BF68C
	private bool IsCellCountAllowed()
	{
		int num = 1;
		int num2 = 1;
		foreach (Vector2 vector in this.Subdivisions)
		{
			num *= (int)vector.x;
			num2 *= (int)vector.y;
		}
		this.CellCount = num * num2;
		return this.CellCount <= 250 - this.FIRST_GROUP_ID;
	}

	// Token: 0x06002692 RID: 9874 RVA: 0x000C1500 File Offset: 0x000BF700
	public List<int> GetActiveCells(Vector3 position)
	{
		List<int> list = new List<int>(0);
		this.CellTree.RootNode.GetInsideCells(list, this.YIsUpAxis, position);
		List<int> list2 = new List<int>(0);
		this.CellTree.RootNode.GetNearbyCells(list2, this.YIsUpAxis, position);
		foreach (int item in list2)
		{
			if (!list.Contains(item))
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x04001B05 RID: 6917
	private const int MAX_NUMBER_OF_ALLOWED_CELLS = 250;

	// Token: 0x04001B06 RID: 6918
	public const int MAX_NUMBER_OF_SUBDIVISIONS = 3;

	// Token: 0x04001B07 RID: 6919
	public readonly int FIRST_GROUP_ID = 1;

	// Token: 0x04001B08 RID: 6920
	public readonly int[] SUBDIVISION_FIRST_LEVEL_ORDER = new int[]
	{
		0,
		1,
		1,
		1
	};

	// Token: 0x04001B09 RID: 6921
	public readonly int[] SUBDIVISION_SECOND_LEVEL_ORDER = new int[]
	{
		0,
		2,
		1,
		2,
		0,
		2,
		1,
		2
	};

	// Token: 0x04001B0A RID: 6922
	public readonly int[] SUBDIVISION_THIRD_LEVEL_ORDER = new int[]
	{
		0,
		3,
		2,
		3,
		1,
		3,
		2,
		3,
		1,
		3,
		2,
		3
	};

	// Token: 0x04001B0B RID: 6923
	public Vector2 Center;

	// Token: 0x04001B0C RID: 6924
	public Vector2 Size = new Vector2(25f, 25f);

	// Token: 0x04001B0D RID: 6925
	public Vector2[] Subdivisions = new Vector2[3];

	// Token: 0x04001B0E RID: 6926
	public int NumberOfSubdivisions;

	// Token: 0x04001B0F RID: 6927
	public bool YIsUpAxis = true;

	// Token: 0x04001B10 RID: 6928
	public bool RecreateCellHierarchy;

	// Token: 0x04001B11 RID: 6929
	private int idCounter;
}
