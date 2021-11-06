using System;
using UnityEngine;

// Token: 0x020007DC RID: 2012
public class ButtonJoystickAdjust : MonoBehaviour
{
	// Token: 0x060048DF RID: 18655 RVA: 0x00194C84 File Offset: 0x00192E84
	private void Start()
	{
		if (this.ZoneBottonRight == null || this.ZoneTopLeft == null)
		{
			this._isZone = false;
		}
		if (this.defaultControlSprite != null && this.daterControlSprite != null)
		{
			this.defaultControlSprite.SetActive(!Defs.isDaterRegim);
			this.daterControlSprite.SetActive(Defs.isDaterRegim);
		}
	}

	// Token: 0x060048E0 RID: 18656 RVA: 0x00194D00 File Offset: 0x00192F00
	private void OnDrag(Vector2 delta)
	{
		delta /= Defs.Coef;
		this._isDrag = true;
		if (this._isZone)
		{
			Vector3 localPosition = this.ZoneTopLeft.transform.localPosition;
			Vector3 localPosition2 = this.ZoneBottonRight.transform.localPosition;
			if (this._nonClampedPosition != null)
			{
				this._nonClampedPosition = new Vector3?(new Vector3(this._nonClampedPosition.Value.x + delta.x, this._nonClampedPosition.Value.y + delta.y, 0f));
				Vector3 localPosition3 = new Vector3(Mathf.Clamp(this._nonClampedPosition.Value.x, localPosition.x, localPosition2.x), Mathf.Clamp(this._nonClampedPosition.Value.y, localPosition2.y, localPosition.y), this._nonClampedPosition.Value.z);
				base.transform.localPosition = localPosition3;
			}
		}
		else
		{
			Vector3 localPosition4 = new Vector3(base.transform.localPosition.x + delta.x, base.transform.localPosition.y + delta.y, 0f);
			base.transform.localPosition = localPosition4;
		}
	}

	// Token: 0x060048E1 RID: 18657 RVA: 0x00194E78 File Offset: 0x00193078
	private void OnPress(bool isDown)
	{
		this.IsPress = isDown;
		if (isDown)
		{
			this._nonClampedPosition = new Vector3?(new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, 0f));
			EventHandler<EventArgs> pressedDown = ButtonJoystickAdjust.PressedDown;
			if (pressedDown != null)
			{
				pressedDown(base.gameObject, EventArgs.Empty);
			}
		}
		else
		{
			this._nonClampedPosition = null;
		}
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x00194F00 File Offset: 0x00193100
	private void LateUpdate()
	{
		if (this.IsPress)
		{
			if (this._curTimeBlink < 0f)
			{
				if (this._isHidden)
				{
					TweenAlpha.Begin(base.gameObject, this.TimeBlink, 0.9f);
					this._isHidden = false;
				}
				else
				{
					TweenAlpha.Begin(base.gameObject, this.TimeBlink, 0.1f);
					this._isHidden = true;
				}
				this._curTimeBlink = this.TimeBlink;
			}
			else
			{
				this._curTimeBlink -= Time.deltaTime;
			}
			this._isDragLate = false;
		}
		else if (!this._isDragLate)
		{
			TweenAlpha.Begin(base.gameObject, 0.5f, 1f);
			this._isHidden = false;
			this._curTimeBlink = this.TimeBlink;
			this._isDragLate = true;
		}
	}

	// Token: 0x060048E3 RID: 18659 RVA: 0x00194FE0 File Offset: 0x001931E0
	public bool IsDrag()
	{
		bool isDrag = this._isDrag;
		this._isDrag = false;
		return isDrag;
	}

	// Token: 0x060048E4 RID: 18660 RVA: 0x00194FFC File Offset: 0x001931FC
	public Vector2 GetJoystickPosition()
	{
		Vector3 localPosition = base.transform.localPosition;
		return new Vector2(localPosition.x, localPosition.y);
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x00195028 File Offset: 0x00193228
	public void SetJoystickPosition(Vector2 position)
	{
		base.transform.localPosition = position;
	}

	// Token: 0x040035F9 RID: 13817
	[SerializeField]
	private GameObject ZoneTopLeft;

	// Token: 0x040035FA RID: 13818
	[SerializeField]
	private GameObject ZoneBottonRight;

	// Token: 0x040035FB RID: 13819
	private bool _isZone = true;

	// Token: 0x040035FC RID: 13820
	public float TimeBlink = 0.5f;

	// Token: 0x040035FD RID: 13821
	private float _curTimeBlink;

	// Token: 0x040035FE RID: 13822
	public bool IsPress;

	// Token: 0x040035FF RID: 13823
	public bool _isDrag;

	// Token: 0x04003600 RID: 13824
	private bool _isHidden;

	// Token: 0x04003601 RID: 13825
	private bool _isDragLate = true;

	// Token: 0x04003602 RID: 13826
	public static EventHandler<EventArgs> PressedDown;

	// Token: 0x04003603 RID: 13827
	public GameObject defaultControlSprite;

	// Token: 0x04003604 RID: 13828
	public GameObject daterControlSprite;

	// Token: 0x04003605 RID: 13829
	private Vector3? _nonClampedPosition;
}
