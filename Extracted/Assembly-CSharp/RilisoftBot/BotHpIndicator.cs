using System;
using System.Collections;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

namespace RilisoftBot
{
	// Token: 0x0200058B RID: 1419
	public class BotHpIndicator : MonoBehaviour
	{
		// Token: 0x06003172 RID: 12658 RVA: 0x00101C78 File Offset: 0x000FFE78
		private IEnumerator Start()
		{
			this._frame.SetActive(false);
			yield return new WaitForSeconds(0.2f);
			yield return this.WaitHpOwner();
			yield return this.UpdateIndicator();
			yield break;
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x00101C94 File Offset: 0x000FFE94
		private IEnumerator UpdateIndicator()
		{
			for (;;)
			{
				if (this._hp == null || this._healthBar == null || Math.Abs(this._hp.BaseHealth) < 0.0001f)
				{
					yield return null;
				}
				if (this._hp.Health <= 0f && this._frame.activeInHierarchy)
				{
					this._frame.SetActive(false);
					yield return null;
				}
				this._currentScale = this._hp.Health / this._hp.BaseHealth;
				if (Math.Abs(this._currentScale - this._prevScale) > 0.0001f)
				{
					this._frame.SetActive(true);
					this._currShowTime = 2f;
					this._healthBar.localScale = new Vector3(this._currentScale, this._healthBar.localScale.y, this._healthBar.localScale.z);
				}
				this._prevScale = this._currentScale;
				if (this._currShowTime > 0f)
				{
					this._currShowTime -= Time.deltaTime;
				}
				else
				{
					this._currShowTime = 0f;
					this._frame.SetActive(false);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x00101CB0 File Offset: 0x000FFEB0
		private IEnumerator WaitHpOwner()
		{
			bool setted = false;
			while (!setted)
			{
				foreach (GameObject pr in base.gameObject.AncestorsAndSelf())
				{
					BaseBot bot = pr.GetComponent<BaseBot>();
					if (bot != null)
					{
						this._hp = new BotHpIndicator.HealthProvider(() => bot.health, () => bot.baseHealth);
						setted = true;
						break;
					}
				}
				foreach (GameObject pr2 in base.gameObject.AncestorsAndSelf())
				{
					TrainingEnemy dummy = pr2.GetComponent<TrainingEnemy>();
					if (dummy != null)
					{
						this._hp = new BotHpIndicator.HealthProvider(() => (float)dummy.hitPoints, () => (float)dummy.baseHitPoints);
						setted = true;
						break;
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x04002470 RID: 9328
		[SerializeField]
		private GameObject _frame;

		// Token: 0x04002471 RID: 9329
		private float _currShowTime;

		// Token: 0x04002472 RID: 9330
		[SerializeField]
		private Transform _healthBar;

		// Token: 0x04002473 RID: 9331
		[SerializeField]
		[ReadOnly]
		private float _currentScale;

		// Token: 0x04002474 RID: 9332
		private float _prevScale = 1f;

		// Token: 0x04002475 RID: 9333
		private BotHpIndicator.HealthProvider _hp;

		// Token: 0x0200058C RID: 1420
		internal class HealthProvider
		{
			// Token: 0x06003175 RID: 12661 RVA: 0x00101CCC File Offset: 0x000FFECC
			public HealthProvider(Func<float> healthGetter, Func<float> baseHealthGetter)
			{
				Func<float> healthGetter2 = healthGetter;
				if (healthGetter == null)
				{
					healthGetter2 = (() => 0f);
				}
				this._healthGetter = healthGetter2;
				Func<float> baseHealthGetter2 = baseHealthGetter;
				if (baseHealthGetter == null)
				{
					baseHealthGetter2 = (() => 0f);
				}
				this._baseHealthGetter = baseHealthGetter2;
			}

			// Token: 0x17000861 RID: 2145
			// (get) Token: 0x06003176 RID: 12662 RVA: 0x00101D38 File Offset: 0x000FFF38
			public float Health
			{
				get
				{
					return this._healthGetter();
				}
			}

			// Token: 0x17000862 RID: 2146
			// (get) Token: 0x06003177 RID: 12663 RVA: 0x00101D48 File Offset: 0x000FFF48
			public float BaseHealth
			{
				get
				{
					return this._baseHealthGetter();
				}
			}

			// Token: 0x04002476 RID: 9334
			private readonly Func<float> _healthGetter;

			// Token: 0x04002477 RID: 9335
			private readonly Func<float> _baseHealthGetter;
		}
	}
}
