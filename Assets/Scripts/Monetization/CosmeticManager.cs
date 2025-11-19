using System;
using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Utils;

namespace DecisionKingdom.Monetization
{
    /// <summary>
    /// Cosmetic Manager - Handles card backs, UI themes, and other visual customizations
    /// </summary>
    public class CosmeticManager : Singleton<CosmeticManager>
    {
        // Events
        public event Action<CardBackDesign> OnCardBackChanged;
        public event Action<UITheme> OnThemeChanged;
        public event Action<PortraitStyle> OnPortraitStyleChanged;
        public event Action OnCosmeticsUnlocked;

        // Current selections
        private CardBackDesign currentCardBack;
        private UITheme currentTheme;
        private PortraitStyle currentPortraitStyle;

        // Unlocked items
        private HashSet<CardBackDesign> unlockedCardBacks;
        private HashSet<UITheme> unlockedThemes;
        private HashSet<PortraitStyle> unlockedPortraitStyles;

        // Initialization
        private bool isInitialized;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        /// <summary>
        /// Initialize cosmetic system
        /// </summary>
        private void Initialize()
        {
            unlockedCardBacks = new HashSet<CardBackDesign>();
            unlockedThemes = new HashSet<UITheme>();
            unlockedPortraitStyles = new HashSet<PortraitStyle>();

            // Default items are always unlocked
            unlockedCardBacks.Add(CardBackDesign.Default);
            unlockedThemes.Add(UITheme.Default);
            unlockedPortraitStyles.Add(PortraitStyle.Default);

            // Load saved data
            LoadCosmetics();

            isInitialized = true;
            Debug.Log("[CosmeticManager] Initialized successfully");
        }

        #region Card Backs

        /// <summary>
        /// Set current card back design
        /// </summary>
        public void SetCardBack(CardBackDesign design)
        {
            if (!IsCardBackUnlocked(design))
            {
                Debug.LogWarning($"[CosmeticManager] Card back not unlocked: {design}");
                return;
            }

            currentCardBack = design;
            SaveCosmetics();

            Debug.Log($"[CosmeticManager] Card back changed to: {design}");
            OnCardBackChanged?.Invoke(design);
        }

        /// <summary>
        /// Get current card back design
        /// </summary>
        public CardBackDesign GetCurrentCardBack()
        {
            return currentCardBack;
        }

        /// <summary>
        /// Check if a card back is unlocked
        /// </summary>
        public bool IsCardBackUnlocked(CardBackDesign design)
        {
            // Default is always unlocked
            if (design == CardBackDesign.Default)
            {
                return true;
            }

            // Check if unlocked via purchase
            return unlockedCardBacks.Contains(design);
        }

        /// <summary>
        /// Unlock a card back design
        /// </summary>
        public void UnlockCardBack(CardBackDesign design)
        {
            if (unlockedCardBacks.Add(design))
            {
                SaveCosmetics();
                Debug.Log($"[CosmeticManager] Card back unlocked: {design}");
                OnCosmeticsUnlocked?.Invoke();
            }
        }

        /// <summary>
        /// Unlock card backs from a pack
        /// </summary>
        public void UnlockCardBackPack(int packNumber)
        {
            List<CardBackDesign> designs = GetCardBackPackContents(packNumber);
            foreach (var design in designs)
            {
                unlockedCardBacks.Add(design);
            }
            SaveCosmetics();
            Debug.Log($"[CosmeticManager] Card back pack {packNumber} unlocked");
            OnCosmeticsUnlocked?.Invoke();
        }

        /// <summary>
        /// Get card backs in a pack
        /// </summary>
        private List<CardBackDesign> GetCardBackPackContents(int packNumber)
        {
            var designs = new List<CardBackDesign>();

            switch (packNumber)
            {
                case 1:
                    designs.Add(CardBackDesign.Royal);
                    designs.Add(CardBackDesign.Dragon);
                    designs.Add(CardBackDesign.Celtic);
                    designs.Add(CardBackDesign.Gothic);
                    designs.Add(CardBackDesign.Minimalist);
                    break;
                case 2:
                    designs.Add(CardBackDesign.Golden);
                    designs.Add(CardBackDesign.Dark);
                    designs.Add(CardBackDesign.Floral);
                    designs.Add(CardBackDesign.Geometric);
                    break;
            }

            return designs;
        }

        /// <summary>
        /// Get all unlocked card backs
        /// </summary>
        public List<CardBackDesign> GetUnlockedCardBacks()
        {
            return new List<CardBackDesign>(unlockedCardBacks);
        }

