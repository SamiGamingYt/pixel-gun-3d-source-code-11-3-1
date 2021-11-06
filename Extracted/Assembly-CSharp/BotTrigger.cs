using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200057E RID: 1406
public class BotTrigger : MonoBehaviour
{
	// Token: 0x060030C2 RID: 12482 RVA: 0x000FE088 File Offset: 0x000FC288
	private void Awake()
	{
		if (Defs.isCOOP)
		{
			base.enabled = false;
		}
	}

	// Token: 0x060030C3 RID: 12483 RVA: 0x000FE09C File Offset: 0x000FC29C
	private void Start()
	{
		this.myTransform = base.transform;
		using (IEnumerator enumerator = base.transform.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				this._modelChild = transform.gameObject;
			}
		}
		this._soundClips = this._modelChild.GetComponent<Sounds>();
		this._eai = base.GetComponent<BotAI>();
		this._player = GameObject.FindGameObjectWithTag("Player");
		if (this._player != null)
		{
			this._playerMoveC = this._player.GetComponent<SkinName>().playerMoveC;
		}
	}

	// Token: 0x060030C4 RID: 12484 RVA: 0x000FE178 File Offset: 0x000FC378
	private void Update()
	{
		if (!this.shouldDetectPlayer)
		{
			return;
		}
		if (!this._entered)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Turret");
			if (gameObject != null && gameObject.GetComponent<TurretController>() != null && (gameObject.GetComponent<TurretController>().isKilled || !gameObject.GetComponent<TurretController>().isRun))
			{
				gameObject = null;
			}
			float num = (!(gameObject != null)) ? 1E+09f : Vector3.Distance(this.myTransform.position, gameObject.transform.position);
			bool flag = gameObject != null && num <= this._soundClips.detectRadius;
			float num2 = Vector3.Distance(this.myTransform.position, this._player.transform.position);
			bool flag2 = !this._playerMoveC.isInvisible && num2 <= this._soundClips.detectRadius;
			Transform transform = null;
			if (flag2 && flag)
			{
				if (num2 < num)
				{
					transform = this._player.transform;
				}
				else
				{
					transform = gameObject.transform;
				}
			}
			else
			{
				if (flag2)
				{
					transform = this._player.transform;
				}
				if (flag)
				{
					transform = gameObject.transform;
				}
			}
			if (transform != null)
			{
				this._eai.SetTarget(transform, true);
				this._entered = true;
			}
		}
		else if (this._eai.Target == null || (this._eai.Target.CompareTag("Player") && (this._playerMoveC.isInvisible || (this._entered && Vector3.SqrMagnitude(base.transform.position - this._player.transform.position) > this._soundClips.detectRadius * this._soundClips.detectRadius))) || (this._eai.Target.CompareTag("Turret") && this._eai.Target.GetComponent<TurretController>().isKilled && this._eai.Target.GetComponent<TurretController>().isRun))
		{
			this._eai.SetTarget(null, false);
			this._entered = false;
		}
	}

	// Token: 0x040023D8 RID: 9176
	public bool shouldDetectPlayer = true;

	// Token: 0x040023D9 RID: 9177
	private bool _entered;

	// Token: 0x040023DA RID: 9178
	private BotAI _eai;

	// Token: 0x040023DB RID: 9179
	private GameObject _player;

	// Token: 0x040023DC RID: 9180
	private Player_move_c _playerMoveC;

	// Token: 0x040023DD RID: 9181
	private GameObject _modelChild;

	// Token: 0x040023DE RID: 9182
	private Sounds _soundClips;

	// Token: 0x040023DF RID: 9183
	private Transform myTransform;
}
