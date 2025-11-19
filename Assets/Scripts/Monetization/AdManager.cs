using System;
using System.Collections;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Utils;

namespace DecisionKingdom.Monetization
{
    /// <summary>
    /// Ad Manager - Handles all advertisement operations
    /// Supports AdMob, Unity Ads, and other ad networks
    /// </summary>
    public class AdManager : Singleton<AdManager>
    {
        // Events
        public event Action<AdType> OnAdLoaded;
        public event Action<AdType> OnAdStarted;
        public event Action<AdType> OnAdCompleted;
        public event Action<AdType> OnAdSkipped;
        public event Action<AdType, string> OnAdFailed;

        // State
        private AdWatchState interstitialState;
        private AdWatchState rewardedState;
        private bool isInitialized;

        // Tracking
        private int gameOverCount;
        private int rewardedAdsWatchedToday;
        private int revivesUsedThisSession;
        private DateTime lastAdShownTime;
        private DateTime lastRewardedAdDate;

        // Pending rewards
        private AdType pendingRewardType;
        private bool hasPendingReward;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        /// <summary>
        /// Initialize the ad system
        /// </summary>
        private void Initialize()
        {
            interstitialState = AdWatchState.NotAvailable;
            rewardedState = AdWatchState.NotAvailable;
            lastAdShownTime = DateTime.MinValue;

            // Load saved state
            LoadAdState();

            // Initialize platform-specific ad SDK
            InitializePlatformSDK();

            isInitialized = true;
            Debug.Log("[AdManager] Initialized successfully");
        }

        /// <summary>
        /// Initialize platform-specific ad SDK
        /// </summary>
        private void InitializePlatformSDK()
        {
            // Ad SDK initialization
            // Supports: AdMob, Unity Ads, IronSource, AppLovin MAX
            // To enable ads, add the ad SDK package and configure app IDs

#if UNITY_EDITOR
            Debug.Log("[AdManager] Running in Editor - Ad simulation mode");
            interstitialState = AdWatchState.Available;
            rewardedState = AdWatchState.Available;
#elif UNITY_IOS || UNITY_ANDROID
            Debug.Log("[AdManager] Initializing mobile ads - SDK integration pending");

            // For AdMob integration:
            // 1. Add Google Mobile Ads Unity plugin
            // 2. Configure App ID in GoogleMobileAdsSettings
            // 3. Update ad unit IDs in Constants.cs
            // Example: MobileAds.Initialize(initStatus => {
            //     Debug.Log("[AdManager] AdMob initialized");
            //     LoadInterstitial();
            //     LoadRewarded();
            // });

            // For Unity Ads integration:
            // 1. Enable Unity Ads in Services
            // 2. Configure game ID
            // Example: Advertisement.Initialize(gameId, testMode, this);

            // For now, set ads as available for testing
            interstitialState = AdWatchState.Available;
            rewardedState = AdWatchState.Available;

            LoadInterstitial();
            LoadRewarded();
#else
            Debug.LogWarning("[AdManager] No ad SDK available for this platform");
            interstitialState = AdWatchState.NotAvailable;
            rewardedState = AdWatchState.NotAvailable;
#endif
        }

        #region Ad Loading

        /// <summary>
        /// Load interstitial ad
        /// </summary>
        public void LoadInterstitial()
        {
            if (IAPManager.Instance != null && IAPManager.Instance.IsAdFree())
            {
                Debug.Log("[AdManager] Ad-free user, skipping interstitial load");
                return;
            }

            interstitialState = AdWatchState.Loading;
            Debug.Log("[AdManager] Loading interstitial ad...");

#if UNITY_EDITOR
            // Simulate loading
            StartCoroutine(SimulateAdLoad(AdType.Interstitial));
#else
            // Actual ad loading
            // InterstitialAd.Load(adUnitId, request, callback);
#endif
        }

        /// <summary>
        /// Load rewarded ad
        /// </summary>
        public void LoadRewarded()
        {
            rewardedState = AdWatchState.Loading;
            Debug.Log("[AdManager] Loading rewarded ad...");

#if UNITY_EDITOR
            // Simulate loading
            StartCoroutine(SimulateAdLoad(AdType.RewardedRevive));
#else
            // Actual ad loading
            // RewardedAd.Load(adUnitId, request, callback);
#endif
        }

        private IEnumerator SimulateAdLoad(AdType adType)
        {
            yield return new WaitForSeconds(0.5f);

            if (adType == AdType.Interstitial)
            {
                interstitialState = AdWatchState.Available;
            }
            else
            {
                rewardedState = AdWatchState.Available;
            }

            OnAdLoaded?.Invoke(adType);
            Debug.Log($"[AdManager] Ad loaded: {adType}");
        }

        #endregion

        #region Interstitial Ads

        /// <summary>
        /// Called when a game over occurs - determines if ad should show
        /// </summary>
        public void OnGameOver()
        {
            gameOverCount++;
            SaveAdState();

            // Check if should show interstitial
            if (ShouldShowInterstitial())
            {
                ShowInterstitial();
            }
        }

