using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000784 RID: 1924
	public class VisibleAtScene : MonoBehaviour
	{
		// Token: 0x060043AB RID: 17323 RVA: 0x0016A868 File Offset: 0x00168A68
		private void Awake()
		{
			if (this._includeMenuScene)
			{
				this._scenes.Add(Defs.MainMenuScene);
			}
			this._baseVisible = base.gameObject.activeSelf;
			this._scenes = (from s in this._scenes
			select s.ToLower()).ToList<string>();
			this.SetVisible(SceneLoader.ActiveSceneName);
			Singleton<SceneLoader>.Instance.OnSceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x0016A8F8 File Offset: 0x00168AF8
		private void OnDestroy()
		{
			Singleton<SceneLoader>.Instance.OnSceneLoaded -= this.OnSceneLoaded;
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x0016A910 File Offset: 0x00168B10
		private void OnSceneLoaded(SceneLoadInfo inf)
		{
			this.SetVisible(inf.SceneName);
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x0016A920 File Offset: 0x00168B20
		private void SetVisible(string currentSceneName)
		{
			currentSceneName = currentSceneName.ToLower();
			if (this._scenes.Contains(currentSceneName))
			{
				base.gameObject.SetActiveSafe(this._visible == VisibleState.On);
			}
			else
			{
				base.gameObject.SetActiveSafe(this._baseVisible);
			}
		}

		// Token: 0x04003163 RID: 12643
		[SerializeField]
		private VisibleState _visible;

		// Token: 0x04003164 RID: 12644
		[SerializeField]
		private bool _includeMenuScene;

		// Token: 0x04003165 RID: 12645
		[SerializeField]
		private List<string> _scenes = new List<string>();

		// Token: 0x04003166 RID: 12646
		private bool _baseVisible;
	}
}
