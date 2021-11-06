using System;
using UnityEngine;

// Token: 0x020003D3 RID: 979
internal sealed class ObjectPictFlag : MonoBehaviour
{
	// Token: 0x0600236E RID: 9070 RVA: 0x000B0700 File Offset: 0x000AE900
	public void SetTexture(Texture _texture)
	{
		base.GetComponent<GUITexture>().texture = _texture;
	}

	// Token: 0x0600236F RID: 9071 RVA: 0x000B0710 File Offset: 0x000AE910
	private void Update()
	{
		try
		{
			this.cam = NickLabelController.currentCamera;
			if (this.cam != null)
			{
				this.camTransform = this.cam.transform;
			}
			if (this.target == null || this.cam == null)
			{
				if (Time.frameCount % 60 == 0)
				{
					Debug.Log("target == null");
				}
				base.transform.position = new Vector3(-1000f, -1000f, -1000f);
			}
			else
			{
				this.posLabel = this.cam.WorldToViewportPoint(this.target.position);
				if (this.posLabel.z >= 0f && ShopNGUIController.sharedShop != null && !ShopNGUIController.GuiActive)
				{
					base.transform.position = this.posLabel;
				}
				else
				{
					base.transform.position = new Vector3(-1000f, -1000f, -1000f);
				}
				if (this.isBaza && this.myFlagController.isBaza && this.myFlagController.flagModel.activeInHierarchy)
				{
					base.transform.position = new Vector3(-1000f, -1000f, -1000f);
				}
				if (!this.isBaza && !this.target.parent.GetComponent<FlagController>().flagModel.activeInHierarchy)
				{
					base.transform.position = new Vector3(-1000f, -1000f, -1000f);
				}
			}
		}
		catch (Exception arg)
		{
			Debug.Log("Exception in ObjectLabel: " + arg);
		}
	}

	// Token: 0x040017E4 RID: 6116
	public Camera cameraToUse;

	// Token: 0x040017E5 RID: 6117
	public Camera cam;

	// Token: 0x040017E6 RID: 6118
	public Transform target;

	// Token: 0x040017E7 RID: 6119
	public Vector3 posLabel;

	// Token: 0x040017E8 RID: 6120
	private Transform camTransform;

	// Token: 0x040017E9 RID: 6121
	public bool isBaza;

	// Token: 0x040017EA RID: 6122
	public FlagController myFlagController;
}
