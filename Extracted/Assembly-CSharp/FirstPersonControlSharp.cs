using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x0200081D RID: 2077
public sealed class FirstPersonControlSharp : MonoBehaviour
{
	// Token: 0x17000C6E RID: 3182
	// (get) Token: 0x06004B98 RID: 19352 RVA: 0x001B2C4C File Offset: 0x001B0E4C
	// (set) Token: 0x06004B99 RID: 19353 RVA: 0x001B2C54 File Offset: 0x001B0E54
	private bool jump
	{
		get
		{
			return Defs.isJump;
		}
		set
		{
			Defs.isJump = value;
		}
	}

	// Token: 0x06004B9A RID: 19354 RVA: 0x001B2C5C File Offset: 0x001B0E5C
	private void Awake()
	{
		this.isHunger = Defs.isHunger;
		this.isInet = Defs.isInet;
		this.isMulti = Defs.isMulti;
	}

	// Token: 0x06004B9B RID: 19355 RVA: 0x001B2C80 File Offset: 0x001B0E80
	private void Start()
	{
		this.mySkinName = base.GetComponent<SkinName>();
		if (!this.isInet)
		{
			this.isMine = base.GetComponent<NetworkView>().isMine;
		}
		else
		{
			this.isMine = PhotonView.Get(this).isMine;
		}
		if (this.isHunger)
		{
			this.hungerGameController = HungerGameController.Instance;
		}
		if (!this.isMulti || this.isMine)
		{
			this.HandleInvertCamUpdated();
			PauseNGUIController.InvertCamUpdated += this.HandleInvertCamUpdated;
			this.oldJumpCount = PlayerPrefs.GetInt("NewbieJumperAchievement", 0);
			this.oldNinjaJumpsCount = ((!Storager.hasKey("NinjaJumpsCount")) ? 0 : Storager.getInt("NinjaJumpsCount", false));
		}
		this.thisTransform = base.GetComponent<Transform>();
		this.character = base.GetComponent<CharacterController>();
		this._moveC = this.playerGameObject.GetComponent<Player_move_c>();
	}

	// Token: 0x06004B9C RID: 19356 RVA: 0x001B2D70 File Offset: 0x001B0F70
	private void HandleInvertCamUpdated()
	{
		this._invert = (PlayerPrefs.GetInt(Defs.InvertCamSN, 0) == 1);
	}

	// Token: 0x06004B9D RID: 19357 RVA: 0x001B2D88 File Offset: 0x001B0F88
	private void OnEndGame()
	{
		if (!this.isMulti || this.isMine)
		{
			if (JoystickController.leftJoystick)
			{
				JoystickController.leftJoystick.transform.parent.gameObject.SetActive(false);
			}
			if (JoystickController.rightJoystick)
			{
				JoystickController.rightJoystick.gameObject.SetActive(false);
			}
		}
		base.enabled = false;
	}

	// Token: 0x06004B9E RID: 19358 RVA: 0x001B2DFC File Offset: 0x001B0FFC
	[RPC]
	[PunRPC]
	private void setIp(string _ip)
	{
		this.myIp = _ip;
	}

	// Token: 0x06004B9F RID: 19359 RVA: 0x001B2E08 File Offset: 0x001B1008
	private Vector2 updateKeyboardControls()
	{
		int num = 0;
		int num2 = 0;
		if (Input.GetKey("w"))
		{
			num = 1;
		}
		if (Input.GetKey("s"))
		{
			num = -1;
		}
		if (Input.GetKey("a"))
		{
			num2 = -1;
		}
		if (Input.GetKey("d"))
		{
			num2 = 1;
		}
		Vector2 result = new Vector2((float)num2, (float)num);
		return result;
	}

