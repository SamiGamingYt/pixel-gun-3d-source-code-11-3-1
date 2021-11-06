using System;
using Photon;
using UnityEngine;

// Token: 0x02000443 RID: 1091
[RequireComponent(typeof(PhotonView))]
public class MoveByKeys : Photon.MonoBehaviour
{
	// Token: 0x060026C4 RID: 9924 RVA: 0x000C22FC File Offset: 0x000C04FC
	public void Start()
	{
		this.isSprite = (base.GetComponent<SpriteRenderer>() != null);
		this.body2d = base.GetComponent<Rigidbody2D>();
		this.body = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x000C2334 File Offset: 0x000C0534
	public void FixedUpdate()
	{
		if (!base.photonView.isMine)
		{
			return;
		}
		if (Input.GetAxisRaw("Horizontal") < -0.1f || Input.GetAxisRaw("Horizontal") > 0.1f)
		{
			base.transform.position += Vector3.right * (this.Speed * Time.deltaTime) * Input.GetAxisRaw("Horizontal");
		}
		if (this.jumpingTime <= 0f)
		{
			if ((this.body != null || this.body2d != null) && Input.GetKey(KeyCode.Space))
			{
				this.jumpingTime = this.JumpTimeout;
				Vector2 vector = Vector2.up * this.JumpForce;
				if (this.body2d != null)
				{
					this.body2d.AddForce(vector);
				}
				else if (this.body != null)
				{
					this.body.AddForce(vector);
				}
			}
		}
		else
		{
			this.jumpingTime -= Time.deltaTime;
		}
		if (!this.isSprite && (Input.GetAxisRaw("Vertical") < -0.1f || Input.GetAxisRaw("Vertical") > 0.1f))
		{
			base.transform.position += Vector3.forward * (this.Speed * Time.deltaTime) * Input.GetAxisRaw("Vertical");
		}
	}

	// Token: 0x04001B3D RID: 6973
	public float Speed = 10f;

	// Token: 0x04001B3E RID: 6974
	public float JumpForce = 200f;

	// Token: 0x04001B3F RID: 6975
	public float JumpTimeout = 0.5f;

	// Token: 0x04001B40 RID: 6976
	private bool isSprite;

	// Token: 0x04001B41 RID: 6977
	private float jumpingTime;

	// Token: 0x04001B42 RID: 6978
	private Rigidbody body;

	// Token: 0x04001B43 RID: 6979
	private Rigidbody2D body2d;
}
