using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020005C6 RID: 1478
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x0600330A RID: 13066 RVA: 0x0010833C File Offset: 0x0010653C
		protected static bool IsSetted
		{
			get
			{
				return Singleton<T>._instance != null;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x0600330B RID: 13067 RVA: 0x00108350 File Offset: 0x00106550
		// (set) Token: 0x0600330C RID: 13068 RVA: 0x0010849C File Offset: 0x0010669C
		public static T Instance
		{
			get
			{
				if (Singleton<T>._instance != null)
				{
					return Singleton<T>._instance;
				}
				Singleton<T>._instance = UnityEngine.Object.FindObjectOfType<T>();
				if (Singleton<T>._instance != null)
				{
					Singleton<T>._instance.SendMessage("OnInstanceCreated", SendMessageOptions.DontRequireReceiver);
					return Singleton<T>._instance;
				}
				GameObject gameObject = null;
				ISingletonFromPrefab singletonFromPrefab = Singleton<T>._instance as ISingletonFromPrefab;
				if (singletonFromPrefab != null)
				{
					if (singletonFromPrefab.SingletonPrefab != null)
					{
						GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(singletonFromPrefab.SingletonPrefab);
						T component = gameObject2.GetComponent<T>();
						if (component != null)
						{
							Singleton<T>._instance = component;
							gameObject = gameObject2;
						}
						else
						{
							Debug.LogError("[Singleton] can not find singleton class in prefab");
						}
					}
					else
					{
						Debug.LogError("[Singleton] prefab not setted");
					}
				}
				else
				{
					gameObject = new GameObject(typeof(T).Name);
					Singleton<T>._instance = gameObject.AddComponent<T>();
				}
				GameObject gameObject3 = gameObject;
				gameObject3.name += " [Singleton]";
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				Singleton<T>._instance.SendMessage("OnInstanceCreated", SendMessageOptions.DontRequireReceiver);
				if (UnityEngine.Object.FindObjectsOfType<T>().Length > 1)
				{
					Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton!");
				}
				return Singleton<T>._instance;
			}
			protected set
			{
				Singleton<T>._instance = value;
				if (UnityEngine.Object.FindObjectsOfType<T>().Length > 1)
				{
					Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton!");
				}
				UnityEngine.Object.DontDestroyOnLoad(Singleton<T>._instance.gameObject);
				Singleton<T>._instance.SendMessage("OnInstanceCreated", SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x001084F4 File Offset: 0x001066F4
		protected virtual void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x0400258B RID: 9611
		private const string MSG_DUPLICATE = "[Singleton] Something went really wrong - there should never be more than 1 singleton!";

		// Token: 0x0400258C RID: 9612
		private const string MSG_PREFAB_NOT_SETTED = "[Singleton] prefab not setted";

		// Token: 0x0400258D RID: 9613
		private const string MSG_NOT_FOUND_IN_PREFAB = "[Singleton] can not find singleton class in prefab";

		// Token: 0x0400258E RID: 9614
		private static T _instance;
	}
}
