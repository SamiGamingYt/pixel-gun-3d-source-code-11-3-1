using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020002B0 RID: 688
	[Serializable]
	public class EventCallback
	{
		// Token: 0x06001579 RID: 5497 RVA: 0x00055D60 File Offset: 0x00053F60
		public void Execute(UnityEngine.Object Sender = null)
		{
			if (this.Target)
			{
				this.Target.SendMessage(this.MethodName, Sender, SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x04000CCD RID: 3277
		public MonoBehaviour Target;

		// Token: 0x04000CCE RID: 3278
		public string MethodName = string.Empty;
	}
}
