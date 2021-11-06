using System;
using UnityEngine;

// Token: 0x020007E6 RID: 2022
public class ChatScroll : MonoBehaviour
{
	// Token: 0x06004933 RID: 18739 RVA: 0x00196F98 File Offset: 0x00195198
	private void Start()
	{
		base.transform.position = new Vector3(posNGUI.getPosX((float)Screen.width * 0.1f), posNGUI.getPosY((float)Screen.height * 0.03f), 1f);
	}

	// Token: 0x06004934 RID: 18740 RVA: 0x00196FDC File Offset: 0x001951DC
	private void Update()
	{
		if (!Application.isEditor)
		{
			int touchCount = Input.touchCount;
			for (int i = 0; i < touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began)
				{
					this.isTouch = true;
					this._Textlist.OnSelect(true);
				}
				if (touch.phase == TouchPhase.Ended)
				{
					this.isTouch = false;
					this._Textlist.OnSelect(false);
				}
			}
		}
		else
		{
			if (Input.GetMouseButtonDown(0))
			{
				this.isTouch = true;
				this._Textlist.OnSelect(true);
			}
			if (Input.GetMouseButtonUp(0))
			{
				this.isTouch = false;
				this._Textlist.OnSelect(false);
			}
		}
	}

	// Token: 0x06004935 RID: 18741 RVA: 0x00197094 File Offset: 0x00195294
	private void LateUpdate()
	{
		if (this.isTouch)
		{
			this._Textlist.OnScroll((float)Math.Round((double)(-(double)Input.GetAxis("Mouse Y") / 5f), 1));
		}
	}

	// Token: 0x04003662 RID: 13922
	public UITextListEdit _Textlist;

	// Token: 0x04003663 RID: 13923
	private bool isTouch;
}
