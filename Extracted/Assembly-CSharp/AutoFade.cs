using System;
using System.Collections;
using Rilisoft;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000567 RID: 1383
public class AutoFade : MonoBehaviour
{
	// Token: 0x17000843 RID: 2115
	// (get) Token: 0x06002FE8 RID: 12264 RVA: 0x000FA37C File Offset: 0x000F857C
	private static AutoFade Instance
	{
		get
		{
			if (AutoFade.m_Instance == null)
			{
				AutoFade.m_Instance = new GameObject("AutoFade").AddComponent<AutoFade>();
			}
			return AutoFade.m_Instance;
		}
	}

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x06002FE9 RID: 12265 RVA: 0x000FA3A8 File Offset: 0x000F85A8
	public static bool Fading
	{
		get
		{
			return AutoFade.Instance.m_Fading;
		}
	}

	// Token: 0x06002FEA RID: 12266 RVA: 0x000FA3B4 File Offset: 0x000F85B4
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		AutoFade.m_Instance = this;
		Shader shader = Shader.Find("Mobile/Particles/Alpha Blended");
		this.m_Material = new Material(shader);
	}

	// Token: 0x06002FEB RID: 12267 RVA: 0x000FA3E4 File Offset: 0x000F85E4
	private void DrawQuad(Color aColor, float aAlpha)
	{
		if (ShopNGUIController.GuiActive || BankController.Instance.uiRoot.gameObject.activeInHierarchy)
		{
			return;
		}
		aColor.a = aAlpha;
		if (this.m_Material.SetPass(0))
		{
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(7);
			GL.Color(aColor);
			GL.Vertex3(0f, 0f, -1f);
			GL.Vertex3(0f, 1f, -1f);
			GL.Vertex3(1f, 1f, -1f);
			GL.Vertex3(1f, 0f, -1f);
			GL.End();
			GL.PopMatrix();
		}
		else
		{
			Debug.LogWarning("Couldnot set pass for material.");
		}
	}

	// Token: 0x06002FEC RID: 12268 RVA: 0x000FA4B0 File Offset: 0x000F86B0
	private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor, bool collectGrabage)
	{
		float t = 0f;
		while (t < 1f)
		{
			yield return new WaitForEndOfFrame();
			t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
			this.DrawQuad(aColor, t);
		}
		if (collectGrabage)
		{
			GC.Collect();
		}
		if (this.isLoadScene)
		{
			if (this.m_LevelName != string.Empty)
			{
				Singleton<SceneLoader>.Instance.LoadScene(this.m_LevelName, LoadSceneMode.Single);
			}
		}
		else
		{
			while (this.killedTime > 0f)
			{
				this.killedTime -= Time.deltaTime;
				this.DrawQuad(aColor, t);
				yield return new WaitForEndOfFrame();
			}
		}
		while (t > 0f)
		{
			if (Mathf.Abs(aFadeInTime) < 1E-06f)
			{
				break;
			}
			t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
			this.DrawQuad(aColor, t);
			yield return new WaitForEndOfFrame();
		}
		this.m_Fading = false;
		yield break;
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x000FA508 File Offset: 0x000F8708
	private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor, bool collectGarbage = false)
	{
		this.m_Fading = true;
		base.StartCoroutine(this.Fade(aFadeOutTime, aFadeInTime, aColor, collectGarbage));
	}

	// Token: 0x06002FEE RID: 12270 RVA: 0x000FA524 File Offset: 0x000F8724
	public static void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		if (AutoFade.Fading)
		{
			return;
		}
		AutoFade.Instance.isLoadScene = true;
		AutoFade.Instance.m_LevelName = aLevelName;
		AutoFade.Instance.StartFade(aFadeOutTime, aFadeInTime, aColor, false);
	}

	// Token: 0x06002FEF RID: 12271 RVA: 0x000FA560 File Offset: 0x000F8760
	public static void fadeKilled(float aFadeOutTime, float aFadeKilledTime, float aFadeInTime, Color aColor)
	{
		if (AutoFade.Fading)
		{
			return;
		}
		AutoFade.Instance.isLoadScene = false;
		AutoFade.Instance.killedTime = aFadeKilledTime;
		AutoFade.Instance.StartFade(aFadeOutTime, aFadeInTime, aColor, true);
	}

	// Token: 0x04002335 RID: 9013
	private static AutoFade m_Instance;

	// Token: 0x04002336 RID: 9014
	private Material m_Material;

	// Token: 0x04002337 RID: 9015
	private string m_LevelName = string.Empty;

	// Token: 0x04002338 RID: 9016
	private bool m_Fading;

	// Token: 0x04002339 RID: 9017
	private bool isLoadScene = true;

	// Token: 0x0400233A RID: 9018
	private float killedTime;
}
