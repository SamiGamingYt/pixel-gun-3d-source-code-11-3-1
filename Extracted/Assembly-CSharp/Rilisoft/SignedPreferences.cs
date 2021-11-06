using System;
using System.Collections.Generic;

namespace Rilisoft
{
	// Token: 0x020007BB RID: 1979
	internal abstract class SignedPreferences : Preferences
	{
		// Token: 0x060047AC RID: 18348 RVA: 0x0018C22C File Offset: 0x0018A42C
		protected SignedPreferences(Preferences backPreferences)
		{
			this._backPreferences = backPreferences;
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x0018C23C File Offset: 0x0018A43C
		public bool Verify(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.VerifyCore(key);
		}

		// Token: 0x060047AE RID: 18350
		protected abstract void AddSignedCore(string key, string value);

		// Token: 0x060047AF RID: 18351
		protected abstract bool RemoveSignedCore(string key);

		// Token: 0x060047B0 RID: 18352
		protected abstract bool VerifyCore(string key);

		// Token: 0x060047B1 RID: 18353 RVA: 0x0018C258 File Offset: 0x0018A458
		protected override void AddCore(string key, string value)
		{
			this.AddSignedCore(key, value);
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x0018C264 File Offset: 0x0018A464
		protected override bool ContainsKeyCore(string key)
		{
			return this._backPreferences.ContainsKey(key);
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x0018C274 File Offset: 0x0018A474
		protected override void CopyToCore(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			this._backPreferences.CopyTo(array, arrayIndex);
		}

		// Token: 0x060047B4 RID: 18356 RVA: 0x0018C284 File Offset: 0x0018A484
		protected override bool RemoveCore(string key)
		{
			return this.RemoveSignedCore(key);
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x0018C290 File Offset: 0x0018A490
		protected override bool TryGetValueCore(string key, out string value)
		{
			return this._backPreferences.TryGetValue(key, out value);
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x0018C2A0 File Offset: 0x0018A4A0
		public override void Save()
		{
			this._backPreferences.Save();
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x060047B7 RID: 18359 RVA: 0x0018C2B0 File Offset: 0x0018A4B0
		public override ICollection<string> Keys
		{
			get
			{
				return this._backPreferences.Keys;
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x060047B8 RID: 18360 RVA: 0x0018C2C0 File Offset: 0x0018A4C0
		public override ICollection<string> Values
		{
			get
			{
				return this._backPreferences.Values;
			}
		}

		// Token: 0x060047B9 RID: 18361 RVA: 0x0018C2D0 File Offset: 0x0018A4D0
		public override void Clear()
		{
			this._backPreferences.Clear();
		}

		// Token: 0x17000BE6 RID: 3046
		// (get) Token: 0x060047BA RID: 18362 RVA: 0x0018C2E0 File Offset: 0x0018A4E0
		public override int Count
		{
			get
			{
				return this._backPreferences.Count;
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x060047BB RID: 18363 RVA: 0x0018C2F0 File Offset: 0x0018A4F0
		public override bool IsReadOnly
		{
			get
			{
				return this._backPreferences.IsReadOnly;
			}
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x0018C300 File Offset: 0x0018A500
		public override IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return this._backPreferences.GetEnumerator();
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x060047BD RID: 18365 RVA: 0x0018C310 File Offset: 0x0018A510
		protected Preferences BackPreferences
		{
			get
			{
				return this._backPreferences;
			}
		}

		// Token: 0x040034D9 RID: 13529
		private readonly Preferences _backPreferences;
	}
}
