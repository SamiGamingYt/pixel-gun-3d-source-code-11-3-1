using System;
using Photon;
using UnityEngine;

// Token: 0x0200045C RID: 1116
[RequireComponent(typeof(PhotonView))]
public class ShowInfoOfPlayer : Photon.MonoBehaviour
{
	// Token: 0x06002736 RID: 10038 RVA: 0x000C43F4 File Offset: 0x000C25F4
	private void Start()
	{
		if (this.font == null)
		{
			this.font = (Font)Resources.FindObjectsOfTypeAll(typeof(Font))[0];
			Debug.LogWarning("No font defined. Found font: " + this.font);
		}
		if (this.tm == null)
		{
			this.textGo = new GameObject("3d text");
			this.textGo.transform.parent = base.gameObject.transform;
			this.textGo.transform.localPosition = Vector3.zero;
			MeshRenderer meshRenderer = this.textGo.AddComponent<MeshRenderer>();
			meshRenderer.material = this.font.material;
			this.tm = this.textGo.AddComponent<TextMesh>();
			this.tm.font = this.font;
			this.tm.anchor = TextAnchor.MiddleCenter;
			if (this.CharacterSize > 0f)
			{
				this.tm.characterSize = this.CharacterSize;
			}
		}
	}

	// Token: 0x06002737 RID: 10039 RVA: 0x000C4500 File Offset: 0x000C2700
	private void Update()
	{
		bool flag = !this.DisableOnOwnObjects || base.photonView.isMine;
		if (this.textGo != null)
		{
			this.textGo.SetActive(flag);
		}
		if (!flag)
		{
			return;
		}
		PhotonPlayer owner = base.photonView.owner;
		if (owner != null)
		{
			this.tm.text = ((!string.IsNullOrEmpty(owner.name)) ? owner.name : ("player" + owner.ID));
		}
		else if (base.photonView.isSceneView)
		{
			this.tm.text = "scn";
		}
		else
		{
			this.tm.text = "n/a";
		}
	}

	// Token: 0x04001B7D RID: 7037
	private GameObject textGo;

	// Token: 0x04001B7E RID: 7038
	private TextMesh tm;

	// Token: 0x04001B7F RID: 7039
	public float CharacterSize;

	// Token: 0x04001B80 RID: 7040
	public Font font;

	// Token: 0x04001B81 RID: 7041
	public bool DisableOnOwnObjects;
}
