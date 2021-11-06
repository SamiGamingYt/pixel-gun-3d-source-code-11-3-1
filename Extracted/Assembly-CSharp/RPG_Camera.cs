using System;
using UnityEngine;

// Token: 0x020007D6 RID: 2006
public class RPG_Camera : MonoBehaviour
{
	// Token: 0x060048C0 RID: 18624 RVA: 0x001938CC File Offset: 0x00191ACC
	private void Awake()
	{
		RPG_Camera.instance = this;
		RPG_Camera.cam = base.GetComponent<Camera>();
		this.nearClipPlaneNormal = RPG_Camera.cam.nearClipPlane;
	}

	// Token: 0x060048C1 RID: 18625 RVA: 0x001938F0 File Offset: 0x00191AF0
	private void Start()
	{
		this.distance = Mathf.Clamp(this.distance, this.distanceMin, this.distanceMax);
		this.desiredDistance = this.distance;
		RPG_Camera.halfFieldOfView = RPG_Camera.cam.fieldOfView / 2f * 0.017453292f;
		RPG_Camera.planeAspect = RPG_Camera.cam.aspect;
		RPG_Camera.halfPlaneHeight = RPG_Camera.cam.nearClipPlane * Mathf.Tan(RPG_Camera.halfFieldOfView);
		RPG_Camera.halfPlaneWidth = RPG_Camera.halfPlaneHeight * RPG_Camera.planeAspect;
		this.mouseY = 15f;
	}

	// Token: 0x060048C2 RID: 18626 RVA: 0x00193988 File Offset: 0x00191B88
	private void OnDestroy()
	{
		RPG_Camera.cam = null;
	}

	// Token: 0x060048C3 RID: 18627 RVA: 0x00193990 File Offset: 0x00191B90
	public static void CameraSetup()
	{
		GameObject gameObject;
		if (RPG_Camera.cam != null)
		{
			gameObject = RPG_Camera.cam.gameObject;
		}
		else
		{
			gameObject = new GameObject("Main Camera");
			gameObject.AddComponent<Camera>();
			gameObject.tag = "MainCamera";
		}
		if (!gameObject.GetComponent("RPG_Camera"))
		{
			gameObject.AddComponent<RPG_Camera>();
		}
		RPG_Camera rpg_Camera = gameObject.GetComponent("RPG_Camera") as RPG_Camera;
		GameObject gameObject2 = GameObject.Find("cameraPivot");
		rpg_Camera.cameraPivot = gameObject2.transform;
	}

	// Token: 0x060048C4 RID: 18628 RVA: 0x00193A20 File Offset: 0x00191C20
	private void Update()
	{
		if (this.cameraPivot == null)
		{
			return;
		}
		if (!RotatorKillCam.isDraggin && this.distance < 2f)
		{
			this.deltaMouseX += Time.deltaTime * this.speedRotateXfree;
		}
		this.curTargetEulerAngles = this.cameraPivot.eulerAngles;
		this.GetInput();
		this.GetDesiredPosition();
		this.PositionUpdate();
	}

	// Token: 0x060048C5 RID: 18629 RVA: 0x00193A98 File Offset: 0x00191C98
	private void GetInput()
	{
		if ((double)this.distance > 0.1)
		{
			this.camBottom = Physics.Linecast(base.transform.position, base.transform.position - Vector3.up * this.camBottomDistance, this.collisionLayer);
		}
		bool flag = this.camBottom && base.transform.position.y - this.cameraPivot.transform.position.y <= 0f;
		this.mouseY = this.ClampAngle(this.mouseY, -89.5f, 89.5f);
		this.mouseXSmooth = Mathf.SmoothDamp(this.mouseXSmooth, this.mouseX, ref this.mouseXVel, this.mouseSmoothingFactor);
		this.mouseYSmooth = Mathf.SmoothDamp(this.mouseYSmooth, this.mouseY, ref this.mouseYVel, this.mouseSmoothingFactor);
		if (flag)
		{
			this.mouseYMin = this.mouseY;
		}
		else
		{
			this.mouseYMin = -89.5f;
		}
		this.mouseYSmooth = this.ClampAngle(this.mouseYSmooth, this.mouseYMin, this.mouseYMax);
		if (this.desiredDistance > this.distanceMax)
		{
			this.desiredDistance = this.distanceMax;
		}
		if (this.desiredDistance < this.distanceMin)
		{
			this.desiredDistance = this.distanceMin;
		}
		this.controlVector = Vector2.zero;
	}

