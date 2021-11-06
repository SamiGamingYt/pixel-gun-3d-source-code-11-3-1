using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x0200070D RID: 1805
internal sealed class ProfileView : MonoBehaviour
{
	// Token: 0x1400007E RID: 126
	// (add) Token: 0x06003ED4 RID: 16084 RVA: 0x001513C0 File Offset: 0x0014F5C0
	// (remove) Token: 0x06003ED5 RID: 16085 RVA: 0x001513E0 File Offset: 0x0014F5E0
	public event EventHandler BackButtonPressed
	{
		add
		{
			if (this.backButton != null)
			{
				this.backButton.Clicked += value;
			}
		}
		remove
		{
			if (this.backButton != null)
			{
				this.backButton.Clicked -= value;
			}
		}
	}

	// Token: 0x1400007F RID: 127
	// (add) Token: 0x06003ED6 RID: 16086 RVA: 0x00151400 File Offset: 0x0014F600
	// (remove) Token: 0x06003ED7 RID: 16087 RVA: 0x0015141C File Offset: 0x0014F61C
	public event EventHandler<ProfileView.InputEventArgs> NicknameInput;

	// Token: 0x06003ED8 RID: 16088 RVA: 0x00151438 File Offset: 0x0014F638
	private void Awake()
	{
		if (this.copyIdButton != null && BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
		{
			this.copyIdButton.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003ED9 RID: 16089 RVA: 0x00151474 File Offset: 0x0014F674
	public void OnSubmit()
	{
		if (this.nicknameInput == null)
		{
			return;
		}
		EventHandler<ProfileView.InputEventArgs> eventHandler = this.NicknameInput;
		if (eventHandler != null)
		{
			eventHandler(this, new ProfileView.InputEventArgs(this.nicknameInput.value));
		}
	}

	// Token: 0x06003EDA RID: 16090 RVA: 0x001514B8 File Offset: 0x0014F6B8
	public void OnCopyIdClick()
	{
		FriendsController.CopyMyIdToClipboard();
	}

	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x06003EDB RID: 16091 RVA: 0x001514C0 File Offset: 0x0014F6C0
	// (set) Token: 0x06003EDC RID: 16092 RVA: 0x001514D0 File Offset: 0x0014F6D0
	public string DeathmatchWinCount
	{
		get
		{
			return ProfileView.GetText(this.deathmatchWinCount);
		}
		set
		{
			ProfileView.SetText(this.deathmatchWinCount, value);
		}
	}

	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x06003EDD RID: 16093 RVA: 0x001514E0 File Offset: 0x0014F6E0
	// (set) Token: 0x06003EDE RID: 16094 RVA: 0x001514F0 File Offset: 0x0014F6F0
	public string TotalWeeklyWinCount
	{
		get
		{
			return ProfileView.GetText(this.totalWeeklyWinsCount);
		}
		set
		{
			ProfileView.SetText(this.totalWeeklyWinsCount, value);
		}
	}

	// Token: 0x17000A6A RID: 2666
	// (get) Token: 0x06003EDF RID: 16095 RVA: 0x00151500 File Offset: 0x0014F700
	// (set) Token: 0x06003EE0 RID: 16096 RVA: 0x00151510 File Offset: 0x0014F710
	public string TeamBattleWinCount
	{
		get
		{
			return ProfileView.GetText(this.teamBattleWinCount);
		}
		set
		{
			ProfileView.SetText(this.teamBattleWinCount, value);
		}
	}

	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x06003EE1 RID: 16097 RVA: 0x00151520 File Offset: 0x0014F720
	// (set) Token: 0x06003EE2 RID: 16098 RVA: 0x00151530 File Offset: 0x0014F730
	public string CapturePointWinCount
	{
		get
		{
			return ProfileView.GetText(this.capturePointCount);
		}
		set
		{
			ProfileView.SetText(this.capturePointCount, value);
		}
	}

	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x06003EE3 RID: 16099 RVA: 0x00151540 File Offset: 0x0014F740
	// (set) Token: 0x06003EE4 RID: 16100 RVA: 0x00151550 File Offset: 0x0014F750
	public string DuelWinCount
	{
		get
		{
			return ProfileView.GetText(this.duelCount);
		}
		set
		{
			ProfileView.SetText(this.duelCount, value);
		}
	}

	// Token: 0x17000A6D RID: 2669
	// (get) Token: 0x06003EE5 RID: 16101 RVA: 0x00151560 File Offset: 0x0014F760
	// (set) Token: 0x06003EE6 RID: 16102 RVA: 0x00151570 File Offset: 0x0014F770
	public string DeadlyGamesWinCount
	{
		get
		{
			return ProfileView.GetText(this.deadlyGamesWinCount);
		}
		set
		{
			ProfileView.SetText(this.deadlyGamesWinCount, value);
		}
	}

	// Token: 0x17000A6E RID: 2670
	// (get) Token: 0x06003EE7 RID: 16103 RVA: 0x00151580 File Offset: 0x0014F780
	// (set) Token: 0x06003EE8 RID: 16104 RVA: 0x00151590 File Offset: 0x0014F790
	public string FlagCaptureWinCount
	{
		get
		{
			return ProfileView.GetText(this.flagCaptureWinCount);
		}
		set
		{
			ProfileView.SetText(this.flagCaptureWinCount, value);
		}
	}

	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x06003EE9 RID: 16105 RVA: 0x001515A0 File Offset: 0x0014F7A0
	// (set) Token: 0x06003EEA RID: 16106 RVA: 0x001515B0 File Offset: 0x0014F7B0
	public string TotalWinCount
	{
		get
		{
			return ProfileView.GetText(this.totalWinCount);
		}
		set
		{
			ProfileView.SetText(this.totalWinCount, value);
		}
	}

	// Token: 0x17000A70 RID: 2672
	// (get) Token: 0x06003EEB RID: 16107 RVA: 0x001515C0 File Offset: 0x0014F7C0
	// (set) Token: 0x06003EEC RID: 16108 RVA: 0x001515D0 File Offset: 0x0014F7D0
	public string PixelgunFriendsID
	{
		get
		{
			return ProfileView.GetText(this.pixelgunFriendsID);
		}
		set
		{
			ProfileView.SetText(this.pixelgunFriendsID, "ID: " + value);
		}
	}

	// Token: 0x17000A71 RID: 2673
	// (get) Token: 0x06003EED RID: 16109 RVA: 0x001515E8 File Offset: 0x0014F7E8
	// (set) Token: 0x06003EEE RID: 16110 RVA: 0x001515F8 File Offset: 0x0014F7F8
	public string CoopTimeSurvivalPointCount
	{
		get
		{
			return ProfileView.GetText(this.coopTimeSurvivalPointCount);
		}
		set
		{
			ProfileView.SetText(this.coopTimeSurvivalPointCount, value);
		}
	}

	// Token: 0x17000A72 RID: 2674
	// (get) Token: 0x06003EEF RID: 16111 RVA: 0x00151608 File Offset: 0x0014F808
	// (set) Token: 0x06003EF0 RID: 16112 RVA: 0x00151618 File Offset: 0x0014F818
	public string GameTotalKills
	{
		get
		{
			return ProfileView.GetText(this.lbGameTotalKills);
		}
		set
		{
			ProfileView.SetText(this.lbGameTotalKills, value);
		}
	}

	// Token: 0x17000A73 RID: 2675
	// (get) Token: 0x06003EF1 RID: 16113 RVA: 0x00151628 File Offset: 0x0014F828
	// (set) Token: 0x06003EF2 RID: 16114 RVA: 0x00151638 File Offset: 0x0014F838
	public string GameKillrate
	{
		get
		{
			return ProfileView.GetText(this.lbGameKillrate);
		}
		set
		{
			ProfileView.SetText(this.lbGameKillrate, value);
		}
	}

	// Token: 0x17000A74 RID: 2676
	// (get) Token: 0x06003EF3 RID: 16115 RVA: 0x00151648 File Offset: 0x0014F848
	// (set) Token: 0x06003EF4 RID: 16116 RVA: 0x00151658 File Offset: 0x0014F858
	public string GameAccuracy
	{
		get
		{
			return ProfileView.GetText(this.lbGameAccuracy);
		}
		set
		{
			ProfileView.SetText(this.lbGameAccuracy, value);
		}
	}

	// Token: 0x17000A75 RID: 2677
	// (get) Token: 0x06003EF5 RID: 16117 RVA: 0x00151668 File Offset: 0x0014F868
	// (set) Token: 0x06003EF6 RID: 16118 RVA: 0x00151678 File Offset: 0x0014F878
	public string GameLikes
	{
		get
		{
			return ProfileView.GetText(this.lbGameLikes);
		}
		set
		{
			ProfileView.SetText(this.lbGameLikes, value);
		}
	}

	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x06003EF7 RID: 16119 RVA: 0x00151688 File Offset: 0x0014F888
	// (set) Token: 0x06003EF8 RID: 16120 RVA: 0x00151698 File Offset: 0x0014F898
	public string WaveCountLabel
	{
		get
		{
			return ProfileView.GetText(this.waveCountLabel);
		}
		set
		{
			ProfileView.SetText(this.waveCountLabel, value);
		}
	}

	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x06003EF9 RID: 16121 RVA: 0x001516A8 File Offset: 0x0014F8A8
	// (set) Token: 0x06003EFA RID: 16122 RVA: 0x001516B8 File Offset: 0x0014F8B8
	public string KilledCountLabel
	{
		get
		{
			return ProfileView.GetText(this.killedCountLabel);
		}
		set
		{
			ProfileView.SetText(this.killedCountLabel, value);
		}
	}

	// Token: 0x17000A78 RID: 2680
	// (get) Token: 0x06003EFB RID: 16123 RVA: 0x001516C8 File Offset: 0x0014F8C8
	// (set) Token: 0x06003EFC RID: 16124 RVA: 0x001516D8 File Offset: 0x0014F8D8
	public string SurvivalScoreLabel
	{
		get
		{
			return ProfileView.GetText(this.survivalScoreLabel);
		}
		set
		{
			ProfileView.SetText(this.survivalScoreLabel, value);
		}
	}

	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x06003EFD RID: 16125 RVA: 0x001516E8 File Offset: 0x0014F8E8
	// (set) Token: 0x06003EFE RID: 16126 RVA: 0x001516F8 File Offset: 0x0014F8F8
	public string Box1StarsLabel
	{
		get
		{
			return ProfileView.GetText(this.box1StarsLabel);
		}
		set
		{
			ProfileView.SetText(this.box1StarsLabel, value);
			if (this.box1StarsLabel != null)
			{
				UISprite componentInChildren = this.box1StarsLabel.GetComponentInChildren<UISprite>();
				if (componentInChildren != null)
				{
					Vector3 localPosition = this.box1StarsLabel.transform.localPosition;
					componentInChildren.transform.localPosition = new Vector3((float)(-(float)this.box1StarsLabel.width), localPosition.y, localPosition.z);
				}
			}
		}
	}

	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x06003EFF RID: 16127 RVA: 0x00151778 File Offset: 0x0014F978
	// (set) Token: 0x06003F00 RID: 16128 RVA: 0x00151788 File Offset: 0x0014F988
	public string Box2StarsLabel
	{
		get
		{
			return ProfileView.GetText(this.box2StarsLabel);
		}
		set
		{
			ProfileView.SetText(this.box2StarsLabel, value);
			if (this.box2StarsLabel != null)
			{
				UISprite componentInChildren = this.box2StarsLabel.GetComponentInChildren<UISprite>();
				if (componentInChildren != null)
				{
					Vector3 localPosition = this.box2StarsLabel.transform.localPosition;
					componentInChildren.transform.localPosition = new Vector3((float)(-(float)this.box2StarsLabel.width), localPosition.y, localPosition.z);
				}
			}
		}
	}

	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x06003F01 RID: 16129 RVA: 0x00151808 File Offset: 0x0014FA08
	// (set) Token: 0x06003F02 RID: 16130 RVA: 0x00151818 File Offset: 0x0014FA18
	public string Box3StarsLabel
	{
		get
		{
			return ProfileView.GetText(this.box3StarsLabel);
		}
		set
		{
			ProfileView.SetText(this.box3StarsLabel, value);
			if (this.box3StarsLabel != null)
			{
				UISprite componentInChildren = this.box3StarsLabel.GetComponentInChildren<UISprite>();
				if (componentInChildren != null)
				{
					Vector3 localPosition = this.box3StarsLabel.transform.localPosition;
					componentInChildren.transform.localPosition = new Vector3((float)(-(float)this.box3StarsLabel.width), localPosition.y, localPosition.z);
				}
			}
		}
	}

	// Token: 0x17000A7C RID: 2684
	// (get) Token: 0x06003F03 RID: 16131 RVA: 0x00151898 File Offset: 0x0014FA98
	// (set) Token: 0x06003F04 RID: 16132 RVA: 0x001518A8 File Offset: 0x0014FAA8
	public string SecretCoinsLabel
	{
		get
		{
			return ProfileView.GetText(this.secretCoinsLabel);
		}
		set
		{
			if (this.secretCoinsLabel == null)
			{
				return;
			}
			ProfileView.SetText(this.secretCoinsLabel, value);
			UISprite componentInChildren = this.secretCoinsLabel.GetComponentInChildren<UISprite>();
			if (componentInChildren == null)
			{
				return;
			}
			Vector3 localPosition = this.secretCoinsLabel.transform.localPosition;
			componentInChildren.transform.localPosition = new Vector3((float)(-(float)this.secretCoinsLabel.width), localPosition.y, localPosition.z);
		}
	}

	// Token: 0x17000A7D RID: 2685
	// (get) Token: 0x06003F05 RID: 16133 RVA: 0x00151928 File Offset: 0x0014FB28
	// (set) Token: 0x06003F06 RID: 16134 RVA: 0x00151938 File Offset: 0x0014FB38
	public string SecretGemsLabel
	{
		get
		{
			return ProfileView.GetText(this.secretGemsLabel);
		}
		set
		{
			if (this.secretGemsLabel == null)
			{
				return;
			}
			ProfileView.SetText(this.secretGemsLabel, value);
			UISprite componentInChildren = this.secretGemsLabel.GetComponentInChildren<UISprite>();
			if (componentInChildren == null)
			{
				return;
			}
			Vector3 localPosition = this.secretGemsLabel.transform.localPosition;
			componentInChildren.transform.localPosition = new Vector3((float)(-(float)this.secretGemsLabel.width), localPosition.y, localPosition.z);
		}
	}

	// Token: 0x17000A7E RID: 2686
	// (get) Token: 0x06003F07 RID: 16135 RVA: 0x001519B8 File Offset: 0x0014FBB8
	// (set) Token: 0x06003F08 RID: 16136 RVA: 0x001519F8 File Offset: 0x0014FBF8
	public string Nickname
	{
		get
		{
			return (!(this.nicknameInput != null)) ? string.Empty : (this.nicknameInput.value ?? string.Empty);
		}
		set
		{
			if (this.nicknameInput != null)
			{
				this.nicknameInput.value = (value ?? string.Empty);
			}
		}
	}

	// Token: 0x06003F09 RID: 16137 RVA: 0x00151A24 File Offset: 0x0014FC24
	public void SetClanLogo(string logoBase64)
	{
		if (this.clanLogo == null)
		{
			Debug.LogWarning("clanLogo == null");
			return;
		}
		Texture2D texture2D = CharacterView.GetClanLogo(logoBase64);
		if (texture2D == null)
		{
			this.clanLogo.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			this.clanLogo.mainTexture = texture2D;
		}
	}

	// Token: 0x06003F0A RID: 16138 RVA: 0x00151A8C File Offset: 0x0014FC8C
	public void SetWeaponAndSkin(string tg, bool replaceRemovedWeapons)
	{
		this.characterView.SetWeaponAndSkin(tg, SkinsController.currentSkinForPers, replaceRemovedWeapons);
		ShopNGUIController.DisableLightProbesRecursively(this.characterView.gameObject);
	}

	// Token: 0x06003F0B RID: 16139 RVA: 0x00151ABC File Offset: 0x0014FCBC
	public void UpdateHat(string hat)
	{
		this.characterView.UpdateHat(hat);
		ShopNGUIController.SetPersHatVisible(this.characterView.hatPoint);
	}

	// Token: 0x06003F0C RID: 16140 RVA: 0x00151ADC File Offset: 0x0014FCDC
	public void RemoveHat()
	{
		this.characterView.RemoveHat();
	}

	// Token: 0x06003F0D RID: 16141 RVA: 0x00151AEC File Offset: 0x0014FCEC
	public void UpdateMask(string mask)
	{
		this.characterView.UpdateMask(mask);
	}

	// Token: 0x06003F0E RID: 16142 RVA: 0x00151AFC File Offset: 0x0014FCFC
	public void RemoveMask()
	{
		this.characterView.RemoveMask();
	}

	// Token: 0x06003F0F RID: 16143 RVA: 0x00151B0C File Offset: 0x0014FD0C
	public void UpdateCape(string cape)
	{
		this.characterView.UpdateCape(cape, null);
	}

	// Token: 0x06003F10 RID: 16144 RVA: 0x00151B1C File Offset: 0x0014FD1C
	public void RemoveCape()
	{
		this.characterView.RemoveCape();
	}

	// Token: 0x06003F11 RID: 16145 RVA: 0x00151B2C File Offset: 0x0014FD2C
	public void UpdateBoots(string bs)
	{
		this.characterView.UpdateBoots(bs);
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x00151B3C File Offset: 0x0014FD3C
	public void RemoveBoots()
	{
		this.characterView.RemoveBoots();
	}

	// Token: 0x06003F13 RID: 16147 RVA: 0x00151B4C File Offset: 0x0014FD4C
	public void UpdateArmor(string armor)
	{
		this.characterView.UpdateArmor(armor);
		ShopNGUIController.SetPersArmorVisible(this.characterView.armorPoint);
	}

	// Token: 0x06003F14 RID: 16148 RVA: 0x00151B6C File Offset: 0x0014FD6C
	public void RemoveArmor()
	{
		this.characterView.RemoveArmor();
	}

	// Token: 0x06003F15 RID: 16149 RVA: 0x00151B7C File Offset: 0x0014FD7C
	private static string GetText(UILabel label)
	{
		if (label == null)
		{
			return string.Empty;
		}
		return label.text ?? string.Empty;
	}

	// Token: 0x06003F16 RID: 16150 RVA: 0x00151BB0 File Offset: 0x0014FDB0
	private static void SetText(UILabel label, string value)
	{
		if (label != null)
		{
			label.text = (value ?? string.Empty);
		}
	}

	// Token: 0x17000A7F RID: 2687
	// (get) Token: 0x06003F17 RID: 16151 RVA: 0x00151BD4 File Offset: 0x0014FDD4
	private bool IdPlayerExist
	{
		get
		{
			return FriendsController.sharedController != null && !string.IsNullOrEmpty(FriendsController.sharedController.id);
		}
	}

	// Token: 0x06003F18 RID: 16152 RVA: 0x00151BFC File Offset: 0x0014FDFC
	public void CheckBtnCopy()
	{
		base.StartCoroutine(this.Crt_CheckBtnCopy());
	}

	// Token: 0x06003F19 RID: 16153 RVA: 0x00151C0C File Offset: 0x0014FE0C
	private IEnumerator Crt_CheckBtnCopy()
	{
		if (this.copyIdButton)
		{
			this.copyIdButton.gameObject.SetActive(false);
		}
		while (!this.IdPlayerExist)
		{
			yield return new WaitForSeconds(1f);
		}
		if (this.copyIdButton)
		{
			this.copyIdButton.gameObject.SetActive(true);
		}
		yield break;
	}

	// Token: 0x04002E61 RID: 11873
	public UILabel pixelgunFriendsID;

	// Token: 0x04002E62 RID: 11874
	public GameObject interfaceHolder;

	// Token: 0x04002E63 RID: 11875
	public UIRoot interfaceHolder2d;

	// Token: 0x04002E64 RID: 11876
	public GameObject worldHolder3d;

	// Token: 0x04002E65 RID: 11877
	public UILabel totalWeeklyWinsCount;

	// Token: 0x04002E66 RID: 11878
	public UILabel deathmatchWinCount;

	// Token: 0x04002E67 RID: 11879
	public UILabel teamBattleWinCount;

	// Token: 0x04002E68 RID: 11880
	public UILabel capturePointCount;

	// Token: 0x04002E69 RID: 11881
	public UILabel deadlyGamesWinCount;

	// Token: 0x04002E6A RID: 11882
	public UILabel flagCaptureWinCount;

	// Token: 0x04002E6B RID: 11883
	public UILabel duelCount;

	// Token: 0x04002E6C RID: 11884
	public UILabel totalWinCount;

	// Token: 0x04002E6D RID: 11885
	public UILabel lbGameTotalKills;

	// Token: 0x04002E6E RID: 11886
	public UILabel lbGameKillrate;

	// Token: 0x04002E6F RID: 11887
	public UILabel lbGameAccuracy;

	// Token: 0x04002E70 RID: 11888
	public UILabel lbGameLikes;

	// Token: 0x04002E71 RID: 11889
	public UILabel coopTimeSurvivalPointCount;

	// Token: 0x04002E72 RID: 11890
	public UILabel waveCountLabel;

	// Token: 0x04002E73 RID: 11891
	public UILabel killedCountLabel;

	// Token: 0x04002E74 RID: 11892
	public UILabel survivalScoreLabel;

	// Token: 0x04002E75 RID: 11893
	public UILabel box1StarsLabel;

	// Token: 0x04002E76 RID: 11894
	public UILabel box2StarsLabel;

	// Token: 0x04002E77 RID: 11895
	public UILabel box3StarsLabel;

	// Token: 0x04002E78 RID: 11896
	public UILabel secretCoinsLabel;

	// Token: 0x04002E79 RID: 11897
	public UILabel secretGemsLabel;

	// Token: 0x04002E7A RID: 11898
	public UIInputRilisoft nicknameInput;

	// Token: 0x04002E7B RID: 11899
	public UITexture clanLogo;

	// Token: 0x04002E7C RID: 11900
	public ButtonHandler backButton;

	// Token: 0x04002E7D RID: 11901
	public UIButton achievementsButton;

	// Token: 0x04002E7E RID: 11902
	public UIButton leaderboardsButton;

	// Token: 0x04002E7F RID: 11903
	public UIButton copyIdButton;

	// Token: 0x04002E80 RID: 11904
	public CharacterView characterView;

	// Token: 0x0200070E RID: 1806
	public class InputEventArgs : EventArgs
	{
		// Token: 0x06003F1A RID: 16154 RVA: 0x00151C28 File Offset: 0x0014FE28
		public InputEventArgs(string input)
		{
			this.Input = (input ?? string.Empty);
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003F1B RID: 16155 RVA: 0x00151C44 File Offset: 0x0014FE44
		// (set) Token: 0x06003F1C RID: 16156 RVA: 0x00151C4C File Offset: 0x0014FE4C
		public string Input { get; private set; }
	}
}