        /// <summary>
        /// Check if interstitial should be shown
        /// </summary>
        private bool ShouldShowInterstitial()
        {
            // Don't show if ad-free
            if (IAPManager.Instance != null && IAPManager.Instance.IsAdFree())
            {
                return false;
            }

            // Check frequency
            if (gameOverCount % Constants.AD_INTERSTITIAL_FREQUENCY != 0)
            {
                return false;
            }

            // Check cooldown
            if ((DateTime.Now - lastAdShownTime).TotalSeconds < Constants.AD_COOLDOWN_SECONDS)
            {
                return false;
            }

            // Check if ad is available
            if (interstitialState != AdWatchState.Available)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Show interstitial ad
        /// </summary>
        public void ShowInterstitial()
        {
            if (interstitialState != AdWatchState.Available)
            {
                Debug.LogWarning("[AdManager] Interstitial not available");
                return;
            }

            interstitialState = AdWatchState.Showing;
            lastAdShownTime = DateTime.Now;

            Debug.Log("[AdManager] Showing interstitial ad");
            OnAdStarted?.Invoke(AdType.Interstitial);

#if UNITY_EDITOR
            // Simulate showing
            StartCoroutine(SimulateAdShow(AdType.Interstitial));
#else
            // Show actual ad
            // interstitialAd.Show();
#endif
        }

        #endregion

        #region Rewarded Ads

        /// <summary>
        /// Check if rewarded ad is available
        /// </summary>
        public bool IsRewardedAdAvailable(AdType rewardType = AdType.RewardedRevive)
        {
            // Check if ad is loaded
            if (rewardedState != AdWatchState.Available)
            {
                return false;
            }

            // Check daily limit
            if (rewardedAdsWatchedToday >= Constants.REWARDED_AD_DAILY_LIMIT)
            {
                return false;
            }

            // Check revive session limit
            if (rewardType == AdType.RewardedRevive &&
                revivesUsedThisSession >= Constants.REVIVE_PER_SESSION_LIMIT)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Show rewarded ad for revive
        /// </summary>
        public void ShowRewardedAdForRevive()
        {
            if (!IsRewardedAdAvailable(AdType.RewardedRevive))
            {
                Debug.LogWarning("[AdManager] Rewarded ad for revive not available");
                OnAdFailed?.Invoke(AdType.RewardedRevive, "Ad not available");
                return;
            }

            ShowRewardedAd(AdType.RewardedRevive);
        }

        /// <summary>
        /// Show rewarded ad for resource boost
        /// </summary>
        public void ShowRewardedAdForResourceBoost()
        {
            if (!IsRewardedAdAvailable(AdType.RewardedResourceBoost))
            {
                Debug.LogWarning("[AdManager] Rewarded ad for resource boost not available");
                OnAdFailed?.Invoke(AdType.RewardedResourceBoost, "Ad not available");
                return;
            }

            ShowRewardedAd(AdType.RewardedResourceBoost);
        }

        /// <summary>
        /// Show rewarded ad for double PP
        /// </summary>
        public void ShowRewardedAdForDoublePP()
        {
            if (!IsRewardedAdAvailable(AdType.RewardedDoublePP))
            {
                Debug.LogWarning("[AdManager] Rewarded ad for double PP not available");
                OnAdFailed?.Invoke(AdType.RewardedDoublePP, "Ad not available");
                return;
            }

            ShowRewardedAd(AdType.RewardedDoublePP);
        }

        /// <summary>
        /// Show rewarded ad
        /// </summary>
        private void ShowRewardedAd(AdType rewardType)
        {
            rewardedState = AdWatchState.Showing;
            pendingRewardType = rewardType;
            hasPendingReward = true;

            Debug.Log($"[AdManager] Showing rewarded ad for: {rewardType}");
            OnAdStarted?.Invoke(rewardType);

#if UNITY_EDITOR
            // Simulate showing
            StartCoroutine(SimulateAdShow(rewardType));
#else
            // Show actual ad
            // rewardedAd.Show(callback);
#endif
        }

        /// <summary>
        /// Get remaining rewarded ads today
        /// </summary>
        public int GetRemainingRewardedAdsToday()
        {
            return Mathf.Max(0, Constants.REWARDED_AD_DAILY_LIMIT - rewardedAdsWatchedToday);
        }

        /// <summary>
        /// Check if revive is available this session
        /// </summary>
        public bool CanRevive()
        {
            return revivesUsedThisSession < Constants.REVIVE_PER_SESSION_LIMIT &&
                   IsRewardedAdAvailable(AdType.RewardedRevive);
        }

        #endregion

        #region Ad Callbacks

        private IEnumerator SimulateAdShow(AdType adType)
        {
            yield return new WaitForSeconds(2f);

            // Simulate successful completion
            OnAdWatchCompleted(adType);
        }

        /// <summary>
        /// Called when ad watch is completed
        /// </summary>
        public void OnAdWatchCompleted(AdType adType)
        {
            Debug.Log($"[AdManager] Ad completed: {adType}");

            if (adType == AdType.Interstitial)
            {
                interstitialState = AdWatchState.Completed;
                OnAdCompleted?.Invoke(adType);

                // Load next interstitial
                LoadInterstitial();
            }
            else
            {
                rewardedState = AdWatchState.Completed;
                rewardedAdsWatchedToday++;

                // Grant reward
                if (hasPendingReward)
                {
                    GrantReward(pendingRewardType);
                    hasPendingReward = false;
                }

                OnAdCompleted?.Invoke(adType);

                // Load next rewarded
                LoadRewarded();
            }

            SaveAdState();
        }

        /// <summary>
        /// Called when ad is skipped
        /// </summary>
        public void OnAdWatchSkipped(AdType adType)
        {
            Debug.Log($"[AdManager] Ad skipped: {adType}");

            if (adType == AdType.Interstitial)
            {
                interstitialState = AdWatchState.Available;
            }
            else
            {
                rewardedState = AdWatchState.Available;
                hasPendingReward = false;
            }

            OnAdSkipped?.Invoke(adType);
        }

        /// <summary>
        /// Called when ad fails
        /// </summary>
        public void OnAdWatchFailed(AdType adType, string error)
        {
            Debug.LogWarning($"[AdManager] Ad failed: {adType} - {error}");

            if (adType == AdType.Interstitial)
            {
                interstitialState = AdWatchState.Failed;
            }
            else
            {
                rewardedState = AdWatchState.Failed;
                hasPendingReward = false;
            }

            OnAdFailed?.Invoke(adType, error);
        }

        /// <summary>
        /// Grant reward based on ad type
        /// </summary>
        private void GrantReward(AdType rewardType)
        {
            Debug.Log($"[AdManager] Granting reward for: {rewardType}");

            switch (rewardType)
            {
                case AdType.RewardedRevive:
                    revivesUsedThisSession++;
                    // Revive will be handled by MonetizationManager
                    break;

                case AdType.RewardedResourceBoost:
                    // Resource boost will be handled by MonetizationManager
                    break;

                case AdType.RewardedDoublePP:
                    // Double PP will be handled by MonetizationManager
                    break;
            }
        }

        #endregion

        #region Session Management

        /// <summary>
        /// Reset session-specific tracking (call at game start)
        /// </summary>
        public void ResetSession()
        {
            revivesUsedThisSession = 0;
            Debug.Log("[AdManager] Session reset");
        }

        /// <summary>
        /// Reset daily tracking (call at day change)
        /// </summary>
        public void ResetDaily()
        {
            if (lastRewardedAdDate.Date != DateTime.Now.Date)
            {
                rewardedAdsWatchedToday = 0;
                lastRewardedAdDate = DateTime.Now;
                SaveAdState();
                Debug.Log("[AdManager] Daily ad counters reset");
            }
        }

        #endregion

        #region Save/Load

        /// <summary>
        /// Save ad state to persistent storage
        /// </summary>
        private void SaveAdState()
        {
            var data = new AdSaveData
            {
                gameOverCount = gameOverCount,
                rewardedAdsWatchedToday = rewardedAdsWatchedToday,
                lastRewardedAdDate = lastRewardedAdDate.ToBinary()
            };

            string json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(Constants.SAVE_KEY_AD_STATE, json);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Load ad state from persistent storage
        /// </summary>
        private void LoadAdState()
        {
            string json = PlayerPrefs.GetString(Constants.SAVE_KEY_AD_STATE, "");

            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var data = JsonUtility.FromJson<AdSaveData>(json);

                    if (data != null)
                    {
                        gameOverCount = data.gameOverCount;
                        rewardedAdsWatchedToday = data.rewardedAdsWatchedToday;
                        lastRewardedAdDate = DateTime.FromBinary(data.lastRewardedAdDate);

                        // Reset daily counters if needed
                        if (lastRewardedAdDate.Date != DateTime.Now.Date)
                        {
                            rewardedAdsWatchedToday = 0;
                            lastRewardedAdDate = DateTime.Now;
                        }
                    }

                    Debug.Log("[AdManager] Ad state loaded");
                }
                catch (Exception e)
                {
                    Debug.LogError($"[AdManager] Failed to load ad state: {e.Message}");
                }
            }
        }

        #endregion

        #region Query Methods

        /// <summary>
        /// Get current interstitial ad state
        /// </summary>
        public AdWatchState GetInterstitialState()
        {
            return interstitialState;
        }

        /// <summary>
        /// Get current rewarded ad state
        /// </summary>
        public AdWatchState GetRewardedState()
        {
            return rewardedState;
        }

        /// <summary>
        /// Get game over count
        /// </summary>
        public int GetGameOverCount()
        {
            return gameOverCount;
        }

        #endregion

        #region Data Classes

        [Serializable]
        private class AdSaveData
        {
            public int gameOverCount;
            public int rewardedAdsWatchedToday;
            public long lastRewardedAdDate;
        }

        #endregion
    }
}
