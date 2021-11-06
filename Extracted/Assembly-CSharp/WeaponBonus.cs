using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x02000881 RID: 2177
public class WeaponBonus : Photon.MonoBehaviour
{
	// Token: 0x06004E7F RID: 20095 RVA: 0x001C7000 File Offset: 0x001C5200
	private void Start()
	{
		string str = base.gameObject.name.Substring(0, base.gameObject.name.Length - 13);
		this.weaponPrefab = Resources.Load<GameObject>("Weapons/" + str);
		this._weaponManager = WeaponManager.sharedManager;
		this.isHunger = Defs.isHunger;
		if (!this.isHunger)
		{
			this._player = GameObject.FindGameObjectWithTag("Player");
			GameObject gameObject = GameObject.FindGameObjectWithTag("PlayerGun");
			if (gameObject != null)
			{
				this._playerMoveC = gameObject.GetComponent<Player_move_c>();
			}
			else
			{
				Debug.LogWarning("WeaponBonus.Start(): playerGun == null");
			}
		}
		else
		{
			this._player = this._weaponManager.myPlayer;
			if (this._player != null)
			{
				GameObject playerGameObject = this._player.GetComponent<SkinName>().playerGameObject;
				if (playerGameObject != null)
				{
					this._playerMoveC = playerGameObject.GetComponent<Player_move_c>();
				}
				else
				{
					Debug.LogWarning("WeaponBonus.Start(): playerGo == null");
				}
			}
		}
		if (!Defs.IsSurvival && !this.isHunger)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("BonusFX"), Vector3.zero, Quaternion.identity) as GameObject;
			gameObject2.transform.parent = base.transform;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.layer = base.gameObject.layer;
			ZombieCreator.SetLayerRecursively(gameObject2, base.gameObject.layer);
		}
	}

	// Token: 0x06004E80 RID: 20096 RVA: 0x001C7184 File Offset: 0x001C5384
	private void Update()
	{
		if (!this.oldIsMaster && PhotonNetwork.isMasterClient && this.isKilled)
		{
			PhotonNetwork.Destroy(base.gameObject);
			return;
		}
		this.oldIsMaster = PhotonNetwork.isMasterClient;
		float num = 120f;
		base.transform.Rotate(base.transform.InverseTransformDirection(Vector3.up), num * Time.deltaTime);
		if (this.runLoading)
		{
			return;
		}
		if (this.isHunger && (this._player == null || this._playerMoveC == null))
		{
			this._player = this._weaponManager.myPlayer;
			if (!(this._player != null))
			{
				return;
			}
			this._playerMoveC = this._player.GetComponent<SkinName>().playerGameObject.GetComponent<Player_move_c>();
		}
		if (this._playerMoveC == null || this._playerMoveC.isGrenadePress)
		{
			return;
		}
		if (!this.isKilled && !this._playerMoveC.isKilled && Vector3.SqrMagnitude(base.transform.position - this._player.transform.position) < 2.25f)
		{
			this._playerMoveC.AddWeapon(this.weaponPrefab);
			this.isKilled = true;
			if (Defs.IsSurvival || (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None) || this.isHunger)
			{
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
				{
					TrainingController.isNextStep = TrainingState.GetTheGun;
				}
				if (!this.isHunger)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				else
				{
					base.photonView.RPC("DestroyRPC", PhotonTargets.AllBuffered, new object[0]);
				}
				return;
			}
			string[] array = Storager.getString(Defs.WeaponsGotInCampaign, false).Split(new char[]
			{
				'#'
			});
			List<string> list = new List<string>();
			foreach (string item in array)
			{
				list.Add(item);
			}
			if (!list.Contains(LevelBox.weaponsFromBosses[Application.loadedLevelName]))
			{
				list.Add(LevelBox.weaponsFromBosses[Application.loadedLevelName]);
				Storager.setString(Defs.WeaponsGotInCampaign, string.Join("#", list.ToArray()), false);
			}
			UnityEngine.Object.Destroy(base.gameObject);
			this.runLoading = true;
			LevelCompleteLoader.action = null;
			LevelCompleteLoader.sceneName = "LevelComplete";
			AutoFade.LoadLevel("LevelToCompleteProm", 2f, 0f, Color.white);
			if (InGameGUI.sharedInGameGUI != null)
			{
				InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(true);
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Resources.Load("PauseONGuiDrawer") as GameObject);
			gameObject.transform.parent = base.transform;
		}
	}

	// Token: 0x06004E81 RID: 20097 RVA: 0x001C747C File Offset: 0x001C567C
	[RPC]
	[PunRPC]
	public void DestroyRPC()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		else
		{
			base.transform.position = new Vector3(0f, -10000f, 0f);
		}
		if (InGameGUI.sharedInGameGUI != null)
		{
			InGameGUI.sharedInGameGUI.SetEnablePerfectLabel(false);
		}
	}

	// Token: 0x06004E82 RID: 20098 RVA: 0x001C74E0 File Offset: 0x001C56E0
	private void OnDestroy()
	{
		if (Defs.IsSurvival || (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None) || this.isHunger)
		{
			return;
		}
	}

	// Token: 0x04003D0F RID: 15631
	public GameObject weaponPrefab;

	// Token: 0x04003D10 RID: 15632
	private GameObject _player;

	// Token: 0x04003D11 RID: 15633
	private Player_move_c _playerMoveC;

	// Token: 0x04003D12 RID: 15634
	private bool runLoading;

	// Token: 0x04003D13 RID: 15635
	private bool oldIsMaster;

	// Token: 0x04003D14 RID: 15636
	public WeaponManager _weaponManager;

	// Token: 0x04003D15 RID: 15637
	private bool isHunger;

	// Token: 0x04003D16 RID: 15638
	public bool isKilled;
}
