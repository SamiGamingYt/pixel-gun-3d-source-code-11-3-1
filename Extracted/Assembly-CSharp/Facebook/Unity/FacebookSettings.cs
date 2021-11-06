using System;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook.Unity
{
	// Token: 0x020000D0 RID: 208
	public class FacebookSettings : ScriptableObject
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x0002D150 File Offset: 0x0002B350
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x0002D15C File Offset: 0x0002B35C
		public static int SelectedAppIndex
		{
			get
			{
				return FacebookSettings.Instance.selectedAppIndex;
			}
			set
			{
				if (FacebookSettings.Instance.selectedAppIndex != value)
				{
					FacebookSettings.Instance.selectedAppIndex = value;
					FacebookSettings.DirtyEditor();
				}
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0002D18C File Offset: 0x0002B38C
		// (set) Token: 0x06000641 RID: 1601 RVA: 0x0002D198 File Offset: 0x0002B398
		public static List<string> AppIds
		{
			get
			{
				return FacebookSettings.Instance.appIds;
			}
			set
			{
				if (FacebookSettings.Instance.appIds != value)
				{
					FacebookSettings.Instance.appIds = value;
					FacebookSettings.DirtyEditor();
				}
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0002D1C8 File Offset: 0x0002B3C8
		// (set) Token: 0x06000643 RID: 1603 RVA: 0x0002D1D4 File Offset: 0x0002B3D4
		public static List<string> AppLabels
		{
			get
			{
				return FacebookSettings.Instance.appLabels;
			}
			set
			{
				if (FacebookSettings.Instance.appLabels != value)
				{
					FacebookSettings.Instance.appLabels = value;
					FacebookSettings.DirtyEditor();
				}
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0002D204 File Offset: 0x0002B404
		public static string AppId
		{
			get
			{
				return FacebookSettings.AppIds[FacebookSettings.SelectedAppIndex];
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0002D218 File Offset: 0x0002B418
		public static bool IsValidAppId
		{
			get
			{
				return FacebookSettings.AppId != null && FacebookSettings.AppId.Length > 0 && !FacebookSettings.AppId.Equals("0");
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0002D254 File Offset: 0x0002B454
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x0002D260 File Offset: 0x0002B460
		public static bool Cookie
		{
			get
			{
				return FacebookSettings.Instance.cookie;
			}
			set
			{
				if (FacebookSettings.Instance.cookie != value)
				{
					FacebookSettings.Instance.cookie = value;
					FacebookSettings.DirtyEditor();
				}
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0002D290 File Offset: 0x0002B490
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x0002D29C File Offset: 0x0002B49C
		public static bool Logging
		{
			get
			{
				return FacebookSettings.Instance.logging;
			}
			set
			{
				if (FacebookSettings.Instance.logging != value)
				{
					FacebookSettings.Instance.logging = value;
					FacebookSettings.DirtyEditor();
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0002D2CC File Offset: 0x0002B4CC
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x0002D2D8 File Offset: 0x0002B4D8
		public static bool Status
		{
			get
			{
				return FacebookSettings.Instance.status;
			}
			set
			{
				if (FacebookSettings.Instance.status != value)
				{
					FacebookSettings.Instance.status = value;
					FacebookSettings.DirtyEditor();
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0002D308 File Offset: 0x0002B508
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x0002D314 File Offset: 0x0002B514
		public static bool Xfbml
		{
			get
			{
				return FacebookSettings.Instance.xfbml;
			}
			set
			{
				if (FacebookSettings.Instance.xfbml != value)
				{
					FacebookSettings.Instance.xfbml = value;
					FacebookSettings.DirtyEditor();
				}
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0002D344 File Offset: 0x0002B544
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x0002D350 File Offset: 0x0002B550
		public static string IosURLSuffix
		{
			get
			{
				return FacebookSettings.Instance.iosURLSuffix;
			}
			set
			{
				if (FacebookSettings.Instance.iosURLSuffix != value)
				{
					FacebookSettings.Instance.iosURLSuffix = value;
					FacebookSettings.DirtyEditor();
				}
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x0002D378 File Offset: 0x0002B578
		public static string ChannelUrl
		{
			get
			{
				return "/channel.html";
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x0002D380 File Offset: 0x0002B580
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x0002D38C File Offset: 0x0002B58C
		public static bool FrictionlessRequests
		{
			get
			{
				return FacebookSettings.Instance.frictionlessRequests;
			}
			set
			{
				if (FacebookSettings.Instance.frictionlessRequests != value)
				{
					FacebookSettings.Instance.frictionlessRequests = value;
					FacebookSettings.DirtyEditor();
				}
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x0002D3BC File Offset: 0x0002B5BC
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x0002D3C8 File Offset: 0x0002B5C8
		public static List<FacebookSettings.UrlSchemes> AppLinkSchemes
		{
			get
			{
				return FacebookSettings.Instance.appLinkSchemes;
			}
			set
			{
				if (FacebookSettings.Instance.appLinkSchemes != value)
				{
					FacebookSettings.Instance.appLinkSchemes = value;
					FacebookSettings.DirtyEditor();
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x0002D3F8 File Offset: 0x0002B5F8
		private static FacebookSettings Instance
		{
			get
			{
				if (FacebookSettings.instance == null)
				{
					FacebookSettings.instance = (Resources.Load("FacebookSettings") as FacebookSettings);
					if (FacebookSettings.instance == null)
					{
						FacebookSettings.instance = ScriptableObject.CreateInstance<FacebookSettings>();
					}
				}
				return FacebookSettings.instance;
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0002D448 File Offset: 0x0002B648
		public static void SettingsChanged()
		{
			FacebookSettings.DirtyEditor();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0002D450 File Offset: 0x0002B650
		private static void DirtyEditor()
		{
		}

		// Token: 0x0400062C RID: 1580
		private const string FacebookSettingsAssetName = "FacebookSettings";

		// Token: 0x0400062D RID: 1581
		private const string FacebookSettingsPath = "FacebookSDK/SDK/Resources";

		// Token: 0x0400062E RID: 1582
		private const string FacebookSettingsAssetExtension = ".asset";

		// Token: 0x0400062F RID: 1583
		private static FacebookSettings instance;

		// Token: 0x04000630 RID: 1584
		[SerializeField]
		private int selectedAppIndex;

		// Token: 0x04000631 RID: 1585
		[SerializeField]
		private List<string> appIds = new List<string>
		{
			"0"
		};

		// Token: 0x04000632 RID: 1586
		[SerializeField]
		private List<string> appLabels = new List<string>
		{
			"App Name"
		};

		// Token: 0x04000633 RID: 1587
		[SerializeField]
		private bool cookie = true;

		// Token: 0x04000634 RID: 1588
		[SerializeField]
		private bool logging = true;

		// Token: 0x04000635 RID: 1589
		[SerializeField]
		private bool status = true;

		// Token: 0x04000636 RID: 1590
		[SerializeField]
		private bool xfbml;

		// Token: 0x04000637 RID: 1591
		[SerializeField]
		private bool frictionlessRequests = true;

		// Token: 0x04000638 RID: 1592
		[SerializeField]
		private string iosURLSuffix = string.Empty;

		// Token: 0x04000639 RID: 1593
		[SerializeField]
		private List<FacebookSettings.UrlSchemes> appLinkSchemes = new List<FacebookSettings.UrlSchemes>
		{
			new FacebookSettings.UrlSchemes(null)
		};

		// Token: 0x020000D1 RID: 209
		[Serializable]
		public class UrlSchemes
		{
			// Token: 0x06000658 RID: 1624 RVA: 0x0002D454 File Offset: 0x0002B654
			public UrlSchemes(List<string> schemes = null)
			{
				this.list = ((schemes != null) ? schemes : new List<string>());
			}

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x06000659 RID: 1625 RVA: 0x0002D474 File Offset: 0x0002B674
			// (set) Token: 0x0600065A RID: 1626 RVA: 0x0002D47C File Offset: 0x0002B67C
			public List<string> Schemes
			{
				get
				{
					return this.list;
				}
				set
				{
					this.list = value;
				}
			}

			// Token: 0x0400063A RID: 1594
			[SerializeField]
			private List<string> list;
		}
	}
}