        /// <summary>
        /// Get all card backs (locked and unlocked)
        /// </summary>
        public List<CardBackDesign> GetAllCardBacks()
        {
            var designs = new List<CardBackDesign>();
            foreach (CardBackDesign design in Enum.GetValues(typeof(CardBackDesign)))
            {
                designs.Add(design);
            }
            return designs;
        }

        /// <summary>
        /// Get localized card back name
        /// </summary>
        public string GetCardBackName(CardBackDesign design, bool turkish = true)
        {
            int index = (int)design;
            if (turkish && index < Constants.CARD_BACK_NAMES_TR.Length)
            {
                return Constants.CARD_BACK_NAMES_TR[index];
            }
            return design.ToString();
        }

        #endregion

        #region UI Themes

        /// <summary>
        /// Set current UI theme
        /// </summary>
        public void SetTheme(UITheme theme)
        {
            if (!IsThemeUnlocked(theme))
            {
                Debug.LogWarning($"[CosmeticManager] Theme not unlocked: {theme}");
                return;
            }

            currentTheme = theme;
            SaveCosmetics();

            Debug.Log($"[CosmeticManager] Theme changed to: {theme}");
            OnThemeChanged?.Invoke(theme);
        }

        /// <summary>
        /// Get current UI theme
        /// </summary>
        public UITheme GetCurrentTheme()
        {
            return currentTheme;
        }

        /// <summary>
        /// Check if a theme is unlocked
        /// </summary>
        public bool IsThemeUnlocked(UITheme theme)
        {
            // Default is always unlocked
            if (theme == UITheme.Default)
            {
                return true;
            }

            // Check if unlocked via purchase
            return unlockedThemes.Contains(theme);
        }

        /// <summary>
        /// Unlock a UI theme
        /// </summary>
        public void UnlockTheme(UITheme theme)
        {
            if (unlockedThemes.Add(theme))
            {
                SaveCosmetics();
                Debug.Log($"[CosmeticManager] Theme unlocked: {theme}");
                OnCosmeticsUnlocked?.Invoke();
            }
        }

        /// <summary>
        /// Unlock all themes from theme pack
        /// </summary>
        public void UnlockThemePack()
        {
            foreach (UITheme theme in Enum.GetValues(typeof(UITheme)))
            {
                unlockedThemes.Add(theme);
            }
            SaveCosmetics();
            Debug.Log("[CosmeticManager] Theme pack unlocked");
            OnCosmeticsUnlocked?.Invoke();
        }

        /// <summary>
        /// Get all unlocked themes
        /// </summary>
        public List<UITheme> GetUnlockedThemes()
        {
            return new List<UITheme>(unlockedThemes);
        }

        /// <summary>
        /// Get all themes (locked and unlocked)
        /// </summary>
        public List<UITheme> GetAllThemes()
        {
            var themes = new List<UITheme>();
            foreach (UITheme theme in Enum.GetValues(typeof(UITheme)))
            {
                themes.Add(theme);
            }
            return themes;
        }

        /// <summary>
        /// Get localized theme name
        /// </summary>
        public string GetThemeName(UITheme theme, bool turkish = true)
        {
            int index = (int)theme;
            if (turkish && index < Constants.UI_THEME_NAMES_TR.Length)
            {
                return Constants.UI_THEME_NAMES_TR[index];
            }
            return theme.ToString();
        }

        #endregion

        #region Portrait Styles

        /// <summary>
        /// Set current portrait style
        /// </summary>
        public void SetPortraitStyle(PortraitStyle style)
        {
            if (!IsPortraitStyleUnlocked(style))
            {
                Debug.LogWarning($"[CosmeticManager] Portrait style not unlocked: {style}");
                return;
            }

            currentPortraitStyle = style;
            SaveCosmetics();

            Debug.Log($"[CosmeticManager] Portrait style changed to: {style}");
            OnPortraitStyleChanged?.Invoke(style);
        }

        /// <summary>
        /// Get current portrait style
        /// </summary>
        public PortraitStyle GetCurrentPortraitStyle()
        {
            return currentPortraitStyle;
        }

        /// <summary>
        /// Check if a portrait style is unlocked
        /// </summary>
        public bool IsPortraitStyleUnlocked(PortraitStyle style)
        {
            // Default is always unlocked
            if (style == PortraitStyle.Default)
            {
                return true;
            }

            // Check if unlocked via purchase
            return unlockedPortraitStyles.Contains(style);
        }

        /// <summary>
        /// Unlock a portrait style
        /// </summary>
        public void UnlockPortraitStyle(PortraitStyle style)
        {
            if (unlockedPortraitStyles.Add(style))
            {
                SaveCosmetics();
                Debug.Log($"[CosmeticManager] Portrait style unlocked: {style}");
                OnCosmeticsUnlocked?.Invoke();
            }
        }

