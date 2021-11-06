using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004EE RID: 1262
public class ABTestController : MonoBehaviour
{
	// Token: 0x170007A2 RID: 1954
	// (get) Token: 0x06002C9C RID: 11420 RVA: 0x000EC4B4 File Offset: 0x000EA6B4
	public bool isRunABTest
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x000EC4B8 File Offset: 0x000EA6B8
	private void Start()
	{
		ABTestController.currentABTests.Add(new ABTestQuestSystem());
		if (TrainingController.TrainingCompleted)
		{
			this.SkipAllNotStartedTests();
		}
		this.InitAllABTests();
		base.StartCoroutine(this.UpdateConfigsAllABTests());
	}

	// Token: 0x06002C9E RID: 11422 RVA: 0x000EC4F8 File Offset: 0x000EA6F8
	private void SkipAllNotStartedTests()
	{
		foreach (ABTestBase abtestBase in ABTestController.currentABTests)
		{
			if (abtestBase.cohort == ABTestController.ABTestCohortsType.NONE)
			{
				abtestBase.cohort = ABTestController.ABTestCohortsType.SKIP;
			}
		}
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x000EC568 File Offset: 0x000EA768
	private void InitAllABTests()
	{
		foreach (ABTestBase abtestBase in ABTestController.currentABTests)
		{
			abtestBase.InitTest();
		}
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x000EC5CC File Offset: 0x000EA7CC
	private IEnumerator UpdateConfigsAllABTests()
	{
		foreach (ABTestBase abtest in ABTestController.currentABTests)
		{
			abtest.UpdateABTestConfig();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x000EC5E0 File Offset: 0x000EA7E0
	private void OnApplicationPause(bool pause)
	{
		base.StartCoroutine(this.UpdateConfigsAllABTests());
	}

	// Token: 0x04002199 RID: 8601
	public static List<ABTestBase> currentABTests = new List<ABTestBase>();

	// Token: 0x0400219A RID: 8602
	public static bool useBuffSystem;

	// Token: 0x020004EF RID: 1263
	public enum ABTestCohortsType
	{
		// Token: 0x0400219C RID: 8604
		NONE,
		// Token: 0x0400219D RID: 8605
		A,
		// Token: 0x0400219E RID: 8606
		B,
		// Token: 0x0400219F RID: 8607
		SKIP
	}
}
