using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200067B RID: 1659
	[DisallowMultipleComponent]
	internal sealed class KeychainCleaner : MonoBehaviour
	{
		// Token: 0x060039B1 RID: 14769 RVA: 0x0012B4E0 File Offset: 0x001296E0
		private KeychainCleaner()
		{
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x0012B4E8 File Offset: 0x001296E8
		internal void AcquireLock()
		{
			this._lock = true;
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x0012B4F4 File Offset: 0x001296F4
		internal bool LockAcquired()
		{
			return this._lock;
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x0012B4FC File Offset: 0x001296FC
		internal void ReleaseLock()
		{
			this._lock = false;
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x0012B508 File Offset: 0x00129708
		private IEnumerator QuitFromEditorCoroutine()
		{
			if (!Application.isEditor)
			{
				yield break;
			}
			for (int i = 0; i != 2; i++)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x0012B51C File Offset: 0x0012971C
		private void Quit()
		{
			if (!Application.isEditor)
			{
				Application.Quit();
				return;
			}
			base.StartCoroutine(this.QuitFromEditorCoroutine());
		}

		// Token: 0x060039B7 RID: 14775 RVA: 0x0012B53C File Offset: 0x0012973C
		private void Clear()
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
			if (Application.isEditor)
			{
				return;
			}
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x0012B554 File Offset: 0x00129754
		private void OnResetButtonClicked()
		{
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x0012B558 File Offset: 0x00129758
		private void DrawResetKeychainButton()
		{
			Rect position = new Rect((float)Screen.width * 0.7f, 0f, (float)Screen.width * 0.3f, (float)Screen.height * 0.2f);
			this._resetKeychainButtonStyle.fontSize = Mathf.RoundToInt((float)Screen.height * 0.05f);
			if (GUI.Button(position, "Начать заново", this._resetKeychainButtonStyle))
			{
				this.OnResetButtonClicked();
			}
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x0012B5D0 File Offset: 0x001297D0
		private void OnApplicationQuit()
		{
		}

		// Token: 0x04002A71 RID: 10865
		[SerializeField]
		private GUIStyle _resetKeychainButtonStyle;

		// Token: 0x04002A72 RID: 10866
		private bool _lock;
	}
}
