using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x020007CB RID: 1995
public sealed class SkinName : MonoBehaviour
{
	// Token: 0x17000BEC RID: 3052
	// (get) Token: 0x0600486D RID: 18541 RVA: 0x001915CC File Offset: 0x0018F7CC
	// (set) Token: 0x0600486E RID: 18542 RVA: 0x0019161C File Offset: 0x0018F81C
	public AudioClip walkMech
	{
		get
		{
			return (!Defs.isDaterRegim) ? ((!(this.playerMoveC.currentMech != null)) ? null : this.playerMoveC.currentMech.stepSound) : this.walkMechBear;
		}
		set
		{
		}
	}

	// Token: 0x0600486F RID: 18543 RVA: 0x00191620 File Offset: 0x0018F820
	public void MoveCamera(Vector2 delta)
	{
		this.firstPersonControl.MoveCamera(delta);
	}

	// Token: 0x06004870 RID: 18544 RVA: 0x00191630 File Offset: 0x0018F830
	public void BlockFirstPersonController()
	{
		this.firstPersonControl.enabled = false;
	}

	// Token: 0x06004871 RID: 18545 RVA: 0x00191640 File Offset: 0x0018F840
	public void sendAnimJump()
	{
		int num = (!this.character.isGrounded) ? 2 : 0;
		if (this.interpolateScript.myAnim != num)
		{
			if (Defs.isSoundFX && num == 2 && !EffectsController.WeAreStealth)
			{
				NGUITools.PlaySound(this.jumpAudio);
			}
			this.interpolateScript.myAnim = num;
			this.interpolateScript.weAreSteals = EffectsController.WeAreStealth;
			if (this.isMulti)
			{
				this.SetAnim(num, EffectsController.WeAreStealth);
			}
		}
	}

