using System;
using System.Globalization;
using GooglePlayGames.BasicApi.SavedGame;

namespace Rilisoft
{
	// Token: 0x02000714 RID: 1812
	internal static class MetadataExtensions
	{
		// Token: 0x06003F3D RID: 16189 RVA: 0x00152868 File Offset: 0x00150A68
		public static string GetDescription(this ISavedGameMetadata metadata)
		{
			if (metadata == null)
			{
				return "<null>";
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} ({1:s})", new object[]
			{
				metadata.Description,
				metadata.LastModifiedTimestamp
			});
		}
	}
}
