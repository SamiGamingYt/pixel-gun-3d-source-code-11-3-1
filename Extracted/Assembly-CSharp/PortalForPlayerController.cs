using System;
using UnityEngine;

// Token: 0x02000494 RID: 1172
public sealed class PortalForPlayerController : MonoBehaviour
{
	// Token: 0x060029E3 RID: 10723 RVA: 0x000DD008 File Offset: 0x000DB208
	private void Start()
	{
		this.myPointOut = base.transform.GetChild(0);
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x000DD01C File Offset: 0x000DB21C
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name.Equals("BodyCollider") && other.transform.parent != null && other.transform.parent.gameObject.Equals(WeaponManager.sharedManager.myPlayer))
		{
			WeaponManager.sharedManager.myPlayer.transform.position = this.myDublicatePortal.myPointOut.position;
			WeaponManager.sharedManager.myPlayerMoveC.myPersonNetwork.isTeleported = true;
			float y = this.myPointOut.transform.rotation.eulerAngles.y;
			float num = this.myDublicatePortal.myPointOut.transform.rotation.eulerAngles.y;
			if (num < y)
			{
				num += 360f;
			}
			float yAngle = num - y - 180f;
			WeaponManager.sharedManager.myPlayer.transform.Rotate(0f, yAngle, 0f);
			WeaponManager.sharedManager.myPlayerMoveC.PlayPortalSound();
		}
	}

	// Token: 0x04001EFD RID: 7933
	public PortalForPlayerController myDublicatePortal;

	// Token: 0x04001EFE RID: 7934
	private Transform myPointOut;
}
