using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Rilisoft;
using UnityEngine;

// Token: 0x020003CA RID: 970
[DisallowMultipleComponent]
internal sealed class NewsLobbyItem : MonoBehaviour
{
	// Token: 0x0600234B RID: 9035 RVA: 0x000AF664 File Offset: 0x000AD864
	public void LoadPreview(string url)
	{
		base.StartCoroutine(this.LoadPreviewPicture(url));
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x000AF674 File Offset: 0x000AD874
	private IEnumerator LoadPreviewPicture(string picLink)
	{
		if (this.previewPic.mainTexture != null && this.previewPicUrl == picLink)
		{
			yield break;
		}
		this.previewPic.width = 100;
		if (this.previewPic.mainTexture != null)
		{
			UnityEngine.Object.Destroy(this.previewPic.mainTexture);
		}
		Task<bool> currentlyRunningRequest;
		if (NewsLobbyItem.s_currentlyRunningRequests.TryGetValue(picLink, out currentlyRunningRequest))
		{
			if (Defs.IsDeveloperBuild && currentlyRunningRequest.IsCompleted)
			{
				Debug.LogFormat("Request is completed: {0}", new object[]
				{
					picLink
				});
			}
			float finishWaiting = Time.realtimeSinceStartup + 5f;
			while (!currentlyRunningRequest.IsCompleted)
			{
				if (finishWaiting < Time.realtimeSinceStartup && Defs.IsDeveloperBuild)
				{
					Debug.LogFormat("Stop waiting for completion: {0}", new object[]
					{
						picLink
					});
					break;
				}
				yield return null;
			}
		}
		string cachePath = PersistentCache.Instance.GetCachePathByUri(picLink);
		if (!string.IsNullOrEmpty(cachePath))
		{
			try
			{
				bool cacheExists = File.Exists(cachePath);
				if (Defs.IsDeveloperBuild && !cacheExists)
				{
					string formattedPath = (!Application.isEditor) ? cachePath : string.Format("<color=magenta>{0}</color>", cachePath);
					Debug.LogFormat("Cache miss: '{0}'", new object[]
					{
						formattedPath
					});
				}
				if (cacheExists)
				{
					byte[] cacheBytes = File.ReadAllBytes(cachePath);
					Texture2D cachedTexture = new Texture2D(2, 2);
					cachedTexture.LoadImage(cacheBytes);
					cachedTexture.filterMode = FilterMode.Point;
					this.previewPicUrl = picLink;
					this.previewPic.mainTexture = cachedTexture;
					this.previewPic.mainTexture.filterMode = FilterMode.Point;
					this.previewPic.width = 100;
					yield break;
				}
			}
			catch (Exception ex4)
			{
				Exception ex = ex4;
				Debug.LogWarning("Caught exception while reading cached preview. See next message for details.");
				Debug.LogException(ex);
			}
		}
		WWW loadPic = Tools.CreateWwwIfNotConnected(picLink);
		if (loadPic == null)
		{
			yield return new WaitForSeconds(60f);
			base.StartCoroutine(this.LoadPreviewPicture(picLink));
			yield break;
		}
		TaskCompletionSource<bool> promise = new TaskCompletionSource<bool>();
		NewsLobbyItem.s_currentlyRunningRequests[picLink] = promise.Task;
		yield return loadPic;
		if (!string.IsNullOrEmpty(loadPic.error))
		{
			promise.TrySetException(new InvalidOperationException(loadPic.error));
			NewsLobbyItem.s_currentlyRunningRequests.Remove(picLink);
			Debug.LogWarning("Download preview pic error: " + loadPic.error);
			if (loadPic.error.StartsWith("Resolving host timed out"))
			{
				yield return new WaitForSeconds(1f);
				if (Application.isEditor && FriendsController.isDebugLogWWW)
				{
					Debug.Log("Reloading timed out pic");
				}
				base.StartCoroutine(this.LoadPreviewPicture(picLink));
			}
			yield break;
		}
		this.previewPicUrl = picLink;
		this.previewPic.mainTexture = loadPic.texture;
		this.previewPic.mainTexture.filterMode = FilterMode.Point;
		this.previewPic.width = 100;
		if (!string.IsNullOrEmpty(cachePath))
		{
			try
			{
				if (Defs.IsDeveloperBuild)
				{
					string formattedPath2 = (!Application.isEditor) ? cachePath : ("<color=magenta>" + cachePath + "</color>");
					Debug.LogFormat("Trying to save preview to cache '{0}'", new object[]
					{
						formattedPath2
					});
				}
				string directoryPath = Path.GetDirectoryName(cachePath);
				if (!Directory.Exists(directoryPath))
				{
					Directory.CreateDirectory(directoryPath);
				}
				byte[] cacheBytes2 = loadPic.texture.EncodeToPNG();
				File.WriteAllBytes(cachePath, cacheBytes2);
				promise.TrySetResult(true);
			}
			catch (IOException ex5)
			{
				IOException ex2 = ex5;
				Debug.LogWarning("Caught IOException while saving preview to cache. See next message for details.");
				Debug.LogException(ex2);
				promise.TrySetException(ex2);
				NewsLobbyItem.s_currentlyRunningRequests.Remove(picLink);
			}
			catch (Exception ex6)
			{
				Exception ex3 = ex6;
				Debug.LogWarning("Caught exception while saving preview to cache. See next message for details.");
				Debug.LogException(ex3);
				promise.TrySetException(ex3);
				NewsLobbyItem.s_currentlyRunningRequests.Remove(picLink);
			}
		}
		else
		{
			promise.TrySetException(new InvalidOperationException("Cache path is null or empty."));
			NewsLobbyItem.s_currentlyRunningRequests.Remove(picLink);
		}
		using (new ScopeLogger("Dispose " + picLink, Defs.IsDeveloperBuild))
		{
			loadPic.Dispose();
		}
		yield break;
	}

	// Token: 0x0400178F RID: 6031
	public GameObject indicatorNew;

	// Token: 0x04001790 RID: 6032
	public UILabel headerLabel;

	// Token: 0x04001791 RID: 6033
	public UILabel shortDescLabel;

	// Token: 0x04001792 RID: 6034
	public UILabel dateLabel;

	// Token: 0x04001793 RID: 6035
	public UITexture previewPic;

	// Token: 0x04001794 RID: 6036
	public string previewPicUrl;

	// Token: 0x04001795 RID: 6037
	private static readonly Dictionary<string, Task<bool>> s_currentlyRunningRequests = new Dictionary<string, Task<bool>>();
}
