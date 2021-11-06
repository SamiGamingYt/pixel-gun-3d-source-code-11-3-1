using System;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x0200081B RID: 2075
	[RequireComponent(typeof(UISprite))]
	public class SpriteByLanguage : MonoBehaviour
	{
		// Token: 0x06004B88 RID: 19336 RVA: 0x001B2898 File Offset: 0x001B0A98
		private void Update()
		{
			if (this._prevLng != LocalizationStore.CurrentLanguage)
			{
				this._prevLng = LocalizationStore.CurrentLanguage;
				this.SetLang(this._prevLng);
			}
		}

		// Token: 0x06004B89 RID: 19337 RVA: 0x001B28D4 File Offset: 0x001B0AD4
		private void SetLang(string l)
		{
			if (l.IsNullOrEmpty())
			{
				return;
			}
			string spriteName = string.Empty;
			if (l == "Russian")
			{
				spriteName = "flag_rus";
			}
			else if (l == "English")
			{
				spriteName = "flag_us";
			}
			else if (l == "French")
			{
				spriteName = "flag_fr";
			}
			else if (l == "German")
			{
				spriteName = "flag_ger";
			}
			else if (l == "Japanese")
			{
				spriteName = "flag_jp";
			}
			else if (l == "Spanish")
			{
				spriteName = "flag_esp";
			}
			else if (l == "Chinese (chinese)")
			{
				spriteName = "flag_ch";
			}
			else if (l == "Korean")
			{
				spriteName = "flag_kr";
			}
			else
			{
				if (!(l == "Portuguese (brazil)"))
				{
					return;
				}
				spriteName = "flag_br";
			}
			UISprite component = base.GetComponent<UISprite>();
			component.spriteName = spriteName;
		}

		// Token: 0x04003A9D RID: 15005
		private string _prevLng = string.Empty;
	}
}
