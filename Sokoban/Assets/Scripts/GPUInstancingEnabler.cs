using UnityEngine;

public class GPUInstancingEnabler : MonoBehaviour
{
  private void Awake()
  {
    MaterialPropertyBlock materialPropertyBlock = new();
    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    meshRenderer.SetPropertyBlock(materialPropertyBlock);
  }
}