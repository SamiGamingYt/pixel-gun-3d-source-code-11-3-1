using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class DuelPlayerInfo : MonoBehaviour
{
	// Token: 0x060004B5 RID: 1205 RVA: 0x0002693C File Offset: 0x00024B3C
	public void FillByTable(NetworkStartTable table)
	{
		this.level.text = table.myRanks.ToString();
		foreach (UILabel uilabel in this.playerName)
		{
			uilabel.text = table.NamePlayer;
		}
		this.clanName.text = table.myClanName;
		this.clanTexture.mainTexture = table.myClanTexture;
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x000269AC File Offset: 0x00024BAC
	public void SetPointInWorld(Transform pointTransform, Transform character)
	{
		this.character = character;
		this.pointInWorld = pointTransform;
		if (this.defaultRotation == Quaternion.identity)
		{
			this.defaultRotation = character.rotation;
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x000269E0 File Offset: 0x00024BE0
	private void Awake()
	{
		this.touchZone = new Rect(-0.2f * (float)Screen.width, -0.65f * (float)Screen.height, 0.45f * (float)Screen.width, 0.65f * (float)Screen.height);
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x00026A28 File Offset: 0x00024C28
	private void Update()
	{
		if (this.pointInWorld == null || NickLabelController.currentCamera == null)
		{
			return;
		}
		Vector3 v = NickLabelController.currentCamera.WorldToScreenPoint(this.pointInWorld.position + Vector3.up);
		base.transform.localPosition = new Vector3(v.x / (float)Screen.height * 768f, v.y / (float)Screen.height * 768f, 0f);
		this.RotateCharacters(v);
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00026AC0 File Offset: 0x00024CC0
	private void RotateCharacters(Vector2 relativePoint)
	{
		Vector2 vector = Vector2.zero;
		Vector2 a = Vector2.zero;
		bool flag = false;
		if (Application.isEditor)
		{
			Vector2 vector2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			if ((this.tapFingerID == -1 && this.touchZone.Contains(vector2 - relativePoint)) || this.tapFingerID == 1)
			{
				if (Input.GetMouseButtonDown(0))
				{
					this.tapFingerID = 1;
				}
				else if (Input.GetMouseButtonUp(0))
				{
					this.tapFingerID = -1;
					this.lastPosition = Vector2.zero;
				}
				else if (Input.GetMouseButton(0))
				{
					flag = true;
					a = vector2;
				}
			}
		}
		else
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if ((this.tapFingerID == -1 && !DuelPlayerInfo.fingerInUse.Equals(touch.fingerId) && this.touchZone.Contains(touch.position - relativePoint)) || touch.fingerId == this.tapFingerID)
				{
					if (touch.phase == TouchPhase.Began)
					{
						this.tapFingerID = touch.fingerId;
					}
					else if (touch.phase == TouchPhase.Ended)
					{
						this.tapFingerID = -1;
						this.lastPosition = Vector2.zero;
					}
					else
					{
						flag = true;
						a = touch.position;
					}
				}
			}
		}
		if (flag)
		{
			DuelPlayerInfo.fingerInUse = this.tapFingerID;
			this.lastTapTime = Time.time;
			if (this.lastPosition == Vector2.zero)
			{
				this.lastPosition = a;
			}
			vector = a - this.lastPosition;
			this.lastPosition = a;
			this.character.Rotate(Vector3.up, vector.x * Time.deltaTime * RilisoftRotator.RotationRateForCharacterInMenues);
		}
		else
		{
			DuelPlayerInfo.fingerInUse = -1;
			if (Time.time - this.lastTapTime > ShopNGUIController.IdleTimeoutPers)
			{
				this.character.rotation = Quaternion.RotateTowards(this.character.rotation, this.defaultRotation, 300f * Time.deltaTime);
			}
		}
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00026D00 File Offset: 0x00024F00
	private void OnDisable()
	{
		this.tapFingerID = -1;
		DuelPlayerInfo.fingerInUse = -1;
	}

	// Token: 0x04000512 RID: 1298
	public UILabel level;

	// Token: 0x04000513 RID: 1299
	public UILabel[] playerName;

	// Token: 0x04000514 RID: 1300
	public UILabel clanName;

	// Token: 0x04000515 RID: 1301
	public UITexture clanTexture;

	// Token: 0x04000516 RID: 1302
	private Transform pointInWorld;

	// Token: 0x04000517 RID: 1303
	private Transform character;

	// Token: 0x04000518 RID: 1304
	public Rect touchZone;

	// Token: 0x04000519 RID: 1305
	private Quaternion defaultRotation = Quaternion.identity;

	// Token: 0x0400051A RID: 1306
	private Vector2 lastPosition = Vector2.zero;

	// Token: 0x0400051B RID: 1307
	private float lastTapTime;

	// Token: 0x0400051C RID: 1308
	private int tapFingerID = -1;

	// Token: 0x0400051D RID: 1309
	private static int fingerInUse = -1;
}
