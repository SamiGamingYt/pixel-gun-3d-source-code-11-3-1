using System;

namespace Rilisoft.NullExtensions
{
	// Token: 0x020006CB RID: 1739
	public static class NullEx
	{
		// Token: 0x06003C89 RID: 15497 RVA: 0x0013A68C File Offset: 0x0013888C
		public static TResult Map<TInput, TResult>(this TInput o, Func<TInput, TResult> selector) where TInput : class
		{
			if (o == null)
			{
				return default(TResult);
			}
			return selector(o);
		}

		// Token: 0x06003C8A RID: 15498 RVA: 0x0013A6B8 File Offset: 0x001388B8
		public static TResult Map<TInput, TResult>(this TInput o, Func<TInput, TResult> selector, TResult defaultValue) where TInput : class
		{
			if (o == null)
			{
				return defaultValue;
			}
			return selector(o);
		}

		// Token: 0x06003C8B RID: 15499 RVA: 0x0013A6D0 File Offset: 0x001388D0
		public static TInput Filter<TInput>(this TInput o, Func<TInput, bool> pred) where TInput : class
		{
			if (o == null)
			{
				return (TInput)((object)null);
			}
			return (!pred(o)) ? ((TInput)((object)null)) : o;
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x0013A708 File Offset: 0x00138908
		public static TInput Do<TInput>(this TInput o, Action<TInput> action) where TInput : class
		{
			if (o == null)
			{
				return (TInput)((object)null);
			}
			action(o);
			return o;
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x0013A724 File Offset: 0x00138924
		public static TInput Do<TInput>(this TInput o, Action<TInput> action, Action defaultAction) where TInput : class
		{
			if (o == null)
			{
				defaultAction();
				return (TInput)((object)null);
			}
			action(o);
			return o;
		}
	}
}
