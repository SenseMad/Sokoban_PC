using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionBetweenScenes : MonoBehaviour
{
  [SerializeField] private float _sceneChangeTime = 1.0f;

  [SerializeField] private CanvasGroup canvasGroup;

  public bool IsSceneTransition { get; private set; }

  public void StartSceneChange(string parNameScene)
  {
    StartCoroutine(SceneFading(parNameScene));
  }

  public void StartSceneChange(int parIndexScene)
  {
    StartCoroutine(SceneFading(parIndexScene));
  }

  private IEnumerator SceneFading(string parNameScene)
  {
    IsSceneTransition = true;

    /*while (canvasGroup.alpha < 1)
    {
      canvasGroup.alpha += Time.deltaTime;
      yield return null;
    }*/

    yield return new WaitForSeconds(_sceneChangeTime);

    var asyncOperation = SceneManager.LoadSceneAsync(parNameScene, LoadSceneMode.Single);
    asyncOperation.allowSceneActivation = true;
  }

  private IEnumerator SceneFading(int parIndexScene)
  {
    IsSceneTransition = true;;

    /*while (canvasGroup.alpha < 1)
    {
      canvasGroup.alpha += Time.deltaTime;
      yield return null;
    }*/

    yield return new WaitForSeconds(_sceneChangeTime);

    var asyncOperation = SceneManager.LoadSceneAsync(parIndexScene, LoadSceneMode.Single);
    asyncOperation.allowSceneActivation = true;
  }

  private IEnumerator FadeOutAndLoadNextScene()
  {
    canvasGroup.alpha = 1;

    while (canvasGroup.alpha > 0)
    {
      canvasGroup.alpha -= Time.deltaTime / _sceneChangeTime;
      yield return null;
    }
  }
}