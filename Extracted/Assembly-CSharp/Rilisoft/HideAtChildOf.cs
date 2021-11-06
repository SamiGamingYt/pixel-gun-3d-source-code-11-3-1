using System;
using System.Collections.Generic;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000654 RID: 1620
	public class HideAtChildOf : MonoBehaviour
	{
		// Token: 0x0600384F RID: 14415 RVA: 0x0012262C File Offset: 0x0012082C
		private void Start()
		{
			if (this._rootObjectName.IsNullOrEmpty())
			{
				return;
			}
			this._rootObjectName = this._rootObjectName.ToLower();
			IEnumerable<GameObject> enumerable = base.gameObject.Ancestors();
			foreach (GameObject gameObject in enumerable)
			{
				if (gameObject.name.ToLower() == this._rootObjectName)
				{
					base.gameObject.SetActive(false);
					break;
				}
			}
		}

		// Token: 0x0400293F RID: 10559
		[SerializeField]
		private string _rootObjectName;
	}
}
