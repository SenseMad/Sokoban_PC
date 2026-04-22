using Sokoban.GameManagement;
using Sokoban.LevelManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Sokoban.UI
{
    public class UIShop : MenuUI
    {
        [SerializeField] private RectTransform _content;

        [SerializeField] private ShopButton _prefabShopButton;

        [SerializeField] private Panel _panel;

        [SerializeField] private GameObject _arrowLeft;
        [SerializeField] private GameObject _arrowRight;

        [SerializeField] private TextMeshProUGUI _foodCollected;

        [Header("Mobile Button")]
        [SerializeField] private Button _backButton;

        //--------------------------------------

        private GameManager gameManager;

        private List<ShopButton> listShopButtons = new();

        //======================================

        protected override void Awake()
        {
            base.Awake();

            gameManager = GameManager.Instance;
        }

        protected override void OnEnable()
        {
            OnAmountFoodCollected(gameManager.ProgressData.AmountFoodCollected);

            indexActiveButton = gameManager.ProgressData.CurrentActiveIndexSkin;

            DisplayButtonsUI();

            if (YG2.envir.isMobile)
            {
                if (_backButton != null)
                {
                    _backButton.gameObject.SetActive(true);
                    _backButton.onClick.AddListener(CloseMenu);
                }
            }

            base.OnEnable();

            gameManager.ProgressData.OnAmountFoodCollected += OnAmountFoodCollected;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (YG2.envir.isMobile)
            {
                if (_backButton != null)
                {
                    _backButton.gameObject.SetActive(false);
                    _backButton.onClick.RemoveListener(CloseMenu);
                }
            }

            ClearButtonsUI();

            gameManager.ProgressData.OnAmountFoodCollected -= OnAmountFoodCollected;
        }

        protected override void Update()
        {
            MoveMenuHorizontally();
        }

        //======================================

        private void SetActiveButton(int newIndex, bool playSound = true)
        {
            if (_listButtons.Count == 0)
                return;

            newIndex = Mathf.Clamp(newIndex, 0, _listButtons.Count - 1);

            if (indexActiveButton == newIndex)
                return;

            IsSelectedButton = false;

            indexActiveButton = newIndex;
            LocationElements();
            UpdateArrows();

            if (playSound)
                Sound();

            IsSelectedButton = true;
        }

        private void UpdateArrows()
        {
            if (_arrowLeft != null)
                _arrowLeft.SetActive(indexActiveButton > 0);

            if (_arrowRight != null)
                _arrowRight.SetActive(indexActiveButton < _listButtons.Count - 1);
        }

        public void Next()
        {
            if (Time.time <= nextTimeMoveNextValue)
                return;

            nextTimeMoveNextValue = Time.time + timeMoveNextValue;

            if (indexActiveButton + 1 > _listButtons.Count - 1)
                return;

            SetActiveButton(indexActiveButton + 1);
        }

        public void Back()
        {
            if (Time.time <= nextTimeMoveNextValue)
                return;

            nextTimeMoveNextValue = Time.time + timeMoveNextValue;

            if (indexActiveButton - 1 < 0)
                return;

            SetActiveButton(indexActiveButton - 1);
        }        

        private void DisplayButtonsUI()
        {
            if (_listButtons.Count != 0)
                return;

            foreach (var skinData in ShopData.Instance.SkinDatas)
            {
                ShopButton shopButton = Instantiate(_prefabShopButton, _content);

                if (gameManager.ProgressData.PurchasedSkins.Contains(skinData.IndexSkin))
                {
                    shopButton.UnSelect();

                    if (gameManager.ProgressData.CurrentActiveIndexSkin == skinData.IndexSkin)
                        shopButton.Select();
                }
                else
                {
                    shopButton.NotPurchased();
                }

                shopButton.Initialize(skinData);

                int buttonIndex = _listButtons.Count;
                shopButton.Button.onClick.AddListener(() => OnShopButtonClicked(buttonIndex, skinData, shopButton));

                _listButtons.Add(shopButton.Button);
                listShopButtons.Add(shopButton);
            }

            UpdateArrows();
            LocationElements();
        }

        public void ClearButtonsUI()
        {
            for (int i = 0; i < _listButtons.Count; i++)
            {
                Destroy(_listButtons[i].gameObject);
            }

            _listButtons = new();
            listShopButtons = new();
        }

        //======================================

        private void OnShopButtonClicked(int buttonIndex, SkinData skinData, ShopButton shopButton)
        {
            if (indexActiveButton != buttonIndex)
            {
                SetActiveButton(buttonIndex);
                return;
            }

            SelectSkin(skinData, shopButton);
        }

        private void SelectSkin(SkinData parSkinData, ShopButton parShopButton)
        {
            if (gameManager.ProgressData.PurchasedSkins.Contains(parSkinData.IndexSkin))
            {
                foreach (var oldShopButton in listShopButtons)
                {
                    if (oldShopButton.IndexSkin != gameManager.ProgressData.CurrentActiveIndexSkin)
                        continue;

                    oldShopButton.UnSelect();

                    gameManager.ProgressData.CurrentActiveIndexSkin = parSkinData.IndexSkin;

                    parShopButton.Select();
                    break;
                }

                LevelManager.Instance.SkinReplace();

                gameManager.SaveData();
                return;
            }

            BuySkin(parSkinData, parShopButton);
        }

        private void BuySkin(SkinData parSkinData, ShopButton parShopButton)
        {
            if (!TryAmountFood(parSkinData.PriceSkin))
            {
                Debug.LogWarning($"[Недостаточно денег] Чтобы купить скин, необходимо {parSkinData.PriceSkin}, а у вас {gameManager.ProgressData.AmountFoodCollected}");
                return;
            }

            gameManager.ProgressData.PurchasedSkins.Add(parSkinData.IndexSkin);
            gameManager.Achievements.UpdateAchivementBuySkin();

            parShopButton.UnSelect();

            gameManager.SaveData();
        }

        private bool TryAmountFood(int parValue)
        {
            if (gameManager.ProgressData.AmountFoodCollected < parValue)
                return false;

            gameManager.ProgressData.AmountFoodCollected -= parValue;
            return true;
        }

        //======================================

        private void LocationElements()
        {
            for (int i = 0; i < _listButtons.Count; i++)
            {
                float scale = (i == indexActiveButton) ? 1.5f : 0.7f;

                _listButtons[i].transform.localScale = new Vector3(scale, scale, 1f);

                float xPosition = (i - indexActiveButton) * 400;

                _listButtons[i].transform.localPosition = new Vector3(xPosition, 0f, 0f);
            }
        }

        //======================================

        protected override void MoveMenuHorizontally()
        {
            if (_listButtons.Count == 0)
                return;

            if (Time.time > nextTimeMoveNextValue)
            {
                nextTimeMoveNextValue = Time.time + timeMoveNextValue;

                if (inputHandler.GetChangingValuesInput() > 0)
                {
                    if (indexActiveButton + 1 <= _listButtons.Count - 1)
                        SetActiveButton(indexActiveButton + 1);
                }

                if (inputHandler.GetChangingValuesInput() < 0)
                {
                    if (indexActiveButton - 1 >= 0)
                        SetActiveButton(indexActiveButton - 1);
                }
            }

            if (inputHandler.GetChangingValuesInput() == 0)
            {
                nextTimeMoveNextValue = Time.time;
            }
        }

        //======================================

        private void OnAmountFoodCollected(int parValue)
        {
            _foodCollected.text = $"{parValue}";
        }

        //======================================
    }
}