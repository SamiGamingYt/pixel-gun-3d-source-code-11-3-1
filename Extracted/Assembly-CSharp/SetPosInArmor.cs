using System;
using UnityEngine;

// Token: 0x020007A3 RID: 1955
public class SetPosInArmor : MonoBehaviour
{
	// Token: 0x060045DB RID: 17883 RVA: 0x00179C0C File Offset: 0x00177E0C
	public void SetPosition()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
			base.transform.rotation = this.target.rotation;
		}
	}

	// Token: 0x060045DC RID: 17884 RVA: 0x00179C58 File Offset: 0x00177E58
	private void Start()
	{
		this.myTransform = base.transform;
	}

	// Token: 0x060045DD RID: 17885 RVA: 0x00179C68 File Offset: 0x00177E68
	private void Update()
	{
		if (this.target != null)
		{
			this.SetPosition();
		}
		else if (this.myTransform.root.GetComponent<SkinName>() != null && this.myTransform.root.GetComponent<SkinName>().playerMoveC.transform.childCount > 0 && this.myTransform.root.GetComponent<SkinName>().playerMoveC.transform.GetChild(0).GetComponent<WeaponSounds>() != null)
		{
			if (base.gameObject.name.Equals("Armor_Arm_Left"))
			{
				this.target = this.myTransform.root.GetComponent<SkinName>().playerMoveC.transform.GetChild(0).GetComponent<WeaponSounds>().LeftArmorHand;
			}
			else
			{
				this.target = this.myTransform.root.GetComponent<SkinName>().playerMoveC.transform.GetChild(0).GetComponent<WeaponSounds>().RightArmorHand;
			}
		}
	}

	// Token: 0x04003338 RID: 13112
	public Transform target;

	// Token: 0x04003339 RID: 13113
	private Transform myTransform;
}
