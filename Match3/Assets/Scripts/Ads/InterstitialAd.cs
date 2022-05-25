using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    public class InterstitialAd : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener,
        IUnityAdsShowListener
    {
#if UNITY_ANDROID
        private string androidGameId = "4769807";
#endif
#if UNITY_EDITOR
        private bool testMode = true;
#else
        private bool testMode = false;
#endif
        private string bannerAndroid = "Banner_Android";
        private BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
        private string interstitialAndroid = "Interstitial_Android";
        void Awake()
        {
            InitializeAd();
            InitializeBanner();
        }

        private void InitializeAd() => Advertisement.Initialize(androidGameId, testMode, this);
        private void InitializeBanner() => Advertisement.Banner.SetPosition(_bannerPosition);
        private void LoadAd() => Advertisement.Load(interstitialAndroid, this);
        private void LoadBanner() => Advertisement.Banner.Load(bannerAndroid);
        public void ShowAd() => Advertisement.Show(interstitialAndroid, this);
        public void OnInitializationComplete()
        {
            Debug.Log(nameof(OnInitializationComplete));
            LoadAd();
            LoadBanner();
        }
        public void OnInitializationFailed(UnityAdsInitializationError error, string message) =>
            Debug.Log($"{nameof(OnInitializationFailed)}:{error}");
        public void OnUnityAdsAdLoaded(string placementId) => Debug.Log(nameof(OnUnityAdsAdLoaded));
        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) =>
            Debug.Log($"{nameof(OnUnityAdsFailedToLoad)}:{error}");
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) =>
            Debug.Log($"{nameof(OnUnityAdsShowFailure)}:{error}");
        public void OnUnityAdsShowStart(string placementId) => Debug.Log(nameof(OnUnityAdsShowStart));
        public void OnUnityAdsShowClick(string placementId) => Debug.Log(nameof(OnUnityAdsShowClick));
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) =>
            Debug.Log(nameof(OnUnityAdsShowComplete));
    }
}