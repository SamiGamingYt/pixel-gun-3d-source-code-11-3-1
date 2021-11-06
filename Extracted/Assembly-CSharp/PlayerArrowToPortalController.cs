using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x0200047A RID: 1146
internal sealed class PlayerArrowToPortalController : MonoBehaviour
{
	// Token: 0x060027E4 RID: 10212 RVA: 0x000C73BC File Offset: 0x000C55BC
	public PlayerArrowToPortalController()
	{
		this._arrowToPortalPoint = new Lazy<Transform>(() => this.ArrowToPortalPoint.transform);
		this._camera = new Lazy<Camera>(() => this.ArrowToPortalPoint.transform.parent.GetComponent<Camera>());
	}

	// Token: 0x060027E6 RID: 10214 RVA: 0x000C7448 File Offset: 0x000C5648
	private void Update()
	{
		if (this._poi != null && this._arrowToPortal != null)
		{
			float num = this.ArrowHeight + 0.4f;
			float z = num / Mathf.Tan(this._camera.Value.fieldOfView * 0.5f * 0.017453292f);
			Vector3 localPosition = this._arrowToPortalPoint.Value.localPosition;
			localPosition.y = this.ArrowHeight;
			localPosition.z = z;
			this._arrowToPortalPoint.Value.localPosition = localPosition;
			Vector3 position = this._poi.position;
			position.y = 0f;
			Vector3 position2 = this._arrowToPortal.position;
			position2.y = 0f;
			Vector3 forward = position - position2;
			this._arrowToPortal.rotation = Quaternion.LookRotation(forward);
			float num2 = Mathf.Clamp(this.ArrowAngle, 0f, 45f);
			this._arrowToPortal.RotateAround(this._arrowToPortal.position, this._arrowToPortalPoint.Value.parent.transform.right, this._arrowToPortalPoint.Value.parent.rotation.eulerAngles.x + num2 - 0.5f * this._camera.Value.fieldOfView);
			Vector3 position3 = this._camera.Value.gameObject.transform.position;
			position3.y = 0f;
			float num3 = Vector3.SqrMagnitude(position2 - position3);
			float num4 = Vector3.SqrMagnitude(position - position2);
			float num5 = Vector3.SqrMagnitude(position - position3);
			float num6 = Mathf.Max(4f, 0.25f * num3);
			bool flag = num4 < num6 || num5 < num6;
			foreach (Renderer renderer in this._renderers)
			{
				renderer.enabled = !flag;
			}
		}
	}

	// Token: 0x060027E7 RID: 10215 RVA: 0x000C7668 File Offset: 0x000C5868
	public void SetPointOfInterest(Transform poi)
	{
		this.SetPointOfInterest(poi, Color.green);
	}

	// Token: 0x060027E8 RID: 10216 RVA: 0x000C7678 File Offset: 0x000C5878
	public void SetPointOfInterest(Transform poi, Color color)
	{
		this._poi = poi;
		GameObject arrowFromPool = PlayerArrowToPortalController.GetArrowFromPool();
		if (arrowFromPool == null)
		{
			return;
		}
		this._arrowToPortal = arrowFromPool.transform;
		this._renderers = arrowFromPool.GetComponentsInChildren<Renderer>();
		this._arrowToPortal.parent = this._arrowToPortalPoint.Value;
		this._arrowToPortal.localPosition = Vector3.zero;
		if (this._renderers.Length > 0)
		{
			Renderer renderer = this._renderers[0];
			if (renderer == null)
			{
				return;
			}
			Texture texture;
			if (color == Color.red)
			{
				texture = this.redTexture;
			}
			else
			{
				texture = this.greenTexture;
			}
			if (texture != null && !object.ReferenceEquals(texture, renderer.material.mainTexture))
			{
				renderer.material.mainTexture = texture;
			}
		}
	}

	// Token: 0x060027E9 RID: 10217 RVA: 0x000C7754 File Offset: 0x000C5954
	public void RemovePointOfInterest()
	{
		if (this._arrowToPortal == null)
		{
			return;
		}
		this._poi = null;
		PlayerArrowToPortalController.DisposeArrowToPool(this._arrowToPortal.gameObject);
		this._arrowToPortal = null;
		this._renderers = new Renderer[0];
	}

	// Token: 0x060027EA RID: 10218 RVA: 0x000C77A0 File Offset: 0x000C59A0
	public static bool PopulateArrowPoolIfEmpty()
	{
		if (PlayerArrowToPortalController._arrowPool.Count > 0)
		{
			return true;
		}
		if (PlayerArrowToPortalController._arrowPrefab.Value == null)
		{
			return false;
		}
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(PlayerArrowToPortalController._arrowPrefab.Value);
		gameObject.SetActive(false);
		Transform transform = gameObject.transform;
		transform.parent = null;
		transform.localPosition = Vector3.zero;
		PlayerArrowToPortalController._arrowPool.Enqueue(gameObject);
		return true;
	}

	// Token: 0x060027EB RID: 10219 RVA: 0x000C7818 File Offset: 0x000C5A18
	public static GameObject GetArrowFromPool()
	{
		GameObject gameObject = null;
		while (PlayerArrowToPortalController._arrowPool.Count > 0 && gameObject == null)
		{
			gameObject = PlayerArrowToPortalController._arrowPool.Dequeue();
			if (gameObject == null)
			{
				Debug.LogWarning("Arrow pointer from pool is null.");
			}
		}
		if (gameObject == null)
		{
			if (!PlayerArrowToPortalController.PopulateArrowPoolIfEmpty())
			{
				return null;
			}
			gameObject = PlayerArrowToPortalController._arrowPool.Dequeue();
		}
		Transform transform = gameObject.transform;
		transform.parent = null;
		transform.localPosition = Vector3.zero;
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x060027EC RID: 10220 RVA: 0x000C78B0 File Offset: 0x000C5AB0
	public static void DisposeArrowToPool(GameObject arrow)
	{
		if (arrow == null)
		{
			return;
		}
		arrow.SetActive(false);
		PlayerArrowToPortalController._arrowPool.Enqueue(arrow);
	}

	// Token: 0x04001C2F RID: 7215
	public GameObject ArrowToPortalPoint;

	// Token: 0x04001C30 RID: 7216
	[Range(0f, 45f)]
	public float ArrowAngle = 30f;

	// Token: 0x04001C31 RID: 7217
	[Range(0f, 4f)]
	public float ArrowHeight = 1.5f;

	// Token: 0x04001C32 RID: 7218
	[SerializeField]
	private Texture greenTexture;

	// Token: 0x04001C33 RID: 7219
	[SerializeField]
	private Texture redTexture;

	// Token: 0x04001C34 RID: 7220
	private static readonly Queue<GameObject> _arrowPool = new Queue<GameObject>();

	// Token: 0x04001C35 RID: 7221
	private static readonly Lazy<UnityEngine.Object> _arrowPrefab = new Lazy<UnityEngine.Object>(() => Resources.Load("ArrowToPortal"));

	// Token: 0x04001C36 RID: 7222
	private Transform _arrowToPortal;

	// Token: 0x04001C37 RID: 7223
	private readonly Lazy<Transform> _arrowToPortalPoint;

	// Token: 0x04001C38 RID: 7224
	private readonly Lazy<Camera> _camera;

	// Token: 0x04001C39 RID: 7225
	private Transform _poi;

	// Token: 0x04001C3A RID: 7226
	private Renderer[] _renderers;
}
