using System;
using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Utils;

namespace DecisionKingdom.Monetization
{
    /// <summary>
    /// In-App Purchase Manager - Handles all IAP operations
    /// Supports iOS, Android, and Steam platforms
    /// </summary>
    public class IAPManager : Singleton<IAPManager>
    {
        // Events
        public event Action<IAPProductType> OnPurchaseCompleted;
        public event Action<IAPProductType, string> OnPurchaseFailed;
        public event Action OnPurchasesRestored;

        // Data
        private Dictionary<IAPProductType, PurchaseState> purchaseStates;
        private HashSet<IAPProductType> purchasedProducts;
        private bool isInitialized;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        /// <summary>
        /// Initialize the IAP system
        /// </summary>
        private void Initialize()
        {
            purchaseStates = new Dictionary<IAPProductType, PurchaseState>();
            purchasedProducts = new HashSet<IAPProductType>();

            // Initialize all products as not purchased
            foreach (IAPProductType productType in Enum.GetValues(typeof(IAPProductType)))
            {
                purchaseStates[productType] = PurchaseState.NotPurchased;
            }

            // Load saved purchases
            LoadPurchases();

            // Initialize platform-specific IAP SDK
            InitializePlatformSDK();

            isInitialized = true;
            Debug.Log("[IAPManager] Initialized successfully");
        }

        /// <summary>
        /// Initialize platform-specific IAP SDK
        /// </summary>
        private void InitializePlatformSDK()
        {
            // Unity IAP initialization
            // To enable actual purchases, add Unity IAP package and uncomment the relevant code
            // Package: com.unity.purchasing

#if UNITY_EDITOR
            Debug.Log("[IAPManager] Running in Editor - IAP simulation mode");
            // Editor mode: purchases are simulated for testing
#elif UNITY_IOS
            Debug.Log("[IAPManager] iOS StoreKit - SDK integration pending");
            // For iOS integration:
            // 1. Add Unity IAP package
            // 2. Configure App Store Connect
            // 3. Add product IDs to IAP catalog
            // 4. Implement IStoreListener interface
            // Example: UnityPurchasing.Initialize(this, builder);
#elif UNITY_ANDROID
            Debug.Log("[IAPManager] Google Play Billing - SDK integration pending");
            // For Android integration:
            // 1. Add Unity IAP package
            // 2. Configure Google Play Console
            // 3. Add license key to Unity IAP settings
            // 4. Implement IStoreListener interface
            // Example: UnityPurchasing.Initialize(this, builder);
#elif UNITY_STANDALONE
            Debug.Log("[IAPManager] Steam API - SDK integration pending");
            // For Steam integration:
            // 1. Add Steamworks.NET
            // 2. Configure Steamworks app
            // 3. Implement Steam overlay callbacks
#else
            Debug.LogWarning("[IAPManager] No IAP SDK available for this platform");
#endif
        }

        #region Purchase Operations

        /// <summary>
        /// Initiate a purchase for a product
        /// </summary>
        public void PurchaseProduct(IAPProductType productType)
        {
            if (!isInitialized)
            {
                Debug.LogError("[IAPManager] Not initialized");
                OnPurchaseFailed?.Invoke(productType, "IAP system not initialized");
                return;
            }

            if (IsPurchased(productType))
            {
                Debug.LogWarning($"[IAPManager] Product already purchased: {productType}");
                OnPurchaseFailed?.Invoke(productType, "Product already purchased");
                return;
            }

            purchaseStates[productType] = PurchaseState.Pending;
            Debug.Log($"[IAPManager] Initiating purchase for: {productType}");

            // Get product ID for platform
            string productId = GetProductId(productType);

#if UNITY_EDITOR
            // Simulate successful purchase in editor
            SimulatePurchase(productType);
#else
            // Actual purchase flow
            StartPlatformPurchase(productId, productType);
#endif
        }

        /// <summary>
        /// Simulate purchase for testing in editor
        /// </summary>
        private void SimulatePurchase(IAPProductType productType)
        {
            Debug.Log($"[IAPManager] Simulating purchase: {productType}");

            // Simulate some delay
            StartCoroutine(SimulatePurchaseCoroutine(productType));
        }

        private System.Collections.IEnumerator SimulatePurchaseCoroutine(IAPProductType productType)
        {
            yield return new WaitForSeconds(1f);
            ProcessSuccessfulPurchase(productType);
        }

