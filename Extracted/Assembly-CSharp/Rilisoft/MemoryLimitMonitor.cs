using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020006BF RID: 1727
	internal sealed class MemoryLimitMonitor : MonoBehaviour
	{
		// Token: 0x06003C36 RID: 15414 RVA: 0x00138464 File Offset: 0x00136664
		private static int GetBitsPerPixel(TextureFormat format)
		{
			switch (format)
			{
			case TextureFormat.Alpha8:
				return 8;
			case TextureFormat.ARGB4444:
				return 16;
			case TextureFormat.RGB24:
				return 24;
			case TextureFormat.RGBA32:
				return 32;
			case TextureFormat.ARGB32:
				return 32;
			default:
				switch (format)
				{
				case TextureFormat.PVRTC_RGB2:
					return 2;
				case TextureFormat.PVRTC_RGBA2:
					return 2;
				case TextureFormat.PVRTC_RGB4:
					return 4;
				case TextureFormat.PVRTC_RGBA4:
					return 4;
				case TextureFormat.ETC_RGB4:
					return 4;
				case TextureFormat.ATC_RGB4:
					return 4;
				case TextureFormat.ATC_RGBA8:
					return 8;
				default:
					return 0;
				}
				break;
			case TextureFormat.RGB565:
				return 16;
			case TextureFormat.DXT1:
				return 4;
			case TextureFormat.DXT5:
				return 8;
			case TextureFormat.BGRA32:
				return 32;
			}
		}

		// Token: 0x04002C73 RID: 11379
		private int _timestamp;

		// Token: 0x04002C74 RID: 11380
		private long _currentMemoryUsage;

		// Token: 0x04002C75 RID: 11381
		private long _peakMemoryUsage;

		// Token: 0x04002C76 RID: 11382
		private string _texturesStrings = string.Empty;
	}
}