	// Token: 0x06004872 RID: 18546 RVA: 0x001916D0 File Offset: 0x0018F8D0
	[RPC]
	[PunRPC]
	public void SetAnim(int _typeAnim, bool stealth)
	{
		string text = "Idle";
		this.currentAnim = _typeAnim;
		if (_typeAnim == 0)
		{
			text = "Idle";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
			this._playWalkSound = false;
		}
		else if (_typeAnim == 1)
		{
			text = "Walk";
			if (!stealth && Defs.isSoundFX)
			{
				this._playWalkSound = true;
			}
		}
		else if (_typeAnim == 2)
		{
			text = "Jump";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (_typeAnim == 4)
		{
			text = "Walk_Back";
			if (!stealth && Defs.isSoundFX)
			{
				this._playWalkSound = true;
			}
		}
		else if (_typeAnim == 5)
		{
			text = "Walk_Left";
			if (!stealth && Defs.isSoundFX)
			{
				this._playWalkSound = true;
			}
		}
		else if (_typeAnim == 6)
		{
			text = "Walk_Right";
			if (!stealth && Defs.isSoundFX)
			{
				this._playWalkSound = true;
			}
		}
		if (_typeAnim == 7)
		{
			text = "Jetpack_Run_Front";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (_typeAnim == 8)
		{
			text = "Jetpack_Run_Back";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (_typeAnim == 9)
		{
			text = "Jetpack_Run_Left";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (_typeAnim == 10)
		{
			text = "Jetpack_Run_Righte";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (_typeAnim == 11)
		{
			text = "Jetpack_Idle";
			if (Defs.isSoundFX)
			{
				this._audio.Stop();
			}
		}
		if (this.isMulti && !this.isMine)
		{
			if (this.playerMoveC.isMechActive || this.playerMoveC.isBearActive)
			{
				if (Defs.isDaterRegim)
				{
					this.playerMoveC.mechBearBodyAnimation.Play(text);
				}
				else if (this.playerMoveC.currentMech != null)
				{
					this.playerMoveC.currentMech.bodyAnimation.Play(text);
				}
			}
			this.FPSplayerObject.GetComponent<Animation>().Play(text);
			if (this.capesPoint.transform.childCount > 0 && this.capesPoint.transform.GetChild(0).GetComponent<Animation>().GetClip(text) != null)
			{
				this.capesPoint.transform.GetChild(0).GetComponent<Animation>().Play(text);
			}
		}
	}

	// Token: 0x06004873 RID: 18547 RVA: 0x00191978 File Offset: 0x0018FB78
	[PunRPC]
	[RPC]
	private void SetAnim(int _typeAnim)
	{
		this.SetAnim(_typeAnim, true);
	}

	// Token: 0x06004874 RID: 18548 RVA: 0x00191984 File Offset: 0x0018FB84
	[PunRPC]
	private void setCapeCustomRPC(byte[] _skinByte)
	{
		Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
		texture2D.LoadImage(_skinByte);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		if (texture2D.width != 12 || texture2D.height != 16)
		{
			return;
		}
		this.currentCapeTex = texture2D;
		this.currentCape = "cape_Custom";
		this.SetCapeModel(this.currentCape, this.currentCapeTex, this._currentIsWearInvisible);
	}

	// Token: 0x06004875 RID: 18549 RVA: 0x001919F8 File Offset: 0x0018FBF8
	[RPC]
	private void setCapeCustomRPCLocal(string str)
	{
		byte[] data = Convert.FromBase64String(str);
		Texture2D texture2D = new Texture2D(12, 16, TextureFormat.ARGB32, false);
		texture2D.LoadImage(data);
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		if (texture2D.width != 12 || texture2D.height != 16)
		{
			return;
		}
		this.SetCapeModel("cape_Custom", texture2D, this._currentIsWearInvisible);
	}

	// Token: 0x06004876 RID: 18550 RVA: 0x00191A5C File Offset: 0x0018FC5C
	private IEnumerator SetCapeCurrentModel(string cape, Texture capeTex)
	{
		if (Device.isPixelGunLow)
		{
			yield break;
		}
		LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Capes/" + cape, false);
		while (!request.isDone)
		{
			yield return null;
		}
		GameObject _capPrefab = request.asset as GameObject;
		if (_capPrefab == null)
		{
			yield break;
		}
		GameObject _cap = UnityEngine.Object.Instantiate<GameObject>(_capPrefab);
		Transform _capTransform = _cap.transform;
		_capTransform.parent = this.capesPoint.transform;
		_capTransform.localPosition = Vector3.zero;
		_capTransform.localRotation = Quaternion.identity;
		if (cape.Equals("cape_Custom"))
		{
			_cap.GetComponent<CustomCapePicker>().shouldLoadTexture = false;
			Player_move_c.SetTextureRecursivelyFrom(_cap, capeTex, new GameObject[0]);
		}
		yield break;
	}

	// Token: 0x06004877 RID: 18551 RVA: 0x00191A94 File Offset: 0x0018FC94
	private void UpdateEffectsOnPlayerMoveC()
	{
		if (this.playerMoveC != null)
		{
			this.playerMoveC.UpdateEffectsForCurrentWeapon(this.currentCape, this.currentMask, this.currentHat);
		}
		else
		{
			Debug.LogError("playerMoveC.UpdateEffectsForCurrentWeapon playerMoveC == null");
		}
	}

	// Token: 0x06004878 RID: 18552 RVA: 0x00191AD4 File Offset: 0x0018FCD4
	[PunRPC]
	[RPC]
	private void setCapeRPC(string _currentCape)
	{
		this.SetCapeModel(_currentCape, null, this._currentIsWearInvisible);
	}

	// Token: 0x06004879 RID: 18553 RVA: 0x00191AE4 File Offset: 0x0018FCE4
	private void SetCapeModel(string cape, Texture tex, bool isInvisible)
	{
		this.currentCapeTex = tex;
		this.currentCape = cape;
		if (this.capesPoint.transform.childCount > 0)
		{
			for (int i = 0; i < this.capesPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.capesPoint.transform.GetChild(i).gameObject);
			}
		}
		this.UpdateEffectsOnPlayerMoveC();
		if (isInvisible || cape == ShopNGUIController.NoneEquippedForWearCategory(ShopNGUIController.CategoryNames.CapesCategory))
		{
			return;
		}
		base.StartCoroutine(this.SetCapeCurrentModel(cape, tex));
	}

	// Token: 0x0600487A RID: 18554 RVA: 0x00191B80 File Offset: 0x0018FD80
	[PunRPC]
	[RPC]
	private void SetArmorVisInvisibleRPC(string _currentArmor, bool _isInviseble)
	{
		if (this.armorPoint.transform.childCount > 0)
		{
			for (int i = 0; i < this.armorPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.armorPoint.transform.GetChild(i).gameObject);
			}
		}
		this.currentArmor = _currentArmor;
		if (_isInviseble)
		{
			return;
		}
		base.StartCoroutine(this.SetArmorModel(_isInviseble));
	}

	// Token: 0x0600487B RID: 18555 RVA: 0x00191BFC File Offset: 0x0018FDFC
	private IEnumerator SetArmorModel(bool invisible)
	{
		GameObject _armPrefab = null;
		if (Device.isPixelGunLow && !string.IsNullOrEmpty(this.currentArmor) && this.currentArmor != Defs.ArmorNewNoneEqupped)
		{
			_armPrefab = Resources.Load<GameObject>("Armor_Low");
		}
		else
		{
			LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Armor/" + this.currentArmor, false);
			while (!request.isDone)
			{
				yield return null;
			}
			_armPrefab = (request.asset as GameObject);
		}
		if (_armPrefab == null)
		{
			yield break;
		}
		GameObject _armor = UnityEngine.Object.Instantiate<GameObject>(_armPrefab);
		Transform _armorTranform = _armor.transform;
		if (Device.isPixelGunLow)
		{
			try
			{
				SkinnedMeshRenderer armorRendered = _armorTranform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>();
				armorRendered.material = Resources.Load<Material>("LowPolyArmorMaterials/" + this.currentArmor + "_low");
			}
			catch (Exception ex)
			{
				Exception e = ex;
				Debug.LogError(string.Concat(new object[]
				{
					"Exception setting material for low armor: ",
					this.currentArmor,
					"   exception: ",
					e
				}));
			}
		}
		if (invisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(_armorTranform, false);
		}
		ArmorRefs ar = _armorTranform.GetChild(0).GetComponent<ArmorRefs>();
		if (ar != null)
		{
			if (this.playerMoveC != null && this.playerMoveC.transform.childCount > 0)
			{
				WeaponSounds ws = this.playerMoveC.myCurrentWeaponSounds;
				ar.leftBone.GetComponent<SetPosInArmor>().target = ws.LeftArmorHand;
				ar.rightBone.GetComponent<SetPosInArmor>().target = ws.RightArmorHand;
			}
			_armorTranform.parent = this.armorPoint.transform;
			_armorTranform.localPosition = Vector3.zero;
			_armorTranform.localRotation = Quaternion.identity;
			_armorTranform.localScale = new Vector3(1f, 1f, 1f);
		}
		yield break;
	}

	// Token: 0x0600487C RID: 18556 RVA: 0x00191C28 File Offset: 0x0018FE28
	[PunRPC]
	[RPC]
	private void setBootsRPC(string _currentBoots)
	{
		this.SetBoots(_currentBoots, this._currentIsWearInvisible);
	}

	// Token: 0x0600487D RID: 18557 RVA: 0x00191C38 File Offset: 0x0018FE38
	private void SetBoots(string itemId, bool isInvisible)
	{
		if (this.LeftBootPoint.transform.childCount > 0)
		{
			UnityEngine.Object.Destroy(this.LeftBootPoint.transform.GetChild(0).gameObject);
		}
		if (this.RightBootPoint.transform.childCount > 0)
		{
			UnityEngine.Object.Destroy(this.RightBootPoint.transform.GetChild(0).gameObject);
		}
		this.currentBoots = itemId;
		if (this.currentBoots.IsNullOrEmpty() || this.currentBoots.Equals(Defs.BootsNoneEqupped))
		{
			return;
		}
		if (isInvisible)
		{
			return;
		}
		base.StartCoroutine(this.SetBootsModel(isInvisible));
	}

	// Token: 0x0600487E RID: 18558 RVA: 0x00191CEC File Offset: 0x0018FEEC
	private IEnumerator SetBootsModel(bool isInvisible)
	{
		if (!Device.isPixelGunLow)
		{
			LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Boots/BootPrefab", false);
			while (!request.isDone)
			{
				yield return null;
			}
			GameObject bootPrefab = request.asset as GameObject;
			if (bootPrefab != null)
			{
				GameObject leftBootInstance = UnityEngine.Object.Instantiate<GameObject>(bootPrefab);
				GameObject rightBootInstance = UnityEngine.Object.Instantiate<GameObject>(bootPrefab);
				leftBootInstance.transform.SetParent(this.LeftBootPoint.transform, false);
				rightBootInstance.transform.SetParent(this.RightBootPoint.transform, false);
				leftBootInstance.transform.localScale = new Vector3(-1f, 1f, 1f);
				leftBootInstance.GetComponent<BootsMaterial>().SetBootsMaterial(Defs.bootsMaterialDict[this.currentBoots]);
				rightBootInstance.GetComponent<BootsMaterial>().SetBootsMaterial(Defs.bootsMaterialDict[this.currentBoots]);
				if (isInvisible)
				{
					ShopNGUIController.SetRenderersVisibleFromPoint(leftBootInstance.transform, false);
					ShopNGUIController.SetRenderersVisibleFromPoint(rightBootInstance.transform, false);
				}
			}
		}
		yield break;
	}

	// Token: 0x0600487F RID: 18559 RVA: 0x00191D18 File Offset: 0x0018FF18
	[PunRPC]
	[RPC]
	private void SetMaskRPC(string _currentMask)
	{
		this.SetMask(_currentMask, this._currentIsWearInvisible);
	}

	// Token: 0x06004880 RID: 18560 RVA: 0x00191D28 File Offset: 0x0018FF28
	private void SetMask(string itemId, bool isInvisible)
	{
		if (this.maskPoint.transform.childCount > 0)
		{
			for (int i = 0; i < this.maskPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.maskPoint.transform.GetChild(i).gameObject);
			}
		}
		this.currentMask = itemId;
		this.UpdateEffectsOnPlayerMoveC();
		if (isInvisible)
		{
			return;
		}
		base.StartCoroutine(this.SetMaskModel(isInvisible));
	}

	// Token: 0x06004881 RID: 18561 RVA: 0x00191DAC File Offset: 0x0018FFAC
	private IEnumerator SetMaskModel(bool isInvisible)
	{
		if (!Device.isPixelGunLow)
		{
			LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Masks/" + this.currentMask, false);
			while (!request.isDone)
			{
				yield return null;
			}
			GameObject maskPrefab = request.asset as GameObject;
			if (maskPrefab != null)
			{
				GameObject maskInstance = UnityEngine.Object.Instantiate<GameObject>(maskPrefab);
				Transform maskTransform = maskInstance.transform;
				maskTransform.parent = this.maskPoint.transform;
				maskTransform.localPosition = Vector3.zero;
				maskTransform.localRotation = Quaternion.identity;
				maskTransform.localScale = Vector3.one;
				if (isInvisible)
				{
					ShopNGUIController.SetRenderersVisibleFromPoint(maskTransform, false);
				}
			}
		}
		yield break;
	}

	// Token: 0x06004882 RID: 18562 RVA: 0x00191DD8 File Offset: 0x0018FFD8
	[PunRPC]
	[RPC]
	private void SetHatWithInvisebleRPC(string _currentHat, bool _isHatInviseble)
	{
		this.SetHat(_currentHat, _isHatInviseble || this._currentIsWearInvisible);
	}

	// Token: 0x06004883 RID: 18563 RVA: 0x00191DF0 File Offset: 0x0018FFF0
	private void SetHat(string itemId, bool isInvisible)
	{
		if (this.hatsPoint.transform.childCount > 0)
		{
			for (int i = 0; i < this.hatsPoint.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.hatsPoint.transform.GetChild(i).gameObject);
			}
		}
		this.currentHat = itemId;
		this.UpdateEffectsOnPlayerMoveC();
		if (isInvisible)
		{
			return;
		}
		base.StartCoroutine(this.SetHatModel(isInvisible));
	}

	// Token: 0x06004884 RID: 18564 RVA: 0x00191E74 File Offset: 0x00190074
	private IEnumerator SetHatModel(bool invisible)
	{
		if (Device.isPixelGunLow)
		{
			yield break;
		}
		LoadAsyncTool.ObjectRequest request = LoadAsyncTool.Get("Hats/" + this.currentHat, false);
		while (!request.isDone)
		{
			yield return null;
		}
		GameObject _hatPrefab = request.asset as GameObject;
		if (_hatPrefab == null)
		{
			yield break;
		}
		GameObject _hat = UnityEngine.Object.Instantiate<GameObject>(_hatPrefab);
		Transform _hatTransform = _hat.transform;
		_hatTransform.parent = this.hatsPoint.transform;
		_hatTransform.localPosition = Vector3.zero;
		_hatTransform.localRotation = Quaternion.identity;
		_hatTransform.localScale = Vector3.one;
		if (invisible)
		{
			ShopNGUIController.SetRenderersVisibleFromPoint(_hatTransform, false);
		}
		yield break;
	}

	// Token: 0x06004885 RID: 18565 RVA: 0x00191EA0 File Offset: 0x001900A0
	private void Awake()
	{
		this.isLocal = !Defs.isInet;
		this.firstPersonControl = base.GetComponent<FirstPersonControlSharp>();
		this._audio = base.GetComponent<AudioSource>();
		this.photonView = PhotonView.Get(this);
	}

	// Token: 0x06004886 RID: 18566 RVA: 0x00191EE0 File Offset: 0x001900E0
	private void Start()
	{
		this._weaponManager = WeaponManager.sharedManager;
		this.playerMoveC = this.playerGameObject.GetComponent<Player_move_c>();
		this.character = base.transform.GetComponent<CharacterController>();
		this.isMulti = Defs.isMulti;
		this.pixelView = base.GetComponent<PixelView>();
		if (this.photonView && this.photonView.isMine)
		{
			PhotonObjectCacher.AddObject(base.gameObject);
		}
		this.isInet = Defs.isInet;
		if (!this.isInet)
		{
			this.isMine = base.GetComponent<NetworkView>().isMine;
		}
		else
		{
			this.isMine = this.photonView.isMine;
		}
		if (((!Defs.isInet && !base.GetComponent<NetworkView>().isMine) || (Defs.isInet && !this.photonView.isMine)) && Defs.isMulti)
		{
			this.camPlayer.active = false;
			this.character.enabled = false;
		}
		else
		{
			this.FPSplayerObject.SetActive(false);
		}
		if (!Defs.isMulti || (!Defs.isInet && base.GetComponent<NetworkView>().isMine) || (Defs.isInet && this.photonView.isMine))
		{
			base.gameObject.layer = 11;
			this.bodyLayer.layer = 11;
			this.headObj.layer = 11;
		}
		if (this.isMine)
		{
			this.SetWearVisible(null);
			this.SetCape(null);
			this.SetHat(null);
			this.SetBoots(null);
			this.SetArmor(null);
			this.SetMask(null);
			this.SetPet(null);
			this.SetGadgetes(null);
		}
	}

	// Token: 0x06004887 RID: 18567 RVA: 0x001920A8 File Offset: 0x001902A8
	private void OnDestroy()
	{
		if (this._armorPopularityCacheIsDirty)
		{
			Statistics.Instance.SaveArmorPopularity();
			this._armorPopularityCacheIsDirty = false;
		}
		PhotonObjectCacher.RemoveObject(base.gameObject);
	}

	// Token: 0x06004888 RID: 18568 RVA: 0x001920D4 File Offset: 0x001902D4
	public void SetMask(PhotonPlayer player = null)
	{
		if (Defs.isHunger)
		{
			return;
		}
		string @string = Storager.getString("MaskEquippedSN", false);
		this.currentMask = @string;
		this.UpdateEffectsOnPlayerMoveC();
		if (!Defs.isMulti)
		{
			return;
		}
		if (this.isInet)
		{
			if (player == null)
			{
				this.photonView.RPC("SetMaskRPC", PhotonTargets.Others, new object[]
				{
					@string
				});
			}
			else
			{
				this.photonView.RPC("SetMaskRPC", player, new object[]
				{
					@string
				});
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SetMaskRPC", RPCMode.Others, new object[]
			{
				@string
			});
		}
	}

	// Token: 0x06004889 RID: 18569 RVA: 0x0019217C File Offset: 0x0019037C
	public void SetCape(PhotonPlayer player = null)
	{
		if (Defs.isHunger)
		{
			return;
		}
		string @string = Storager.getString(Defs.CapeEquppedSN, false);
		this.currentCape = @string;
		this.UpdateEffectsOnPlayerMoveC();
		if (!Defs.isMulti)
		{
			return;
		}
		if (!@string.Equals("cape_Custom"))
		{
			if (this.isInet)
			{
				if (player == null)
				{
					this.photonView.RPC("setCapeRPC", PhotonTargets.Others, new object[]
					{
						@string
					});
				}
				else
				{
					this.photonView.RPC("setCapeRPC", player, new object[]
					{
						@string
					});
				}
			}
			else
			{
				base.GetComponent<NetworkView>().RPC("setCapeRPC", RPCMode.Others, new object[]
				{
					@string
				});
			}
			return;
		}
		if (@string.Equals("cape_Custom"))
		{
			Texture2D capeUserTexture = SkinsController.capeUserTexture;
			byte[] array = capeUserTexture.EncodeToPNG();
			if (capeUserTexture.width != 12 || capeUserTexture.height != 16)
			{
				return;
			}
			if (this.isInet)
			{
				if (player == null)
				{
					this.photonView.RPC("setCapeCustomRPC", PhotonTargets.Others, new object[]
					{
						array
					});
				}
				else
				{
					this.photonView.RPC("setCapeCustomRPC", player, new object[]
					{
						array
					});
				}
			}
			else
			{
				string text = Convert.ToBase64String(array);
				base.GetComponent<NetworkView>().RPC("setCapeCustomRPCLocal", RPCMode.Others, new object[]
				{
					text
				});
			}
		}
	}

	// Token: 0x0600488A RID: 18570 RVA: 0x001922E0 File Offset: 0x001904E0
	public void SetArmor(PhotonPlayer player = null)
	{
		if (Defs.isHunger || Defs.isDaterRegim)
		{
			return;
		}
		string @string = Storager.getString(Defs.ArmorNewEquppedSN, false);
		this.currentArmor = @string;
		if (!Defs.isMulti)
		{
			return;
		}
		bool flag = !ShopNGUIController.ShowArmor;
		if (this.isInet)
		{
			if (player == null)
			{
				this.photonView.RPC("SetArmorVisInvisibleRPC", PhotonTargets.Others, new object[]
				{
					@string,
					flag
				});
			}
			else
			{
				this.photonView.RPC("SetArmorVisInvisibleRPC", player, new object[]
				{
					@string,
					flag
				});
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SetArmorVisInvisibleRPC", RPCMode.Others, new object[]
			{
				@string,
				flag
			});
		}
		this.IncrementArmorPopularity(@string);
	}

	// Token: 0x0600488B RID: 18571 RVA: 0x001923B8 File Offset: 0x001905B8
	public void SetBoots(PhotonPlayer player = null)
	{
		string @string = Storager.getString(Defs.BootsEquppedSN, false);
		this.currentBoots = @string;
		if (Defs.isHunger)
		{
			this.currentBoots = string.Empty;
		}
		if (!Defs.isMulti)
		{
			return;
		}
		if (this.isInet)
		{
			if (player == null)
			{
				this.photonView.RPC("setBootsRPC", PhotonTargets.Others, new object[]
				{
					@string
				});
			}
			else
			{
				this.photonView.RPC("setBootsRPC", player, new object[]
				{
					@string
				});
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("setBootsRPC", RPCMode.Others, new object[]
			{
				@string
			});
		}
	}

	// Token: 0x0600488C RID: 18572 RVA: 0x00192464 File Offset: 0x00190664
	public void SetPet(PhotonPlayer player = null)
	{
		this.currentPet = Singleton<PetsManager>.Instance.GetEqipedPetId();
		if (!Defs.isMulti)
		{
			return;
		}
		if (Defs.isInet)
		{
			if (player == null)
			{
				this.photonView.RPC("SetPetRPC", PhotonTargets.Others, new object[]
				{
					this.currentPet
				});
			}
			else
			{
				this.photonView.RPC("SetPetRPC", player, new object[]
				{
					this.currentPet
				});
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SetPetRPC", RPCMode.Others, new object[]
			{
				this.currentPet
			});
		}
	}

	// Token: 0x0600488D RID: 18573 RVA: 0x00192508 File Offset: 0x00190708
	public void SetWearVisible(PhotonPlayer player = null)
	{
		this._currentIsWearInvisible = !ShopNGUIController.ShowWear;
		if (Defs.isHunger)
		{
			return;
		}
		if (!Defs.isMulti)
		{
			return;
		}
		if (this.isInet && !PhotonNetwork.connected)
		{
			return;
		}
		if (this.isInet)
		{
			if (player == null)
			{
				this.photonView.RPC("SetWearIsInvisibleRPC", PhotonTargets.Others, new object[]
				{
					this._currentIsWearInvisible
				});
			}
			else
			{
				this.photonView.RPC("SetWearIsInvisibleRPC", player, new object[]
				{
					this._currentIsWearInvisible
				});
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SetWearIsInvisibleRPC", RPCMode.Others, new object[]
			{
				this._currentIsWearInvisible
			});
		}
	}

	// Token: 0x0600488E RID: 18574 RVA: 0x001925DC File Offset: 0x001907DC
	[RPC]
	[PunRPC]
	private void SetWearIsInvisibleRPC(bool isInvisible)
	{
		this._currentIsWearInvisible = isInvisible;
		this.SetMask(this.currentMask, this._currentIsWearInvisible);
		this.SetBoots(this.currentBoots, this._currentIsWearInvisible);
		this.SetCapeModel(this.currentCape, this.currentCapeTex, this._currentIsWearInvisible);
		this.SetHat(this.currentHat, this._currentIsWearInvisible);
	}

	// Token: 0x0600488F RID: 18575 RVA: 0x00192640 File Offset: 0x00190840
	[RPC]
	[PunRPC]
	private void SetPetRPC(string _currentPet)
	{
		this.currentPet = _currentPet;
	}

	// Token: 0x06004890 RID: 18576 RVA: 0x0019264C File Offset: 0x0019084C
	public void SetGadgetes(PhotonPlayer player = null)
	{
		string text = string.Empty;
		Gadget gadget = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetInfo.GadgetCategory.Support, out gadget))
		{
			text = gadget.Info.Id;
		}
		string text2 = string.Empty;
		Gadget gadget2 = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetInfo.GadgetCategory.Throwing, out gadget2))
		{
			text2 = gadget2.Info.Id;
		}
		string text3 = string.Empty;
		Gadget gadget3 = null;
		if (InGameGadgetSet.CurrentSet.TryGetValue(GadgetInfo.GadgetCategory.Tools, out gadget3))
		{
			text3 = gadget3.Info.Id;
		}
		this.currentGadgetSupport = text;
		this.currentGadgetThrowing = text2;
		this.currentGadgetTools = text3;
		if (!Defs.isMulti)
		{
			return;
		}
		if (this.isInet)
		{
			if (player == null)
			{
				this.photonView.RPC("SetGadgetesRPC", PhotonTargets.Others, new object[]
				{
					this.currentGadgetSupport,
					this.currentGadgetThrowing,
					this.currentGadgetTools
				});
			}
			else
			{
				this.photonView.RPC("SetGadgetesRPC", player, new object[]
				{
					this.currentGadgetSupport,
					this.currentGadgetThrowing,
					this.currentGadgetTools
				});
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SetGadgetesRPC", RPCMode.Others, new object[]
			{
				this.currentGadgetSupport,
				this.currentGadgetThrowing,
				this.currentGadgetTools
			});
		}
	}

	// Token: 0x06004891 RID: 18577 RVA: 0x001927B0 File Offset: 0x001909B0
	[RPC]
	[PunRPC]
	private void SetGadgetesRPC(string _currentGadgetSupport, string _currentGadgetTrowing, string _currentGadgetTools)
	{
		this.currentGadgetSupport = _currentGadgetSupport;
		this.currentGadgetThrowing = _currentGadgetTrowing;
		this.currentGadgetTools = _currentGadgetTools;
	}

	// Token: 0x06004892 RID: 18578 RVA: 0x001927C8 File Offset: 0x001909C8
	public void OnPhotonPlayerConnected(PhotonPlayer player)
	{
		if (this.photonView && this.photonView.isMine)
		{
			this.SetWearVisible(null);
			this.SetHat(player);
			this.SetCape(player);
			this.SetBoots(player);
			this.SetArmor(player);
			this.SetMask(player);
			this.SetPet(player);
			this.SetGadgetes(player);
		}
	}

	// Token: 0x06004893 RID: 18579 RVA: 0x00192830 File Offset: 0x00190A30
	private void OnPlayerConnected(NetworkPlayer player)
	{
		if (base.GetComponent<NetworkView>().isMine)
		{
			this.SetWearVisible(null);
			this.SetHat(null);
			this.SetCape(null);
			this.SetBoots(null);
			this.SetArmor(null);
			this.SetMask(null);
			this.SetPet(null);
			this.SetGadgetes(null);
		}
	}

	// Token: 0x06004894 RID: 18580 RVA: 0x00192888 File Offset: 0x00190A88
	public void SetHat(PhotonPlayer player = null)
	{
		if (Defs.isHunger)
		{
			return;
		}
		string text = Storager.getString(Defs.HatEquppedSN, false);
		if (text != null && (Defs.isHunger || Defs.isDaterRegim) && !Wear.NonArmorHat(text))
		{
			text = "hat_NoneEquipped";
		}
		this.currentHat = text;
		if (!Defs.isMulti)
		{
			return;
		}
		bool flag = !ShopNGUIController.ShowHat && !Wear.NonArmorHat(text);
		if (this.isInet)
		{
			if (player == null)
			{
				this.photonView.RPC("SetHatWithInvisebleRPC", PhotonTargets.Others, new object[]
				{
					text,
					flag
				});
			}
			else
			{
				this.photonView.RPC("SetHatWithInvisebleRPC", player, new object[]
				{
					text,
					flag
				});
			}
		}
		else
		{
			base.GetComponent<NetworkView>().RPC("SetHatWithInvisebleRPC", RPCMode.Others, new object[]
			{
				text,
				flag
			});
		}
	}

	// Token: 0x06004895 RID: 18581 RVA: 0x00192988 File Offset: 0x00190B88
	private void Update()
	{
		if ((this.isMulti && this.isMine) || !this.isMulti)
		{
			if (this.playerMoveC.isKilled)
			{
				this.isPlayDownSound = false;
			}
			int num = 0;
			if ((this.character.velocity.y > 0.01f || this.character.velocity.y < -0.01f) && !this.character.isGrounded && !Defs.isJetpackEnabled)
			{
				num = 2;
			}
			else if (this.character.velocity.x != 0f || this.character.velocity.z != 0f)
			{
				if (this.character.isGrounded)
				{
					float x = JoystickController.leftJoystick.value.x;
					float y = JoystickController.leftJoystick.value.y;
					if (Mathf.Abs(y) >= Mathf.Abs(x))
					{
						if (y >= 0f)
						{
							num = 1;
						}
						else
						{
							num = 4;
						}
					}
					else if (x >= 0f)
					{
						num = 6;
					}
					else
					{
						num = 5;
					}
				}
				else if (Defs.isJetpackEnabled)
				{
					float x2 = JoystickController.leftJoystick.value.x;
					float y2 = JoystickController.leftJoystick.value.y;
					if (Mathf.Abs(y2) >= Mathf.Abs(x2))
					{
						if (y2 >= 0f)
						{
							num = 7;
						}
						else
						{
							num = 8;
						}
					}
					else if (x2 >= 0f)
					{
						num = 10;
					}
					else
					{
						num = 9;
					}
				}
			}
			else if (Defs.isJetpackEnabled && !this.character.isGrounded)
			{
				num = 11;
			}
			if (this.character.velocity.y < -2.5f && !this.character.isGrounded)
			{
				this.isPlayDownSound = true;
			}
			if (this.isPlayDownSound && this.character.isGrounded)
			{
				if (Defs.isSoundFX && !EffectsController.WeAreStealth)
				{
					NGUITools.PlaySound(this.jumpDownAudio);
				}
				this.isPlayDownSound = false;
			}
			if (num != this.typeAnim)
			{
				this.typeAnim = num;
				if (((this.isMulti && this.isMine) || !this.isMulti) && this.typeAnim != 2)
				{
					this.interpolateScript.myAnim = this.typeAnim;
					this.interpolateScript.weAreSteals = EffectsController.WeAreStealth;
					this.SetAnim(this.typeAnim, EffectsController.WeAreStealth);
				}
			}
		}
		if (this._playWalkSound)
		{
			AudioClip audioClip = (!this.playerMoveC.isMechActive && !this.playerMoveC.isBearActive) ? this.walkAudio : this.walkMech;
			if (!this._audio.isPlaying || this._audio.clip != audioClip)
			{
				this._audio.loop = false;
				this._audio.clip = audioClip;
				this._audio.Play();
			}
		}
	}

	// Token: 0x06004896 RID: 18582 RVA: 0x00192CDC File Offset: 0x00190EDC
	public IEnumerator _SetAndResetImpactedByTrampoline()
	{
		this._impactedByTramp = true;
		yield return new WaitForSeconds(0.1f);
		this._impactedByTramp = false;
		yield break;
	}

	// Token: 0x06004897 RID: 18583 RVA: 0x00192CF8 File Offset: 0x00190EF8
	private void OnControllerColliderHit(ControllerColliderHit col)
	{
		this.onRink = false;
		if ((!this.isMulti || this.isMine) && this._irt != null && !this._impactedByTramp)
		{
			UnityEngine.Object.Destroy(this._irt);
			this._irt = null;
		}
		if (col.gameObject.CompareTag("Conveyor") && (!this.isMulti || this.isMine))
		{
			if (!this.onConveyor)
			{
				this.conveyorDirection = Vector3.zero;
			}
			this.onConveyor = true;
			Conveyor component = col.transform.GetComponent<Conveyor>();
			if (component.accelerateSpeed)
			{
				this.conveyorDirection = Vector3.Lerp(this.conveyorDirection, col.transform.forward * component.maxspeed, component.acceleration);
			}
			else
			{
				this.conveyorDirection = col.transform.forward * component.maxspeed;
			}
			return;
		}
		this.onConveyor = false;
		if (col.gameObject.CompareTag("Rink") && (!this.isMulti || this.isMine))
		{
			this.onRink = true;
			return;
		}
		if (!this._impactedByTramp && (col.gameObject.CompareTag("Trampoline") || col.gameObject.CompareTag("ConveyorTrampoline")) && (!this.isMulti || this.isMine))
		{
			if (this._irt == null)
			{
				this._irt = base.gameObject.AddComponent<ImpactReceiverTrampoline>();
			}
			if (col.gameObject.CompareTag("Trampoline"))
			{
				TrampolineParameters component2 = col.gameObject.GetComponent<TrampolineParameters>();
				this._irt.AddImpact(col.transform.up, (!(component2 != null)) ? 45f : component2.force);
			}
			else
			{
				this._irt.AddImpact(col.transform.forward, this.conveyorDirection.magnitude * 1.4f);
				this.conveyorDirection = Vector3.zero;
			}
			if (Defs.isSoundFX)
			{
				AudioSource component3 = col.gameObject.GetComponent<AudioSource>();
				if (component3 != null)
				{
					component3.Play();
				}
			}
			base.StartCoroutine(this._SetAndResetImpactedByTrampoline());
			return;
		}
		bool flag = !this.isMulti || this.isMine;
		if (flag && this.IsDeadCollider(col.gameObject) && !this.playerMoveC.isKilled)
		{
			this.isPlayDownSound = false;
			this.playerMoveC.KillSelf();
		}
	}

	// Token: 0x06004898 RID: 18584 RVA: 0x00192FB4 File Offset: 0x001911B4
	private bool IsDeadCollider(GameObject go)
	{
		return go.name == "DeadCollider";
	}

	// Token: 0x06004899 RID: 18585 RVA: 0x00192FC8 File Offset: 0x001911C8
	private void OnTriggerEnter(Collider col)
	{
		if ((!this.isMulti || this.isMine) && col.gameObject.name.Equals("DamageCollider"))
		{
			col.gameObject.GetComponent<DamageCollider>().RegisterPlayer();
		}
	}

	// Token: 0x0600489A RID: 18586 RVA: 0x00193018 File Offset: 0x00191218
	private void OnTriggerExit(Collider col)
	{
		if ((!this.isMulti || this.isMine) && col.gameObject.GetComponent<DamageCollider>() != null)
		{
			col.gameObject.GetComponent<DamageCollider>().UnregisterPlayer();
		}
	}

	// Token: 0x0600489B RID: 18587 RVA: 0x00193064 File Offset: 0x00191264
	private void IncrementArmorPopularity(string currentArmor)
	{
		if (this.isInet && this.isMulti && this.isMine)
		{
			string key = "None";
			if (currentArmor != Defs.ArmorNewNoneEqupped)
			{
				key = ItemDb.GetItemNameNonLocalized(currentArmor, currentArmor, ShopNGUIController.CategoryNames.ArmorCategory, "Unknown");
			}
			Statistics.Instance.IncrementArmorPopularity(key, true);
			this._armorPopularityCacheIsDirty = true;
		}
	}

	// Token: 0x04003555 RID: 13653
	[NonSerialized]
	public string currentHat;

	// Token: 0x04003556 RID: 13654
	[NonSerialized]
	public string currentArmor;

	// Token: 0x04003557 RID: 13655
	[NonSerialized]
	public string currentCape;

	// Token: 0x04003558 RID: 13656
	[NonSerialized]
	public Texture currentCapeTex;

	// Token: 0x04003559 RID: 13657
	[NonSerialized]
	public string currentBoots;

	// Token: 0x0400355A RID: 13658
	[NonSerialized]
	public string currentMask;

	// Token: 0x0400355B RID: 13659
	[NonSerialized]
	public string currentPet;

	// Token: 0x0400355C RID: 13660
	[NonSerialized]
	public string currentGadgetSupport;

	// Token: 0x0400355D RID: 13661
	[NonSerialized]
	public string currentGadgetTools;

	// Token: 0x0400355E RID: 13662
	[NonSerialized]
	public string currentGadgetThrowing;

	// Token: 0x0400355F RID: 13663
	[NonSerialized]
	public bool _currentIsWearInvisible;

	// Token: 0x04003560 RID: 13664
	public Transform onGroundEffectsPoint;

	// Token: 0x04003561 RID: 13665
	public GameObject playerGameObject;

	// Token: 0x04003562 RID: 13666
	public Player_move_c playerMoveC;

	// Token: 0x04003563 RID: 13667
	public string skinName;

	// Token: 0x04003564 RID: 13668
	public GameObject hatsPoint;

	// Token: 0x04003565 RID: 13669
	public GameObject capesPoint;

	// Token: 0x04003566 RID: 13670
	public GameObject bootsPoint;

	// Token: 0x04003567 RID: 13671
	public GameObject armorPoint;

	// Token: 0x04003568 RID: 13672
	public GameObject maskPoint;

	// Token: 0x04003569 RID: 13673
	public GameObject LeftBootPoint;

	// Token: 0x0400356A RID: 13674
	public GameObject RightBootPoint;

	// Token: 0x0400356B RID: 13675
	public string NickName;

	// Token: 0x0400356C RID: 13676
	public GameObject camPlayer;

	// Token: 0x0400356D RID: 13677
	public GameObject headObj;

	// Token: 0x0400356E RID: 13678
	public GameObject bodyLayer;

	// Token: 0x0400356F RID: 13679
	public CharacterController character;

	// Token: 0x04003570 RID: 13680
	public PhotonView photonView;

	// Token: 0x04003571 RID: 13681
	public PixelView pixelView;

	// Token: 0x04003572 RID: 13682
	public int typeAnim;

	// Token: 0x04003573 RID: 13683
	public WeaponManager _weaponManager;

	// Token: 0x04003574 RID: 13684
	public bool isInet;

	// Token: 0x04003575 RID: 13685
	public bool isLocal;

	// Token: 0x04003576 RID: 13686
	public bool isMine;

	// Token: 0x04003577 RID: 13687
	public bool isMulti;

	// Token: 0x04003578 RID: 13688
	public AudioClip walkAudio;

	// Token: 0x04003579 RID: 13689
	public AudioClip jumpAudio;

	// Token: 0x0400357A RID: 13690
	public AudioClip jumpDownAudio;

	// Token: 0x0400357B RID: 13691
	public AudioClip walkMechBear;

	// Token: 0x0400357C RID: 13692
	public bool isPlayDownSound;

	// Token: 0x0400357D RID: 13693
	public GameObject FPSplayerObject;

	// Token: 0x0400357E RID: 13694
	public ThirdPersonNetwork1 interpolateScript;

	// Token: 0x0400357F RID: 13695
	private bool _impactedByTramp;

	// Token: 0x04003580 RID: 13696
	public bool onRink;

	// Token: 0x04003581 RID: 13697
	public bool onConveyor;

	// Token: 0x04003582 RID: 13698
	public Vector3 conveyorDirection;

	// Token: 0x04003583 RID: 13699
	private ImpactReceiverTrampoline _irt;

	// Token: 0x04003584 RID: 13700
	private bool _armorPopularityCacheIsDirty;

	// Token: 0x04003585 RID: 13701
	public FirstPersonControlSharp firstPersonControl;

	// Token: 0x04003586 RID: 13702
	public int currentAnim;

	// Token: 0x04003587 RID: 13703
	private bool _playWalkSound;

	// Token: 0x04003588 RID: 13704
	private AudioSource _audio;
}
