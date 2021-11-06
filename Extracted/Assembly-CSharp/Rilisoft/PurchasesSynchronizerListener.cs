using System;
using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000718 RID: 1816
	internal sealed class PurchasesSynchronizerListener : MonoBehaviour
	{
		// Token: 0x06003F59 RID: 16217 RVA: 0x00153618 File Offset: 0x00151818
		private void Start()
		{
			PurchasesSynchronizer.Instance.PurchasesSavingStarted += this.HandlePurchasesSavingStarted;
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x00153630 File Offset: 0x00151830
		private void OnDestroy()
		{
			PurchasesSynchronizer.Instance.PurchasesSavingStarted -= this.HandlePurchasesSavingStarted;
			if (this._escapeSubscription != null)
			{
				this._escapeSubscription.Dispose();
			}
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x0015366C File Offset: 0x0015186C
		private void HandlePurchasesSavingStarted(object sender, PurchasesSavingEventArgs e)
		{
			string callee = string.Format(CultureInfo.InvariantCulture, "{0}.HandlePurchasesSavingStarted()", new object[]
			{
				base.GetType().Name
			});
			using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
			{
				try
				{
					this._escapeSubscription = BackSystem.Instance.Register(new Action(this.HandleEscape), "PurchasesSynchronizerListener");
					string activeWithCaption = LocalizationStore.Get("Key_1974");
					ActivityIndicator.SetActiveWithCaption(activeWithCaption);
					InfoWindowController.BlockAllClick();
					base.StartCoroutine(this.WaitCompletionCoroutine(e.Future));
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x00153748 File Offset: 0x00151948
		private IEnumerator WaitCompletionCoroutine(Task<bool> future)
		{
			string thisMethod = string.Format(CultureInfo.InvariantCulture, "{0}.WaitCompletionCoroutine()", new object[]
			{
				base.GetType().Name
			});
			using (new ScopeLogger(thisMethod, Defs.IsDeveloperBuild))
			{
				while (!future.IsCompleted)
				{
					yield return null;
				}
				InfoWindowController.HideCurrentWindow();
				ActivityIndicator.IsActiveIndicator = false;
				if (this._escapeSubscription != null)
				{
					this._escapeSubscription.Dispose();
				}
			}
			yield break;
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x00153774 File Offset: 0x00151974
		private void HandleEscape()
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("Ignoring [Escape] while syncing.");
			}
		}

		// Token: 0x04002E9A RID: 11930
		private IDisposable _escapeSubscription;
	}
}
