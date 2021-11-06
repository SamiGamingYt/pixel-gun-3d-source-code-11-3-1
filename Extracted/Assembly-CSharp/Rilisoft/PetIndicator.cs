using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006F3 RID: 1779
	public class PetIndicator : MonoBehaviour
	{
		// Token: 0x06003DD5 RID: 15829 RVA: 0x00141A8C File Offset: 0x0013FC8C
		private void Awake()
		{
			this._shaderEnable = Shader.Find("Unlit/Transparent Colored");
			this._shaderDisable = Shader.Find("Unlit/TransparentGrayscale");
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x00141ABC File Offset: 0x0013FCBC
		private void Start()
		{
			if (!GlobalGameController.LeftHanded)
			{
				base.transform.localPosition += base.transform.right * ((float)Screen.width * (768f / (float)Screen.height) - base.transform.localPosition.x * 2f);
			}
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x00141B28 File Offset: 0x0013FD28
		private void Update()
		{
			if (WeaponManager.sharedManager == null || WeaponManager.sharedManager.myPlayerMoveC == null || WeaponManager.sharedManager.myPlayerMoveC.myPetEngine == null)
			{
				this._rootObject.SetActiveSafe(false);
				return;
			}
			this._rootObject.SetActiveSafe(true);
			this._myPetEngine = WeaponManager.sharedManager.myPlayerMoveC.myPetEngine;
			if (this._prevPetId != this._myPetEngine.Info.IdWithoutUp)
			{
				this._iconSprite.sprite2D = Resources.Load<Sprite>(string.Format("Pets/Icons/{0}_icon", this._myPetEngine.Info.IdWithoutUp));
				this._prevPetId = this._myPetEngine.Info.IdWithoutUp;
			}
			if (this._prevHp != this._myPetEngine.CurrentHealth)
			{
				if (this._myPetEngine.CurrentHealth > 0f)
				{
					this._hpBarObject.SetActiveSafe(true);
					Vector3 localScale = new Vector3(this._myPetEngine.CurrentHealth / this._myPetEngine.Info.HP, this._HPIndicator.localScale.y, this._HPIndicator.localScale.z);
					this._HPIndicator.localScale = localScale;
					this._iconSprite.shader = this._shaderEnable;
				}
				else
				{
					this._hpBarObject.SetActiveSafe(false);
					this._iconSprite.shader = this._shaderDisable;
				}
				this._prevHp = this._myPetEngine.CurrentHealth;
			}
			if (WeaponManager.sharedManager.myPlayerMoveC.myPetEngine.RespawnTimeLeft > 0f)
			{
				this._RewiveIndicator.gameObject.SetActive(true);
				this._RewiveIndicator.fillAmount = (WeaponManager.sharedManager.myPlayerMoveC.myPetEngine.RespawnTime - WeaponManager.sharedManager.myPlayerMoveC.myPetEngine.RespawnTimeLeft) / WeaponManager.sharedManager.myPlayerMoveC.myPetEngine.RespawnTime;
			}
			else
			{
				this._RewiveIndicator.gameObject.SetActive(false);
			}
		}

		// Token: 0x04002DAA RID: 11690
		[SerializeField]
		private GameObject _rootObject;

		// Token: 0x04002DAB RID: 11691
		[SerializeField]
		private GameObject _hpBarObject;

		// Token: 0x04002DAC RID: 11692
		[SerializeField]
		private UI2DSprite _iconSprite;

		// Token: 0x04002DAD RID: 11693
		[SerializeField]
		private Transform _HPIndicator;

		// Token: 0x04002DAE RID: 11694
		[SerializeField]
		private UITexture _RewiveIndicator;

		// Token: 0x04002DAF RID: 11695
		private PetEngine _myPetEngine;

		// Token: 0x04002DB0 RID: 11696
		private string _prevPetId;

		// Token: 0x04002DB1 RID: 11697
		private float _prevHp;

		// Token: 0x04002DB2 RID: 11698
		private Shader _shaderEnable;

		// Token: 0x04002DB3 RID: 11699
		private Shader _shaderDisable;
	}
}
