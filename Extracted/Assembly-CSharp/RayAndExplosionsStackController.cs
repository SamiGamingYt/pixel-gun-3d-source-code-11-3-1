using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
public class RayAndExplosionsStackController : MonoBehaviour
{
	// Token: 0x06002B71 RID: 11121 RVA: 0x000E4F4C File Offset: 0x000E314C
	private void Awake()
	{
		RayAndExplosionsStackController.sharedController = this;
		this.mytranform = base.GetComponent<Transform>();
	}

	// Token: 0x06002B72 RID: 11122 RVA: 0x000E4F60 File Offset: 0x000E3160
	public GameObject GetObjectFromName(string _name)
	{
		GameObject gameObject = null;
		bool flag = this.gameObjects.ContainsKey(_name);
		if (flag)
		{
			while (this.gameObjects[_name].Count > 0 && this.gameObjects[_name][0] == null)
			{
				this.gameObjects[_name].RemoveAt(0);
			}
		}
		if (flag && this.gameObjects[_name].Count > 0)
		{
			gameObject = this.gameObjects[_name][0];
			this.gameObjects[_name].RemoveAt(0);
			gameObject.SetActive(true);
		}
		else
		{
			GameObject gameObject2 = Resources.Load(_name) as GameObject;
			if (gameObject2 != null)
			{
				gameObject = (UnityEngine.Object.Instantiate(gameObject2, Vector3.down * 10000f, Quaternion.identity) as GameObject);
				gameObject.GetComponent<Transform>().parent = this.mytranform;
				gameObject.GetComponent<RayAndExplosionsStackItem>().myName = _name;
			}
		}
		if (gameObject == null && Application.isEditor)
		{
			Debug.LogError("GameOblect " + _name + " in RayAndExplosionsStackController not create!!!");
		}
		return gameObject;
	}

	// Token: 0x06002B73 RID: 11123 RVA: 0x000E509C File Offset: 0x000E329C
	public void ReturnObjectFromName(GameObject returnObject, string _name)
	{
		returnObject.GetComponent<Transform>().position = Vector3.down * 10000f;
		returnObject.SetActive(false);
		returnObject.transform.parent = this.mytranform;
		if (!this.gameObjects.ContainsKey(_name))
		{
			this.gameObjects.Add(_name, new List<GameObject>());
		}
		if (!this.timeUseGameObjects.ContainsKey(_name))
		{
			this.timeUseGameObjects.Add(_name, Time.realtimeSinceStartup);
		}
		else
		{
			this.timeUseGameObjects[_name] = Time.realtimeSinceStartup;
		}
		this.gameObjects[_name].Add(returnObject);
	}

	// Token: 0x06002B74 RID: 11124 RVA: 0x000E5148 File Offset: 0x000E3348
	private void OnDestroy()
	{
		RayAndExplosionsStackController.sharedController = null;
	}

	// Token: 0x0400207C RID: 8316
	public static RayAndExplosionsStackController sharedController;

	// Token: 0x0400207D RID: 8317
	private Dictionary<string, List<GameObject>> gameObjects = new Dictionary<string, List<GameObject>>();

	// Token: 0x0400207E RID: 8318
	private Dictionary<string, float> timeUseGameObjects = new Dictionary<string, float>();

	// Token: 0x0400207F RID: 8319
	public Transform mytranform;
}
