using System;

namespace GooglePlayGames.BasicApi.Events
{
	// Token: 0x0200016D RID: 365
	public interface IEvent
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000BCE RID: 3022
		string Id { get; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000BCF RID: 3023
		string Name { get; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000BD0 RID: 3024
		string Description { get; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000BD1 RID: 3025
		string ImageUrl { get; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000BD2 RID: 3026
		ulong CurrentCount { get; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000BD3 RID: 3027
		EventVisibility Visibility { get; }
	}
}
