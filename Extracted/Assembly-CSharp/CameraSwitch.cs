using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004DE RID: 1246
public class CameraSwitch : MonoBehaviour
{
	// Token: 0x06002C61 RID: 11361 RVA: 0x000EB4E8 File Offset: 0x000E96E8
	private void OnEnable()
	{
		this.text.text = this.objects[this.m_CurrentActiveObject].name;
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x000EB508 File Offset: 0x000E9708
	public void NextCamera()
	{
		int num = (this.m_CurrentActiveObject + 1 < this.objects.Length) ? (this.m_CurrentActiveObject + 1) : 0;
		for (int i = 0; i < this.objects.Length; i++)
		{
			this.objects[i].SetActive(i == num);
		}
		this.m_CurrentActiveObject = num;
		this.text.text = this.objects[this.m_CurrentActiveObject].name;
	}

	// Token: 0x0400215F RID: 8543
	public GameObject[] objects;

	// Token: 0x04002160 RID: 8544
	public Text text;

	// Token: 0x04002161 RID: 8545
	private int m_CurrentActiveObject;
}
