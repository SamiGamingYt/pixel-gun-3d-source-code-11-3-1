using System;
using System.Collections.Generic;
using System.Text;
using Facebook.MiniJSON;
using UnityEngine;

namespace Facebook.Unity.Editor.Dialogs
{
	// Token: 0x020000F2 RID: 242
	internal class MockShareDialog : EditorFacebookMockDialog
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0002F128 File Offset: 0x0002D328
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x0002F130 File Offset: 0x0002D330
		public string SubTitle { private get; set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x0002F13C File Offset: 0x0002D33C
		protected override string DialogTitle
		{
			get
			{
				return "Mock " + this.SubTitle + " Dialog";
			}
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0002F154 File Offset: 0x0002D354
		protected override void DoGui()
		{
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0002F158 File Offset: 0x0002D358
		protected override void SendSuccessResult()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (FB.IsLoggedIn)
			{
				dictionary["postId"] = this.GenerateFakePostID();
			}
			else
			{
				dictionary["did_complete"] = true;
			}
			if (!string.IsNullOrEmpty(base.CallbackID))
			{
				dictionary["callback_id"] = base.CallbackID;
			}
			if (base.Callback != null)
			{
				base.Callback(Json.Serialize(dictionary));
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0002F1DC File Offset: 0x0002D3DC
		protected override void SendCancelResult()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["cancelled"] = "true";
			if (!string.IsNullOrEmpty(base.CallbackID))
			{
				dictionary["callback_id"] = base.CallbackID;
			}
			base.Callback(Json.Serialize(dictionary));
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0002F234 File Offset: 0x0002D434
		private string GenerateFakePostID()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(AccessToken.CurrentAccessToken.UserId);
			stringBuilder.Append('_');
			for (int i = 0; i < 17; i++)
			{
				stringBuilder.Append(UnityEngine.Random.Range(0, 10));
			}
			return stringBuilder.ToString();
		}
	}
}
