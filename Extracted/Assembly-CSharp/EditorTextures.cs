using System;
using System.Collections;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

// Token: 0x020000A5 RID: 165
internal sealed class EditorTextures : MonoBehaviour
{
	// Token: 0x060004D2 RID: 1234 RVA: 0x00027574 File Offset: 0x00025774
	private void Start()
	{
		if (this.prevHistoryButton != null)
		{
			this.prevHistoryButton.Clicked += this.HandlePrevHistoryButtonClicked;
			this.prevHistoryUIButton = this.prevHistoryButton.gameObject.GetComponent<UIButton>();
		}
		if (this.nextHistoryButton != null)
		{
			this.nextHistoryButton.Clicked += this.HandleNextHistoryButtonClicked;
			this.nextHistoryUIButton = this.nextHistoryButton.gameObject.GetComponent<UIButton>();
		}
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00027600 File Offset: 0x00025800
	public void ToggleSymmetry(bool isSymmetry)
	{
		this.symmetry = isSymmetry;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x0002760C File Offset: 0x0002580C
	public void SetStartCanvas(Texture2D _texure)
	{
		this.canvasTexture.mainTexture = EditorTextures.CreateCopyTexture(EditorTextures.CreateCopyTexture(_texure));
		float num = 400f / (float)this.canvasTexture.mainTexture.width;
		float num2 = 400f / (float)this.canvasTexture.mainTexture.height;
		int num3 = (num >= num2) ? Mathf.RoundToInt(num2) : Mathf.RoundToInt(num);
		this.canvasTexture.width = this.canvasTexture.mainTexture.width * num3;
		this.canvasTexture.height = this.canvasTexture.mainTexture.height * num3;
		this.UpdateFonCanvas();
		this.arrHistory.Clear();
		this.AddCanvasTextureInHistory();
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x000276CC File Offset: 0x000258CC
	private void HandlePrevHistoryButtonClicked(object sender, EventArgs e)
	{
		if (this.currentHistoryIndex > 0)
		{
			this.currentHistoryIndex--;
		}
		this.UpdateTextureFromHistory();
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x000276FC File Offset: 0x000258FC
	private void HandleNextHistoryButtonClicked(object sender, EventArgs e)
	{
		if (this.currentHistoryIndex < this.arrHistory.Count - 1)
		{
			this.currentHistoryIndex++;
		}
		this.UpdateTextureFromHistory();
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00027738 File Offset: 0x00025938
	private void UpdateTextureFromHistory()
	{
		this.canvasTexture.mainTexture = EditorTextures.CreateCopyTexture((Texture2D)this.arrHistory[this.currentHistoryIndex]);
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0002776C File Offset: 0x0002596C
	public void AddCanvasTextureInHistory()
	{
		while (this.currentHistoryIndex < this.arrHistory.Count - 1)
		{
			this.arrHistory.RemoveAt(this.arrHistory.Count - 1);
		}
		this.arrHistory.Add(EditorTextures.CreateCopyTexture((Texture2D)this.canvasTexture.mainTexture));
		if (this.arrHistory.Count > 30)
		{
			this.arrHistory.RemoveAt(0);
		}
		this.currentHistoryIndex = this.arrHistory.Count - 1;
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00027800 File Offset: 0x00025A00
	private void Update()
	{
		if (this.prevHistoryUIButton != null && this.nextHistoryUIButton != null)
		{
			if (this.prevHistoryUIButton.isEnabled != (this.currentHistoryIndex != 0))
			{
				this.prevHistoryUIButton.isEnabled = (this.currentHistoryIndex != 0);
			}
			if (this.nextHistoryUIButton.isEnabled != this.currentHistoryIndex < this.arrHistory.Count - 1)
			{
				this.nextHistoryUIButton.isEnabled = (this.currentHistoryIndex < this.arrHistory.Count - 1);
			}
		}
		int touchCount = Input.touchCount;
		Touch touch = (touchCount <= 0) ? default(Touch) : Input.GetTouch(0);
		if (this.isMouseDown && ((touchCount > 0 && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)) || Input.GetMouseButtonUp(0)))
		{
			Vector2 pos = (touchCount <= 0) ? new Vector2(Input.mousePosition.x, Input.mousePosition.y) : new Vector2(touch.position.x, touch.position.y);
			if (this.IsCanvasConteinPosition(pos))
			{
				this.OnCanvasClickUp();
			}
			this.isMouseDown = false;
			this.oldEditPixelPos = new Vector2(-1f, -1f);
			this.AddCanvasTextureInHistory();
		}
		if (this.isSetNewTexture)
		{
			this.isSetNewTexture = false;
			this.UpdateFonCanvas();
		}
		if ((touchCount > 0 && touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled) || this.isMouseDown || Input.GetMouseButtonDown(0))
		{
			Vector2 pos2 = (touchCount <= 0) ? new Vector2(Input.mousePosition.x, Input.mousePosition.y) : new Vector2(touch.position.x, touch.position.y);
			if (this.IsCanvasConteinPosition(pos2))
			{
				this.isMouseDown = true;
				Vector2 editPixelPos = this.GetEditPixelPos(pos2);
				if (!editPixelPos.Equals(this.oldEditPixelPos))
				{
					this.oldEditPixelPos = editPixelPos;
					this.EditCanvas(editPixelPos);
				}
			}
		}
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00027A70 File Offset: 0x00025C70
	private void EditCanvas(Vector2 pos)
	{
		if (this.saveFrame != null && this.saveFrame.activeSelf)
		{
			return;
		}
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Pipette)
		{
			if (SkinEditorController.sharedController != null)
			{
				SkinEditorController.sharedController.newColor.color = ((Texture2D)this.canvasTexture.mainTexture).GetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
				SkinEditorController.sharedController.HandleSetColorClicked(null, null);
			}
			return;
		}
		SkinEditorController.isEditingPartSkin = true;
		Texture2D texture2D = EditorTextures.CreateCopyTexture(this.canvasTexture.mainTexture as Texture2D);
		texture2D.wrapMode = TextureWrapMode.Clamp;
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Pencil)
		{
			texture2D.SetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
			if (this.symmetry)
			{
				texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
			}
		}
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Brash)
		{
			texture2D.SetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
			if (Mathf.RoundToInt(pos.x) > 0)
			{
				texture2D.SetPixel(Mathf.RoundToInt(pos.x) - 1, Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
			}
			if (Mathf.RoundToInt(pos.x) < texture2D.width - 1)
			{
				texture2D.SetPixel(Mathf.RoundToInt(pos.x) + 1, Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
			}
			if (Mathf.RoundToInt(pos.y) > 0)
			{
				texture2D.SetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y) - 1, SkinEditorController.colorForPaint);
			}
			if (Mathf.RoundToInt(pos.y) < texture2D.height - 1)
			{
				texture2D.SetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y) + 1, SkinEditorController.colorForPaint);
			}
			if (this.symmetry)
			{
				texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
				if (Mathf.RoundToInt(pos.x) > 0)
				{
					texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x) + 1, Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
				}
				if (Mathf.RoundToInt(pos.x) < texture2D.width - 1)
				{
					texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x) - 1, Mathf.RoundToInt(pos.y), SkinEditorController.colorForPaint);
				}
				if (Mathf.RoundToInt(pos.y) > 0)
				{
					texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y) - 1, SkinEditorController.colorForPaint);
				}
				if (Mathf.RoundToInt(pos.y) < texture2D.height - 1)
				{
					texture2D.SetPixel(texture2D.width - 1 - Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y) + 1, SkinEditorController.colorForPaint);
				}
			}
		}
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Eraser)
		{
			texture2D.SetPixel(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), this.colorForEraser);
		}
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Fill)
		{
			int num = Mathf.RoundToInt(pos.x) + Mathf.RoundToInt(pos.y) * texture2D.width;
			Color[] pixels = texture2D.GetPixels();
			Color color = pixels[num];
			if (color != SkinEditorController.colorForPaint)
			{
				int item = num;
				List<int> list = new List<int>();
				list.Add(item);
				while (list.Count > 0)
				{
					int num2 = Mathf.FloorToInt((float)list[0] / (float)texture2D.width);
					int num3 = list[0] - num2 * texture2D.width;
					pixels[list[0]] = SkinEditorController.colorForPaint;
					list.RemoveAt(0);
					if (num3 + 1 < texture2D.width && pixels[num3 + 1 + num2 * texture2D.width] == color && !list.Contains(num3 + 1 + num2 * texture2D.width))
					{
						list.Add(num3 + 1 + num2 * texture2D.width);
					}
					if (num3 - 1 >= 0 && pixels[num3 - 1 + num2 * texture2D.width] == color && !list.Contains(num3 - 1 + num2 * texture2D.width))
					{
						list.Add(num3 - 1 + num2 * texture2D.width);
					}
					if (num2 + 1 < texture2D.height && pixels[num3 + (num2 + 1) * texture2D.width] == color && !list.Contains(num3 + (num2 + 1) * texture2D.width))
					{
						list.Add(num3 + (num2 + 1) * texture2D.width);
					}
					if (num2 - 1 >= 0 && pixels[num3 + (num2 - 1) * texture2D.width] == color && !list.Contains(num3 + (num2 - 1) * texture2D.width))
					{
						list.Add(num3 + (num2 - 1) * texture2D.width);
					}
				}
				texture2D.SetPixels(pixels);
			}
		}
		texture2D.Apply();
		this.isSetNewTexture = true;
		this.canvasTexture.mainTexture = texture2D;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00028078 File Offset: 0x00026278
	private void OnCanvasClickUp()
	{
		if (this.saveFrame != null && this.saveFrame.activeSelf)
		{
			return;
		}
		if (SkinEditorController.brashMode == SkinEditorController.BrashMode.Pipette)
		{
			if (SkinEditorController.sharedController != null)
			{
				SkinEditorController.sharedController.SetColorClickedUp();
			}
			return;
		}
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x000280D0 File Offset: 0x000262D0
	private bool IsCanvasConteinPosition(Vector2 pos)
	{
		float num = (float)Screen.height / 768f;
		Vector2 vector = new Vector2(((float)Screen.width - num * (float)this.canvasTexture.width) * 0.5f, ((float)Screen.height + num * (float)this.canvasTexture.height) * 0.5f);
		Vector2 vector2 = new Vector2(((float)Screen.width + num * (float)this.canvasTexture.width) * 0.5f, ((float)Screen.height - num * (float)this.canvasTexture.height) * 0.5f);
		return pos.x > vector.x && pos.x < vector2.x && pos.y < vector.y && pos.y > vector2.y;
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x000281B8 File Offset: 0x000263B8
	private Vector2 GetEditPixelPos(Vector2 pos)
	{
		float num = (float)Screen.height / 768f;
		return new Vector2((float)Mathf.FloorToInt((pos.x - ((float)Screen.width - (float)this.canvasTexture.width * num) * 0.5f) / ((float)this.canvasTexture.width * num) * (float)this.canvasTexture.mainTexture.width), (float)Mathf.FloorToInt((pos.y - ((float)Screen.height - (float)this.canvasTexture.height * num) * 0.5f) / ((float)this.canvasTexture.height * num) * (float)this.canvasTexture.mainTexture.height));
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x0002826C File Offset: 0x0002646C
	public static Texture2D CreateCopyTexture(Texture tekTexture)
	{
		return EditorTextures.CreateCopyTexture((Texture2D)tekTexture);
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x0002827C File Offset: 0x0002647C
	public static Texture2D CreateCopyTexture(Texture2D tekTexture)
	{
		Texture2D texture2D = new Texture2D(tekTexture.width, tekTexture.height, TextureFormat.RGBA32, false);
		texture2D.SetPixels(tekTexture.GetPixels());
		texture2D.filterMode = FilterMode.Point;
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x000282B8 File Offset: 0x000264B8
	public void UpdateFonCanvas()
	{
		this.fonCanvas.width = this.canvasTexture.width;
		this.fonCanvas.height = this.canvasTexture.height;
		this.fonCanvas.mainTexture = this.canvasTexture.mainTexture;
	}

	// Token: 0x04000543 RID: 1347
	private Color colorForEraser = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04000544 RID: 1348
	public UITexture canvasTexture;

	// Token: 0x04000545 RID: 1349
	private bool isMouseDown;

	// Token: 0x04000546 RID: 1350
	private Vector2 oldEditPixelPos = new Vector2(-1f, -1f);

	// Token: 0x04000547 RID: 1351
	private bool isSetNewTexture;

	// Token: 0x04000548 RID: 1352
	public UITexture fonCanvas;

	// Token: 0x04000549 RID: 1353
	public ButtonHandler prevHistoryButton;

	// Token: 0x0400054A RID: 1354
	public ButtonHandler nextHistoryButton;

	// Token: 0x0400054B RID: 1355
	private UIButton prevHistoryUIButton;

	// Token: 0x0400054C RID: 1356
	private UIButton nextHistoryUIButton;

	// Token: 0x0400054D RID: 1357
	public ArrayList arrHistory = new ArrayList();

	// Token: 0x0400054E RID: 1358
	public int currentHistoryIndex;

	// Token: 0x0400054F RID: 1359
	private bool saveToHistory;

	// Token: 0x04000550 RID: 1360
	public GameObject saveFrame;

	// Token: 0x04000551 RID: 1361
	private bool symmetry;
}
