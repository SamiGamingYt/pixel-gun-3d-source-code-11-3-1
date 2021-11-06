using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006D4 RID: 1748
[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
	// Token: 0x06003CC9 RID: 15561 RVA: 0x0013BE04 File Offset: 0x0013A004
	private void OnEnable()
	{
		base.StartCoroutine(this.Manage());
	}

	// Token: 0x06003CCA RID: 15562 RVA: 0x0013BE14 File Offset: 0x0013A014
	private IEnumerator Manage()
	{
		NavMeshAgent agent = base.GetComponent<NavMeshAgent>();
		agent.autoTraverseOffMeshLink = false;
		for (;;)
		{
			if (agent.isOnOffMeshLink)
			{
				if (this._moveMethod == OffMeshLinkMoveMethod.NormalSpeed)
				{
					yield return base.StartCoroutine(this.NormalSpeed(agent));
				}
				else if (this._moveMethod == OffMeshLinkMoveMethod.Parabola)
				{
					yield return base.StartCoroutine(this.Parabola(agent, this._parabolaHeight, this._duration));
				}
				else if (this._moveMethod == OffMeshLinkMoveMethod.Curve)
				{
					yield return base.StartCoroutine(this.Curve(agent, this._duration, this._curve));
				}
				agent.CompleteOffMeshLink();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003CCB RID: 15563 RVA: 0x0013BE30 File Offset: 0x0013A030
	private IEnumerator NormalSpeed(NavMeshAgent agent)
	{
		Vector3 endPos = agent.currentOffMeshLinkData.endPos + Vector3.up * agent.baseOffset;
		while (agent.transform.position != endPos)
		{
			agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003CCC RID: 15564 RVA: 0x0013BE54 File Offset: 0x0013A054
	private IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
	{
		OffMeshLinkData data = agent.currentOffMeshLinkData;
		Vector3 startPos = agent.transform.position;
		Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			float yOffset = height * 4f * (normalizedTime - normalizedTime * normalizedTime);
			agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
			normalizedTime += Time.deltaTime / duration;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003CCD RID: 15565 RVA: 0x0013BE94 File Offset: 0x0013A094
	private IEnumerator Curve(NavMeshAgent agent, float duration, AnimationCurve curve)
	{
		OffMeshLinkData data = agent.currentOffMeshLinkData;
		Vector3 startPos = agent.transform.position;
		Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
		float normalizedTime = 0f;
		while (normalizedTime < 1f)
		{
			float yOffset = curve.Evaluate(normalizedTime);
			agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
			normalizedTime += Time.deltaTime / duration;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002CF0 RID: 11504
	[SerializeField]
	private OffMeshLinkMoveMethod _moveMethod = OffMeshLinkMoveMethod.Parabola;

	// Token: 0x04002CF1 RID: 11505
	[SerializeField]
	private float _duration = 0.5f;

	// Token: 0x04002CF2 RID: 11506
	[Header("special parameters")]
	[SerializeField]
	private float _parabolaHeight = 2f;

	// Token: 0x04002CF3 RID: 11507
	[SerializeField]
	private AnimationCurve _curve = new AnimationCurve();
}
