using System;
using UnityEngine;

// Token: 0x02000708 RID: 1800
public class LeagueItemStot : MonoBehaviour
{
	// Token: 0x06003E8F RID: 16015 RVA: 0x0014F478 File Offset: 0x0014D678
	private void Awake()
	{
		this._baseTextureColor = this._texture.color;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003E90 RID: 16016 RVA: 0x0014F498 File Offset: 0x0014D698
	public void Set(Texture texture, bool opened, bool purchased)
	{
		base.gameObject.SetActive(true);
		if (!opened && !purchased)
		{
			this._texture.color = Color.white;
			this._lockIndicator.gameObject.SetActive(true);
		}
		else
		{
			this._texture.color = this._baseTextureColor;
			this._lockIndicator.gameObject.SetActive(false);
		}
		this._texture.mainTexture = texture;
	}

	// Token: 0x06003E91 RID: 16017 RVA: 0x0014F514 File Offset: 0x0014D714
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04002E31 RID: 11825
	[SerializeField]
	private UITexture _texture;

	// Token: 0x04002E32 RID: 11826
	[SerializeField]
	private GameObject _lockIndicator;

	// Token: 0x04002E33 RID: 11827
	private Color _baseTextureColor;
}
