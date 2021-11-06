using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002FB RID: 763
public class LobbyDayNightController : MonoBehaviour
{
	// Token: 0x06001A73 RID: 6771 RVA: 0x0006B304 File Offset: 0x00069504
	private void Start()
	{
		DateTime now = DateTime.Now;
		this.timeDelta = this.dayLength - (float)(now.Hour * 60 * 60) + (float)(now.Minute * 60) + (float)now.Second;
		this.cicleTime = this.dayLength / 6f;
		base.StartCoroutine(this.MatColorChange());
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x0006B368 File Offset: 0x00069568
	private void Update()
	{
		this.timeDelta -= Time.deltaTime;
		if (this.timeDelta < 0f)
		{
			this.timeDelta = this.dayLength;
		}
		foreach (LobbyDayNightController.MaterialToChange materialToChange in this.matToChange)
		{
			int num = Mathf.FloorToInt(this.timeDelta / this.dayLength * 6f);
			if (num == 6)
			{
				num = 0;
			}
			int num2 = num + 1;
			if (num2 > 5)
			{
				num2 = 0;
			}
			float t = (this.timeDelta - this.cicleTime * (float)num) / this.cicleTime;
			if (materialToChange.changecolor)
			{
				materialToChange.currentColor = Color.Lerp(materialToChange.cicleColors[num], materialToChange.cicleColors[num2], t);
			}
			if (materialToChange.changeLM)
			{
				materialToChange.currentLerp = Mathf.Lerp(materialToChange.cicleLerp[num], materialToChange.cicleLerp[num2], t);
			}
			if (materialToChange.objectsToOnAtNight != null)
			{
				if ((num == 5 || num == 0) && !materialToChange.objectsIsActive)
				{
					this.ActiveGo(materialToChange.objectsToOnAtNight, true);
					materialToChange.objectsIsActive = true;
				}
				if (num > 0 && num < 5 && materialToChange.objectsIsActive)
				{
					this.ActiveGo(materialToChange.objectsToOnAtNight, false);
					materialToChange.objectsIsActive = false;
				}
			}
		}
	}

	// Token: 0x06001A75 RID: 6773 RVA: 0x0006B4D8 File Offset: 0x000696D8
	private void ActiveGo(GameObject[] go, bool active)
	{
		foreach (GameObject gameObject in go)
		{
			gameObject.SetActive(active);
		}
	}

	// Token: 0x06001A76 RID: 6774 RVA: 0x0006B508 File Offset: 0x00069708
	private IEnumerator MatColorChange()
	{
		for (;;)
		{
			yield return null;
			foreach (LobbyDayNightController.MaterialToChange mTCh in this.matToChange)
			{
				foreach (Material mat in mTCh.materials)
				{
					if (mTCh.changecolor)
					{
						mat.color = mTCh.currentColor;
					}
					if (mat.HasProperty("_Lerp"))
					{
						mat.SetFloat("_Lerp", mTCh.currentLerp);
					}
				}
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield break;
	}

	// Token: 0x04000F89 RID: 3977
	public LobbyDayNightController.MaterialToChange[] matToChange;

	// Token: 0x04000F8A RID: 3978
	private float cicleTime;

	// Token: 0x04000F8B RID: 3979
	private float timeDelta;

	// Token: 0x04000F8C RID: 3980
	private float dayLength = 86400f;

	// Token: 0x020002FC RID: 764
	[Serializable]
	public class MaterialToChange
	{
		// Token: 0x04000F8D RID: 3981
		public string description = "description";

		// Token: 0x04000F8E RID: 3982
		public Color[] cicleColors = new Color[6];

		// Token: 0x04000F8F RID: 3983
		public Material[] materials;

		// Token: 0x04000F90 RID: 3984
		public float[] cicleLerp = new float[6];

		// Token: 0x04000F91 RID: 3985
		public bool changecolor;

		// Token: 0x04000F92 RID: 3986
		public bool changeLM;

		// Token: 0x04000F93 RID: 3987
		[HideInInspector]
		public Color currentColor;

		// Token: 0x04000F94 RID: 3988
		[HideInInspector]
		public float currentLerp;

		// Token: 0x04000F95 RID: 3989
		public GameObject[] objectsToOnAtNight;

		// Token: 0x04000F96 RID: 3990
		[HideInInspector]
		public bool objectsIsActive;
	}
}
