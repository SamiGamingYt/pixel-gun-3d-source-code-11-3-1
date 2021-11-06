using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
internal sealed class ABTestInfoInConsol : MonoBehaviour
{
	// Token: 0x06000002 RID: 2 RVA: 0x000020F4 File Offset: 0x000002F4
	private void Update()
	{
		UILabel component = base.GetComponent<UILabel>();
		string text = "Текущие кагорты: ";
		if (Defs.abTestBalansCohort != Defs.ABTestCohortsType.NONE && Defs.abTestBalansCohort != Defs.ABTestCohortsType.SKIP)
		{
			text = text + Defs.abTestBalansCohortName + " ";
		}
		if (Defs.cohortABTestAdvert != Defs.ABTestCohortsType.NONE && Defs.cohortABTestAdvert != Defs.ABTestCohortsType.SKIP)
		{
			text = text + FriendsController.configNameABTestAdvert + " ";
		}
		foreach (ABTestBase abtestBase in ABTestController.currentABTests)
		{
			if (abtestBase.cohort != ABTestController.ABTestCohortsType.NONE && abtestBase.cohort != ABTestController.ABTestCohortsType.SKIP)
			{
				text = text + abtestBase.cohortName + " ";
			}
		}
		component.text = text;
	}
}