	// Token: 0x060048C6 RID: 18630 RVA: 0x00193C28 File Offset: 0x00191E28
	private void GetDesiredPosition()
	{
		this.distance = this.desiredDistance;
		this.desiredPosition = this.GetCameraPosition(this.mouseYSmooth, this.mouseX, this.distance);
		this.constraint = false;
		float num = this.CheckCameraClipPlane(this.cameraPivot.position, this.desiredPosition);
		if (num != -1f)
		{
			this.distance = num;
			this.desiredPosition = this.GetCameraPosition(this.mouseYSmooth, this.mouseX, this.distance);
			this.constraint = true;
		}
		this.distance -= RPG_Camera.cam.nearClipPlane;
		if (this.lastDistance < this.distance || !this.constraint)
		{
			this.distance = Mathf.SmoothDamp(this.lastDistance, this.distance, ref this.distanceVel, this.camDistanceSpeed);
		}
		if (this.distance < this.distanceMin)
		{
			this.distance = this.distanceMin;
		}
		this.lastDistance = this.distance;
		this.desiredPosition = this.GetCameraPosition(this.mouseYSmooth, this.mouseX, this.distance);
		if (this.distance < 4f && this.hitInfo.normal != Vector3.zero)
		{
			this.desiredPosition -= this.hitInfo.normal * this.offsetY * (4f - this.distance) * 0.25f;
			this.isCameraIntersect = true;
		}
		else
		{
			this.isCameraIntersect = false;
			if (RPG_Camera.cam.nearClipPlane != this.nearClipPlaneNormal)
			{
				RPG_Camera.cam.nearClipPlane = this.nearClipPlaneNormal;
			}
		}
	}

	// Token: 0x060048C7 RID: 18631 RVA: 0x00193DF8 File Offset: 0x00191FF8
	private void PositionUpdate()
	{
		base.transform.position = this.desiredPosition;
		if (this.distance > this.distanceMin)
		{
			base.transform.LookAt(this.cameraPivot);
			base.transform.eulerAngles -= new Vector3(2f, 0f, 0f);
		}
	}

	// Token: 0x060048C8 RID: 18632 RVA: 0x00193E64 File Offset: 0x00192064
	public void UpdateMouseX()
	{
		if (this.cameraPivot == null)
		{
			return;
		}
		this.mouseX = 150f + this.deltaMouseX + this.cameraPivot.rotation.eulerAngles.y;
		while (this.mouseX > 360f)
		{
			this.mouseX -= 360f;
		}
	}

	// Token: 0x060048C9 RID: 18633 RVA: 0x00193ED8 File Offset: 0x001920D8
	private void CharacterFade()
	{
		if (RPG_Animation.instance == null)
		{
			return;
		}
		if (this.distance < this.firstPersonThreshold)
		{
			RPG_Animation.instance.GetComponent<Renderer>().enabled = false;
		}
		else if (this.distance < this.characterFadeThreshold)
		{
			RPG_Animation.instance.GetComponent<Renderer>().enabled = true;
			float num = 1f - (this.characterFadeThreshold - this.distance) / (this.characterFadeThreshold - this.firstPersonThreshold);
			if (RPG_Animation.instance.GetComponent<Renderer>().material.color.a != num)
			{
				RPG_Animation.instance.GetComponent<Renderer>().material.color = new Color(RPG_Animation.instance.GetComponent<Renderer>().material.color.r, RPG_Animation.instance.GetComponent<Renderer>().material.color.g, RPG_Animation.instance.GetComponent<Renderer>().material.color.b, num);
			}
		}
		else
		{
			RPG_Animation.instance.GetComponent<Renderer>().enabled = true;
			if (RPG_Animation.instance.GetComponent<Renderer>().material.color.a != 1f)
			{
				RPG_Animation.instance.GetComponent<Renderer>().material.color = new Color(RPG_Animation.instance.GetComponent<Renderer>().material.color.r, RPG_Animation.instance.GetComponent<Renderer>().material.color.g, RPG_Animation.instance.GetComponent<Renderer>().material.color.b, 1f);
			}
		}
	}

	// Token: 0x060048CA RID: 18634 RVA: 0x001940A4 File Offset: 0x001922A4
	private Vector3 GetCameraPosition(float xAxis, float yAxis, float distance)
	{
		Vector3 point = new Vector3(0f, 0f, -distance);
		Quaternion rotation = Quaternion.Euler(xAxis, yAxis, 0f);
		return this.cameraPivot.position + rotation * point;
	}

