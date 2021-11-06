using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000301 RID: 769
internal sealed class MenuBackgroundMusic : MonoBehaviour
{
	// Token: 0x06001B27 RID: 6951 RVA: 0x0006F80C File Offset: 0x0006DA0C
	public void PlayCustomMusicFrom(GameObject audioSourceObj)
	{
		this.RemoveNullsFromCustomMusicStack();
		if (audioSourceObj != null && Defs.isSoundMusic)
		{
			AudioSource component = audioSourceObj.GetComponent<AudioSource>();
			this.PlayMusic(component);
			if (!this._customMusicStack.Contains(component))
			{
				if (this._customMusicStack.Count > 0)
				{
					this.StopMusic(this._customMusicStack[this._customMusicStack.Count - 1]);
				}
				this._customMusicStack.Add(audioSourceObj.GetComponent<AudioSource>());
			}
		}
		string name = SceneManager.GetActiveScene().name;
		if (Array.IndexOf<string>(MenuBackgroundMusic.scenetsToPlayMusicOn, name) >= 0)
		{
			this.Stop();
		}
		else
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
			if (gameObject != null)
			{
				AudioSource component2 = gameObject.GetComponent<AudioSource>();
				if (component2 != null)
				{
					this.StopMusic(component2);
				}
			}
		}
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x0006F8F0 File Offset: 0x0006DAF0
	public void StopCustomMusicFrom(GameObject audioSourceObj)
	{
		this.RemoveNullsFromCustomMusicStack();
		AudioSource component = audioSourceObj.GetComponent<AudioSource>();
		if (audioSourceObj != null && component != null)
		{
			this.StopMusic(component);
			this._customMusicStack.Remove(component);
		}
		if (this._customMusicStack.Count > 0)
		{
			this.PlayMusic(this._customMusicStack[this._customMusicStack.Count - 1]);
		}
		else if (Array.IndexOf<string>(MenuBackgroundMusic.scenetsToPlayMusicOn, Application.loadedLevelName) >= 0)
		{
			this.Play();
		}
		else
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("BackgroundMusic");
			if (gameObject != null)
			{
				AudioSource component2 = gameObject.GetComponent<AudioSource>();
				if (component2 != null)
				{
					this.PlayMusic(component2);
				}
			}
		}
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x0006F9BC File Offset: 0x0006DBBC
	internal void Start()
	{
		MenuBackgroundMusic.sharedMusic = this;
		Defs.isSoundMusic = PlayerPrefsX.GetBool(PlayerPrefsX.SoundMusicSetting, true);
		Defs.isSoundFX = PlayerPrefsX.GetBool(PlayerPrefsX.SoundFXSetting, true);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		string text = Storager.getString("lobby_background_music", true);
		if (text.IsNullOrEmpty())
		{
			text = MenuBackgroundMusic.LobbyBackgroundClip.Ambient.ToString();
			Storager.setString("lobby_background_music", text, true);
		}
		MenuBackgroundMusic.LobbyBackgroundClip? lobbyBackgroundClip = text.ToEnum(new MenuBackgroundMusic.LobbyBackgroundClip?(MenuBackgroundMusic.LobbyBackgroundClip.None));
		if (lobbyBackgroundClip != null && lobbyBackgroundClip != MenuBackgroundMusic.LobbyBackgroundClip.None)
		{
			AudioSource component = MenuBackgroundMusic.sharedMusic.GetComponent<AudioSource>();
			if (component != null)
			{
				AudioClip clip = Resources.Load<AudioClip>("MenuMusic/menu_music_" + lobbyBackgroundClip.Value.ToString().ToLower());
				component.clip = clip;
			}
		}
	}

