using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000444 RID: 1092
[RequireComponent(typeof(PhotonView))]
public class NetworkCullingHandler : MonoBehaviour
{
	// Token: 0x060026C7 RID: 9927 RVA: 0x000C24E4 File Offset: 0x000C06E4
	private void OnEnable()
	{
		if (this.pView == null)
		{
			this.pView = base.GetComponent<PhotonView>();
			if (!this.pView.isMine)
			{
				return;
			}
		}
		if (this.cullArea == null)
		{
			this.cullArea = UnityEngine.Object.FindObjectOfType<CullArea>();
		}
		this.previousActiveCells = new List<int>(0);
		this.activeCells = new List<int>(0);
		this.currentPosition = (this.lastPosition = base.transform.position);
	}

	// Token: 0x060026C8 RID: 9928 RVA: 0x000C2570 File Offset: 0x000C0770
	private void Start()
	{
		if (!this.pView.isMine)
		{
			return;
		}
		if (PhotonNetwork.inRoom)
		{
			if (this.cullArea.NumberOfSubdivisions == 0)
			{
				this.pView.group = this.cullArea.FIRST_GROUP_ID;
				PhotonNetwork.SetReceivingEnabled(this.cullArea.FIRST_GROUP_ID, true);
				PhotonNetwork.SetSendingEnabled(this.cullArea.FIRST_GROUP_ID, true);
			}
			else
			{
				this.CheckGroupsChanged();
				base.InvokeRepeating("UpdateActiveGroup", 0f, 1f / (float)PhotonNetwork.sendRateOnSerialize);
			}
		}
	}

	// Token: 0x060026C9 RID: 9929 RVA: 0x000C2608 File Offset: 0x000C0808
	private void Update()
	{
		if (!this.pView.isMine)
		{
			return;
		}
		this.lastPosition = this.currentPosition;
		this.currentPosition = base.transform.position;
		if (this.currentPosition != this.lastPosition)
		{
			this.CheckGroupsChanged();
		}
	}

	// Token: 0x060026CA RID: 9930 RVA: 0x000C2660 File Offset: 0x000C0860
	private void OnDisable()
	{
		base.CancelInvoke();
	}

	// Token: 0x060026CB RID: 9931 RVA: 0x000C2668 File Offset: 0x000C0868
	private void CheckGroupsChanged()
	{
		if (this.cullArea.NumberOfSubdivisions == 0)
		{
			return;
		}
		this.previousActiveCells = new List<int>(this.activeCells);
		this.activeCells = this.cullArea.GetActiveCells(base.transform.position);
		if (this.activeCells.Count != this.previousActiveCells.Count)
		{
			this.UpdateInterestGroups();
			return;
		}
		foreach (int item in this.activeCells)
		{
			if (!this.previousActiveCells.Contains(item))
			{
				this.UpdateInterestGroups();
				break;
			}
		}
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x000C2744 File Offset: 0x000C0944
	private void UpdateInterestGroups()
	{
		foreach (int group in this.previousActiveCells)
		{
			PhotonNetwork.SetReceivingEnabled(group, false);
			PhotonNetwork.SetSendingEnabled(group, false);
		}
		foreach (int group2 in this.activeCells)
		{
			PhotonNetwork.SetReceivingEnabled(group2, true);
			PhotonNetwork.SetSendingEnabled(group2, true);
		}
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x000C2810 File Offset: 0x000C0A10
	private void UpdateActiveGroup()
	{
		while (this.activeCells.Count <= this.cullArea.NumberOfSubdivisions)
		{
			this.activeCells.Add(this.cullArea.FIRST_GROUP_ID);
		}
		if (this.cullArea.NumberOfSubdivisions == 1)
		{
			this.orderIndex = ++this.orderIndex % this.cullArea.SUBDIVISION_FIRST_LEVEL_ORDER.Length;
			this.pView.group = this.activeCells[this.cullArea.SUBDIVISION_FIRST_LEVEL_ORDER[this.orderIndex]];
		}
		else if (this.cullArea.NumberOfSubdivisions == 2)
		{
			this.orderIndex = ++this.orderIndex % this.cullArea.SUBDIVISION_SECOND_LEVEL_ORDER.Length;
			this.pView.group = this.activeCells[this.cullArea.SUBDIVISION_SECOND_LEVEL_ORDER[this.orderIndex]];
		}
		else if (this.cullArea.NumberOfSubdivisions == 3)
		{
			this.orderIndex = ++this.orderIndex % this.cullArea.SUBDIVISION_THIRD_LEVEL_ORDER.Length;
			this.pView.group = this.activeCells[this.cullArea.SUBDIVISION_THIRD_LEVEL_ORDER[this.orderIndex]];
		}
	}

	// Token: 0x060026CE RID: 9934 RVA: 0x000C2978 File Offset: 0x000C0B78
	private void OnGUI()
	{
		if (!this.pView.isMine)
		{
			return;
		}
		string text = "Inside cells:\n";
		string text2 = "Subscribed cells:\n";
		for (int i = 0; i < this.activeCells.Count; i++)
		{
			if (i <= this.cullArea.NumberOfSubdivisions)
			{
				text = text + this.activeCells[i] + "  ";
			}
			text2 = text2 + this.activeCells[i] + "  ";
		}
		GUI.Label(new Rect(20f, (float)Screen.height - 100f, 200f, 40f), "<color=white>" + text + "</color>", new GUIStyle
		{
			alignment = TextAnchor.UpperLeft,
			fontSize = 16
		});
		GUI.Label(new Rect(20f, (float)Screen.height - 60f, 200f, 40f), "<color=white>" + text2 + "</color>", new GUIStyle
		{
			alignment = TextAnchor.UpperLeft,
			fontSize = 16
		});
	}

	// Token: 0x04001B44 RID: 6980
	private int orderIndex;

	// Token: 0x04001B45 RID: 6981
	private CullArea cullArea;

	// Token: 0x04001B46 RID: 6982
	private List<int> previousActiveCells;

	// Token: 0x04001B47 RID: 6983
	private List<int> activeCells;

	// Token: 0x04001B48 RID: 6984
	private PhotonView pView;

	// Token: 0x04001B49 RID: 6985
	private Vector3 lastPosition;

	// Token: 0x04001B4A RID: 6986
	private Vector3 currentPosition;
}
