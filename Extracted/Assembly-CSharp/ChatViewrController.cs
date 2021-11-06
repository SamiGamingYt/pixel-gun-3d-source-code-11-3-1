using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000063 RID: 99
public sealed class ChatViewrController : MonoBehaviour
{
	// Token: 0x0600029A RID: 666 RVA: 0x00016ECC File Offset: 0x000150CC
	private void Awake()
	{
		this.isBuySmile = StickersController.IsBuyAnyPack();
		this.buySmileBannerPrefab.SetActive(false);
		this.hideSmileButton.SetActive(false);
		this.sendMessageInput.gameObject.SetActive(false);
		this.sendMessageInput = this.sendMessageInputDater;
		this.fastCommands.SetActive(false);
		if (this.isBuySmile)
		{
			this.showSmileButton.SetActive(true);
			this.buySmileButton.SetActive(false);
		}
		else
		{
			this.showSmileButton.SetActive(false);
			this.buySmileButton.SetActive(true);
		}
		if (this.sendMessageInput != null)
		{
			MyUIInput myUIInput = this.sendMessageInput;
			myUIInput.onKeyboardInter = (Action)Delegate.Combine(myUIInput.onKeyboardInter, new Action(this.SendMessageFromInput));
			MyUIInput myUIInput2 = this.sendMessageInput;
			myUIInput2.onKeyboardCancel = (Action)Delegate.Combine(myUIInput2.onKeyboardCancel, new Action(this.CancelSendPrivateMessage));
			MyUIInput myUIInput3 = this.sendMessageInput;
			myUIInput3.onKeyboardVisible = (Action)Delegate.Combine(myUIInput3.onKeyboardVisible, new Action(this.OnKeyboardVisible));
			MyUIInput myUIInput4 = this.sendMessageInput;
			myUIInput4.onKeyboardHide = (Action)Delegate.Combine(myUIInput4.onKeyboardHide, new Action(this.OnKeyboardHide));
		}
	}

	// Token: 0x0600029B RID: 667 RVA: 0x00017014 File Offset: 0x00015214
	private void Start()
	{
		ChatViewrController.sharedController = this;
		if (this.sendMessageInput != null)
		{
			this.sendMessageInput.isSelected = true;
		}
	}

