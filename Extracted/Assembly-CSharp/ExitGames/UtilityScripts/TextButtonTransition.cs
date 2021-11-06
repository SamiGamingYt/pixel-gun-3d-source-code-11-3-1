using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{
	// Token: 0x02000464 RID: 1124
	[RequireComponent(typeof(Text))]
	public class TextButtonTransition : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x0600275D RID: 10077 RVA: 0x000C4C64 File Offset: 0x000C2E64
		public void Awake()
		{
			this._text = base.GetComponent<Text>();
		}

		// Token: 0x0600275E RID: 10078 RVA: 0x000C4C74 File Offset: 0x000C2E74
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._text.color = this.HoverColor;
		}

		// Token: 0x0600275F RID: 10079 RVA: 0x000C4C88 File Offset: 0x000C2E88
		public void OnPointerExit(PointerEventData eventData)
		{
			this._text.color = this.NormalColor;
		}

		// Token: 0x04001B8D RID: 7053
		private Text _text;

		// Token: 0x04001B8E RID: 7054
		public Color NormalColor = Color.white;

		// Token: 0x04001B8F RID: 7055
		public Color HoverColor = Color.black;
	}
}
