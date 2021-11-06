using System;
using UnityEngine;

// Token: 0x02000379 RID: 889
public abstract class UIRect : MonoBehaviour
{
	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x06001F1A RID: 7962 RVA: 0x0008EDCC File Offset: 0x0008CFCC
	public GameObject cachedGameObject
	{
		get
		{
			if (this.mGo == null)
			{
				this.mGo = base.gameObject;
			}
			return this.mGo;
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06001F1B RID: 7963 RVA: 0x0008EDF4 File Offset: 0x0008CFF4
	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.transform;
			}
			return this.mTrans;
		}
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x06001F1C RID: 7964 RVA: 0x0008EE1C File Offset: 0x0008D01C
	public Camera anchorCamera
	{
		get
		{
			if (!this.mAnchorsCached)
			{
				this.ResetAnchors();
			}
			return this.mCam;
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06001F1D RID: 7965 RVA: 0x0008EE38 File Offset: 0x0008D038
	public bool isFullyAnchored
	{
		get
		{
			return this.leftAnchor.target && this.rightAnchor.target && this.topAnchor.target && this.bottomAnchor.target;
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x06001F1E RID: 7966 RVA: 0x0008EE98 File Offset: 0x0008D098
	public virtual bool isAnchoredHorizontally
	{
		get
		{
			return this.leftAnchor.target || this.rightAnchor.target;
		}
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06001F1F RID: 7967 RVA: 0x0008EED0 File Offset: 0x0008D0D0
	public virtual bool isAnchoredVertically
	{
		get
		{
			return this.bottomAnchor.target || this.topAnchor.target;
		}
	}

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x06001F20 RID: 7968 RVA: 0x0008EF08 File Offset: 0x0008D108
	public virtual bool canBeAnchored
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x06001F21 RID: 7969 RVA: 0x0008EF0C File Offset: 0x0008D10C
	public UIRect parent
	{
		get
		{
			if (!this.mParentFound)
			{
				this.mParentFound = true;
				this.mParent = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
			}
			return this.mParent;
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x06001F22 RID: 7970 RVA: 0x0008EF48 File Offset: 0x0008D148
	public UIRoot root
	{
		get
		{
			if (this.parent != null)
			{
				return this.mParent.root;
			}
			if (!this.mRootSet)
			{
				this.mRootSet = true;
				this.mRoot = NGUITools.FindInParents<UIRoot>(this.cachedTransform);
			}
			return this.mRoot;
		}
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x06001F23 RID: 7971 RVA: 0x0008EF9C File Offset: 0x0008D19C
	public bool isAnchored
	{
		get
		{
			return (this.leftAnchor.target || this.rightAnchor.target || this.topAnchor.target || this.bottomAnchor.target) && this.canBeAnchored;
		}
	}

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x06001F24 RID: 7972
	// (set) Token: 0x06001F25 RID: 7973
	public abstract float alpha { get; set; }

	// Token: 0x06001F26 RID: 7974
	public abstract float CalculateFinalAlpha(int frameID);

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06001F27 RID: 7975
	public abstract Vector3[] localCorners { get; }

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06001F28 RID: 7976
	public abstract Vector3[] worldCorners { get; }

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06001F29 RID: 7977 RVA: 0x0008F008 File Offset: 0x0008D208
	protected float cameraRayDistance
	{
		get
		{
			if (this.anchorCamera == null)
			{
				return 0f;
			}
			if (!this.mCam.orthographic)
			{
				Transform cachedTransform = this.cachedTransform;
				Transform transform = this.mCam.transform;
				Plane plane = new Plane(cachedTransform.rotation * Vector3.back, cachedTransform.position);
				Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);
				float result;
				if (plane.Raycast(ray, out result))
				{
					return result;
				}
			}
			return Mathf.Lerp(this.mCam.nearClipPlane, this.mCam.farClipPlane, 0.5f);
		}
	}

	// Token: 0x06001F2A RID: 7978 RVA: 0x0008F0BC File Offset: 0x0008D2BC
	public virtual void Invalidate(bool includeChildren)
	{
		this.mChanged = true;
		if (includeChildren)
		{
			for (int i = 0; i < this.mChildren.size; i++)
			{
				this.mChildren.buffer[i].Invalidate(true);
			}
		}
	}

	// Token: 0x06001F2B RID: 7979 RVA: 0x0008F108 File Offset: 0x0008D308
	public virtual Vector3[] GetSides(Transform relativeTo)
	{
		if (this.anchorCamera != null)
		{
			return this.mCam.GetSides(this.cameraRayDistance, relativeTo);
		}
		Vector3 position = this.cachedTransform.position;
		for (int i = 0; i < 4; i++)
		{
			UIRect.mSides[i] = position;
		}
		if (relativeTo != null)
		{
			for (int j = 0; j < 4; j++)
			{
				UIRect.mSides[j] = relativeTo.InverseTransformPoint(UIRect.mSides[j]);
			}
		}
		return UIRect.mSides;
	}

	// Token: 0x06001F2C RID: 7980 RVA: 0x0008F1B4 File Offset: 0x0008D3B4
	protected Vector3 GetLocalPos(UIRect.AnchorPoint ac, Transform trans)
	{
		if (this.anchorCamera == null || ac.targetCam == null)
		{
			return this.cachedTransform.localPosition;
		}
		Rect rect = ac.targetCam.rect;
		Vector3 vector = ac.targetCam.WorldToViewportPoint(ac.target.position);
		Vector3 vector2 = new Vector3(vector.x * rect.width + rect.x, vector.y * rect.height + rect.y, vector.z);
		vector2 = this.mCam.ViewportToWorldPoint(vector2);
		if (trans != null)
		{
			vector2 = trans.InverseTransformPoint(vector2);
		}
		vector2.x = Mathf.Floor(vector2.x + 0.5f);
		vector2.y = Mathf.Floor(vector2.y + 0.5f);
		return vector2;
	}

	// Token: 0x06001F2D RID: 7981 RVA: 0x0008F2A4 File Offset: 0x0008D4A4
	protected virtual void OnEnable()
	{
		this.mUpdateFrame = -1;
		if (this.updateAnchors == UIRect.AnchorUpdate.OnEnable)
		{
			this.mAnchorsCached = false;
			this.mUpdateAnchors = true;
		}
		if (this.mStarted)
		{
			this.OnInit();
		}
		this.mUpdateFrame = -1;
	}

	// Token: 0x06001F2E RID: 7982 RVA: 0x0008F2EC File Offset: 0x0008D4EC
	protected virtual void OnInit()
	{
		this.mChanged = true;
		this.mRootSet = false;
		this.mParentFound = false;
		if (this.parent != null)
		{
			this.mParent.mChildren.Add(this);
		}
	}

	// Token: 0x06001F2F RID: 7983 RVA: 0x0008F328 File Offset: 0x0008D528
	protected virtual void OnDisable()
	{
		if (this.mParent)
		{
			this.mParent.mChildren.Remove(this);
		}
		this.mParent = null;
		this.mRoot = null;
		this.mRootSet = false;
		this.mParentFound = false;
	}

	// Token: 0x06001F30 RID: 7984 RVA: 0x0008F374 File Offset: 0x0008D574
	protected virtual void Awake()
	{
		this.mStarted = false;
		this.mGo = base.gameObject;
		this.mTrans = base.transform;
	}

	// Token: 0x06001F31 RID: 7985 RVA: 0x0008F398 File Offset: 0x0008D598
	protected void Start()
	{
		this.mStarted = true;
		this.OnInit();
		this.OnStart();
	}

	// Token: 0x06001F32 RID: 7986 RVA: 0x0008F3B0 File Offset: 0x0008D5B0
	public void Update()
	{
		if (!this.mAnchorsCached)
		{
			this.ResetAnchors();
		}
		int frameCount = Time.frameCount;
		if (this.mUpdateFrame != frameCount)
		{
			if (this.updateAnchors == UIRect.AnchorUpdate.OnUpdate || this.mUpdateAnchors)
			{
				this.UpdateAnchorsInternal(frameCount);
			}
			this.OnUpdate();
		}
	}

	// Token: 0x06001F33 RID: 7987 RVA: 0x0008F404 File Offset: 0x0008D604
	protected void UpdateAnchorsInternal(int frame)
	{
		this.mUpdateFrame = frame;
		this.mUpdateAnchors = false;
		bool flag = false;
		if (this.leftAnchor.target)
		{
			flag = true;
			if (this.leftAnchor.rect != null && this.leftAnchor.rect.mUpdateFrame != frame)
			{
				this.leftAnchor.rect.Update();
			}
		}
		if (this.bottomAnchor.target)
		{
			flag = true;
			if (this.bottomAnchor.rect != null && this.bottomAnchor.rect.mUpdateFrame != frame)
			{
				this.bottomAnchor.rect.Update();
			}
		}
		if (this.rightAnchor.target)
		{
			flag = true;
			if (this.rightAnchor.rect != null && this.rightAnchor.rect.mUpdateFrame != frame)
			{
				this.rightAnchor.rect.Update();
			}
		}
		if (this.topAnchor.target)
		{
			flag = true;
			if (this.topAnchor.rect != null && this.topAnchor.rect.mUpdateFrame != frame)
			{
				this.topAnchor.rect.Update();
			}
		}
		if (flag)
		{
			this.OnAnchor();
		}
	}

	// Token: 0x06001F34 RID: 7988 RVA: 0x0008F57C File Offset: 0x0008D77C
	public void UpdateAnchors()
	{
		if (this.isAnchored)
		{
			this.mUpdateFrame = -1;
			this.mUpdateAnchors = true;
			this.UpdateAnchorsInternal(Time.frameCount);
		}
	}

	// Token: 0x06001F35 RID: 7989
	protected abstract void OnAnchor();

	// Token: 0x06001F36 RID: 7990 RVA: 0x0008F5B0 File Offset: 0x0008D7B0
	public void SetAnchor(Transform t)
	{
		this.leftAnchor.target = t;
		this.rightAnchor.target = t;
		this.topAnchor.target = t;
		this.bottomAnchor.target = t;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x06001F37 RID: 7991 RVA: 0x0008F5FC File Offset: 0x0008D7FC
	public void SetAnchor(GameObject go)
	{
		Transform target = (!(go != null)) ? null : go.transform;
		this.leftAnchor.target = target;
		this.rightAnchor.target = target;
		this.topAnchor.target = target;
		this.bottomAnchor.target = target;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x06001F38 RID: 7992 RVA: 0x0008F660 File Offset: 0x0008D860
	public void SetAnchor(GameObject go, int left, int bottom, int right, int top)
	{
		Transform target = (!(go != null)) ? null : go.transform;
		this.leftAnchor.target = target;
		this.rightAnchor.target = target;
		this.topAnchor.target = target;
		this.bottomAnchor.target = target;
		this.leftAnchor.relative = 0f;
		this.rightAnchor.relative = 1f;
		this.bottomAnchor.relative = 0f;
		this.topAnchor.relative = 1f;
		this.leftAnchor.absolute = left;
		this.rightAnchor.absolute = right;
		this.bottomAnchor.absolute = bottom;
		this.topAnchor.absolute = top;
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x06001F39 RID: 7993 RVA: 0x0008F734 File Offset: 0x0008D934
	public void ResetAnchors()
	{
		this.mAnchorsCached = true;
		this.leftAnchor.rect = ((!this.leftAnchor.target) ? null : this.leftAnchor.target.GetComponent<UIRect>());
		this.bottomAnchor.rect = ((!this.bottomAnchor.target) ? null : this.bottomAnchor.target.GetComponent<UIRect>());
		this.rightAnchor.rect = ((!this.rightAnchor.target) ? null : this.rightAnchor.target.GetComponent<UIRect>());
		this.topAnchor.rect = ((!this.topAnchor.target) ? null : this.topAnchor.target.GetComponent<UIRect>());
		this.mCam = NGUITools.FindCameraForLayer(this.cachedGameObject.layer);
		this.FindCameraFor(this.leftAnchor);
		this.FindCameraFor(this.bottomAnchor);
		this.FindCameraFor(this.rightAnchor);
		this.FindCameraFor(this.topAnchor);
		this.mUpdateAnchors = true;
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x0008F870 File Offset: 0x0008DA70
	[ContextMenu("Reset And Update Anchors")]
	public void ResetAndUpdateAnchors()
	{
		this.ResetAnchors();
		this.UpdateAnchors();
	}

	// Token: 0x06001F3B RID: 7995
	public abstract void SetRect(float x, float y, float width, float height);

	// Token: 0x06001F3C RID: 7996 RVA: 0x0008F880 File Offset: 0x0008DA80
	private void FindCameraFor(UIRect.AnchorPoint ap)
	{
		if (ap.target == null || ap.rect != null)
		{
			ap.targetCam = null;
		}
		else
		{
			ap.targetCam = NGUITools.FindCameraForLayer(ap.target.gameObject.layer);
		}
	}

	// Token: 0x06001F3D RID: 7997 RVA: 0x0008F8D8 File Offset: 0x0008DAD8
	public virtual void ParentHasChanged()
	{
		this.mParentFound = false;
		UIRect y = NGUITools.FindInParents<UIRect>(this.cachedTransform.parent);
		if (this.mParent != y)
		{
			if (this.mParent)
			{
				this.mParent.mChildren.Remove(this);
			}
			this.mParent = y;
			if (this.mParent)
			{
				this.mParent.mChildren.Add(this);
			}
			this.mRootSet = false;
		}
	}

	// Token: 0x06001F3E RID: 7998
	protected abstract void OnStart();

	// Token: 0x06001F3F RID: 7999 RVA: 0x0008F960 File Offset: 0x0008DB60
	protected virtual void OnUpdate()
	{
	}

	// Token: 0x040013B4 RID: 5044
	public UIRect.AnchorPoint leftAnchor = new UIRect.AnchorPoint();

	// Token: 0x040013B5 RID: 5045
	public UIRect.AnchorPoint rightAnchor = new UIRect.AnchorPoint(1f);

	// Token: 0x040013B6 RID: 5046
	public UIRect.AnchorPoint bottomAnchor = new UIRect.AnchorPoint();

	// Token: 0x040013B7 RID: 5047
	public UIRect.AnchorPoint topAnchor = new UIRect.AnchorPoint(1f);

	// Token: 0x040013B8 RID: 5048
	public UIRect.AnchorUpdate updateAnchors = UIRect.AnchorUpdate.OnUpdate;

	// Token: 0x040013B9 RID: 5049
	[NonSerialized]
	protected GameObject mGo;

	// Token: 0x040013BA RID: 5050
	[NonSerialized]
	protected Transform mTrans;

	// Token: 0x040013BB RID: 5051
	[NonSerialized]
	protected BetterList<UIRect> mChildren = new BetterList<UIRect>();

	// Token: 0x040013BC RID: 5052
	[NonSerialized]
	protected bool mChanged = true;

	// Token: 0x040013BD RID: 5053
	[NonSerialized]
	protected bool mParentFound;

	// Token: 0x040013BE RID: 5054
	[NonSerialized]
	private bool mUpdateAnchors = true;

	// Token: 0x040013BF RID: 5055
	[NonSerialized]
	private int mUpdateFrame = -1;

	// Token: 0x040013C0 RID: 5056
	[NonSerialized]
	private bool mAnchorsCached;

	// Token: 0x040013C1 RID: 5057
	[NonSerialized]
	private UIRoot mRoot;

	// Token: 0x040013C2 RID: 5058
	[NonSerialized]
	private UIRect mParent;

	// Token: 0x040013C3 RID: 5059
	[NonSerialized]
	private bool mRootSet;

	// Token: 0x040013C4 RID: 5060
	[NonSerialized]
	protected Camera mCam;

	// Token: 0x040013C5 RID: 5061
	protected bool mStarted;

	// Token: 0x040013C6 RID: 5062
	[NonSerialized]
	public float finalAlpha = 1f;

	// Token: 0x040013C7 RID: 5063
	protected static Vector3[] mSides = new Vector3[4];

	// Token: 0x0200037A RID: 890
	[Serializable]
	public class AnchorPoint
	{
		// Token: 0x06001F40 RID: 8000 RVA: 0x0008F964 File Offset: 0x0008DB64
		public AnchorPoint()
		{
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x0008F96C File Offset: 0x0008DB6C
		public AnchorPoint(float relative)
		{
			this.relative = relative;
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0008F97C File Offset: 0x0008DB7C
		public void Set(float relative, float absolute)
		{
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x0008F998 File Offset: 0x0008DB98
		public void Set(Transform target, float relative, float absolute)
		{
			this.target = target;
			this.relative = relative;
			this.absolute = Mathf.FloorToInt(absolute + 0.5f);
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x0008F9C8 File Offset: 0x0008DBC8
		public void SetToNearest(float abs0, float abs1, float abs2)
		{
			this.SetToNearest(0f, 0.5f, 1f, abs0, abs1, abs2);
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0008F9E4 File Offset: 0x0008DBE4
		public void SetToNearest(float rel0, float rel1, float rel2, float abs0, float abs1, float abs2)
		{
			float num = Mathf.Abs(abs0);
			float num2 = Mathf.Abs(abs1);
			float num3 = Mathf.Abs(abs2);
			if (num < num2 && num < num3)
			{
				this.Set(rel0, abs0);
			}
			else if (num2 < num && num2 < num3)
			{
				this.Set(rel1, abs1);
			}
			else
			{
				this.Set(rel2, abs2);
			}
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0008FA4C File Offset: 0x0008DC4C
		public void SetHorizontal(Transform parent, float localPos)
		{
			if (this.rect)
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float num = Mathf.Lerp(sides[0].x, sides[2].x, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - num + 0.5f);
			}
			else
			{
				Vector3 position = this.target.position;
				if (parent != null)
				{
					position = parent.InverseTransformPoint(position);
				}
				this.absolute = Mathf.FloorToInt(localPos - position.x + 0.5f);
			}
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0008FAF0 File Offset: 0x0008DCF0
		public void SetVertical(Transform parent, float localPos)
		{
			if (this.rect)
			{
				Vector3[] sides = this.rect.GetSides(parent);
				float num = Mathf.Lerp(sides[3].y, sides[1].y, this.relative);
				this.absolute = Mathf.FloorToInt(localPos - num + 0.5f);
			}
			else
			{
				Vector3 position = this.target.position;
				if (parent != null)
				{
					position = parent.InverseTransformPoint(position);
				}
				this.absolute = Mathf.FloorToInt(localPos - position.y + 0.5f);
			}
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0008FB94 File Offset: 0x0008DD94
		public Vector3[] GetSides(Transform relativeTo)
		{
			if (this.target != null)
			{
				if (this.rect != null)
				{
					return this.rect.GetSides(relativeTo);
				}
				if (this.target.GetComponent<Camera>() != null)
				{
					return this.target.GetComponent<Camera>().GetSides(relativeTo);
				}
			}
			return null;
		}

		// Token: 0x040013C8 RID: 5064
		public Transform target;

		// Token: 0x040013C9 RID: 5065
		public float relative;

		// Token: 0x040013CA RID: 5066
		public int absolute;

		// Token: 0x040013CB RID: 5067
		[NonSerialized]
		public UIRect rect;

		// Token: 0x040013CC RID: 5068
		[NonSerialized]
		public Camera targetCam;
	}

	// Token: 0x0200037B RID: 891
	public enum AnchorUpdate
	{
		// Token: 0x040013CE RID: 5070
		OnEnable,
		// Token: 0x040013CF RID: 5071
		OnUpdate,
		// Token: 0x040013D0 RID: 5072
		OnStart
	}
}