        /// <summary>
        /// Unlock portrait styles from a pack
        /// </summary>
        public void UnlockPortraitPack(int packNumber)
        {
            List<PortraitStyle> styles = GetPortraitPackContents(packNumber);
            foreach (var style in styles)
            {
                unlockedPortraitStyles.Add(style);
            }
            SaveCosmetics();
            Debug.Log($"[CosmeticManager] Portrait pack {packNumber} unlocked");
            OnCosmeticsUnlocked?.Invoke();
        }

        /// <summary>
        /// Get portrait styles in a pack
        /// </summary>
        private List<PortraitStyle> GetPortraitPackContents(int packNumber)
        {
            var styles = new List<PortraitStyle>();

            switch (packNumber)
            {
                case 1: // Classic Pack - Historical illustrations
                    styles.Add(PortraitStyle.Classic);
                    styles.Add(PortraitStyle.Medieval);
                    styles.Add(PortraitStyle.Renaissance);
                    break;
                case 2: // Modern Pack - Digital art styles
                    styles.Add(PortraitStyle.Anime);
                    styles.Add(PortraitStyle.Realistic);
                    styles.Add(PortraitStyle.Cartoon);
                    break;
                case 3: // Special Pack - Unique styles
                    styles.Add(PortraitStyle.Pixel);
                    styles.Add(PortraitStyle.Watercolor);
                    styles.Add(PortraitStyle.Noir);
                    break;
            }

            return styles;
        }

        /// <summary>
        /// Unlock all portrait styles
        /// </summary>
        public void UnlockAllPortraitStyles()
        {
            foreach (PortraitStyle style in Enum.GetValues(typeof(PortraitStyle)))
            {
                unlockedPortraitStyles.Add(style);
            }
            SaveCosmetics();
            Debug.Log("[CosmeticManager] All portrait styles unlocked");
            OnCosmeticsUnlocked?.Invoke();
        }

        /// <summary>
        /// Get all unlocked portrait styles
        /// </summary>
        public List<PortraitStyle> GetUnlockedPortraitStyles()
        {
            return new List<PortraitStyle>(unlockedPortraitStyles);
        }

        /// <summary>
        /// Get all portrait styles (locked and unlocked)
        /// </summary>
        public List<PortraitStyle> GetAllPortraitStyles()
        {
            var styles = new List<PortraitStyle>();
            foreach (PortraitStyle style in Enum.GetValues(typeof(PortraitStyle)))
            {
                styles.Add(style);
            }
            return styles;
        }

        /// <summary>
        /// Get localized portrait style name
        /// </summary>
        public string GetPortraitStyleName(PortraitStyle style, bool turkish = true)
        {
            if (turkish)
            {
                return style switch
                {
                    PortraitStyle.Default => "Varsayılan",
                    PortraitStyle.Classic => "Klasik",
                    PortraitStyle.Medieval => "Ortaçağ",
                    PortraitStyle.Renaissance => "Rönesans",
                    PortraitStyle.Anime => "Anime",
                    PortraitStyle.Realistic => "Gerçekçi",
                    PortraitStyle.Cartoon => "Karikatür",
                    PortraitStyle.Pixel => "Piksel",
                    PortraitStyle.Watercolor => "Suluboya",
                    PortraitStyle.Noir => "Noir",
                    _ => style.ToString()
                };
            }
            return style.ToString();
        }

        /// <summary>
        /// Get portrait style description
        /// </summary>
        public string GetPortraitStyleDescription(PortraitStyle style, bool turkish = true)
        {
            if (turkish)
            {
                return style switch
                {
                    PortraitStyle.Default => "Standart karakter portreleri",
                    PortraitStyle.Classic => "Tarihi yağlı boya tarzı portreler",
                    PortraitStyle.Medieval => "Ortaçağ el yazması illustrasyonları",
                    PortraitStyle.Renaissance => "Rönesans dönemi sanat stili",
                    PortraitStyle.Anime => "Japon anime çizim stili",
                    PortraitStyle.Realistic => "Fotogerçekçi dijital sanat",
                    PortraitStyle.Cartoon => "Renkli karikatür stili",
                    PortraitStyle.Pixel => "Retro pixel art stili",
                    PortraitStyle.Watercolor => "Suluboya resim stili",
                    PortraitStyle.Noir => "Siyah-beyaz film noir stili",
                    _ => ""
                };
            }
            return "";
        }

