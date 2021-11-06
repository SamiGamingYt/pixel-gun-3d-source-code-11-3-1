using System;
using UnityEngine;

// Token: 0x020007EF RID: 2031
public sealed class Friend
{
	// Token: 0x060049CB RID: 18891 RVA: 0x001994C0 File Offset: 0x001976C0
	public Friend(string friendName, string friendId, bool friendIsUser)
	{
		this.name = friendName;
		this.id = friendId;
		this.isUser = friendIsUser;
		this.avatar = null;
	}

	// Token: 0x060049CC RID: 18892 RVA: 0x001994F0 File Offset: 0x001976F0
	public void SetAvatar(Texture2D txt)
	{
		this.avatar = txt;
	}

	// Token: 0x060049CD RID: 18893 RVA: 0x001994FC File Offset: 0x001976FC
	public void SetTimeLastVisit(DateTime visitTime)
	{
		this.nextVisit = visitTime;
	}

	// Token: 0x060049CE RID: 18894 RVA: 0x00199508 File Offset: 0x00197708
	public void SetTimeLabel(UILabel tL)
	{
		this.timeLabel = tL;
	}

	// Token: 0x060049CF RID: 18895 RVA: 0x00199514 File Offset: 0x00197714
	public void SetAvatarObj(UITexture aT)
	{
		this.avatarTexture = aT;
	}

	// Token: 0x040036AA RID: 13994
	public string name;

	// Token: 0x040036AB RID: 13995
	public string id;

	// Token: 0x040036AC RID: 13996
	public bool isUser;

	// Token: 0x040036AD RID: 13997
	public Texture2D avatar;

	// Token: 0x040036AE RID: 13998
	public DateTime nextVisit;

	// Token: 0x040036AF RID: 13999
	public UILabel timeLabel;

	// Token: 0x040036B0 RID: 14000
	public UITexture avatarTexture;
}
