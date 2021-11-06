using System;
using UnityEngine;

// Token: 0x0200085E RID: 2142
[DisallowMultipleComponent]
public sealed class TimeLabel : MonoBehaviour
{
	// Token: 0x06004D78 RID: 19832 RVA: 0x001C0254 File Offset: 0x001BE454
	private void Start()
	{
		base.gameObject.SetActive(Defs.isMulti);
		this._label = base.GetComponent<UILabel>();
	}

	// Token: 0x06004D79 RID: 19833 RVA: 0x001C0274 File Offset: 0x001BE474
	private void Update()
	{
		if (InGameGUI.sharedInGameGUI && this._label)
		{
			this._label.text = InGameGUI.sharedInGameGUI.timeLeft();
			if (!Defs.isHunger)
			{
				float num = (!Defs.isDuel) ? ((float)TimeGameController.sharedController.timerToEndMatch) : DuelController.instance.timeLeft;
				if (num <= this.startTime && (!Defs.isDuel || DuelController.instance.gameStatus == DuelController.GameStatus.Playing))
				{
					float num2 = Mathf.Round(num) - num;
					this.blink = (num2 > 0f);
					this._label.transform.localScale = Vector3.MoveTowards(this._label.transform.localScale, (!this.blink) ? Vector3.one : (Vector3.one * Mathf.Min(1.4f + (this.startTime - num) / 20f, 2f)), (!this.blink) ? (2.4f * Time.deltaTime) : (12f * Time.deltaTime));
					this._label.color = ((!this.blink) ? Color.white : Color.red);
					this._label.GetComponentInChildren<TweenRotation>().enabled = true;
					this._label.GetComponentInChildren<TweenRotation>().PlayForward();
					if (Defs.isSoundFX)
					{
						this.timerSound.enabled = true;
					}
					this.timerSound.loop = true;
					if (PauseGUIController.Instance != null && PauseGUIController.Instance.IsPaused)
					{
						this.timerParticles.gameObject.SetActive(false);
					}
					else
					{
						this.timerParticles.gameObject.SetActive(true);
					}
					ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = this.timerParticles.textureSheetAnimation;
					ParticleSystemCurveMode mode = textureSheetAnimation.frameOverTime.mode;
					textureSheetAnimation.frameOverTime = new ParticleSystem.MinMaxCurve((num - 1f) / 9f);
					if (num < 1f)
					{
						this.timerParticles.gameObject.SetActive(false);
					}
				}
				else
				{
					this.timerParticles.gameObject.SetActive(false);
					this.timerSound.enabled = false;
					this._label.color = Color.white;
					this._label.transform.localScale = Vector3.one;
					this._label.GetComponentInChildren<TweenRotation>().ResetToBeginning();
					this._label.GetComponentInChildren<TweenRotation>().enabled = false;
				}
			}
		}
	}

	// Token: 0x04003BED RID: 15341
	private UILabel _label;

	// Token: 0x04003BEE RID: 15342
	public UISprite timerBackground;

	// Token: 0x04003BEF RID: 15343
	public AudioSource timerSound;

	// Token: 0x04003BF0 RID: 15344
	public ParticleSystem timerParticles;

	// Token: 0x04003BF1 RID: 15345
	private Vector3 targetScale = Vector3.one;

	// Token: 0x04003BF2 RID: 15346
	private bool blink;

	// Token: 0x04003BF3 RID: 15347
	private float startTime = 11f;
}
