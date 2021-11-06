using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000099 RID: 153
public class DinamicDayCicle : MonoBehaviour
{
	// Token: 0x0600045D RID: 1117 RVA: 0x00024DEC File Offset: 0x00022FEC
	private void Start()
	{
		this.ResetColors();
		base.StartCoroutine(this.MatColorChange());
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x00024E04 File Offset: 0x00023004
	private void Update()
	{
		if (TimeGameController.sharedController != null && PhotonNetwork.room != null && !string.IsNullOrEmpty(ConnectSceneNGUIController.maxKillProperty))
		{
			if (PhotonNetwork.room.customProperties.ContainsKey(ConnectSceneNGUIController.maxKillProperty))
			{
				int num = -1;
				int.TryParse(PhotonNetwork.room.customProperties[ConnectSceneNGUIController.maxKillProperty].ToString(), out num);
				if (num < 0)
				{
					this.ResetColors();
					return;
				}
				this.matchTime = (float)num * 60f;
				if ((float)TimeGameController.sharedController.timerToEndMatch < this.matchTime)
				{
					this.timeDelta = this.matchTime - (float)TimeGameController.sharedController.timerToEndMatch;
					if (this.matchTime != this.timeDelta)
					{
						foreach (MaterialToChange materialToChange in this.matToChange)
						{
							this.cicleTime = this.matchTime / (float)materialToChange.cicleColors.Length;
							this.currentCicle = Mathf.FloorToInt(this.timeDelta / this.matchTime * (float)materialToChange.cicleColors.Length);
							this.nextCicle = Mathf.Min(this.currentCicle + 1, materialToChange.cicleColors.Length - 1);
							this.lerpFactor = (this.timeDelta - this.cicleTime * (float)this.currentCicle) / this.cicleTime;
							if (materialToChange.changecolor && this.currentCicle < materialToChange.cicleColors.Length)
							{
								materialToChange.currentColor = Color.Lerp(materialToChange.cicleColors[this.currentCicle], materialToChange.cicleColors[this.nextCicle], this.lerpFactor);
							}
							if (materialToChange.cicleLerp != null && materialToChange.cicleLerp.Length == materialToChange.cicleColors.Length && this.currentCicle < materialToChange.cicleColors.Length)
							{
								materialToChange.currentLerp = Mathf.Lerp(materialToChange.cicleLerp[this.currentCicle], materialToChange.cicleLerp[this.nextCicle], this.lerpFactor);
							}
						}
					}
				}
			}
		}
		else
		{
			this.ResetColors();
		}
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0002502C File Offset: 0x0002322C
	private void ResetColors()
	{
		foreach (MaterialToChange materialToChange in this.matToChange)
		{
			if (materialToChange.changecolor)
			{
				materialToChange.currentColor = materialToChange.cicleColors[0];
			}
			if (materialToChange.cicleLerp != null && materialToChange.cicleLerp.Length == materialToChange.cicleColors.Length)
			{
				materialToChange.currentLerp = materialToChange.cicleLerp[0];
			}
		}
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x000250A8 File Offset: 0x000232A8
	private IEnumerator MatColorChange()
	{
		for (;;)
		{
			if (this.matchTime != this.timeDelta)
			{
				foreach (MaterialToChange mTCh in this.matToChange)
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
			}
			yield return new WaitForSeconds(0.5f);
		}
		yield break;
	}

	// Token: 0x040004E1 RID: 1249
	public MaterialToChange[] matToChange;

	// Token: 0x040004E2 RID: 1250
	public float lerpFactor;

	// Token: 0x040004E3 RID: 1251
	private int nextCicle;

	// Token: 0x040004E4 RID: 1252
	private float cicleTime;

	// Token: 0x040004E5 RID: 1253
	public int currentCicle;

	// Token: 0x040004E6 RID: 1254
	private float matchTime;

	// Token: 0x040004E7 RID: 1255
	private float timeDelta;
}