	// Token: 0x06004BA0 RID: 19360 RVA: 0x001B2E6C File Offset: 0x001B106C
	private void Jump()
	{
		if (!TrainingController.TrainingCompleted)
		{
			TrainingController.timeShowJump = 1000f;
			HintController.instance.HideHintByName("press_jump");
		}
		this.jump = true;
		this.canJump = false;
		if (!Defs.isJetpackEnabled && (!WeaponManager.sharedManager.myPlayerMoveC.isMechActive || !WeaponManager.sharedManager.myPlayerMoveC.IsGadgetEffectActive(Player_move_c.GadgetEffect.demon)))
		{
			QuestMediator.NotifyJump();
			this.mySkinName.sendAnimJump();
		}
		if (TrainingController.TrainingCompleted && (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android || BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer) && Social.localUser.authenticated)
		{
			int num = this.oldJumpCount + 1;
			if (this.oldJumpCount < 10)
			{
				this.oldJumpCount = num;
				if (num == 10)
				{
					PlayerPrefs.SetInt("NewbieJumperAchievement", num);
					float newProgress = 100f;
					string text = (BuildSettings.BuildTargetPlatform != RuntimePlatform.IPhonePlayer && Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon) ? "CgkIr8rGkPIJEAIQAQ" : "Jumper_id";
					if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
					{
						AGSAchievementsClient.UpdateAchievementProgress(text, newProgress, 0);
					}
					else
					{
						Social.ReportProgress(text, (double)newProgress, delegate(bool success)
						{
							string text2 = string.Format("Newbie Jumper achievement progress {0:0.0}%: {1}", newProgress, success);
						});
					}
				}
			}
		}
	}

