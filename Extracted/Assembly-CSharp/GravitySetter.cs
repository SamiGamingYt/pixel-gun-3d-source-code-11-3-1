using System;
using Rilisoft;
using UnityEngine;

// Token: 0x0200028B RID: 651
public class GravitySetter : MonoBehaviour
{
	// Token: 0x060014D6 RID: 5334 RVA: 0x00052854 File Offset: 0x00050A54
	private void OnLevelWasLoaded(int lev)
	{
		if (SceneLoader.ActiveSceneName.Equals("Space"))
		{
			Physics.gravity = new Vector3(0f, GravitySetter.spaceBaseGravity, 0f);
		}
		else if (SceneLoader.ActiveSceneName.Equals("Matrix"))
		{
			Physics.gravity = new Vector3(0f, GravitySetter.matrixGravity, 0f);
		}
		else
		{
			Physics.gravity = new Vector3(0f, GravitySetter.normalGravity, 0f);
		}
	}

	// Token: 0x04000C27 RID: 3111
	public static readonly float normalGravity = -9.81f;

	// Token: 0x04000C28 RID: 3112
	public static readonly float spaceBaseGravity = -6.54f;

	// Token: 0x04000C29 RID: 3113
	public static readonly float matrixGravity = -4.9049997f;
}
