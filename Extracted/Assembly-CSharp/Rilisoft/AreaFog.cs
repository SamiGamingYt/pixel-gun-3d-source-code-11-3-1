using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200055F RID: 1375
	public class AreaFog : AreaBase
	{
		// Token: 0x06002FC3 RID: 12227 RVA: 0x000F9A30 File Offset: 0x000F7C30
		private new void Awake()
		{
			this._prevSettings = new FogSettings().FromCurrent();
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x000F9A44 File Offset: 0x000F7C44
		public override void CheckIn(GameObject to)
		{
			base.CheckIn(to);
			this._tokenSource.Cancel();
			this._tokenSource = new CancellationTokenSource();
			base.StartCoroutine(this.Change(this._settings, this.animationTime, this._tokenSource.Token));
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x000F9A94 File Offset: 0x000F7C94
		public override void CheckOut(GameObject from)
		{
			base.CheckOut(from);
			this._tokenSource.Cancel();
			this._tokenSource = new CancellationTokenSource();
			base.StartCoroutine(this.Change(this._prevSettings, this.animationTime, this._tokenSource.Token));
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x000F9AE4 File Offset: 0x000F7CE4
		private IEnumerator Change(FogSettings to, float time, CancellationToken token)
		{
			RenderSettings.fog = to.Active;
			if (!RenderSettings.fog)
			{
				yield break;
			}
			FogSettings fr = new FogSettings().FromCurrent();
			RenderSettings.fogMode = to.Mode;
			float elapsed = 0f;
			while (elapsed < time)
			{
				if (token.IsCancellationRequested)
				{
					yield break;
				}
				elapsed += Time.deltaTime;
				float rate = elapsed / time;
				RenderSettings.fogStartDistance = Mathf.Lerp(fr.Start, to.Start, rate);
				RenderSettings.fogEndDistance = Mathf.Lerp(fr.End, to.End, rate);
				RenderSettings.fogColor = Color.Lerp(fr.Color, to.Color, rate);
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400231A RID: 8986
		[SerializeField]
		private float animationTime = 1f;

		// Token: 0x0400231B RID: 8987
		[SerializeField]
		private FogSettings _settings;

		// Token: 0x0400231C RID: 8988
		[SerializeField]
		[ReadOnly]
		private FogSettings _prevSettings;

		// Token: 0x0400231D RID: 8989
		private CancellationTokenSource _tokenSource = new CancellationTokenSource();
	}
}
