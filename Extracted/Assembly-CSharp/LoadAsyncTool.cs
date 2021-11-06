using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002F5 RID: 757
public class LoadAsyncTool
{
	// Token: 0x06001A54 RID: 6740 RVA: 0x0006A71C File Offset: 0x0006891C
	public static LoadAsyncTool.ObjectRequest Get(string path, bool loadImmediately = false)
	{
		LoadAsyncTool.ObjectRequest objectRequest = (!Device.isPixelGunLow) ? LoadAsyncTool.GetFromBuffer(path) : null;
		if (objectRequest == null)
		{
			objectRequest = new LoadAsyncTool.ObjectRequest(path, loadImmediately);
			if (!Device.isPixelGunLow)
			{
				LoadAsyncTool.AddToBuffer(path, objectRequest);
			}
		}
		return objectRequest;
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x0006A760 File Offset: 0x00068960
	private static void AddToBuffer(string key, LoadAsyncTool.ObjectRequest value)
	{
		if (LoadAsyncTool.bufferDict.ContainsKey(key))
		{
			return;
		}
		if (LoadAsyncTool.keyBuffer[LoadAsyncTool.currentIndex] != null)
		{
			LoadAsyncTool.bufferDict[LoadAsyncTool.keyBuffer[LoadAsyncTool.currentIndex]].asset = null;
			LoadAsyncTool.bufferDict.Remove(LoadAsyncTool.keyBuffer[LoadAsyncTool.currentIndex]);
		}
		LoadAsyncTool.keyBuffer[LoadAsyncTool.currentIndex] = key;
		LoadAsyncTool.bufferDict.Add(key, value);
		LoadAsyncTool.currentIndex++;
		if (LoadAsyncTool.currentIndex >= LoadAsyncTool.keyBuffer.Length)
		{
			if (Device.isPixelGunLow)
			{
				Resources.UnloadUnusedAssets();
				Debug.Log("<color=#FF5555>Resources.UnloadUnusedAssets</color>");
			}
			LoadAsyncTool.currentIndex = 0;
		}
	}

	// Token: 0x06001A56 RID: 6742 RVA: 0x0006A814 File Offset: 0x00068A14
	private static LoadAsyncTool.ObjectRequest GetFromBuffer(string key)
	{
		if (LoadAsyncTool.bufferDict.ContainsKey(key))
		{
			return LoadAsyncTool.bufferDict[key];
		}
		return null;
	}

	// Token: 0x04000F6F RID: 3951
	private static Dictionary<string, LoadAsyncTool.ObjectRequest> bufferDict = new Dictionary<string, LoadAsyncTool.ObjectRequest>();

	// Token: 0x04000F70 RID: 3952
	private static string[] keyBuffer = new string[70];

	// Token: 0x04000F71 RID: 3953
	private static int currentIndex;

	// Token: 0x020002F6 RID: 758
	public class ObjectRequest
	{
		// Token: 0x06001A57 RID: 6743 RVA: 0x0006A834 File Offset: 0x00068A34
		public ObjectRequest(string path, bool loadImmediately)
		{
			this.assetPath = path;
			if (!loadImmediately)
			{
				Debug.Log("<color=#5555FF>Load: " + this.assetPath + "</color>");
				this.request = Resources.LoadAsync(this.assetPath);
			}
			else
			{
				this.LoadImmediately();
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001A58 RID: 6744 RVA: 0x0006A88C File Offset: 0x00068A8C
		// (set) Token: 0x06001A59 RID: 6745 RVA: 0x0006A8C4 File Offset: 0x00068AC4
		public UnityEngine.Object asset
		{
			get
			{
				if (this._asset == null && !this.isDone)
				{
					this.LoadImmediately();
				}
				return this._asset;
			}
			set
			{
				this._asset = value;
			}
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x0006A8D0 File Offset: 0x00068AD0
		public void LoadImmediately()
		{
			if (this.request != null)
			{
				this.request = null;
			}
			this.asset = Resources.Load(this.assetPath);
			this.done = true;
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x0006A904 File Offset: 0x00068B04
		public bool isDone
		{
			get
			{
				if (this.done)
				{
					return true;
				}
				if (this.request.isDone)
				{
					Debug.Log("<color=#5555FF>Request done: " + this.assetPath + "</color>");
					this.asset = this.request.asset;
					this.request = null;
					this.done = true;
					return true;
				}
				return false;
			}
		}

		// Token: 0x04000F72 RID: 3954
		private ResourceRequest request;

		// Token: 0x04000F73 RID: 3955
		private bool done;

		// Token: 0x04000F74 RID: 3956
		private string assetPath;

		// Token: 0x04000F75 RID: 3957
		private UnityEngine.Object _asset;
	}
}
