using System;
using System.Collections.Generic;
using Facebook.MiniJSON;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x020000F7 RID: 247
	internal class GraphResult : ResultBase, IGraphResult, IResult
	{
		// Token: 0x0600078E RID: 1934 RVA: 0x0002F500 File Offset: 0x0002D700
		internal GraphResult(WWW result) : base(result.text, result.error, false)
		{
			this.Init(this.RawResult);
			if (result.error == null)
			{
				this.Texture = result.texture;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0002F544 File Offset: 0x0002D744
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x0002F54C File Offset: 0x0002D74C
		public IList<object> ResultList { get; private set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0002F558 File Offset: 0x0002D758
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x0002F560 File Offset: 0x0002D760
		public Texture2D Texture { get; private set; }

		// Token: 0x06000793 RID: 1939 RVA: 0x0002F56C File Offset: 0x0002D76C
		private void Init(string rawResult)
		{
			if (string.IsNullOrEmpty(rawResult))
			{
				return;
			}
			object obj = Json.Deserialize(this.RawResult);
			IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
			if (dictionary != null)
			{
				this.ResultDictionary = dictionary;
				return;
			}
			IList<object> list = obj as IList<object>;
			if (list != null)
			{
				this.ResultList = list;
				return;
			}
		}
	}
}