        /// <summary>
        /// Start platform-specific purchase flow
        /// </summary>
        private void StartPlatformPurchase(string productId, IAPProductType productType)
        {
            // Platform purchase implementation
            // When Unity IAP is integrated, this will initiate the actual purchase

            Debug.Log($"[IAPManager] Starting platform purchase: {productId}");

#if UNITY_EDITOR
            // Editor: simulate purchase (already handled by SimulatePurchase)
            Debug.LogWarning("[IAPManager] Platform purchase called in editor - use SimulatePurchase instead");
#else
            // Production: initiate store purchase
            // With Unity IAP: _storeController.InitiatePurchase(productId);

            // For now, log that SDK integration is needed
            Debug.LogError($"[IAPManager] SDK not integrated - cannot process purchase: {productId}");
            ProcessFailedPurchase(productType, "IAP SDK not integrated. Please add Unity IAP package.");
#endif
        }

        /// <summary>
        /// Process a successful purchase
        /// </summary>
        public void ProcessSuccessfulPurchase(IAPProductType productType)
        {
            purchaseStates[productType] = PurchaseState.Purchased;
            purchasedProducts.Add(productType);

            // Handle bundles
            ProcessBundlePurchase(productType);

            // Save purchases
            SavePurchases();

            Debug.Log($"[IAPManager] Purchase successful: {productType}");
            OnPurchaseCompleted?.Invoke(productType);
        }

        /// <summary>
        /// Process bundle purchases (unlock individual items)
        /// </summary>
        private void ProcessBundlePurchase(IAPProductType productType)
        {
            switch (productType)
            {
                case IAPProductType.EraBundle:
                    // Unlock all eras
                    purchasedProducts.Add(IAPProductType.EraRenaissance);
                    purchasedProducts.Add(IAPProductType.EraIndustrial);
                    purchasedProducts.Add(IAPProductType.EraModern);
                    purchasedProducts.Add(IAPProductType.EraFuture);
                    purchaseStates[IAPProductType.EraRenaissance] = PurchaseState.Purchased;
                    purchaseStates[IAPProductType.EraIndustrial] = PurchaseState.Purchased;
                    purchaseStates[IAPProductType.EraModern] = PurchaseState.Purchased;
                    purchaseStates[IAPProductType.EraFuture] = PurchaseState.Purchased;
                    break;

                case IAPProductType.ScenarioBundle:
                    // Unlock all scenarios
                    purchasedProducts.Add(IAPProductType.ScenarioYoungHeir);
                    purchasedProducts.Add(IAPProductType.ScenarioCoupLeader);
                    purchasedProducts.Add(IAPProductType.ScenarioRichMerchant);
                    purchaseStates[IAPProductType.ScenarioYoungHeir] = PurchaseState.Purchased;
                    purchaseStates[IAPProductType.ScenarioCoupLeader] = PurchaseState.Purchased;
                    purchaseStates[IAPProductType.ScenarioRichMerchant] = PurchaseState.Purchased;
                    break;

                case IAPProductType.CosmeticBundle:
                    // Unlock all cosmetics
                    purchasedProducts.Add(IAPProductType.CosmeticCardBackPack1);
                    purchasedProducts.Add(IAPProductType.CosmeticCardBackPack2);
                    purchasedProducts.Add(IAPProductType.CosmeticThemePack);
                    purchaseStates[IAPProductType.CosmeticCardBackPack1] = PurchaseState.Purchased;
                    purchaseStates[IAPProductType.CosmeticCardBackPack2] = PurchaseState.Purchased;
                    purchaseStates[IAPProductType.CosmeticThemePack] = PurchaseState.Purchased;
                    break;

                case IAPProductType.CompleteBundle:
                    // Unlock everything
                    foreach (IAPProductType type in Enum.GetValues(typeof(IAPProductType)))
                    {
                        purchasedProducts.Add(type);
                        purchaseStates[type] = PurchaseState.Purchased;
                    }
                    break;
            }
        }

        /// <summary>
        /// Process a failed purchase
        /// </summary>
        public void ProcessFailedPurchase(IAPProductType productType, string errorMessage)
        {
            purchaseStates[productType] = PurchaseState.Failed;

            Debug.LogWarning($"[IAPManager] Purchase failed: {productType} - {errorMessage}");
            OnPurchaseFailed?.Invoke(productType, errorMessage);
        }

