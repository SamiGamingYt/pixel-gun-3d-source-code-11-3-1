using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

// Token: 0x0200067C RID: 1660
public class LANBroadcastService : MonoBehaviour
{
	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x060039BC RID: 14780 RVA: 0x0012B62C File Offset: 0x0012982C
	public string Message
	{
		get
		{
			return this.strMessage;
		}
	}

	// Token: 0x060039BD RID: 14781 RVA: 0x0012B634 File Offset: 0x00129834
	private void Start()
	{
		this.lstReceivedMessages = new List<LANBroadcastService.ReceivedMessage>();
		this.ipaddress = Network.player.ipAddress.ToString();
	}

	// Token: 0x060039BE RID: 14782 RVA: 0x0012B664 File Offset: 0x00129864
	private void Update()
	{
		if (this.currentState == LANBroadcastService.enuState.Announcing && Time.time > this.fTimeLastMessageSent + this.fIntervalMessageSending)
		{
			string s = string.Concat(new object[]
			{
				this.strServerReady,
				"ý",
				this.serverMessage.name,
				"ý",
				this.serverMessage.map,
				"ý",
				this.serverMessage.connectedPlayers,
				"ý",
				this.serverMessage.playerLimit,
				"ý",
				this.serverMessage.comment,
				"ý",
				this.serverMessage.regim,
				"ý",
				GlobalGameController.MultiplayerProtocolVersion
			});
			byte[] bytes = Encoding.Unicode.GetBytes(s);
			if (this.objUDPClient != null)
			{
				try
				{
					this.objUDPClient.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Broadcast, 22043));
				}
				catch (Exception ex)
				{
					Debug.Log("soccet close");
				}
			}
			else
			{
				Debug.Log("objUDPClient=NULL");
			}
			this.fTimeLastMessageSent = Time.time;
		}
		if (this.currentState == LANBroadcastService.enuState.Searching)
		{
			if (this.lstReceivedMessages == null)
			{
				return;
			}
			for (int i = 0; i < this.lstReceivedMessages.Count; i++)
			{
				LANBroadcastService.ReceivedMessage item = this.lstReceivedMessages[i];
				if (item.fTime < 0f)
				{
					LANBroadcastService.ReceivedMessage item2 = default(LANBroadcastService.ReceivedMessage);
					item2.ipAddress = item.ipAddress;
					item2.name = item.name;
					item2.map = item.map;
					item2.connectedPlayers = item.connectedPlayers;
					item2.playerLimit = item.playerLimit;
					item2.comment = item.comment;
					item2.fTime = Time.time;
					item2.regim = item.regim;
					this.lstReceivedMessages.RemoveAt(i);
					this.lstReceivedMessages.Add(item2);
				}
				if (Time.time > item.fTime + this.fTimeMessagesLive)
				{
					this.lstReceivedMessages.Remove(item);
					break;
				}
			}
		}
		if (this.currentState == LANBroadcastService.enuState.Searching)
		{
		}
	}

	// Token: 0x060039BF RID: 14783 RVA: 0x0012B8F8 File Offset: 0x00129AF8
	private void BeginAsyncReceive()
	{
		if (this.objUDPClient == null)
		{
			return;
		}
		this.objUDPClient.BeginReceive(new AsyncCallback(this.EndAsyncReceive), null);
	}

	// Token: 0x060039C0 RID: 14784 RVA: 0x0012B920 File Offset: 0x00129B20
	private void EndAsyncReceive(IAsyncResult objResult)
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
			string[] array2 = @string.Split(new char[]
			{
				'ý'
			}, @string.Length);
			if (array2.Length == 8 && array2[7].Equals(GlobalGameController.MultiplayerProtocolVersion))
			{
				for (int i = 0; i < this.lstReceivedMessages.Count; i++)
				{
					LANBroadcastService.ReceivedMessage receivedMessage = this.lstReceivedMessages[i];
					if (ipendPoint.Address.ToString().Equals(receivedMessage.ipAddress))
					{
						this.lstReceivedMessages.RemoveAt(i);
					}
				}
				LANBroadcastService.ReceivedMessage item = default(LANBroadcastService.ReceivedMessage);
				item.ipAddress = ipendPoint.Address.ToString();
				item.name = array2[1];
				item.map = array2[2];
				item.connectedPlayers = int.Parse(array2[3]);
				item.playerLimit = int.Parse(array2[4]);
				item.comment = array2[5];
				item.regim = int.Parse(array2[6]);
				item.protocol = array2[7];
				item.fTime = -1f;
				this.lstReceivedMessages.Add(item);
			}
		}
		if (this.currentState == LANBroadcastService.enuState.Searching)
		{
			this.BeginAsyncReceive();
		}
	}

	// Token: 0x060039C1 RID: 14785 RVA: 0x0012BAAC File Offset: 0x00129CAC
	private void StartAnnouncing()
	{
		this.currentState = LANBroadcastService.enuState.Announcing;
		this.strMessage = "Announcing we are a server...";
	}

	// Token: 0x060039C2 RID: 14786 RVA: 0x0012BAC0 File Offset: 0x00129CC0
	private void StopAnnouncing()
	{
		this.currentState = LANBroadcastService.enuState.NotActive;
		this.strMessage = "Announcements stopped.";
	}

	// Token: 0x060039C3 RID: 14787 RVA: 0x0012BAD4 File Offset: 0x00129CD4
	private void StartSearching()
	{
		if (this.lstReceivedMessages == null)
		{
			this.lstReceivedMessages = new List<LANBroadcastService.ReceivedMessage>();
		}
		this.lstReceivedMessages.Clear();
		this.BeginAsyncReceive();
		this.fTimeSearchStarted = Time.time;
		this.currentState = LANBroadcastService.enuState.Searching;
		this.strMessage = "Searching for other players...";
	}

	// Token: 0x060039C4 RID: 14788 RVA: 0x0012BB28 File Offset: 0x00129D28
	private void StopSearching()
	{
		this.currentState = LANBroadcastService.enuState.NotActive;
		this.strMessage = "Search stopped.";
	}

	// Token: 0x060039C5 RID: 14789 RVA: 0x0012BB3C File Offset: 0x00129D3C
	public void StartSearchBroadCasting(LANBroadcastService.delJoinServer connectToServer)
	{
		this.delWhenServerFound = connectToServer;
		this.StartBroadcastingSession();
		this.StartSearching();
	}

	// Token: 0x060039C6 RID: 14790 RVA: 0x0012BB54 File Offset: 0x00129D54
	public void StartAnnounceBroadCasting()
	{
		this.StartBroadcastingSession();
		this.StartAnnouncing();
	}

	// Token: 0x060039C7 RID: 14791 RVA: 0x0012BB64 File Offset: 0x00129D64
	private void StartBroadcastingSession()
	{
		if (this.currentState != LANBroadcastService.enuState.NotActive)
		{
			this.StopBroadCasting();
		}
		this.objUDPClient = new UdpClient(22043);
		this.objUDPClient.EnableBroadcast = true;
		this.fTimeLastMessageSent = Time.time;
	}

	// Token: 0x060039C8 RID: 14792 RVA: 0x0012BBAC File Offset: 0x00129DAC
	public void StopBroadCasting()
	{
		if (this.currentState == LANBroadcastService.enuState.Searching)
		{
			this.StopSearching();
		}
		else if (this.currentState == LANBroadcastService.enuState.Announcing)
		{
			this.StopAnnouncing();
		}
		if (this.objUDPClient != null)
		{
			UdpClient udpClient = this.objUDPClient;
			this.objUDPClient = null;
			udpClient.Close();
		}
	}

	// Token: 0x04002A73 RID: 10867
	public LANBroadcastService.ReceivedMessage serverMessage;

	// Token: 0x04002A74 RID: 10868
	private string strMessage = string.Empty;

	// Token: 0x04002A75 RID: 10869
	private LANBroadcastService.enuState currentState;

	// Token: 0x04002A76 RID: 10870
	private UdpClient objUDPClient;

	// Token: 0x04002A77 RID: 10871
	public List<LANBroadcastService.ReceivedMessage> lstReceivedMessages;

	// Token: 0x04002A78 RID: 10872
	private LANBroadcastService.delJoinServer delWhenServerFound;

	// Token: 0x04002A79 RID: 10873
	private LANBroadcastService.delStartServer delWhenServerMustStarted;

	// Token: 0x04002A7A RID: 10874
	private string strServerNotReady = "wanttobeaserver";

	// Token: 0x04002A7B RID: 10875
	private string strServerReady = "iamaserver";

	// Token: 0x04002A7C RID: 10876
	private float fTimeLastMessageSent;

	// Token: 0x04002A7D RID: 10877
	private float fIntervalMessageSending = 1f;

	// Token: 0x04002A7E RID: 10878
	private float fTimeMessagesLive = 5f;

	// Token: 0x04002A7F RID: 10879
	private float fTimeToSearch = 5f;

	// Token: 0x04002A80 RID: 10880
	private float fTimeSearchStarted;

	// Token: 0x04002A81 RID: 10881
	private string ipaddress;

	// Token: 0x0200067D RID: 1661
	private enum enuState
	{
		// Token: 0x04002A83 RID: 10883
		NotActive,
		// Token: 0x04002A84 RID: 10884
		Searching,
		// Token: 0x04002A85 RID: 10885
		Announcing
	}

	// Token: 0x0200067E RID: 1662
	public struct ReceivedMessage
	{
		// Token: 0x04002A86 RID: 10886
		public string ipAddress;

		// Token: 0x04002A87 RID: 10887
		public string name;

		// Token: 0x04002A88 RID: 10888
		public string map;

		// Token: 0x04002A89 RID: 10889
		public int connectedPlayers;

		// Token: 0x04002A8A RID: 10890
		public int playerLimit;

		// Token: 0x04002A8B RID: 10891
		public string comment;

		// Token: 0x04002A8C RID: 10892
		public float fTime;

		// Token: 0x04002A8D RID: 10893
		public int regim;

		// Token: 0x04002A8E RID: 10894
		public string protocol;
	}

	// Token: 0x02000922 RID: 2338
	// (Invoke) Token: 0x06005128 RID: 20776
	public delegate void delJoinServer(string strIP);

	// Token: 0x02000923 RID: 2339
	// (Invoke) Token: 0x0600512C RID: 20780
	public delegate void delStartServer();
}
