using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200060F RID: 1551
	internal sealed class FriendProfileController : IDisposable
	{
		// Token: 0x0600351B RID: 13595 RVA: 0x00112C10 File Offset: 0x00110E10
		public FriendProfileController(IFriendsGUIController friendsGuiController, bool oldInterface = true)
		{
			this.Initialize(friendsGuiController, oldInterface);
		}

		// Token: 0x0600351C RID: 13596 RVA: 0x00112C2C File Offset: 0x00110E2C
		public FriendProfileController(Action<bool> onCloseEvent)
		{
			this.Initialize(null, false);
			this.OnCloseEvent = onCloseEvent;
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x0600351E RID: 13598 RVA: 0x00112C60 File Offset: 0x00110E60
		public GameObject FriendProfileGo
		{
			get
			{
				return this._friendProfileViewGo;
			}
		}

		// Token: 0x0600351F RID: 13599 RVA: 0x00112C68 File Offset: 0x00110E68
		private void SetTitle(string playerId, ProfileWindowType type)
		{
			bool flag = FriendsController.IsPlayerOurClanMember(playerId);
			if (this._isPlayerOurFriend && flag)
			{
				if (type == ProfileWindowType.clan)
				{
					this._friendProfileView.SetTitle(LocalizationStore.Get("Key_1527"));
				}
				else
				{
					this._friendProfileView.SetTitle(LocalizationStore.Get("Key_1526"));
				}
			}
			else if (this._isPlayerOurFriend)
			{
				this._friendProfileView.SetTitle(LocalizationStore.Get("Key_1526"));
			}
			else if (flag)
			{
				this._friendProfileView.SetTitle(LocalizationStore.Get("Key_1527"));
			}
			else
			{
				this._friendProfileView.SetTitle(LocalizationStore.Get("Key_1525"));
			}
		}

		// Token: 0x06003520 RID: 13600 RVA: 0x00112D24 File Offset: 0x00110F24
		private void SetupStateBottomButtons(string playerId, ProfileWindowType type)
		{
			bool flag = FriendsController.IsPlayerOurClanMember(playerId);
			bool flag2 = FriendsController.IsSelfClanLeader();
			bool flag3 = FriendsController.IsMyPlayerId(playerId);
			bool flag4 = FriendsController.IsAlreadySendInvitePlayer(playerId);
			bool flag5 = FriendsController.IsAlreadySendClanInvitePlayer(playerId);
			bool flag6 = FriendsController.IsFriendsMax();
			bool flag7 = FriendsController.IsMaxClanMembers();
			bool activeChatButton = this._isPlayerOurFriend && type == ProfileWindowType.friend && !flag3;
			bool flag8 = !flag && flag2 && !flag3 && !flag7;
			bool flag9 = !this._isPlayerOurFriend && !flag3 && !flag6;
			bool activeRemoveButton = this._isPlayerOurFriend && !flag3;
			this._friendProfileView.SetActiveAddButton(flag9 && !flag4);
			this._friendProfileView.SetActiveAddButtonSent(flag9 && flag4);
			this._friendProfileView.SetActiveInviteButton(flag8 && !flag5);
			this._friendProfileView.SetActiveAddClanButtonSent(flag8 && flag5);
			this._friendProfileView.SetActiveChatButton(activeChatButton);
			this._friendProfileView.SetActiveRemoveButton(activeRemoveButton);
		}

		// Token: 0x06003521 RID: 13601 RVA: 0x00112E44 File Offset: 0x00111044
		private void SetWindowStateByFriendAndClanData(string playerId, ProfileWindowType type)
		{
			this.SetTitle(playerId, type);
			this.SetupStateBottomButtons(playerId, type);
		}

		// Token: 0x06003522 RID: 13602 RVA: 0x00112E58 File Offset: 0x00111058
		private void Initialize(IFriendsGUIController friendsGuiController, bool oldInterface = true)
		{
			this._friendsGuiController = friendsGuiController;
			string path = (!oldInterface) ? "FriendProfileView(UI)" : "FriendProfileView";
			this._friendProfileViewGo = (UnityEngine.Object.Instantiate(Resources.Load(path)) as GameObject);
			if (this._friendProfileViewGo == null)
			{
				this._disposed = true;
				return;
			}
			this._friendProfileViewGo.SetActive(false);
			this._friendProfileView = this._friendProfileViewGo.GetComponent<FriendProfileView>();
			if (this._friendProfileView == null)
			{
				UnityEngine.Object.DestroyObject(this._friendProfileViewGo);
				this._friendProfileViewGo = null;
				this._disposed = true;
				return;
			}
			FriendPreviewClicker.FriendPreviewClicked += this.HandleProfileClicked;
			this._friendProfileView.BackButtonClickEvent += this.HandleBackClicked;
			this._friendProfileView.JoinButtonClickEvent += this.HandleJoinClicked;
			this._friendProfileView.CopyMyIdButtonClickEvent += this.HandleCopyMyIdClicked;
			this._friendProfileView.ChatButtonClickEvent += this.HandleChatClicked;
			this._friendProfileView.AddButtonClickEvent += this.HandleAddFriendClicked;
			this._friendProfileView.RemoveButtonClickEvent += this.HandleRemoveFriendClicked;
			this._friendProfileView.InviteToClanButtonClickEvent += this.HandleInviteToClanClicked;
			this._friendProfileView.UpdateRequested += this.HandleUpdateRequested;
			FriendsController.FullInfoUpdated += this.HandleUpdateRequested;
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x00112FD4 File Offset: 0x001111D4
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			FriendPreviewClicker.FriendPreviewClicked -= this.HandleProfileClicked;
			this._friendProfileView.BackButtonClickEvent -= this.HandleBackClicked;
			this._friendProfileView.JoinButtonClickEvent -= this.HandleJoinClicked;
			this._friendProfileView.CopyMyIdButtonClickEvent -= this.HandleCopyMyIdClicked;
			this._friendProfileView.ChatButtonClickEvent -= this.HandleChatClicked;
			this._friendProfileView.AddButtonClickEvent -= this.HandleAddFriendClicked;
			this._friendProfileView.RemoveButtonClickEvent -= this.HandleRemoveFriendClicked;
			this._friendProfileView.InviteToClanButtonClickEvent -= this.HandleInviteToClanClicked;
			this._friendProfileView.UpdateRequested -= this.HandleUpdateRequested;
			FriendsController.FullInfoUpdated -= this.HandleUpdateRequested;
			this._friendProfileView = null;
			UnityEngine.Object.DestroyObject(this._friendProfileViewGo);
			this._friendProfileViewGo = null;
			this._disposed = true;
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x001130E8 File Offset: 0x001112E8
		private void SetDefaultStateProfile()
		{
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x001130EC File Offset: 0x001112EC
		private void UpdateAllData(string friendId)
		{
			Dictionary<string, object> fullPlayerDataById = FriendsController.GetFullPlayerDataById(friendId);
			if (fullPlayerDataById == null)
			{
				return;
			}
			this.UpdatePlayer(fullPlayerDataById);
			this.UpdateScores(fullPlayerDataById);
			this.UpdateAccessories(fullPlayerDataById);
			FriendsController sharedController = FriendsController.sharedController;
			if (sharedController != null)
			{
				Dictionary<string, Dictionary<string, string>> onlineInfo = sharedController.onlineInfo;
				if (onlineInfo.ContainsKey(friendId))
				{
					this.UpdateOnline(onlineInfo[friendId]);
				}
				else if (this._isPlayerOurFriend)
				{
					this._friendProfileView.Online = OnlineState.offline;
				}
				else
				{
					this._friendProfileView.Online = OnlineState.none;
				}
			}
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x0011317C File Offset: 0x0011137C
		private void Update()
		{
			if (string.IsNullOrEmpty(this._friendId))
			{
				return;
			}
			this.UpdateAllData(this._friendId);
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x0011319C File Offset: 0x0011139C
		private void UpdateAccessories(Dictionary<string, object> playerInfo)
		{
			if (playerInfo == null || playerInfo.Count == 0)
			{
				return;
			}
			object obj;
			if (playerInfo.TryGetValue("accessories", out obj))
			{
				List<object> list = obj as List<object>;
				if (list != null)
				{
					IEnumerable<Dictionary<string, object>> enumerable = list.OfType<Dictionary<string, object>>();
					foreach (Dictionary<string, object> dictionary in enumerable)
					{
						string text = string.Empty;
						object obj2;
						if (dictionary.TryGetValue("name", out obj2))
						{
							text = ((obj2 as string) ?? string.Empty);
						}
						object obj3;
						int num;
						if (dictionary.TryGetValue("type", out obj3) && int.TryParse(obj3 as string, out num))
						{
							switch (num)
							{
							case 0:
								if (text.Equals("cape_Custom", StringComparison.Ordinal))
								{
									object obj4;
									if (dictionary.TryGetValue("skin", out obj4))
									{
										string text2 = obj4 as string;
										if (!string.IsNullOrEmpty(text2))
										{
											byte[] customCape = Convert.FromBase64String(text2);
											this._friendProfileView.SetCustomCape(customCape);
										}
									}
								}
								else
								{
									this._friendProfileView.SetStockCape(text);
								}
								break;
							case 1:
								this._friendProfileView.SetHat(text);
								break;
							case 2:
								this._friendProfileView.SetBoots(text);
								break;
							case 3:
								this._friendProfileView.SetArmor(text);
								break;
							case 4:
								this._friendProfileView.SetMask(text);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x0011335C File Offset: 0x0011155C
		private void UpdateOnline(Dictionary<string, string> onlineInfo)
		{
			FriendsController.ResultParseOnlineData resultParseOnlineData = FriendsController.ParseOnlineData(onlineInfo);
			if (resultParseOnlineData == null)
			{
				this._friendProfileView.Online = OnlineState.none;
				return;
			}
			this._friendProfileView.Online = resultParseOnlineData.GetOnlineStatus();
			this._friendProfileView.FriendGameMode = resultParseOnlineData.GetGameModeName();
			this._friendProfileView.FriendLocation = resultParseOnlineData.GetMapName();
			this._friendProfileView.IsCanConnectToFriend = resultParseOnlineData.IsCanConnect;
			this._friendProfileView.NotConnectCondition = resultParseOnlineData.GetNotConnectConditionString();
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x001133D8 File Offset: 0x001115D8
		private void UpdatePlayer(Dictionary<string, object> playerInfo)
		{
			if (playerInfo == null || playerInfo.Count == 0)
			{
				Debug.LogWarning("playerInfo == null || playerInfo.Count == 0");
				return;
			}
			Dictionary<string, object> dictionary = null;
			dictionary = (playerInfo["player"] as Dictionary<string, object>);
			if (dictionary != null)
			{
				object obj;
				int friendCount;
				if (dictionary.TryGetValue("friends", out obj) && int.TryParse(obj as string, out friendCount))
				{
					this._friendProfileView.FriendCount = friendCount;
				}
				else
				{
					this._friendProfileView.FriendCount = -1;
				}
				object obj2;
				if (dictionary.TryGetValue("nick", out obj2))
				{
					this._friendProfileView.FriendName = (obj2 as string);
				}
				object value;
				int rank;
				if (dictionary.TryGetValue("rank", out value) && int.TryParse(Convert.ToString(value), out rank))
				{
					this._friendProfileView.Rank = rank;
				}
				object obj3;
				if (dictionary.TryGetValue("skin", out obj3))
				{
					string text = obj3 as string;
					if (!string.IsNullOrEmpty(text))
					{
						byte[] array = Convert.FromBase64String(text);
						if (array != null && array.Length > 0)
						{
							this._friendProfileView.SetSkin(array);
						}
					}
				}
				object obj4;
				if (dictionary.TryGetValue("clan_name", out obj4))
				{
					this._friendProfileView.clanName.gameObject.SetActive(true);
					string text2 = obj4 as string;
					if (!string.IsNullOrEmpty(text2))
					{
						int num = 10000;
						if (text2 != null && text2.Length > num)
						{
							text2 = string.Format("{0}..{1}", text2.Substring(0, (num - 2) / 2), text2.Substring(text2.Length - (num - 2) / 2, (num - 2) / 2));
						}
						this._friendProfileView.clanName.text = (text2 ?? string.Empty);
					}
					else
					{
						this._friendProfileView.clanName.gameObject.SetActive(false);
					}
				}
				object obj5;
				if (dictionary.TryGetValue("clan_logo", out obj5))
				{
					string text3 = obj5 as string;
					if (!string.IsNullOrEmpty(text3))
					{
						this._friendProfileView.clanLogo.gameObject.SetActive(true);
						byte[] array2 = Convert.FromBase64String(text3);
						if (array2 != null && array2.Length > 0)
						{
							try
							{
								Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
								texture2D.LoadImage(array2);
								texture2D.filterMode = FilterMode.Point;
								texture2D.Apply();
								Texture mainTexture = this._friendProfileView.clanLogo.mainTexture;
								this._friendProfileView.clanLogo.mainTexture = texture2D;
								if (mainTexture != null)
								{
									UnityEngine.Object.DestroyImmediate(mainTexture, true);
								}
							}
							catch (Exception ex)
							{
								Texture mainTexture2 = this._friendProfileView.clanLogo.mainTexture;
								this._friendProfileView.clanLogo.mainTexture = null;
								if (mainTexture2 != null)
								{
									UnityEngine.Object.DestroyImmediate(mainTexture2, true);
								}
							}
						}
					}
					else
					{
						this._friendProfileView.clanLogo.gameObject.SetActive(false);
					}
				}
				string playerNameOrDefault = ProfileController.GetPlayerNameOrDefault();
				this._friendProfileView.Username = playerNameOrDefault;
				object value2;
				if (dictionary.TryGetValue("wins", out value2))
				{
					int winCount = Convert.ToInt32(value2);
					this._friendProfileView.WinCount = winCount;
				}
				else
				{
					this._friendProfileView.WinCount = -1;
				}
				object obj6;
				if (dictionary.TryGetValue("total_wins", out obj6))
				{
					int totalWinCount;
					if (int.TryParse(obj6 as string, out totalWinCount))
					{
						this._friendProfileView.TotalWinCount = totalWinCount;
					}
					else
					{
						Debug.LogWarning("Can not parse “total_wins” field: " + obj6);
					}
				}
				else
				{
					this._friendProfileView.TotalWinCount = -1;
				}
			}
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x001137A4 File Offset: 0x001119A4
		private void UpdateScores(Dictionary<string, object> playerInfo)
		{
			if (playerInfo == null || playerInfo.Count == 0)
			{
				return;
			}
			object obj;
			if (playerInfo.TryGetValue("scores", out obj))
			{
				List<object> list = obj as List<object>;
				if (list != null)
				{
					IEnumerable<Dictionary<string, object>> source = list.OfType<Dictionary<string, object>>();
					if (source.Any<Dictionary<string, object>>())
					{
						Dictionary<string, object> dictionary = source.FirstOrDefault((Dictionary<string, object> d) => d.ContainsKey("game") && d["game"].Equals("0"));
						if (dictionary != null)
						{
							object obj2;
							int survivalScore;
							if (dictionary.TryGetValue("max_score", out obj2) && int.TryParse(obj2 as string, out survivalScore))
							{
								this._friendProfileView.SurvivalScore = survivalScore;
							}
							else
							{
								this._friendProfileView.SurvivalScore = -1;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x00113864 File Offset: 0x00111A64
		internal void HandleProfileClicked(string id)
		{
			this.HandleProfileClickedCore(id, ProfileWindowType.other, null);
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x00113870 File Offset: 0x00111A70
		internal void HandleProfileClickedCore(string id, ProfileWindowType type, Action<bool> onCloseEvent)
		{
			if (this._disposed)
			{
				return;
			}
			this.OnCloseEvent = onCloseEvent;
			this._needUpdateFriendList = false;
			this._friendId = id;
			this._friendProfileView.FriendId = id;
			this._windowType = type;
			FriendProfileController.currentFriendId = id;
			this._friendProfileView.Reset();
			this._isPlayerOurFriend = FriendsController.IsPlayerOurFriend(id);
			this.Update();
			if (this._friendsGuiController != null)
			{
				this._friendsGuiController.Hide(true);
			}
			FriendsController.sharedController.StartRefreshingInfo(this._friendId);
			this._friendProfileViewGo.SetActive(true);
			this.SetWindowStateByFriendAndClanData(this._friendId, type);
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x00113914 File Offset: 0x00111B14
		public void HandleBackClicked()
		{
			this._friendProfileView.Reset();
			this._friendProfileViewGo.SetActive(false);
			FriendsController.sharedController.StopRefreshingInfo();
			if (this._friendsGuiController != null)
			{
				this._friendsGuiController.Hide(false);
			}
			else if (this.OnCloseEvent != null)
			{
				this.OnCloseEvent(this._needUpdateFriendList);
			}
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x0011397C File Offset: 0x00111B7C
		private void HandleJoinClicked()
		{
			ButtonClickSound.TryPlayClick();
			if (!this._friendProfileView.IsCanConnectToFriend)
			{
				InfoWindowController.ShowInfoBox(this._friendProfileView.NotConnectCondition);
				return;
			}
			if (FriendsController.sharedController.onlineInfo.ContainsKey(this._friendId))
			{
				int game_mode = int.Parse(FriendsController.sharedController.onlineInfo[this._friendId]["game_mode"]);
				string room_name = FriendsController.sharedController.onlineInfo[this._friendId]["room_name"];
				string text = FriendsController.sharedController.onlineInfo[this._friendId]["map"];
				SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(int.Parse(text));
				if (infoScene != null)
				{
					JoinRoomFromFrends.sharedJoinRoomFromFrends.ConnectToRoom(game_mode, room_name, text);
				}
			}
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x00113A58 File Offset: 0x00111C58
		private void HandleCopyMyIdClicked()
		{
			FriendsController.CopyPlayerIdToClipboard(this._friendId);
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x00113A68 File Offset: 0x00111C68
		private void HandleChatClicked()
		{
			this.HandleBackClicked();
			if (this._windowType == ProfileWindowType.friend)
			{
				FriendsWindowController instance = FriendsWindowController.Instance;
				if (instance != null)
				{
					instance.SetActiveChatTab(this._friendId);
				}
			}
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x00113AA4 File Offset: 0x00111CA4
		private void OnCompleteAddOrDeleteResponse(bool isComplete, bool isRequestExist, bool isAddRequest)
		{
			if (isAddRequest)
			{
				this._friendProfileView.SetEnableAddButton(true);
			}
			else
			{
				this._friendProfileView.SetEnableRemoveButton(true);
			}
			InfoWindowController.CheckShowRequestServerInfoBox(isComplete, isRequestExist);
			if (isComplete)
			{
				this._needUpdateFriendList = true;
				this._isPlayerOurFriend = FriendsController.IsPlayerOurFriend(this._friendId);
				this.SetWindowStateByFriendAndClanData(this._friendId, this._windowType);
			}
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x00113B0C File Offset: 0x00111D0C
		public void CallbackRequestDeleteFriend(bool isComplete)
		{
			AnalyticsFacade.SendCustomEvent("Social", new Dictionary<string, object>
			{
				{
					"Deleted Friends",
					"Delete"
				}
			});
			this.OnCompleteAddOrDeleteResponse(isComplete, false, false);
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x00113B44 File Offset: 0x00111D44
		public void CallbackFriendAddRequest(bool isComplete, bool isRequestExist)
		{
			this.OnCompleteAddOrDeleteResponse(isComplete, isRequestExist, true);
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x00113B50 File Offset: 0x00111D50
		private void HandleAddFriendClicked()
		{
			this._friendProfileView.SetEnableAddButton(false);
			Dictionary<string, object> socialEventParameters = new Dictionary<string, object>
			{
				{
					"Added Friends",
					"Profile"
				},
				{
					"Deleted Friends",
					"Add"
				}
			};
			FriendsController.SendFriendshipRequest(this._friendId, socialEventParameters, new Action<bool, bool>(this.CallbackFriendAddRequest));
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x00113BAC File Offset: 0x00111DAC
		private void HandleRemoveFriendClicked()
		{
			this._friendProfileView.SetEnableRemoveButton(false);
			FriendsController.DeleteFriend(this._friendId, new Action<bool>(this.CallbackRequestDeleteFriend));
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x00113BD4 File Offset: 0x00111DD4
		public void CallbackClanInviteRequest(bool isComplete, bool isRequestExist)
		{
			this._friendProfileView.SetEnableInviteClanButton(true);
			InfoWindowController.CheckShowRequestServerInfoBox(isComplete, isRequestExist);
			if (isComplete)
			{
				this.SetWindowStateByFriendAndClanData(this._friendId, this._windowType);
			}
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x00113C04 File Offset: 0x00111E04
		private void HandleInviteToClanClicked()
		{
			this._friendProfileView.SetEnableInviteClanButton(false);
			FriendsController.SendPlayerInviteToClan(this._friendId, new Action<bool, bool>(this.CallbackClanInviteRequest));
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x00113C2C File Offset: 0x00111E2C
		private void HandleUpdateRequested()
		{
			this.Update();
		}

		// Token: 0x040026F8 RID: 9976
		public static string currentFriendId;

		// Token: 0x040026F9 RID: 9977
		private bool _disposed;

		// Token: 0x040026FA RID: 9978
		private FriendProfileView _friendProfileView;

		// Token: 0x040026FB RID: 9979
		private GameObject _friendProfileViewGo;

		// Token: 0x040026FC RID: 9980
		private IFriendsGUIController _friendsGuiController;

		// Token: 0x040026FD RID: 9981
		private string _friendId = string.Empty;

		// Token: 0x040026FE RID: 9982
		private ProfileWindowType _windowType;

		// Token: 0x040026FF RID: 9983
		private bool _needUpdateFriendList;

		// Token: 0x04002700 RID: 9984
		private bool _isPlayerOurFriend;

		// Token: 0x04002701 RID: 9985
		private Action<bool> OnCloseEvent;

		// Token: 0x02000610 RID: 1552
		private enum AccessoriesType
		{
			// Token: 0x04002704 RID: 9988
			cape,
			// Token: 0x04002705 RID: 9989
			hat,
			// Token: 0x04002706 RID: 9990
			boots,
			// Token: 0x04002707 RID: 9991
			armor,
			// Token: 0x04002708 RID: 9992
			mask
		}
	}
}