        /// <summary>
        /// Restore previous purchases
        /// </summary>
        public void RestorePurchases()
        {
            Debug.Log("[IAPManager] Restoring purchases...");

#if UNITY_EDITOR
            // In editor, just reload saved data
            LoadPurchases();
            OnPurchasesRestored?.Invoke();
#elif UNITY_IOS
            // iOS restore purchases
            RestoreiOSPurchases();
#elif UNITY_ANDROID
            // Android doesn't need explicit restore for non-consumables
            LoadPurchases();
            OnPurchasesRestored?.Invoke();
#endif
        }

        private void RestoreiOSPurchases()
        {
            // iOS restore purchases implementation
            // With Unity IAP: _appleExtensions.RestoreTransactions(OnTransactionsRestored);

            Debug.Log("[IAPManager] Restoring iOS purchases");

#if UNITY_IOS && !UNITY_EDITOR
            // Production iOS: restore via App Store
            // When Unity IAP is integrated:
            // var apple = _extensionProvider.GetExtension<IAppleExtensions>();
            // apple.RestoreTransactions(result => {
            //     if (result) OnPurchasesRestored?.Invoke();
            // });

            Debug.LogWarning("[IAPManager] iOS restore - SDK integration pending");

            // For now, reload local purchases as fallback
            LoadPurchases();
            OnPurchasesRestored?.Invoke();
#else
            // Non-iOS or Editor: just reload local data
            LoadPurchases();
            OnPurchasesRestored?.Invoke();
#endif
        }

        #endregion

        #region Query Methods

        /// <summary>
        /// Check if a product is purchased
        /// </summary>
        public bool IsPurchased(IAPProductType productType)
        {
            return purchasedProducts.Contains(productType);
        }

