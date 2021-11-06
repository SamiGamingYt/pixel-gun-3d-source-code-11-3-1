using System;
using System.Collections.Generic;
using System.Text;

namespace ExitGames.Client.Photon.Chat
{
	// Token: 0x02000466 RID: 1126
	public class ChatChannel
	{
		// Token: 0x06002766 RID: 10086 RVA: 0x000C4E00 File Offset: 0x000C3000
		public ChatChannel(string name)
		{
			this.Name = name;
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06002767 RID: 10087 RVA: 0x000C4E28 File Offset: 0x000C3028
		// (set) Token: 0x06002768 RID: 10088 RVA: 0x000C4E30 File Offset: 0x000C3030
		public bool IsPrivate { get; protected internal set; }

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06002769 RID: 10089 RVA: 0x000C4E3C File Offset: 0x000C303C
		public int MessageCount
		{
			get
			{
				return this.Messages.Count;
			}
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000C4E4C File Offset: 0x000C304C
		public void Add(string sender, object message)
		{
			this.Senders.Add(sender);
			this.Messages.Add(message);
			this.TruncateMessages();
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000C4E6C File Offset: 0x000C306C
		public void Add(string[] senders, object[] messages)
		{
			this.Senders.AddRange(senders);
			this.Messages.AddRange(messages);
			this.TruncateMessages();
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000C4E8C File Offset: 0x000C308C
		public void TruncateMessages()
		{
			if (this.MessageLimit <= 0 || this.Messages.Count <= this.MessageLimit)
			{
				return;
			}
			int count = this.Messages.Count - this.MessageLimit;
			this.Senders.RemoveRange(0, count);
			this.Messages.RemoveRange(0, count);
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000C4EEC File Offset: 0x000C30EC
		public void ClearMessages()
		{
			this.Senders.Clear();
			this.Messages.Clear();
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000C4F04 File Offset: 0x000C3104
		public string ToStringMessages()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.Messages.Count; i++)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", this.Senders[i], this.Messages[i]));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001B97 RID: 7063
		public readonly string Name;

		// Token: 0x04001B98 RID: 7064
		public readonly List<string> Senders = new List<string>();

		// Token: 0x04001B99 RID: 7065
		public readonly List<object> Messages = new List<object>();

		// Token: 0x04001B9A RID: 7066
		public int MessageLimit;
	}
}
