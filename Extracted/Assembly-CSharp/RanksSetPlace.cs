using System;
using UnityEngine;

// Token: 0x020004AF RID: 1199
public class RanksSetPlace : MonoBehaviour
{
	// Token: 0x06002B24 RID: 11044 RVA: 0x000E30BC File Offset: 0x000E12BC
	public void SetPlace(int place)
	{
		if (place <= 3 && this.isShowCups)
		{
			this.placeLabel.text = string.Empty;
			this.cupGold.SetActive(place == 1);
			this.cupSilver.SetActive(place == 2);
			this.cupBronze.SetActive(place == 3);
		}
		else
		{
			this.cupGold.SetActive(false);
			this.cupSilver.SetActive(false);
			this.cupBronze.SetActive(false);
			this.placeLabel.text = place.ToString();
		}
	}

	// Token: 0x0400202F RID: 8239
	public UILabel placeLabel;

	// Token: 0x04002030 RID: 8240
	public GameObject cupGold;

	// Token: 0x04002031 RID: 8241
	public GameObject cupSilver;

	// Token: 0x04002032 RID: 8242
	public GameObject cupBronze;

	// Token: 0x04002033 RID: 8243
	public bool isShowCups;
}
