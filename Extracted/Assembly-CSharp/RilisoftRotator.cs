using System;
using Rilisoft;
using UnityEngine;

// Token: 0x020004CE RID: 1230
public class RilisoftRotator : MonoBehaviour
{
	// Token: 0x06002BED RID: 11245 RVA: 0x000E71E8 File Offset: 0x000E53E8
	public static void RotateCharacter(Transform character, float rotationRate, Rect touchZone, ref float idleTimerStartedTime, ref float lastTimeRotated, Func<bool> canProcess = null)
	{
		if (canProcess == null || canProcess())
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Moved && touchZone.Contains(touch.position))
				{
					idleTimerStartedTime = Time.realtimeSinceStartup;
					character.Rotate(Vector3.up, touch.deltaPosition.x * rotationRate * 0.5f * (Time.realtimeSinceStartup - lastTimeRotated));
				}
			}
			if (Application.isEditor)
			{
				float num = Input.GetAxis("Mouse ScrollWheel") * 10f * rotationRate * (Time.realtimeSinceStartup - lastTimeRotated);
				character.Rotate(Vector3.up, num);
				if (num != 0f)
				{
					idleTimerStartedTime = Time.realtimeSinceStartup;
				}
			}
		}
		lastTimeRotated = Time.realtimeSinceStartup;
	}

	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x06002BEE RID: 11246 RVA: 0x000E72BC File Offset: 0x000E54BC
	public static float RotationRateForCharacterInMenues
	{
		get
		{
			float num = -120f;
			return num * ((BuildSettings.BuildTargetPlatform != RuntimePlatform.Android) ? 0.5f : 2f);
		}
	}

	// Token: 0x06002BEF RID: 11247 RVA: 0x000E72F0 File Offset: 0x000E54F0
	private void Start()
	{
		this._transform = base.transform;
	}

	// Token: 0x06002BF0 RID: 11248 RVA: 0x000E7300 File Offset: 0x000E5500
	private void Update()
	{
		this._transform.Rotate(Vector3.forward, this.rate * Time.deltaTime, Space.Self);
	}

	// Token: 0x040020DA RID: 8410
	public float rate = 10f;

	// Token: 0x040020DB RID: 8411
	private Transform _transform;
}
