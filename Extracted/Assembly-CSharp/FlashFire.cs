using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class FlashFire : MonoBehaviour
{
	// Token: 0x0600083B RID: 2107 RVA: 0x00031EB8 File Offset: 0x000300B8
	private void Awake()
	{
		this.ws = base.GetComponent<WeaponSounds>();
	}

	// Token: 0x0600083C RID: 2108 RVA: 0x00031EC8 File Offset: 0x000300C8
	private void Start()
	{
		if (this.gunFlashObj == null)
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				bool flag = false;
				if (transform.gameObject.name.Equals("BulletSpawnPoint"))
				{
					foreach (object obj2 in transform)
					{
						Transform transform2 = (Transform)obj2;
						if (transform2.gameObject.name.Equals("GunFlash"))
						{
							flag = true;
							this.gunFlashObj = transform2.gameObject;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		WeaponManager.SetGunFlashActive(this.gunFlashObj, false);
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00032000 File Offset: 0x00030200
	private void Update()
	{
		if (this.activeTime > 0f)
		{
			this.activeTime -= Time.deltaTime;
			if (this.activeTime <= 0f)
			{
				WeaponManager.SetGunFlashActive(this.gunFlashObj, false);
			}
		}
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x0003204C File Offset: 0x0003024C
	public void fire(Player_move_c moveC)
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.fireCourotine(moveC));
		}
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00032078 File Offset: 0x00030278
	public IEnumerator fireCourotine(Player_move_c moveC)
	{
		WeaponManager.SetGunFlashActive(this.gunFlashObj, true);
		this.activeTime = this.timeFireAction;
		if (this.ws != null && this.ws.railgun && this.gunFlashObj != null)
		{
			Ray ray = new Ray(this.gunFlashObj.transform.parent.position, this.gunFlashObj.transform.parent.parent.forward);
			bool isReflection = false;
			int _countReflection = 0;
			if (this.ws.countReflectionRay == 1)
			{
				WeaponManager.AddRay(ray.origin, ray.direction, this.ws.railName, 150f);
			}
			else
			{
				do
				{
					Player_move_c.RayHitsInfo rayHitsInfo = moveC.GetHitsFromRay(ray, false);
					bool isOneRayOrFirstNoReflection = _countReflection == 0 && !rayHitsInfo.obstacleFound;
					Vector3 dirRay = ray.direction;
					float lenRay = (!isOneRayOrFirstNoReflection) ? rayHitsInfo.lenRay : 150f;
					WeaponManager.AddRay(ray.origin, dirRay, this.ws.railName, lenRay);
					if (rayHitsInfo.obstacleFound)
					{
						ray = rayHitsInfo.rayReflect;
						isReflection = true;
					}
					yield return new WaitForSeconds((float)_countReflection * 0.05f);
					_countReflection++;
				}
				while (isReflection && _countReflection < this.ws.countReflectionRay);
			}
		}
		yield break;
	}

	// Token: 0x040006D8 RID: 1752
	public GameObject gunFlashObj;

	// Token: 0x040006D9 RID: 1753
	public float timeFireAction = 0.2f;

	// Token: 0x040006DA RID: 1754
	private float activeTime;

	// Token: 0x040006DB RID: 1755
	private WeaponSounds ws;
}