	// Token: 0x06001B2A RID: 6954 RVA: 0x0006FAA4 File Offset: 0x0006DCA4
	internal void Play()
	{
		if (Defs.isSoundMusic)
		{
			this.PlayMusic(base.GetComponent<AudioSource>());
		}
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x0006FABC File Offset: 0x0006DCBC
	public void Stop()
	{
		this.StopMusic(base.GetComponent<AudioSource>());
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x0006FACC File Offset: 0x0006DCCC
	private void RemoveNullsFromCustomMusicStack()
	{
		List<AudioSource> customMusicStack = this._customMusicStack;
		this._customMusicStack = new List<AudioSource>();
		foreach (AudioSource audioSource in customMusicStack)
		{
			if (audioSource != null)
			{
				this._customMusicStack.Add(audioSource);
			}
		}
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x0006FB50 File Offset: 0x0006DD50
	private IEnumerator WaitFreeAwardControllerAndSubscribeCoroutine()
	{
		using (new ScopeLogger("WaitFreeAwardControllerAndSubscribeCoroutine", false))
		{
			while (FreeAwardController.Instance == null)
			{
				yield return null;
			}
			FreeAwardController.Instance.StateChanged -= this.HandleFreeAwardControllerStateChanged;
			FreeAwardController.Instance.StateChanged += this.HandleFreeAwardControllerStateChanged;
		}
		yield break;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x0006FB6C File Offset: 0x0006DD6C
	private void OnLevelWasLoaded(int idx)
	{
		base.StopAllCoroutines();
		CoroutineRunner.Instance.StartCoroutine(this.WaitFreeAwardControllerAndSubscribeCoroutine());
		foreach (AudioSource audioSource in this._customMusicStack)
		{
			if (audioSource != null)
			{
				audioSource.Stop();
			}
		}
		this._customMusicStack.Clear();
		if (Array.IndexOf<string>(MenuBackgroundMusic.scenetsToPlayMusicOn, Application.loadedLevelName) >= 0 || MenuBackgroundMusic.keepPlaying)
		{
			if (!base.GetComponent<AudioSource>().isPlaying && PlayerPrefsX.GetBool(PlayerPrefsX.SoundMusicSetting, true))
			{
				this.PlayMusic(base.GetComponent<AudioSource>());
			}
		}
		else
		{
			this.StopMusic(base.GetComponent<AudioSource>());
		}
		MenuBackgroundMusic.keepPlaying = false;
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x0006FC64 File Offset: 0x0006DE64
	private void HandleFreeAwardControllerStateChanged(object sender, FreeAwardController.StateEventArgs e)
	{
		string callee = string.Format(CultureInfo.InvariantCulture, "HandleFreeAwardControllerStateChanged({0} -> {1})", new object[]
		{
			e.OldState,
			e.State
		});
		using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
		{
			if (e.State is FreeAwardController.WatchingState)
			{
				this.Stop();
			}
			else if (e.OldState is FreeAwardController.WatchingState)
			{
				this.Play();
			}
		}
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x0006FD00 File Offset: 0x0006DF00
	public void PlayMusic(AudioSource audioSource)
	{
		if (audioSource == null)
		{
			return;
		}
		if (!Defs.isSoundMusic)
		{
			return;
		}
		if (Switcher.comicsSound != null && audioSource != Switcher.comicsSound.GetComponent<AudioSource>())
		{
			UnityEngine.Object.Destroy(Switcher.comicsSound);
			Switcher.comicsSound = null;
		}
		if (PhotonNetwork.connected)
		{
			float time = Convert.ToSingle(PhotonNetwork.time) - audioSource.clip.length * (float)Mathf.FloorToInt(Convert.ToSingle(PhotonNetwork.time) / audioSource.clip.length);
			audioSource.time = time;
		}
		audioSource.Play();
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x0006FDAC File Offset: 0x0006DFAC
	public void StopMusic(AudioSource audioSource)
	{
		if (audioSource == null)
		{
			return;
		}
		audioSource.Stop();
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x0006FDC4 File Offset: 0x0006DFC4
	private IEnumerator PlayMusicInternal(AudioSource audioSource)
	{
		float targetVolume = 1f;
		audioSource.volume = 1f;
		audioSource.Play();
		this.currentAudioSource = audioSource;
		float startTime = Time.realtimeSinceStartup;
		float fadeTime = 0.5f;
		while (Time.realtimeSinceStartup - startTime <= fadeTime)
		{
			if (audioSource == null)
			{
				audioSource.volume = 1f;
				yield break;
			}
			audioSource.volume = targetVolume * (Time.realtimeSinceStartup - startTime) / fadeTime;
			Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ PlayMusicInternal " + audioSource.volume);
			yield return null;
		}
		audioSource.volume = 1f;
		Debug.Log("----------------------------------------------------------------- PlayMusicInternal " + audioSource.volume);
		yield break;
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x0006FDF0 File Offset: 0x0006DFF0
	private IEnumerator StopMusicInternal(AudioSource audioSource)
	{
		float currentVolume = 1f;
		float startTime = Time.realtimeSinceStartup;
		float fadeTime = 0.5f;
		while (Time.realtimeSinceStartup - startTime <= fadeTime)
		{
			if (audioSource == null)
			{
				yield break;
			}
			audioSource.volume = currentVolume * (1f - (Time.realtimeSinceStartup - startTime) / fadeTime);
			Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ StopMusicInternal " + audioSource.volume);
			yield return null;
		}
		audioSource.volume = 0f;
		audioSource.Stop();
		this.currentAudioSource = null;
		audioSource.volume = 1f;
		Debug.Log("----------------------------------------------------------------- StopMusicInternal " + audioSource.volume);
		yield break;
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x0006FE1C File Offset: 0x0006E01C
	private void PlayCurrentMusic()
	{
		if (this.currentAudioSource != null)
		{
			this.PlayMusic(this.currentAudioSource);
		}
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x0006FE3C File Offset: 0x0006E03C
	private void PauseCurrentMusic()
	{
		if (this.currentAudioSource != null)
		{
			this.currentAudioSource.Pause();
		}
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x0006FE5C File Offset: 0x0006E05C
	private void OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			this.PlayCurrentMusic();
		}
		else
		{
			this.PauseCurrentMusic();
		}
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x0006FE78 File Offset: 0x0006E078
	public static void SetBackgroundClip(MenuBackgroundMusic.LobbyBackgroundClip clipType)
	{
		if (MenuBackgroundMusic.sharedMusic != null)
		{
			AudioSource component = MenuBackgroundMusic.sharedMusic.GetComponent<AudioSource>();
			if (component != null)
			{
				string text = clipType.ToString().ToLower();
				if (MenuBackgroundMusic.SettedLobbyBackgrounClip.ToLower() != text && clipType != MenuBackgroundMusic.LobbyBackgroundClip.None)
				{
					Storager.setString("lobby_background_music", clipType.ToString(), true);
					AudioClip clip = Resources.Load<AudioClip>("MenuMusic/menu_music_" + text);
					MenuBackgroundMusic.sharedMusic.Stop();
					component.clip = clip;
					MenuBackgroundMusic.sharedMusic.Play();
				}
			}
		}
	}

	// Token: 0x170004BC RID: 1212
	// (get) Token: 0x06001B38 RID: 6968 RVA: 0x0006FF1C File Offset: 0x0006E11C
	public static string SettedLobbyBackgrounClip
	{
		get
		{
			return Storager.getString("lobby_background_music", true);
		}
	}

	// Token: 0x04001053 RID: 4179
	public const string KEY_LOBBY_SETTED_BG_MUSIC = "lobby_background_music";

	// Token: 0x04001054 RID: 4180
	private List<AudioSource> _customMusicStack = new List<AudioSource>();

	// Token: 0x04001055 RID: 4181
	private AudioSource currentAudioSource;

	// Token: 0x04001056 RID: 4182
	public static bool keepPlaying = false;

	// Token: 0x04001057 RID: 4183
	public static MenuBackgroundMusic sharedMusic;

	// Token: 0x04001058 RID: 4184
	private static string[] scenetsToPlayMusicOn = new string[]
	{
		Defs.MainMenuScene,
		"ConnectScene",
		"ConnectSceneSandbox",
		"SettingScene",
		"SkinEditor",
		"ChooseLevel",
		"CampaignChooseBox",
		"ProfileShop",
		"Friends",
		"Clans"
	};

	// Token: 0x02000302 RID: 770
	public enum LobbyBackgroundClip
	{
		// Token: 0x0400105A RID: 4186
		None,
		// Token: 0x0400105B RID: 4187
		Classic,
		// Token: 0x0400105C RID: 4188
		Ambient,
		// Token: 0x0400105D RID: 4189
		Modern
	}
}
