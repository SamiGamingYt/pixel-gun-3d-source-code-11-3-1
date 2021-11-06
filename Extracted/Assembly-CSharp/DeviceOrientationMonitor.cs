using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x020005DE RID: 1502
internal sealed class DeviceOrientationMonitor : MonoBehaviour
{
	// Token: 0x06003380 RID: 13184 RVA: 0x0010AB98 File Offset: 0x00108D98
	// Note: this type is marked as 'beforefieldinit'.
	static DeviceOrientationMonitor()
	{
		DeviceOrientationMonitor.OnOrientationChange = delegate(DeviceOrientation o)
		{
		};
	}

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x06003381 RID: 13185 RVA: 0x0010ABD4 File Offset: 0x00108DD4
	// (remove) Token: 0x06003382 RID: 13186 RVA: 0x0010ABEC File Offset: 0x00108DEC
	public static event Action<DeviceOrientation> OnOrientationChange;

	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x06003383 RID: 13187 RVA: 0x0010AC04 File Offset: 0x00108E04
	// (set) Token: 0x06003384 RID: 13188 RVA: 0x0010AC0C File Offset: 0x00108E0C
	public static DeviceOrientation CurrentOrientation { get; private set; }

	// Token: 0x06003385 RID: 13189 RVA: 0x0010AC14 File Offset: 0x00108E14
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06003386 RID: 13190 RVA: 0x0010AC1C File Offset: 0x00108E1C
	private void OnEnable()
	{
		base.StartCoroutine(this.CheckForChange());
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x0010AC2C File Offset: 0x00108E2C
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x0010AC34 File Offset: 0x00108E34
	private IEnumerator CheckForChange()
	{
		DeviceOrientationMonitor.CurrentOrientation = Input.deviceOrientation;
		WaitForRealSeconds delay = new WaitForRealSeconds(DeviceOrientationMonitor.CheckDelay);
		for (;;)
		{
			DeviceOrientation deviceOrientation = Input.deviceOrientation;
			if (deviceOrientation == DeviceOrientation.LandscapeLeft || deviceOrientation == DeviceOrientation.LandscapeRight)
			{
				if (DeviceOrientationMonitor.CurrentOrientation != Input.deviceOrientation)
				{
					DeviceOrientationMonitor.CurrentOrientation = Input.deviceOrientation;
					DeviceOrientationMonitor.OnOrientationChange(DeviceOrientationMonitor.CurrentOrientation);
				}
			}
			yield return delay;
		}
		yield break;
	}

	// Token: 0x040025CD RID: 9677
	public static float CheckDelay = 0.5f;
}
