using System;

// Token: 0x0200043A RID: 1082
public class CellTree
{
	// Token: 0x06002693 RID: 9875 RVA: 0x000C15AC File Offset: 0x000BF7AC
	public CellTree()
	{
	}

	// Token: 0x06002694 RID: 9876 RVA: 0x000C15B4 File Offset: 0x000BF7B4
	public CellTree(CellTreeNode root)
	{
		this.RootNode = root;
	}

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x06002695 RID: 9877 RVA: 0x000C15C4 File Offset: 0x000BF7C4
	// (set) Token: 0x06002696 RID: 9878 RVA: 0x000C15CC File Offset: 0x000BF7CC
	public CellTreeNode RootNode { get; private set; }
}
