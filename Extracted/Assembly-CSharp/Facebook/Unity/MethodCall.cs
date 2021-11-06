using System;

namespace Facebook.Unity
{
	// Token: 0x020000D7 RID: 215
	internal abstract class MethodCall<T> where T : IResult
	{
		// Token: 0x06000685 RID: 1669 RVA: 0x0002D650 File Offset: 0x0002B850
		public MethodCall(FacebookBase facebookImpl, string methodName)
		{
			this.Parameters = new MethodArguments();
			this.FacebookImpl = facebookImpl;
			this.MethodName = methodName;
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x0002D67C File Offset: 0x0002B87C
		// (set) Token: 0x06000687 RID: 1671 RVA: 0x0002D684 File Offset: 0x0002B884
		public string MethodName { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x0002D690 File Offset: 0x0002B890
		// (set) Token: 0x06000689 RID: 1673 RVA: 0x0002D698 File Offset: 0x0002B898
		public FacebookDelegate<T> Callback { protected get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0002D6A4 File Offset: 0x0002B8A4
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x0002D6AC File Offset: 0x0002B8AC
		protected FacebookBase FacebookImpl { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0002D6B8 File Offset: 0x0002B8B8
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x0002D6C0 File Offset: 0x0002B8C0
		protected MethodArguments Parameters { get; set; }

		// Token: 0x0600068E RID: 1678
		public abstract void Call(MethodArguments args = null);
	}
}
