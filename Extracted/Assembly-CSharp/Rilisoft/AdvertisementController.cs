using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000547 RID: 1351
	public sealed class AdvertisementController : MonoBehaviour
	{
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06002F05 RID: 12037 RVA: 0x000F58DC File Offset: 0x000F3ADC
		public Texture2D AdvertisementTexture
		{
			get
			{
				return this._advertisementTexture;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06002F06 RID: 12038 RVA: 0x000F58E4 File Offset: 0x000F3AE4
		public AdvertisementController.State CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000F58EC File Offset: 0x000F3AEC
		public void Close()
		{
			if (this._currentState != AdvertisementController.State.Complete)
			{
				Debug.LogError("AdvertisementController cannot be started in " + this._currentState + " state.");
				return;
			}
			this._advertisementTexture = null;
			if (this._imageRequest != null)
			{
				this._imageRequest.Dispose();
				this._imageRequest = null;
			}
			this._currentState = AdvertisementController.State.Closed;
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000F5950 File Offset: 0x000F3B50
		public void Run()
		{
			if (!this._permittedStatesForRun.Contains(this._currentState))
			{
				Debug.LogError("AdvertisementController cannot be started in " + this._currentState + " state.");
				return;
			}
			if (Debug.isDebugBuild)
			{
				Debug.Log("Start checking advertisement.");
			}
			if (this._imageRequest != null)
			{
				this._imageRequest.Dispose();
				this._imageRequest = null;
			}
			if (this._checkingRequest != null)
			{
				this._checkingRequest.Dispose();
			}
			if (AdsConfigManager.Instance.LastLoadedConfig == null)
			{
				return;
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
			PerelivSettings perelivSettings = PerelivConfigManager.Instance.GetPerelivSettings(playerCategory);
			if (!string.IsNullOrEmpty(perelivSettings.Error))
			{
				Debug.LogWarning(perelivSettings.Error);
				return;
			}
			if (!perelivSettings.Enabled)
			{
				return;
			}
			if (string.IsNullOrEmpty(perelivSettings.ImageUrl))
			{
				Debug.LogWarning("Pereliv image url is empty.");
				return;
			}
			if (this.updateBanner || this.updateFromMultiBanner)
			{
				this._advertisementTexture = Resources.Load<Texture2D>("update_available");
				this._currentState = AdvertisementController.State.Complete;
			}
			else
			{
				if (Application.isEditor && FriendsController.isDebugLogWWW)
				{
					string message = string.Format("<color=yellow><size=14>PerelivSettings.ImageUrl: {0}</size></color>", perelivSettings.ImageUrl);
					Debug.Log(message);
				}
				this._checkingRequest = Tools.CreateWwwIfNotConnected(perelivSettings.ImageUrl);
				this._currentState = AdvertisementController.State.Checking;
			}
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000F5AC0 File Offset: 0x000F3CC0
		private void OnDestroy()
		{
			if (this._checkingRequest != null)
			{
				this._checkingRequest.Dispose();
			}
			if (this._imageRequest != null)
			{
				this._imageRequest.Dispose();
			}
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000F5AFC File Offset: 0x000F3CFC
		private void Update()
		{
			switch (this._currentState)
			{
			case AdvertisementController.State.Idle:
				break;
			case AdvertisementController.State.Checking:
				if (this._checkingRequest == null)
				{
					Debug.LogError("Checking request is null.");
					this._currentState = AdvertisementController.State.Idle;
				}
				else if (!string.IsNullOrEmpty(this._checkingRequest.error))
				{
					Debug.LogWarning(this._checkingRequest.error);
					this._checkingRequest.Dispose();
					this._checkingRequest = null;
					this._disabledTimeStamp = Time.time;
					this._currentState = AdvertisementController.State.Disabled;
				}
				else if (this._checkingRequest.isDone)
				{
					if (Debug.isDebugBuild)
					{
						Debug.Log("Complete checking advertisement.");
					}
					this._advertisementTexture = null;
					this._imageRequest = this._checkingRequest;
					this._checkingRequest = null;
					this._currentState = AdvertisementController.State.Downloading;
				}
				break;
			case AdvertisementController.State.Disabled:
				if (Time.time - this._disabledTimeStamp > 300f)
				{
					this._disabledTimeStamp = 0f;
					this.Run();
				}
				break;
			case AdvertisementController.State.Downloading:
				if (this._imageRequest == null)
				{
					Debug.LogError("Image request is null.");
					this._currentState = AdvertisementController.State.Idle;
				}
				else if (!string.IsNullOrEmpty(this._imageRequest.error))
				{
					Debug.LogWarning(this._imageRequest.error);
					this._currentState = AdvertisementController.State.Error;
				}
				else if (this._imageRequest.isDone)
				{
					if (Debug.isDebugBuild)
					{
						Debug.Log("Complete downloading advertisement.");
					}
					this._advertisementTexture = this._imageRequest.texture;
					this._currentState = AdvertisementController.State.Complete;
				}
				break;
			case AdvertisementController.State.Error:
				break;
			case AdvertisementController.State.Complete:
				break;
			case AdvertisementController.State.Closed:
				break;
			default:
				Debug.LogError("Unknown state.");
				break;
			}
		}

		// Token: 0x040022B2 RID: 8882
		public bool updateBanner;

		// Token: 0x040022B3 RID: 8883
		public bool updateFromMultiBanner;

		// Token: 0x040022B4 RID: 8884
		private Texture2D _advertisementTexture;

		// Token: 0x040022B5 RID: 8885
		private AdvertisementController.State _currentState;

		// Token: 0x040022B6 RID: 8886
		private WWW _checkingRequest;

		// Token: 0x040022B7 RID: 8887
		private float _disabledTimeStamp;

		// Token: 0x040022B8 RID: 8888
		private WWW _imageRequest;

		// Token: 0x040022B9 RID: 8889
		private readonly HashSet<AdvertisementController.State> _permittedStatesForRun = new HashSet<AdvertisementController.State>
		{
			AdvertisementController.State.Idle,
			AdvertisementController.State.Disabled,
			AdvertisementController.State.Closed
		};

		// Token: 0x02000548 RID: 1352
		public enum State
		{
			// Token: 0x040022BB RID: 8891
			Idle,
			// Token: 0x040022BC RID: 8892
			Checking,
			// Token: 0x040022BD RID: 8893
			Disabled,
			// Token: 0x040022BE RID: 8894
			Downloading,
			// Token: 0x040022BF RID: 8895
			Error,
			// Token: 0x040022C0 RID: 8896
			Complete,
			// Token: 0x040022C1 RID: 8897
			Closed
		}
	}
}
