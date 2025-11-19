using System;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Utils;

namespace DecisionKingdom.Monetization
{
    /// <summary>
    /// Main Monetization Manager - Coordinates all monetization systems
    /// Integrates IAP, Ads, and Cosmetics with game systems
    /// </summary>
    public class MonetizationManager : Singleton<MonetizationManager>
    {
        // Events
        public event Action<PremiumStatus> OnPremiumStatusChanged;
        public event Action OnReviveGranted;
        public event Action<ResourceType, int> OnResourceBoostGranted;
        public event Action<int> OnPPMultiplierGranted;

        // Components
        private IAPManager iapManager;
        private AdManager adManager;
        private CosmeticManager cosmeticManager;

        // State
        private PremiumStatus premiumStatus;
        private bool isInitialized;
        private float pendingPPMultiplier = 1f;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        /// <summary>
        /// Initialize monetization system
        /// </summary>
        private void Initialize()
        {
            // Get or create sub-managers
            iapManager = IAPManager.Instance;
            adManager = AdManager.Instance;
            cosmeticManager = CosmeticManager.Instance;

            // Subscribe to events
            SubscribeToEvents();

            // Load premium status
            LoadPremiumStatus();

            isInitialized = true;
            Debug.Log("[MonetizationManager] Initialized successfully");
        }

        /// <summary>
        /// Subscribe to sub-manager events
        /// </summary>
        private void SubscribeToEvents()
        {
            // IAP events
            if (iapManager != null)
            {
                iapManager.OnPurchaseCompleted += HandlePurchaseCompleted;
                iapManager.OnPurchaseFailed += HandlePurchaseFailed;
                iapManager.OnPurchasesRestored += HandlePurchasesRestored;
            }

            // Ad events
            if (adManager != null)
            {
                adManager.OnAdCompleted += HandleAdCompleted;
                adManager.OnAdFailed += HandleAdFailed;
            }
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (iapManager != null)
            {
                iapManager.OnPurchaseCompleted -= HandlePurchaseCompleted;
                iapManager.OnPurchaseFailed -= HandlePurchaseFailed;
                iapManager.OnPurchasesRestored -= HandlePurchasesRestored;
            }

            if (adManager != null)
            {
                adManager.OnAdCompleted -= HandleAdCompleted;
                adManager.OnAdFailed -= HandleAdFailed;
            }
        }

        #region Purchase Handling

        /// <summary>
        /// Handle completed purchase
        /// </summary>
        private void HandlePurchaseCompleted(IAPProductType productType)
        {
            Debug.Log($"[MonetizationManager] Processing purchase: {productType}");

            // Update premium status if needed
            if (productType == IAPProductType.AdRemoval ||
                productType == IAPProductType.CompleteBundle)
            {
                UpdatePremiumStatus();
            }

            // Process cosmetic purchases
            if (IsCosmeticProduct(productType))
            {
                cosmeticManager?.ProcessCosmeticPurchase(productType);
            }

            // Process era/scenario unlocks
            ProcessContentUnlock(productType);
        }

        /// <summary>
        /// Handle failed purchase
        /// </summary>
        private void HandlePurchaseFailed(IAPProductType productType, string error)
        {
            Debug.LogWarning($"[MonetizationManager] Purchase failed: {productType} - {error}");
            // UI notification would be handled here
        }

        /// <summary>
        /// Handle restored purchases
        /// </summary>
        private void HandlePurchasesRestored()
        {
            Debug.Log("[MonetizationManager] Purchases restored");
            UpdatePremiumStatus();

            // Re-process cosmetic unlocks
            foreach (var product in iapManager.GetPurchasedProducts())
            {
                if (IsCosmeticProduct(product))
                {
                    cosmeticManager?.ProcessCosmeticPurchase(product);
                }
            }
        }

        /// <summary>
        /// Check if product is cosmetic
        /// </summary>
        private bool IsCosmeticProduct(IAPProductType productType)
        {
            return productType == IAPProductType.CosmeticCardBackPack1 ||
                   productType == IAPProductType.CosmeticCardBackPack2 ||
                   productType == IAPProductType.CosmeticThemePack ||
                   productType == IAPProductType.CosmeticBundle ||
                   productType == IAPProductType.CompleteBundle;
        }

        /// <summary>
        /// Process content unlock from purchase
        /// </summary>
        private void ProcessContentUnlock(IAPProductType productType)
        {
            // Era and scenario unlocks are handled by PrestigeManager
            // This just logs and triggers any necessary updates

            switch (productType)
            {
                case IAPProductType.EraRenaissance:
                case IAPProductType.EraIndustrial:
                case IAPProductType.EraModern:
                case IAPProductType.EraFuture:
                case IAPProductType.EraBundle:
                    Debug.Log($"[MonetizationManager] Era unlocked via purchase: {productType}");
                    break;

                case IAPProductType.ScenarioYoungHeir:
                case IAPProductType.ScenarioCoupLeader:
                case IAPProductType.ScenarioRichMerchant:
                case IAPProductType.ScenarioBundle:
                    Debug.Log($"[MonetizationManager] Scenario unlocked via purchase: {productType}");
                    break;
            }
        }

