using System;
using UnityEngine;

namespace HeurekaGames
{
	// Token: 0x0200029B RID: 667
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x00053FDC File Offset: 0x000521DC
		public static T Instance
		{
			get
			{
				if (Singleton<T>.applicationIsQuitting)
				{
					Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
					return (T)((object)null);
				}
				object @lock = Singleton<T>._lock;
				T instance;
				lock (@lock)
				{
					if (Singleton<T>._instance == null)
					{
						Singleton<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
						if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
						{
							Debug.LogError("[Singleton] Something went really wrong  - there should never be more than 1 singleton! Reopenning the scene might fix it.");
							return Singleton<T>._instance;
						}
						if (Singleton<T>._instance == null)
						{
							GameObject gameObject = new GameObject();
							Singleton<T>._instance = gameObject.AddComponent<T>();
							gameObject.name = "(singleton) " + typeof(T).ToString();
							Debug.Log("[Singleton] An instance of " + typeof(T) + " was created.");
						}
						else
						{
							Debug.Log("[Singleton] Using instance already created: " + Singleton<T>._instance.gameObject.name);
						}
					}
					instance = Singleton<T>._instance;
				}
				return instance;
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00054140 File Offset: 0x00052340
		public void OnDestroy()
		{
			Singleton<T>.applicationIsQuitting = true;
		}

		// Token: 0x04000C5E RID: 3166
		private static T _instance;

		// Token: 0x04000C5F RID: 3167
		private static object _lock = new object();

		// Token: 0x04000C60 RID: 3168
		private static bool applicationIsQuitting = false;
	}
}
