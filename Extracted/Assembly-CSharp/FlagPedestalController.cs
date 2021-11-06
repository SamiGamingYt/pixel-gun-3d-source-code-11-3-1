using System;
using UnityEngine;

// Token: 0x0200011B RID: 283
public class FlagPedestalController : MonoBehaviour
{
	// Token: 0x06000839 RID: 2105 RVA: 0x00031E34 File Offset: 0x00030034
	public void SetColor(int _color)
	{
		if (_color == 1)
		{
			this.BluePedestal.SetActive(true);
			this.RedPedestal.SetActive(false);
		}
		else if (_color == 2)
		{
			this.BluePedestal.SetActive(false);
			this.RedPedestal.SetActive(true);
		}
		else
		{
			this.BluePedestal.SetActive(false);
			this.RedPedestal.SetActive(false);
		}
	}

	// Token: 0x040006D6 RID: 1750
	public GameObject BluePedestal;

	// Token: 0x040006D7 RID: 1751
	public GameObject RedPedestal;
}
