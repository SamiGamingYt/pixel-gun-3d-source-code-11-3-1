using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000B0 RID: 176
[DisallowMultipleComponent]
internal sealed class EnemiesLabel : MonoBehaviour
{
	// Token: 0x0600052D RID: 1325 RVA: 0x0002A070 File Offset: 0x00028270
	private void Start()
	{
		bool flag = !Defs.isMulti;
		base.gameObject.SetActive(flag);
		if (!flag)
		{
			return;
		}
		this._label = base.GetComponent<UILabel>();
		this._zombieCreator = GameObject.FindGameObjectWithTag("GameController").GetComponent<ZombieCreator>();
		this._sceneName = (SceneManager.GetActiveScene().name ?? string.Empty);
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0002A0DC File Offset: 0x000282DC
	private void Update()
	{
		this._label.text = this.GetEnemiesCountString();
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x0002A0F0 File Offset: 0x000282F0
	private void OnDestroy()
	{
		this._enemiesCountMemo = new KeyValuePair<int, string>(0, string.Empty);
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0002A104 File Offset: 0x00028304
	private string GetEnemiesCountString()
	{
		int num = ZombieCreator.GetEnemiesToKill(this._sceneName) - this._zombieCreator.NumOfDeadZombies;
		if (num != this._enemiesCountMemo.Key)
		{
			string value = num.ToString(CultureInfo.InvariantCulture);
			this._enemiesCountMemo = new KeyValuePair<int, string>(num, value);
		}
		return this._enemiesCountMemo.Value;
	}

	// Token: 0x040005AA RID: 1450
	private UILabel _label;

	// Token: 0x040005AB RID: 1451
	private ZombieCreator _zombieCreator;

	// Token: 0x040005AC RID: 1452
	private KeyValuePair<int, string> _enemiesCountMemo = new KeyValuePair<int, string>(0, "0");

	// Token: 0x040005AD RID: 1453
	private string _sceneName = string.Empty;
}
