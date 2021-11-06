using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200043B RID: 1083
public class CellTreeNode
{
	// Token: 0x06002697 RID: 9879 RVA: 0x000C15D8 File Offset: 0x000BF7D8
	public CellTreeNode()
	{
	}

	// Token: 0x06002698 RID: 9880 RVA: 0x000C15E0 File Offset: 0x000BF7E0
	public CellTreeNode(int id, CellTreeNode.ENodeType nodeType, CellTreeNode parent)
	{
		this.Id = id;
		this.NodeType = nodeType;
		this.Parent = parent;
	}

	// Token: 0x06002699 RID: 9881 RVA: 0x000C1600 File Offset: 0x000BF800
	public void AddChild(CellTreeNode child)
	{
		if (this.Childs == null)
		{
			this.Childs = new List<CellTreeNode>(1);
		}
		this.Childs.Add(child);
	}

	// Token: 0x0600269A RID: 9882 RVA: 0x000C1628 File Offset: 0x000BF828
	public void Draw()
	{
	}

	// Token: 0x0600269B RID: 9883 RVA: 0x000C162C File Offset: 0x000BF82C
	public void GetAllLeafNodes(List<CellTreeNode> leafNodes)
	{
		if (this.Childs != null)
		{
			foreach (CellTreeNode cellTreeNode in this.Childs)
			{
				cellTreeNode.GetAllLeafNodes(leafNodes);
			}
		}
		else
		{
			leafNodes.Add(this);
		}
	}

	// Token: 0x0600269C RID: 9884 RVA: 0x000C16AC File Offset: 0x000BF8AC
	public void GetInsideCells(List<int> insideCells, bool yIsUpAxis, Vector3 position)
	{
		if (this.IsPointInsideCell(yIsUpAxis, position))
		{
			insideCells.Add(this.Id);
			if (this.Childs != null)
			{
				foreach (CellTreeNode cellTreeNode in this.Childs)
				{
					cellTreeNode.GetInsideCells(insideCells, yIsUpAxis, position);
				}
			}
		}
	}

	// Token: 0x0600269D RID: 9885 RVA: 0x000C1738 File Offset: 0x000BF938
	public void GetNearbyCells(List<int> nearbyCells, bool yIsUpAxis, Vector3 position)
	{
		if (this.IsPointNearCell(yIsUpAxis, position))
		{
			if (this.NodeType != CellTreeNode.ENodeType.Leaf)
			{
				foreach (CellTreeNode cellTreeNode in this.Childs)
				{
					cellTreeNode.GetNearbyCells(nearbyCells, yIsUpAxis, position);
				}
			}
			else
			{
				nearbyCells.Add(this.Id);
			}
		}
	}

	// Token: 0x0600269E RID: 9886 RVA: 0x000C17CC File Offset: 0x000BF9CC
	public bool IsPointInsideCell(bool yIsUpAxis, Vector3 point)
	{
		if (point.x < this.TopLeft.x || point.x > this.BottomRight.x)
		{
			return false;
		}
		if (yIsUpAxis)
		{
			if (point.y >= this.TopLeft.y && point.y <= this.BottomRight.y)
			{
				return true;
			}
		}
		else if (point.z >= this.TopLeft.z && point.z <= this.BottomRight.z)
		{
			return true;
		}
		return false;
	}

	// Token: 0x0600269F RID: 9887 RVA: 0x000C1878 File Offset: 0x000BFA78
	public bool IsPointNearCell(bool yIsUpAxis, Vector3 point)
	{
		if (this.maxDistance == 0f)
		{
			this.maxDistance = (this.Size.x + this.Size.y + this.Size.z) / 2f;
		}
		return (point - this.Center).sqrMagnitude <= this.maxDistance * this.maxDistance;
	}

	// Token: 0x04001B16 RID: 6934
	public int Id;

	// Token: 0x04001B17 RID: 6935
	public Vector3 Center;

	// Token: 0x04001B18 RID: 6936
	public Vector3 Size;

	// Token: 0x04001B19 RID: 6937
	public Vector3 TopLeft;

	// Token: 0x04001B1A RID: 6938
	public Vector3 BottomRight;

	// Token: 0x04001B1B RID: 6939
	public CellTreeNode.ENodeType NodeType;

	// Token: 0x04001B1C RID: 6940
	public CellTreeNode Parent;

	// Token: 0x04001B1D RID: 6941
	public List<CellTreeNode> Childs;

	// Token: 0x04001B1E RID: 6942
	private float maxDistance;

	// Token: 0x0200043C RID: 1084
	public enum ENodeType
	{
		// Token: 0x04001B20 RID: 6944
		Root,
		// Token: 0x04001B21 RID: 6945
		Node,
		// Token: 0x04001B22 RID: 6946
		Leaf
	}
}
