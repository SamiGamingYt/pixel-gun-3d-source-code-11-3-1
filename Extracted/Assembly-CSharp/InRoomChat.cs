using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x0200043E RID: 1086
[RequireComponent(typeof(PhotonView))]
public class InRoomChat : Photon.MonoBehaviour
{
	// Token: 0x060026A4 RID: 9892 RVA: 0x000C1A40 File Offset: 0x000BFC40
	public void Start()
	{
		if (this.AlignBottom)
		{
			this.GuiRect.y = (float)Screen.height - this.GuiRect.height;
		}
	}

	// Token: 0x060026A5 RID: 9893 RVA: 0x000C1A78 File Offset: 0x000BFC78
	public void OnGUI()
	{
		if (!this.IsVisible || !PhotonNetwork.inRoom)
		{
			return;
		}
		if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
		{
			if (!string.IsNullOrEmpty(this.inputLine))
			{
				base.photonView.RPC("Chat", PhotonTargets.All, new object[]
				{
					this.inputLine
				});
				this.inputLine = string.Empty;
				GUI.FocusControl(string.Empty);
				return;
			}
			GUI.FocusControl("ChatInput");
		}
		GUI.SetNextControlName(string.Empty);
		GUILayout.BeginArea(this.GuiRect);
		this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			GUILayout.Label(this.messages[i], new GUILayoutOption[0]);
		}
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUI.SetNextControlName("ChatInput");
		this.inputLine = GUILayout.TextField(this.inputLine, new GUILayoutOption[0]);
		if (GUILayout.Button("Send", new GUILayoutOption[]
		{
			GUILayout.ExpandWidth(false)
		}))
		{
			base.photonView.RPC("Chat", PhotonTargets.All, new object[]
			{
				this.inputLine
			});
			this.inputLine = string.Empty;
			GUI.FocusControl(string.Empty);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	// Token: 0x060026A6 RID: 9894 RVA: 0x000C1C14 File Offset: 0x000BFE14
	[PunRPC]
	public void Chat(string newLine, PhotonMessageInfo mi)
	{
		string str = "anonymous";
		if (mi.sender != null)
		{
			if (!string.IsNullOrEmpty(mi.sender.name))
			{
				str = mi.sender.name;
			}
			else
			{
				str = "player " + mi.sender.ID;
			}
		}
		this.messages.Add(str + ": " + newLine);
	}

	// Token: 0x060026A7 RID: 9895 RVA: 0x000C1C90 File Offset: 0x000BFE90
	public void AddLine(string newLine)
	{
		this.messages.Add(newLine);
	}

	// Token: 0x04001B26 RID: 6950
	public Rect GuiRect = new Rect(0f, 0f, 250f, 300f);

	// Token: 0x04001B27 RID: 6951
	public bool IsVisible = true;

	// Token: 0x04001B28 RID: 6952
	public bool AlignBottom;

	// Token: 0x04001B29 RID: 6953
	public List<string> messages = new List<string>();

	// Token: 0x04001B2A RID: 6954
	private string inputLine = string.Empty;

	// Token: 0x04001B2B RID: 6955
	private Vector2 scrollPos = Vector2.zero;

	// Token: 0x04001B2C RID: 6956
	public static readonly string ChatRPC = "Chat";
}
