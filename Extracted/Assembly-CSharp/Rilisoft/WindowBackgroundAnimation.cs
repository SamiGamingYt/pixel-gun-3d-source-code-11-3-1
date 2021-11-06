using System;
using System.Collections;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000790 RID: 1936
	public class WindowBackgroundAnimation : MonoBehaviour
	{
		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06004552 RID: 17746 RVA: 0x00175FF4 File Offset: 0x001741F4
		private UIRoot interfaceHolder
		{
			get
			{
				if (this.interfaceHolderValue == null)
				{
					this.interfaceHolderValue = base.gameObject.GetComponentInParents<UIRoot>();
				}
				return this.interfaceHolderValue;
			}
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x0017602C File Offset: 0x0017422C
		private void OnEnable()
		{
			if (this.PlayOnEnable)
			{
				this.Play();
			}
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x00176040 File Offset: 0x00174240
		public void Play()
		{
			this._currentBgArrowPrefabIndex = -1;
			base.StartCoroutine(this.LoopBackgroundAnimation());
		}

		// Token: 0x06004555 RID: 17749 RVA: 0x00176058 File Offset: 0x00174258
		private IEnumerator LoopBackgroundAnimation()
		{
			GameObject arrowRowPrefab = this.Arrows[0];
			if (this._bgArrowRows == null)
			{
				this._bgArrowRows = new GameObject[8];
				for (int i = 0; i < this._bgArrowRows.Length; i++)
				{
					GameObject newArrowRow = UnityEngine.Object.Instantiate<GameObject>(arrowRowPrefab);
					newArrowRow.transform.parent = arrowRowPrefab.transform.parent;
					this._bgArrowRows[i] = newArrowRow;
				}
			}
			for (int j = 0; j < this.Arrows.Length; j++)
			{
				this.Arrows[j].SetActive(false);
			}
			this._currentBgArrowPrefabIndex = -1;
			for (;;)
			{
				if (this.interfaceHolder != null && this.interfaceHolder.gameObject.activeInHierarchy)
				{
					for (int k = 0; k < this.ShineNodes.Length; k++)
					{
						GameObject shine = this.ShineNodes[k];
						if (shine != null && shine.activeInHierarchy)
						{
							shine.transform.Rotate(Vector3.forward, Time.deltaTime * 10f, Space.Self);
							if (k != this._currentBgArrowPrefabIndex)
							{
								this._currentBgArrowPrefabIndex = k;
								this.ResetBackgroundArrows(this.Arrows[k].transform);
							}
						}
					}
					for (int l = 0; l < this._bgArrowRows.Length; l++)
					{
						if (!(this._bgArrowRows[l] == null))
						{
							Transform t = this._bgArrowRows[l].transform;
							float newLocalY = t.localPosition.y + Time.deltaTime * 60f;
							if (newLocalY > 474f)
							{
								newLocalY -= 880f;
							}
							t.localPosition = new Vector3(t.localPosition.x, newLocalY, t.localPosition.z);
						}
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x00176074 File Offset: 0x00174274
		private void ResetBackgroundArrows(Transform target)
		{
			for (int i = 0; i < this._bgArrowRows.Length; i++)
			{
				Transform transform = this._bgArrowRows[i].transform;
				transform.parent = target.parent;
				transform.localScale = Vector3.one;
				transform.localPosition = new Vector3(target.localPosition.x + ((i % 2 != 1) ? 0f : 90f), target.localPosition.y - 110f * (float)i, target.localPosition.z);
				transform.localRotation = target.localRotation;
			}
		}

		// Token: 0x040032D6 RID: 13014
		public GameObject[] Arrows;

		// Token: 0x040032D7 RID: 13015
		public GameObject[] ShineNodes;

		// Token: 0x040032D8 RID: 13016
		public bool PlayOnEnable = true;

		// Token: 0x040032D9 RID: 13017
		private int _currentBgArrowPrefabIndex = -1;

		// Token: 0x040032DA RID: 13018
		private GameObject[] _bgArrowRows;

		// Token: 0x040032DB RID: 13019
		private UIRoot interfaceHolderValue;
	}
}
