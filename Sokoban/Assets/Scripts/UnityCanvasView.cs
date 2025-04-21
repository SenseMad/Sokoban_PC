using UnityEngine;

namespace PSME_View.Canvas
{
  /// <summary>
  /// Отображение "Холст"
  /// </summary>
  public class UnityCanvasView : MonoBehaviour
  {
    [SerializeField] private Camera camera;

    private RenderTexture renderTexture;

    private void Start()
    {
      renderTexture = new RenderTexture(3840, 2160, 24);
    }

    private void OnEnable()
    {
      InputHandler.Instance.AI_Player.UI.Reload.performed += Reload_performed;
    }
    
    private void OnDestroy()
    {
      InputHandler.Instance.AI_Player.UI.Reload.performed -= Reload_performed;
    }

    private void Reload_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
      Rect rect = new(0, 0, renderTexture.width, renderTexture.height);
      Texture2D texture = new(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

      camera.targetTexture = renderTexture;
      camera.Render();

      RenderTexture currentRenderTexture = RenderTexture.active;
      RenderTexture.active = renderTexture;
      texture.ReadPixels(rect, 0, 0);
      texture.Apply();

      byte[] bytes = texture.EncodeToPNG();
      System.IO.File.WriteAllBytes($"{Application.dataPath}/{Random.Range(0, 10000)}.png", bytes);

      camera.targetTexture = null;
      RenderTexture.active = null;
      Destroy(texture);

      /*RenderTexture.active = renderTexture;
      Texture2D texture = new(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
      texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
      texture.Apply();

      byte[] bytes = texture.EncodeToPNG();
      System.IO.File.WriteAllBytes($"{Application.dataPath}/{Random.Range(0, 10000)}.png", bytes);

      RenderTexture.active = null;
      Destroy(texture);*/
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
      if (source.width != renderTexture.width || source.height != renderTexture.height)
      {
        if (renderTexture != null)
          DestroyImmediate(renderTexture);
        renderTexture = new RenderTexture(source.width, source.height, 24);
      }

      Graphics.Blit(source, renderTexture);
      Graphics.Blit(source, destination);
    }

    //==========================================================================
  }
}
