using System;
using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006EF RID: 1775
	public class PetClickHandler : MonoBehaviour
	{
		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x00140A48 File Offset: 0x0013EC48
		private static bool CanClickToPet
		{
			get
			{
				return (FeedbackMenuController.Instance == null || !FeedbackMenuController.Instance.gameObject.activeSelf) && (PauseGUIController.Instance == null || !PauseGUIController.Instance.IsPaused) && (BankController.Instance == null || BankController.Instance.uiRoot == null || !BankController.Instance.uiRoot.gameObject.activeInHierarchy) && (NewsLobbyController.sharedController == null || !NewsLobbyController.sharedController.isActiveAndEnabled) && (LeaderboardScript.Instance == null || !LeaderboardScript.Instance.UIEnabled) && (FriendsWindowGUI.Instance == null || !FriendsWindowGUI.Instance.cameraObject.activeInHierarchy) && !Nest.Instance.BannerIsVisible && (FriendsController.sharedController == null || !FriendsController.sharedController.ProfileInterfaceActive) && (BannerWindowController.SharedController == null || !BannerWindowController.SharedController.IsAnyBannerShown) && (FreeAwardController.Instance == null || FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>()) && (MainMenuController.sharedController == null || !MainMenuController.sharedController.InAdventureScreen) && (MainMenuController.sharedController == null || (!MainMenuController.sharedController.SettingsJoysticksPanel.activeInHierarchy && !MainMenuController.sharedController.settingsPanel.activeInHierarchy));
			}
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x00140C04 File Offset: 0x0013EE04
		private void OnDisable()
		{
			this._mouseDownPos = null;
			this._clickAnimIsPlaying = false;
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x00140C28 File Offset: 0x0013EE28
		private void Start()
		{
			this._myPetEngine = this.GetPetEngine(base.gameObject);
			if (this._myPetEngine == null)
			{
				Debug.LogError("PetEngine not found");
			}
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00140C58 File Offset: 0x0013EE58
		private void Update()
		{
			if (this._myPetEngine == null)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0))
			{
				this._mouseDownPos = new Vector3?(Input.mousePosition);
				this._clickStartCamera = this.GetCurrentCamera();
				return;
			}
			if (Input.GetMouseButtonUp(0) && this._mouseDownPos != null)
			{
				if (this._clickStartCamera != this.GetCurrentCamera())
				{
					return;
				}
				if (Vector3.Distance(Input.mousePosition, this._mouseDownPos.Value) <= this._threshold && PetClickHandler.CanClickToPet)
				{
					if (ShopNGUIController.GuiActive && this._myPetEngine != ShopNGUIController.sharedShop.ShopCharacterInterface.myPetEngine)
					{
						return;
					}
					GameObject gameObject = null;
					if (this.CheckTouchToPet(out gameObject) && gameObject.Equals(this._myPetEngine.gameObject))
					{
						this.PlayClickAnimation();
					}
				}
			}
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x00140D50 File Offset: 0x0013EF50
		private bool CheckTouchToPet(out GameObject touchedGo)
		{
			touchedGo = null;
			Camera currentCamera = this.GetCurrentCamera();
			if (currentCamera == null)
			{
				return false;
			}
			RaycastHit raycastHit;
			bool flag = Physics.Raycast(currentCamera.ScreenPointToRay(Input.mousePosition), out raycastHit);
			if (flag && (raycastHit.transform.gameObject.CompareTag("Pet") || raycastHit.transform.gameObject.IsSubobjectOf(this._myPetEngine.gameObject)))
			{
				touchedGo = this._myPetEngine.gameObject;
				return true;
			}
			return false;
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x00140DDC File Offset: 0x0013EFDC
		private Camera GetCurrentCamera()
		{
			Camera result = null;
			if (ShopNGUIController.GuiActive)
			{
				result = ShopNGUIController.sharedShop.Camera3D;
			}
			else if (ProfileController.Instance != null && ProfileController.Instance.InterfaceEnabled)
			{
				result = ProfileController.Instance.Camera3D;
			}
			else if (MainMenuController.sharedController != null)
			{
				result = MainMenuController.sharedController.Camera3D;
			}
			return result;
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x00140E50 File Offset: 0x0013F050
		private void PlayClickAnimation()
		{
			if (this._clickAnimIsPlaying)
			{
				return;
			}
			if (!this._myPetEngine.AnimationHandler.AnimationIsExists("Tap"))
			{
				return;
			}
			this._clickAnimIsPlaying = true;
			this._myPetEngine.AnimationHandler.SubscribeTo("Tap", AnimationHandler.AnimState.Finished, true, delegate
			{
				this._clickAnimIsPlaying = false;
				PetAnimation animation = this._myPetEngine.GetAnimation(PetAnimationType.Profile);
				if (animation != null)
				{
					this._myPetEngine.Animator.Play(animation.AnimationName);
				}
			});
			this._myPetEngine.Animator.Play("Tap");
			if (Defs.isSoundFX && this._myPetEngine.ClipTap != null)
			{
				this._myPetEngine.AudioSourceOne.spatialBlend = 0f;
				this._myPetEngine.AudioSourceOne.clip = this._myPetEngine.ClipTap;
				this._myPetEngine.AudioSourceOne.Play();
			}
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x00140F24 File Offset: 0x0013F124
		private PetEngine GetPetEngine(GameObject petGo)
		{
			if (petGo == null)
			{
				return null;
			}
			PetEngine petEngine = petGo.GetComponent<PetEngine>();
			if (petEngine != null)
			{
				return petEngine;
			}
			petEngine = petGo.GetComponentInParents<PetEngine>();
			if (petEngine != null)
			{
				return petEngine;
			}
			GameObject gameObject = petGo.Ancestors().LastOrDefault<GameObject>();
			if (gameObject != null)
			{
				GameObject[] array = gameObject.Descendants().ToArray<GameObject>();
				foreach (GameObject gameObject2 in array)
				{
					if (gameObject2.GetComponent<PetEngine>() != null)
					{
						return gameObject2.GetComponent<PetEngine>();
					}
				}
			}
			return null;
		}

		// Token: 0x04002D87 RID: 11655
		public const string TAP_ANIMATION_NAME = "Tap";

		// Token: 0x04002D88 RID: 11656
		[SerializeField]
		private float _threshold = 10f;

		// Token: 0x04002D89 RID: 11657
		private Vector3? _mouseDownPos;

		// Token: 0x04002D8A RID: 11658
		private PetEngine _myPetEngine;

		// Token: 0x04002D8B RID: 11659
		private Camera _clickStartCamera;

		// Token: 0x04002D8C RID: 11660
		private bool _clickAnimIsPlaying;
	}
}
