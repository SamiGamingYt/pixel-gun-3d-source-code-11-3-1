using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000854 RID: 2132
[AddComponentMenu("Tasharen/Water")]
[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class TasharenWater : MonoBehaviour
{
	// Token: 0x17000CAB RID: 3243
	// (get) Token: 0x06004D3A RID: 19770 RVA: 0x001BDDCC File Offset: 0x001BBFCC
	public int reflectionTextureSize
	{
		get
		{
			switch (this.quality)
			{
			case TasharenWater.Quality.Medium:
			case TasharenWater.Quality.High:
				return 512;
			case TasharenWater.Quality.Uber:
				return 1024;
			default:
				return 0;
			}
		}
	}

	// Token: 0x17000CAC RID: 3244
	// (get) Token: 0x06004D3B RID: 19771 RVA: 0x001BDE08 File Offset: 0x001BC008
	public LayerMask reflectionMask
	{
		get
		{
			switch (this.quality)
			{
			case TasharenWater.Quality.Medium:
				return this.mediumReflectionMask;
			case TasharenWater.Quality.High:
			case TasharenWater.Quality.Uber:
				return this.highReflectionMask;
			default:
				return 0;
			}
		}
	}

	// Token: 0x17000CAD RID: 3245
	// (get) Token: 0x06004D3C RID: 19772 RVA: 0x001BDE4C File Offset: 0x001BC04C
	public bool useRefraction
	{
		get
		{
			return this.quality > TasharenWater.Quality.Fastest;
		}
	}

	// Token: 0x06004D3D RID: 19773 RVA: 0x001BDE58 File Offset: 0x001BC058
	private static float SignExt(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	// Token: 0x06004D3E RID: 19774 RVA: 0x001BDE84 File Offset: 0x001BC084
	private static void CalculateObliqueMatrix(ref Matrix4x4 projection, Vector4 clipPlane)
	{
		Vector4 b = projection.inverse * new Vector4(TasharenWater.SignExt(clipPlane.x), TasharenWater.SignExt(clipPlane.y), 1f, 1f);
		Vector4 vector = clipPlane * (2f / Vector4.Dot(clipPlane, b));
		projection[2] = vector.x - projection[3];
		projection[6] = vector.y - projection[7];
		projection[10] = vector.z - projection[11];
		projection[14] = vector.w - projection[15];
	}

	// Token: 0x06004D3F RID: 19775 RVA: 0x001BDF34 File Offset: 0x001BC134
	private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
	{
		reflectionMat.m00 = 1f - 2f * plane[0] * plane[0];
		reflectionMat.m01 = -2f * plane[0] * plane[1];
		reflectionMat.m02 = -2f * plane[0] * plane[2];
		reflectionMat.m03 = -2f * plane[3] * plane[0];
		reflectionMat.m10 = -2f * plane[1] * plane[0];
		reflectionMat.m11 = 1f - 2f * plane[1] * plane[1];
		reflectionMat.m12 = -2f * plane[1] * plane[2];
		reflectionMat.m13 = -2f * plane[3] * plane[1];
		reflectionMat.m20 = -2f * plane[2] * plane[0];
		reflectionMat.m21 = -2f * plane[2] * plane[1];
		reflectionMat.m22 = 1f - 2f * plane[2] * plane[2];
		reflectionMat.m23 = -2f * plane[3] * plane[2];
		reflectionMat.m30 = 0f;
		reflectionMat.m31 = 0f;
		reflectionMat.m32 = 0f;
		reflectionMat.m33 = 1f;
	}

	// Token: 0x06004D40 RID: 19776 RVA: 0x001BE0DC File Offset: 0x001BC2DC
	public static TasharenWater.Quality GetQuality()
	{
		return (TasharenWater.Quality)PlayerPrefs.GetInt("Water", 3);
	}

	// Token: 0x06004D41 RID: 19777 RVA: 0x001BE0F8 File Offset: 0x001BC2F8
	public static void SetQuality(TasharenWater.Quality q)
	{
		TasharenWater[] array = UnityEngine.Object.FindObjectsOfType(typeof(TasharenWater)) as TasharenWater[];
		if (array.Length > 0)
		{
			foreach (TasharenWater tasharenWater in array)
			{
				tasharenWater.quality = q;
			}
		}
		else
		{
			PlayerPrefs.SetInt("Water", (int)q);
		}
	}

	// Token: 0x06004D42 RID: 19778 RVA: 0x001BE154 File Offset: 0x001BC354
	private void Awake()
	{
		this.mTrans = base.transform;
		this.mRen = base.GetComponent<Renderer>();
		this.quality = TasharenWater.GetQuality();
	}

	// Token: 0x06004D43 RID: 19779 RVA: 0x001BE17C File Offset: 0x001BC37C
	private void OnDisable()
	{
		this.Clear();
		foreach (object obj in this.mCameras)
		{
			UnityEngine.Object.DestroyImmediate(((Camera)((DictionaryEntry)obj).Value).gameObject);
		}
		this.mCameras.Clear();
	}

	// Token: 0x06004D44 RID: 19780 RVA: 0x001BE20C File Offset: 0x001BC40C
	private void Clear()
	{
		if (this.mTex)
		{
			UnityEngine.Object.DestroyImmediate(this.mTex);
			this.mTex = null;
		}
	}

	// Token: 0x06004D45 RID: 19781 RVA: 0x001BE23C File Offset: 0x001BC43C
	private void CopyCamera(Camera src, Camera dest)
	{
		if (src.clearFlags == CameraClearFlags.Skybox)
		{
			Skybox component = src.GetComponent<Skybox>();
			Skybox component2 = dest.GetComponent<Skybox>();
			if (!component || !component.material)
			{
				component2.enabled = false;
			}
			else
			{
				component2.enabled = true;
				component2.material = component.material;
			}
		}
		dest.clearFlags = src.clearFlags;
		dest.backgroundColor = src.backgroundColor;
		dest.farClipPlane = src.farClipPlane;
		dest.nearClipPlane = src.nearClipPlane;
		dest.orthographic = src.orthographic;
		dest.fieldOfView = src.fieldOfView;
		dest.aspect = src.aspect;
		dest.orthographicSize = src.orthographicSize;
		dest.depthTextureMode = DepthTextureMode.None;
		dest.renderingPath = RenderingPath.Forward;
	}

	// Token: 0x06004D46 RID: 19782 RVA: 0x001BE30C File Offset: 0x001BC50C
	private Camera GetReflectionCamera(Camera current, Material mat, int textureSize)
	{
		if (!this.mTex || this.mTexSize != textureSize)
		{
			if (this.mTex)
			{
				UnityEngine.Object.DestroyImmediate(this.mTex);
			}
			this.mTex = new RenderTexture(textureSize, textureSize, 16);
			this.mTex.name = "__MirrorReflection" + base.GetInstanceID();
			this.mTex.isPowerOfTwo = true;
			this.mTex.hideFlags = HideFlags.DontSave;
			this.mTexSize = textureSize;
		}
		Camera camera = this.mCameras[current] as Camera;
		if (!camera)
		{
			camera = new GameObject(string.Concat(new object[]
			{
				"Mirror Refl Camera id",
				base.GetInstanceID(),
				" for ",
				current.GetInstanceID()
			}), new Type[]
			{
				typeof(Camera),
				typeof(Skybox)
			})
			{
				hideFlags = HideFlags.HideAndDontSave
			}.GetComponent<Camera>();
			camera.enabled = false;
			Transform transform = camera.transform;
			transform.position = this.mTrans.position;
			transform.rotation = this.mTrans.rotation;
			camera.gameObject.AddComponent<FlareLayer>();
			this.mCameras[current] = camera;
		}
		if (mat.HasProperty("_ReflectionTex"))
		{
			mat.SetTexture("_ReflectionTex", this.mTex);
		}
		return camera;
	}

	// Token: 0x06004D47 RID: 19783 RVA: 0x001BE494 File Offset: 0x001BC694
	private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
	{
		Matrix4x4 worldToCameraMatrix = cam.worldToCameraMatrix;
		Vector3 lhs = worldToCameraMatrix.MultiplyPoint(pos);
		Vector3 rhs = worldToCameraMatrix.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(rhs.x, rhs.y, rhs.z, -Vector3.Dot(lhs, rhs));
	}

	// Token: 0x06004D48 RID: 19784 RVA: 0x001BE4EC File Offset: 0x001BC6EC
	private void LateUpdate()
	{
		if (this.keepUnderCamera)
		{
			Transform transform = Camera.main.transform;
			Vector3 position = transform.position;
			position.y = this.mTrans.position.y;
			if (this.mTrans.position != position)
			{
				this.mTrans.position = position;
			}
		}
	}

	// Token: 0x06004D49 RID: 19785 RVA: 0x001BE554 File Offset: 0x001BC754
	private void OnWillRenderObject()
	{
		if (TasharenWater.mIsRendering)
		{
			return;
		}
		if (!base.enabled || !this.mRen || !this.mRen.enabled)
		{
			this.Clear();
			return;
		}
		Material sharedMaterial = this.mRen.sharedMaterial;
		if (!sharedMaterial)
		{
			return;
		}
		this.quality = TasharenWater.Quality.Fastest;
		sharedMaterial.shader.maximumLOD = 100;
	}

	// Token: 0x04003BA5 RID: 15269
	public TasharenWater.Quality quality = TasharenWater.Quality.High;

	// Token: 0x04003BA6 RID: 15270
	public LayerMask highReflectionMask = -1;

	// Token: 0x04003BA7 RID: 15271
	public LayerMask mediumReflectionMask = -1;

	// Token: 0x04003BA8 RID: 15272
	public bool keepUnderCamera = true;

	// Token: 0x04003BA9 RID: 15273
	private Transform mTrans;

	// Token: 0x04003BAA RID: 15274
	private Hashtable mCameras = new Hashtable();

	// Token: 0x04003BAB RID: 15275
	private RenderTexture mTex;

	// Token: 0x04003BAC RID: 15276
	private int mTexSize;

	// Token: 0x04003BAD RID: 15277
	private Renderer mRen;

	// Token: 0x04003BAE RID: 15278
	private static bool mIsRendering;

	// Token: 0x02000855 RID: 2133
	public enum Quality
	{
		// Token: 0x04003BB0 RID: 15280
		Fastest,
		// Token: 0x04003BB1 RID: 15281
		Low,
		// Token: 0x04003BB2 RID: 15282
		Medium,
		// Token: 0x04003BB3 RID: 15283
		High,
		// Token: 0x04003BB4 RID: 15284
		Uber
	}
}
