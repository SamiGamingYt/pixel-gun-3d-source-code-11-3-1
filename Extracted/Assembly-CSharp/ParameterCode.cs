using System;

// Token: 0x020003F6 RID: 1014
public class ParameterCode
{
	// Token: 0x040018EA RID: 6378
	public const byte SuppressRoomEvents = 237;

	// Token: 0x040018EB RID: 6379
	public const byte EmptyRoomTTL = 236;

	// Token: 0x040018EC RID: 6380
	public const byte PlayerTTL = 235;

	// Token: 0x040018ED RID: 6381
	public const byte EventForward = 234;

	// Token: 0x040018EE RID: 6382
	[Obsolete("Use: IsInactive")]
	public const byte IsComingBack = 233;

	// Token: 0x040018EF RID: 6383
	public const byte IsInactive = 233;

	// Token: 0x040018F0 RID: 6384
	public const byte CheckUserOnJoin = 232;

	// Token: 0x040018F1 RID: 6385
	public const byte ExpectedValues = 231;

	// Token: 0x040018F2 RID: 6386
	public const byte Address = 230;

	// Token: 0x040018F3 RID: 6387
	public const byte PeerCount = 229;

	// Token: 0x040018F4 RID: 6388
	public const byte GameCount = 228;

	// Token: 0x040018F5 RID: 6389
	public const byte MasterPeerCount = 227;

	// Token: 0x040018F6 RID: 6390
	public const byte UserId = 225;

	// Token: 0x040018F7 RID: 6391
	public const byte ApplicationId = 224;

	// Token: 0x040018F8 RID: 6392
	public const byte Position = 223;

	// Token: 0x040018F9 RID: 6393
	public const byte MatchMakingType = 223;

	// Token: 0x040018FA RID: 6394
	public const byte GameList = 222;

	// Token: 0x040018FB RID: 6395
	public const byte Secret = 221;

	// Token: 0x040018FC RID: 6396
	public const byte AppVersion = 220;

	// Token: 0x040018FD RID: 6397
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureNodeInfo = 210;

	// Token: 0x040018FE RID: 6398
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureLocalNodeId = 209;

	// Token: 0x040018FF RID: 6399
	[Obsolete("TCP routing was removed after becoming obsolete.")]
	public const byte AzureMasterNodeId = 208;

	// Token: 0x04001900 RID: 6400
	public const byte RoomName = 255;

	// Token: 0x04001901 RID: 6401
	public const byte Broadcast = 250;

	// Token: 0x04001902 RID: 6402
	public const byte ActorList = 252;

	// Token: 0x04001903 RID: 6403
	public const byte ActorNr = 254;

	// Token: 0x04001904 RID: 6404
	public const byte PlayerProperties = 249;

	// Token: 0x04001905 RID: 6405
	public const byte CustomEventContent = 245;

	// Token: 0x04001906 RID: 6406
	public const byte Data = 245;

	// Token: 0x04001907 RID: 6407
	public const byte Code = 244;

	// Token: 0x04001908 RID: 6408
	public const byte GameProperties = 248;

	// Token: 0x04001909 RID: 6409
	public const byte Properties = 251;

	// Token: 0x0400190A RID: 6410
	public const byte TargetActorNr = 253;

	// Token: 0x0400190B RID: 6411
	public const byte ReceiverGroup = 246;

	// Token: 0x0400190C RID: 6412
	public const byte Cache = 247;

	// Token: 0x0400190D RID: 6413
	public const byte CleanupCacheOnLeave = 241;

	// Token: 0x0400190E RID: 6414
	public const byte Group = 240;

	// Token: 0x0400190F RID: 6415
	public const byte Remove = 239;

	// Token: 0x04001910 RID: 6416
	public const byte PublishUserId = 239;

	// Token: 0x04001911 RID: 6417
	public const byte Add = 238;

	// Token: 0x04001912 RID: 6418
	public const byte Info = 218;

	// Token: 0x04001913 RID: 6419
	public const byte ClientAuthenticationType = 217;

	// Token: 0x04001914 RID: 6420
	public const byte ClientAuthenticationParams = 216;

	// Token: 0x04001915 RID: 6421
	public const byte JoinMode = 215;

	// Token: 0x04001916 RID: 6422
	public const byte ClientAuthenticationData = 214;

	// Token: 0x04001917 RID: 6423
	public const byte MasterClientId = 203;

	// Token: 0x04001918 RID: 6424
	public const byte FindFriendsRequestList = 1;

	// Token: 0x04001919 RID: 6425
	public const byte FindFriendsResponseOnlineList = 1;

	// Token: 0x0400191A RID: 6426
	public const byte FindFriendsResponseRoomIdList = 2;

	// Token: 0x0400191B RID: 6427
	public const byte LobbyName = 213;

	// Token: 0x0400191C RID: 6428
	public const byte LobbyType = 212;

	// Token: 0x0400191D RID: 6429
	public const byte LobbyStats = 211;

	// Token: 0x0400191E RID: 6430
	public const byte Region = 210;

	// Token: 0x0400191F RID: 6431
	public const byte UriPath = 209;

	// Token: 0x04001920 RID: 6432
	public const byte WebRpcParameters = 208;

	// Token: 0x04001921 RID: 6433
	public const byte WebRpcReturnCode = 207;

	// Token: 0x04001922 RID: 6434
	public const byte WebRpcReturnMessage = 206;

	// Token: 0x04001923 RID: 6435
	public const byte CacheSliceIndex = 205;

	// Token: 0x04001924 RID: 6436
	public const byte Plugins = 204;

	// Token: 0x04001925 RID: 6437
	public const byte NickName = 202;

	// Token: 0x04001926 RID: 6438
	public const byte PluginName = 201;

	// Token: 0x04001927 RID: 6439
	public const byte PluginVersion = 200;

	// Token: 0x04001928 RID: 6440
	public const byte ExpectedProtocol = 195;

	// Token: 0x04001929 RID: 6441
	public const byte CustomInitData = 194;

	// Token: 0x0400192A RID: 6442
	public const byte EncryptionMode = 193;

	// Token: 0x0400192B RID: 6443
	public const byte EncryptionData = 192;
}
