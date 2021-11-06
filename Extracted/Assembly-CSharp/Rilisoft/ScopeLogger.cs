using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000752 RID: 1874
	internal struct ScopeLogger : IDisposable
	{
		// Token: 0x060041C7 RID: 16839 RVA: 0x0015DD60 File Offset: 0x0015BF60
		public ScopeLogger(string caller, string callee, bool enabled)
		{
			this._caller = (caller ?? string.Empty);
			this._callee = (callee ?? string.Empty);
			this._enabled = enabled;
			if (this._enabled)
			{
				this._startTime = Time.realtimeSinceStartup;
				this._startFrame = Time.frameCount;
				string text = (!string.IsNullOrEmpty(this._caller)) ? "{0} > {1}: {2:f3}, {3}" : "> {1}: {2:f3}, {3}";
				string format = (!Application.isEditor) ? text : ("<color=orange>" + text + "</color>");
				Debug.LogFormat(format, new object[]
				{
					this._caller,
					this._callee,
					this._startTime,
					this._startFrame
				});
			}
			else
			{
				this._startTime = 0f;
				this._startFrame = 0;
			}
			this._initialized = true;
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x0015DE54 File Offset: 0x0015C054
		public ScopeLogger(string callee, bool enabled)
		{
			this = new ScopeLogger(string.Empty, callee, enabled);
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x0015DE64 File Offset: 0x0015C064
		public void Dispose()
		{
			if (!this._initialized)
			{
				return;
			}
			if (this._enabled)
			{
				string text = (!string.IsNullOrEmpty(this._caller)) ? "{0} < {1}: +{2:f3}, +{3}" : "< {1}: +{2:f3}, +{3}";
				string format = (!Application.isEditor) ? text : ("<color=orange>" + text + "</color>");
				Debug.LogFormat(format, new object[]
				{
					this._caller,
					this._callee,
					Time.realtimeSinceStartup - this._startTime,
					Time.frameCount - this._startFrame
				});
			}
			this._callee = string.Empty;
			this._caller = string.Empty;
			this._initialized = false;
		}

		// Token: 0x0400300E RID: 12302
		private string _callee;

		// Token: 0x0400300F RID: 12303
		private string _caller;

		// Token: 0x04003010 RID: 12304
		private bool _enabled;

		// Token: 0x04003011 RID: 12305
		private bool _initialized;

		// Token: 0x04003012 RID: 12306
		private readonly int _startFrame;

		// Token: 0x04003013 RID: 12307
		private readonly float _startTime;
	}
}
