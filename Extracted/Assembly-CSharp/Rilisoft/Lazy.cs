using System;

namespace Rilisoft
{
	// Token: 0x020002ED RID: 749
	internal class Lazy<T>
	{
		// Token: 0x06001A26 RID: 6694 RVA: 0x000699A8 File Offset: 0x00067BA8
		public Lazy(Func<T> valueFactory)
		{
			this.m_threadSafeObj = new object();
			this.m_valueFactory = valueFactory;
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x000699F4 File Offset: 0x00067BF4
		public T Value
		{
			get
			{
				if (this.m_boxed == null)
				{
					return this.LazyInitValue();
				}
				Lazy<T>.Boxed boxed = this.m_boxed as Lazy<T>.Boxed;
				if (boxed != null)
				{
					return boxed.m_value;
				}
				Exception ex = this.m_boxed as Exception;
				throw ex;
			}
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x00069A3C File Offset: 0x00067C3C
		private T LazyInitValue()
		{
			Lazy<T>.Boxed boxed = null;
			object threadSafeObj = this.m_threadSafeObj;
			try
			{
				if (threadSafeObj != Lazy<T>.ALREADY_INVOKED_SENTINEL)
				{
				}
				if (this.m_boxed == null)
				{
					boxed = this.CreateValue();
					this.m_boxed = boxed;
					this.m_threadSafeObj = Lazy<T>.ALREADY_INVOKED_SENTINEL;
				}
				else
				{
					boxed = (this.m_boxed as Lazy<T>.Boxed);
					if (boxed == null)
					{
						Exception ex = this.m_boxed as Exception;
						throw ex;
					}
				}
			}
			finally
			{
			}
			return boxed.m_value;
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x00069AD4 File Offset: 0x00067CD4
		private Lazy<T>.Boxed CreateValue()
		{
			Lazy<T>.Boxed result = null;
			try
			{
				if (this.m_valueFactory == Lazy<T>.ALREADY_INVOKED_SENTINEL)
				{
					throw new InvalidOperationException();
				}
				Func<T> valueFactory = this.m_valueFactory;
				this.m_valueFactory = Lazy<T>.ALREADY_INVOKED_SENTINEL;
				result = new Lazy<T>.Boxed(valueFactory());
			}
			catch (Exception boxed)
			{
				this.m_boxed = boxed;
				throw;
			}
			return result;
		}

		// Token: 0x04000F3F RID: 3903
		private static Func<T> ALREADY_INVOKED_SENTINEL = () => default(T);

		// Token: 0x04000F40 RID: 3904
		private object m_boxed;

		// Token: 0x04000F41 RID: 3905
		private Func<T> m_valueFactory;

		// Token: 0x04000F42 RID: 3906
		private volatile object m_threadSafeObj;

		// Token: 0x020002EE RID: 750
		private class Boxed
		{
			// Token: 0x06001A2C RID: 6700 RVA: 0x00069B68 File Offset: 0x00067D68
			internal Boxed(T value)
			{
				this.m_value = value;
			}

			// Token: 0x04000F44 RID: 3908
			internal T m_value;
		}
	}
}
