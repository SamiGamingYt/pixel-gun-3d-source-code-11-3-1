using System;
using UnityEngine;

// Token: 0x0200015E RID: 350
public class GoMapInEndGame : MonoBehaviour
{
	// Token: 0x06000B77 RID: 2935 RVA: 0x00040A6C File Offset: 0x0003EC6C
	private void OnEnable()
	{
		this.enableTime = Time.time;
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x00040A7C File Offset: 0x0003EC7C
	private void Start()
	{
		if (!Defs.isInet || Defs.isDaterRegim)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x00040AAC File Offset: 0x0003ECAC
	public void SetMap(SceneInfo scInfo)
	{
		if (scInfo == null)
		{
			this.mapIndex = -1;
			this.mapTexture.mainTexture = Resources.Load<Texture>("LevelLoadingsSmall/Random_Map");
			this.mapLabel.text = LocalizationStore.Get("Key_2463");
		}
		else
		{
			this.mapIndex = scInfo.indexMap;
			this.mapTexture.mainTexture = Resources.Load<Texture>("LevelLoadingsSmall/Loading_" + scInfo.NameScene);
			if (scInfo != null)
			{
				this.mapLabel.text = scInfo.TranslateName;
			}
		}
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x00040B44 File Offset: 0x0003ED44
	public void OnClick()
	{
		if (Time.time - this.enableTime < 2f)
		{
			return;
		}
		if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
		{
			return;
		}
		if (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown)
		{
			return;
		}
		Defs.typeDisconnectGame = Defs.DisconectGameType.SelectNewMap;
		if (this.mapIndex != -1)
		{
			SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(this.mapIndex);
			Initializer.Instance.goMapName = infoScene.NameScene;
		}
		else
		{
			int randomMapIndex = ConnectSceneNGUIController.GetRandomMapIndex();
			if (randomMapIndex != -1)
			{
				SceneInfo infoScene2 = SceneInfoController.instance.GetInfoScene(randomMapIndex);
				Initializer.Instance.goMapName = infoScene2.NameScene;
			}
			else
			{
				Initializer.Instance.goMapName = string.Empty;
			}
		}
		GlobalGameController.countKillsRed = 0;
		GlobalGameController.countKillsBlue = 0;
		PhotonNetwork.LeaveRoom();
		this.IsLeavingRoom = true;
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00040C38 File Offset: 0x0003EE38
	// (set) Token: 0x06000B7C RID: 2940 RVA: 0x00040C40 File Offset: 0x0003EE40
	public bool IsLeavingRoom
	{
		get
		{
			return this._isLeavingRoom;
		}
		protected set
		{
			this._isLeavingRoom = value;
		}
	}

	// Token: 0x04000918 RID: 2328
	public int mapIndex;

	// Token: 0x04000919 RID: 2329
	public UITexture mapTexture;

	// Token: 0x0400091A RID: 2330
	public UILabel mapLabel;

	// Token: 0x0400091B RID: 2331
	private float enableTime;

	// Token: 0x0400091C RID: 2332
	private bool _isLeavingRoom;
}
