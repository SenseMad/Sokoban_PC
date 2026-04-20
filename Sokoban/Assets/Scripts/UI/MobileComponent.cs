using System.Collections.Generic;
using UnityEngine;
using YG;

public sealed class MobileComponent : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameObjects;

    private void Start()
    {
        foreach (var go in _gameObjects)
        {
            go.SetActive(YG2.envir.isMobile);
        }
    }
}