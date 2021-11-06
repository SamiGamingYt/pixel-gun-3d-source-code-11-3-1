using System;
using System.Collections.Generic;
using System.Globalization;
using Facebook.MiniJSON;

namespace Facebook.Unity.Canvas
{
	// Token: 0x020000BB RID: 187
	internal sealed class CanvasFacebook : FacebookBase, ICanvasFacebook, ICanvasFacebookCallbackHandler, ICanvasFacebookImplementation, IFacebook, IFacebookCallbackHandler
	{
		// Token: 0x0600057C RID: 1404 RVA: 0x0002B920 File Offset: 0x00029B20
		public CanvasFacebook() : this(new CanvasJSWrapper(), new CallbackManager())
		{
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0002B934 File Offset: 0x00029B34
		public CanvasFacebook(ICanvasJSWrapper canvasJSWrapper, CallbackManager callbackManager) : base(callbackManager)
		{
			this.canvasJSWrapper = canvasJSWrapper;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0002B944 File Offset: 0x00029B44
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x0002B94C File Offset: 0x00029B4C
		public override bool LimitEventUsage { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0002B958 File Offset: 0x00029B58
		public override string SDKName
		{
			get
			{
				return "FBJSSDK";
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0002B960 File Offset: 0x00029B60
		public override string SDKVersion
		{
			get
			{
				return this.canvasJSWrapper.GetSDKVersion();
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0002B970 File Offset: 0x00029B70
		public override string SDKUserAgent
		{
			get
			{
				FacebookUnityPlatform currentPlatform = Constants.CurrentPlatform;
				string productName;
				if (currentPlatform != FacebookUnityPlatform.WebGL && currentPlatform != FacebookUnityPlatform.WebPlayer)
				{
					FacebookLogger.Warn("Currently running on uknown web platform");
					productName = "FBUnityWebUnknown";
				}
				else
				{
					productName = string.Format(CultureInfo.InvariantCulture, "FBUnity{0}", new object[]
					{
						Constants.CurrentPlatform.ToString()
					});
				}
				return string.Format(CultureInfo.InvariantCulture, "{0} {1}", new object[]
				{
					base.SDKUserAgent,
					Utilities.GetUserAgent(productName, FacebookSdkVersion.Build)
				});
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0002BA04 File Offset: 0x00029C04
		public void Init(string appId, bool cookie, bool logging, bool status, bool xfbml, string channelUrl, string authResponse, bool frictionlessRequests, string jsSDKLocale, HideUnityDelegate hideUnityDelegate, InitDelegate onInitComplete)
		{
			if (this.canvasJSWrapper.IntegrationMethodJs == null)
			{
				throw new Exception("Cannot initialize facebook javascript");
			}
			base.Init(hideUnityDelegate, onInitComplete);
			this.canvasJSWrapper.ExternalEval(this.canvasJSWrapper.IntegrationMethodJs);
			this.appId = appId;
			bool flag = true;
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("appId", appId);
			methodArguments.AddPrimative<bool>("cookie", cookie);
			methodArguments.AddPrimative<bool>("logging", logging);
			methodArguments.AddPrimative<bool>("status", status);
			methodArguments.AddPrimative<bool>("xfbml", xfbml);
			methodArguments.AddString("channelUrl", channelUrl);
			methodArguments.AddString("authResponse", authResponse);
			methodArguments.AddPrimative<bool>("frictionlessRequests", frictionlessRequests);
			methodArguments.AddString("version", FB.GraphApiVersion);
			this.canvasJSWrapper.ExternalCall("FBUnity.init", new object[]
			{
				(!flag) ? 0 : 1,
				"https://connect.facebook.net",
				jsSDKLocale,
				(!Constants.DebugMode) ? 0 : 1,
				methodArguments.ToJsonString(),
				(!status) ? 0 : 1
			});
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0002BB40 File Offset: 0x00029D40
		public override void LogInWithPublishPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.canvasJSWrapper.DisableFullScreen();
			this.canvasJSWrapper.ExternalCall("FBUnity.login", new object[]
			{
				permissions,
				base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback)
			});
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0002BB84 File Offset: 0x00029D84
		public override void LogInWithReadPermissions(IEnumerable<string> permissions, FacebookDelegate<ILoginResult> callback)
		{
			this.canvasJSWrapper.DisableFullScreen();
			this.canvasJSWrapper.ExternalCall("FBUnity.login", new object[]
			{
				permissions,
				base.CallbackManager.AddFacebookDelegate<ILoginResult>(callback)
			});
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0002BBC8 File Offset: 0x00029DC8
		public override void LogOut()
		{
			base.LogOut();
			this.canvasJSWrapper.ExternalCall("FBUnity.logout", new object[0]);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0002BBE8 File Offset: 0x00029DE8
		public override void AppRequest(string message, OGActionType? actionType, string objectId, IEnumerable<string> to, IEnumerable<object> filters, IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, FacebookDelegate<IAppRequestResult> callback)
		{
			base.ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("message", message);
			methodArguments.AddCommaSeparatedList("to", to);
			methodArguments.AddString("action_type", (actionType == null) ? null : actionType.ToString());
			methodArguments.AddString("object_id", objectId);
			methodArguments.AddList<object>("filters", filters);
			methodArguments.AddList<string>("exclude_ids", excludeIds);
			methodArguments.AddNullablePrimitive<int>("max_recipients", maxRecipients);
			methodArguments.AddString("data", data);
			methodArguments.AddString("title", title);
			new CanvasFacebook.CanvasUIMethodCall<IAppRequestResult>(this, "apprequests", "OnAppRequestsComplete")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0002BCBC File Offset: 0x00029EBC
		public override void ActivateApp(string appId)
		{
			this.canvasJSWrapper.ExternalCall("FBUnity.activateApp", new object[0]);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0002BCD4 File Offset: 0x00029ED4
		public override void ShareLink(Uri contentURL, string contentTitle, string contentDescription, Uri photoURL, FacebookDelegate<IShareResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddUri("link", contentURL);
			methodArguments.AddString("name", contentTitle);
			methodArguments.AddString("description", contentDescription);
			methodArguments.AddUri("picture", photoURL);
			new CanvasFacebook.CanvasUIMethodCall<IShareResult>(this, "feed", "OnShareLinkComplete")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0002BD38 File Offset: 0x00029F38
		public override void FeedShare(string toId, Uri link, string linkName, string linkCaption, string linkDescription, Uri picture, string mediaSource, FacebookDelegate<IShareResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("to", toId);
			methodArguments.AddUri("link", link);
			methodArguments.AddString("name", linkName);
			methodArguments.AddString("caption", linkCaption);
			methodArguments.AddString("description", linkDescription);
			methodArguments.AddUri("picture", picture);
			methodArguments.AddString("source", mediaSource);
			new CanvasFacebook.CanvasUIMethodCall<IShareResult>(this, "feed", "OnShareLinkComplete")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0002BDC4 File Offset: 0x00029FC4
		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, FacebookDelegate<IPayResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("product", product);
			methodArguments.AddString("action", action);
			methodArguments.AddPrimative<int>("quantity", quantity);
			methodArguments.AddNullablePrimitive<int>("quantity_min", quantityMin);
			methodArguments.AddNullablePrimitive<int>("quantity_max", quantityMax);
			methodArguments.AddString("request_id", requestId);
			methodArguments.AddString("pricepoint_id", pricepointId);
			methodArguments.AddString("test_currency", testCurrency);
			new CanvasFacebook.CanvasUIMethodCall<IPayResult>(this, "pay", "OnPayComplete")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0002BE5C File Offset: 0x0002A05C
		public override void GameGroupCreate(string name, string description, string privacy, FacebookDelegate<IGroupCreateResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("name", name);
			methodArguments.AddString("description", description);
			methodArguments.AddString("privacy", privacy);
			methodArguments.AddString("display", "async");
			new CanvasFacebook.CanvasUIMethodCall<IGroupCreateResult>(this, "game_group_create", "OnGroupCreateComplete")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0002BEC4 File Offset: 0x0002A0C4
		public override void GameGroupJoin(string id, FacebookDelegate<IGroupJoinResult> callback)
		{
			MethodArguments methodArguments = new MethodArguments();
			methodArguments.AddString("id", id);
			methodArguments.AddString("display", "async");
			new CanvasFacebook.CanvasUIMethodCall<IGroupJoinResult>(this, "game_group_join", "OnJoinGroupComplete")
			{
				Callback = callback
			}.Call(methodArguments);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0002BF14 File Offset: 0x0002A114
		public override void GetAppLink(FacebookDelegate<IAppLinkResult> callback)
		{
			Dictionary<string, object> obj = new Dictionary<string, object>
			{
				{
					"url",
					this.appLinkUrl
				}
			};
			callback(new AppLinkResult(Json.Serialize(obj)));
			this.appLinkUrl = string.Empty;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0002BF58 File Offset: 0x0002A158
		public override void AppEventsLogEvent(string logEvent, float? valueToSum, Dictionary<string, object> parameters)
		{
			this.canvasJSWrapper.ExternalCall("FBUnity.logAppEvent", new object[]
			{
				logEvent,
				valueToSum,
				Json.Serialize(parameters)
			});
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0002BF94 File Offset: 0x0002A194
		public override void AppEventsLogPurchase(float logPurchase, string currency, Dictionary<string, object> parameters)
		{
			this.canvasJSWrapper.ExternalCall("FBUnity.logPurchase", new object[]
			{
				logPurchase,
				currency,
				Json.Serialize(parameters)
			});
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0002BFD0 File Offset: 0x0002A1D0
		public override void OnLoginComplete(string responseJsonData)
		{
			string response = CanvasFacebook.FormatAuthResponse(responseJsonData);
			base.OnAuthResponse(new LoginResult(response));
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x0002BFF0 File Offset: 0x0002A1F0
		public override void OnGetAppLinkComplete(string message)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0002BFF8 File Offset: 0x0002A1F8
		public void OnFacebookAuthResponseChange(string responseJsonData)
		{
			string response = CanvasFacebook.FormatAuthResponse(responseJsonData);
			LoginResult loginResult = new LoginResult(response);
			AccessToken.CurrentAccessToken = loginResult.AccessToken;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0002C020 File Offset: 0x0002A220
		public void OnPayComplete(string responseJsonData)
		{
			string result = CanvasFacebook.FormatResult(responseJsonData);
			PayResult result2 = new PayResult(result);
			base.CallbackManager.OnFacebookResponse(result2);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0002C048 File Offset: 0x0002A248
		public override void OnAppRequestsComplete(string responseJsonData)
		{
			string result = CanvasFacebook.FormatResult(responseJsonData);
			AppRequestResult result2 = new AppRequestResult(result);
			base.CallbackManager.OnFacebookResponse(result2);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0002C070 File Offset: 0x0002A270
		public override void OnShareLinkComplete(string responseJsonData)
		{
			string result = CanvasFacebook.FormatResult(responseJsonData);
			ShareResult result2 = new ShareResult(result);
			base.CallbackManager.OnFacebookResponse(result2);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0002C098 File Offset: 0x0002A298
		public override void OnGroupCreateComplete(string responseJsonData)
		{
			string result = CanvasFacebook.FormatResult(responseJsonData);
			GroupCreateResult result2 = new GroupCreateResult(result);
			base.CallbackManager.OnFacebookResponse(result2);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0002C0C0 File Offset: 0x0002A2C0
		public override void OnGroupJoinComplete(string responseJsonData)
		{
			string result = CanvasFacebook.FormatResult(responseJsonData);
			GroupJoinResult result2 = new GroupJoinResult(result);
			base.CallbackManager.OnFacebookResponse(result2);
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0002C0E8 File Offset: 0x0002A2E8
		public void OnUrlResponse(string url)
		{
			this.appLinkUrl = url;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0002C0F4 File Offset: 0x0002A2F4
		private static string FormatAuthResponse(string result)
		{
			if (string.IsNullOrEmpty(result))
			{
				return result;
			}
			IDictionary<string, object> formattedResponseDictionary = CanvasFacebook.GetFormattedResponseDictionary(result);
			IDictionary<string, object> dictionary;
			if (formattedResponseDictionary.TryGetValue("authResponse", out dictionary))
			{
				formattedResponseDictionary.Remove("authResponse");
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					formattedResponseDictionary[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			return Json.Serialize(formattedResponseDictionary);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0002C198 File Offset: 0x0002A398
		private static string FormatResult(string result)
		{
			if (string.IsNullOrEmpty(result))
			{
				return result;
			}
			return Json.Serialize(CanvasFacebook.GetFormattedResponseDictionary(result));
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0002C1B4 File Offset: 0x0002A3B4
		private static IDictionary<string, object> GetFormattedResponseDictionary(string result)
		{
			IDictionary<string, object> dictionary = (IDictionary<string, object>)Json.Deserialize(result);
			IDictionary<string, object> dictionary2;
			if (dictionary.TryGetValue("response", out dictionary2))
			{
				object value;
				if (dictionary.TryGetValue("callback_id", out value))
				{
					dictionary2["callback_id"] = value;
				}
				return dictionary2;
			}
			return dictionary;
		}

		// Token: 0x040005F6 RID: 1526
		internal const string MethodAppRequests = "apprequests";

		// Token: 0x040005F7 RID: 1527
		internal const string MethodFeed = "feed";

		// Token: 0x040005F8 RID: 1528
		internal const string MethodPay = "pay";

		// Token: 0x040005F9 RID: 1529
		internal const string MethodGameGroupCreate = "game_group_create";

		// Token: 0x040005FA RID: 1530
		internal const string MethodGameGroupJoin = "game_group_join";

		// Token: 0x040005FB RID: 1531
		internal const string CancelledResponse = "{\"cancelled\":true}";

		// Token: 0x040005FC RID: 1532
		internal const string FacebookConnectURL = "https://connect.facebook.net";

		// Token: 0x040005FD RID: 1533
		private const string AuthResponseKey = "authResponse";

		// Token: 0x040005FE RID: 1534
		private const string ResponseKey = "response";

		// Token: 0x040005FF RID: 1535
		private string appId;

		// Token: 0x04000600 RID: 1536
		private string appLinkUrl;

		// Token: 0x04000601 RID: 1537
		private ICanvasJSWrapper canvasJSWrapper;

		// Token: 0x020000BC RID: 188
		private class CanvasUIMethodCall<T> : MethodCall<T> where T : IResult
		{
			// Token: 0x0600059D RID: 1437 RVA: 0x0002C200 File Offset: 0x0002A400
			public CanvasUIMethodCall(CanvasFacebook canvasImpl, string methodName, string callbackMethod) : base(canvasImpl, methodName)
			{
				this.canvasImpl = canvasImpl;
				this.callbackMethod = callbackMethod;
			}

			// Token: 0x0600059E RID: 1438 RVA: 0x0002C218 File Offset: 0x0002A418
			public override void Call(MethodArguments args)
			{
				this.UI(base.MethodName, args, base.Callback);
			}

			// Token: 0x0600059F RID: 1439 RVA: 0x0002C238 File Offset: 0x0002A438
			private void UI(string method, MethodArguments args, FacebookDelegate<T> callback = null)
			{
				this.canvasImpl.canvasJSWrapper.DisableFullScreen();
				MethodArguments methodArguments = new MethodArguments(args);
				methodArguments.AddString("app_id", this.canvasImpl.appId);
				methodArguments.AddString("method", method);
				string text = this.canvasImpl.CallbackManager.AddFacebookDelegate<T>(callback);
				this.canvasImpl.canvasJSWrapper.ExternalCall("FBUnity.ui", new object[]
				{
					methodArguments.ToJsonString(),
					text,
					this.callbackMethod
				});
			}

			// Token: 0x04000603 RID: 1539
			private CanvasFacebook canvasImpl;

			// Token: 0x04000604 RID: 1540
			private string callbackMethod;
		}
	}
}
