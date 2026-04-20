using Sokoban.GridEditor;
using UnityEngine;

public sealed class MobileCameraButtons : MonoBehaviour
{
    private PlayerObjects _playerObjects;
    private GridLevel _gridLevel;

    private void Start()
    {
        _gridLevel = FindObjectOfType<GridLevel>();

        if (_gridLevel != null)
            _gridLevel.OnLevelCreated.AddListener(RefreshPlayerReference);

        RefreshPlayerReference();
    }

    private void OnDestroy()
    {
        if (_gridLevel != null)
            _gridLevel.OnLevelCreated.RemoveListener(RefreshPlayerReference);
    }

    private void RefreshPlayerReference()
    {
        _playerObjects = FindObjectOfType<PlayerObjects>();
    }

    public void RotateLeft()
    {
        if (_playerObjects == null)
            RefreshPlayerReference();

        if (_playerObjects != null)
            _playerObjects.RotateCameraLeft();
    }

    public void RotateRight()
    {
        if (_playerObjects == null)
            RefreshPlayerReference();

        if (_playerObjects != null)
            _playerObjects.RotateCameraRight();
    }
}