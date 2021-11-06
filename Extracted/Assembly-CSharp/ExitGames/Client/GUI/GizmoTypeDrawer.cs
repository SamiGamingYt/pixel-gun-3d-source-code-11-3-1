using System;
using UnityEngine;

namespace ExitGames.Client.GUI
{
	// Token: 0x020003EE RID: 1006
	public class GizmoTypeDrawer
	{
		// Token: 0x060023EA RID: 9194 RVA: 0x000B2C34 File Offset: 0x000B0E34
		public static void Draw(Vector3 center, GizmoType type, Color color, float size)
		{
			Gizmos.color = color;
			switch (type)
			{
			case GizmoType.WireSphere:
				Gizmos.DrawWireSphere(center, size * 0.5f);
				break;
			case GizmoType.Sphere:
				Gizmos.DrawSphere(center, size * 0.5f);
				break;
			case GizmoType.WireCube:
				Gizmos.DrawWireCube(center, Vector3.one * size);
				break;
			case GizmoType.Cube:
				Gizmos.DrawCube(center, Vector3.one * size);
				break;
			}
		}
	}
}
