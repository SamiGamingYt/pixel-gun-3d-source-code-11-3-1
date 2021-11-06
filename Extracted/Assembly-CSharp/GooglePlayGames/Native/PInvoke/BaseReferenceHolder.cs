using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	// Token: 0x02000236 RID: 566
	internal abstract class BaseReferenceHolder : IDisposable
	{
		// Token: 0x060011E2 RID: 4578 RVA: 0x0004C760 File Offset: 0x0004A960
		public BaseReferenceHolder(IntPtr pointer)
		{
			this.mSelfPointer = PInvokeUtilities.CheckNonNull(new HandleRef(this, pointer));
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0004C788 File Offset: 0x0004A988
		protected bool IsDisposed()
		{
			return PInvokeUtilities.IsNull(this.mSelfPointer);
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0004C798 File Offset: 0x0004A998
		protected HandleRef SelfPtr()
		{
			if (this.IsDisposed())
			{
				throw new InvalidOperationException("Attempted to use object after it was cleaned up");
			}
			return this.mSelfPointer;
		}

		// Token: 0x060011E6 RID: 4582
		protected abstract void CallDispose(HandleRef selfPointer);

		// Token: 0x060011E7 RID: 4583 RVA: 0x0004C7B8 File Offset: 0x0004A9B8
		~BaseReferenceHolder()
		{
			this.Dispose(true);
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x0004C7F4 File Offset: 0x0004A9F4
		public void Dispose()
		{
			this.Dispose(false);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x0004C804 File Offset: 0x0004AA04
		internal IntPtr AsPointer()
		{
			return this.SelfPtr().Handle;
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x0004C820 File Offset: 0x0004AA20
		private void Dispose(bool fromFinalizer)
		{
			if ((fromFinalizer || !BaseReferenceHolder._refs.ContainsKey(this.mSelfPointer)) && !PInvokeUtilities.IsNull(this.mSelfPointer))
			{
				this.CallDispose(this.mSelfPointer);
				this.mSelfPointer = new HandleRef(this, IntPtr.Zero);
			}
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0004C878 File Offset: 0x0004AA78
		internal void ReferToMe()
		{
			BaseReferenceHolder._refs[this.SelfPtr()] = this;
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x0004C88C File Offset: 0x0004AA8C
		internal void ForgetMe()
		{
			if (BaseReferenceHolder._refs.ContainsKey(this.SelfPtr()))
			{
				BaseReferenceHolder._refs.Remove(this.SelfPtr());
				this.Dispose(false);
			}
		}

		// Token: 0x04000BF7 RID: 3063
		private static Dictionary<HandleRef, BaseReferenceHolder> _refs = new Dictionary<HandleRef, BaseReferenceHolder>();

		// Token: 0x04000BF8 RID: 3064
		private HandleRef mSelfPointer;
	}
}
