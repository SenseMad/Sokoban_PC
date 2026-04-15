using UnityEngine;
using YG;

namespace YandexManager
{
    public sealed class YandexGameplayService : MonoBehaviour
    {
        public static YandexGameplayService Instance { get; private set;  }

        [SerializeField] private bool _dontDestroyOnLoad = true;

        private bool _resumeGameplayAfterPause;

        public bool IsGameplayActive { get; private set; }

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

        public void SetGameplayActive(bool active)
        {
            if (active)
                StartGameplay();
            else
                StopGameplay();
        }

        public void StartGameplay()
        {
            if (IsGameplayActive)
                return;

            if (!YandexSdkBootstrap.IsInitialized)
            {
                Debug.Log("[YG] GameplayStart skipped: SDK is not initialized yet");
                return;
            }

            IsGameplayActive = true;
            YG2.GameplayStart();

            Debug.Log("[YG] Gameplay started");
        }

        public void StopGameplay()
        {
            if (!IsGameplayActive)
                return;

            if (!YandexSdkBootstrap.IsInitialized)
                return;

            IsGameplayActive = false;
            YG2.GameplayStop();

            Debug.Log("[YG] Gameplay stopped");
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!YandexSdkBootstrap.IsInitialized)
                return;

            if (pauseStatus)
            {
                _resumeGameplayAfterPause = IsGameplayActive;

                if (IsGameplayActive)
                    StopGameplay();

                return;
            }

            if (_resumeGameplayAfterPause)
            {
                _resumeGameplayAfterPause = false;
                StartGameplay();
            }
        }

        /*private void OnApplicationPause(bool pauseStatus)
        {
            if (!YandexSdkBootstrap.IsInitialized)
                return;

            if (pauseStatus)
                StopGameplay();
            else if (!IsGameplayActive)
                StartGameplay();
        }*/
    }
}