	// Token: 0x06004BA1 RID: 19361 RVA: 0x001B2FC4 File Offset: 0x001B11C4
	private void Update()
	{
		if (this.isMulti && !this.isMine)
		{
			return;
		}
		if (this.mySkinName.playerMoveC.isKilled)
		{
			return;
		}
		if (JoystickController.leftJoystick == null || JoystickController.rightJoystick == null)
		{
			return;
		}
		if (this.mySkinName.playerMoveC.isRocketJump && this.character.isGrounded)
		{
			this.mySkinName.playerMoveC.isRocketJump = false;
		}
		this._movement = this.thisTransform.TransformDirection(new Vector3(JoystickController.leftJoystick.value.x, 0f, JoystickController.leftJoystick.value.y));
		if ((!this.isHunger || !this.hungerGameController.isGo) && this.isHunger)
		{
			this._movement = Vector3.zero;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining < TrainingState.TapToMove)
		{
			this._movement = Vector3.zero;
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.TapToMove && this._movement != Vector3.zero)
		{
			TrainingController.isNextStep = TrainingState.TapToMove;
		}
		this._movement.y = 0f;
		this._movement.Normalize();
		Vector2 vector = new Vector2(Mathf.Abs(JoystickController.leftJoystick.value.x), Mathf.Abs(JoystickController.leftJoystick.value.y));
		if (JoystickController.leftTouchPad.isShooting && JoystickController.leftTouchPad.isActiveFireButton)
		{
			vector = new Vector2(0f, 0f);
		}
		if (vector.y > vector.x)
		{
			if (JoystickController.leftJoystick.value.y > 0f)
			{
				this._movement *= this.forwardSpeed * EffectsController.SpeedModifier(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1) * vector.y;
			}
			else
			{
				this._movement *= this.backwardSpeed * EffectsController.SpeedModifier(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1) * vector.y;
			}
		}
		else
		{
			this._movement *= this.sidestepSpeed * EffectsController.SpeedModifier(WeaponManager.sharedManager.currentWeaponSounds.categoryNabor - 1) * vector.x * (float)((!this.character.isGrounded) ? 1 : 1);
		}
		bool flag = Defs.isJetpackEnabled || (WeaponManager.sharedManager.myPlayerMoveC.isMechActive && WeaponManager.sharedManager.myPlayerMoveC.IsGadgetEffectActive(Player_move_c.GadgetEffect.demon));
		if (this.character.isGrounded)
		{
			if (EffectsController.NinjaJumpEnabled)
			{
				this.ninjaJumpUsed = false;
			}
			this.canJump = true;
			this.jump = false;
			TouchPadController rightJoystick = JoystickController.rightJoystick;
			if (this.canJump && (rightJoystick.jumpPressed || JoystickController.leftTouchPad.isJumpPressed))
			{
				if (!flag)
				{
					rightJoystick.jumpPressed = false;
				}
				this.Jump();
			}
			if (this.jump)
			{
				this.secondJumpEnabled = false;
				if (!JoystickController.leftTouchPad.isJumpPressed)
				{
					base.StartCoroutine(this.EnableSecondJump());
				}
				else
				{
					this.jumpBy3dTouch = true;
				}
				this.velocity = Vector3.zero;
				this.velocity.y = this.jumpSpeed * EffectsController.JumpModifier;
			}
		}
		else
		{
			if (!JoystickController.leftTouchPad.isJumpPressed && this.jumpBy3dTouch)
			{
				this.secondJumpEnabled = true;
				this.jumpBy3dTouch = false;
			}
			if (this.jump && this.mySkinName.interpolateScript.myAnim == 0 && !flag)
			{
				this.mySkinName.sendAnimJump();
			}
			TouchPadController rightJoystick2 = JoystickController.rightJoystick;
			TouchPadInJoystick leftTouchPad = JoystickController.leftTouchPad;
			if ((rightJoystick2.jumpPressed || leftTouchPad.isJumpPressed) && ((EffectsController.NinjaJumpEnabled && !this.ninjaJumpUsed && this.secondJumpEnabled) || flag))
			{
				if (!flag)
				{
					QuestMediator.NotifyJump();
					this.RegisterNinjAchievment();
				}
				this.ninjaJumpUsed = true;
				this.canJump = false;
				if (!flag)
				{
					this.mySkinName.sendAnimJump();
				}
				this.velocity.y = 1.1f * (this.jumpSpeed * EffectsController.JumpModifier);
			}
			if (!flag)
			{
				rightJoystick2.jumpPressed = false;
			}
			this.velocity.y = this.velocity.y + Physics.gravity.y * this.gravityMultiplier * Time.deltaTime;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC.IsPlayerEffectActive(Player_move_c.PlayerEffect.rocketFly))
		{
			this.velocity.y = 25f;
		}
		this._movement += this.velocity;
		this._movement += Physics.gravity * this.gravityMultiplier;
		if (Defs.isMulti && !Defs.isCOOP && !WeaponManager.sharedManager.myPlayerMoveC.isImmortality)
		{
			Vector3 vector2 = Vector3.zero;
			bool flag2 = false;
			for (int i = 0; i < Initializer.singularities.Count; i++)
			{
				if (!Initializer.singularities[i].owner.Equals(WeaponManager.sharedManager.myPlayerMoveC) && (!ConnectSceneNGUIController.isTeamRegim || WeaponManager.sharedManager.myPlayerMoveC.myCommand != Initializer.singularities[i].owner.myCommand))
				{
					SingularityHole singularityHole = Initializer.singularities[i];
					Vector3 vector3 = singularityHole.transform.position - base.transform.position;
					float force = singularityHole.GetForce(vector3.sqrMagnitude);
					if (force > 0f)
					{
						vector2 += vector3.normalized * force;
					}
					if (force < 0f)
					{
						flag2 = true;
					}
				}
			}
			if (vector2.sqrMagnitude >= 0f)
			{
				if (vector2.y > 0f || flag2)
				{
					this._movement.y = 0f;
					this.velocity.y = 0f;
				}
				this._movement += vector2;
			}
			for (int j = 0; j < Initializer.players.Count; j++)
			{
				if (Initializer.players[j].IsPlayerEffectActive(Player_move_c.PlayerEffect.attrackPlayer))
				{
					Vector3 normalized = (Initializer.players[j].myPlayerTransform.position + Initializer.players[j].myPlayerTransform.forward * 1.2f - base.transform.position).normalized;
					this._movement += normalized * 5f;
				}
			}
		}
		this._movement *= Time.deltaTime;
		this.timeUpdateAnim -= Time.deltaTime;
		if (this.timeUpdateAnim < 0f)
		{
			if (this.character.isGrounded)
			{
				this.timeUpdateAnim = 0.5f;
				Vector2 vector4 = new Vector2(this._movement.x, this._movement.z);
				if (vector4.sqrMagnitude > 0f)
				{
					this._moveC.WalkAnimation();
				}
				else
				{
					this._moveC.IdleAnimation();
				}
			}
			else
			{
				this._moveC.WalkAnimation();
			}
		}
		this.Update2();
	}

