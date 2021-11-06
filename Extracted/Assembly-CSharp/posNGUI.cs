using System;
using UnityEngine;

// Token: 0x02000817 RID: 2071
public class posNGUI : MonoBehaviour
{
	// Token: 0x06004B74 RID: 19316 RVA: 0x001B25D8 File Offset: 0x001B07D8
	private void Awake()
	{
		posNGUI.rootObj = base.GetComponent<UIRoot>();
		posNGUI.heightScreen = (float)Mathf.Min(1366, Mathf.RoundToInt(768f * (float)Screen.height / (float)Screen.width));
		posNGUI.scaleNGUI = 2f / posNGUI.heightScreen;
		posNGUI.nachY = posNGUI.rootObj.transform.position.y;
		posNGUI.rootObj.gameObject.transform.localScale = new Vector3(posNGUI.scaleNGUI, posNGUI.scaleNGUI, posNGUI.scaleNGUI);
	}

	// Token: 0x06004B75 RID: 19317 RVA: 0x001B266C File Offset: 0x001B086C
	public static Vector3 getPosNGUI(Vector3 tekPos)
	{
		return new Vector3(posNGUI.getPosX(tekPos.x), posNGUI.getPosY(tekPos.y), tekPos.z * posNGUI.scaleNGUI);
	}

	// Token: 0x06004B76 RID: 19318 RVA: 0x001B26A4 File Offset: 0x001B08A4
	public static float getPosX(float tekPosX)
	{
		return (tekPosX - 384f) * posNGUI.scaleNGUI;
	}

	// Token: 0x06004B77 RID: 19319 RVA: 0x001B26B4 File Offset: 0x001B08B4
	public static float getPosY(float tekPosY)
	{
		return posNGUI.nachY + (-tekPosY + posNGUI.heightScreen * 0.5f) * posNGUI.scaleNGUI;
	}

	// Token: 0x06004B78 RID: 19320 RVA: 0x001B26D0 File Offset: 0x001B08D0
	public static Vector3 getSize(Vector3 tekSize)
	{
		return new Vector3(tekSize.x * posNGUI.scaleNGUI, tekSize.y * posNGUI.scaleNGUI, tekSize.z * posNGUI.scaleNGUI);
	}

	// Token: 0x06004B79 RID: 19321 RVA: 0x001B270C File Offset: 0x001B090C
	public static float getSizeWidth(float tekWidth)
	{
		return tekWidth * posNGUI.scaleNGUI;
	}

	// Token: 0x06004B7A RID: 19322 RVA: 0x001B2718 File Offset: 0x001B0918
	public static float getSizeHeight(float tekHeight)
	{
		return tekHeight * posNGUI.scaleNGUI;
	}

	// Token: 0x06004B7B RID: 19323 RVA: 0x001B2724 File Offset: 0x001B0924
	public static Vector3 getEulerZ(float tekUgol)
	{
		return new Vector3(0f, 0f, tekUgol);
	}

	// Token: 0x06004B7C RID: 19324 RVA: 0x001B2738 File Offset: 0x001B0938
	public static void setFillRect(GameObject thisGameObj)
	{
		thisGameObj.transform.localScale = new Vector3(770f, posNGUI.heightScreen + 2f, 1f);
	}

	// Token: 0x04003A93 RID: 14995
	private static UIRoot rootObj;

	// Token: 0x04003A94 RID: 14996
	public static float scaleNGUI;

	// Token: 0x04003A95 RID: 14997
	public static float heightScreen;

	// Token: 0x04003A96 RID: 14998
	public static float nachY;
}
