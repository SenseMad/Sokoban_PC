using System;
using UnityEngine;
using YG;

namespace YandexManager
{
    public sealed class YandexAdService : MonoBehaviour
    {
        public static YandexAdService Instance { get; private set; }

        [SerializeField] private bool _dontDestroyOnLoad = true;
        [SerializeField] private bool _stopGameplayBeforeAd = true;

        private Action<bool> _onInterstitialClosed;

        public event Action InterstitialOpened;
        public event Action InterstitialClosed;
        public event Action<bool> InterstitialClosedWithResult;
        public event Action InterstitialError;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            transform.SetParent(null);

            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

#if InterstitialAdv_yg
        private void OnEnable()
        {
            YG2.onOpenInterAdv += HandleOpenInterAdv;
            YG2.onCloseInterAdv += HandleCloseInterAdv;
            YG2.onCloseInterAdvWasShow += HandleCloseInterAdvWasShow;
            YG2.onErrorInterAdv += HandleErrorInterAdv;
            YG2.onPauseGame += HandlePauseGame;
        }

        private void OnDisable()
        {
            YG2.onOpenInterAdv -= HandleOpenInterAdv;
            YG2.onCloseInterAdv -= HandleCloseInterAdv;
            YG2.onCloseInterAdvWasShow -= HandleCloseInterAdvWasShow;
            YG2.onErrorInterAdv -= HandleErrorInterAdv;
            YG2.onPauseGame -= HandlePauseGame;
        }

        /*public bool CanShowInterstitial()
        {
            if (!YandexSdkBootstrap.IsInitialized)
                return false;

            return YG2.isTimerAdvCompleted && !YG2.nowAdsShow;
        }*/

        public bool CanShowInterstitial()
        {
            if (!YandexSdkBootstrap.IsInitialized)
                return false;

            return !YG2.nowAdsShow;
        }

        /*public bool CanShowInterstitial()
        {
            if (!YandexSdkBootstrap.IsInitialized)
            {
                Debug.Log("[YG] CanShowInterstitial = false: SDK not initialized");
                return false;
            }

            if (!YG2.isTimerAdvCompleted)
            {
                Debug.Log("[YG] CanShowInterstitial = false: interstitial timer not completed");
                return false;
            }

            if (YG2.nowAdsShow)
            {
                Debug.Log("[YG] CanShowInterstitial = false: another ad is showing now");
                return false;
            }

            Debug.Log("[YG] CanShowInterstitial = true");
            return true;
        }*/

        public void ShowInterstitial(Action<bool> onClosed = null)
        {
            if (!YandexSdkBootstrap.IsInitialized)
            {
                Debug.Log("[YG] Interstitial skipped: SDK is not initialized");
                onClosed?.Invoke(false);
                return;
            }

            if (!CanShowInterstitial())
            {
                Debug.Log("[YG] Interstitial skipped: timer is not completed or ad is already showing");
                onClosed?.Invoke(false);
                return;
            }

            _onInterstitialClosed = onClosed;

            var yandexGameplayService = YandexGameplayService.Instance;
            if (_stopGameplayBeforeAd && yandexGameplayService != null)
                yandexGameplayService.StopGameplay();

            Debug.Log("[YG] InterstitialAdvShow");
            YG2.InterstitialAdvShow();
        }

        private void HandleOpenInterAdv()
        {
            Debug.Log("[YG] Interstitial opened");
            InterstitialOpened?.Invoke();
        }

        private void HandleCloseInterAdv()
        {
            Debug.Log("[YG] Interstitial closed");
            InterstitialClosed?.Invoke();
        }

        private void HandleCloseInterAdvWasShow(bool wasShown)
        {
            Debug.Log($"[YG] Interstitial closed. Was shown = {wasShown}");

            var callback = _onInterstitialClosed;
            _onInterstitialClosed = null;

            InterstitialClosedWithResult?.Invoke(wasShown);
            callback?.Invoke(wasShown);
        }

        private void HandleErrorInterAdv()
        {
            Debug.LogWarning("[YG] Interstitial error");

            var callback = _onInterstitialClosed;
            _onInterstitialClosed = null;

            InterstitialError?.Invoke();
            callback?.Invoke(false);
        }

        private void HandlePauseGame(bool isPaused)
        {
            Debug.Log($"[YG] onPauseGame = {isPaused}");
        }
#else
        public bool CanShowInterstitial() => false;

        public void ShowInterstitial(Action<bool> onClosed = null)
        {
            Debug.LogWarning("[YG] InterstitialAdv module is not imported");
            onClosed?.Invoke(false);
        }
#endif
    }
}