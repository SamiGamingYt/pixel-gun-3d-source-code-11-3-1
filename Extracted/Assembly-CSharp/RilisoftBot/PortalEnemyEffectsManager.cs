using System;
using System.Collections;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x02000591 RID: 1425
	[RequireComponent(typeof(BaseBot))]
	public class PortalEnemyEffectsManager : MonoBehaviour, IEnemyEffectsManager
	{
		// Token: 0x0600319A RID: 12698 RVA: 0x00102418 File Offset: 0x00100618
		private void Awake()
		{
			this._bot = base.GetComponent<BaseBot>();
			this._portalMaterialPref = Resources.Load<Material>("Enemy_Portal");
			if (this._portalMaterialPref == null)
			{
				Debug.LogError("material not found");
			}
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x00102454 File Offset: 0x00100654
		public void ShowSpawnEffect()
		{
			this.ShowSpawnMaterials();
			this.ShowSpawnPortal();
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x00102464 File Offset: 0x00100664
		private void ShowSpawnMaterials()
		{
			base.StartCoroutine(this.ShowSpawnMaterialsCoroutine());
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x00102474 File Offset: 0x00100674
		private IEnumerator ShowSpawnMaterialsCoroutine()
		{
			yield return null;
			Renderer[] rends = base.GetComponentsInChildren<Renderer>();
			foreach (Renderer rend in rends)
			{
				base.StartCoroutine(this.AnimateMaterial(rend));
			}
			yield break;
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x00102490 File Offset: 0x00100690
		private IEnumerator AnimateMaterial(Renderer rend)
		{
			Material baseMaterial = rend.material;
			if (rend.gameObject.GetComponent<BotChangeDamageMaterial>() != null)
			{
				string skinKey = this._bot.name + "_Level" + CurrentCampaignGame.currentLevel;
				Texture tx = SkinsManagerPixlGun.sharedManager.skins[skinKey] as Texture;
				if (tx != null)
				{
					baseMaterial.mainTexture = tx;
				}
			}
			rend.material = new Material(this._portalMaterialPref);
			rend.material.mainTexture = baseMaterial.mainTexture;
			rend.material.SetFloat("_Burn", 0.25f);
			float timeElapsed = 0f;
			while (timeElapsed < 1f)
			{
				timeElapsed += Time.deltaTime;
				float curVal = timeElapsed * 1.25f / 1f;
				rend.material.SetFloat("_Burn", curVal);
				yield return null;
			}
			rend.material = baseMaterial;
			yield return null;
			yield break;
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x001024BC File Offset: 0x001006BC
		private void ShowSpawnPortal()
		{
			EnemyPortal portal = EnemyPortalStackController.sharedController.GetPortal();
			if (portal == null)
			{
				return;
			}
			portal.Show(base.gameObject.transform.position);
		}

		// Token: 0x0400248A RID: 9354
		private const string SpawnShaderParamName = "_Burn";

		// Token: 0x0400248B RID: 9355
		private const float SpawnPlayTime = 1f;

		// Token: 0x0400248C RID: 9356
		private const float SpawnBurnAmountStart = 0.25f;

		// Token: 0x0400248D RID: 9357
		private const float SpawnBurnAmountEnd = 1.25f;

		// Token: 0x0400248E RID: 9358
		private BaseBot _bot;

		// Token: 0x0400248F RID: 9359
		private Material _portalMaterialPref;
	}
}
