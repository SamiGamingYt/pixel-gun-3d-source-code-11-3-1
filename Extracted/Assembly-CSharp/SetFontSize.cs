using System;
using System.Reflection;
using UnityEngine;

// Token: 0x0200079E RID: 1950
public class SetFontSize : MonoBehaviour
{
	// Token: 0x060045CD RID: 17869 RVA: 0x00179684 File Offset: 0x00177884
	private void Start()
	{
		this.myLabel = base.GetComponent<UILabel>();
		this.UpdateFontSize();
	}

	// Token: 0x060045CE RID: 17870 RVA: 0x00179698 File Offset: 0x00177898
	[Obfuscation(Exclude = true)]
	private void UpdateFontSize()
	{
		if (this.myLabel != null)
		{
			this.myLabel.fontSize = this.myLabel.height;
		}
	}

	// Token: 0x060045CF RID: 17871 RVA: 0x001796C4 File Offset: 0x001778C4
	private void OnEnable()
	{
		base.Invoke("UpdateFontSize", 0.05f);
	}

	// Token: 0x060045D0 RID: 17872 RVA: 0x001796D8 File Offset: 0x001778D8
	private void Update()
	{
		if (this.myLabel.fontSize != this.myLabel.height)
		{
			this.myLabel.fontSize = this.myLabel.height;
		}
	}

	// Token: 0x0400332D RID: 13101
	private UILabel myLabel;
}
