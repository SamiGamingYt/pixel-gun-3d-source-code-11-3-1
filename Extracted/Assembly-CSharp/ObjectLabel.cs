using System;
using UnityEngine;

// Token: 0x020006CC RID: 1740
[RequireComponent(typeof(GUIText))]
public class ObjectLabel : MonoBehaviour
{
	// Token: 0x06003C90 RID: 15504 RVA: 0x0013A7A4 File Offset: 0x001389A4
	private void Start()
	{
		this._weaponManager = WeaponManager.sharedManager;
		this.isHunger = Defs.isHunger;
		if (this.isHunger && GameObject.FindGameObjectWithTag("HungerGameController") != null)
		{
			this.hungerGameController = GameObject.FindGameObjectWithTag("HungerGameController").GetComponent<HungerGameController>();
		}
		this.expController = GameObject.FindGameObjectWithTag("ExperienceController").GetComponent<ExperienceController>();
		float num = 36f * Defs.Coef;
		this.thisTransform = base.transform;
		this.cam = ObjectLabel.currentCamera;
		this.camTransform = this.cam.transform;
		base.transform.GetComponent<GUITexture>().pixelInset = new Rect(-75f * this.koofScreen, -3f * this.koofScreen, 30f * this.koofScreen, 30f * this.koofScreen);
		base.transform.GetComponent<GUIText>().pixelOffset = new Vector2(-45f * this.koofScreen, 0f);
		base.transform.GetComponent<GUIText>().fontSize = Mathf.RoundToInt(20f * this.koofScreen);
		this.isCompany = Defs.isCompany;
		this.clanTexture.pixelInset = new Rect(-64f * this.koofScreen, -18f * this.koofScreen, 15f * this.koofScreen, 15f * this.koofScreen);
		this.clanName.pixelOffset = new Vector2(-41f * this.koofScreen, -4f);
		this.clanName.fontSize = Mathf.RoundToInt(16f * this.koofScreen);
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x0013A95C File Offset: 0x00138B5C
	public void ResetTimeShow()
	{
		this.timeShow = 1f;
	}

	// Token: 0x06003C92 RID: 15506 RVA: 0x0013A96C File Offset: 0x00138B6C
	private void Update()
	{
		if (this.timeShow > 0f)
		{
			this.timeShow -= Time.deltaTime;
		}
		if (this.target == null || this.cam == null)
		{
			Debug.Log("target == null || cam == null");
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if ((this.isHunger && this.hungerGameController != null && !this.hungerGameController.isGo) || this._weaponManager.myPlayer == null)
		{
			this.ResetTimeShow();
		}
		if ((ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.TeamFight || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.FlagCapture || ConnectSceneNGUIController.regim == ConnectSceneNGUIController.RegimGame.CapturePoints) && this._weaponManager.myPlayer != null && this._weaponManager.myPlayerMoveC != null && this.target.GetComponent<Player_move_c>() != null && this._weaponManager.myPlayerMoveC.myCommand == this.target.GetComponent<Player_move_c>().myCommand)
		{
			this.ResetTimeShow();
		}
		try
		{
			this.cam = ObjectLabel.currentCamera;
			this.camTransform = this.cam.transform;
			GUITexture component = base.transform.GetComponent<GUITexture>();
			if (component == null)
			{
				Debug.LogError("guiTexture == null");
			}
			else
			{
				if (!this.isMenu)
				{
					Player_move_c component2 = this.target.GetComponent<Player_move_c>();
					if (!this.isSetColor)
					{
						if (component2.myCommand == 1)
						{
							base.gameObject.GetComponent<GUIText>().color = Color.blue;
							this.isSetColor = true;
						}
						if (component2.myCommand == 2)
						{
							base.gameObject.GetComponent<GUIText>().color = Color.red;
							this.isSetColor = true;
						}
					}
					int myRanks = component2.myTable.GetComponent<NetworkStartTable>().myRanks;
					if (myRanks < 0 || myRanks >= this.expController.marks.Length)
					{
						string message = string.Format("Rank is equal to {0}, but the range [0, {1}) expected.", myRanks, this.expController.marks.Length);
						Debug.LogError(message);
					}
					else
					{
						component.texture = this.expController.marks[myRanks];
					}
					this.clanTexture.texture = component2.myTable.GetComponent<NetworkStartTable>().myClanTexture;
					this.clanName.text = component2.myTable.GetComponent<NetworkStartTable>().myClanName;
				}
				else
				{
					component.pixelInset = new Rect(-130f * this.koofScreen, -6f * this.koofScreen, 36f * this.koofScreen, 36f * this.koofScreen);
					base.transform.GetComponent<GUIText>().pixelOffset = new Vector2(-85f * this.koofScreen, 0f);
					base.transform.GetComponent<GUIText>().fontSize = Mathf.RoundToInt(20f * Defs.Coef);
					this.offset = new Vector3(0f, 2.25f, 0f);
					component.texture = this.expController.marks[this.expController.currentLevel];
					this.clanTexture.pixelInset = new Rect(-110f * this.koofScreen, -18f * this.koofScreen, 15f * this.koofScreen, 15f * this.koofScreen);
					this.clanName.pixelOffset = new Vector2(-85f * this.koofScreen, -2f);
					this.clanName.fontSize = Mathf.RoundToInt(16f * this.koofScreen);
					if (this.clanTexture.texture == null)
					{
						if (!string.IsNullOrEmpty(FriendsController.sharedController.clanLogo))
						{
							byte[] data = Convert.FromBase64String(FriendsController.sharedController.clanLogo);
							Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoWidth);
							texture2D.LoadImage(data);
							texture2D.filterMode = FilterMode.Point;
							texture2D.Apply();
							this.clanTexture.texture = texture2D;
						}
						else
						{
							this.clanTexture.texture = null;
						}
						this.clanName.text = FriendsController.sharedController.clanName;
					}
				}
				if (this.timeShow > 0f)
				{
					this.posLabel = this.cam.WorldToViewportPoint(this.target.position + this.offset);
				}
				if (this.timeShow > 0f && this.posLabel.z >= 0f)
				{
					this.thisTransform.position = this.posLabel;
				}
				else
				{
					this.thisTransform.position = new Vector3(-1000f, -1000f, -1000f);
				}
				if (!this.isMenu && this.target.transform.parent.transform.position.y < -1000f)
				{
					this.thisTransform.position = new Vector3(-1000f, -1000f, -1000f);
				}
			}
		}
		catch (Exception arg)
		{
			Debug.Log("Exception in ObjectLabel: " + arg);
		}
	}

	// Token: 0x04002CB5 RID: 11445
	public static Camera currentCamera;

	// Token: 0x04002CB6 RID: 11446
	public Transform target;

	// Token: 0x04002CB7 RID: 11447
	public Vector3 offset = Vector3.up;

	// Token: 0x04002CB8 RID: 11448
	public bool clampToScreen;

	// Token: 0x04002CB9 RID: 11449
	public float clampBorderSize = 0.05f;

	// Token: 0x04002CBA RID: 11450
	public bool useMainCamera = true;

	// Token: 0x04002CBB RID: 11451
	public Camera cameraToUse;

	// Token: 0x04002CBC RID: 11452
	public Camera cam;

	// Token: 0x04002CBD RID: 11453
	public float timeShow;

	// Token: 0x04002CBE RID: 11454
	public Vector3 posLabel;

	// Token: 0x04002CBF RID: 11455
	public bool isShow;

	// Token: 0x04002CC0 RID: 11456
	public bool isMenu;

	// Token: 0x04002CC1 RID: 11457
	public bool isShadow;

	// Token: 0x04002CC2 RID: 11458
	private Transform thisTransform;

	// Token: 0x04002CC3 RID: 11459
	private Transform camTransform;

	// Token: 0x04002CC4 RID: 11460
	private ExperienceController expController;

	// Token: 0x04002CC5 RID: 11461
	private bool isSetColor;

	// Token: 0x04002CC6 RID: 11462
	public WeaponManager _weaponManager;

	// Token: 0x04002CC7 RID: 11463
	private int rank = 1;

	// Token: 0x04002CC8 RID: 11464
	private float koofScreen = (float)Screen.height / 768f;

	// Token: 0x04002CC9 RID: 11465
	private HungerGameController hungerGameController;

	// Token: 0x04002CCA RID: 11466
	private bool isHunger;

	// Token: 0x04002CCB RID: 11467
	private bool isCompany;

	// Token: 0x04002CCC RID: 11468
	public GUITexture clanTexture;

	// Token: 0x04002CCD RID: 11469
	public GUIText clanName;
}
