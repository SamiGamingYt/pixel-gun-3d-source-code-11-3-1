using System;
using ArabicSupport;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x020002C3 RID: 707
	[AddComponentMenu("I2/Localization/Localize")]
	public class Localize : MonoBehaviour
	{
		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06001684 RID: 5764 RVA: 0x0005A444 File Offset: 0x00058644
		// (remove) Token: 0x06001685 RID: 5765 RVA: 0x0005A460 File Offset: 0x00058660
		public event Action EventFindTarget;

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001686 RID: 5766 RVA: 0x0005A47C File Offset: 0x0005867C
		// (set) Token: 0x06001687 RID: 5767 RVA: 0x0005A484 File Offset: 0x00058684
		public string Term
		{
			get
			{
				return this.mTerm;
			}
			set
			{
				this.mTerm = value;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001688 RID: 5768 RVA: 0x0005A490 File Offset: 0x00058690
		// (set) Token: 0x06001689 RID: 5769 RVA: 0x0005A498 File Offset: 0x00058698
		public string SecondaryTerm
		{
			get
			{
				return this.mTermSecondary;
			}
			set
			{
				this.mTermSecondary = value;
			}
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x0005A4A4 File Offset: 0x000586A4
		private void Awake()
		{
			this.RegisterTargets();
			this.EventFindTarget();
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x0005A4B8 File Offset: 0x000586B8
		private void RegisterTargets()
		{
			if (this.EventFindTarget != null)
			{
				return;
			}
			this.RegisterEvents_NGUI();
			Localize.RegisterEvents_DFGUI();
			this.RegisterEvents_UGUI();
			Localize.RegisterEvents_2DToolKit();
			Localize.RegisterEvents_TextMeshPro();
			this.RegisterEvents_UnityStandard();
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x0005A4F4 File Offset: 0x000586F4
		private void OnEnable()
		{
			this.OnLocalize();
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x0005A4FC File Offset: 0x000586FC
		public void OnLocalize()
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			if (!this.HasTargetCache())
			{
				this.FindTarget();
			}
			if (!this.HasTargetCache())
			{
				return;
			}
			this.GetFinalTerms(out this.FinalTerm, out this.FinalSecondaryTerm);
			if (string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				return;
			}
			Localize.MainTranslation = LocalizationManager.GetTermTranslation(this.FinalTerm);
			Localize.SecondaryTranslation = LocalizationManager.GetTermTranslation(this.FinalSecondaryTerm);
			if (string.IsNullOrEmpty(Localize.MainTranslation) && string.IsNullOrEmpty(Localize.SecondaryTranslation))
			{
				return;
			}
			this.LocalizeCallBack.Execute(this);
			if (LocalizationManager.IsRight2Left && !this.IgnoreRTL)
			{
				if (this.AllowMainTermToBeRTL && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					Localize.MainTranslation = ArabicFixer.Fix(Localize.MainTranslation);
				}
				if (this.AllowSecondTermToBeRTL && !string.IsNullOrEmpty(Localize.SecondaryTranslation))
				{
					Localize.SecondaryTranslation = ArabicFixer.Fix(Localize.SecondaryTranslation);
				}
			}
			Localize.TermModification termModification = this.PrimaryTermModifier;
			if (termModification != Localize.TermModification.ToUpper)
			{
				if (termModification == Localize.TermModification.ToLower)
				{
					Localize.MainTranslation = Localize.MainTranslation.ToLower();
				}
			}
			else
			{
				Localize.MainTranslation = Localize.MainTranslation.ToUpper();
			}
			termModification = this.SecondaryTermModifier;
			if (termModification != Localize.TermModification.ToUpper)
			{
				if (termModification == Localize.TermModification.ToLower)
				{
					Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToLower();
				}
			}
			else
			{
				Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToUpper();
			}
			this.EventDoLocalize(Localize.MainTranslation, Localize.SecondaryTranslation);
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x0005A6D4 File Offset: 0x000588D4
		public bool FindTarget()
		{
			if (this.EventFindTarget == null)
			{
				this.RegisterTargets();
			}
			this.EventFindTarget();
			return this.HasTargetCache();
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x0005A704 File Offset: 0x00058904
		public void FindAndCacheTarget<T>(ref T targetCache, Localize.DelegateSetFinalTerms setFinalTerms, Localize.DelegateDoLocalize doLocalize, bool UseSecondaryTerm, bool MainRTL, bool SecondRTL) where T : Component
		{
			if (this.mTarget != null)
			{
				targetCache = (this.mTarget as T);
			}
			else
			{
				this.mTarget = (targetCache = base.GetComponent<T>());
			}
			if (targetCache != null)
			{
				this.EventSetFinalTerms = setFinalTerms;
				this.EventDoLocalize = doLocalize;
				this.CanUseSecondaryTerm = UseSecondaryTerm;
				this.AllowMainTermToBeRTL = MainRTL;
				this.AllowSecondTermToBeRTL = SecondRTL;
			}
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x0005A794 File Offset: 0x00058994
		private void FindAndCacheTarget(ref GameObject targetCache, Localize.DelegateSetFinalTerms setFinalTerms, Localize.DelegateDoLocalize doLocalize, bool UseSecondaryTerm, bool MainRTL, bool SecondRTL)
		{
			if (this.mTarget != targetCache && targetCache)
			{
				UnityEngine.Object.Destroy(targetCache);
			}
			if (this.mTarget != null)
			{
				targetCache = (this.mTarget as GameObject);
			}
			else
			{
				Transform transform = base.transform;
				GameObject gameObject;
				targetCache = (gameObject = ((transform.childCount >= 1) ? transform.GetChild(0).gameObject : null));
				this.mTarget = gameObject;
			}
			if (targetCache != null)
			{
				this.EventSetFinalTerms = setFinalTerms;
				this.EventDoLocalize = doLocalize;
				this.CanUseSecondaryTerm = UseSecondaryTerm;
				this.AllowMainTermToBeRTL = MainRTL;
				this.AllowSecondTermToBeRTL = SecondRTL;
			}
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x0005A84C File Offset: 0x00058A4C
		private bool HasTargetCache()
		{
			return this.EventDoLocalize != null;
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x0005A85C File Offset: 0x00058A5C
		public bool GetFinalTerms(out string PrimaryTerm, out string SecondaryTerm)
		{
			if (!this.mTarget && !this.HasTargetCache())
			{
				this.FindTarget();
			}
			if (!string.IsNullOrEmpty(this.mTerm))
			{
				return this.SetFinalTerms(this.mTerm, this.mTermSecondary, out PrimaryTerm, out SecondaryTerm);
			}
			if (!string.IsNullOrEmpty(this.FinalTerm))
			{
				return this.SetFinalTerms(this.FinalTerm, this.FinalSecondaryTerm, out PrimaryTerm, out SecondaryTerm);
			}
			if (this.EventSetFinalTerms != null)
			{
				return this.EventSetFinalTerms(this.mTerm, this.mTermSecondary, out PrimaryTerm, out SecondaryTerm);
			}
			return this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm);
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x0005A90C File Offset: 0x00058B0C
		private bool SetFinalTerms(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			PrimaryTerm = Main;
			SecondaryTerm = Secondary;
			return true;
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0005A918 File Offset: 0x00058B18
		public void SetTerm(string primary, string secondary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.Term = primary;
			}
			if (!string.IsNullOrEmpty(secondary))
			{
				this.SecondaryTerm = secondary;
			}
			this.OnLocalize();
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x0005A950 File Offset: 0x00058B50
		private T GetSecondaryTranslatedObj<T>(ref string MainTranslation, ref string SecondaryTranslation) where T : UnityEngine.Object
		{
			string text;
			this.DeserializeTranslation(MainTranslation, out MainTranslation, out text);
			if (string.IsNullOrEmpty(text))
			{
				text = SecondaryTranslation;
			}
			if (string.IsNullOrEmpty(text))
			{
				return (T)((object)null);
			}
			T translatedObject = this.GetTranslatedObject<T>(text);
			if (translatedObject == null)
			{
				int num = text.LastIndexOfAny("/\\".ToCharArray());
				if (num >= 0)
				{
					text = text.Substring(num + 1);
					translatedObject = this.GetTranslatedObject<T>(text);
				}
			}
			return translatedObject;
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x0005A9CC File Offset: 0x00058BCC
		private T GetTranslatedObject<T>(string Translation) where T : UnityEngine.Object
		{
			return this.FindTranslatedObject<T>(Translation);
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x0005A9E4 File Offset: 0x00058BE4
		private void DeserializeTranslation(string translation, out string value, out string secondary)
		{
			if (!string.IsNullOrEmpty(translation) && translation.Length > 1 && translation[0] == '[')
			{
				int num = translation.IndexOf(']');
				if (num > 0)
				{
					secondary = translation.Substring(1, num - 1);
					value = translation.Substring(num + 1);
					return;
				}
			}
			value = translation;
			secondary = string.Empty;
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x0005AA48 File Offset: 0x00058C48
		public T FindTranslatedObject<T>(string value) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(value))
			{
				return (T)((object)null);
			}
			if (this.TranslatedObjects != null)
			{
				int i = 0;
				int num = this.TranslatedObjects.Length;
				while (i < num)
				{
					if (this.TranslatedObjects[i] as T != null && value == this.TranslatedObjects[i].name)
					{
						return this.TranslatedObjects[i] as T;
					}
					i++;
				}
			}
			T t = LocalizationManager.FindAsset(value) as T;
			if (t)
			{
				return t;
			}
			return ResourceManager.pInstance.GetAsset<T>(value);
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x0005AB0C File Offset: 0x00058D0C
		private bool HasTranslatedObject(UnityEngine.Object Obj)
		{
			return Array.IndexOf<UnityEngine.Object>(this.TranslatedObjects, Obj) >= 0 || ResourceManager.pInstance.HasAsset(Obj);
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x0005AB30 File Offset: 0x00058D30
		public void SetGlobalLanguage(string Language)
		{
			LocalizationManager.CurrentLanguage = Language;
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x0005AB38 File Offset: 0x00058D38
		public static void RegisterEvents_2DToolKit()
		{
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x0005AB3C File Offset: 0x00058D3C
		public static void RegisterEvents_DFGUI()
		{
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x0005AB40 File Offset: 0x00058D40
		public void RegisterEvents_NGUI()
		{
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_UILabel));
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_UISprite));
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_UITexture));
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x0005ABB4 File Offset: 0x00058DB4
		private void FindTarget_UILabel()
		{
			this.FindAndCacheTarget<UILabel>(ref this.mTarget_UILabel, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_UIlabel), new Localize.DelegateDoLocalize(this.DoLocalize_UILabel), true, true, false);
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x0005ABE8 File Offset: 0x00058DE8
		private void FindTarget_UISprite()
		{
			this.FindAndCacheTarget<UISprite>(ref this.mTarget_UISprite, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_UISprite), new Localize.DelegateDoLocalize(this.DoLocalize_UISprite), true, false, false);
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x0005AC1C File Offset: 0x00058E1C
		private void FindTarget_UITexture()
		{
			this.FindAndCacheTarget<UITexture>(ref this.mTarget_UITexture, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_UITexture), new Localize.DelegateDoLocalize(this.DoLocalize_UITexture), false, false, false);
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x0005AC50 File Offset: 0x00058E50
		private bool SetFinalTerms_UIlabel(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string secondary = (!(this.mTarget_UILabel.ambigiousFont != null)) ? string.Empty : this.mTarget_UILabel.ambigiousFont.name;
			return this.SetFinalTerms(this.mTarget_UILabel.text, secondary, out primaryTerm, out secondaryTerm);
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x0005ACA4 File Offset: 0x00058EA4
		public bool SetFinalTerms_UISprite(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string secondary = (!(this.mTarget_UISprite.atlas != null)) ? string.Empty : this.mTarget_UISprite.atlas.name;
			return this.SetFinalTerms(this.mTarget_UISprite.spriteName, secondary, out primaryTerm, out secondaryTerm);
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x0005ACF8 File Offset: 0x00058EF8
		public bool SetFinalTerms_UITexture(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			return this.SetFinalTerms(this.mTarget_UITexture.mainTexture.name, null, out primaryTerm, out secondaryTerm);
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x0005AD20 File Offset: 0x00058F20
		public void DoLocalize_UILabel(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				this.mTarget_UILabel.ambigiousFont = secondaryTranslatedObj;
				return;
			}
			UIFont secondaryTranslatedObj2 = this.GetSecondaryTranslatedObj<UIFont>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj2 != null)
			{
				this.mTarget_UILabel.ambigiousFont = secondaryTranslatedObj2;
				return;
			}
			UIInput uiinput = NGUITools.FindInParents<UIInput>(this.mTarget_UILabel.gameObject);
			if (uiinput != null && uiinput.label == this.mTarget_UILabel)
			{
				if (uiinput.defaultText == MainTranslation)
				{
					return;
				}
				uiinput.defaultText = MainTranslation;
			}
			else
			{
				if (this.mTarget_UILabel.text == MainTranslation)
				{
					return;
				}
				this.mTarget_UILabel.text = MainTranslation;
			}
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x0005ADEC File Offset: 0x00058FEC
		public void DoLocalize_UISprite(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_UISprite.spriteName == MainTranslation)
			{
				return;
			}
			UIAtlas secondaryTranslatedObj = this.GetSecondaryTranslatedObj<UIAtlas>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				this.mTarget_UISprite.atlas = secondaryTranslatedObj;
			}
			this.mTarget_UISprite.spriteName = MainTranslation;
			this.mTarget_UISprite.MakePixelPerfect();
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x0005AE4C File Offset: 0x0005904C
		public void DoLocalize_UITexture(string MainTranslation, string SecondaryTranslation)
		{
			Texture mainTexture = this.mTarget_UITexture.mainTexture;
			if (mainTexture && mainTexture.name == MainTranslation)
			{
				return;
			}
			this.mTarget_UITexture.mainTexture = this.FindTranslatedObject<Texture>(MainTranslation);
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x0005AE94 File Offset: 0x00059094
		public static void RegisterEvents_TextMeshPro()
		{
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x0005AE98 File Offset: 0x00059098
		public void RegisterEvents_UGUI()
		{
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_uGUI_Text));
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_uGUI_Image));
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_uGUI_RawImage));
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x0005AF0C File Offset: 0x0005910C
		private void FindTarget_uGUI_Text()
		{
			this.FindAndCacheTarget<Text>(ref this.mTarget_uGUI_Text, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_Text), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_Text), true, true, false);
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x0005AF40 File Offset: 0x00059140
		private void FindTarget_uGUI_Image()
		{
			this.FindAndCacheTarget<Image>(ref this.mTarget_uGUI_Image, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_Image), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_Image), false, false, false);
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x0005AF74 File Offset: 0x00059174
		private void FindTarget_uGUI_RawImage()
		{
			this.FindAndCacheTarget<RawImage>(ref this.mTarget_uGUI_RawImage, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_RawImage), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_RawImage), false, false, false);
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x0005AFA8 File Offset: 0x000591A8
		private bool SetFinalTerms_uGUI_Text(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string secondary = (!(this.mTarget_uGUI_Text.font != null)) ? string.Empty : this.mTarget_uGUI_Text.font.name;
			return this.SetFinalTerms(this.mTarget_uGUI_Text.text, secondary, out primaryTerm, out secondaryTerm);
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x0005AFFC File Offset: 0x000591FC
		public bool SetFinalTerms_uGUI_Image(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			return this.SetFinalTerms(this.mTarget_uGUI_Image.mainTexture.name, null, out primaryTerm, out secondaryTerm);
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x0005B024 File Offset: 0x00059224
		public bool SetFinalTerms_uGUI_RawImage(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			return this.SetFinalTerms(this.mTarget_uGUI_RawImage.texture.name, null, out primaryTerm, out secondaryTerm);
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x0005B04C File Offset: 0x0005924C
		public static T FindInParents<T>(Transform tr) where T : Component
		{
			if (!tr)
			{
				return (T)((object)null);
			}
			T component = tr.GetComponent<T>();
			while (!component && tr)
			{
				component = tr.GetComponent<T>();
				tr = tr.parent;
			}
			return component;
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x0005B0A4 File Offset: 0x000592A4
		public void DoLocalize_uGUI_Text(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_uGUI_Text.text == MainTranslation)
			{
				return;
			}
			this.mTarget_uGUI_Text.text = MainTranslation;
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				this.mTarget_uGUI_Text.font = secondaryTranslatedObj;
			}
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x0005B0F8 File Offset: 0x000592F8
		public void DoLocalize_uGUI_Image(string MainTranslation, string SecondaryTranslation)
		{
			Sprite sprite = this.mTarget_uGUI_Image.sprite;
			if (sprite && sprite.name == MainTranslation)
			{
				return;
			}
			this.mTarget_uGUI_Image.sprite = this.FindTranslatedObject<Sprite>(MainTranslation);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x0005B140 File Offset: 0x00059340
		public void DoLocalize_uGUI_RawImage(string MainTranslation, string SecondaryTranslation)
		{
			Texture texture = this.mTarget_uGUI_RawImage.texture;
			if (texture && texture.name == MainTranslation)
			{
				return;
			}
			this.mTarget_uGUI_RawImage.texture = this.FindTranslatedObject<Texture>(MainTranslation);
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x0005B188 File Offset: 0x00059388
		public void RegisterEvents_UnityStandard()
		{
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_GUIText));
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_TextMesh));
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_AudioSource));
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_GUITexture));
			this.EventFindTarget = (Action)Delegate.Combine(this.EventFindTarget, new Action(this.FindTarget_Child));
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x0005B240 File Offset: 0x00059440
		private void FindTarget_GUIText()
		{
			this.FindAndCacheTarget<GUIText>(ref this.mTarget_GUIText, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_GUIText), new Localize.DelegateDoLocalize(this.DoLocalize_GUIText), true, true, false);
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x0005B274 File Offset: 0x00059474
		private void FindTarget_TextMesh()
		{
			this.FindAndCacheTarget<TextMesh>(ref this.mTarget_TextMesh, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_TextMesh), new Localize.DelegateDoLocalize(this.DoLocalize_TextMesh), true, true, false);
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x0005B2A8 File Offset: 0x000594A8
		private void FindTarget_AudioSource()
		{
			this.FindAndCacheTarget<AudioSource>(ref this.mTarget_AudioSource, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_AudioSource), new Localize.DelegateDoLocalize(this.DoLocalize_AudioSource), false, false, false);
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x0005B2DC File Offset: 0x000594DC
		private void FindTarget_GUITexture()
		{
			this.FindAndCacheTarget<GUITexture>(ref this.mTarget_GUITexture, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_GUITexture), new Localize.DelegateDoLocalize(this.DoLocalize_GUITexture), false, false, false);
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x0005B310 File Offset: 0x00059510
		private void FindTarget_Child()
		{
			this.FindAndCacheTarget(ref this.mTarget_Child, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_Child), new Localize.DelegateDoLocalize(this.DoLocalize_Child), false, false, false);
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x0005B344 File Offset: 0x00059544
		public bool SetFinalTerms_GUIText(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			string secondary = (!(this.mTarget_GUIText.font != null)) ? string.Empty : this.mTarget_GUIText.font.name;
			return this.SetFinalTerms(this.mTarget_GUIText.text, secondary, out PrimaryTerm, out SecondaryTerm);
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x0005B398 File Offset: 0x00059598
		public bool SetFinalTerms_TextMesh(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			string secondary = (!(this.mTarget_TextMesh.font != null)) ? string.Empty : this.mTarget_TextMesh.font.name;
			return this.SetFinalTerms(this.mTarget_TextMesh.text, secondary, out PrimaryTerm, out SecondaryTerm);
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0005B3EC File Offset: 0x000595EC
		public bool SetFinalTerms_GUITexture(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (!this.mTarget_GUITexture || !this.mTarget_GUITexture.texture)
			{
				this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm);
				return false;
			}
			return this.SetFinalTerms(this.mTarget_GUITexture.texture.name, string.Empty, out PrimaryTerm, out SecondaryTerm);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x0005B454 File Offset: 0x00059654
		public bool SetFinalTerms_AudioSource(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (!this.mTarget_AudioSource || !this.mTarget_AudioSource.clip)
			{
				this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm);
				return false;
			}
			return this.SetFinalTerms(this.mTarget_AudioSource.clip.name, string.Empty, out PrimaryTerm, out SecondaryTerm);
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x0005B4BC File Offset: 0x000596BC
		public bool SetFinalTerms_Child(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			return this.SetFinalTerms(this.mTarget_Child.name, string.Empty, out PrimaryTerm, out SecondaryTerm);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x0005B4D8 File Offset: 0x000596D8
		private void DoLocalize_GUIText(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_GUIText.text == MainTranslation)
			{
				return;
			}
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				this.mTarget_GUIText.font = secondaryTranslatedObj;
			}
			this.mTarget_GUIText.text = MainTranslation;
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x0005B52C File Offset: 0x0005972C
		private void DoLocalize_TextMesh(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_TextMesh.text == MainTranslation)
			{
				return;
			}
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				this.mTarget_TextMesh.font = secondaryTranslatedObj;
				base.GetComponent<Renderer>().sharedMaterial = secondaryTranslatedObj.material;
			}
			this.mTarget_TextMesh.text = MainTranslation;
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x0005B590 File Offset: 0x00059790
		private void DoLocalize_AudioSource(string MainTranslation, string SecondaryTranslation)
		{
			bool isPlaying = this.mTarget_AudioSource.isPlaying;
			AudioClip clip = this.mTarget_AudioSource.clip;
			AudioClip audioClip = this.FindTranslatedObject<AudioClip>(MainTranslation);
			if (clip == audioClip)
			{
				return;
			}
			this.mTarget_AudioSource.clip = audioClip;
			if (isPlaying && this.mTarget_AudioSource.clip)
			{
				this.mTarget_AudioSource.Play();
			}
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x0005B5FC File Offset: 0x000597FC
		private void DoLocalize_GUITexture(string MainTranslation, string SecondaryTranslation)
		{
			Texture texture = this.mTarget_GUITexture.texture;
			if (texture && texture.name == MainTranslation)
			{
				return;
			}
			this.mTarget_GUITexture.texture = this.FindTranslatedObject<Texture>(MainTranslation);
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x0005B644 File Offset: 0x00059844
		private void DoLocalize_Child(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_Child && this.mTarget_Child.name == MainTranslation)
			{
				return;
			}
			GameObject gameObject = this.mTarget_Child;
			GameObject gameObject2 = this.FindTranslatedObject<GameObject>(MainTranslation);
			if (gameObject2)
			{
				this.mTarget_Child = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
				Transform transform = this.mTarget_Child.transform;
				Transform transform2 = (!gameObject) ? gameObject2.transform : gameObject.transform;
				transform.parent = base.transform;
				transform.localScale = transform2.localScale;
				transform.localRotation = transform2.localRotation;
				transform.localPosition = transform2.localPosition;
			}
			if (gameObject)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}

		// Token: 0x04000D0B RID: 3339
		public string mTerm;

		// Token: 0x04000D0C RID: 3340
		public string mTermSecondary;

		// Token: 0x04000D0D RID: 3341
		public string FinalTerm;

		// Token: 0x04000D0E RID: 3342
		public string FinalSecondaryTerm;

		// Token: 0x04000D0F RID: 3343
		public Localize.TermModification PrimaryTermModifier;

		// Token: 0x04000D10 RID: 3344
		public Localize.TermModification SecondaryTermModifier;

		// Token: 0x04000D11 RID: 3345
		public UnityEngine.Object mTarget;

		// Token: 0x04000D12 RID: 3346
		public Localize.DelegateSetFinalTerms EventSetFinalTerms;

		// Token: 0x04000D13 RID: 3347
		public Localize.DelegateDoLocalize EventDoLocalize;

		// Token: 0x04000D14 RID: 3348
		public bool CanUseSecondaryTerm;

		// Token: 0x04000D15 RID: 3349
		public bool AllowMainTermToBeRTL;

		// Token: 0x04000D16 RID: 3350
		public bool AllowSecondTermToBeRTL;

		// Token: 0x04000D17 RID: 3351
		public bool IgnoreRTL;

		// Token: 0x04000D18 RID: 3352
		public UnityEngine.Object[] TranslatedObjects;

		// Token: 0x04000D19 RID: 3353
		public EventCallback LocalizeCallBack = new EventCallback();

		// Token: 0x04000D1A RID: 3354
		public static string MainTranslation;

		// Token: 0x04000D1B RID: 3355
		public static string SecondaryTranslation;

		// Token: 0x04000D1C RID: 3356
		private UILabel mTarget_UILabel;

		// Token: 0x04000D1D RID: 3357
		private UISprite mTarget_UISprite;

		// Token: 0x04000D1E RID: 3358
		private UITexture mTarget_UITexture;

		// Token: 0x04000D1F RID: 3359
		private Text mTarget_uGUI_Text;

		// Token: 0x04000D20 RID: 3360
		private Image mTarget_uGUI_Image;

		// Token: 0x04000D21 RID: 3361
		private RawImage mTarget_uGUI_RawImage;

		// Token: 0x04000D22 RID: 3362
		private GUIText mTarget_GUIText;

		// Token: 0x04000D23 RID: 3363
		private TextMesh mTarget_TextMesh;

		// Token: 0x04000D24 RID: 3364
		private AudioSource mTarget_AudioSource;

		// Token: 0x04000D25 RID: 3365
		private GUITexture mTarget_GUITexture;

		// Token: 0x04000D26 RID: 3366
		private GameObject mTarget_Child;

		// Token: 0x020002C4 RID: 708
		public enum TermModification
		{
			// Token: 0x04000D29 RID: 3369
			DontModify,
			// Token: 0x04000D2A RID: 3370
			ToUpper,
			// Token: 0x04000D2B RID: 3371
			ToLower
		}

		// Token: 0x020008E6 RID: 2278
		// (Invoke) Token: 0x06005038 RID: 20536
		public delegate bool DelegateSetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm);

		// Token: 0x020008E7 RID: 2279
		// (Invoke) Token: 0x0600503C RID: 20540
		public delegate void DelegateDoLocalize(string primaryTerm, string secondaryTerm);
	}
}
