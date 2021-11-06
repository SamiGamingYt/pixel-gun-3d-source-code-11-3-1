using System;
using UnityEngine;

// Token: 0x02000447 RID: 1095
public class OnClickInstantiate : MonoBehaviour
{
	// Token: 0x060026D8 RID: 9944 RVA: 0x000C2C0C File Offset: 0x000C0E0C
	private void OnClick()
	{
		if (!PhotonNetwork.inRoom)
		{
			return;
		}
		int instantiateType = this.InstantiateType;
		if (instantiateType != 0)
		{
			if (instantiateType == 1)
			{
				PhotonNetwork.InstantiateSceneObject(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null);
			}
		}
		else
		{
			PhotonNetwork.Instantiate(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
		}
	}

	// Token: 0x060026D9 RID: 9945 RVA: 0x000C2CB4 File Offset: 0x000C0EB4
	private void OnGUI()
	{
		if (this.showGui)
		{
			GUILayout.BeginArea(new Rect((float)(Screen.width - 180), 0f, 180f, 50f));
			this.InstantiateType = GUILayout.Toolbar(this.InstantiateType, this.InstantiateTypeNames, new GUILayoutOption[0]);
			GUILayout.EndArea();
		}
	}

	// Token: 0x04001B4C RID: 6988
	public GameObject Prefab;

	// Token: 0x04001B4D RID: 6989
	public int InstantiateType;

	// Token: 0x04001B4E RID: 6990
	private string[] InstantiateTypeNames = new string[]
	{
		"Mine",
		"Scene"
	};

	// Token: 0x04001B4F RID: 6991
	public bool showGui;
}
