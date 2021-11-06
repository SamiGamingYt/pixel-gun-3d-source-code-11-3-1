using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x02000467 RID: 1127
	public class ChatClient : IPhotonPeerListener
	{
		// Token: 0x0600276F RID: 10095 RVA: 0x000C4F64 File Offset: 0x000C3164
		public ChatClient(IChatClientListener listener, ConnectionProtocol protocol = ConnectionProtocol.Udp)
		{
			this.listener = listener;
			this.State = ChatState.Uninitialized;
			this.chatPeer = new ChatPeer(this, protocol);
			this.PublicChannels = new Dictionary<string, ChatChannel>();
			this.PrivateChannels = new Dictionary<string, ChatChannel>();
			this.PublicChannelsUnsubscribing = new HashSet<string>();
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000C4FC8 File Offset: 0x000C31C8
		void IPhotonPeerListener.DebugReturn(DebugLevel level, string message)
		{
			this.listener.DebugReturn(level, message);
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000C4FD8 File Offset: 0x000C31D8
		void IPhotonPeerListener.OnEvent(EventData eventData)
		{
			switch (eventData.Code)
			{
			case 0:
				this.HandleChatMessagesEvent(eventData);
				break;
			case 2:
				this.HandlePrivateMessageEvent(eventData);
				break;
			case 4:
				this.HandleStatusUpdate(eventData);
				break;
			case 5:
				this.HandleSubscribeEvent(eventData);
				break;
			case 6:
				this.HandleUnsubscribeEvent(eventData);
				break;
			}
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000C5050 File Offset: 0x000C3250
		void IPhotonPeerListener.OnOperationResponse(OperationResponse operationResponse)
		{
			byte operationCode = operationResponse.OperationCode;
			switch (operationCode)
			{
			case 0:
			case 1:
			case 2:
			case 3:
				break;
			default:
				if (operationCode == 230)
				{
					this.HandleAuthResponse(operationResponse);
					return;
				}
				break;
			}
			if (operationResponse.ReturnCode != 0 && this.DebugOut >= DebugLevel.ERROR)
			{
				if (operationResponse.ReturnCode == -2)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Chat Operation {0} unknown on server. Check your AppId and make sure it's for a Chat application.", operationResponse.OperationCode));
				}
				else
				{
					this.listener.DebugReturn(DebugLevel.ERROR, string.Format("Chat Operation {0} failed (Code: {1}). Debug Message: {2}", operationResponse.OperationCode, operationResponse.ReturnCode, operationResponse.DebugMessage));
				}
			}
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000C5118 File Offset: 0x000C3318
		void IPhotonPeerListener.OnStatusChanged(StatusCode statusCode)
		{
			if (statusCode != StatusCode.Connect)
			{
				if (statusCode != StatusCode.Disconnect)
				{
					if (statusCode != StatusCode.EncryptionEstablished)
					{
						if (statusCode == StatusCode.EncryptionFailedToEstablish)
						{
							this.State = ChatState.Disconnecting;
							this.chatPeer.Disconnect();
						}
					}
					else if (!this.didAuthenticate)
					{
						this.didAuthenticate = this.chatPeer.AuthenticateOnNameServer(this.AppId, this.AppVersion, this.chatRegion, this.AuthValues);
						if (!this.didAuthenticate && this.DebugOut >= DebugLevel.ERROR)
						{
							((IPhotonPeerListener)this).DebugReturn(DebugLevel.ERROR, "Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected. State: " + this.State);
						}
					}
				}
				else if (this.State == ChatState.Authenticated)
				{
					this.ConnectToFrontEnd();
				}
				else
				{
					this.State = ChatState.Disconnected;
					this.listener.OnChatStateChange(ChatState.Disconnected);
					this.listener.OnDisconnected();
				}
			}
			else
			{
				if (!this.chatPeer.IsProtocolSecure)
				{
					Debug.Log("Establishing Encryption");
					this.chatPeer.EstablishEncryption();
				}
				else
				{
					Debug.Log("Skipping Encryption");
					if (!this.didAuthenticate)
					{
						this.didAuthenticate = this.chatPeer.AuthenticateOnNameServer(this.AppId, this.AppVersion, this.chatRegion, this.AuthValues);
						if (!this.didAuthenticate && this.DebugOut >= DebugLevel.ERROR)
						{
							((IPhotonPeerListener)this).DebugReturn(DebugLevel.ERROR, "Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected. State: " + this.State);
						}
					}
				}
				if (this.State == ChatState.ConnectingToNameServer)
				{
					this.State = ChatState.ConnectedToNameServer;
					this.listener.OnChatStateChange(this.State);
				}
				else if (this.State == ChatState.ConnectingToFrontEnd)
				{
					this.AuthenticateOnFrontEnd();
				}
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06002774 RID: 10100 RVA: 0x000C52F8 File Offset: 0x000C34F8
		// (set) Token: 0x06002775 RID: 10101 RVA: 0x000C5300 File Offset: 0x000C3500
		public string NameServerAddress { get; private set; }

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06002776 RID: 10102 RVA: 0x000C530C File Offset: 0x000C350C
		// (set) Token: 0x06002777 RID: 10103 RVA: 0x000C5314 File Offset: 0x000C3514
		public string FrontendAddress { get; private set; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002778 RID: 10104 RVA: 0x000C5320 File Offset: 0x000C3520
		// (set) Token: 0x06002779 RID: 10105 RVA: 0x000C5328 File Offset: 0x000C3528
		public string ChatRegion
		{
			get
			{
				return this.chatRegion;
			}
			set
			{
				this.chatRegion = value;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x0600277A RID: 10106 RVA: 0x000C5334 File Offset: 0x000C3534
		// (set) Token: 0x0600277B RID: 10107 RVA: 0x000C533C File Offset: 0x000C353C
		public ChatState State { get; private set; }

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x0600277C RID: 10108 RVA: 0x000C5348 File Offset: 0x000C3548
		// (set) Token: 0x0600277D RID: 10109 RVA: 0x000C5350 File Offset: 0x000C3550
		public ChatDisconnectCause DisconnectedCause { get; private set; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x0600277E RID: 10110 RVA: 0x000C535C File Offset: 0x000C355C
		public bool CanChat
		{
			get
			{
				return this.State == ChatState.ConnectedToFrontEnd && this.HasPeer;
			}
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000C5374 File Offset: 0x000C3574
		public bool CanChatInChannel(string channelName)
		{
			return this.CanChat && this.PublicChannels.ContainsKey(channelName) && !this.PublicChannelsUnsubscribing.Contains(channelName);
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002780 RID: 10112 RVA: 0x000C53B0 File Offset: 0x000C35B0
		private bool HasPeer
		{
			get
			{
				return this.chatPeer != null;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002781 RID: 10113 RVA: 0x000C53C0 File Offset: 0x000C35C0
		// (set) Token: 0x06002782 RID: 10114 RVA: 0x000C53C8 File Offset: 0x000C35C8
		public string AppVersion { get; private set; }

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002783 RID: 10115 RVA: 0x000C53D4 File Offset: 0x000C35D4
		// (set) Token: 0x06002784 RID: 10116 RVA: 0x000C53DC File Offset: 0x000C35DC
		public string AppId { get; private set; }

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002785 RID: 10117 RVA: 0x000C53E8 File Offset: 0x000C35E8
		// (set) Token: 0x06002786 RID: 10118 RVA: 0x000C53F0 File Offset: 0x000C35F0
		public AuthenticationValues AuthValues { get; set; }

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002787 RID: 10119 RVA: 0x000C53FC File Offset: 0x000C35FC
		// (set) Token: 0x06002788 RID: 10120 RVA: 0x000C541C File Offset: 0x000C361C
		public string UserId
		{
			get
			{
				return (this.AuthValues == null) ? null : this.AuthValues.UserId;
			}
			private set
			{
				if (this.AuthValues == null)
				{
					this.AuthValues = new AuthenticationValues();
				}
				this.AuthValues.UserId = value;
			}
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000C544C File Offset: 0x000C364C
		public bool Connect(string appId, string appVersion, AuthenticationValues authValues)
		{
			this.chatPeer.TimePingInterval = 3000;
			this.DisconnectedCause = ChatDisconnectCause.None;
			if (authValues == null)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Connect failed: no authentication values specified");
				}
				return false;
			}
			this.AuthValues = authValues;
			if (this.AuthValues.UserId == null || this.AuthValues.UserId == string.Empty)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Connect failed: no UserId specified in authentication values");
				}
				return false;
			}
			this.AppId = appId;
			this.AppVersion = appVersion;
			this.didAuthenticate = false;
			this.msDeltaForServiceCalls = 100;
			this.chatPeer.QuickResendAttempts = 2;
			this.chatPeer.SentCountAllowance = 7;
			this.PublicChannels.Clear();
			this.PrivateChannels.Clear();
			this.PublicChannelsUnsubscribing.Clear();
			this.NameServerAddress = this.chatPeer.NameServerAddress;
			bool flag = this.chatPeer.Connect();
			if (flag)
			{
				this.State = ChatState.ConnectingToNameServer;
			}
			return flag;
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x000C556C File Offset: 0x000C376C
		public void Service()
		{
			if (this.HasPeer && (Environment.TickCount - this.msTimestampOfLastServiceCall > this.msDeltaForServiceCalls || this.msTimestampOfLastServiceCall == 0))
			{
				this.msTimestampOfLastServiceCall = Environment.TickCount;
				this.chatPeer.Service();
			}
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000C55BC File Offset: 0x000C37BC
		public void Disconnect()
		{
			if (this.HasPeer && this.chatPeer.PeerState != PeerStateValue.Disconnected)
			{
				this.chatPeer.Disconnect();
			}
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000C55F0 File Offset: 0x000C37F0
		public void StopThread()
		{
			if (this.HasPeer)
			{
				this.chatPeer.StopThread();
			}
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x000C5608 File Offset: 0x000C3808
		public bool Subscribe(string[] channels)
		{
			return this.Subscribe(channels, 0);
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000C5614 File Offset: 0x000C3814
		public bool Subscribe(string[] channels, int messagesFromHistory)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Subscribe called while not connected to front end server.");
				}
				return false;
			}
			if (channels == null || channels.Length == 0)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "Subscribe can't be called for empty or null channels-list.");
				}
				return false;
			}
			return this.SendChannelOperation(channels, 0, messagesFromHistory);
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x000C5684 File Offset: 0x000C3884
		public bool Unsubscribe(string[] channels)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Unsubscribe called while not connected to front end server.");
				}
				return false;
			}
			if (channels == null || channels.Length == 0)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "Unsubscribe can't be called for empty or null channels-list.");
				}
				return false;
			}
			foreach (string item in channels)
			{
				this.PublicChannelsUnsubscribing.Add(item);
			}
			return this.SendChannelOperation(channels, 1, 0);
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x000C5718 File Offset: 0x000C3918
		public bool PublishMessage(string channelName, object message)
		{
			return this.publishMessage(channelName, message, true);
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000C5724 File Offset: 0x000C3924
		internal bool PublishMessageUnreliable(string channelName, object message)
		{
			return this.publishMessage(channelName, message, false);
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000C5730 File Offset: 0x000C3930
		private bool publishMessage(string channelName, object message, bool reliable)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "PublishMessage called while not connected to front end server.");
				}
				return false;
			}
			if (string.IsNullOrEmpty(channelName) || message == null)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "PublishMessage parameters must be non-null and not empty.");
				}
				return false;
			}
			Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>
			{
				{
					1,
					channelName
				},
				{
					3,
					message
				}
			};
			return this.chatPeer.OpCustom(2, customOpParameters, reliable);
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x000C57C0 File Offset: 0x000C39C0
		public bool SendPrivateMessage(string target, object message)
		{
			return this.SendPrivateMessage(target, message, false);
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000C57CC File Offset: 0x000C39CC
		public bool SendPrivateMessage(string target, object message, bool encrypt)
		{
			return this.sendPrivateMessage(target, message, encrypt, true);
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x000C57D8 File Offset: 0x000C39D8
		internal bool SendPrivateMessageUnreliable(string target, object message, bool encrypt)
		{
			return this.sendPrivateMessage(target, message, encrypt, false);
		}

		// Token: 0x06002796 RID: 10134 RVA: 0x000C57E4 File Offset: 0x000C39E4
		private bool sendPrivateMessage(string target, object message, bool encrypt, bool reliable)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "SendPrivateMessage called while not connected to front end server.");
				}
				return false;
			}
			if (string.IsNullOrEmpty(target) || message == null)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "SendPrivateMessage parameters must be non-null and not empty.");
				}
				return false;
			}
			Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>
			{
				{
					225,
					target
				},
				{
					3,
					message
				}
			};
			return this.chatPeer.OpCustom(3, customOpParameters, reliable, 0, encrypt);
		}

		// Token: 0x06002797 RID: 10135 RVA: 0x000C587C File Offset: 0x000C3A7C
		private bool SetOnlineStatus(int status, object message, bool skipMessage)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "SetOnlineStatus called while not connected to front end server.");
				}
				return false;
			}
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>
			{
				{
					10,
					status
				}
			};
			if (skipMessage)
			{
				dictionary[12] = true;
			}
			else
			{
				dictionary[3] = message;
			}
			return this.chatPeer.OpCustom(5, dictionary, true);
		}

		// Token: 0x06002798 RID: 10136 RVA: 0x000C58F8 File Offset: 0x000C3AF8
		public bool SetOnlineStatus(int status)
		{
			return this.SetOnlineStatus(status, null, true);
		}

		// Token: 0x06002799 RID: 10137 RVA: 0x000C5904 File Offset: 0x000C3B04
		public bool SetOnlineStatus(int status, object message)
		{
			return this.SetOnlineStatus(status, message, false);
		}

		// Token: 0x0600279A RID: 10138 RVA: 0x000C5910 File Offset: 0x000C3B10
		public bool AddFriends(string[] friends)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "AddFriends called while not connected to front end server.");
				}
				return false;
			}
			if (friends == null || friends.Length == 0)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "AddFriends can't be called for empty or null list.");
				}
				return false;
			}
			if (friends.Length > 1024)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, string.Concat(new object[]
					{
						"AddFriends max list size exceeded: ",
						friends.Length,
						" > ",
						1024
					}));
				}
				return false;
			}
			Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>
			{
				{
					11,
					friends
				}
			};
			return this.chatPeer.OpCustom(6, customOpParameters, true);
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000C59F0 File Offset: 0x000C3BF0
		public bool RemoveFriends(string[] friends)
		{
			if (!this.CanChat)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "RemoveFriends called while not connected to front end server.");
				}
				return false;
			}
			if (friends == null || friends.Length == 0)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "RemoveFriends can't be called for empty or null list.");
				}
				return false;
			}
			if (friends.Length > 1024)
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, string.Concat(new object[]
					{
						"RemoveFriends max list size exceeded: ",
						friends.Length,
						" > ",
						1024
					}));
				}
				return false;
			}
			Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>
			{
				{
					11,
					friends
				}
			};
			return this.chatPeer.OpCustom(7, customOpParameters, true);
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000C5AD0 File Offset: 0x000C3CD0
		public string GetPrivateChannelNameByUser(string userName)
		{
			return string.Format("{0}:{1}", this.UserId, userName);
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000C5AE4 File Offset: 0x000C3CE4
		public bool TryGetChannel(string channelName, bool isPrivate, out ChatChannel channel)
		{
			if (!isPrivate)
			{
				return this.PublicChannels.TryGetValue(channelName, out channel);
			}
			return this.PrivateChannels.TryGetValue(channelName, out channel);
		}

		// Token: 0x0600279E RID: 10142 RVA: 0x000C5B08 File Offset: 0x000C3D08
		public bool TryGetChannel(string channelName, out ChatChannel channel)
		{
			bool flag = this.PublicChannels.TryGetValue(channelName, out channel);
			return flag || this.PrivateChannels.TryGetValue(channelName, out channel);
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000C5B3C File Offset: 0x000C3D3C
		public void SendAcksOnly()
		{
			if (this.chatPeer != null)
			{
				this.chatPeer.SendAcksOnly();
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060027A1 RID: 10145 RVA: 0x000C5B68 File Offset: 0x000C3D68
		// (set) Token: 0x060027A0 RID: 10144 RVA: 0x000C5B58 File Offset: 0x000C3D58
		public DebugLevel DebugOut
		{
			get
			{
				return this.chatPeer.DebugOut;
			}
			set
			{
				this.chatPeer.DebugOut = value;
			}
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000C5B78 File Offset: 0x000C3D78
		private bool SendChannelOperation(string[] channels, byte operation, int historyLength)
		{
			Dictionary<byte, object> dictionary = new Dictionary<byte, object>
			{
				{
					0,
					channels
				}
			};
			if (historyLength != 0)
			{
				dictionary.Add(14, historyLength);
			}
			return this.chatPeer.OpCustom(operation, dictionary, true);
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000C5BB8 File Offset: 0x000C3DB8
		private void HandlePrivateMessageEvent(EventData eventData)
		{
			object message = eventData.Parameters[3];
			string text = (string)eventData.Parameters[5];
			string privateChannelNameByUser;
			if (this.UserId != null && this.UserId.Equals(text))
			{
				string userName = (string)eventData.Parameters[225];
				privateChannelNameByUser = this.GetPrivateChannelNameByUser(userName);
			}
			else
			{
				privateChannelNameByUser = this.GetPrivateChannelNameByUser(text);
			}
			ChatChannel chatChannel;
			if (!this.PrivateChannels.TryGetValue(privateChannelNameByUser, out chatChannel))
			{
				chatChannel = new ChatChannel(privateChannelNameByUser);
				chatChannel.IsPrivate = true;
				chatChannel.MessageLimit = this.MessageLimit;
				this.PrivateChannels.Add(chatChannel.Name, chatChannel);
			}
			chatChannel.Add(text, message);
			this.listener.OnPrivateMessage(text, message, privateChannelNameByUser);
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000C5C88 File Offset: 0x000C3E88
		private void HandleChatMessagesEvent(EventData eventData)
		{
			object[] messages = (object[])eventData.Parameters[2];
			string[] senders = (string[])eventData.Parameters[4];
			string text = (string)eventData.Parameters[1];
			ChatChannel chatChannel;
			if (!this.PublicChannels.TryGetValue(text, out chatChannel))
			{
				if (this.DebugOut >= DebugLevel.WARNING)
				{
					this.listener.DebugReturn(DebugLevel.WARNING, "Channel " + text + " for incoming message event not found.");
				}
				return;
			}
			chatChannel.Add(senders, messages);
			this.listener.OnGetMessages(text, senders, messages);
		}

		// Token: 0x060027A5 RID: 10149 RVA: 0x000C5D20 File Offset: 0x000C3F20
		private void HandleSubscribeEvent(EventData eventData)
		{
			string[] array = (string[])eventData.Parameters[0];
			bool[] array2 = (bool[])eventData.Parameters[15];
			for (int i = 0; i < array.Length; i++)
			{
				if (array2[i])
				{
					string text = array[i];
					if (!this.PublicChannels.ContainsKey(text))
					{
						ChatChannel chatChannel = new ChatChannel(text);
						chatChannel.MessageLimit = this.MessageLimit;
						this.PublicChannels.Add(chatChannel.Name, chatChannel);
					}
				}
			}
			this.listener.OnSubscribed(array, array2);
		}

		// Token: 0x060027A6 RID: 10150 RVA: 0x000C5DBC File Offset: 0x000C3FBC
		private void HandleUnsubscribeEvent(EventData eventData)
		{
			foreach (string text in (string[])eventData[0])
			{
				this.PublicChannels.Remove(text);
				this.PublicChannelsUnsubscribing.Remove(text);
			}
			string[] array;
			this.listener.OnUnsubscribed(array);
		}

		// Token: 0x060027A7 RID: 10151 RVA: 0x000C5E14 File Offset: 0x000C4014
		private void HandleAuthResponse(OperationResponse operationResponse)
		{
			if (this.DebugOut >= DebugLevel.INFO)
			{
				this.listener.DebugReturn(DebugLevel.INFO, operationResponse.ToStringFull() + " on: " + this.chatPeer.NameServerAddress);
			}
			if (operationResponse.ReturnCode == 0)
			{
				if (this.State == ChatState.ConnectedToNameServer)
				{
					this.State = ChatState.Authenticated;
					this.listener.OnChatStateChange(this.State);
					if (operationResponse.Parameters.ContainsKey(221))
					{
						if (this.AuthValues == null)
						{
							this.AuthValues = new AuthenticationValues();
						}
						this.AuthValues.Token = (operationResponse[221] as string);
						this.FrontendAddress = (string)operationResponse[230];
						this.chatPeer.Disconnect();
					}
					else if (this.DebugOut >= DebugLevel.ERROR)
					{
						this.listener.DebugReturn(DebugLevel.ERROR, "No secret in authentication response.");
					}
				}
				else if (this.State == ChatState.ConnectingToFrontEnd)
				{
					this.msDeltaForServiceCalls *= 4;
					this.State = ChatState.ConnectedToFrontEnd;
					this.listener.OnChatStateChange(this.State);
					this.listener.OnConnected();
				}
			}
			else
			{
				short returnCode = operationResponse.ReturnCode;
				switch (returnCode)
				{
				case 32755:
					this.DisconnectedCause = ChatDisconnectCause.CustomAuthenticationFailed;
					break;
				case 32756:
					this.DisconnectedCause = ChatDisconnectCause.InvalidRegion;
					break;
				case 32757:
					this.DisconnectedCause = ChatDisconnectCause.MaxCcuReached;
					break;
				default:
					if (returnCode != -3)
					{
						if (returnCode == 32767)
						{
							this.DisconnectedCause = ChatDisconnectCause.InvalidAuthentication;
						}
					}
					else
					{
						this.DisconnectedCause = ChatDisconnectCause.OperationNotAllowedInCurrentState;
					}
					break;
				}
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Authentication request error: " + operationResponse.ReturnCode + ". Disconnecting.");
				}
				this.State = ChatState.Disconnecting;
				this.chatPeer.Disconnect();
			}
		}

		// Token: 0x060027A8 RID: 10152 RVA: 0x000C6010 File Offset: 0x000C4210
		private void HandleStatusUpdate(EventData eventData)
		{
			string user = (string)eventData.Parameters[5];
			int status = (int)eventData.Parameters[10];
			object message = null;
			bool flag = eventData.Parameters.ContainsKey(3);
			if (flag)
			{
				message = eventData.Parameters[3];
			}
			this.listener.OnStatusUpdate(user, status, flag, message);
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000C6074 File Offset: 0x000C4274
		private void ConnectToFrontEnd()
		{
			this.State = ChatState.ConnectingToFrontEnd;
			if (this.DebugOut >= DebugLevel.INFO)
			{
				this.listener.DebugReturn(DebugLevel.INFO, "Connecting to frontend " + this.FrontendAddress);
			}
			this.chatPeer.Connect(this.FrontendAddress, "chat");
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x000C60C8 File Offset: 0x000C42C8
		private bool AuthenticateOnFrontEnd()
		{
			if (this.AuthValues == null)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Can't authenticate on front end server. Authentication Values are not set");
				}
				return false;
			}
			if (this.AuthValues.Token == null || this.AuthValues.Token == string.Empty)
			{
				if (this.DebugOut >= DebugLevel.ERROR)
				{
					this.listener.DebugReturn(DebugLevel.ERROR, "Can't authenticate on front end server. Secret is not set");
				}
				return false;
			}
			Dictionary<byte, object> customOpParameters = new Dictionary<byte, object>
			{
				{
					221,
					this.AuthValues.Token
				}
			};
			return this.chatPeer.OpCustom(230, customOpParameters, true);
		}

		// Token: 0x04001B9C RID: 7068
		private const int FriendRequestListMax = 1024;

		// Token: 0x04001B9D RID: 7069
		private const string ChatAppName = "chat";

		// Token: 0x04001B9E RID: 7070
		private string chatRegion = "EU";

		// Token: 0x04001B9F RID: 7071
		public int MessageLimit;

		// Token: 0x04001BA0 RID: 7072
		public readonly Dictionary<string, ChatChannel> PublicChannels;

		// Token: 0x04001BA1 RID: 7073
		public readonly Dictionary<string, ChatChannel> PrivateChannels;

		// Token: 0x04001BA2 RID: 7074
		private readonly HashSet<string> PublicChannelsUnsubscribing;

		// Token: 0x04001BA3 RID: 7075
		private readonly IChatClientListener listener;

		// Token: 0x04001BA4 RID: 7076
		public ChatPeer chatPeer;

		// Token: 0x04001BA5 RID: 7077
		private bool didAuthenticate;

		// Token: 0x04001BA6 RID: 7078
		private int msDeltaForServiceCalls = 50;

		// Token: 0x04001BA7 RID: 7079
		private int msTimestampOfLastServiceCall;
	}
}
