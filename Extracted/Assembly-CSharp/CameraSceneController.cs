using System;
using UnityEngine;

// Token: 0x02000056 RID: 86
public sealed class CameraSceneController : MonoBehaviour
{
	// Token: 0x06000230 RID: 560 RVA: 0x00013BE0 File Offset: 0x00011DE0
	private void Awake()
	{
		CameraSceneController.sharedController = this;
		this.myTransform = base.transform;
		this.EnableSounds = false;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x00013BFC File Offset: 0x00011DFC
	private void Start()
	{
		SceneInfo infoScene = SceneInfoController.instance.GetInfoScene(Application.loadedLevelName);
		if (infoScene != null)
		{
			this.posCam = infoScene.positionCam;
			this.rotateCam = Quaternion.Euler(infoScene.rotationCam);
		}
		this.myTransform.position = this.posCam;
		this.myTransform.rotation = this.rotateCam;
		this.killCamController.enabled = false;
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000232 RID: 562 RVA: 0x00013C70 File Offset: 0x00011E70
	// (set) Token: 0x06000233 RID: 563 RVA: 0x00013C9C File Offset: 0x00011E9C
	public bool EnableSounds
	{
		get
		{
			return this.objListener.localPosition.Equals(Vector3.zero);
		}
		set
		{
			if (value)
			{
				this.objListener.localPosition = Vector3.zero;
			}
			else
			{
				this.objListener.localPosition = new Vector3(0f, 10000f, 0f);
			}
		}
	}

	// Token: 0x06000234 RID: 564 RVA: 0x00013CE4 File Offset: 0x00011EE4
	public void SetTargetKillCam(Transform target = null)
	{
		if (target == null)
		{
			this.killCamController.enabled = false;
			this.killCamController.cameraPivot = null;
			this.myTransform.position = this.posCam;
			this.myTransform.rotation = this.rotateCam;
			this.EnableSounds = false;
		}
		else
		{
			this.killCamController.enabled = true;
			this.killCamController.cameraPivot = target;
			this.myTransform.position = target.position;
			this.myTransform.rotation = target.rotation;
			this.EnableSounds = true;
		}
	}

	// Token: 0x06000235 RID: 565 RVA: 0x00013D84 File Offset: 0x00011F84
	private void OnDestroy()
	{
		CameraSceneController.sharedController = null;
	}

	// Token: 0x04000250 RID: 592
	public static CameraSceneController sharedController;

	// Token: 0x04000251 RID: 593
	private Vector3 posCam = new Vector3(17f, 11f, 17f);

	// Token: 0x04000252 RID: 594
	private Quaternion rotateCam = Quaternion.Euler(new Vector3(39f, 226f, 0f));

	// Token: 0x04000253 RID: 595
	private Transform myTransform;

	// Token: 0x04000254 RID: 596
	public RPG_Camera killCamController;

	// Token: 0x04000255 RID: 597
	public Transform objListener;
}
