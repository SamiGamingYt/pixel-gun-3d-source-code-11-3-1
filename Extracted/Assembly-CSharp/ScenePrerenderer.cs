using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
public class ScenePrerenderer : MonoBehaviour
{
	// Token: 0x06002C7A RID: 11386 RVA: 0x000EBCD8 File Offset: 0x000E9ED8
	private void Awake()
	{
		this._rt = new RenderTexture(32, 32, 24);
		this._rt.Create();
		this.activeCamera.targetTexture = this._rt;
		this.activeCamera.useOcclusionCulling = false;
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x000EBD20 File Offset: 0x000E9F20
	private void Start()
	{
		this.Render_();
	}

	// Token: 0x06002C7C RID: 11388 RVA: 0x000EBD28 File Offset: 0x000E9F28
	private void Render_()
	{
		List<GameObject> zombiePrefabs = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>().zombiePrefabs;
		GameObject[] array = new GameObject[zombiePrefabs.Count];
		int num = 0;
		foreach (GameObject gameObject in zombiePrefabs)
		{
			GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(gameObject.transform.GetChild(0).gameObject, new Vector3(base.transform.position.x, base.transform.position.y - 20f, base.transform.position.z), gameObject.transform.GetChild(0).gameObject.transform.rotation);
			string text = "(Clone)";
			int num2 = gameObject2.name.IndexOf(text);
			gameObject2.name = ((num2 >= 0) ? gameObject2.name.Remove(num2, text.Length) : gameObject2.name);
			gameObject2.transform.parent = base.transform.parent;
			SkinsController.GetSkinForObj(gameObject2);
			array[num] = gameObject2;
			num++;
		}
		this.activeCamera.Render();
		RenderTexture.active = this._rt;
		this.activeCamera.targetTexture = null;
		RenderTexture.active = null;
		foreach (GameObject obj in this.objsToRender)
		{
			UnityEngine.Object.Destroy(obj);
		}
		UnityEngine.Object.Destroy(base.transform.parent.parent.gameObject);
		UnityEngine.Object.Destroy(this.activeCamera);
	}

	// Token: 0x0400218B RID: 8587
	public Camera activeCamera;

	// Token: 0x0400218C RID: 8588
	private RenderTexture _rt;

	// Token: 0x0400218D RID: 8589
	public bool FinishPrerendering;

	// Token: 0x0400218E RID: 8590
	public GameObject[] objsToRender;

	// Token: 0x0400218F RID: 8591
	private GameObject _enemiesToRender;
}
