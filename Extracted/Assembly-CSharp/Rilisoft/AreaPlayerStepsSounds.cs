using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000561 RID: 1377
	public class AreaPlayerStepsSounds : AreaBase
	{
		// Token: 0x06002FCC RID: 12236 RVA: 0x000F9BC4 File Offset: 0x000F7DC4
		public override void CheckIn(GameObject to)
		{
			base.CheckIn(to);
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x000F9BD0 File Offset: 0x000F7DD0
		public override void CheckOut(GameObject from)
		{
			base.CheckOut(from);
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x000F9BDC File Offset: 0x000F7DDC
		private SkinName GetSoundsComponent(GameObject go)
		{
			int hashCode = go.GetHashCode();
			if (AreaPlayerStepsSounds._soundsComponents.ContainsKey(hashCode))
			{
				return AreaPlayerStepsSounds._soundsComponents[hashCode];
			}
			SkinName componentInChildren = go.Ancestors().First((GameObject a) => a.Parent() == null).GetComponentInChildren<SkinName>();
			AreaPlayerStepsSounds._soundsComponents.Add(hashCode, componentInChildren);
			return componentInChildren;
		}

		// Token: 0x04002323 RID: 8995
		[SerializeField]
		private PlayerStepsSoundsData _sounds;

		// Token: 0x04002324 RID: 8996
		[SerializeField]
		[ReadOnly]
		private PlayerStepsSoundsData _soundsOriginal;

		// Token: 0x04002325 RID: 8997
		private static readonly Dictionary<int, SkinName> _soundsComponents = new Dictionary<int, SkinName>();
	}
}
