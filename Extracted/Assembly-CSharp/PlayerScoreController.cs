using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000483 RID: 1155
public class PlayerScoreController : MonoBehaviour
{
	// Token: 0x06002825 RID: 10277 RVA: 0x000C9980 File Offset: 0x000C7B80
	private void Start()
	{
		if (Defs.isMulti && ((Defs.isInet && !base.GetComponent<PhotonView>().isMine) || (!Defs.isInet && !base.GetComponent<NetworkView>().isMine)))
		{
			base.enabled = false;
		}
		else
		{
			foreach (KeyValuePair<string, string> keyValuePair in PlayerEventScoreController.audioClipNameOnEvent)
			{
				string value = keyValuePair.Value;
				AudioClip audioClip = Resources.Load("ScoreEventSounds/" + value) as AudioClip;
				if (audioClip != null)
				{
					this.clips.Add(keyValuePair.Key, audioClip);
				}
			}
		}
	}

	// Token: 0x06002826 RID: 10278 RVA: 0x000C9A68 File Offset: 0x000C7C68
	public void AddScoreOnEvent(PlayerEventScoreController.ScoreEvent _event, float _koef = 1f)
	{
		if ((_event == PlayerEventScoreController.ScoreEvent.deadHeadShot || _event == PlayerEventScoreController.ScoreEvent.deadHeadShot) && Time.time - this.timeOldHeadShot < 1.5f)
		{
			_event = PlayerEventScoreController.ScoreEvent.doubleHeadShot;
		}
		int num = (int)((float)PlayerEventScoreController.scoreOnEvent[_event.ToString()] * _koef);
		if (num != 0)
		{
			this.currentScore = WeaponManager.sharedManager.myNetworkStartTable.score;
			this.currentScore = Mathf.Max(0, num + this.currentScore);
			string text = PlayerEventScoreController.messageOnEvent[_event.ToString()];
			if (!string.IsNullOrEmpty(text))
			{
				this.AddScoreMessage(string.Concat(new object[]
				{
					(num <= 0) ? string.Empty : "+",
					num,
					" ",
					LocalizationStore.Get(text)
				}), num);
			}
		}
		string text2 = PlayerEventScoreController.pictureNameOnEvent[_event.ToString()];
		if (!string.IsNullOrEmpty(text2) && InGameGUI.sharedInGameGUI != null)
		{
			bool flag = true;
			if (text2.Equals("Kill") && WeaponManager.sharedManager.myPlayerMoveC != null && WeaponManager.sharedManager.myPlayerMoveC.multiKill > 0)
			{
				flag = false;
			}
			if (flag && !this.pictNameList.Contains(text2))
			{
				this.pictNameList.Add(text2);
			}
		}
		if (num != 0 && Defs.isMulti)
		{
			GlobalGameController.Score = this.currentScore;
			WeaponManager.sharedManager.myNetworkStartTable.score = this.currentScore;
			WeaponManager.sharedManager.myNetworkStartTable.SynhScore();
		}
	}

	// Token: 0x06002827 RID: 10279 RVA: 0x000C9C20 File Offset: 0x000C7E20
	private void AddScoreMessage(string _message, int _addScore)
	{
		this.addScoreString[2] = this.addScoreString[1];
		this.addScoreString[1] = _message;
		if (this.timerAddScoreShow[0] > 0f)
		{
			this.sumScore += _addScore;
		}
		else
		{
			this.sumScore = _addScore;
		}
		this.addScoreString[0] = this.sumScore.ToString();
		this.timerAddScoreShow[2] = this.timerAddScoreShow[1];
		this.timerAddScoreShow[1] = this.maxTimerMessage;
		this.timerAddScoreShow[0] = this.maxTimerSumMessage;
	}

	// Token: 0x06002828 RID: 10280 RVA: 0x000C9CB4 File Offset: 0x000C7EB4
	private void Update()
	{
		this.timeShowPict += Time.deltaTime;
		if (this.timeShowPict > this.minTimeShowPict && this.pictNameList.Count > 0)
		{
			string text = this.pictNameList[0];
			this.timeShowPict = 0f;
			InGameGUI.sharedInGameGUI.timerShowScorePict = InGameGUI.sharedInGameGUI.maxTimerShowScorePict;
			InGameGUI.sharedInGameGUI.scorePictName = text;
			if (this.clips.ContainsKey(text) && Defs.isSoundFX)
			{
				NGUITools.PlaySound(this.clips[text]);
			}
			this.pictNameList.RemoveAt(0);
		}
		if (this.timerAddScoreShow[2] > 0f)
		{
			this.timerAddScoreShow[2] -= Time.deltaTime;
		}
		if (this.timerAddScoreShow[1] > 0f)
		{
			this.timerAddScoreShow[1] -= Time.deltaTime;
		}
		if (this.timerAddScoreShow[0] > 0f)
		{
			this.timerAddScoreShow[0] -= Time.deltaTime;
		}
	}

	// Token: 0x04001CE7 RID: 7399
	public int currentScore;

	// Token: 0x04001CE8 RID: 7400
	public string[] addScoreString = new string[]
	{
		string.Empty,
		string.Empty,
		string.Empty
	};

	// Token: 0x04001CE9 RID: 7401
	public int sumScore;

	// Token: 0x04001CEA RID: 7402
	public float[] timerAddScoreShow = new float[3];

	// Token: 0x04001CEB RID: 7403
	public float maxTimerMessage = 2f;

	// Token: 0x04001CEC RID: 7404
	public float maxTimerSumMessage = 4f;

	// Token: 0x04001CED RID: 7405
	private float timeOldHeadShot;

	// Token: 0x04001CEE RID: 7406
	private List<string> pictNameList = new List<string>();

	// Token: 0x04001CEF RID: 7407
	private float timeShowPict;

	// Token: 0x04001CF0 RID: 7408
	private float minTimeShowPict = 1f;

	// Token: 0x04001CF1 RID: 7409
	private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
}
