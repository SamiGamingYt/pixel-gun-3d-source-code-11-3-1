using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000780 RID: 1920
	public static class RiliExtensions
	{
		// Token: 0x06004368 RID: 17256 RVA: 0x001680A0 File Offset: 0x001662A0
		public static bool TryGetValue<T>(this Dictionary<string, object> dict, string key, out T value)
		{
			if (dict == null)
			{
				value = default(T);
				return false;
			}
			object obj;
			if (!dict.TryGetValue(key, out obj))
			{
				value = default(T);
				return false;
			}
			if (obj is T)
			{
				value = (T)((object)obj);
				return true;
			}
			bool result;
			try
			{
				value = (T)((object)Convert.ChangeType(obj, typeof(T)));
				result = true;
			}
			catch
			{
				value = default(T);
				result = false;
			}
			return result;
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x00168160 File Offset: 0x00166360
		public static IEnumerable<T> WithoutLast<T>(this IEnumerable<T> source)
		{
			using (IEnumerator<T> e = source.GetEnumerator())
			{
				if (e.MoveNext())
				{
					T value = e.Current;
					while (e.MoveNext())
					{
						yield return value;
						value = e.Current;
					}
				}
			}
			yield break;
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x0016818C File Offset: 0x0016638C
		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
		{
			return source == null || !source.Any<T>();
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x001681A0 File Offset: 0x001663A0
		public static bool IsEmpty<T>(this IEnumerable<T> source)
		{
			return !source.Any<T>();
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x001681AC File Offset: 0x001663AC
		public static void Swap<T>(ref T lhs, ref T rhs)
		{
			T t = lhs;
			lhs = rhs;
			rhs = t;
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x001681D4 File Offset: 0x001663D4
		public static void SetInstantlyNoHandlers(this UIToggle toggle, bool state)
		{
			if (toggle == null)
			{
				return;
			}
			List<EventDelegate> onChange = toggle.onChange;
			toggle.onChange = new List<EventDelegate>();
			bool instantTween = toggle.instantTween;
			toggle.instantTween = true;
			toggle.Set(state);
			toggle.onChange = onChange;
			toggle.instantTween = instantTween;
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x00168224 File Offset: 0x00166424
		public static string nameNoClone(this UnityEngine.Object obj)
		{
			if (obj == null)
			{
				return null;
			}
			return obj.name.Replace("(Clone)", string.Empty);
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0016824C File Offset: 0x0016644C
		public static bool IsNullOrEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x00168254 File Offset: 0x00166454
		public static T? ToEnum<T>(this string str, T? defaultVal = null) where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			if (str.IsNullOrEmpty())
			{
				Debug.LogError("String is null or empty");
				return defaultVal;
			}
			str = str.ToLower();
			foreach (object obj in Enum.GetValues(typeof(T)))
			{
				T value = (T)((object)obj);
				if (value.ToString().ToLower() == str)
				{
					return new T?(value);
				}
			}
			Debug.LogErrorFormat("'{0}' does not contain '{1}'", new object[]
			{
				typeof(T).Name,
				str
			});
			return defaultVal;
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x00168358 File Offset: 0x00166558
		public static string[] EnumValues<T>() where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			return Enum.GetValues(typeof(T)).Cast<string>().ToArray<string>();
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x001683A0 File Offset: 0x001665A0
		public static int[] EnumNumbers<T>() where T : struct
		{
			if (!typeof(T).IsEnum)
			{
				throw new ArgumentException("T must be an enumerated type");
			}
			return Enum.GetValues(typeof(T)).Cast<int>().ToArray<int>();
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x001683E8 File Offset: 0x001665E8
		public static int EnumLen<T>()
		{
			return Enum.GetNames(typeof(T)).Length;
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x001683FC File Offset: 0x001665FC
		public static void ForEachEnum<T>(Action<T> action)
		{
			if (action != null)
			{
				Array values = Enum.GetValues(typeof(T));
				foreach (object obj in values)
				{
					action((T)((object)obj));
				}
			}
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x0016847C File Offset: 0x0016667C
		public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
		{
			foreach (T obj in enumeration)
			{
				action(obj);
			}
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x001684DC File Offset: 0x001666DC
		public static int ToInt(this bool boolValue)
		{
			return (!boolValue) ? 0 : 1;
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x001684EC File Offset: 0x001666EC
		public static bool ToBool(this int intValue)
		{
			return intValue > 0;
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x001684FC File Offset: 0x001666FC
		public static List<Transform> Neighbors(this Transform tr, bool withSelf = false)
		{
			if (tr == null || tr.parent == null)
			{
				return new List<Transform>();
			}
			return (from Transform neighbor in tr.parent
			where withSelf || neighbor != tr
			select neighbor).ToList<Transform>();
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x00168570 File Offset: 0x00166770
		public static GameObject GetChildGameObject(this GameObject go, string name, bool includeInactive = false)
		{
			Transform transform = go.transform.GetComponentsInChildren<Transform>(includeInactive).FirstOrDefault((Transform t) => t.gameObject.name == name);
			return (!(transform != null)) ? null : transform.gameObject;
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x001685C0 File Offset: 0x001667C0
		public static T GetComponentInChildren<T>(this GameObject go, string name, bool includeInactive = false)
		{
			Transform[] componentsInChildren = go.transform.GetComponentsInChildren<Transform>(includeInactive);
			foreach (Transform transform in componentsInChildren)
			{
				if (transform.gameObject.name == name)
				{
					return transform.gameObject.GetComponent<T>();
				}
			}
			return default(T);
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x00168620 File Offset: 0x00166820
		public static GameObject GetGameObjectInParent(this GameObject go, string name, bool includeInactive = false)
		{
			Transform[] componentsInParent = go.transform.GetComponentsInParent<Transform>(includeInactive);
			foreach (Transform transform in componentsInParent)
			{
				if (transform.gameObject.name == name)
				{
					return transform.gameObject;
				}
			}
			return null;
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x00168674 File Offset: 0x00166874
		public static T GetComponentInParents<T>(this GameObject go)
		{
			T component = go.GetComponent<T>();
			if (component != null && !component.Equals(default(T)))
			{
				return component;
			}
			Transform parent = go.transform.parent;
			return parent.gameObject.GetComponentInParents<T>();
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x001686CC File Offset: 0x001668CC
		public static bool IsSubobjectOf(this GameObject go, GameObject assumedRoot)
		{
			if (go.Equals(null) || assumedRoot.Equals(null))
			{
				return false;
			}
			foreach (GameObject gameObject in assumedRoot.Descendants())
			{
				if (gameObject.Equals(go))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x00168758 File Offset: 0x00166958
		public static void SetActiveSafe(this GameObject go, bool state)
		{
			if (go == null)
			{
				return;
			}
			if (go.activeInHierarchy != state)
			{
				go.SetActive(state);
			}
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x00168788 File Offset: 0x00166988
		public static void SetActiveSafeSelf(this GameObject go, bool state)
		{
			if (go != null && go.activeSelf != state)
			{
				go.SetActive(state);
			}
		}

		// Token: 0x06004380 RID: 17280 RVA: 0x001687AC File Offset: 0x001669AC
		public static T GetOrAddComponent<T>(this Component child) where T : Component
		{
			T t = child.GetComponent<T>();
			if (t == null)
			{
				t = child.gameObject.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x001687E0 File Offset: 0x001669E0
		public static T GetOrAddComponent<T>(this GameObject child) where T : Component
		{
			T t = child.GetComponent<T>();
			if (t == null)
			{
				t = child.gameObject.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x00168814 File Offset: 0x00166A14
		public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int count)
		{
			if (source == null)
			{
				return source;
			}
			List<T> list = source.ToList<T>();
			if (!list.Any<T>())
			{
				return list;
			}
			List<T> list2 = new List<T>();
			if (count < 1)
			{
				return list2;
			}
			bool flag = true;
			while (flag)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				list2.Add(list[index]);
				list.RemoveAt(index);
				if (list.Count == 0 || list2.Count == count)
				{
					flag = false;
				}
			}
			return list2;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x00168894 File Offset: 0x00166A94
		public static Color ColorFromRGB(int r, int g, int b, int a = 255)
		{
			return new Color((float)(r / 255), (float)(g / 255), (float)(b / 255), (float)(a / 255));
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x001688BC File Offset: 0x00166ABC
		public static Color ColorFromHex(string hex)
		{
			hex = hex.Replace("0x", string.Empty);
			hex = hex.Replace("#", string.Empty);
			byte a = byte.MaxValue;
			byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			if (hex.Length == 8)
			{
				a = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
			}
			return new Color32(r, g, b, a);
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x0016895C File Offset: 0x00166B5C
		public static string ToHex(this Color32 color)
		{
			return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x001689A4 File Offset: 0x00166BA4
		public static T ParseJSONField<T>(this Dictionary<string, object> dict, string name, Func<object, T> converter, bool ignoreNotFoundError = false)
		{
			if (dict == null)
			{
				Debug.LogError("dict is null");
				return default(T);
			}
			if (dict.ContainsKey(name))
			{
				try
				{
					return converter(dict[name]);
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Format("[PARSE] error parse '{0}' : {1}, {2}", name, ex.Message, ex.StackTrace));
					return default(T);
				}
			}
			if (ignoreNotFoundError)
			{
				return default(T);
			}
			Debug.LogError(string.Format("[PARSE] unknown key '{0}'", name));
			return default(T);
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x00168A6C File Offset: 0x00166C6C
		public static void ShuffleThis<T>(this IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = RiliExtensions._random.Next(i + 1);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x00168ABC File Offset: 0x00166CBC
		public static IList<T> Shuffle<T>(this IList<T> list)
		{
			List<T> list2 = new List<T>(list);
			int i = list2.Count;
			while (i > 1)
			{
				i--;
				int index = RiliExtensions._random.Next(i + 1);
				T value = list2[index];
				list2[index] = list2[i];
				list2[i] = value;
			}
			return list2;
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x00168B14 File Offset: 0x00166D14
		public static IEnumerator MoveOverTime(this GameObject go, Vector3 from, Vector3 to, float seconds, Action onFinished = null)
		{
			go.transform.position = from;
			float elapsedTime = 0f;
			while (elapsedTime < seconds)
			{
				go.transform.position = Vector3.Lerp(from, to, elapsedTime / seconds);
				elapsedTime += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			go.transform.position = to;
			if (onFinished != null)
			{
				onFinished();
			}
			yield break;
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x00168B70 File Offset: 0x00166D70
		public static void DrawGUIRect(Rect position, Color color)
		{
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, color);
			texture2D.Apply();
			GUI.skin.box.normal.background = texture2D;
			GUI.Box(position, GUIContent.none);
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x00168BB4 File Offset: 0x00166DB4
		public static string CombinePaths(params string[] paths)
		{
			if (paths == null)
			{
				throw new ArgumentNullException("paths");
			}
			Func<string, string, string> func = (string path1, string path2) => Path.Combine(path1, path2);
			return paths.Aggregate(func);
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x00168BF8 File Offset: 0x00166DF8
		public static Vector2 PointOnCircle(float angleDegrees, float radius)
		{
			float f = angleDegrees * 3.1415927f / 180f;
			float x = radius * Mathf.Cos(f);
			float y = radius * Mathf.Sin(f);
			Vector2 result = new Vector2(x, y);
			return result;
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x00168C38 File Offset: 0x00166E38
		public static Vector2 RandomOnCircle(float radius)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			insideUnitCircle.Normalize();
			return insideUnitCircle * radius;
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x00168C5C File Offset: 0x00166E5C
		public static Vector3 RandomOnSphere(float radius)
		{
			Vector3 insideUnitSphere = UnityEngine.Random.insideUnitSphere;
			insideUnitSphere.Normalize();
			return insideUnitSphere * radius;
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x00168C80 File Offset: 0x00166E80
		public static Vector3 Add(this Vector3 ts, Vector3 v)
		{
			return ts + v;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x00168C8C File Offset: 0x00166E8C
		public static Vector3 Add(this Vector3 ts, Vector2 v)
		{
			return new Vector3(ts.x + v.x, ts.y + v.y, ts.z);
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x00168CC4 File Offset: 0x00166EC4
		public static string GetTimeStringDays(long seconds)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)seconds);
			string result = string.Empty;
			if (seconds >= 86400L)
			{
				long num = seconds / 86400L;
				long num2 = seconds % 86400L;
				if (num2 > 43200L)
				{
					num += 1L;
				}
				result = num.ToString();
			}
			else
			{
				result = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			}
			return result;
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x00168D4C File Offset: 0x00166F4C
		public static string GetTimeStringLocalizedShort(long seconds)
		{
			if (seconds >= 86400L)
			{
				long num = seconds / 86400L;
				long num2 = seconds % 86400L;
				if (num2 > 43200L)
				{
					num += 1L;
				}
				return string.Format(LocalizationStore.Get("Key_2913"), num);
			}
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)seconds);
			if (timeSpan.Days > 0)
			{
				return string.Format(LocalizationStore.Get("Key_2913"), timeSpan.Days);
			}
			if (timeSpan.Hours > 0)
			{
				return string.Format(LocalizationStore.Get("Key_2917"), timeSpan.Hours);
			}
			if (timeSpan.Minutes > 0)
			{
				return string.Format(LocalizationStore.Get("Key_2918"), timeSpan.Minutes);
			}
			return string.Format(LocalizationStore.Get("Key_2918"), 1);
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x00168E38 File Offset: 0x00167038
		public static string GetTimeString(long secs, string delimer = ":")
		{
			if (secs < 1L)
			{
				return string.Empty;
			}
			int num = (int)(secs / 3600L);
			int num2 = (int)(secs / 60L) - num * 60;
			int num3 = (int)secs - num * 3600 - num2 * 60;
			string text = (num >= 10) ? num.ToString() : ("0" + num);
			string text2 = (num2 >= 10) ? num2.ToString() : ("0" + num2);
			string text3 = (num3 >= 10) ? num3.ToString() : ("0" + num3);
			return string.Concat(new string[]
			{
				text,
				delimer,
				text2,
				delimer,
				text3
			});
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06004394 RID: 17300 RVA: 0x00168F0C File Offset: 0x0016710C
		public static long SystemTime
		{
			get
			{
				return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			}
		}

		// Token: 0x0400314A RID: 12618
		private static System.Random _random = new System.Random();
	}
}