	// Token: 0x060048CB RID: 18635 RVA: 0x001940E8 File Offset: 0x001922E8
	private float CheckCameraClipPlane(Vector3 from, Vector3 to)
	{
		float num = -1f;
		RPG_Camera.ClipPlaneVertexes clipPlaneAt = this.GetClipPlaneAt(to);
		if (Physics.Linecast(from, to, out this.hitInfo, this.collisionLayer) && this.IsIgnorCollider(this.hitInfo))
		{
			num = this.hitInfo.distance - RPG_Camera.cam.nearClipPlane;
		}
		else if (Physics.Linecast(from - base.transform.right * RPG_Camera.halfPlaneWidth + base.transform.up * RPG_Camera.halfPlaneHeight, clipPlaneAt.UpperLeft, out this.hitInfo, this.collisionLayer) && this.IsIgnorCollider(this.hitInfo))
		{
			if (this.hitInfo.distance < num || num == -1f)
			{
				num = Vector3.Distance(this.hitInfo.point + base.transform.right * RPG_Camera.halfPlaneWidth - base.transform.up * RPG_Camera.halfPlaneHeight, from);
			}
		}
		else if (Physics.Linecast(from + base.transform.right * RPG_Camera.halfPlaneWidth + base.transform.up * RPG_Camera.halfPlaneHeight, clipPlaneAt.UpperRight, out this.hitInfo, this.collisionLayer) && this.IsIgnorCollider(this.hitInfo))
		{
			if (this.hitInfo.distance < num || num == -1f)
			{
				num = Vector3.Distance(this.hitInfo.point - base.transform.right * RPG_Camera.halfPlaneWidth - base.transform.up * RPG_Camera.halfPlaneHeight, from);
			}
		}
		else if (Physics.Linecast(from - base.transform.right * RPG_Camera.halfPlaneWidth - base.transform.up * RPG_Camera.halfPlaneHeight, clipPlaneAt.LowerLeft, out this.hitInfo, this.collisionLayer) && this.IsIgnorCollider(this.hitInfo))
		{
			if (this.hitInfo.distance < num || num == -1f)
			{
				num = Vector3.Distance(this.hitInfo.point + base.transform.right * RPG_Camera.halfPlaneWidth + base.transform.up * RPG_Camera.halfPlaneHeight, from);
			}
		}
		else if (Physics.Linecast(from + base.transform.right * RPG_Camera.halfPlaneWidth - base.transform.up * RPG_Camera.halfPlaneHeight, clipPlaneAt.LowerRight, out this.hitInfo, this.collisionLayer) && this.IsIgnorCollider(this.hitInfo) && (this.hitInfo.distance < num || num == -1f))
		{
			num = Vector3.Distance(this.hitInfo.point - base.transform.right * RPG_Camera.halfPlaneWidth + base.transform.up * RPG_Camera.halfPlaneHeight, from);
		}
		return num;
	}

	// Token: 0x060048CC RID: 18636 RVA: 0x00194488 File Offset: 0x00192688
	private bool IsIgnorCollider(RaycastHit curHitInfo)
	{
		return curHitInfo.collider.tag != "Player" && curHitInfo.collider.tag != "Vision" && curHitInfo.collider.tag != "colliderPoint" && curHitInfo.collider.tag != "Helicopter";
	}

