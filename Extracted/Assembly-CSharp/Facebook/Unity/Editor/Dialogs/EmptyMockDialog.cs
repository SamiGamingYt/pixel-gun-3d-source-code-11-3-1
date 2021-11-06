using System;
using System.Collections.Generic;
using Facebook.MiniJSON;

namespace Facebook.Unity.Editor.Dialogs
{
	// Token: 0x020000F0 RID: 240
	internal class EmptyMockDialog : EditorFacebookMockDialog
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x0002EF1C File Offset: 0x0002D11C
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x0002EF24 File Offset: 0x0002D124
		public string EmptyDialogTitle { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x0002EF30 File Offset: 0x0002D130
		protected override string DialogTitle
		{
			get
			{
				return this.EmptyDialogTitle;
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002EF38 File Offset: 0x0002D138
		protected override void DoGui()
		{
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0002EF3C File Offset: 0x0002D13C
		protected override void SendSuccessResult()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["did_complete"] = true;
			if (!string.IsNullOrEmpty(base.CallbackID))
			{
				dictionary["callback_id"] = base.CallbackID;
			}
			if (base.Callback != null)
			{
				base.Callback(Json.Serialize(dictionary));
			}
		}
	}
}
