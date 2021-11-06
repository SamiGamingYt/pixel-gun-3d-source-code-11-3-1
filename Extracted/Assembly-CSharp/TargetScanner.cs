using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x020006FE RID: 1790
public class TargetScanner : MonoBehaviour
{
	// Token: 0x06003E37 RID: 15927 RVA: 0x0014DBF0 File Offset: 0x0014BDF0
	private void OnEnable()
	{
		base.StartCoroutine(this.CheckTargets());
	}

	// Token: 0x06003E38 RID: 15928 RVA: 0x0014DC00 File Offset: 0x0014BE00
	private IEnumerator CheckTargets()
	{
		if (Defs.isDaterRegim)
		{
			yield break;
		}
		for (;;)
		{
			GameObject closestTargetObj = null;
			float closestTarget = float.MaxValue;
			Initializer.TargetsList targets = new Initializer.TargetsList(WeaponManager.sharedManager.myPlayerMoveC, false, false);
			foreach (Transform enemy in targets)
			{
				if (!(enemy == null) && !(enemy == base.gameObject) && !(enemy == WeaponManager.sharedManager.myPlayer))
				{
					Vector3 direction = enemy.position - base.transform.position;
					Vector3 lookPoint = (!(this.LoopPoint != null)) ? base.transform.position : this.LoopPoint.position;
					float targetDistance = direction.sqrMagnitude;
					if ((targetDistance < closestTarget && targetDistance < Mathf.Pow(this.DetectRadius, 2f)) || Defs.isDaterRegim)
					{
						Vector3 popravochka = Vector3.zero;
						BoxCollider _collider = enemy.GetComponent<BoxCollider>();
						if (_collider != null)
						{
							popravochka = _collider.center;
						}
						Ray ray = new Ray(lookPoint, enemy.position + popravochka - lookPoint);
						int mask = Tools.AllWithoutDamageCollidersMaskAndWithoutRocket & ~(1 << LayerMask.NameToLayer("Pets"));
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit, this.DetectRadius, mask) && (hit.collider.gameObject == enemy.gameObject || (hit.collider.gameObject.transform.parent != null && (hit.collider.gameObject.transform.parent.Equals(enemy) || hit.collider.gameObject.transform.parent.Equals(enemy.parent)))))
						{
							closestTarget = targetDistance;
							closestTargetObj = enemy.gameObject;
						}
					}
					yield return null;
				}
			}
			this.Target = closestTargetObj;
			yield return new WaitForSeconds(this.UpdateFrequency);
		}
		yield break;
	}

	// Token: 0x04002DFD RID: 11773
	public float DetectRadius = 30f;

	// Token: 0x04002DFE RID: 11774
	[Range(0f, 2f)]
	public float UpdateFrequency = 0.3f;

	// Token: 0x04002DFF RID: 11775
	[ReadOnly]
	public GameObject Target;

	// Token: 0x04002E00 RID: 11776
	public Transform LoopPoint;
}
