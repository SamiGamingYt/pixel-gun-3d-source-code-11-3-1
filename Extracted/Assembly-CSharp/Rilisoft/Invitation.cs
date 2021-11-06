using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x020002D3 RID: 723
	internal sealed class Invitation : MonoBehaviour
	{
		// Token: 0x0600195F RID: 6495 RVA: 0x00064B8C File Offset: 0x00062D8C
		private void Start()
		{
			this._timeToUpdateInactivityState = 25f;
			this.inactivityStartTm = float.PositiveInfinity;
			this.UpdateInfo();
			if (this.JoinClan != null)
			{
				this.JoinClan.SetActive(this.IsClanInv && string.IsNullOrEmpty(FriendsController.sharedController.ClanID) && string.IsNullOrEmpty(FriendsController.sharedController.JoinClanSent));
			}
			if (this.RejectClan != null)
			{
				this.RejectClan.SetActive(this.IsClanInv);
			}
			if (this.youAlready != null)
			{
				this.youAlready.SetActive(this.IsClanInv && !string.IsNullOrEmpty(FriendsController.sharedController.ClanID));
			}
			if (this.ClanLogo != null)
			{
				this.ClanLogo.gameObject.SetActive(this.IsClanInv);
			}
			if (this.accept != null)
			{
				this.accept.SetActive(!this.IsClanInv);
			}
			this.reject.SetActive(!this.IsClanInv);
			this.rank.gameObject.SetActive(!this.IsClanInv);
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x00064CDC File Offset: 0x00062EDC
		public void KeepClanData()
		{
			FriendsController.sharedController.tempClanID = this.id;
			FriendsController.sharedController.tempClanLogo = (this.clanLogoString ?? string.Empty);
			FriendsController.sharedController.tempClanName = (this.nm.text ?? string.Empty);
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x00064D38 File Offset: 0x00062F38
		private void UpdateInfo()
		{
			if (this.IsClanInv)
			{
				foreach (Dictionary<string, string> dictionary in FriendsController.sharedController.ClanInvites)
				{
					string text;
					if (dictionary.TryGetValue("id", out text) && text.Equals(this.id))
					{
						string s;
						if (dictionary.TryGetValue("logo", out s))
						{
							try
							{
								byte[] data = Convert.FromBase64String(s);
								Texture2D texture2D = new Texture2D(8, 8, TextureFormat.ARGB32, false);
								texture2D.LoadImage(data);
								texture2D.filterMode = FilterMode.Point;
								texture2D.Apply();
								Texture mainTexture = this.ClanLogo.mainTexture;
								this.ClanLogo.mainTexture = texture2D;
								if (mainTexture != null)
								{
									UnityEngine.Object.Destroy(mainTexture);
								}
							}
							catch (Exception ex)
							{
								Texture mainTexture2 = this.ClanLogo.mainTexture;
								this.ClanLogo.mainTexture = null;
								if (mainTexture2 != null)
								{
									UnityEngine.Object.Destroy(mainTexture2);
								}
							}
						}
						string text2;
						if (dictionary.TryGetValue("name", out text2))
						{
							this.nm.text = text2;
						}
						break;
					}
				}
				return;
			}
			Dictionary<string, object> dictionary2;
			object obj;
			if (this.id != null && FriendsController.sharedController.playersInfo.TryGetValue(this.id, out dictionary2) && dictionary2.TryGetValue("player", out obj))
			{
				Dictionary<string, object> dictionary3 = obj as Dictionary<string, object>;
				this.nm.text = (dictionary3["nick"] as string);
				string text3 = Convert.ToString(dictionary3["rank"]);
				this.rank.spriteName = "Rank_" + ((!text3.Equals("0")) ? text3 : "1");
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x00064F50 File Offset: 0x00063150
		public void DisableButtons()
		{
			if (this.accept != null)
			{
				this.accept.SetActive(false);
			}
			this.reject.SetActive(false);
			this.inactivityStartTm = Time.realtimeSinceStartup;
			if (this.JoinClan != null)
			{
				this.JoinClan.SetActive(false);
			}
			if (this.RejectClan != null)
			{
				this.RejectClan.SetActive(false);
			}
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x00064FCC File Offset: 0x000631CC
		private void Update()
		{
			if (Time.realtimeSinceStartup - this.inactivityStartTm > this._timeToUpdateInactivityState)
			{
				this.inactivityStartTm = float.PositiveInfinity;
				if (this.accept != null)
				{
					this.accept.SetActive(true);
				}
				this.reject.SetActive(!this.IsClanInv);
				if (this.JoinClan != null)
				{
					this.JoinClan.SetActive(this.IsClanInv && string.IsNullOrEmpty(FriendsController.sharedController.ClanID) && string.IsNullOrEmpty(FriendsController.sharedController.JoinClanSent));
				}
				if (this.RejectClan != null)
				{
					this.RejectClan.SetActive(this.IsClanInv);
				}
				if (this.youAlready != null)
				{
					this.youAlready.SetActive(this.IsClanInv && !string.IsNullOrEmpty(FriendsController.sharedController.ClanID));
				}
			}
			if (Time.realtimeSinceStartup - this.timeLastCheck > 1f)
			{
				this.timeLastCheck = Time.realtimeSinceStartup;
				this.UpdateInfo();
			}
		}

		// Token: 0x04000E6F RID: 3695
		public UILabel nm;

		// Token: 0x04000E70 RID: 3696
		public GameObject accept;

		// Token: 0x04000E71 RID: 3697
		public GameObject reject;

		// Token: 0x04000E72 RID: 3698
		public GameObject JoinClan;

		// Token: 0x04000E73 RID: 3699
		public GameObject RejectClan;

		// Token: 0x04000E74 RID: 3700
		public GameObject youAlready;

		// Token: 0x04000E75 RID: 3701
		public UISprite rank;

		// Token: 0x04000E76 RID: 3702
		public string id;

		// Token: 0x04000E77 RID: 3703
		public string recordId;

		// Token: 0x04000E78 RID: 3704
		public bool outgoing;

		// Token: 0x04000E79 RID: 3705
		public bool IsClanInv;

		// Token: 0x04000E7A RID: 3706
		public UITexture ClanLogo;

		// Token: 0x04000E7B RID: 3707
		public string clanLogoString;

		// Token: 0x04000E7C RID: 3708
		private float timeLastCheck;

		// Token: 0x04000E7D RID: 3709
		private float _timeToUpdateInactivityState;

		// Token: 0x04000E7E RID: 3710
		private float inactivityStartTm;
	}
}
