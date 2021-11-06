using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

namespace FlyingWormConsole3
{
	// Token: 0x02000080 RID: 128
	public class ConsoleProRemoteServer : MonoBehaviour
	{
		// Token: 0x060003E6 RID: 998 RVA: 0x00022320 File Offset: 0x00020520
		private void Awake()
		{
			if (ConsoleProRemoteServer.instance != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			ConsoleProRemoteServer.instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			Debug.Log("Starting Console Pro Server on port : " + this.port);
			ConsoleProRemoteServer.listener.Prefixes.Add("http://*:" + this.port + "/");
			ConsoleProRemoteServer.listener.Start();
			ConsoleProRemoteServer.listener.BeginGetContext(new AsyncCallback(this.ListenerCallback), null);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000223C0 File Offset: 0x000205C0
		private void OnEnable()
		{
			Application.logMessageReceived += this.LogCallback;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000223D4 File Offset: 0x000205D4
		private void OnDisable()
		{
			Application.logMessageReceived -= this.LogCallback;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x000223E8 File Offset: 0x000205E8
		public void LogCallback(string logString, string stackTrace, LogType type)
		{
			if (!logString.StartsWith("CPIGNORE"))
			{
				this.QueueLog(logString, stackTrace, type);
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00022404 File Offset: 0x00020604
		private void QueueLog(string logString, string stackTrace, LogType type)
		{
			this.logs.Add(new ConsoleProRemoteServer.QueuedLog
			{
				message = logString,
				stackTrace = stackTrace,
				type = type
			});
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00022438 File Offset: 0x00020638
		private void ListenerCallback(IAsyncResult result)
		{
			ConsoleProRemoteServer.HTTPContext context = new ConsoleProRemoteServer.HTTPContext(ConsoleProRemoteServer.listener.EndGetContext(result));
			this.HandleRequest(context);
			ConsoleProRemoteServer.listener.BeginGetContext(new AsyncCallback(this.ListenerCallback), null);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00022478 File Offset: 0x00020678
		private void HandleRequest(ConsoleProRemoteServer.HTTPContext context)
		{
			bool flag = false;
			string command = context.Command;
			if (command != null)
			{
				if (ConsoleProRemoteServer.<>f__switch$map1 == null)
				{
					ConsoleProRemoteServer.<>f__switch$map1 = new Dictionary<string, int>(1)
					{
						{
							"/NewLogs",
							0
						}
					};
				}
				int num;
				if (ConsoleProRemoteServer.<>f__switch$map1.TryGetValue(command, out num))
				{
					if (num == 0)
					{
						flag = true;
						if (this.logs.Count > 0)
						{
							string text = string.Empty;
							for (int i = 0; i < this.logs.Count; i++)
							{
								ConsoleProRemoteServer.QueuedLog queuedLog = this.logs[i];
								text = text + "::::" + queuedLog.type;
								text = text + "||||" + queuedLog.message;
								text = text + ">>>>" + queuedLog.stackTrace + ">>>>";
							}
							context.RespondWithString(text);
							this.logs.Clear();
						}
						else
						{
							context.RespondWithString(string.Empty);
						}
					}
				}
			}
			if (!flag)
			{
				context.Response.StatusCode = 404;
				context.Response.StatusDescription = "Not Found";
			}
			context.Response.OutputStream.Close();
		}

		// Token: 0x04000472 RID: 1138
		public int port = 51000;

		// Token: 0x04000473 RID: 1139
		private static HttpListener listener = new HttpListener();

		// Token: 0x04000474 RID: 1140
		[NonSerialized]
		public List<ConsoleProRemoteServer.QueuedLog> logs = new List<ConsoleProRemoteServer.QueuedLog>();

		// Token: 0x04000475 RID: 1141
		private static ConsoleProRemoteServer instance = null;

		// Token: 0x02000081 RID: 129
		public class HTTPContext
		{
			// Token: 0x060003ED RID: 1005 RVA: 0x000225BC File Offset: 0x000207BC
			public HTTPContext(HttpListenerContext inContext)
			{
				this.context = inContext;
			}

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x060003EE RID: 1006 RVA: 0x000225CC File Offset: 0x000207CC
			public string Command
			{
				get
				{
					return WWW.UnEscapeURL(this.context.Request.Url.AbsolutePath);
				}
			}

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x060003EF RID: 1007 RVA: 0x000225E8 File Offset: 0x000207E8
			public HttpListenerRequest Request
			{
				get
				{
					return this.context.Request;
				}
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x060003F0 RID: 1008 RVA: 0x000225F8 File Offset: 0x000207F8
			public HttpListenerResponse Response
			{
				get
				{
					return this.context.Response;
				}
			}

			// Token: 0x060003F1 RID: 1009 RVA: 0x00022608 File Offset: 0x00020808
			public void RespondWithString(string inString)
			{
				this.Response.StatusDescription = "OK";
				this.Response.StatusCode = 200;
				if (!string.IsNullOrEmpty(inString))
				{
					this.Response.ContentType = "text/plain";
					byte[] bytes = Encoding.UTF8.GetBytes(inString);
					this.Response.ContentLength64 = (long)bytes.Length;
					this.Response.OutputStream.Write(bytes, 0, bytes.Length);
				}
			}

			// Token: 0x04000477 RID: 1143
			public HttpListenerContext context;

			// Token: 0x04000478 RID: 1144
			public string path;
		}

		// Token: 0x02000082 RID: 130
		[Serializable]
		public class QueuedLog
		{
			// Token: 0x04000479 RID: 1145
			public string message;

			// Token: 0x0400047A RID: 1146
			public string stackTrace;

			// Token: 0x0400047B RID: 1147
			public LogType type;
		}
	}
}
