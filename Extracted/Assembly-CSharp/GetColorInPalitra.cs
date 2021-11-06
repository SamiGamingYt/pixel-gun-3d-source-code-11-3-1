using System;
using UnityEngine;

// Token: 0x0200015A RID: 346
internal sealed class GetColorInPalitra : MonoBehaviour
{
	// Token: 0x06000B6B RID: 2923 RVA: 0x000405C8 File Offset: 0x0003E7C8
	private void Update()
	{
		bool flag = false;
		Vector2 zero = Vector2.zero;
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			flag = (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled);
			zero = new Vector2(touch.position.x, touch.position.y);
		}
		if (flag && this.IsCanvasConteinPosition(zero))
		{
			Vector2 editPixelPos = this.GetEditPixelPos(zero);
			Color pixel = ((Texture2D)this.canvasTexture.mainTexture).GetPixel(Mathf.RoundToInt(editPixelPos.x), Mathf.RoundToInt(editPixelPos.y));
			this.newColor.color = pixel;
			this.okColorInPalitraButton.defaultColor = pixel;
			this.okColorInPalitraButton.pressed = pixel;
			this.okColorInPalitraButton.hover = pixel;
		}
	}

	// Token: 0x06000B6C RID: 2924 RVA: 0x000406B4 File Offset: 0x0003E8B4
	private bool IsCanvasConteinPosition(Vector2 pos)
	{
		float num = (float)Screen.height / 768f;
		Vector2 vector = new Vector2(((float)Screen.width - (float)this.canvasTexture.width * num) * 0.5f + this.canvasTexture.transform.localPosition.x * num, ((float)Screen.height - (float)this.canvasTexture.height * num) * 0.5f + this.canvasTexture.transform.localPosition.y * num);
		Rect rect = new Rect(vector.x, vector.y, (float)this.canvasTexture.width * num, (float)this.canvasTexture.height * num);
		return rect.Contains(pos);
	}

	// Token: 0x06000B6D RID: 2925 RVA: 0x0004077C File Offset: 0x0003E97C
	private Vector2 GetEditPixelPos(Vector2 pos)
	{
		float num = (float)Screen.height / 768f;
		Vector2 vector = pos - new Vector2(((float)Screen.width - (float)this.canvasTexture.width * num) * 0.5f + this.canvasTexture.transform.localPosition.x * num, ((float)Screen.height - (float)this.canvasTexture.height * num) * 0.5f + this.canvasTexture.transform.localPosition.y * num);
		return new Vector2((float)Mathf.FloorToInt(vector.x / ((float)this.canvasTexture.width * num) * (float)this.canvasTexture.mainTexture.width), (float)Mathf.FloorToInt(vector.y / ((float)this.canvasTexture.height * num) * (float)this.canvasTexture.mainTexture.height));
	}

	// Token: 0x04000911 RID: 2321
	public UITexture canvasTexture;

	// Token: 0x04000912 RID: 2322
	public UISprite newColor;

	// Token: 0x04000913 RID: 2323
	public UIButton okColorInPalitraButton;
}
