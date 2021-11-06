using System;
using UnityEngine;

// Token: 0x02000656 RID: 1622
public class ImpactReceiverTrampoline : MonoBehaviour
{
	// Token: 0x06003855 RID: 14421 RVA: 0x001227F0 File Offset: 0x001209F0
	private void Start()
	{
		this.character = base.GetComponent<CharacterController>();
	}

	// Token: 0x06003856 RID: 14422 RVA: 0x00122800 File Offset: 0x00120A00
	private void Update()
	{
		if (this.impact.magnitude > 0.2f)
		{
			this.character.Move(this.impact * Time.deltaTime);
		}
		else
		{
			UnityEngine.Object.Destroy(this);
		}
		this.impact = Vector3.Lerp(this.impact, Vector3.zero, 1f * Time.deltaTime);
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x0012286C File Offset: 0x00120A6C
	public void AddImpact(Vector3 dir, float force)
	{
		this.impact += dir.normalized * force / this.mass;
	}

	// Token: 0x04002943 RID: 10563
	private float mass = 1f;

	// Token: 0x04002944 RID: 10564
	private Vector3 impact = Vector3.zero;

	// Token: 0x04002945 RID: 10565
	private CharacterController character;
}
