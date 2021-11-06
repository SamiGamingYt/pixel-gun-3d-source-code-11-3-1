using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000690 RID: 1680
	public class LeprechauntAnimatorStateBehaviour : StateMachineBehaviour
	{
		// Token: 0x06003AC4 RID: 15044 RVA: 0x0012FDB4 File Offset: 0x0012DFB4
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			base.OnStateExit(animator, stateInfo, layerIndex);
			if (LeprechauntLobbyView.Instance == null)
			{
				return;
			}
			LeprechauntLobbyView.Instance.OnAnimatorStateExit(this._stateName);
		}

		// Token: 0x04002B65 RID: 11109
		[SerializeField]
		private string _stateName;
	}
}
