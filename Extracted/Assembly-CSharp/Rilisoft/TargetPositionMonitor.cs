using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006E7 RID: 1767
	public class TargetPositionMonitor : MonoBehaviour
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06003D86 RID: 15750 RVA: 0x0013F650 File Offset: 0x0013D850
		// (set) Token: 0x06003D87 RID: 15751 RVA: 0x0013F658 File Offset: 0x0013D858
		public float MinDistance { get; private set; }

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06003D88 RID: 15752 RVA: 0x0013F664 File Offset: 0x0013D864
		// (set) Token: 0x06003D89 RID: 15753 RVA: 0x0013F66C File Offset: 0x0013D86C
		public float CheckInterval { get; private set; }

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06003D8A RID: 15754 RVA: 0x0013F678 File Offset: 0x0013D878
		// (set) Token: 0x06003D8B RID: 15755 RVA: 0x0013F680 File Offset: 0x0013D880
		public bool Enabled { get; private set; }

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06003D8C RID: 15756 RVA: 0x0013F68C File Offset: 0x0013D88C
		// (set) Token: 0x06003D8D RID: 15757 RVA: 0x0013F694 File Offset: 0x0013D894
		public Func<Vector3> TargetPositionGetter { get; private set; }

		// Token: 0x06003D8E RID: 15758 RVA: 0x0013F6A0 File Offset: 0x0013D8A0
		public void StartMonitoring(Func<Vector3> targetPositionGetter, float minDistance = 0.1f, float checkInterval = 0.1f)
		{
			this.TargetPositionGetter = null;
			this._path.Clear();
			this._checkTimeElapsed = 0f;
			this._lastIdx = 0;
			if (targetPositionGetter == null)
			{
				this.Enabled = false;
				return;
			}
			this.TargetPositionGetter = targetPositionGetter;
			this.MinDistance = minDistance;
			this.CheckInterval = checkInterval;
			this._path.Add(this.TargetPositionGetter());
			this.Enabled = true;
		}

		// Token: 0x06003D8F RID: 15759 RVA: 0x0013F714 File Offset: 0x0013D914
		public void Reset()
		{
			if (this.Enabled)
			{
				this._path.Clear();
				this._checkTimeElapsed = 0f;
				this._lastIdx = 0;
				this._path.Add(this.TargetPositionGetter());
			}
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x0013F760 File Offset: 0x0013D960
		public void StopMonitoring()
		{
			this._path.Clear();
			this.Enabled = false;
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x0013F774 File Offset: 0x0013D974
		public bool HasNextPoint()
		{
			return this._path.Count - 1 > this._lastIdx;
		}

		// Token: 0x06003D92 RID: 15762 RVA: 0x0013F78C File Offset: 0x0013D98C
		public Vector3 GetCurrentPoint()
		{
			return this._path[this._lastIdx];
		}

		// Token: 0x06003D93 RID: 15763 RVA: 0x0013F7A0 File Offset: 0x0013D9A0
		public Vector3 GetNextPoint()
		{
			this._lastIdx++;
			return this._path[this._lastIdx];
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x0013F7C4 File Offset: 0x0013D9C4
		private void Update()
		{
			if (!this.Enabled || this.TargetPositionGetter == null)
			{
				return;
			}
			this._checkTimeElapsed += Time.deltaTime;
			if (this._checkTimeElapsed < this.CheckInterval)
			{
				return;
			}
			this._checkTimeElapsed = 0f;
			Vector3 a = this._path[this._path.Count - 1];
			if (Mathf.Abs((a - this.TargetPositionGetter()).sqrMagnitude) >= this.MinDistance * 2f)
			{
				this._path.Add(this.TargetPositionGetter());
			}
		}

		// Token: 0x04002D5E RID: 11614
		private readonly List<Vector3> _path = new List<Vector3>();

		// Token: 0x04002D5F RID: 11615
		private float _checkTimeElapsed;

		// Token: 0x04002D60 RID: 11616
		private int _lastIdx;
	}
}