	// Token: 0x06004BA2 RID: 19362 RVA: 0x001B37E8 File Offset: 0x001B19E8
	private void Update2()
	{
		if (!this.character.enabled)
		{
			return;
		}
		if (!this.mySkinName.onRink)
		{
			if (this.mySkinName.onConveyor)
			{
				this._movement += this.mySkinName.conveyorDirection * Time.deltaTime;
			}
			this.character.Move(this._movement);
			this._movement = Vector3.zero;
			this.steptRink = false;
		}
		else
		{
			if (!this.steptRink)
			{
				this.rinkMovement = this._movement;
				this.steptRink = true;
			}
			this.rinkMovement = Vector3.MoveTowards(this.rinkMovement, this._movement, 0.068f * Time.deltaTime);
			this.rinkMovement.y = this._movement.y;
			this.character.Move(this.rinkMovement);
		}
		if (this.character.isGrounded)
		{
			this.velocity = Vector3.zero;
		}
		else
		{
			if (this.mySkinName.onRink)
			{
				this.rinkMovement = this._movement;
			}
			this.mySkinName.onConveyor = false;
		}
		Vector2 delta = this.GrabCameraInputDelta();
		if (Device.isPixelGunLow && Defs.isTouchControlSmoothDump)
		{
			this.MoveCamera(delta);
		}
		if (Defs.isMulti && CameraSceneController.sharedController.killCamController.enabled)
		{
			CameraSceneController.sharedController.killCamController.UpdateMouseX();
		}
	}

