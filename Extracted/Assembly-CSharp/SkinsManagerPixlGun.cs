using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007CC RID: 1996
internal sealed class SkinsManagerPixlGun : MonoBehaviour
{
	// Token: 0x0600489D RID: 18589 RVA: 0x001930E0 File Offset: 0x001912E0
	private void OnLevelWasLoaded(int idx)
	{
		if (this.skins.Count > 0)
		{
			this.skins.Clear();
		}
		string path;
		if (Defs.isMulti && Defs.isCOOP && !Defs.isCompany)
		{
			path = "EnemySkins/COOP/";
		}
		else
		{
			if (Defs.isMulti || Defs.isCOOP || Defs.isCompany)
			{
				return;
			}
			if (Defs.IsSurvival)
			{
				path = Defs.SurvSkinsPath;
			}
			else
			{
				path = "EnemySkins/Level" + ((!TrainingController.TrainingCompleted) ? "3" : CurrentCampaignGame.currentLevel.ToString());
			}
		}
		UnityEngine.Object[] array = Resources.LoadAll(path);
		try
		{
			foreach (Texture texture in array)
			{
				this.skins.Add(texture.name, texture);
			}
		}
		catch (Exception arg)
		{
			Debug.Log("Exception in SkinsManagerPixlGun: " + arg);
		}
	}

	// Token: 0x0600489E RID: 18590 RVA: 0x00193210 File Offset: 0x00191410
	private void Start()
	{
		SkinsManagerPixlGun.sharedManager = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600489F RID: 18591 RVA: 0x00193224 File Offset: 0x00191424
	private void OnDestroy()
	{
		SkinsManagerPixlGun.sharedManager = null;
	}

	// Token: 0x04003589 RID: 13705
	public Hashtable skins = new Hashtable();

	// Token: 0x0400358A RID: 13706
	public static SkinsManagerPixlGun sharedManager;
}