        #endregion

        #region Ad Handling

        /// <summary>
        /// Handle completed ad
        /// </summary>
        private void HandleAdCompleted(AdType adType)
        {
            Debug.Log($"[MonetizationManager] Ad completed: {adType}");

            switch (adType)
            {
                case AdType.RewardedRevive:
                    GrantRevive();
                    break;

                case AdType.RewardedResourceBoost:
                    GrantResourceBoost();
                    break;

                case AdType.RewardedDoublePP:
                    GrantPPMultiplier();
                    break;
            }
        }

        /// <summary>
        /// Handle failed ad
        /// </summary>
        private void HandleAdFailed(AdType adType, string error)
        {
            Debug.LogWarning($"[MonetizationManager] Ad failed: {adType} - {error}");
        }

        /// <summary>
        /// Grant revive reward
        /// </summary>
        private void GrantRevive()
        {
            Debug.Log("[MonetizationManager] Granting revive");
            OnReviveGranted?.Invoke();

            // The actual revive logic should be implemented in GameManager
            // This event notifies the game to perform the revive
        }

        /// <summary>
        /// Grant resource boost reward
        /// </summary>
        private void GrantResourceBoost()
        {
            // Boost lowest resource
            ResourceType lowestResource = GetLowestResource();
            int boostAmount = Constants.RESOURCE_BOOST_AMOUNT;

            Debug.Log($"[MonetizationManager] Granting resource boost: {lowestResource} +{boostAmount}");
            OnResourceBoostGranted?.Invoke(lowestResource, boostAmount);
        }

        /// <summary>
        /// Grant PP multiplier reward
        /// </summary>
        private void GrantPPMultiplier()
        {
            pendingPPMultiplier = Constants.PP_MULTIPLIER_AD;
            Debug.Log($"[MonetizationManager] PP multiplier set to: {pendingPPMultiplier}x");
            OnPPMultiplierGranted?.Invoke((int)(pendingPPMultiplier * 100));
        }

        /// <summary>
        /// Get the lowest resource (for boost targeting)
        /// </summary>
        private ResourceType GetLowestResource()
        {
            // This would be integrated with ResourceManager
            // For now, return a default
            return ResourceType.Gold;
        }

        #endregion

        #region Premium Status

        /// <summary>
        /// Update premium status based on purchases
        /// </summary>
        private void UpdatePremiumStatus()
        {
            PremiumStatus newStatus = PremiumStatus.Free;

            if (iapManager != null)
            {
                if (iapManager.IsPurchased(IAPProductType.CompleteBundle))
                {
                    newStatus = PremiumStatus.Premium;
                }
                else if (iapManager.IsPurchased(IAPProductType.AdRemoval))
                {
                    newStatus = PremiumStatus.AdFree;
                }
            }

            if (premiumStatus != newStatus)
            {
                premiumStatus = newStatus;
                SavePremiumStatus();
                OnPremiumStatusChanged?.Invoke(premiumStatus);
                Debug.Log($"[MonetizationManager] Premium status updated to: {premiumStatus}");
            }
        }

        /// <summary>
        /// Get current premium status
        /// </summary>
        public PremiumStatus GetPremiumStatus()
        {
            return premiumStatus;
        }

        /// <summary>
        /// Check if user is ad-free
        /// </summary>
        public bool IsAdFree()
        {
            return premiumStatus == PremiumStatus.AdFree ||
                   premiumStatus == PremiumStatus.Premium;
        }

        /// <summary>
        /// Check if user has premium
        /// </summary>
        public bool IsPremium()
        {
            return premiumStatus == PremiumStatus.Premium;
        }

        #endregion

        #region Public API

        /// <summary>
        /// Purchase a product
        /// </summary>
        public void Purchase(IAPProductType productType)
        {
            iapManager?.PurchaseProduct(productType);
        }

        /// <summary>
        /// Restore purchases
        /// </summary>
        public void RestorePurchases()
        {
            iapManager?.RestorePurchases();
        }

        /// <summary>
        /// Check if a product is purchased
        /// </summary>
        public bool IsPurchased(IAPProductType productType)
        {
            return iapManager?.IsPurchased(productType) ?? false;
        }

        /// <summary>
        /// Show rewarded ad for revive
        /// </summary>
        public void ShowReviveAd()
        {
            adManager?.ShowRewardedAdForRevive();
        }

        /// <summary>
        /// Show rewarded ad for resource boost
        /// </summary>
        public void ShowResourceBoostAd()
        {
            adManager?.ShowRewardedAdForResourceBoost();
        }

        /// <summary>
        /// Show rewarded ad for double PP
        /// </summary>
        public void ShowDoublePPAd()
        {
            adManager?.ShowRewardedAdForDoublePP();
        }

