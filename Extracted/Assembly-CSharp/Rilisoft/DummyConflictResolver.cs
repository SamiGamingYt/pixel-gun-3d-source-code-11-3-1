using System;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000837 RID: 2103
	internal sealed class DummyConflictResolver : IConflictResolver
	{
		// Token: 0x06004C64 RID: 19556 RVA: 0x001B8620 File Offset: 0x001B6820
		private DummyConflictResolver()
		{
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06004C66 RID: 19558 RVA: 0x001B8634 File Offset: 0x001B6834
		internal static DummyConflictResolver Instance
		{
			get
			{
				return DummyConflictResolver.s_instance;
			}
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x001B863C File Offset: 0x001B683C
		public void ChooseMetadata(ISavedGameMetadata chosenMetadata)
		{
			string text = (chosenMetadata == null) ? string.Empty : chosenMetadata.Filename;
			Debug.LogFormat("{0}('{1}').ChooseMetadata()", new object[]
			{
				base.GetType().Name,
				text
			});
		}

		// Token: 0x04003B44 RID: 15172
		private static readonly DummyConflictResolver s_instance = new DummyConflictResolver();
	}
}
