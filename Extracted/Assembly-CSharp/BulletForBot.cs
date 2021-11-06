using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200058D RID: 1421
internal sealed class BulletForBot : MonoBehaviour
{
	// Token: 0x14000046 RID: 70
	// (add) Token: 0x0600317B RID: 12667 RVA: 0x00101D78 File Offset: 0x000FFF78
	// (remove) Token: 0x0600317C RID: 12668 RVA: 0x00101D94 File Offset: 0x000FFF94
	public event BulletForBot.OnBulletDamageDelegate OnBulletDamage;

	// Token: 0x17000863 RID: 2147
	// (get) Token: 0x0600317D RID: 12669 RVA: 0x00101DB0 File Offset: 0x000FFFB0
	// (set) Token: 0x0600317E RID: 12670 RVA: 0x00101DB8 File Offset: 0x000FFFB8
	public bool needDestroyByStop { get; set; }

	// Token: 0x17000864 RID: 2148
	// (get) Token: 0x06003180 RID: 12672 RVA: 0x00101DD0 File Offset: 0x000FFFD0
	// (set) Token: 0x0600317F RID: 12671 RVA: 0x00101DC4 File Offset: 0x000FFFC4
	public bool IsUse { get; private set; }

	// Token: 0x06003181 RID: 12673 RVA: 0x00101DD8 File Offset: 0x000FFFD8
	public void StartBullet(Vector3 startPos, Vector3 endPos, float bulletSpeed, bool isFriendlyFire, bool doDamage)
	{
		this._isMoveByPhysics = false;
		this._startPos = startPos;
		this._endPos = endPos;
		this._isFrienlyFire = isFriendlyFire;
		this._bulletSpeed = bulletSpeed;
		base.transform.position = this._startPos;
		this.IsUse = true;
		base.transform.gameObject.SetActive(true);
		this._startBulletTime = Time.realtimeSinceStartup;
		base.transform.rotation = Quaternion.LookRotation(endPos - startPos);
		this.doDamage = doDamage;
	}

	// Token: 0x06003182 RID: 12674 RVA: 0x00101E5C File Offset: 0x0010005C
	private void StopBullet()
	{
		if (this.needDestroyByStop)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (this._isMoveByPhysics)
		{
			this.SetVisible(false);
		}
		else
		{
			base.transform.gameObject.SetActive(false);
		}
		base.transform.position = Vector3.zero;
		base.transform.rotation = Quaternion.identity;
		this.IsUse = false;
		if (this._isMoveByPhysics)
		{
			this.EnablePhysicsGravityControll(false);
		}
	}

	// Token: 0x06003183 RID: 12675 RVA: 0x00101EE4 File Offset: 0x001000E4
	private void OnTriggerEnter(Collider collisionObj)
	{
		this.CollisionEvent(collisionObj.gameObject);
	}

	// Token: 0x06003184 RID: 12676 RVA: 0x00101EF4 File Offset: 0x001000F4
	private void OnCollisionEnter(Collision collisionObj)
	{
		this.CollisionEvent(collisionObj.gameObject);
	}

	// Token: 0x06003185 RID: 12677 RVA: 0x00101F04 File Offset: 0x00100104
	private void CollisionEvent(GameObject collisionObj)
	{
		if (!this.IsUse)
		{
			return;
		}
		Transform root = collisionObj.transform.root;
		if (base.transform.root == root.transform.root)
		{
			return;
		}
		if (!this._isFrienlyFire && root.tag.Equals("Enemy"))
		{
			return;
		}
		if (root.tag.Equals("Player") || root.tag.Equals("Turret") || root.tag.Equals("Pet"))
		{
			this.CheckRunDamageEvent(root.gameObject);
			return;
		}
		if (this._isFrienlyFire && root.tag.Equals("Enemy"))
		{
			this.CheckRunDamageEvent(root.gameObject);
			return;
		}
		this.CheckRunDamageEvent(null);
	}

	// Token: 0x06003186 RID: 12678 RVA: 0x00101FEC File Offset: 0x001001EC
	private void CheckRunDamageEvent(GameObject target)
	{
		if (this.OnBulletDamage == null)
		{
			return;
		}
		if (this.doDamage)
		{
			this.OnBulletDamage(target, base.transform.position);
		}
		this.StopBullet();
	}

	// Token: 0x06003187 RID: 12679 RVA: 0x00102030 File Offset: 0x00100230
	private void Update()
	{
		if (!this.IsUse)
		{
			return;
		}
		if (!this._isMoveByPhysics)
		{
			Vector3 vector = this._endPos - this._startPos;
			base.transform.position += vector.normalized * this._bulletSpeed * Time.deltaTime;
		}
		if (Time.realtimeSinceStartup - this._startBulletTime >= this.lifeTime)
		{
			this.StopBullet();
		}
	}

	// Token: 0x06003188 RID: 12680 RVA: 0x001020B8 File Offset: 0x001002B8
	private void EnablePhysicsGravityControll(bool enable)
	{
		base.GetComponent<Rigidbody>().useGravity = enable;
		base.GetComponent<Rigidbody>().isKinematic = !enable;
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x001020E0 File Offset: 0x001002E0
	public void ApplyForceFroBullet(Vector3 startPos, Vector3 endPos, bool isFriendlyFire, float forceValue, Vector3 forceVector, bool doDamage)
	{
		this._isMoveByPhysics = true;
		this._isFrienlyFire = isFriendlyFire;
		this._startBulletTime = Time.realtimeSinceStartup;
		base.transform.position = startPos;
		base.transform.rotation = Quaternion.LookRotation(endPos - startPos);
		base.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
		this.SetVisible(true);
		this.EnablePhysicsGravityControll(true);
		this.IsUse = true;
		this.doDamage = doDamage;
		base.StartCoroutine(this.ApplyForce(forceVector * forceValue));
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x0010216C File Offset: 0x0010036C
	private IEnumerator ApplyForce(Vector3 force)
	{
		yield return new WaitForEndOfFrame();
		base.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		yield break;
	}

	// Token: 0x0600318B RID: 12683 RVA: 0x00102198 File Offset: 0x00100398
	private void SetVisible(bool enable)
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(enable);
		}
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().enabled = enable;
		}
		if (base.GetComponent<ParticleSystem>() != null)
		{
			base.GetComponent<ParticleSystem>().enableEmission = enable;
		}
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = enable;
		}
	}

	// Token: 0x0400247A RID: 9338
	[NonSerialized]
	public float lifeTime;

	// Token: 0x0400247B RID: 9339
	private float _bulletSpeed;

	// Token: 0x0400247C RID: 9340
	private Vector3 _startPos;

	// Token: 0x0400247D RID: 9341
	private Vector3 _endPos;

	// Token: 0x0400247E RID: 9342
	private bool _isFrienlyFire;

	// Token: 0x0400247F RID: 9343
	private float _startBulletTime;

	// Token: 0x04002480 RID: 9344
	private bool doDamage = true;

	// Token: 0x04002481 RID: 9345
	private bool _isMoveByPhysics;

	// Token: 0x0200091E RID: 2334
	// (Invoke) Token: 0x06005118 RID: 20760
	public delegate void OnBulletDamageDelegate(GameObject targetDamage, Vector3 positionDamage);
}