        /// <summary>
        /// Get total unlocked portrait styles
        /// </summary>
        public int GetTotalUnlockedPortraitStyles()
        {
            return unlockedPortraitStyles.Count;
        }

        #endregion

        #region Theme Colors

        /// <summary>
        /// Get primary color for current theme
        /// </summary>
        public Color GetPrimaryColor()
        {
            return GetThemeColors(currentTheme).primary;
        }

        /// <summary>
        /// Get secondary color for current theme
        /// </summary>
        public Color GetSecondaryColor()
        {
            return GetThemeColors(currentTheme).secondary;
        }

        /// <summary>
        /// Get background color for current theme
        /// </summary>
        public Color GetBackgroundColor()
        {
            return GetThemeColors(currentTheme).background;
        }

        /// <summary>
        /// Get text color for current theme
        /// </summary>
        public Color GetTextColor()
        {
            return GetThemeColors(currentTheme).text;
        }

        /// <summary>
        /// Get accent color for current theme
        /// </summary>
        public Color GetAccentColor()
        {
            return GetThemeColors(currentTheme).accent;
        }

        /// <summary>
        /// Get all colors for a theme
        /// </summary>
        private ThemeColors GetThemeColors(UITheme theme)
        {
            switch (theme)
            {
                case UITheme.Dark:
                    return new ThemeColors
                    {
                        primary = new Color(0.15f, 0.15f, 0.15f),
                        secondary = new Color(0.25f, 0.25f, 0.25f),
                        background = new Color(0.1f, 0.1f, 0.1f),
                        text = Color.white,
                        accent = new Color(0.4f, 0.6f, 1f)
                    };

                case UITheme.Minimalist:
                    return new ThemeColors
                    {
                        primary = Color.white,
                        secondary = new Color(0.95f, 0.95f, 0.95f),
                        background = Color.white,
                        text = Color.black,
                        accent = new Color(0f, 0f, 0f)
                    };

                case UITheme.Royal:
                    return new ThemeColors
                    {
                        primary = new Color(0.3f, 0.1f, 0.4f),
                        secondary = new Color(0.8f, 0.6f, 0.2f),
                        background = new Color(0.2f, 0.05f, 0.3f),
                        text = Color.white,
                        accent = new Color(1f, 0.85f, 0.4f)
                    };

                case UITheme.Nature:
                    return new ThemeColors
                    {
                        primary = new Color(0.2f, 0.5f, 0.3f),
                        secondary = new Color(0.6f, 0.8f, 0.6f),
                        background = new Color(0.15f, 0.35f, 0.2f),
                        text = Color.white,
                        accent = new Color(0.4f, 0.8f, 0.4f)
                    };

                case UITheme.Steampunk:
                    return new ThemeColors
                    {
                        primary = new Color(0.4f, 0.25f, 0.15f),
                        secondary = new Color(0.7f, 0.5f, 0.3f),
                        background = new Color(0.25f, 0.15f, 0.1f),
                        text = new Color(1f, 0.9f, 0.7f),
                        accent = new Color(0.8f, 0.6f, 0.2f)
                    };

                case UITheme.Futuristic:
                    return new ThemeColors
                    {
                        primary = new Color(0.1f, 0.1f, 0.2f),
                        secondary = new Color(0f, 0.8f, 1f),
                        background = new Color(0.05f, 0.05f, 0.15f),
                        text = Color.white,
                        accent = new Color(0f, 1f, 1f)
                    };

                default: // Default theme
                    return new ThemeColors
                    {
                        primary = new Color(0.6f, 0.4f, 0.2f),
                        secondary = new Color(0.8f, 0.7f, 0.5f),
                        background = new Color(0.95f, 0.9f, 0.8f),
                        text = new Color(0.2f, 0.15f, 0.1f),
                        accent = new Color(0.8f, 0.2f, 0.2f)
                    };
            }
        }

        #endregion

        #region Purchase Integration

        /// <summary>
        /// Process cosmetic purchase from IAP
        /// </summary>
        public void ProcessCosmeticPurchase(IAPProductType productType)
        {
            switch (productType)
            {
                case IAPProductType.CosmeticCardBackPack1:
                    UnlockCardBackPack(1);
                    break;

                case IAPProductType.CosmeticCardBackPack2:
                    UnlockCardBackPack(2);
                    break;

                case IAPProductType.CosmeticThemePack:
                    UnlockThemePack();
                    break;

                case IAPProductType.CosmeticBundle:
                case IAPProductType.CompleteBundle:
                    UnlockCardBackPack(1);
                    UnlockCardBackPack(2);
                    UnlockThemePack();
                    break;
            }
        }

