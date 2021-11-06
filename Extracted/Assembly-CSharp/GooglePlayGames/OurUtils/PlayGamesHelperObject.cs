using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.OurUtils
{
	// Token: 0x020001A7 RID: 423
	public class PlayGamesHelperObject : MonoBehaviour
	{
		// Token: 0x06000DBB RID: 3515 RVA: 0x00044BB4 File Offset: 0x00042DB4
		public static void CreateObject()
		{
			if (PlayGamesHelperObject.instance != null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				GameObject gameObject = new GameObject("PlayGames_QueueRunner");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				PlayGamesHelperObject.instance = gameObject.AddComponent<PlayGamesHelperObject>();
			}
			else
			{
				PlayGamesHelperObject.instance = new PlayGamesHelperObject();
				PlayGamesHelperObject.sIsDummy = true;
			}
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00044C10 File Offset: 0x00042E10
		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00044C20 File Offset: 0x00042E20
		public void OnDisable()
		{
			if (PlayGamesHelperObject.instance == this)
			{
				PlayGamesHelperObject.instance = null;
			}
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00044C38 File Offset: 0x00042E38
		public static void RunCoroutine(IEnumerator action)
		{
			if (PlayGamesHelperObject.instance != null)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					PlayGamesHelperObject.instance.StartCoroutine(action);
				});
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00044C74 File Offset: 0x00042E74
		public static void RunOnGameThread(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (PlayGamesHelperObject.sIsDummy)
			{
				return;
			}
			List<Action> obj = PlayGamesHelperObject.sQueue;
			lock (obj)
			{
				PlayGamesHelperObject.sQueue.Add(action);
				PlayGamesHelperObject.sQueueEmpty = false;
			}
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00044CE4 File Offset: 0x00042EE4
		public void Update()
		{
			if (PlayGamesHelperObject.sIsDummy || PlayGamesHelperObject.sQueueEmpty)
			{
				return;
			}
			this.localQueue.Clear();
			List<Action> obj = PlayGamesHelperObject.sQueue;
			lock (obj)
			{
				this.localQueue.AddRange(PlayGamesHelperObject.sQueue);
				PlayGamesHelperObject.sQueue.Clear();
				PlayGamesHelperObject.sQueueEmpty = true;
			}
			for (int i = 0; i < this.localQueue.Count; i++)
			{
				this.localQueue[i]();
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00044D98 File Offset: 0x00042F98
		public void OnApplicationFocus(bool focused)
		{
			foreach (Action<bool> action in PlayGamesHelperObject.sFocusCallbackList)
			{
				try
				{
					action(focused);
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnApplicationFocus:" + ex.Message + "\n" + ex.StackTrace);
				}
			}
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x00044E40 File Offset: 0x00043040
		public void OnApplicationPause(bool paused)
		{
			foreach (Action<bool> action in PlayGamesHelperObject.sPauseCallbackList)
			{
				try
				{
					action(paused);
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception in OnApplicationPause:" + ex.Message + "\n" + ex.StackTrace);
				}
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00044EE8 File Offset: 0x000430E8
		public static void AddFocusCallback(Action<bool> callback)
		{
			if (!PlayGamesHelperObject.sFocusCallbackList.Contains(callback))
			{
				PlayGamesHelperObject.sFocusCallbackList.Add(callback);
			}
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00044F08 File Offset: 0x00043108
		public static bool RemoveFocusCallback(Action<bool> callback)
		{
			return PlayGamesHelperObject.sFocusCallbackList.Remove(callback);
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00044F18 File Offset: 0x00043118
		public static void AddPauseCallback(Action<bool> callback)
		{
			if (!PlayGamesHelperObject.sPauseCallbackList.Contains(callback))
			{
				PlayGamesHelperObject.sPauseCallbackList.Add(callback);
			}
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00044F38 File Offset: 0x00043138
		public static bool RemovePauseCallback(Action<bool> callback)
		{
			return PlayGamesHelperObject.sPauseCallbackList.Remove(callback);
		}

		// Token: 0x04000A84 RID: 2692
		private static PlayGamesHelperObject instance = null;

		// Token: 0x04000A85 RID: 2693
		private static bool sIsDummy = false;

		// Token: 0x04000A86 RID: 2694
		private static List<Action> sQueue = new List<Action>();

		// Token: 0x04000A87 RID: 2695
		private List<Action> localQueue = new List<Action>();

		// Token: 0x04000A88 RID: 2696
		private static volatile bool sQueueEmpty = true;

		// Token: 0x04000A89 RID: 2697
		private static List<Action<bool>> sPauseCallbackList = new List<Action<bool>>();

		// Token: 0x04000A8A RID: 2698
		private static List<Action<bool>> sFocusCallbackList = new List<Action<bool>>();
	}
}
