using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x0200005F RID: 95
public class ChatController : MonoBehaviour
{
	// Token: 0x06000283 RID: 643 RVA: 0x00016214 File Offset: 0x00014414
	public static void FillPrivatMessageFromPrefs()
	{
		ChatController.privateMessages.Clear();
		int num = 0;
		string @string = PlayerPrefs.GetString(ChatController.privateMessageKey, "{}");
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary != null && dictionary.Count > 0)
		{
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				List<object> list = keyValuePair.Value as List<object>;
				foreach (object obj in list)
				{
					Dictionary<string, object> dictionary2 = obj as Dictionary<string, object>;
					string playerIDFrom = dictionary2["playerIDFrom"] as string;
					string message = dictionary2["message"] as string;
					double timeStamp = (double)Tools.CurrentUnixTime;
					double num2;
					if (double.TryParse(dictionary2["timeStamp"].ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out num2))
					{
						timeStamp = num2;
					}
					else
					{
						Debug.LogWarning(string.Format("Could not parse timestamp {0}", keyValuePair.Value));
					}
					bool flag = bool.Parse(dictionary2["isRead"] as string);
					if (!flag)
					{
						num++;
					}
					bool isSending = bool.Parse(dictionary2["isSending"] as string);
					ChatController.PrivateMessage message2 = new ChatController.PrivateMessage(playerIDFrom, message, timeStamp, isSending, flag);
					ChatController.AddPrivateMessage(keyValuePair.Key, message2);
				}
			}
		}
		ChatController.countNewPrivateMessage = num;
	}

	// Token: 0x06000284 RID: 644 RVA: 0x000163E4 File Offset: 0x000145E4
	public static void FillPrivateMessageForSendFromPrefs()
	{
		ChatController.privateMessagesForSend.Clear();
		string @string = PlayerPrefs.GetString(ChatController.privateMessageSendKey, "[]");
		List<object> list = Json.Deserialize(@string) as List<object>;
		if (list != null && list.Count > 0)
		{
			foreach (object obj in list)
			{
				Dictionary<string, string> dictionary = obj as Dictionary<string, string>;
				string text = dictionary["to"];
				string message = dictionary["text"];
				double timeStamp = double.Parse(dictionary["timeStamp"], NumberStyles.Number, CultureInfo.InvariantCulture);
				bool isSending = true;
				bool isRead = false;
				if (!ChatController.privateMessagesForSend.ContainsKey(text))
				{
					ChatController.privateMessagesForSend.Add(text, new List<ChatController.PrivateMessage>());
				}
				ChatController.privateMessagesForSend[text].Add(new ChatController.PrivateMessage(text, message, timeStamp, isSending, isRead));
			}
		}
	}

	// Token: 0x06000285 RID: 645 RVA: 0x00016500 File Offset: 0x00014700
	public static void SavePrivatMessageInPrefs()
	{
		Dictionary<string, List<Dictionary<string, string>>> dictionary = new Dictionary<string, List<Dictionary<string, string>>>();
		foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> keyValuePair in ChatController.privateMessages)
		{
			List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			foreach (ChatController.PrivateMessage privateMessage in keyValuePair.Value)
			{
				list.Add(new Dictionary<string, string>
				{
					{
						"playerIDFrom",
						privateMessage.playerIDFrom
					},
					{
						"message",
						privateMessage.message
					},
					{
						"timeStamp",
						privateMessage.timeStamp.ToString("F8", CultureInfo.InvariantCulture)
					},
					{
						"isRead",
						privateMessage.isRead.ToString()
					},
					{
						"isSending",
						privateMessage.isSending.ToString()
					}
				});
			}
			dictionary.Add(keyValuePair.Key, list);
		}
		string text = Json.Serialize(dictionary);
		PlayerPrefs.SetString(ChatController.privateMessageKey, text ?? "{}");
		List<Dictionary<string, string>> list2 = new List<Dictionary<string, string>>();
		foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> keyValuePair2 in ChatController.privateMessagesForSend)
		{
			List<Dictionary<string, string>> list3 = new List<Dictionary<string, string>>();
			foreach (ChatController.PrivateMessage privateMessage2 in keyValuePair2.Value)
			{
				list2.Add(new Dictionary<string, string>
				{
					{
						"to",
						keyValuePair2.Key
					},
					{
						"text",
						privateMessage2.message
					},
					{
						"timeStamp",
						privateMessage2.timeStamp.ToString("F8", CultureInfo.InvariantCulture)
					}
				});
			}
		}
		string text2 = Json.Serialize(list2);
		PlayerPrefs.SetString(ChatController.privateMessageSendKey, text2 ?? "[]");
		PlayerPrefs.Save();
	}

	// Token: 0x06000286 RID: 646 RVA: 0x000167A0 File Offset: 0x000149A0
	public static string GetPrivateChatJsonForSend()
	{
		return PlayerPrefs.GetString(ChatController.privateMessageSendKey, "[]");
	}

	// Token: 0x06000287 RID: 647 RVA: 0x000167B4 File Offset: 0x000149B4
	public void ParseUpdateChatMessageResponse(string response)
	{
		Dictionary<string, object> dictionary = Json.Deserialize(response) as Dictionary<string, object>;
		bool flag = false;
		if (dictionary != null && dictionary.Count > 0)
		{
			if (dictionary.ContainsKey("user_messages_added"))
			{
				Dictionary<string, object> dictionary2 = dictionary["user_messages_added"] as Dictionary<string, object>;
				foreach (KeyValuePair<string, object> keyValuePair in dictionary2)
				{
					double num;
					if (!double.TryParse(keyValuePair.Key, NumberStyles.Number, CultureInfo.InvariantCulture, out num))
					{
						Debug.LogWarning(string.Format("Could not parse timestamp {0}    Current culture: {1}, current UI culture: {2}", keyValuePair.Value, CultureInfo.CurrentCulture.Name, CultureInfo.CurrentUICulture.Name));
					}
					else
					{
						foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> keyValuePair2 in ChatController.privateMessagesForSend)
						{
							int num2 = -1;
							for (int i = 0; i < keyValuePair2.Value.Count; i++)
							{
								ChatController.PrivateMessage privateMessage = keyValuePair2.Value[i];
								if (privateMessage.timeStamp == num)
								{
									num2 = i;
									double timeStamp;
									if (double.TryParse(keyValuePair.Value.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out timeStamp))
									{
										privateMessage.timeStamp = timeStamp;
									}
									string key = keyValuePair2.Key;
									ChatController.PrivateMessage message = new ChatController.PrivateMessage(FriendsController.sharedController.id, privateMessage.message, privateMessage.timeStamp, true, true);
									ChatController.AddPrivateMessage(key, message);
									flag = true;
									break;
								}
							}
							if (num2 >= 0)
							{
								keyValuePair2.Value.RemoveAt(num2);
								break;
							}
						}
					}
				}
			}
			if (dictionary.ContainsKey("user_messages"))
			{
				List<object> list = dictionary["user_messages"] as List<object>;
				foreach (object obj in list)
				{
					Dictionary<string, object> dictionary3 = obj as Dictionary<string, object>;
					foreach (KeyValuePair<string, object> keyValuePair3 in dictionary3)
					{
						Dictionary<string, object> dictionary4 = keyValuePair3.Value as Dictionary<string, object>;
						string text = dictionary4["from"] as string;
						string message2 = dictionary4["text"] as string;
						double num3 = (double)Tools.CurrentUnixTime;
						if (!double.TryParse(keyValuePair3.Key, NumberStyles.Number, CultureInfo.InvariantCulture, out num3))
						{
							Debug.LogWarning(string.Format("Could not parse message body key {0};    full response: {1}", keyValuePair3.Key, response));
						}
						double timeStamp2 = num3;
						ChatController.AddPrivateMessage(text, new ChatController.PrivateMessage(text, message2, timeStamp2, true, false));
						ChatController.countNewPrivateMessage++;
						if (PrivateChatController.sharedController != null)
						{
							PrivateChatController.sharedController.UpdateFriendItemsInfoAndSort();
						}
						flag = true;
					}
				}
			}
			if (ChatController.privateMessages != null)
			{
				List<string> list2 = new List<string>();
				foreach (KeyValuePair<string, List<ChatController.PrivateMessage>> keyValuePair4 in ChatController.privateMessages)
				{
					if (!FriendsController.sharedController.friends.Contains(keyValuePair4.Key))
					{
						foreach (ChatController.PrivateMessage privateMessage2 in keyValuePair4.Value)
						{
							if (!privateMessage2.isRead)
							{
								ChatController.countNewPrivateMessage--;
							}
						}
						list2.Add(keyValuePair4.Key);
						flag = true;
					}
				}
				foreach (string key2 in list2)
				{
					ChatController.privateMessages.Remove(key2);
				}
			}
		}
		if (flag)
		{
			ChatController.SavePrivatMessageInPrefs();
			if (PrivateChatController.sharedController != null)
			{
				PrivateChatController.sharedController.UpdateMessageForSelectedUsers(false);
			}
		}
	}

	// Token: 0x06000288 RID: 648 RVA: 0x00016C98 File Offset: 0x00014E98
	public static void AddPrivateMessage(string _playerIdChating, ChatController.PrivateMessage _message)
	{
		if (!ChatController.privateMessages.ContainsKey(_playerIdChating))
		{
			ChatController.privateMessages.Add(_playerIdChating, new List<ChatController.PrivateMessage>());
		}
		ChatController.privateMessages[_playerIdChating].Add(_message);
		while (ChatController.privateMessages[_playerIdChating].Count > Defs.historyPrivateMessageLength)
		{
			if (!ChatController.privateMessages[_playerIdChating][0].isRead)
			{
				ChatController.countNewPrivateMessage--;
			}
			ChatController.privateMessages[_playerIdChating].RemoveAt(0);
		}
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00016D30 File Offset: 0x00014F30
	private void Start()
	{
		ChatController.sharedController = this;
		ChatController.FillPrivatMessageFromPrefs();
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00016D40 File Offset: 0x00014F40
	private void OnDestroy()
	{
		ChatController.sharedController = null;
		base.StopAllCoroutines();
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00016D50 File Offset: 0x00014F50
	public void SelectPrivateChatMode()
	{
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00016D54 File Offset: 0x00014F54
	public void BackButtonClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		if (this._backButtonCallback != null)
		{
			this._backButtonCallback();
			return;
		}
		Singleton<SceneLoader>.Instance.LoadScene(Defs.MainMenuScene, LoadSceneMode.Single);
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00016DA4 File Offset: 0x00014FA4
	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
		yield break;
	}

	// Token: 0x0600028E RID: 654 RVA: 0x00016DC8 File Offset: 0x00014FC8
	public void Show(Action exitCallback)
	{
		base.gameObject.SetActive(true);
		this._backButtonCallback = exitCallback;
	}

	// Token: 0x040002BD RID: 701
	public static ChatController sharedController = null;

	// Token: 0x040002BE RID: 702
	public static int countNewPrivateMessage = 0;

	// Token: 0x040002BF RID: 703
	private static string privateMessageKey = "PrivateMessageKey";

	// Token: 0x040002C0 RID: 704
	private static string privateMessageSendKey = "PrivateMessageSendKey";

	// Token: 0x040002C1 RID: 705
	public static float timerToUpdateMessage;

	// Token: 0x040002C2 RID: 706
	public static bool fastSendMessage;

	// Token: 0x040002C3 RID: 707
	public static float maxTimerToUpdateMessage = 10f;

	// Token: 0x040002C4 RID: 708
	public static Dictionary<string, List<ChatController.PrivateMessage>> privateMessages = new Dictionary<string, List<ChatController.PrivateMessage>>();

	// Token: 0x040002C5 RID: 709
	public static Dictionary<string, List<ChatController.PrivateMessage>> privateMessagesForSend = new Dictionary<string, List<ChatController.PrivateMessage>>();

	// Token: 0x040002C6 RID: 710
	private Action _backButtonCallback;

	// Token: 0x02000060 RID: 96
	public struct PrivateMessage
	{
		// Token: 0x0600028F RID: 655 RVA: 0x00016DE0 File Offset: 0x00014FE0
		public PrivateMessage(string _playerIDFrom, string _message, double _timeStamp, bool _isSending, bool _isRead)
		{
			this.playerIDFrom = _playerIDFrom;
			this.message = _message;
			this.timeStamp = _timeStamp;
			this.isSending = _isSending;
			this.isRead = _isRead;
		}

		// Token: 0x040002C7 RID: 711
		public string playerIDFrom;

		// Token: 0x040002C8 RID: 712
		public string message;

		// Token: 0x040002C9 RID: 713
		public double timeStamp;

		// Token: 0x040002CA RID: 714
		public bool isRead;

		// Token: 0x040002CB RID: 715
		public bool isSending;
	}
}