        #endregion

        #region Save/Load

        /// <summary>
        /// Save cosmetic data
        /// </summary>
        private void SaveCosmetics()
        {
            var data = new CosmeticSaveData
            {
                currentCardBack = currentCardBack,
                currentTheme = currentTheme,
                currentPortraitStyle = currentPortraitStyle,
                unlockedCardBacks = new List<CardBackDesign>(unlockedCardBacks),
                unlockedThemes = new List<UITheme>(unlockedThemes),
                unlockedPortraitStyles = new List<PortraitStyle>(unlockedPortraitStyles)
            };

            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(Constants.SAVE_KEY_COSMETICS, json);
            PlayerPrefs.Save();

            Debug.Log("[CosmeticManager] Cosmetics saved");
        }

        /// <summary>
        /// Load cosmetic data
        /// </summary>
        private void LoadCosmetics()
        {
            string json = PlayerPrefs.GetString(Constants.SAVE_KEY_COSMETICS, "");

            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var data = JsonUtility.FromJson<CosmeticSaveData>(json);

                    if (data != null)
                    {
                        currentCardBack = data.currentCardBack;
                        currentTheme = data.currentTheme;
                        currentPortraitStyle = data.currentPortraitStyle;

                        if (data.unlockedCardBacks != null)
                        {
                            unlockedCardBacks = new HashSet<CardBackDesign>(data.unlockedCardBacks);
                        }

                        if (data.unlockedThemes != null)
                        {
                            unlockedThemes = new HashSet<UITheme>(data.unlockedThemes);
                        }

                        if (data.unlockedPortraitStyles != null)
                        {
                            unlockedPortraitStyles = new HashSet<PortraitStyle>(data.unlockedPortraitStyles);
                        }

                        // Ensure defaults are always available
                        unlockedCardBacks.Add(CardBackDesign.Default);
                        unlockedThemes.Add(UITheme.Default);
                        unlockedPortraitStyles.Add(PortraitStyle.Default);
                    }

                    Debug.Log("[CosmeticManager] Cosmetics loaded");
                }
                catch (Exception e)
                {
                    Debug.LogError($"[CosmeticManager] Failed to load cosmetics: {e.Message}");
                    currentCardBack = CardBackDesign.Default;
                    currentTheme = UITheme.Default;
                    currentPortraitStyle = PortraitStyle.Default;
                }
            }
            else
            {
                currentCardBack = CardBackDesign.Default;
                currentTheme = UITheme.Default;
                currentPortraitStyle = PortraitStyle.Default;
            }
        }

        /// <summary>
        /// Reset all cosmetics to default
        /// </summary>
        public void ResetToDefaults()
        {
            currentCardBack = CardBackDesign.Default;
            currentTheme = UITheme.Default;
            currentPortraitStyle = PortraitStyle.Default;
            SaveCosmetics();

            OnCardBackChanged?.Invoke(currentCardBack);
            OnThemeChanged?.Invoke(currentTheme);
            OnPortraitStyleChanged?.Invoke(currentPortraitStyle);

            Debug.Log("[CosmeticManager] Reset to defaults");
        }

        #endregion

        #region Statistics

        /// <summary>
        /// Get total unlocked card backs
        /// </summary>
        public int GetTotalUnlockedCardBacks()
        {
            return unlockedCardBacks.Count;
        }

        /// <summary>
        /// Get total unlocked themes
        /// </summary>
        public int GetTotalUnlockedThemes()
        {
            return unlockedThemes.Count;
        }

        /// <summary>
        /// Get cosmetic unlock percentage
        /// </summary>
        public float GetUnlockPercentage()
        {
            int totalItems = Enum.GetValues(typeof(CardBackDesign)).Length +
                            Enum.GetValues(typeof(UITheme)).Length +
                            Enum.GetValues(typeof(PortraitStyle)).Length;
            int unlockedItems = unlockedCardBacks.Count + unlockedThemes.Count + unlockedPortraitStyles.Count;

            return (float)unlockedItems / totalItems * 100f;
        }

        #endregion

        #region Data Classes

        private struct ThemeColors
        {
            public Color primary;
            public Color secondary;
            public Color background;
            public Color text;
            public Color accent;
        }

        [Serializable]
        private class CosmeticSaveData
        {
            public CardBackDesign currentCardBack;
            public UITheme currentTheme;
            public PortraitStyle currentPortraitStyle;
            public List<CardBackDesign> unlockedCardBacks;
            public List<UITheme> unlockedThemes;
            public List<PortraitStyle> unlockedPortraitStyles;
        }

        #endregion
    }
}
