using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x02000118 RID: 280
public class FindFriendsFromLocalLAN : MonoBehaviour
{
	// Token: 0x0600081F RID: 2079 RVA: 0x000310DC File Offset: 0x0002F2DC
	private void Start()
	{
		this.ipaddress = Network.player.ipAddress.ToString();
		this.StartBroadcastingSession();
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x00031108 File Offset: 0x0002F308
	private void StartBroadcastingSession()
	{
		this.objUDPClient = new UdpClient(22044);
		this.objUDPClient.EnableBroadcast = true;
		this.BeginAsyncReceive();
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00031138 File Offset: 0x0002F338
	public void StopBroadCasting()
	{
		if (this.objUDPClient != null)
		{
			UdpClient udpClient = this.objUDPClient;
			this.objUDPClient = null;
			udpClient.Close();
		}
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00031164 File Offset: 0x0002F364
	private void BeginAsyncReceive()
	{
		if (this.objUDPClient == null)
		{
			return;
		}
		this.objUDPClient.BeginReceive(new AsyncCallback(this.GetAsyncReceive), null);
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0003118C File Offset: 0x0002F38C
	private IEnumerator OnApplicationPause(bool pause)
	{
		if (pause)
		{
			this.StopBroadCasting();
		}
		else
		{
			yield return null;
			yield return null;
			yield return null;
			this.StartBroadcastingSession();
		}
		yield break;
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x000311B8 File Offset: 0x0002F3B8
	private void GetAsyncReceive(IAsyncResult objResult)
	{
		if (this.objUDPClient == null)
		{
			return;
		}
		IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, 0);
		byte[] array = this.objUDPClient.EndReceive(objResult, ref ipendPoint);
		if (array.Length > 0 && !ipendPoint.Address.ToString().Equals(this.ipaddress))
		{
			string @string = Encoding.Unicode.GetString(array);
			List<object> list = Json.Deserialize(@string) as List<object>;
			string text = string.Empty;
			if (list != null && list.Count == 1)
			{
				text = Convert.ToString(list[0]);
			}
			if (!string.IsNullOrEmpty(text) && !FindFriendsFromLocalLAN.lanPlayerInfo.Contains(text) && !FriendsController.sharedController.friends.Contains(text))
			{
				FindFriendsFromLocalLAN.lanPlayerInfo.Add(text);
				if (this.isActiveFriends)
				{
					this.idsForInfo.Add(text);
				}
			}
			this.isGetMessage = true;
		}
		this.BeginAsyncReceive();
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x000312B4 File Offset: 0x0002F4B4
	private void SendMyInfo()
	{
		if (string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			return;
		}
		this.timeSendMyInfo = Time.time;
		string s = Json.Serialize(new List<string>
		{
			FriendsController.sharedController.id
		});
		byte[] bytes = Encoding.Unicode.GetBytes(s);
		if (this.objUDPClient != null)
		{
			try
			{
				this.objUDPClient.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Broadcast, 22044));
			}
			catch (Exception arg)
			{
				Debug.Log("soccet close " + arg);
			}
		}
		else
		{
			Debug.Log("objUDPClient=NULL");
		}
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x0003137C File Offset: 0x0002F57C
	private void Update()
	{
		this.isActiveFriends = (FriendsWindowGUI.Instance != null && FriendsWindowGUI.Instance.InterfaceEnabled);
		if (this.idsForInfo.Count > 0)
		{
			FriendsController.sharedController.GetInfoAboutPlayers(this.idsForInfo);
			this.idsForInfo.Clear();
		}
		if ((this.isActiveFriends || this.isGetMessage) && Time.time - this.timeSendMyInfo > this.periodSendMyInfo)
		{
			this.isGetMessage = false;
			this.SendMyInfo();
		}
	}

	// Token: 0x040006B6 RID: 1718
	public static bool isFindLocalFriends = false;

	// Token: 0x040006B7 RID: 1719
	private string ipaddress;

	// Token: 0x040006B8 RID: 1720
	public static List<string> lanPlayerInfo = new List<string>();

	// Token: 0x040006B9 RID: 1721
	public static Action lanPlayerInfoUpdate = null;

	// Token: 0x040006BA RID: 1722
	private UdpClient objUDPClient;

	// Token: 0x040006BB RID: 1723
	private float periodSendMyInfo = 30f;

	// Token: 0x040006BC RID: 1724
	private float timeSendMyInfo;

	// Token: 0x040006BD RID: 1725
	private bool isGetMessage;

	// Token: 0x040006BE RID: 1726
	private bool isActiveFriends;

	// Token: 0x040006BF RID: 1727
	private List<string> idsForInfo = new List<string>();
}
