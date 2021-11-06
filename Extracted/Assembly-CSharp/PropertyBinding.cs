using System;
using UnityEngine;

// Token: 0x0200036A RID: 874
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Internal/Property Binding")]
public class PropertyBinding : MonoBehaviour
{
	// Token: 0x06001E97 RID: 7831 RVA: 0x00089F40 File Offset: 0x00088140
	private void Start()
	{
		this.UpdateTarget();
		if (this.update == PropertyBinding.UpdateCondition.OnStart)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x00089F5C File Offset: 0x0008815C
	private void Update()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x06001E99 RID: 7833 RVA: 0x00089F70 File Offset: 0x00088170
	private void LateUpdate()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnLateUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x00089F84 File Offset: 0x00088184
	private void FixedUpdate()
	{
		if (this.update == PropertyBinding.UpdateCondition.OnFixedUpdate)
		{
			this.UpdateTarget();
		}
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x00089F98 File Offset: 0x00088198
	private void OnValidate()
	{
		if (this.source != null)
		{
			this.source.Reset();
		}
		if (this.target != null)
		{
			this.target.Reset();
		}
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x00089FD4 File Offset: 0x000881D4
	[ContextMenu("Update Now")]
	public void UpdateTarget()
	{
		if (this.source != null && this.target != null && this.source.isValid && this.target.isValid)
		{
			if (this.direction == PropertyBinding.Direction.SourceUpdatesTarget)
			{
				this.target.Set(this.source.Get());
			}
			else if (this.direction == PropertyBinding.Direction.TargetUpdatesSource)
			{
				this.source.Set(this.target.Get());
			}
			else if (this.source.GetPropertyType() == this.target.GetPropertyType())
			{
				object obj = this.source.Get();
				if (this.mLastValue == null || !this.mLastValue.Equals(obj))
				{
					this.mLastValue = obj;
					this.target.Set(obj);
				}
				else
				{
					obj = this.target.Get();
					if (!this.mLastValue.Equals(obj))
					{
						this.mLastValue = obj;
						this.source.Set(obj);
					}
				}
			}
		}
	}

	// Token: 0x04001338 RID: 4920
	public PropertyReference source;

	// Token: 0x04001339 RID: 4921
	public PropertyReference target;

	// Token: 0x0400133A RID: 4922
	public PropertyBinding.Direction direction;

	// Token: 0x0400133B RID: 4923
	public PropertyBinding.UpdateCondition update = PropertyBinding.UpdateCondition.OnUpdate;

	// Token: 0x0400133C RID: 4924
	public bool editMode = true;

	// Token: 0x0400133D RID: 4925
	private object mLastValue;

	// Token: 0x0200036B RID: 875
	public enum UpdateCondition
	{
		// Token: 0x0400133F RID: 4927
		OnStart,
		// Token: 0x04001340 RID: 4928
		OnUpdate,
		// Token: 0x04001341 RID: 4929
		OnLateUpdate,
		// Token: 0x04001342 RID: 4930
		OnFixedUpdate
	}

	// Token: 0x0200036C RID: 876
	public enum Direction
	{
		// Token: 0x04001344 RID: 4932
		SourceUpdatesTarget,
		// Token: 0x04001345 RID: 4933
		TargetUpdatesSource,
		// Token: 0x04001346 RID: 4934
		BiDirectional
	}
}
