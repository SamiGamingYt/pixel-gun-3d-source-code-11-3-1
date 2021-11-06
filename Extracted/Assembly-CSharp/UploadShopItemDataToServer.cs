using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Rilisoft.MiniJson;
using UnityEngine;

// Token: 0x020005E3 RID: 1507
public class UploadShopItemDataToServer : MonoBehaviour
{
	// Token: 0x060033AA RID: 13226 RVA: 0x0010B6FC File Offset: 0x001098FC
	private string GenerateJsonStringWithData()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<List<object>> value = new List<List<object>>();
		List<string> value2 = new List<string>
		{
			WeaponTags.DragonGun_Tag,
			WeaponTags.FreezeGun_0_Tag,
			WeaponTags.FreezeGunTag,
			WeaponTags.AK74Tag
		};
		List<string> value3 = new List<string>
		{
			WeaponTags.RailgunTag,
			WeaponTags.MinigunTag,
			WeaponTags.GlockTag
		};
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<List<object>> list3 = new List<List<object>>();
		for (int i = 0; i < this.itemsData.Count; i++)
		{
			if (this.itemsData[i].isNew)
			{
				list.Add(this.itemsData[i].tag);
			}
			if (this.itemsData[i].isTop)
			{
				list2.Add(this.itemsData[i].tag);
			}
			if (this.itemsData[i].discount > 0)
			{
				list3.Add(new List<object>
				{
					this.itemsData[i].tag,
					this.itemsData[i].discount
				});
			}
		}
		dictionary.Add("discounts", value);
		dictionary.Add("news", value2);
		dictionary.Add("news_up", list);
		dictionary.Add("topSellers", value3);
		dictionary.Add("topSellers_up", list2);
		dictionary.Add("discounts_up", list3);
		return Json.Serialize(dictionary);
	}

	// Token: 0x060033AB RID: 13227 RVA: 0x0010B8C4 File Offset: 0x00109AC4
	public void Show(UploadShopItemDataToServer.TypeWindow type)
	{
		base.gameObject.GetComponent<UIPanel>().alpha = 1f;
		this._typePlatform = UploadShopItemDataToServer.PlatformType.Test;
		this.defaultToggle.value = true;
		this._typeWindow = type;
		if (this._typeWindow == UploadShopItemDataToServer.TypeWindow.UploadFileToServer)
		{
			this.buttonApplyLabel.text = "Upload to server";
		}
		else if (this._typeWindow == UploadShopItemDataToServer.TypeWindow.ChangePlatform)
		{
			this.buttonApplyLabel.text = "Download from server";
		}
	}

	// Token: 0x060033AC RID: 13228 RVA: 0x0010B93C File Offset: 0x00109B3C
	public void Hide()
	{
		base.gameObject.GetComponent<UIPanel>().alpha = 0f;
	}

	// Token: 0x060033AD RID: 13229 RVA: 0x0010B954 File Offset: 0x00109B54
	public void ChangeCurrentPlatform(UIToggle toggle)
	{
		if (toggle == null || !toggle.value)
		{
			return;
		}
		string name = toggle.name;
		switch (name)
		{
		case "IOSCheckbox":
			this._typePlatform = UploadShopItemDataToServer.PlatformType.IOS;
			break;
		case "TestCheckbox":
			this._typePlatform = UploadShopItemDataToServer.PlatformType.Test;
			break;
		case "AndroidCheckbox":
			this._typePlatform = UploadShopItemDataToServer.PlatformType.Android;
			break;
		case "AmazonCheckbox":
			this._typePlatform = UploadShopItemDataToServer.PlatformType.Amazon;
			break;
		case "WindowsPhoneCheckbox":
			this._typePlatform = UploadShopItemDataToServer.PlatformType.WindowsPhone;
			break;
		}
	}

	// Token: 0x060033AE RID: 13230 RVA: 0x0010BA48 File Offset: 0x00109C48
	private string GetFileNameForPlatform(UploadShopItemDataToServer.PlatformType type)
	{
		switch (this._typePlatform)
		{
		case UploadShopItemDataToServer.PlatformType.IOS:
			return "promo_actions.php";
		case UploadShopItemDataToServer.PlatformType.Test:
			return "promo_actions_test1.php";
		case UploadShopItemDataToServer.PlatformType.Android:
			return "promo_actions_android.php";
		case UploadShopItemDataToServer.PlatformType.Amazon:
			return "promo_actions_amazon.php";
		case UploadShopItemDataToServer.PlatformType.WindowsPhone:
			return "promo_actions_wp8.php";
		default:
			return "promo_actions_test1.php";
		}
	}

	// Token: 0x060033AF RID: 13231 RVA: 0x0010BAA0 File Offset: 0x00109CA0
	public string GetPromoActionUrl()
	{
		switch (this._typePlatform)
		{
		case UploadShopItemDataToServer.PlatformType.IOS:
			return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions.json";
		case UploadShopItemDataToServer.PlatformType.Test:
			return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_test.json";
		case UploadShopItemDataToServer.PlatformType.Android:
			return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_android.json";
		case UploadShopItemDataToServer.PlatformType.Amazon:
			return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_amazon.json";
		case UploadShopItemDataToServer.PlatformType.WindowsPhone:
			return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions_wp8.json";
		default:
			return "https://secure.pixelgunserver.com/pixelgun3d-config/PromoActions/promo_actions.json";
		}
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x0010BAF8 File Offset: 0x00109CF8
	private string CreatePhpFileByString(string text)
	{
		string fileNameForPlatform = this.GetFileNameForPlatform(this._typePlatform);
		try
		{
			if (File.Exists(fileNameForPlatform))
			{
				File.Delete(fileNameForPlatform);
			}
			using (FileStream fileStream = File.Create(fileNameForPlatform))
			{
				byte[] bytes = new UTF8Encoding(true).GetBytes(text);
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
		}
		return fileNameForPlatform;
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x0010BBA0 File Offset: 0x00109DA0
	private void UploadPhpFileToServer(string fileName)
	{
		try
		{
			FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create("ftp://secure.pixelgunserver.com//test.htm");
			ftpWebRequest.Method = "STOR";
			ftpWebRequest.UsePassive = false;
			ftpWebRequest.Credentials = new NetworkCredential("rilisoft", "11QQwwee");
			FtpWebResponse ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
			Debug.Log(string.Format("Upload File Complete, status {0}", ftpWebResponse.StatusDescription));
			ftpWebResponse.Close();
		}
		catch (WebException ex)
		{
			string statusDescription = ((FtpWebResponse)ex.Response).StatusDescription;
			Debug.Log(statusDescription);
		}
	}

	// Token: 0x060033B2 RID: 13234 RVA: 0x0010BC4C File Offset: 0x00109E4C
	public string GenerateTextForUploadFile()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?php");
		string text = this.GenerateJsonStringWithData();
		text = text.Replace("\"", "\\\"");
		text += "\\r\\n";
		stringBuilder.AppendFormat("$val = \"{0}\";\n", text);
		stringBuilder.AppendLine("echo $val;");
		stringBuilder.AppendLine("?>");
		return stringBuilder.ToString();
	}

	// Token: 0x060033B3 RID: 13235 RVA: 0x0010BCBC File Offset: 0x00109EBC
	private void UploadFileToServer()
	{
		string text = this.GenerateTextForUploadFile();
		string text2 = this.CreatePhpFileByString(text);
		this.UploadPhpFileToServer(text2);
		Debug.Log(text2);
	}

	// Token: 0x060033B4 RID: 13236 RVA: 0x0010BCE8 File Offset: 0x00109EE8
	public void ApplyButtonClick()
	{
		UploadShopItemDataToServer.TypeWindow typeWindow = this._typeWindow;
		if (typeWindow != UploadShopItemDataToServer.TypeWindow.UploadFileToServer)
		{
			if (typeWindow == UploadShopItemDataToServer.TypeWindow.ChangePlatform)
			{
				this.generateButton.gameObject.SetActive(true);
				this.filtersContainer.gameObject.SetActive(true);
				this.checkAllContainer.gameObject.SetActive(true);
				this.defaultFilterToggle.value = true;
			}
		}
		else
		{
			this.UploadFileToServer();
		}
		this.Hide();
	}

	// Token: 0x040025F8 RID: 9720
	public UIToggle defaultToggle;

	// Token: 0x040025F9 RID: 9721
	public UIToggle defaultFilterToggle;

	// Token: 0x040025FA RID: 9722
	public List<EditorShopItemData> itemsData;

	// Token: 0x040025FB RID: 9723
	public UILabel buttonApplyLabel;

	// Token: 0x040025FC RID: 9724
	public UISprite generateButton;

	// Token: 0x040025FD RID: 9725
	public UIWidget filtersContainer;

	// Token: 0x040025FE RID: 9726
	public UIWidget checkAllContainer;

	// Token: 0x040025FF RID: 9727
	private UploadShopItemDataToServer.PlatformType _typePlatform;

	// Token: 0x04002600 RID: 9728
	private UploadShopItemDataToServer.TypeWindow _typeWindow;

	// Token: 0x020005E4 RID: 1508
	private enum PlatformType
	{
		// Token: 0x04002603 RID: 9731
		IOS,
		// Token: 0x04002604 RID: 9732
		Test,
		// Token: 0x04002605 RID: 9733
		Android,
		// Token: 0x04002606 RID: 9734
		Amazon,
		// Token: 0x04002607 RID: 9735
		WindowsPhone
	}

	// Token: 0x020005E5 RID: 1509
	public enum TypeWindow
	{
		// Token: 0x04002609 RID: 9737
		UploadFileToServer,
		// Token: 0x0400260A RID: 9738
		ChangePlatform
	}
}
