using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020007E7 RID: 2023
public class DebugButtonsManager : MonoBehaviour
{
	// Token: 0x06004938 RID: 18744 RVA: 0x001970E4 File Offset: 0x001952E4
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06004939 RID: 18745 RVA: 0x001970EC File Offset: 0x001952EC
	public static void ShowTopBarButton(string text, int width, Action onClickAction)
	{
		if (DebugButtonsManager._instance == null)
		{
			GameObject gameObject = new GameObject("DebugButtonsManager");
			DebugButtonsManager._instance = gameObject.AddComponent<DebugButtonsManager>();
		}
		DebugButtonsManager.TopBarButton topBarButton = DebugButtonsManager._tbButtons.FirstOrDefault((DebugButtonsManager.TopBarButton b) => b.Text == text);
		if (topBarButton != null)
		{
			topBarButton.NeedShow = true;
			return;
		}
		DebugButtonsManager.TopBarButton item = new DebugButtonsManager.TopBarButton(text, width, onClickAction);
		DebugButtonsManager._tbButtons.Add(item);
	}

	// Token: 0x04003664 RID: 13924
	private static DebugButtonsManager _instance;

	// Token: 0x04003665 RID: 13925
	private static bool _topPanelOpened = true;

	// Token: 0x04003666 RID: 13926
	private static readonly List<DebugButtonsManager.TopBarButton> _tbButtons = new List<DebugButtonsManager.TopBarButton>();

	// Token: 0x020007E8 RID: 2024
	private class TopBarButton
	{
		// Token: 0x0600493A RID: 18746 RVA: 0x0019716C File Offset: 0x0019536C
		public TopBarButton(string text, int width, Action act)
		{
			this.Text = text;
			this.Width = width;
			this.Act = act;
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x0600493B RID: 18747 RVA: 0x0019719C File Offset: 0x0019539C
		// (set) Token: 0x0600493C RID: 18748 RVA: 0x001971A4 File Offset: 0x001953A4
		public string Text { get; private set; }

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x0600493D RID: 18749 RVA: 0x001971B0 File Offset: 0x001953B0
		// (set) Token: 0x0600493E RID: 18750 RVA: 0x001971B8 File Offset: 0x001953B8
		public int Width { get; private set; }

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x0600493F RID: 18751 RVA: 0x001971C4 File Offset: 0x001953C4
		// (set) Token: 0x06004940 RID: 18752 RVA: 0x001971CC File Offset: 0x001953CC
		public Action Act { get; private set; }

		// Token: 0x04003667 RID: 13927
		public bool NeedShow = true;
	}
}
