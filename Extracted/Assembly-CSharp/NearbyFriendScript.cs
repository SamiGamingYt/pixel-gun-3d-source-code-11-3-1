using System;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x0200061F RID: 1567
internal sealed class NearbyFriendScript : MonoBehaviour
{
	// Token: 0x170008E9 RID: 2281
	// (get) Token: 0x06003646 RID: 13894 RVA: 0x00118584 File Offset: 0x00116784
	public bool NearbyFriendSupported
	{
		get
		{
			return BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		}
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x00118594 File Offset: 0x00116794
	private void Start()
	{
		if (this.NearbyFriendSupported)
		{
			return;
		}
		if (this.nearbyFriendGrid != null && this.otherFriendGrid != null)
		{
			this.otherFriendGrid.topAnchor.Set(this.nearbyFriendGrid.topAnchor.relative, (float)this.nearbyFriendGrid.topAnchor.absolute);
		}
		if (this.nearbyFriendHeader != null && this.otherFriendHeader != null)
		{
			this.otherFriendHeader.topAnchor.Set(this.nearbyFriendHeader.topAnchor.relative, (float)this.nearbyFriendHeader.topAnchor.absolute);
			this.otherFriendHeader.bottomAnchor.Set(this.nearbyFriendHeader.bottomAnchor.relative, (float)this.nearbyFriendHeader.bottomAnchor.absolute);
		}
		this.nearbyFriendHeader.Do(delegate(UIRect h)
		{
			h.gameObject.SetActive(false);
		});
		this.nearbyFriendGrid.Do(delegate(UIRect g)
		{
			g.gameObject.SetActive(false);
		});
	}

	// Token: 0x040027D7 RID: 10199
	public UIRect nearbyFriendHeader;

	// Token: 0x040027D8 RID: 10200
	public UIRect nearbyFriendGrid;

	// Token: 0x040027D9 RID: 10201
	public UIRect otherFriendHeader;

	// Token: 0x040027DA RID: 10202
	public UIRect otherFriendGrid;
}
