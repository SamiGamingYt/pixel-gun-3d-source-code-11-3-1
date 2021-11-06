using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000AE RID: 174
[ExecuteInEditMode]
public class Ender : MonoBehaviour
{
	// Token: 0x06000528 RID: 1320 RVA: 0x00029EDC File Offset: 0x000280DC
	private IEnumerator Start()
	{
		MainMenu.BlockInterface = true;
		this._camera = this.cam.GetComponent<Camera>();
		this._text = this.text.GetComponent<GUIText>();
		float animLength = this.enderPers.GetComponent<Animation>().GetComponent<Animation>()["Ender_AD"].length;
		yield return new WaitForSeconds(this._pauseBeforeClouds);
		for (int i = 0; i < this.clouds.Length; i++)
		{
			this.clouds[i].SetActive(true);
			if (i == this.clouds.Length - 1)
			{
				this.text.SetActive(true);
			}
			yield return new WaitForSeconds(this._pauseBetweenClouds);
		}
		yield return new WaitForSeconds(this._pauseBetweenTexts);
		this.text.transform.localPosition = new Vector3(0.375f, this.text.transform.localPosition.y, this.text.transform.localPosition.z);
		this._text.text = "See you!\nYou can\nfind me in\nFree Coins!";
		yield return new WaitForSeconds(animLength - this._pauseBeforeClouds - (float)this.clouds.Length * this._pauseBetweenClouds - this._pauseBetweenTexts);
		MainMenu.BlockInterface = false;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00029EF8 File Offset: 0x000280F8
	private void OnGUI()
	{
		if (MainMenuController.sharedController != null && MainMenuController.sharedController.stubLoading.activeSelf)
		{
			return;
		}
		GUI.enabled = true;
		GUI.depth = -10000;
		Rect position = new Rect(0f, (float)Screen.height - (float)this._camera.targetTexture.height * Defs.Coef, (float)this._camera.targetTexture.width * Defs.Coef, (float)this._camera.targetTexture.height * Defs.Coef);
		GUI.DrawTexture(position, this._camera.targetTexture);
		position.width /= 2f;
		if (GUI.Button(position, string.Empty, this.buttonStyle))
		{
			MainMenu.BlockInterface = false;
			UnityEngine.Object.Destroy(base.gameObject);
			Application.OpenURL(MainMenu.GetEndermanUrl());
		}
	}

	// Token: 0x040005A0 RID: 1440
	public GUIStyle buttonStyle;

	// Token: 0x040005A1 RID: 1441
	public GameObject enderPers;

	// Token: 0x040005A2 RID: 1442
	public GameObject cam;

	// Token: 0x040005A3 RID: 1443
	public GameObject[] clouds;

	// Token: 0x040005A4 RID: 1444
	public GameObject text;

	// Token: 0x040005A5 RID: 1445
	private Camera _camera;

	// Token: 0x040005A6 RID: 1446
	private GUIText _text;

	// Token: 0x040005A7 RID: 1447
	private readonly float _pauseBeforeClouds = 1f;

	// Token: 0x040005A8 RID: 1448
	private readonly float _pauseBetweenClouds = 0.1f;

	// Token: 0x040005A9 RID: 1449
	private readonly float _pauseBetweenTexts = 3f;
}
