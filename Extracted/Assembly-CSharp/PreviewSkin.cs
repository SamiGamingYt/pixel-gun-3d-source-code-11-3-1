using System;
using UnityEngine;

// Token: 0x02000499 RID: 1177
public class PreviewSkin : MonoBehaviour
{
	// Token: 0x06002A10 RID: 10768 RVA: 0x000DD870 File Offset: 0x000DBA70
	private void Start()
	{
		this.swipeZone = new Rect(this.sideMargin, this.topBottMargins, (float)Screen.width - this.sideMargin * 2f, (float)Screen.height - this.topBottMargins * 2f);
	}

	// Token: 0x06002A11 RID: 10769 RVA: 0x000DD8BC File Offset: 0x000DBABC
	private void Update()
	{
		if (this.isTapDown || Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began)
		{
			if (this.isTapDown && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
			{
				float num = (Input.touchCount <= 0) ? (this.touchPosition.x - Input.mousePosition.x) : (this.touchPosition.x - Input.GetTouch(0).position.x);
				if (this.selectedGameObject != null && Mathf.Abs(num) > 2f)
				{
					this.Unhighlight(this.selectedGameObject);
					this.selectedGameObject = null;
				}
				else
				{
					float num2 = 0.5f;
					base.transform.Rotate(0f, num2 * num, 0f, Space.Self);
					this.touchPosition = ((Input.touchCount <= 0) ? new Vector2(Input.mousePosition.x, Input.mousePosition.y) : Input.GetTouch(0).position);
				}
			}
			if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
			{
				if (this.selectedGameObject != null)
				{
					ButtonClickSound.Instance.PlayClick();
					this.Unhighlight(this.selectedGameObject);
					if (SkinEditorController.sharedController != null)
					{
						SkinEditorController.sharedController.SelectPart(this.selectedGameObject.name);
					}
					this.selectedGameObject = null;
				}
				this.isTapDown = false;
			}
			return;
		}
		this.touchPosition = ((Input.touchCount <= 0) ? new Vector2(Input.mousePosition.x, Input.mousePosition.y) : Input.GetTouch(0).position);
		if (!this.swipeZone.Contains(this.touchPosition))
		{
			return;
		}
		this.isTapDown = true;
		this.selectedGameObject = this.GameObjectOnTouch(this.touchPosition);
		if (this.selectedGameObject != null)
		{
			this.Highlight(this.selectedGameObject);
		}
	}

	// Token: 0x06002A12 RID: 10770 RVA: 0x000DDB2C File Offset: 0x000DBD2C
	public GameObject GameObjectOnTouch(Vector2 touchPosition)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.previewCamera.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0f)), out raycastHit))
		{
			return raycastHit.collider.gameObject;
		}
		return null;
	}

	// Token: 0x06002A13 RID: 10771 RVA: 0x000DDB78 File Offset: 0x000DBD78
	public void Highlight(GameObject go)
	{
		Renderer component = go.GetComponent<Renderer>();
		if (component == null)
		{
			return;
		}
		Color color = component.materials[0].color;
		component.materials[0].color = new Color(color.r, color.g, color.b, 0.6f);
	}

	// Token: 0x06002A14 RID: 10772 RVA: 0x000DDBD4 File Offset: 0x000DBDD4
	public void Unhighlight(GameObject go)
	{
		Renderer component = go.GetComponent<Renderer>();
		if (component == null)
		{
			return;
		}
		Color color = component.materials[0].color;
		component.materials[0].color = new Color(color.r, color.g, color.b, 1f);
	}

	// Token: 0x06002A15 RID: 10773 RVA: 0x000DDC30 File Offset: 0x000DBE30
	private void OnEnable()
	{
		this.isTapDown = false;
		this.selectedGameObject = null;
	}

	// Token: 0x06002A16 RID: 10774 RVA: 0x000DDC40 File Offset: 0x000DBE40
	private void OnDisable()
	{
		this.isTapDown = false;
		this.selectedGameObject = null;
	}

	// Token: 0x04001F0B RID: 7947
	public Camera previewCamera;

	// Token: 0x04001F0C RID: 7948
	private Vector2 touchPosition;

	// Token: 0x04001F0D RID: 7949
	private bool isTapDown;

	// Token: 0x04001F0E RID: 7950
	private GameObject selectedGameObject;

	// Token: 0x04001F0F RID: 7951
	private float sideMargin = 100f;

	// Token: 0x04001F10 RID: 7952
	private float topBottMargins = 120f;

	// Token: 0x04001F11 RID: 7953
	private Rect swipeZone;

	// Token: 0x04001F12 RID: 7954
	private Vector3 rememberedScale;

	// Token: 0x04001F13 RID: 7955
	private Vector3 rememberedBodyOffs;
}
