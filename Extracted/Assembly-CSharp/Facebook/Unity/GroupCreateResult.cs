using System;

namespace Facebook.Unity
{
	// Token: 0x020000F8 RID: 248
	internal class GroupCreateResult : ResultBase, IGroupCreateResult, IResult
	{
		// Token: 0x06000794 RID: 1940 RVA: 0x0002F5BC File Offset: 0x0002D7BC
		public GroupCreateResult(string result) : base(result)
		{
			string groupId;
			if (this.ResultDictionary != null && this.ResultDictionary.TryGetValue("id", out groupId))
			{
				this.GroupId = groupId;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0002F5FC File Offset: 0x0002D7FC
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x0002F604 File Offset: 0x0002D804
		public string GroupId { get; private set; }

		// Token: 0x04000677 RID: 1655
		public const string IDKey = "id";
	}
}
