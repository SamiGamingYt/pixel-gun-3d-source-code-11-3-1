using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x02000300 RID: 768
public sealed class MapPreviewController : MonoBehaviour
{
	// Token: 0x170004BB RID: 1211
	// (get) Token: 0x06001B1E RID: 6942 RVA: 0x0006F66C File Offset: 0x0006D86C
	// (set) Token: 0x06001B1F RID: 6943 RVA: 0x0006F674 File Offset: 0x0006D874
	private int _rating
	{
		get
		{
			return this._ratingVal;
		}
		set
		{
			this._ratingVal = value;
			if (this._ratingVal >= 0)
			{
				this.popularitySprite.spriteName = string.Format("Nb_Players_{0}", this._ratingVal);
			}
		}
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x0006F6BC File Offset: 0x0006D8BC
	private void Start()
	{
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x0006F6C0 File Offset: 0x0006D8C0
	public void UpdatePopularity()
	{
		base.StopCoroutine(this.SetPopularity());
		base.StartCoroutine(this.SetPopularity());
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x0006F6E8 File Offset: 0x0006D8E8
	private IEnumerator SetPopularity()
	{
		Lazy<HashSet<ConnectSceneNGUIController.RegimGame>> loggedFailedModes = new Lazy<HashSet<ConnectSceneNGUIController.RegimGame>>(() => new HashSet<ConnectSceneNGUIController.RegimGame>());
		Dictionary<string, string> _mapsPoplarityInCurrentRegim;
		for (;;)
		{
			if (FriendsController.mapPopularityDictionary.Count > 0)
			{
				ConnectSceneNGUIController.RegimGame mode = ConnectSceneNGUIController.regim;
				Dictionary<string, Dictionary<string, string>> mapPopularityDictionary = FriendsController.mapPopularityDictionary;
				int num = (int)mode;
				if (!mapPopularityDictionary.TryGetValue(num.ToString(), out _mapsPoplarityInCurrentRegim) && !loggedFailedModes.Value.Contains(mode))
				{
					Debug.LogWarningFormat("Cannot find given key in map popularity dictionary: {0} ({1})", new object[]
					{
						(int)mode,
						mode
					});
					loggedFailedModes.Value.Add(mode);
				}
				if (_mapsPoplarityInCurrentRegim != null)
				{
					break;
				}
				this._rating = 0;
				yield return base.StartCoroutine(this.MyWaitForSeconds(2f));
			}
			else
			{
				yield return base.StartCoroutine(this.MyWaitForSeconds(2f));
			}
		}
		int _countPlayersOnMap = (!_mapsPoplarityInCurrentRegim.ContainsKey(this.mapID.ToString())) ? 0 : int.Parse(_mapsPoplarityInCurrentRegim[this.mapID.ToString()]);
		if (_countPlayersOnMap < 1)
		{
			this._rating = 0;
		}
		else if (_countPlayersOnMap >= 1 && _countPlayersOnMap < 8)
		{
			this._rating = 1;
		}
		else if (_countPlayersOnMap >= 8 && _countPlayersOnMap < 15)
		{
			this._rating = 2;
		}
		else if (_countPlayersOnMap >= 15 && _countPlayersOnMap < 35)
		{
			this._rating = 3;
		}
		else if (_countPlayersOnMap >= 35 && _countPlayersOnMap < 50)
		{
			this._rating = 4;
		}
		else if (_countPlayersOnMap >= 50)
		{
			this._rating = 5;
		}
		yield break;
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x0006F704 File Offset: 0x0006D904
	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
		yield break;
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x0006F728 File Offset: 0x0006D928
	private void OnClick()
	{
		if (ConnectSceneNGUIController.sharedController.selectMap.Equals(this))
		{
			if (!ConnectSceneNGUIController.sharedController.createPanel.activeSelf)
			{
				ConnectSceneNGUIController.sharedController.HandleGoBtnClicked(null, EventArgs.Empty);
			}
		}
		else
		{
			ConnectSceneNGUIController.sharedController.selectMap = this;
			ConnectSceneNGUIController.sharedController.StopFingerAnimation();
		}
	}

	// Token: 0x04001045 RID: 4165
	private readonly string[] _ratingLabelsKeys = new string[]
	{
		"Key_0545",
		"Key_0546",
		"Key_0547",
		"Key_0548",
		"Key_0549",
		"Key_2183"
	};

	// Token: 0x04001046 RID: 4166
	public UILabel NameMapLbl;

	// Token: 0x04001047 RID: 4167
	public GameObject[] SizeMapNameLbl;

	// Token: 0x04001048 RID: 4168
	public UILabel popularityLabel;

	// Token: 0x04001049 RID: 4169
	public UISprite popularitySprite;

	// Token: 0x0400104A RID: 4170
	public GameObject premium;

	// Token: 0x0400104B RID: 4171
	public GameObject milee;

	// Token: 0x0400104C RID: 4172
	public GameObject dater;

	// Token: 0x0400104D RID: 4173
	public int mapID;

	// Token: 0x0400104E RID: 4174
	public string sceneMapName;

	// Token: 0x0400104F RID: 4175
	public UITexture mapPreviewTexture;

	// Token: 0x04001050 RID: 4176
	public GameObject bottomPanel;

	// Token: 0x04001051 RID: 4177
	private MyCenterOnChild centerChild;

	// Token: 0x04001052 RID: 4178
	[ReadOnly]
	[SerializeField]
	private int _ratingVal;
}
