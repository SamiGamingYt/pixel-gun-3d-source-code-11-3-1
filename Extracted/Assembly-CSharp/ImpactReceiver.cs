using System;
using UnityEngine;

// Token: 0x02000655 RID: 1621
public class ImpactReceiver : MonoBehaviour
{
	// Token: 0x06003851 RID: 14417 RVA: 0x00122700 File Offset: 0x00120900
	private void Start()
	{
		this.character = base.GetComponent<CharacterController>();
	}

	// Token: 0x06003852 RID: 14418 RVA: 0x00122710 File Offset: 0x00120910
	private void Update()
	{
		if (this.impact.magnitude > 0.2f)
		{
			this.character.Move(this.impact * Time.deltaTime);
		}
		this.impact = Vector3.Lerp(this.impact, Vector3.zero, 5f * Time.deltaTime);
	}

	// Token: 0x06003853 RID: 14419 RVA: 0x00122770 File Offset: 0x00120970
	public void AddImpact(Vector3 dir, float force)
	{
		dir.Normalize();
		if (dir.y < 0f)
		{
			dir.y = -dir.y;
		}
		this.impact += dir.normalized * force / this.mass;
	}

	// Token: 0x04002940 RID: 10560
	private float mass = 1f;

	// Token: 0x04002941 RID: 10561
	private Vector3 impact = Vector3.zero;

	// Token: 0x04002942 RID: 10562
	private CharacterController character;
}
