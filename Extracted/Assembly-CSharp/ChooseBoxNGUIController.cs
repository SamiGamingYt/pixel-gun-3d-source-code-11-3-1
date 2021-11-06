using System;
using System.Collections;
using Rilisoft;
using UnityEngine;

// Token: 0x02000067 RID: 103
public class ChooseBoxNGUIController : MonoBehaviour
{
	// Token: 0x060002D4 RID: 724 RVA: 0x000187D4 File Offset: 0x000169D4
	private IEnumerator Start()
	{
		this.ScrollTransform.GetComponent<UIPanel>().baseClipRegion = new Vector4(0f, 0f, (float)(760 * Screen.width / Screen.height), 736f);
		this.countMap = this.grid.transform.childCount;
		yield return null;
		Defs.diffGame = PlayerPrefs.GetInt(Defs.DiffSett, 1);
		if (this.difficultyToggle != null)
		{
			this.difficultyToggle.buttons[Defs.diffGame].IsChecked = true;
			this.difficultyToggle.Clicked += delegate(object sender, MultipleToggleEventArgs e)
			{
				ButtonClickSound.Instance.PlayClick();
				PlayerPrefs.SetInt(Defs.DiffSett, e.Num);
				Defs.diffGame = e.Num;
				PlayerPrefs.Save();
			};
		}
		yield break;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x000187F0 File Offset: 0x000169F0
	private void Update()
	{
		if (this.SelectMapPanel.activeInHierarchy)
		{
			if (this.ScrollTransform.localPosition.x > 0f)
			{
				this.selectIndexMap = Mathf.RoundToInt((this.ScrollTransform.localPosition.x - (float)Mathf.FloorToInt(this.ScrollTransform.localPosition.x / this.widthCell / (float)this.countMap) * this.widthCell * (float)this.countMap) / this.widthCell);
				this.selectIndexMap = this.countMap - this.selectIndexMap;
			}
			else
			{
				this.selectIndexMap = -1 * Mathf.RoundToInt((this.ScrollTransform.localPosition.x - (float)Mathf.CeilToInt(this.ScrollTransform.localPosition.x / this.widthCell / (float)this.countMap) * this.widthCell * (float)this.countMap) / this.widthCell);
			}
			if (this.selectIndexMap == this.countMap)
			{
				this.selectIndexMap = 0;
			}
		}
	}

	// Token: 0x040002FF RID: 767
	public MultipleToggleButton difficultyToggle;

	// Token: 0x04000300 RID: 768
	public UIButton backButton;

	// Token: 0x04000301 RID: 769
	public UIButton startButton;

	// Token: 0x04000302 RID: 770
	public GameObject grid;

	// Token: 0x04000303 RID: 771
	public Transform ScrollTransform;

	// Token: 0x04000304 RID: 772
	public GameObject SelectMapPanel;

	// Token: 0x04000305 RID: 773
	public int selectIndexMap;

	// Token: 0x04000306 RID: 774
	public int countMap;

	// Token: 0x04000307 RID: 775
	public float widthCell = 824f;
}
