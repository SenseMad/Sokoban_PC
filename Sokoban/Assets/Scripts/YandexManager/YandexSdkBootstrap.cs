using UnityEngine;
using YG;

namespace YandexManager
{
    public sealed class YandexSdkBootstrap : MonoBehaviour
    {
        private static YandexSdkBootstrap _instance;

        public static bool IsInitialized { get; private set; }

        [SerializeField] private bool _dontDestroyOnLoad = true;
        [SerializeField] private bool _callGameReadyManually = false;
        [SerializeField] private bool _startGameplayOnSdkReady = false;

        private bool _sdkReadyHandled;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            transform.SetParent(null);

            if (_dontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            YG2.onGetSDKData += OnSdkReady;
        }

        private void OnDisable()
        {
            YG2.onGetSDKData -= OnSdkReady;
        }

        private void Start()
        {
            if (YG2.isSDKEnabled)
                OnSdkReady();
        }

        private void OnSdkReady()
        {
            if (_sdkReadyHandled)
                return;

            _sdkReadyHandled = true;
            IsInitialized = true;

            Debug.Log($"[YG] SDK initialized. Platform = {YG2.platform}");

            // ¬ключать это только если в PluginYG выключен AutoGRA.
            if (_callGameReadyManually)
            {
                YG2.GameReadyAPI();
                Debug.Log("[YG] GameReadyAPI called manually");
            }

            var yandexGameplayService = YandexGameplayService.Instance;
            if (yandexGameplayService == null)
                return;

            if (_startGameplayOnSdkReady)
                yandexGameplayService.StartGameplay();
        }
    }
}