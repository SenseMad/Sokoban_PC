using UnityEngine;

namespace Sokoban.UI
{
  public class AuthorsMenu : MenuUI
  {
    [Header("Настройки")]
    [SerializeField] private float _scrollSpeed = 5;

    [SerializeField] private RectTransform _content;

    [SerializeField, Tooltip("True, если титры запущены")]
    private bool _isCreditsRunning = false;

    //--------------------------------------

    private Vector2 startPos;
    private RectTransform parent;

    //======================================

    private void Start()
    {
      startPos = _content.anchoredPosition;

      parent = _content.parent as RectTransform;
    }

    protected override void OnEnable()
    {
      indexActiveButton = 0;

      base.OnEnable();

      _isCreditsRunning = true;
    }

    protected override void OnDisable()
    {
      base.OnDisable();

      _isCreditsRunning = false;
      _content.anchoredPosition = startPos;
    }

    protected override void Update()
    {
      base.Update();

      if (!_isCreditsRunning)
        return;

      float targetY = parent.rect.max.y + _content.rect.size.y;
      _content.localPosition = Vector3.MoveTowards(_content.localPosition, Vector3.up * targetY, _scrollSpeed * Time.deltaTime);

      if (_content.localPosition.y == targetY)
      {
        CloseMenuNoSound();
      }
    }

    //======================================
  }
}