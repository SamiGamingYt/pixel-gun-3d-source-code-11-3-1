using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x020005DC RID: 1500
public sealed class Device
{
	// Token: 0x1700088A RID: 2186
	// (get) Token: 0x06003374 RID: 13172 RVA: 0x0010A92C File Offset: 0x00108B2C
	public static bool IsLoweMemoryDevice
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return SystemInfo.systemMemorySize < 900;
			}
			if (Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
			{
				return !Application.isEditor;
			}
			return Application.platform == RuntimePlatform.OSXEditor && false;
		}
	}

	// Token: 0x1700088B RID: 2187
	// (get) Token: 0x06003375 RID: 13173 RVA: 0x0010A97C File Offset: 0x00108B7C
	public static bool isWeakDevice
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return !Device.IsQuiteGoodAndroidDevice();
			}
			return Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64 || (Application.platform == RuntimePlatform.WindowsEditor && false);
		}
	}

	// Token: 0x1700088C RID: 2188
	// (get) Token: 0x06003376 RID: 13174 RVA: 0x0010A9B0 File Offset: 0x00108BB0
	public static bool isPixelGunLowDevice
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return Device.IsWeakAndroid();
			}
			return Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64;
		}
	}

	// Token: 0x1700088D RID: 2189
	// (get) Token: 0x06003377 RID: 13175 RVA: 0x0010A9D4 File Offset: 0x00108BD4
	public static bool isNonRetinaDevice
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700088E RID: 2190
	// (get) Token: 0x06003378 RID: 13176 RVA: 0x0010A9D8 File Offset: 0x00108BD8
	public static bool isRetinaAndStrong
	{
		get
		{
			return Application.isEditor;
		}
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x0010A9E8 File Offset: 0x00108BE8
	internal static bool TryGetGpuRating(out int rating)
	{
		return Device._gpuRatings.TryGetValue(SystemInfo.graphicsDeviceName, out rating);
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x0010A9FC File Offset: 0x00108BFC
	internal static string FormatGpuModelMemoryRating()
	{
		string format = SystemInfo.graphicsDeviceName + ": {{ Memory: {0}Mb, Rating: {1} }}";
		int num;
		return (!Device._gpuRatings.TryGetValue(SystemInfo.graphicsDeviceName, out num)) ? string.Format(format, SystemInfo.graphicsMemorySize, "?") : string.Format(format, SystemInfo.graphicsMemorySize, num);
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x0010AA60 File Offset: 0x00108C60
	internal static string FormatDeviceModelMemoryRating()
	{
		string format = SystemInfo.deviceModel + ": {{ Memory: {0}Mb, Rating: {1} }}";
		int num;
		return (!Device._gpuRatings.TryGetValue(SystemInfo.graphicsDeviceName, out num)) ? string.Format(format, SystemInfo.systemMemorySize, "?") : string.Format(format, SystemInfo.systemMemorySize, num);
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x0010AAC4 File Offset: 0x00108CC4
	public static bool GpuRatingIsAtLeast(int desiredGpuRating)
	{
		int num;
		return !Device._gpuRatings.TryGetValue(SystemInfo.graphicsDeviceName, out num) || num >= desiredGpuRating;
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x0010AAF4 File Offset: 0x00108CF4
	public static bool IsWeakAndroid()
	{
		int processorCount = SystemInfo.processorCount;
		int processorFrequency = SystemInfo.processorFrequency;
		int systemMemorySize = SystemInfo.systemMemorySize;
		Debug.LogFormat("Device info: {{ 'processorCount':{0}, 'processorFrequency (MHz)':{1}, 'systemMemorySize':{2} }}", new object[]
		{
			processorCount,
			processorFrequency,
			systemMemorySize
		});
		if (processorCount == 4)
		{
			if (systemMemorySize <= 1300 || processorFrequency <= 1400)
			{
				return true;
			}
		}
		else if (processorCount <= 2 && (systemMemorySize <= 1300 || processorFrequency <= 1400))
		{
			return true;
		}
		return processorCount == 1;
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x0010AB8C File Offset: 0x00108D8C
	public static bool IsQuiteGoodAndroidDevice()
	{
		return false;
	}

	// Token: 0x040025C8 RID: 9672
	private static readonly IDictionary<string, int> _gpuRatings = new Dictionary<string, int>
	{
		{
			"Adreno (TM) 330",
			17
		},
		{
			"PowerVR SGX 554MP",
			15
		},
		{
			"Mali-T628",
			15
		},
		{
			"Mali-T624",
			15
		},
		{
			"PowerVR G6430",
			15
		},
		{
			"PowerVR Rogue",
			14
		},
		{
			"Mali-T604",
			11
		},
		{
			"Adreno (TM) 320",
			11
		},
		{
			"PowerVR SGX G6200",
			10
		},
		{
			"PowerVR SGX 543MP",
			8
		},
		{
			"PowerVR SGX 544",
			8
		},
		{
			"PowerVR SGX 544MP",
			8
		},
		{
			"Intel HD Graphics",
			8
		},
		{
			"Mali-450 MP",
			8
		},
		{
			"Vivante GC4000",
			6
		},
		{
			"Adreno (TM) 305",
			5
		},
		{
			"NVIDIA Tegra 3",
			5
		},
		{
			"NVIDIA Tegra 3 / Chainfire3D",
			5
		},
		{
			"Vivante GC2000",
			5
		},
		{
			"GC2000 core / Chainfire3D",
			5
		},
		{
			"Mali-400 MP",
			4
		},
		{
			"MALI-400MP4",
			4
		},
		{
			"Mali-400 MP / Chainfire3D",
			4
		},
		{
			"Adreno (TM) 225",
			4
		},
		{
			"VideoCore IV HW",
			4
		},
		{
			"NVIDIA Tegra",
			3
		},
		{
			"GC1000 core",
			3
		},
		{
			"Adreno (TM) 220",
			3
		},
		{
			"Adreno (TM) 220 / Chainfire3D",
			3
		},
		{
			"Vivante GC1000",
			3
		},
		{
			"Adreno (TM) 203",
			2
		},
		{
			"Adreno (TM) 205",
			2
		},
		{
			"PowerVR SGX 531 / Chainfire3D",
			2
		},
		{
			"PowerVR SGX 540",
			2
		},
		{
			"PowerVR SGX 540 / Chainfire3D",
			2
		},
		{
			"Adreno (TM) 200",
			1
		},
		{
			"Adreno (TM) 200 / Chainfire3D",
			1
		},
		{
			"Immersion.16",
			1
		},
		{
			"Immersion.16 / Chainfire3D",
			1
		},
		{
			"Bluestacks",
			1
		},
		{
			"GC800 core",
			1
		},
		{
			"GC800 core / Chainfire3D",
			1
		},
		{
			"Mali-200",
			1
		},
		{
			"Mali-300",
			1
		},
		{
			"GC400 core",
			1
		},
		{
			"S5 Multicore c",
			1
		},
		{
			"PowerVR SGX530",
			1
		},
		{
			"PowerVR SGX 530",
			1
		},
		{
			"PowerVR SGX 531",
			1
		},
		{
			"PowerVR SGX 535",
			1
		},
		{
			"PowerVR SGX 543",
			1
		}
	};

	// Token: 0x040025C9 RID: 9673
	public static bool isPixelGunLow = true;
}
