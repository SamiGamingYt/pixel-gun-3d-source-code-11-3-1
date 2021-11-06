using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{
	// Token: 0x02000465 RID: 1125
	[RequireComponent(typeof(Text))]
	public class TextToggleIsOnTransition : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x06002761 RID: 10081 RVA: 0x000C4CDC File Offset: 0x000C2EDC
		public void OnEnable()
		{
			this._text = base.GetComponent<Text>();
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000C4D14 File Offset: 0x000C2F14
		public void OnDisable()
		{
			this.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000C4D34 File Offset: 0x000C2F34
		public void OnValueChanged(bool isOn)
		{
			this._text.color = ((!isOn) ? ((!this.isHover) ? this.NormalOffColor : this.NormalOnColor) : ((!this.isHover) ? this.HoverOffColor : this.HoverOnColor));
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000C4D90 File Offset: 0x000C2F90
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.isHover = true;
			this._text.color = ((!this.toggle.isOn) ? this.HoverOffColor : this.HoverOnColor);
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000C4DC8 File Offset: 0x000C2FC8
		public void OnPointerExit(PointerEventData eventData)
		{
			this.isHover = false;
			this._text.color = ((!this.toggle.isOn) ? this.NormalOffColor : this.NormalOnColor);
		}

		// Token: 0x04001B90 RID: 7056
		public Toggle toggle;

		// Token: 0x04001B91 RID: 7057
		private Text _text;

		// Token: 0x04001B92 RID: 7058
		public Color NormalOnColor = Color.white;

		// Token: 0x04001B93 RID: 7059
		public Color NormalOffColor = Color.black;

		// Token: 0x04001B94 RID: 7060
		public Color HoverOnColor = Color.black;

		// Token: 0x04001B95 RID: 7061
		public Color HoverOffColor = Color.black;

		// Token: 0x04001B96 RID: 7062
		private bool isHover;
	}
}