        /// <summary>
        /// Check if revive ad is available
        /// </summary>
        public bool CanShowReviveAd()
        {
            return adManager?.CanRevive() ?? false;
        }

        /// <summary>
        /// Check if rewarded ad is available
        /// </summary>
        public bool CanShowRewardedAd()
        {
            return adManager?.IsRewardedAdAvailable() ?? false;
        }

        /// <summary>
        /// Notify game over for ad tracking
        /// </summary>
        public void NotifyGameOver()
        {
            adManager?.OnGameOver();
        }

        /// <summary>
        /// Reset session tracking
        /// </summary>
        public void ResetSession()
        {
            adManager?.ResetSession();
            pendingPPMultiplier = 1f;
        }

        /// <summary>
        /// Get and consume PP multiplier
        /// </summary>
        public float GetAndConsumePPMultiplier()
        {
            float multiplier = pendingPPMultiplier;
            pendingPPMultiplier = 1f;
            return multiplier;
        }

        /// <summary>
        /// Check if era is unlocked (via PP or purchase)
        /// </summary>
        public bool IsEraUnlocked(Era era)
        {
            // Check purchase unlock
            if (iapManager?.IsEraUnlocked(era) ?? false)
            {
                return true;
            }

            // PP-based unlocks should be checked in PrestigeManager
            return false;
        }

        /// <summary>
        /// Get price for a product
        /// </summary>
        public float GetPrice(IAPProductType productType)
        {
            return iapManager?.GetPrice(productType) ?? 0f;
        }

        /// <summary>
        /// Get formatted price string
        /// </summary>
        public string GetPriceString(IAPProductType productType)
        {
            float price = GetPrice(productType);
            return $"${price:F2}";
        }

        /// <summary>
        /// Set card back
        /// </summary>
        public void SetCardBack(CardBackDesign design)
        {
            cosmeticManager?.SetCardBack(design);
        }

        /// <summary>
        /// Get current card back
        /// </summary>
        public CardBackDesign GetCurrentCardBack()
        {
            return cosmeticManager?.GetCurrentCardBack() ?? CardBackDesign.Default;
        }

        /// <summary>
        /// Set UI theme
        /// </summary>
        public void SetTheme(UITheme theme)
        {
            cosmeticManager?.SetTheme(theme);
        }

        /// <summary>
        /// Get current UI theme
        /// </summary>
        public UITheme GetCurrentTheme()
        {
            return cosmeticManager?.GetCurrentTheme() ?? UITheme.Default;
        }

        #endregion

        #region Store UI Data

        /// <summary>
        /// Get all available products for store UI
        /// </summary>
        public StoreProduct[] GetAllStoreProducts()
        {
            var products = new StoreProduct[(int)IAPProductType.CompleteBundle + 1];

            foreach (IAPProductType productType in Enum.GetValues(typeof(IAPProductType)))
            {
                int index = (int)productType;
                products[index] = new StoreProduct
                {
                    type = productType,
                    name = iapManager?.GetProductName(productType) ?? productType.ToString(),
                    price = iapManager?.GetPrice(productType) ?? 0f,
                    isPurchased = iapManager?.IsPurchased(productType) ?? false
                };
            }

            return products;
        }

        /// <summary>
        /// Get monetization statistics
        /// </summary>
        public MonetizationStats GetStats()
        {
            return new MonetizationStats
            {
                premiumStatus = premiumStatus,
                totalPurchases = iapManager?.GetPurchasedProducts().Count ?? 0,
                gameOverCount = adManager?.GetGameOverCount() ?? 0,
                remainingRewardedAds = adManager?.GetRemainingRewardedAdsToday() ?? 0,
                cosmeticUnlockPercentage = cosmeticManager?.GetUnlockPercentage() ?? 0f
            };
        }

        #endregion

        #region Save/Load

        /// <summary>
        /// Save premium status
        /// </summary>
        private void SavePremiumStatus()
        {
            PlayerPrefs.SetInt(Constants.SAVE_KEY_PREMIUM, (int)premiumStatus);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Load premium status
        /// </summary>
        private void LoadPremiumStatus()
        {
            premiumStatus = (PremiumStatus)PlayerPrefs.GetInt(Constants.SAVE_KEY_PREMIUM, 0);

            // Verify against actual purchases
            UpdatePremiumStatus();
        }

        #endregion

        #region Data Classes

        /// <summary>
        /// Store product data for UI
        /// </summary>
        [Serializable]
        public struct StoreProduct
        {
            public IAPProductType type;
            public string name;
            public float price;
            public bool isPurchased;
        }

        /// <summary>
        /// Monetization statistics
        /// </summary>
        [Serializable]
        public struct MonetizationStats
        {
            public PremiumStatus premiumStatus;
            public int totalPurchases;
            public int gameOverCount;
            public int remainingRewardedAds;
            public float cosmeticUnlockPercentage;
        }

        #endregion
    }
}
