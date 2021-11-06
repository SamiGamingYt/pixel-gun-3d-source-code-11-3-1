using System;
using System.Collections;
using Rilisoft;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020005C1 RID: 1473
internal sealed class ClosingScript : MonoBehaviour
{
	// Token: 0x060032F1 RID: 13041 RVA: 0x00107C84 File Offset: 0x00105E84
	private IEnumerator Start()
	{
		Debug.LogWarning("Closing...");
		if (this.background != null)
		{
			yield return new WaitForRealSeconds(1f);
			RectTransform rectTransform = this.background.rectTransform;
			int startFrameIndex = Time.frameCount;
			float framerate = (float)((!Application.isEditor) ? ((Application.targetFrameRate <= 0) ? 60 : Application.targetFrameRate) : 300);
			float minYScale = 2f / (float)Screen.height;
			for (;;)
			{
				float alpha = (float)(Time.frameCount - startFrameIndex) / (0.5f * framerate);
				float yScale = Mathf.Lerp(1f, 0f, alpha);
				rectTransform.localScale = new Vector3(1f, Mathf.Max(minYScale, yScale), 1f);
				if (yScale < 0.01f)
				{
					break;
				}
				yield return null;
			}
			Color targetColor = new Color(this.background.color.r, this.background.color.g, this.background.color.b, 0.2f);
			for (;;)
			{
				float relativeAlpha = 4f * Time.deltaTime;
				float xScale = Mathf.Lerp(rectTransform.localScale.x, 0f, relativeAlpha);
				rectTransform.localScale = new Vector3(xScale, rectTransform.localScale.y, rectTransform.localScale.z);
				this.background.color = Color.Lerp(this.background.color, targetColor, relativeAlpha);
				if (xScale < 0.001f)
				{
					break;
				}
				yield return null;
			}
		}
		Debug.LogWarning("Quitting intentionally...");
		if (!Application.isEditor)
		{
			Application.Quit();
		}
		yield break;
	}

	// Token: 0x04002570 RID: 9584
	public RawImage background;
}
