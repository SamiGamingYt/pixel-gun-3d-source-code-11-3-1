using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rilisoft
{
	// Token: 0x0200074F RID: 1871
	public class SceneLoader : Singleton<SceneLoader>
	{
		// Token: 0x14000094 RID: 148
		// (add) Token: 0x060041B7 RID: 16823 RVA: 0x0015D9D4 File Offset: 0x0015BBD4
		// (remove) Token: 0x060041B8 RID: 16824 RVA: 0x0015D9F0 File Offset: 0x0015BBF0
		public event Action<SceneLoadInfo> OnSceneLoading;

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x060041B9 RID: 16825 RVA: 0x0015DA0C File Offset: 0x0015BC0C
		// (remove) Token: 0x060041BA RID: 16826 RVA: 0x0015DA28 File Offset: 0x0015BC28
		public event Action<SceneLoadInfo> OnSceneLoaded;

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x060041BB RID: 16827 RVA: 0x0015DA44 File Offset: 0x0015BC44
		public static string ActiveSceneName
		{
			get
			{
				return SceneManager.GetActiveScene().name ?? string.Empty;
			}
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x0015DA6C File Offset: 0x0015BC6C
		private void OnInstanceCreated()
		{
			if (this._scenesList == null)
			{
				throw new Exception("scenes list is null");
			}
			IGrouping<string, ExistsSceneInfo>[] source = (from i in this._scenesList.Infos
			group i by i.Name into g
			where g.Count<ExistsSceneInfo>() > 1
			select g).ToArray<IGrouping<string, ExistsSceneInfo>>();
			if (source.Any<IGrouping<string, ExistsSceneInfo>>())
			{
				string str = (from g in source
				select g.Key).Aggregate((string cur, string next) => string.Format("{0},{1}{2}", cur, next, Environment.NewLine));
				Debug.LogError("[SCENELOADER] duplicate scenes: " + str);
				return;
			}
			this.OnSceneLoaded = (Action<SceneLoadInfo>)Delegate.Combine(this.OnSceneLoaded, new Action<SceneLoadInfo>(this._loadingHistory.Add));
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x0015DB74 File Offset: 0x0015BD74
		public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			ExistsSceneInfo sceneInfo = this.GetSceneInfo(sceneName);
			SceneLoadInfo obj = new SceneLoadInfo
			{
				SceneName = sceneInfo.Name,
				LoadMode = mode
			};
			if (this.OnSceneLoading != null)
			{
				this.OnSceneLoading(obj);
			}
			SceneManager.LoadScene(sceneName, mode);
			if (this.OnSceneLoaded != null)
			{
				this.OnSceneLoaded(obj);
			}
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x0015DBE0 File Offset: 0x0015BDE0
		public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			ExistsSceneInfo sceneInfo = this.GetSceneInfo(sceneName);
			SceneLoadInfo sceneLoadInfo = new SceneLoadInfo
			{
				SceneName = sceneInfo.Name,
				LoadMode = mode
			};
			if (this.OnSceneLoading != null)
			{
				this.OnSceneLoading(sceneLoadInfo);
			}
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneInfo.Name, mode);
			Singleton<SceneLoader>.Instance.StartCoroutine(this.WaitSceneIsLoaded(sceneLoadInfo, asyncOperation));
			return asyncOperation;
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x0015DC50 File Offset: 0x0015BE50
		public ExistsSceneInfo GetSceneInfo(string sceneName)
		{
			ExistsSceneInfo existsSceneInfo = (!string.IsNullOrEmpty(Path.GetDirectoryName(sceneName))) ? this._scenesList.Infos.FirstOrDefault((ExistsSceneInfo i) => i.Path == sceneName) : this._scenesList.Infos.FirstOrDefault((ExistsSceneInfo i) => i.Name == sceneName);
			if (existsSceneInfo == null)
			{
				throw new ArgumentException(string.Format("Unknown scene : '{0}'", sceneName));
			}
			return existsSceneInfo;
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x0015DCDC File Offset: 0x0015BEDC
		private IEnumerator WaitSceneIsLoaded(SceneLoadInfo loadInfo, AsyncOperation op)
		{
			while (!op.isDone)
			{
				yield return null;
			}
			if (this.OnSceneLoaded != null)
			{
				this.OnSceneLoaded(loadInfo);
			}
			yield break;
		}

		// Token: 0x04003002 RID: 12290
		public const string SCENE_INFOS_ASSET_PATH = "Assets/Resources/ScenesList.asset";

		// Token: 0x04003003 RID: 12291
		[SerializeField]
		private ScenesList _scenesList;

		// Token: 0x04003004 RID: 12292
		[SerializeField]
		private List<SceneLoadInfo> _loadingHistory = new List<SceneLoadInfo>();
	}
}
