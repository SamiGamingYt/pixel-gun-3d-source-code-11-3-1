using System;
using System.Globalization;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

// Token: 0x02000686 RID: 1670
internal sealed class LeaderboardItemView : MonoBehaviour
{
	// Token: 0x1400006F RID: 111
	// (add) Token: 0x06003A70 RID: 14960 RVA: 0x0012DEB0 File Offset: 0x0012C0B0
	// (remove) Token: 0x06003A71 RID: 14961 RVA: 0x0012DECC File Offset: 0x0012C0CC
	public event EventHandler<ClickedEventArgs> Clicked;

	// Token: 0x06003A72 RID: 14962 RVA: 0x0012DEE8 File Offset: 0x0012C0E8
	public void NewReset(LeaderboardItemViewModel viewModel)
	{
		LeaderboardItemViewModel leaderboardItemViewModel = viewModel ?? LeaderboardItemViewModel.Empty;
		this.Clicked = null;
		this._id = leaderboardItemViewModel.Id;
		UILabel.Effect effectStyle = UILabel.Effect.Outline;
		this.highlightSprite.Do(delegate(UISprite h)
		{
			h.gameObject.SetActive(viewModel.Highlight);
		});
		this.placeLabel.Do(delegate(UILabel l)
		{
			l.text = viewModel.Place.ToString(CultureInfo.InvariantCulture);
			l.effectStyle = effectStyle;
		});
		this.nicknameLabel.Do(delegate(UILabel l)
		{
			l.text = (viewModel.Nickname ?? string.Empty);
			l.effectStyle = effectStyle;
		});
		this.levelLabel.Do(delegate(UILabel l)
		{
			l.text = viewModel.Rank.ToString(CultureInfo.InvariantCulture);
			l.effectStyle = effectStyle;
		});
		this.clanLogo.Do(delegate(UITexture s)
		{
			LeaderboardScript.SetClanLogo(s, viewModel.ClanLogoTexture);
		});
		this.clanNameLabel.Do(delegate(UILabel l)
		{
			l.text = ((!string.IsNullOrEmpty(viewModel.ClanName)) ? viewModel.ClanName : LocalizationStore.Get("Key_1500"));
			l.effectStyle = effectStyle;
		});
		this.winCountLabel.Do(delegate(UILabel l)
		{
			l.text = viewModel.WinCount.ToString(CultureInfo.InvariantCulture);
			l.effectStyle = effectStyle;
		});
		if (this.background != null)
		{
			if ((float)viewModel.Place % 2f > 0f)
			{
				Color color = new Color(0.8f, 0.9f, 1f);
				base.GetComponent<UIButton>().defaultColor = color;
				this.background.color = color;
			}
			else
			{
				Color color2 = new Color(1f, 1f, 1f);
				base.GetComponent<UIButton>().defaultColor = color2;
				this.background.color = color2;
			}
		}
	}

	// Token: 0x06003A73 RID: 14963 RVA: 0x0012E068 File Offset: 0x0012C268
	public void HandleClick()
	{
		ClickedEventArgs e = new ClickedEventArgs(this._id);
		this.Clicked.Do(delegate(EventHandler<ClickedEventArgs> handler)
		{
			handler(this, e);
		});
	}

	// Token: 0x06003A74 RID: 14964 RVA: 0x0012E0AC File Offset: 0x0012C2AC
	[Obsolete]
	public void Reset(LeaderboardItemViewModel viewModel)
	{
		LeaderboardItemViewModel leaderboardItemViewModel = viewModel ?? LeaderboardItemViewModel.Empty;
		Func<object, string> func = delegate(object s)
		{
			string text = s.ToString();
			if (viewModel.Highlight)
			{
				text = string.Format("[{0}]{1}[-]", "FFFF00", text);
			}
			return text;
		};
		if (this.rankSprite != null)
		{
			this.rankSprite.spriteName = "Rank_" + Mathf.Clamp(leaderboardItemViewModel.Rank, 1, 31);
		}
		if (this.clanLogo != null)
		{
			if (!string.IsNullOrEmpty(leaderboardItemViewModel.ClanLogo))
			{
				try
				{
					byte[] data = Convert.FromBase64String(leaderboardItemViewModel.ClanLogo ?? string.Empty);
					Texture2D texture2D = new Texture2D(Defs.LogoWidth, Defs.LogoHeight, TextureFormat.ARGB32, false);
					texture2D.LoadImage(data);
					texture2D.filterMode = FilterMode.Point;
					texture2D.Apply();
					Texture mainTexture = this.clanLogo.mainTexture;
					this.clanLogo.mainTexture = texture2D;
					if (mainTexture != null)
					{
						UnityEngine.Object.Destroy(mainTexture);
					}
				}
				catch
				{
					Texture mainTexture2 = this.clanLogo.mainTexture;
					this.clanLogo.mainTexture = null;
					if (mainTexture2 != null)
					{
						UnityEngine.Object.Destroy(mainTexture2);
					}
				}
			}
			else
			{
				Texture mainTexture3 = this.clanLogo.mainTexture;
				this.clanLogo.mainTexture = null;
				if (mainTexture3 != null)
				{
					UnityEngine.Object.Destroy(mainTexture3);
				}
			}
		}
		if (this.nicknameLabel != null)
		{
			string arg = leaderboardItemViewModel.Nickname ?? string.Empty;
			this.nicknameLabel.text = func(arg);
		}
		if (this.winCountLabel != null)
		{
			this.winCountLabel.text = ((leaderboardItemViewModel != LeaderboardItemViewModel.Empty) ? func((leaderboardItemViewModel.WinCount != int.MinValue) ? Math.Max(leaderboardItemViewModel.WinCount, 0).ToString() : "—") : string.Empty);
		}
		if (this.placeLabel != null)
		{
			this.placeLabel.text = ((leaderboardItemViewModel != LeaderboardItemViewModel.Empty) ? func((leaderboardItemViewModel.Place >= 0) ? leaderboardItemViewModel.Place.ToString() : LocalizationStore.Key_0588) : string.Empty);
		}
	}

	// Token: 0x06003A75 RID: 14965 RVA: 0x0012E330 File Offset: 0x0012C530
	[Obsolete]
	public void Reset()
	{
		this.Reset(null);
	}

	// Token: 0x04002AFA RID: 11002
	private const string HighlightColor = "FFFF00";

	// Token: 0x04002AFB RID: 11003
	public string _id;

	// Token: 0x04002AFC RID: 11004
	public UISprite rankSprite;

	// Token: 0x04002AFD RID: 11005
	public UILabel nicknameLabel;

	// Token: 0x04002AFE RID: 11006
	public UILabel winCountLabel;

	// Token: 0x04002AFF RID: 11007
	public UILabel placeLabel;

	// Token: 0x04002B00 RID: 11008
	public UITexture clanLogo;

	// Token: 0x04002B01 RID: 11009
	public UILabel clanNameLabel;

	// Token: 0x04002B02 RID: 11010
	public UISprite highlightSprite;

	// Token: 0x04002B03 RID: 11011
	public UILabel levelLabel;

	// Token: 0x04002B04 RID: 11012
	public UISprite background;
}
