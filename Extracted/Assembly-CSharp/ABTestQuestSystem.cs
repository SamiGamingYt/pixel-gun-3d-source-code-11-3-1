using System;

// Token: 0x020004F0 RID: 1264
public class ABTestQuestSystem : ABTestBase
{
	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x000EC5F8 File Offset: 0x000EA7F8
	public override string currentFolder
	{
		get
		{
			return "QuestSystem";
		}
	}

	// Token: 0x06002CA4 RID: 11428 RVA: 0x000EC600 File Offset: 0x000EA800
	protected override void ApplyState(ABTestController.ABTestCohortsType _state, object settingsB)
	{
		base.ApplyState(_state, settingsB);
		QuestSystem.Instance.Enabled = (_state != ABTestController.ABTestCohortsType.B);
	}
}
