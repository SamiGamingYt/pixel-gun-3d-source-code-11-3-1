using System;
using Rilisoft;
using UnityEngine;

// Token: 0x02000304 RID: 772
public sealed class Message : MonoBehaviour
{
	// Token: 0x06001B3B RID: 6971 RVA: 0x0006FF68 File Offset: 0x0006E168
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this._startTime = Time.realtimeSinceStartup;
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x0006FF80 File Offset: 0x0006E180
	private void Remove()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x0006FF90 File Offset: 0x0006E190
	private void OnGUI()
	{
		if (Time.realtimeSinceStartup - this._startTime >= (TrainingController.TrainingCompleted ? this.OnScreenTime : (this.OnScreenTime / 2f)))
		{
			this.Remove();
			return;
		}
		this.rect = Tools.SuccessMessageRect();
		GUI.depth = this.depth;
		this.labelStyle.fontSize = Player_move_c.FontSizeForMessages;
		GUI.Label(this.rect, this.message, this.labelStyle);
	}

	// Token: 0x0400106D RID: 4205
	public GUIStyle labelStyle;

	// Token: 0x0400106E RID: 4206
	public Rect rect = Tools.SuccessMessageRect();

	// Token: 0x0400106F RID: 4207
	public string message = "Purchases restored";

	// Token: 0x04001070 RID: 4208
	public int depth = -2;

	// Token: 0x04001071 RID: 4209
	private float _startTime;

	// Token: 0x04001072 RID: 4210
	public float OnScreenTime = 3f;
}