	// Token: 0x06004BA3 RID: 19363 RVA: 0x001B3974 File Offset: 0x001B1B74
	public void MoveCamera(Vector2 delta)
	{
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None && TrainingController.stepTraining == TrainingState.SwipeToRotate && delta != Vector2.zero)
		{
			TrainingController.isNextStep = TrainingState.SwipeToRotate;
		}
		float sensitivity = Defs.Sensitivity;
		float num = 1f;
		if (this._moveC != null)
		{
			num *= ((!this._moveC.isZooming) ? 1f : 0.2f);
		}
		if (JoystickController.rightJoystick != null)
		{
			JoystickController.rightJoystick.ApplyDeltaTo(delta, this.thisTransform, this.cameraPivot.transform, sensitivity * num, this._invert);
		}
	}

	// Token: 0x06004BA4 RID: 19364 RVA: 0x001B3A2C File Offset: 0x001B1C2C
	private Vector2 GrabCameraInputDelta()
	{
		Vector2 result = Vector2.zero;
		TouchPadController rightJoystick = JoystickController.rightJoystick;
		if (rightJoystick != null)
		{
			result = rightJoystick.GrabDeltaPosition();
		}
		return result;
	}

	// Token: 0x06004BA5 RID: 19365 RVA: 0x001B3A5C File Offset: 0x001B1C5C
	private void RegisterNinjAchievment()
	{
		if (Social.localUser.authenticated)
		{
			int num = this.oldNinjaJumpsCount + 1;
			if (this.oldNinjaJumpsCount < 50)
			{
				Storager.setInt("NinjaJumpsCount", num, false);
			}
			this.oldNinjaJumpsCount = num;
			if (!Storager.hasKey("ParkourNinjaAchievementCompleted") && num >= 50)
			{
				if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					GpgFacade.Instance.IncrementAchievement("CgkIr8rGkPIJEAIQAw", 1, delegate(bool success)
					{
						Debug.Log("Achievement Parkour Ninja incremented: " + success);
					});
				}
				Storager.setInt("ParkourNinjaAchievementCompleted", 1, false);
			}
		}
	}

	// Token: 0x06004BA6 RID: 19366 RVA: 0x001B3B08 File Offset: 0x001B1D08
	private IEnumerator EnableSecondJump()
	{
		yield return new WaitForSeconds(0.25f);
		this.secondJumpEnabled = true;
		yield break;
	}

	// Token: 0x06004BA7 RID: 19367 RVA: 0x001B3B24 File Offset: 0x001B1D24
	private void OnDestroy()
	{
		if (!this.isMulti || this.isMine)
		{
			PauseNGUIController.InvertCamUpdated -= this.HandleInvertCamUpdated;
		}
	}

	// Token: 0x04003AA4 RID: 15012
	private const string newbieJumperAchievement = "NewbieJumperAchievement";

	// Token: 0x04003AA5 RID: 15013
	private const int maxJumpCount = 10;

	// Token: 0x04003AA6 RID: 15014
	private const string keyNinja = "NinjaJumpsCount";

	// Token: 0x04003AA7 RID: 15015
	public Transform cameraPivot;

	// Token: 0x04003AA8 RID: 15016
	public float forwardSpeed = 4f;

	// Token: 0x04003AA9 RID: 15017
	public float backwardSpeed = 1f;

	// Token: 0x04003AAA RID: 15018
	public float sidestepSpeed = 1f;

	// Token: 0x04003AAB RID: 15019
	public float jumpSpeed = 4.5f;

	// Token: 0x04003AAC RID: 15020
	public float inAirMultiplier = 0.25f;

	// Token: 0x04003AAD RID: 15021
	public Vector2 rotationSpeed = new Vector2(2f, 1f);

	// Token: 0x04003AAE RID: 15022
	public float tiltPositiveYAxis = 0.6f;

	// Token: 0x04003AAF RID: 15023
	public float tiltNegativeYAxis = 0.4f;

	// Token: 0x04003AB0 RID: 15024
	public float tiltXAxisMinimum = 0.1f;

	// Token: 0x04003AB1 RID: 15025
	public string myIp;

	// Token: 0x04003AB2 RID: 15026
	public GameObject playerGameObject;

	// Token: 0x04003AB3 RID: 15027
	public int typeAnim;

	// Token: 0x04003AB4 RID: 15028
	private Transform thisTransform;

	// Token: 0x04003AB5 RID: 15029
	public GameObject camPlayer;

	// Token: 0x04003AB6 RID: 15030
	public CharacterController character;

	// Token: 0x04003AB7 RID: 15031
	private Vector3 cameraVelocity;

	// Token: 0x04003AB8 RID: 15032
	private Vector3 velocity;

	// Token: 0x04003AB9 RID: 15033
	private bool canJump = true;

	// Token: 0x04003ABA RID: 15034
	public bool isMine;

	// Token: 0x04003ABB RID: 15035
	private Rect fireZone;

	// Token: 0x04003ABC RID: 15036
	private Rect jumpZone;

	// Token: 0x04003ABD RID: 15037
	private float timeUpdateAnim;

	// Token: 0x04003ABE RID: 15038
	public AudioClip jumpClip;

	// Token: 0x04003ABF RID: 15039
	private Player_move_c _moveC;

	// Token: 0x04003AC0 RID: 15040
	public float gravityMultiplier = 1f;

	// Token: 0x04003AC1 RID: 15041
	private Vector3 mousePosOld = Vector3.zero;

	// Token: 0x04003AC2 RID: 15042
	private bool _invert;

	// Token: 0x04003AC3 RID: 15043
	public bool ninjaJumpUsed = true;

	// Token: 0x04003AC4 RID: 15044
	private HungerGameController hungerGameController;

	// Token: 0x04003AC5 RID: 15045
	private bool isHunger;

	// Token: 0x04003AC6 RID: 15046
	private bool isInet;

	// Token: 0x04003AC7 RID: 15047
	private bool isMulti;

	// Token: 0x04003AC8 RID: 15048
	private SkinName mySkinName;

	// Token: 0x04003AC9 RID: 15049
	private int oldJumpCount;

	// Token: 0x04003ACA RID: 15050
	private int oldNinjaJumpsCount;

	// Token: 0x04003ACB RID: 15051
	private Vector3 _movement;

	// Token: 0x04003ACC RID: 15052
	private Vector2 _cameraMouseDelta;

	// Token: 0x04003ACD RID: 15053
	private bool jumpBy3dTouch;

	// Token: 0x04003ACE RID: 15054
	private Vector3 rinkMovement;

	// Token: 0x04003ACF RID: 15055
	private bool steptRink;

	// Token: 0x04003AD0 RID: 15056
	private bool secondJumpEnabled = true;
}
