using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200069E RID: 1694
	internal sealed class LoadingProgress
	{
		// Token: 0x06003B56 RID: 15190 RVA: 0x001343AC File Offset: 0x001325AC
		private LoadingProgress()
		{
			this._backgroundTexture = Resources.Load<Texture2D>("line_shadow");
			this._progressbarTexture = Resources.Load<Texture2D>("line");
			this._labelStyle = new GUIStyle(GUI.skin.label)
			{
				alignment = TextAnchor.MiddleCenter,
				font = Resources.Load<Font>("04B_03"),
				fontSize = Convert.ToInt32(22f * Defs.Coef),
				normal = new GUIStyleState
				{
					textColor = Color.black
				}
			};
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06003B57 RID: 15191 RVA: 0x0013443C File Offset: 0x0013263C
		public static LoadingProgress Instance
		{
			get
			{
				if (LoadingProgress._instance == null)
				{
					LoadingProgress._instance = new LoadingProgress();
				}
				return LoadingProgress._instance;
			}
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x00134458 File Offset: 0x00132658
		public static void Unload()
		{
			if (LoadingProgress._instance != null)
			{
				Resources.UnloadAsset(LoadingProgress._instance._backgroundTexture);
				Resources.UnloadAsset(LoadingProgress._instance._progressbarTexture);
				LoadingProgress._instance = null;
			}
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x00134494 File Offset: 0x00132694
		public void Draw(float progress)
		{
			float num = Mathf.Clamp01(progress);
			if (this._backgroundTexture != null)
			{
				float num2 = 1.8f * (float)this._backgroundTexture.width * Defs.Coef;
				float num3 = 1.8f * (float)this._backgroundTexture.height * Defs.Coef;
				Rect rect = new Rect(0.5f * ((float)Screen.width - num2), (float)Screen.height - (21f * Defs.Coef + num3), num2, num3);
				float num4 = num2 - 7.2f * Defs.Coef;
				float width = num4 * num;
				float height = num3 - 7.2f * Defs.Coef;
				if (this._progressbarTexture != null)
				{
					Rect position = new Rect(rect.xMin + 3.6f * Defs.Coef, rect.yMin + 3.6f * Defs.Coef, width, height);
					GUI.DrawTexture(position, this._progressbarTexture);
				}
				GUI.DrawTexture(rect, this._backgroundTexture);
				Rect position2 = rect;
				position2.yMin -= 1.8f * Defs.Coef;
				position2.y += 1.8f * Defs.Coef;
				int num5 = Mathf.RoundToInt(num * 100f);
				string text = string.Format("{0}%", num5);
				GUI.Label(position2, text, this._labelStyle);
			}
		}

		// Token: 0x04002BF0 RID: 11248
		private readonly GUIStyle _labelStyle;

		// Token: 0x04002BF1 RID: 11249
		private readonly Texture2D _backgroundTexture;

		// Token: 0x04002BF2 RID: 11250
		private readonly Texture2D _progressbarTexture;

		// Token: 0x04002BF3 RID: 11251
		private static LoadingProgress _instance;
	}
}
