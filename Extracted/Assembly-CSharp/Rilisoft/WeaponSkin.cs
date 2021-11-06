using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	// Token: 0x02000886 RID: 2182
	[Serializable]
	public class WeaponSkin
	{
		// Token: 0x06004E8E RID: 20110 RVA: 0x001C7738 File Offset: 0x001C5938
		public bool IsForWeapon(string weaponName)
		{
			if (weaponName == null)
			{
				Debug.LogError("WeaponSkin IsForWeapon weaponName == null");
				return false;
			}
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponName);
			if (byPrefabName == null)
			{
				Debug.LogError("WeaponSkin IsForWeapon rec == null, weaponName: " + weaponName);
				return false;
			}
			List<string> list = WeaponUpgrades.ChainForTag(byPrefabName.Tag);
			if (list == null)
			{
				return weaponName == this.ToWeapons[0];
			}
			ItemRecord byTag = ItemDb.GetByTag(list[0]);
			if (byTag == null)
			{
				Debug.LogError("WeaponSkin IsForWeapon recOfFirstWeapon == null, weaponName: " + weaponName);
				return false;
			}
			return byTag.PrefabName == this.ToWeapons[0];
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x001C77D0 File Offset: 0x001C59D0
		public bool SetTo(GameObject go)
		{
			bool result;
			try
			{
				if (go == null)
				{
					result = false;
				}
				else
				{
					go = go.GetChildGameObject("Arms_Mesh", true);
					if (go == null)
					{
						result = false;
					}
					else
					{
						bool flag = false;
						foreach (WeaponSkinTexture weaponSkinTexture in this.Textures)
						{
							if (!weaponSkinTexture.ToObjects.IsNullOrEmpty<string>())
							{
								foreach (string name in weaponSkinTexture.ToObjects)
								{
									GameObject childGameObject = go.GetChildGameObject(name, true);
									if (childGameObject != null)
									{
										Renderer component = childGameObject.GetComponent<Renderer>();
										if (component == null)
										{
											flag = true;
										}
										Material material = component.material;
										if (material.shader.name != this.ShaderName)
										{
											Shader shader = Shader.Find(this.ShaderName);
											if (shader == null)
											{
												flag = true;
											}
											material = new Material(shader);
											component.material = material;
										}
										if (weaponSkinTexture.ShaderPropertyName.IsNullOrEmpty())
										{
											weaponSkinTexture.ShaderPropertyName = "_MainTex";
										}
										material.SetTexture(weaponSkinTexture.ShaderPropertyName, weaponSkinTexture.Texture);
										if (childGameObject.GetComponent<UvScroller>() == null)
										{
											childGameObject.AddComponent<UvScroller>();
										}
									}
									else
									{
										flag = true;
									}
								}
							}
						}
						if (flag)
						{
							Debug.LogErrorFormat("[WEAPON SKIN] set error: skin:'{0}', go:'{1}'", new object[]
							{
								this.Id,
								go.name
							});
						}
						result = !flag;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("{0} {1}", new object[]
				{
					ex.Message,
					ex.StackTrace
				});
				result = false;
			}
			return result;
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x001C79CC File Offset: 0x001C5BCC
		public static WeaponSkin CreateFromWeapon(GameObject go, string id, int price, VirtualCurrencyBonusType currency, string weaponId)
		{
			go = go.GetChildGameObject("Arms_Mesh", true);
			if (go == null)
			{
				return null;
			}
			List<WeaponSkinTexture> list = new List<WeaponSkinTexture>();
			foreach (GameObject gameObject in go.Descendants())
			{
				Renderer component = gameObject.GetComponent<Renderer>();
				if (!(component == null))
				{
					Material material = component.material;
					Texture2D texture2D = material.mainTexture as Texture2D;
					if (!(texture2D == null))
					{
						string raw = SkinsController.StringFromTexture(texture2D);
						list.Add(new WeaponSkinTexture(raw, texture2D.width, texture2D.height, new string[]
						{
							gameObject.name
						}));
					}
				}
			}
			WeaponSkin weaponSkin = new WeaponSkin();
			weaponSkin.Id = id;
			weaponSkin.Price = price;
			weaponSkin.Currency = currency;
			weaponSkin.Textures = list.ToArray();
			ItemRecord byPrefabName = ItemDb.GetByPrefabName(weaponId);
			if (byPrefabName != null)
			{
				List<string> list2 = WeaponUpgrades.ChainForTag(byPrefabName.Tag);
				if (list2 == null)
				{
					weaponSkin.ToWeapons = new string[]
					{
						weaponId
					};
				}
				else
				{
					ItemRecord byTag = ItemDb.GetByTag(list2[0]);
					weaponSkin.ToWeapons = new string[]
					{
						byTag.PrefabName
					};
				}
			}
			return weaponSkin;
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x001C7B50 File Offset: 0x001C5D50
		public static string GetWeaponId(string containsString)
		{
			Match match = WeaponSkin._regex.Match(containsString);
			return (!match.Success) ? string.Empty : match.Value;
		}

		// Token: 0x04003D21 RID: 15649
		public string Id = string.Empty;

		// Token: 0x04003D22 RID: 15650
		public string Lkey = string.Empty;

		// Token: 0x04003D23 RID: 15651
		public int Price;

		// Token: 0x04003D24 RID: 15652
		public VirtualCurrencyBonusType Currency;

		// Token: 0x04003D25 RID: 15653
		public RatingSystem.RatingLeague ForLeague;

		// Token: 0x04003D26 RID: 15654
		public string ShaderName = "Mobile/Diffuse";

		// Token: 0x04003D27 RID: 15655
		public WeaponSkinTexture[] Textures = new WeaponSkinTexture[0];

		// Token: 0x04003D28 RID: 15656
		public string[] ToWeapons = new string[0];

		// Token: 0x04003D29 RID: 15657
		private static Regex _regex = new Regex("Weapon([0-9])+");
	}
}
