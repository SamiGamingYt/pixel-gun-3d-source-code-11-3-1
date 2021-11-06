using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class Blinking : MonoBehaviour
{
	// Token: 0x06000178 RID: 376 RVA: 0x0000EED0 File Offset: 0x0000D0D0
	private void Start()
	{
		this.mySprite = base.GetComponent<UISprite>();
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000EEE0 File Offset: 0x0000D0E0
	private void Update()
	{
		this._time += Time.deltaTime;
		if (this.mySprite != null)
		{
			Color color = this.mySprite.color;
			float num = 2f * (this._time - Mathf.Floor(this._time / this.halfCycle) * this.halfCycle) / this.halfCycle;
			if (num > 1f)
			{
				num = 2f - num;
			}
			this.mySprite.color = new Color(color.r, color.g, color.b, num);
		}
	}

	// Token: 0x04000175 RID: 373
	public float halfCycle = 1f;

	// Token: 0x04000176 RID: 374
	private UISprite mySprite;

	// Token: 0x04000177 RID: 375
	private float _time;
}
