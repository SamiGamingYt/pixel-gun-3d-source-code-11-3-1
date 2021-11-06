using System;
using System.Collections.Generic;
using Facebook.MiniJSON;
using UnityEngine;

namespace Facebook.Unity.Editor
{
	// Token: 0x020000EF RID: 239
	internal abstract class EditorFacebookMockDialog : MonoBehaviour
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0002EC6C File Offset: 0x0002CE6C
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x0002EC74 File Offset: 0x0002CE74
		public EditorFacebookMockDialog.OnComplete Callback { protected get; set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x0002EC80 File Offset: 0x0002CE80
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x0002EC88 File Offset: 0x0002CE88
		public string CallbackID { protected get; set; }

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000761 RID: 1889
		protected abstract string DialogTitle { get; }

		// Token: 0x06000762 RID: 1890 RVA: 0x0002EC94 File Offset: 0x0002CE94
		public void Start()
		{
			this.modalRect = new Rect(10f, 10f, (float)(Screen.width - 20), (float)(Screen.height - 20));
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1f));
			texture2D.Apply();
			this.modalStyle = new GUIStyle();
			this.modalStyle.normal.background = texture2D;
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0002ED14 File Offset: 0x0002CF14
		public void OnGUI()
		{
			GUI.ModalWindow(this.GetHashCode(), this.modalRect, new GUI.WindowFunction(this.OnGUIDialog), this.DialogTitle, this.modalStyle);
		}

		// Token: 0x06000764 RID: 1892
		protected abstract void DoGui();

		// Token: 0x06000765 RID: 1893
		protected abstract void SendSuccessResult();

		// Token: 0x06000766 RID: 1894 RVA: 0x0002ED4C File Offset: 0x0002CF4C
		protected virtual void SendCancelResult()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["cancelled"] = true;
			if (!string.IsNullOrEmpty(this.CallbackID))
			{
				dictionary["callback_id"] = this.CallbackID;
			}
			this.Callback(Json.Serialize(dictionary));
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0002EDA4 File Offset: 0x0002CFA4
		protected virtual void SendErrorResult(string errorMessage)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["error"] = errorMessage;
			if (!string.IsNullOrEmpty(this.CallbackID))
			{
				dictionary["callback_id"] = this.CallbackID;
			}
			this.Callback(Json.Serialize(dictionary));
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0002EDF8 File Offset: 0x0002CFF8
		private void OnGUIDialog(int windowId)
		{
			GUILayout.Space(10f);
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label("Warning! Mock dialog responses will NOT match production dialogs", new GUILayoutOption[0]);
			GUILayout.Label("Test your app on one of the supported platforms", new GUILayoutOption[0]);
			this.DoGui();
			GUILayout.EndVertical();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUIContent content = new GUIContent("Send Success");
			Rect rect = GUILayoutUtility.GetRect(content, GUI.skin.button);
			if (GUI.Button(rect, content))
			{
				this.SendSuccessResult();
				UnityEngine.Object.Destroy(this);
			}
			GUIContent content2 = new GUIContent("Send Cancel");
			Rect rect2 = GUILayoutUtility.GetRect(content2, GUI.skin.button);
			if (GUI.Button(rect2, content2, GUI.skin.button))
			{
				this.SendCancelResult();
				UnityEngine.Object.Destroy(this);
			}
			GUIContent content3 = new GUIContent("Send Error");
			Rect rect3 = GUILayoutUtility.GetRect(content2, GUI.skin.button);
			if (GUI.Button(rect3, content3, GUI.skin.button))
			{
				this.SendErrorResult("Error: Error button pressed");
				UnityEngine.Object.Destroy(this);
			}
			GUILayout.EndHorizontal();
		}

		// Token: 0x04000665 RID: 1637
		private Rect modalRect;

		// Token: 0x04000666 RID: 1638
		private GUIStyle modalStyle;

		// Token: 0x0200089A RID: 2202
		// (Invoke) Token: 0x06004F08 RID: 20232
		public delegate void OnComplete(string result);
	}
}
