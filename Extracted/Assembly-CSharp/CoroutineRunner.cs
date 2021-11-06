using System;
using System.Collections;
using System.Threading.Tasks;
using Rilisoft;
using UnityEngine;

// Token: 0x020005CD RID: 1485
public class CoroutineRunner : MonoBehaviour
{
	// Token: 0x1700087E RID: 2174
	// (get) Token: 0x0600332D RID: 13101 RVA: 0x00108D54 File Offset: 0x00106F54
	public static CoroutineRunner Instance
	{
		get
		{
			if (CoroutineRunner._instance == null)
			{
				try
				{
					GameObject gameObject = new GameObject("CoroutineRunner");
					CoroutineRunner._instance = gameObject.AddComponent<CoroutineRunner>();
					UnityEngine.Object.DontDestroyOnLoad(gameObject);
				}
				catch (Exception arg)
				{
					Debug.LogError("[Rilisoft] CoroutineRunner: Instance exception: " + arg);
				}
			}
			return CoroutineRunner._instance;
		}
	}

	// Token: 0x0600332E RID: 13102 RVA: 0x00108DCC File Offset: 0x00106FCC
	internal IEnumerator WrapCoroutine(IEnumerator routine, TaskCompletionSource<bool> promise)
	{
		if (routine == null)
		{
			throw new ArgumentNullException("routine");
		}
		if (promise == null)
		{
			throw new ArgumentNullException("promise");
		}
		return this.WrapCoroutineCore(routine, promise);
	}

	// Token: 0x0600332F RID: 13103 RVA: 0x00108E04 File Offset: 0x00107004
	private IEnumerator WrapCoroutineCore(IEnumerator routine, TaskCompletionSource<bool> promise)
	{
		while (routine.MoveNext())
		{
			object obj = routine.Current;
			yield return obj;
		}
		promise.SetResult(true);
		yield break;
	}

	// Token: 0x06003330 RID: 13104 RVA: 0x00108E34 File Offset: 0x00107034
	public static IEnumerator WaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
		yield break;
	}

	// Token: 0x06003331 RID: 13105 RVA: 0x00108E58 File Offset: 0x00107058
	public static IEnumerator WaitForSecondsActionEveryNFrames(float tm, Action action, int everyNFrames)
	{
		float startTime = Time.realtimeSinceStartup;
		int i = 0;
		do
		{
			yield return null;
			i++;
			if (i % everyNFrames == 0 && action != null)
			{
				action();
			}
		}
		while (Time.realtimeSinceStartup - startTime < tm);
		yield break;
	}

	// Token: 0x06003332 RID: 13106 RVA: 0x00108E98 File Offset: 0x00107098
	public static void WaitUntil(Func<bool> func, Action onActiveAction)
	{
		CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.Instance.WaitUntilCoroutine(func, onActiveAction));
	}

	// Token: 0x06003333 RID: 13107 RVA: 0x00108EB4 File Offset: 0x001070B4
	private IEnumerator WaitUntilCoroutine(Func<bool> func, Action onActiveAction)
	{
		yield return new WaitUntil(func);
		if (onActiveAction != null)
		{
			onActiveAction();
		}
		yield break;
	}

	// Token: 0x06003334 RID: 13108 RVA: 0x00108EE4 File Offset: 0x001070E4
	public static void DeferredAction(float runAfterSecs, Action act)
	{
		CoroutineRunner.Instance.StartCoroutine(CoroutineRunner.Instance.DeferredActionCoroutine(runAfterSecs, act));
	}

	// Token: 0x06003335 RID: 13109 RVA: 0x00108F00 File Offset: 0x00107100
	private IEnumerator DeferredActionCoroutine(float runAfterSecs, Action act)
	{
		yield return new WaitForRealSeconds(runAfterSecs);
		if (act != null)
		{
			act();
		}
		yield break;
	}

	// Token: 0x0400259D RID: 9629
	private static CoroutineRunner _instance;
}
