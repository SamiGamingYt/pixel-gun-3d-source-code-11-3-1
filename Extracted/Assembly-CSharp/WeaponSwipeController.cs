using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200088B RID: 2187
public sealed class WeaponSwipeController : MonoBehaviour
{
	// Token: 0x06004EB3 RID: 20147 RVA: 0x001C83F4 File Offset: 0x001C65F4
	private void Start()
	{
		this._wrapContent = base.GetComponentInChildren<UIWrapContent>();
		this._center = base.GetComponentInChildren<MyCenterOnChild>();
		this._scrollView = base.GetComponent<UIScrollView>();
		MyCenterOnChild center = this._center;
		center.onFinished = (SpringPanel.OnFinished)Delegate.Combine(center.onFinished, new SpringPanel.OnFinished(this.HandleCenteringFinished));
		this.UpdateContent();
	}

	// Token: 0x06004EB4 RID: 20148 RVA: 0x001C8454 File Offset: 0x001C6654
	private void HandleWeaponEquipped()
	{
		this.UpdateContent();
	}

	// Token: 0x06004EB5 RID: 20149 RVA: 0x001C845C File Offset: 0x001C665C
	private void OnEnable()
	{
		base.StartCoroutine(this._DisableSwiping(0.5f));
	}

	// Token: 0x06004EB6 RID: 20150 RVA: 0x001C8470 File Offset: 0x001C6670
	private IEnumerator _DisableSwiping(float tm)
	{
		if (this._center == null)
		{
			yield break;
		}
		int bef;
		if (!int.TryParse(this._center.centeredObject.name.Replace("preview_", string.Empty), out bef))
		{
			yield break;
		}
		this._disabled = true;
		yield return new WaitForSeconds(tm);
		this._disabled = false;
		if (this._center.centeredObject.name.Equals("preview_" + bef))
		{
			yield break;
		}
		Transform goToCent = null;
		foreach (object obj in this._center.transform)
		{
			Transform t = (Transform)obj;
			if (t.gameObject.name.Equals("preview_" + bef))
			{
				goToCent = t;
				break;
			}
		}
		if (goToCent != null)
		{
			this._center.CenterOn(goToCent);
		}
		yield break;
	}

	// Token: 0x06004EB7 RID: 20151 RVA: 0x001C849C File Offset: 0x001C669C
	private void HandleCenteringFinished()
	{
		if (this._disabled)
		{
			return;
		}
		int num;
		if (!int.TryParse(this._center.centeredObject.name.Replace("preview_", string.Empty), out num))
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("HandleCenteringFinished: error parse");
			}
			return;
		}
		num--;
		if (!this.move)
		{
			if (!Defs.isMulti)
			{
				this.move = GameObject.FindGameObjectWithTag("Player").GetComponent<SkinName>().playerMoveC;
			}
			else
			{
				this.move = WeaponManager.sharedManager.myPlayerMoveC;
			}
		}
		if (num != WeaponManager.sharedManager.CurrentWeaponIndex)
		{
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				TrainingState trainingState;
				bool flag = TrainingController.stepTrainingList.TryGetValue("SwipeWeapon", out trainingState) && TrainingController.stepTraining == trainingState;
				if (flag)
				{
					TrainingController.isNextStep = trainingState;
				}
			}
			WeaponManager.sharedManager.CurrentWeaponIndex = num % WeaponManager.sharedManager.playerWeapons.Count;
			WeaponManager.sharedManager.SaveWeaponAsLastUsed(WeaponManager.sharedManager.CurrentWeaponIndex);
			if (this.move != null)
			{
				this.move.ChangeWeapon(WeaponManager.sharedManager.CurrentWeaponIndex, false);
			}
		}
	}

	// Token: 0x06004EB8 RID: 20152 RVA: 0x001C85EC File Offset: 0x001C67EC
	private void OnDestroy()
	{
		MyCenterOnChild center = this._center;
		center.onFinished = (SpringPanel.OnFinished)Delegate.Remove(center.onFinished, new SpringPanel.OnFinished(this.HandleCenteringFinished));
	}

	// Token: 0x06004EB9 RID: 20153 RVA: 0x001C8618 File Offset: 0x001C6818
	public void UpdateContent()
	{
		List<string> list = new List<string>();
		foreach (object obj in WeaponManager.sharedManager.playerWeapons)
		{
			Weapon weapon = (Weapon)obj;
			list.Add(weapon.weaponPrefab.name + "_InGamePreview");
		}
		UITexture[] componentsInChildren = base.GetComponentsInChildren<UITexture>();
		List<Texture> list2 = new List<Texture>();
		foreach (UITexture uitexture in componentsInChildren)
		{
			if (uitexture.mainTexture)
			{
				list2.Add(uitexture.mainTexture);
			}
		}
		List<string> list3 = new List<string>();
		foreach (string text in list)
		{
			bool flag = false;
			foreach (Texture texture in list2)
			{
				if (texture.name.Equals(text))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list3.Add(text);
			}
		}
		foreach (string text2 in list3)
		{
			Texture texture2 = Resources.Load(WeaponManager.WeaponPreviewsPath + "/" + text2) as Texture;
			texture2.name = text2;
			if (texture2 != null)
			{
				list2.Add(texture2);
			}
		}
		Transform child = base.transform.GetChild(0);
		int childCount = child.childCount;
		if (childCount > list.Count)
		{
			for (int j = list.Count; j < childCount; j++)
			{
				Transform child2 = child.GetChild(j);
				child2.parent = null;
				UnityEngine.Object.Destroy(child2.gameObject);
			}
		}
		else if (childCount < list.Count)
		{
			for (int k = childCount; k < list.Count; k++)
			{
				if (k >= childCount)
				{
					GameObject original = Resources.Load("WeaponPreviewPrefab") as GameObject;
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
					gameObject.transform.parent = child;
					gameObject.name = "preview_" + (k + 1);
					gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
				}
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			Transform child3 = child.GetChild(l);
			if (child3)
			{
				foreach (Texture texture3 in list2)
				{
					if (texture3.name.Equals(list[l]))
					{
						child3.GetComponent<UITexture>().mainTexture = texture3;
						break;
					}
				}
			}
		}
		this._wrapContent.SortAlphabetically();
		Transform target = this._center.transform.GetChild(0);
		foreach (object obj2 in this._wrapContent.transform)
		{
			Transform transform = (Transform)obj2;
			if (transform.gameObject.name.Equals("preview_" + (WeaponManager.sharedManager.CurrentWeaponIndex + 1)))
			{
				target = transform;
				break;
			}
		}
		this._center.CenterOn(target);
	}

	// Token: 0x04003D3E RID: 15678
	private UIWrapContent _wrapContent;

	// Token: 0x04003D3F RID: 15679
	private UIScrollView _scrollView;

	// Token: 0x04003D40 RID: 15680
	private Player_move_c move;

	// Token: 0x04003D41 RID: 15681
	private MyCenterOnChild _center;

	// Token: 0x04003D42 RID: 15682
	private bool _disabled;
}
