using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class AppsFlyerTrackerCallbacks : MonoBehaviour
{
	// Token: 0x06000060 RID: 96 RVA: 0x00004A38 File Offset: 0x00002C38
	private void Start()
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks on Start");
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00004A44 File Offset: 0x00002C44
	private void Update()
	{
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00004A48 File Offset: 0x00002C48
	public void didReceiveConversionData(string conversionData)
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks:: got conversion data = " + conversionData);
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00004A5C File Offset: 0x00002C5C
	public void didReceiveConversionDataWithError(string error)
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks:: got conversion data error = " + error);
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00004A70 File Offset: 0x00002C70
	public void didFinishValidateReceipt(string validateResult)
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks:: got didFinishValidateReceipt  = " + validateResult);
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00004A84 File Offset: 0x00002C84
	public void didFinishValidateReceiptWithError(string error)
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks:: got idFinishValidateReceiptWithError error = " + error);
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00004A98 File Offset: 0x00002C98
	public void onAppOpenAttribution(string validateResult)
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks:: got onAppOpenAttribution  = " + validateResult);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00004AAC File Offset: 0x00002CAC
	public void onAppOpenAttributionFailure(string error)
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks:: got onAppOpenAttributionFailure error = " + error);
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00004AC0 File Offset: 0x00002CC0
	public void onInAppBillingSuccess()
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks:: got onInAppBillingSuccess succcess");
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00004ACC File Offset: 0x00002CCC
	public void onInAppBillingFailure(string error)
	{
		MonoBehaviour.print("AppsFlyerTrackerCallbacks:: got onInAppBillingFailure error = " + error);
	}
}
