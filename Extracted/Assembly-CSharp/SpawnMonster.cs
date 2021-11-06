using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200075C RID: 1884
public class SpawnMonster : MonoBehaviour
{
	// Token: 0x17000AF0 RID: 2800
	// (get) Token: 0x06004225 RID: 16933 RVA: 0x0015F9CC File Offset: 0x0015DBCC
	// (set) Token: 0x06004226 RID: 16934 RVA: 0x0015F9D4 File Offset: 0x0015DBD4
	public bool ShouldMove
	{
		get
		{
			return this.shouldMove;
		}
		set
		{
			if (this.shouldMove == value)
			{
				return;
			}
			this.shouldMove = value;
			if (this.shouldMove)
			{
				this.ResetPars();
				base.SendMessage("Walk");
			}
		}
	}

	// Token: 0x06004227 RID: 16935 RVA: 0x0015FA0C File Offset: 0x0015DC0C
	private void Awake()
	{
		if (Defs.isCOOP)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06004228 RID: 16936 RVA: 0x0015FA20 File Offset: 0x0015DC20
	private void Start()
	{
		this._nma = base.GetComponent<NavMeshAgent>();
		using (IEnumerator enumerator = base.transform.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				this._modelChild = transform.gameObject;
			}
		}
		this._soundClips = this._modelChild.GetComponent<Sounds>();
		if (!this.isCycle)
		{
			this._spawnPoint = new Vector2(base.transform.position.x, base.transform.position.z);
		}
		this.ShouldMove = false;
		this.ShouldMove = true;
		this._moveBounds = new Rect(-this.halbLength, -this.halbLength, this.halbLength * 2f, this.halbLength * 2f);
	}

	// Token: 0x06004229 RID: 16937 RVA: 0x0015FB34 File Offset: 0x0015DD34
	private void Update()
	{
		if (!this.shouldMove)
		{
			return;
		}
		if (this._lastTimeGo <= Time.time)
		{
			this._nma.ResetPath();
			this._targetPoint = new Vector3(-this.halbLength + UnityEngine.Random.Range(0f, this.halbLength * 2f), base.transform.position.y, -this.halbLength + UnityEngine.Random.Range(0f, this.halbLength * 2f));
			this._lastTimeGo = Time.time + Vector3.Distance(base.transform.position, this._targetPoint) / this._soundClips.notAttackingSpeed + this._timeForIdle;
			base.transform.LookAt(this._targetPoint);
			this._nma.SetDestination(this._targetPoint);
			this._nma.speed = this._soundClips.notAttackingSpeed;
		}
	}

	// Token: 0x0600422A RID: 16938 RVA: 0x0015FC30 File Offset: 0x0015DE30
	private void ResetPars()
	{
		this._targetIndex = 0;
		this._lastTimeGo = -1f;
	}

	// Token: 0x04003059 RID: 12377
	public bool shouldMove = true;

	// Token: 0x0400305A RID: 12378
	public bool isCycle;

	// Token: 0x0400305B RID: 12379
	public GameObject[] _targetCyclePoints;

	// Token: 0x0400305C RID: 12380
	private int _targetIndex;

	// Token: 0x0400305D RID: 12381
	private float _minDist = 5f;

	// Token: 0x0400305E RID: 12382
	private Vector2 _spawnPoint;

	// Token: 0x0400305F RID: 12383
	private float _lastTimeGo = -1f;

	// Token: 0x04003060 RID: 12384
	private float _timeForIdle = 2f;

	// Token: 0x04003061 RID: 12385
	private Vector3 _targetPoint;

	// Token: 0x04003062 RID: 12386
	private Rect _moveBounds;

	// Token: 0x04003063 RID: 12387
	private float halbLength = 17f;

	// Token: 0x04003064 RID: 12388
	private float _dst;

	// Token: 0x04003065 RID: 12389
	private GameObject _modelChild;

	// Token: 0x04003066 RID: 12390
	private Sounds _soundClips;

	// Token: 0x04003067 RID: 12391
	private NavMeshAgent _nma;
}
