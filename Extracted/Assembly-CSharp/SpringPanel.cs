using System;
using UnityEngine;

// Token: 0x0200036F RID: 879
[RequireComponent(typeof(UIPanel))]
[AddComponentMenu("NGUI/Internal/Spring Panel")]
public class SpringPanel : MonoBehaviour
{
	// Token: 0x06001EB9 RID: 7865 RVA: 0x0008A79C File Offset: 0x0008899C
	private void Start()
	{
		this.mPanel = base.GetComponent<UIPanel>();
		this.mDrag = base.GetComponent<UIScrollView>();
		this.mTrans = base.transform;
	}

	// Token: 0x06001EBA RID: 7866 RVA: 0x0008A7D0 File Offset: 0x000889D0
	private void Update()
	{
		this.AdvanceTowardsPosition();
	}

	// Token: 0x06001EBB RID: 7867 RVA: 0x0008A7D8 File Offset: 0x000889D8
	protected virtual void AdvanceTowardsPosition()
	{
		float deltaTime = RealTime.deltaTime;
		bool flag = false;
		Vector3 localPosition = this.mTrans.localPosition;
		Vector3 vector = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
		if ((vector - this.target).sqrMagnitude < 0.01f)
		{
			vector = this.target;
			base.enabled = false;
			flag = true;
		}
		this.mTrans.localPosition = vector;
		Vector3 vector2 = vector - localPosition;
		Vector2 clipOffset = this.mPanel.clipOffset;
		clipOffset.x -= vector2.x;
		clipOffset.y -= vector2.y;
		this.mPanel.clipOffset = clipOffset;
		if (this.mDrag != null)
		{
			this.mDrag.UpdateScrollbars(false);
		}
		if (flag && this.onFinished != null)
		{
			SpringPanel.current = this;
			this.onFinished();
			SpringPanel.current = null;
		}
	}

	// Token: 0x06001EBC RID: 7868 RVA: 0x0008A8E4 File Offset: 0x00088AE4
	public static SpringPanel Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPanel springPanel = go.GetComponent<SpringPanel>();
		if (springPanel == null)
		{
			springPanel = go.AddComponent<SpringPanel>();
		}
		springPanel.target = pos;
		springPanel.strength = strength;
		springPanel.onFinished = null;
		springPanel.enabled = true;
		return springPanel;
	}

	// Token: 0x0400134C RID: 4940
	public static SpringPanel current;

	// Token: 0x0400134D RID: 4941
	public Vector3 target = Vector3.zero;

	// Token: 0x0400134E RID: 4942
	public float strength = 10f;

	// Token: 0x0400134F RID: 4943
	public SpringPanel.OnFinished onFinished;

	// Token: 0x04001350 RID: 4944
	private UIPanel mPanel;

	// Token: 0x04001351 RID: 4945
	private Transform mTrans;

	// Token: 0x04001352 RID: 4946
	private UIScrollView mDrag;

	// Token: 0x020008FB RID: 2299
	// (Invoke) Token: 0x0600508C RID: 20620
	public delegate void OnFinished();
}