	// Token: 0x060048CD RID: 18637 RVA: 0x00194504 File Offset: 0x00192704
	private float ClampAngle(float angle, float min, float max)
	{
		while (angle < -360f || angle > 360f)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x060048CE RID: 18638 RVA: 0x0019455C File Offset: 0x0019275C
	public RPG_Camera.ClipPlaneVertexes GetClipPlaneAt(Vector3 pos)
	{
		RPG_Camera.ClipPlaneVertexes result = default(RPG_Camera.ClipPlaneVertexes);
		if (RPG_Camera.cam == null)
		{
			return result;
		}
		Transform transform = RPG_Camera.cam.transform;
		float nearClipPlane = RPG_Camera.cam.nearClipPlane;
		result.UpperLeft = pos - transform.right * RPG_Camera.halfPlaneWidth;
		result.UpperLeft += transform.up * RPG_Camera.halfPlaneHeight;
		result.UpperLeft += transform.forward * nearClipPlane;
		result.UpperRight = pos + transform.right * RPG_Camera.halfPlaneWidth;
		result.UpperRight += transform.up * RPG_Camera.halfPlaneHeight;
		result.UpperRight += transform.forward * nearClipPlane;
		result.LowerLeft = pos - transform.right * RPG_Camera.halfPlaneWidth;
		result.LowerLeft -= transform.up * RPG_Camera.halfPlaneHeight;
		result.LowerLeft += transform.forward * nearClipPlane;
		result.LowerRight = pos + transform.right * RPG_Camera.halfPlaneWidth;
		result.LowerRight -= transform.up * RPG_Camera.halfPlaneHeight;
		result.LowerRight += transform.forward * nearClipPlane;
		return result;
	}

	// Token: 0x060048CF RID: 18639 RVA: 0x00194710 File Offset: 0x00192910
	public void RotateWithCharacter()
	{
		float num = Input.GetAxis("Horizontal") * RPG_Controller.instance.turnSpeed;
		this.mouseX += num;
	}

	// Token: 0x040035B3 RID: 13747
	public static RPG_Camera instance;

	// Token: 0x040035B4 RID: 13748
	public Transform cameraPivot;

	// Token: 0x040035B5 RID: 13749
	public float TimeRotationCam = 15f;

	// Token: 0x040035B6 RID: 13750
	public float distance = 5f;

	// Token: 0x040035B7 RID: 13751
	public float distanceMin = 1f;

	// Token: 0x040035B8 RID: 13752
	public float distanceMax = 30f;

	// Token: 0x040035B9 RID: 13753
	public float mouseSpeed = 8f;

	// Token: 0x040035BA RID: 13754
	public float mouseScroll = 15f;

	// Token: 0x040035BB RID: 13755
	public float mouseSmoothingFactor = 0.08f;

	// Token: 0x040035BC RID: 13756
	public float camDistanceSpeed = 0.7f;

	// Token: 0x040035BD RID: 13757
	public float camBottomDistance = 1f;

	// Token: 0x040035BE RID: 13758
	public float firstPersonThreshold = 0.8f;

	// Token: 0x040035BF RID: 13759
	public float characterFadeThreshold = 1.8f;

	// Token: 0x040035C0 RID: 13760
	private float speedRotateXfree = 180f;

	// Token: 0x040035C1 RID: 13761
	public bool isDragging;

	// Token: 0x040035C2 RID: 13762
	private Vector3 desiredPosition;

	// Token: 0x040035C3 RID: 13763
	public float desiredDistance;

	// Token: 0x040035C4 RID: 13764
	public float offsetMaxDistance;

	// Token: 0x040035C5 RID: 13765
	public float offsetY;

	// Token: 0x040035C6 RID: 13766
	public float lastDistance;

	// Token: 0x040035C7 RID: 13767
	public float mouseX;

	// Token: 0x040035C8 RID: 13768
	public float deltaMouseX;

	// Token: 0x040035C9 RID: 13769
	private float mouseXSmooth;

	// Token: 0x040035CA RID: 13770
	private float mouseXVel;

	// Token: 0x040035CB RID: 13771
	public float mouseY;

	// Token: 0x040035CC RID: 13772
	public float mouseYSmooth;

	// Token: 0x040035CD RID: 13773
	private float mouseYVel;

	// Token: 0x040035CE RID: 13774
	private float mouseYMin = -89.5f;

	// Token: 0x040035CF RID: 13775
	private float mouseYMax = 89.5f;

	// Token: 0x040035D0 RID: 13776
	private float distanceVel;

	// Token: 0x040035D1 RID: 13777
	private bool camBottom;

	// Token: 0x040035D2 RID: 13778
	private bool constraint;

	// Token: 0x040035D3 RID: 13779
	private static float halfFieldOfView;

	// Token: 0x040035D4 RID: 13780
	private static float planeAspect;

	// Token: 0x040035D5 RID: 13781
	private static float halfPlaneHeight;

	// Token: 0x040035D6 RID: 13782
	private static float halfPlaneWidth;

	// Token: 0x040035D7 RID: 13783
	public Vector2 controlVector;

	// Token: 0x040035D8 RID: 13784
	public Vector3 curTargetEulerAngles;

	// Token: 0x040035D9 RID: 13785
	private bool enabledSledCamera = true;

	// Token: 0x040035DA RID: 13786
	private bool isCameraIntersect;

	// Token: 0x040035DB RID: 13787
	private RaycastHit hitInfo;

	// Token: 0x040035DC RID: 13788
	[HideInInspector]
	public static Camera cam;

	// Token: 0x040035DD RID: 13789
	private float nearClipPlaneNormal;

	// Token: 0x040035DE RID: 13790
	public LayerMask collisionLayer;

	// Token: 0x020007D7 RID: 2007
	public struct ClipPlaneVertexes
	{
		// Token: 0x040035DF RID: 13791
		public Vector3 UpperLeft;

		// Token: 0x040035E0 RID: 13792
		public Vector3 UpperRight;

		// Token: 0x040035E1 RID: 13793
		public Vector3 LowerLeft;

		// Token: 0x040035E2 RID: 13794
		public Vector3 LowerRight;
	}
}