        /// <summary>
        /// Check if an era is unlocked via purchase
        /// </summary>
        public bool IsEraUnlocked(Era era)
        {
            // Medieval is always unlocked
            if (era == Era.Medieval) return true;

            // Check if complete bundle is purchased
            if (IsPurchased(IAPProductType.CompleteBundle)) return true;

            // Check individual era purchases
            switch (era)
            {
                case Era.Renaissance:
                    return IsPurchased(IAPProductType.EraRenaissance) || IsPurchased(IAPProductType.EraBundle);
                case Era.Industrial:
                    return IsPurchased(IAPProductType.EraIndustrial) || IsPurchased(IAPProductType.EraBundle);
                case Era.Modern:
                    return IsPurchased(IAPProductType.EraModern) || IsPurchased(IAPProductType.EraBundle);
                case Era.Future:
                    return IsPurchased(IAPProductType.EraFuture) || IsPurchased(IAPProductType.EraBundle);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check if ad removal is purchased
        /// </summary>
        public bool IsAdFree()
        {
            return IsPurchased(IAPProductType.AdRemoval) || IsPurchased(IAPProductType.CompleteBundle);
        }

        /// <summary>
        /// Get purchase state for a product
        /// </summary>
        public PurchaseState GetPurchaseState(IAPProductType productType)
        {
            return purchaseStates.ContainsKey(productType)
                ? purchaseStates[productType]
                : PurchaseState.NotPurchased;
        }

        /// <summary>
        /// Get platform-specific product ID
        /// </summary>
        public string GetProductId(IAPProductType productType)
        {
            switch (productType)
            {
                case IAPProductType.EraRenaissance:
                    return Constants.IAP_ERA_RENAISSANCE;
                case IAPProductType.EraIndustrial:
                    return Constants.IAP_ERA_INDUSTRIAL;
                case IAPProductType.EraModern:
                    return Constants.IAP_ERA_MODERN;
                case IAPProductType.EraFuture:
                    return Constants.IAP_ERA_FUTURE;
                case IAPProductType.EraBundle:
                    return Constants.IAP_ERA_BUNDLE;
                case IAPProductType.ScenarioYoungHeir:
                    return Constants.IAP_SCENARIO_YOUNG_HEIR;
                case IAPProductType.ScenarioCoupLeader:
                    return Constants.IAP_SCENARIO_COUP_LEADER;
                case IAPProductType.ScenarioRichMerchant:
                    return Constants.IAP_SCENARIO_RICH_MERCHANT;
                case IAPProductType.ScenarioBundle:
                    return Constants.IAP_SCENARIO_BUNDLE;
                case IAPProductType.CosmeticCardBackPack1:
                    return Constants.IAP_COSMETIC_CARDBACK1;
                case IAPProductType.CosmeticCardBackPack2:
                    return Constants.IAP_COSMETIC_CARDBACK2;
                case IAPProductType.CosmeticThemePack:
                    return Constants.IAP_COSMETIC_THEMES;
                case IAPProductType.CosmeticBundle:
                    return Constants.IAP_COSMETIC_BUNDLE;
                case IAPProductType.AdRemoval:
                    return Constants.IAP_AD_REMOVAL;
                case IAPProductType.CompleteBundle:
                    return Constants.IAP_COMPLETE_BUNDLE;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Get price for a product
        /// </summary>
        public float GetPrice(IAPProductType productType)
        {
            switch (productType)
            {
                case IAPProductType.EraRenaissance:
                    return Constants.PRICE_ERA_RENAISSANCE;
                case IAPProductType.EraIndustrial:
                    return Constants.PRICE_ERA_INDUSTRIAL;
                case IAPProductType.EraModern:
                    return Constants.PRICE_ERA_MODERN;
                case IAPProductType.EraFuture:
                    return Constants.PRICE_ERA_FUTURE;
                case IAPProductType.EraBundle:
                    return Constants.PRICE_ERA_BUNDLE;
                case IAPProductType.ScenarioYoungHeir:
                case IAPProductType.ScenarioCoupLeader:
                case IAPProductType.ScenarioRichMerchant:
                    return Constants.PRICE_SCENARIO;
                case IAPProductType.ScenarioBundle:
                    return Constants.PRICE_SCENARIO_BUNDLE;
                case IAPProductType.CosmeticCardBackPack1:
                case IAPProductType.CosmeticCardBackPack2:
                case IAPProductType.CosmeticThemePack:
                    return Constants.PRICE_COSMETIC;
                case IAPProductType.CosmeticBundle:
                    return Constants.PRICE_COSMETIC_BUNDLE;
                case IAPProductType.AdRemoval:
                    return Constants.PRICE_AD_REMOVAL;
                case IAPProductType.CompleteBundle:
                    return Constants.PRICE_COMPLETE_BUNDLE;
                default:
                    return 0f;
            }
        }

        /// <summary>
        /// Get localized product name
        /// </summary>
        public string GetProductName(IAPProductType productType, bool turkish = true)
        {
            int index = (int)productType;
            if (turkish)
            {
                return index < Constants.IAP_PRODUCT_NAMES_TR.Length
                    ? Constants.IAP_PRODUCT_NAMES_TR[index]
                    : productType.ToString();
            }
            else
            {
                return index < Constants.IAP_PRODUCT_NAMES_EN.Length
                    ? Constants.IAP_PRODUCT_NAMES_EN[index]
                    : productType.ToString();
            }
        }

        /// <summary>
        /// Get all purchased products
        /// </summary>
        public List<IAPProductType> GetPurchasedProducts()
        {
            return new List<IAPProductType>(purchasedProducts);
        }

        #endregion

        #region Save/Load

        /// <summary>
        /// Save purchases to persistent storage
        /// </summary>
        private void SavePurchases()
        {
            var data = new PurchaseSaveData
            {
                purchasedProducts = new List<IAPProductType>(purchasedProducts)
            };

            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(Constants.SAVE_KEY_PURCHASES, json);
            PlayerPrefs.Save();

            Debug.Log("[IAPManager] Purchases saved");
        }

        /// <summary>
        /// Load purchases from persistent storage
        /// </summary>
        private void LoadPurchases()
        {
            string json = PlayerPrefs.GetString(Constants.SAVE_KEY_PURCHASES, "");

            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var data = JsonUtility.FromJson<PurchaseSaveData>(json);

                    if (data != null && data.purchasedProducts != null)
                    {
                        purchasedProducts = new HashSet<IAPProductType>(data.purchasedProducts);

                        // Update purchase states
                        foreach (var product in purchasedProducts)
                        {
                            purchaseStates[product] = PurchaseState.Purchased;
                        }
                    }

                    Debug.Log($"[IAPManager] Loaded {purchasedProducts.Count} purchases");
                }
                catch (Exception e)
                {
                    Debug.LogError($"[IAPManager] Failed to load purchases: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Clear all purchases (for testing)
        /// </summary>
        public void ClearAllPurchases()
        {
            purchasedProducts.Clear();

            foreach (IAPProductType productType in Enum.GetValues(typeof(IAPProductType)))
            {
                purchaseStates[productType] = PurchaseState.NotPurchased;
            }

            SavePurchases();
            Debug.Log("[IAPManager] All purchases cleared");
        }

        #endregion

        #region Data Classes

        [Serializable]
        private class PurchaseSaveData
        {
            public List<IAPProductType> purchasedProducts;
        }

        #endregion
    }
}
