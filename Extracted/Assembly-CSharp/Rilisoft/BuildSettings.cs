using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200004A RID: 74
	public static class BuildSettings
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000201 RID: 513 RVA: 0x000131FC File Offset: 0x000113FC
		public static RuntimePlatform BuildTargetPlatform
		{
			get
			{
				switch (Application.platform)
				{
				case RuntimePlatform.MetroPlayerX86:
				case RuntimePlatform.MetroPlayerX64:
				case RuntimePlatform.MetroPlayerARM:
					return RuntimePlatform.MetroPlayerX64;
				default:
					return Application.platform;
				}
			}
		}
	}
}
