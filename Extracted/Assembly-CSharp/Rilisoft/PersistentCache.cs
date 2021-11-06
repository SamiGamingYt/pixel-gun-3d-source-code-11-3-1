using System;
using System.IO;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006D0 RID: 1744
	internal sealed class PersistentCache
	{
		// Token: 0x06003CAF RID: 15535 RVA: 0x0013B538 File Offset: 0x00139738
		public PersistentCache()
		{
			try
			{
				string text = (!string.IsNullOrEmpty(Application.persistentDataPath)) ? Application.persistentDataPath : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				if (!string.IsNullOrEmpty(text))
				{
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					this._persistentDataPath = text;
				}
			}
			catch (Exception exception)
			{
				Debug.LogWarning("Caught exception while persistent data path initialization. See next error message for details.");
				Debug.LogException(exception);
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06003CB1 RID: 15537 RVA: 0x0013B600 File Offset: 0x00139800
		public string PersistentDataPath
		{
			get
			{
				return this._persistentDataPath;
			}
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x0013B608 File Offset: 0x00139808
		public string GetCachePathByUri(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (string.IsNullOrEmpty(url))
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(this._persistentDataPath))
			{
				return string.Empty;
			}
			string result;
			try
			{
				Uri uri = new Uri(url);
				string[] segments = uri.Segments;
				string path = string.Concat(segments).TrimStart(new char[]
				{
					'/'
				});
				string text = Path.Combine(this._persistentDataPath, path);
				result = text;
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06003CB3 RID: 15539 RVA: 0x0013B6BC File Offset: 0x001398BC
		public static PersistentCache Instance
		{
			get
			{
				return PersistentCache._instance.Value;
			}
		}

		// Token: 0x04002CE1 RID: 11489
		private readonly string _persistentDataPath = string.Empty;

		// Token: 0x04002CE2 RID: 11490
		private static readonly Lazy<PersistentCache> _instance = new Lazy<PersistentCache>(() => new PersistentCache());
	}
}