	// Token: 0x0600029C RID: 668 RVA: 0x0001703C File Offset: 0x0001523C
	private void OnEnable()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.messageDelegate += this.UpdateMessages;
		}
		this.UpdateMessages();
	}

	// Token: 0x0600029D RID: 669 RVA: 0x00017080 File Offset: 0x00015280
	private void OnDisable()
	{
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.messageDelegate -= this.UpdateMessages;
		}
	}

	// Token: 0x0600029E RID: 670 RVA: 0x000170C0 File Offset: 0x000152C0
	private void UpdateMessages()
	{
		if (WeaponManager.sharedManager.myPlayer == null)
		{
			return;
		}
		Player_move_c myPlayerMoveC = WeaponManager.sharedManager.myPlayerMoveC;
		while (this.labelChat.Count < myPlayerMoveC.messages.Count)
		{
			GameObject gameObject = NGUITools.AddChild(this.labelTable.gameObject, this.chatLabelPrefab);
			this.labelChat.Add(gameObject.GetComponent<ChatLabel>());
		}
		for (int i = 0; i < this.labelChat.Count; i++)
		{
			string str = "[00FF26]";
			if ((!Defs.isInet && myPlayerMoveC.messages[i].IDLocal == WeaponManager.sharedManager.myPlayer.GetComponent<NetworkView>().viewID) || (Defs.isInet && myPlayerMoveC.messages[i].ID == WeaponManager.sharedManager.myPlayer.GetComponent<PhotonView>().viewID))
			{
				str = "[00FF26]";
			}
			else
			{
				if (myPlayerMoveC.messages[i].command == 0)
				{
					str = "[FFFF26]";
				}
				if (myPlayerMoveC.messages[i].command == 1)
				{
					str = "[0000FF]";
				}
				if (myPlayerMoveC.messages[i].command == 2)
				{
					str = "[FF0000]";
				}
			}
			ChatLabel chatLabel = this.labelChat[this.labelChat.Count - i - 1];
			chatLabel.nickLabel.text = str + myPlayerMoveC.messages[i].text;
			if (string.IsNullOrEmpty(myPlayerMoveC.messages[i].iconName))
			{
				if (chatLabel.stickerObject.activeInHierarchy)
				{
					chatLabel.stickerObject.SetActive(false);
				}
			}
			else
			{
				if (!chatLabel.stickerObject.activeInHierarchy)
				{
					chatLabel.stickerObject.SetActive(true);
				}
				chatLabel.iconSprite.spriteName = myPlayerMoveC.messages[i].iconName;
			}
			Transform transform = chatLabel.iconSprite.transform;
			transform.localPosition = new Vector3((float)(chatLabel.nickLabel.width + 20), transform.localPosition.y, transform.localPosition.z);
			chatLabel.clanTexture.mainTexture = myPlayerMoveC.messages[i].clanLogo;
			this.labelChat[i].gameObject.SetActive(true);
		}
		this.labelTable.Reposition();
	}

	// Token: 0x0600029F RID: 671 RVA: 0x0001738C File Offset: 0x0001558C
	private void Update()
	{
		if (this.isShowSmilePanel && this.smilePanelTransform.localPosition.y < -150f)
		{
			this.smilesPanel.SetActive(false);
			this.smilesPanel.SetActive(true);
			this.smilePanelTransform.localPosition = new Vector3(this.smilePanelTransform.localPosition.x, this.smilePanelTransform.localPosition.y + Time.deltaTime * 500f, this.smilePanelTransform.localPosition.z);
			this.scrollLabels.MoveRelative(Vector3.up * Time.deltaTime * 500f);
			if (this.smilePanelTransform.localPosition.y > -150f)
			{
				this.smilePanelTransform.localPosition = new Vector3(this.smilePanelTransform.localPosition.x, -150f, this.smilePanelTransform.localPosition.z);
				this.scrollLabels.ResetPosition();
				this.smilePanelTransform.gameObject.SetActive(false);
				this.smilePanelTransform.gameObject.SetActive(true);
			}
		}
		if (!this.isShowSmilePanel && this.smilePanelTransform.localPosition.y > -314f)
		{
			this.smilePanelTransform.localPosition = new Vector3(this.smilePanelTransform.localPosition.x, this.smilePanelTransform.localPosition.y - Time.deltaTime * 500f, this.smilePanelTransform.localPosition.z);
			this.scrollLabels.MoveRelative(Vector3.down * Time.deltaTime * 500f);
			if (this.smilePanelTransform.localPosition.y < -314f)
			{
				this.smilePanelTransform.localPosition = new Vector3(this.smilePanelTransform.localPosition.x, -314f, this.smilePanelTransform.localPosition.z);
				this.scrollLabels.ResetPosition();
				this.smilePanelTransform.gameObject.SetActive(false);
				this.smilePanelTransform.gameObject.SetActive(true);
			}
		}
		if (this.sendMessageButton.isEnabled == string.IsNullOrEmpty(this.sendMessageInput.value))
		{
			this.sendMessageButton.isEnabled = !string.IsNullOrEmpty(this.sendMessageInput.value);
		}
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00017648 File Offset: 0x00015848
	private void LateUpdate()
	{
		if (this.needReset)
		{
			this.needReset = false;
			this.scrollLabels.ResetPosition();
		}
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00017668 File Offset: 0x00015868
	public void ShowSmilePannelOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.isShowSmilePanel = true;
		this.showSmileButton.SetActive(false);
		this.hideSmileButton.SetActive(true);
		this.scrollLabels.ResetPosition();
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x000176BC File Offset: 0x000158BC
	public void HideSmilePannelOnClick()
	{
		if (ButtonClickSound.Instance != null)
		{
			ButtonClickSound.Instance.PlayClick();
		}
		this.HideSmilePannel();
	}

	// Token: 0x060002A3 RID: 675 RVA: 0x000176EC File Offset: 0x000158EC
	public void OnClickSendMessageFromButton()
	{
		if (!string.IsNullOrEmpty(this.sendMessageInput.value))
		{
			this.PostChat(this.sendMessageInput.value);
			this.sendMessageInput.value = string.Empty;
		}
		if (this.isShowSmilePanel)
		{
			this.HideSmilePannel();
		}
		this.needReset = true;
	}

	// Token: 0x060002A4 RID: 676 RVA: 0x00017748 File Offset: 0x00015948
	public void SendMessageFromInput()
	{
		if (!string.IsNullOrEmpty(this.sendMessageInput.value))
		{
			this.PostChat(this.sendMessageInput.value);
			this.sendMessageInput.value = string.Empty;
		}
		if (this.isShowSmilePanel)
		{
			this.HideSmilePannel();
		}
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x0001779C File Offset: 0x0001599C
	private void HideSmilePannel()
	{
		this.isShowSmilePanel = false;
		this.showSmileButton.SetActive(true);
		this.hideSmileButton.SetActive(false);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x000177C0 File Offset: 0x000159C0
	public void CancelSendPrivateMessage()
	{
		this.sendMessageInput.value = string.Empty;
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x000177D4 File Offset: 0x000159D4
	public void OnKeyboardVisible()
	{
		this.keyboardSize = this.sendMessageInput.heightKeyboard;
		if (Application.isEditor)
		{
			this.keyboardSize = 200f;
		}
		this.bottomAnchor.localPosition = new Vector3(this.bottomAnchor.localPosition.x, this.bottomAnchor.localPosition.y + this.keyboardSize / Defs.Coef, this.bottomAnchor.localPosition.z);
		base.StartCoroutine(this.ResetpositionCoroutine());
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x0001786C File Offset: 0x00015A6C
	public void OnKeyboardHide()
	{
		this.bottomAnchor.localPosition = new Vector3(this.bottomAnchor.localPosition.x, this.bottomAnchor.localPosition.y - this.keyboardSize / Defs.Coef, this.bottomAnchor.localPosition.z);
		base.StartCoroutine(this.ResetpositionCoroutine());
		this.smilePanelTransform.gameObject.SetActive(false);
		this.smilePanelTransform.gameObject.SetActive(true);
	}

	// Token: 0x060002A9 RID: 681 RVA: 0x00017900 File Offset: 0x00015B00
	private IEnumerator ResetpositionCoroutine()
	{
		yield return null;
		this.scrollLabels.ResetPosition();
		this.smilePanelTransform.gameObject.SetActive(false);
		this.smilePanelTransform.gameObject.SetActive(true);
		yield break;
	}

	// Token: 0x060002AA RID: 682 RVA: 0x0001791C File Offset: 0x00015B1C
	public void PostChat(string _text)
	{
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(this.sendChatClip);
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.SendChat(_text, this.isClanMode, string.Empty);
		}
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00017970 File Offset: 0x00015B70
	public void HandleCloseChat()
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.Log("[Close Chat] pressed.");
		}
		this.CloseChat(false);
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00017990 File Offset: 0x00015B90
	public void CloseChat(bool isHardClose = false)
	{
		if (!isHardClose && this.buySmileBannerPrefab.activeSelf)
		{
			Debug.LogFormat("Ignoring CloseChat({0}), buySmiley: {1}", new object[]
			{
				isHardClose,
				this.buySmileBannerPrefab.activeSelf
			});
			return;
		}
		if (WeaponManager.sharedManager.myPlayerMoveC != null)
		{
			WeaponManager.sharedManager.myPlayerMoveC.showChat = false;
			WeaponManager.sharedManager.myPlayerMoveC.AddButtonHandlers();
			WeaponManager.sharedManager.myPlayerMoveC.inGameGUI.gameObject.SetActive(true);
			if (WeaponManager.sharedManager.myPlayerMoveC.isMechActive)
			{
				if (Defs.isDaterRegim)
				{
					WeaponManager.sharedManager.myPlayerMoveC.mechBearPoint.SetActive(true);
				}
				else if (WeaponManager.sharedManager.myPlayerMoveC.currentMech != null)
				{
					WeaponManager.sharedManager.myPlayerMoveC.currentMech.point.SetActive(true);
				}
			}
			else
			{
				WeaponManager.sharedManager.currentWeaponSounds.gameObject.SetActive(true);
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
		ChatViewrController.sharedController = null;
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00017AC8 File Offset: 0x00015CC8
	private void OnDestroy()
	{
		MyUIInput myUIInput = this.sendMessageInput;
		myUIInput.onKeyboardInter = (Action)Delegate.Remove(myUIInput.onKeyboardInter, new Action(this.SendMessageFromInput));
		MyUIInput myUIInput2 = this.sendMessageInput;
		myUIInput2.onKeyboardCancel = (Action)Delegate.Remove(myUIInput2.onKeyboardCancel, new Action(this.CancelSendPrivateMessage));
		MyUIInput myUIInput3 = this.sendMessageInput;
		myUIInput3.onKeyboardVisible = (Action)Delegate.Remove(myUIInput3.onKeyboardVisible, new Action(this.OnKeyboardVisible));
		MyUIInput myUIInput4 = this.sendMessageInput;
		myUIInput4.onKeyboardHide = (Action)Delegate.Remove(myUIInput4.onKeyboardHide, new Action(this.OnKeyboardHide));
		ChatViewrController.sharedController = null;
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00017B78 File Offset: 0x00015D78
	public void BuySmileOnClick()
	{
		this.buySmileBannerPrefab.SetActive(true);
		this.sendMessageInput.isSelected = false;
		this.sendMessageInput.DeselectInput();
	}

	// Token: 0x040002D1 RID: 721
	public static ChatViewrController sharedController;

	// Token: 0x040002D2 RID: 722
	public MyUIInput sendMessageInput;

	// Token: 0x040002D3 RID: 723
	public MyUIInput sendMessageInputDater;

	// Token: 0x040002D4 RID: 724
	public GameObject fastCommands;

	// Token: 0x040002D5 RID: 725
	public Transform chatLabelsHolder;

	// Token: 0x040002D6 RID: 726
	private List<ChatLabel> labelChat = new List<ChatLabel>();

	// Token: 0x040002D7 RID: 727
	public GameObject buySmileBannerPrefab;

	// Token: 0x040002D8 RID: 728
	public Transform smilePanelTransform;

	// Token: 0x040002D9 RID: 729
	public GameObject smilesPanel;

	// Token: 0x040002DA RID: 730
	public GameObject showSmileButton;

	// Token: 0x040002DB RID: 731
	public GameObject hideSmileButton;

	// Token: 0x040002DC RID: 732
	public GameObject buySmileButton;

	// Token: 0x040002DD RID: 733
	public GameObject chatLabelPrefab;

	// Token: 0x040002DE RID: 734
	public AudioClip sendChatClip;

	// Token: 0x040002DF RID: 735
	public bool isClanMode;

	// Token: 0x040002E0 RID: 736
	public UIButton sendMessageButton;

	// Token: 0x040002E1 RID: 737
	public Transform bottomAnchor;

	// Token: 0x040002E2 RID: 738
	public UITable labelTable;

	// Token: 0x040002E3 RID: 739
	public UIScrollView scrollLabels;

	// Token: 0x040002E4 RID: 740
	[NonSerialized]
	public bool isBuySmile;

	// Token: 0x040002E5 RID: 741
	private float keyboardSize;

	// Token: 0x040002E6 RID: 742
	public bool isShowSmilePanel;

	// Token: 0x040002E7 RID: 743
	private bool needReset;
}
