using UnityEngine;

public class SpikeObject : InteractiveObjects
{
  [SerializeField] private GameObject _normalState;

  [SerializeField] private GameObject _activatedState;

  //--------------------------------------

  private bool isPlayerStandsSpikes;
  
  private readonly float delayTime = 5.0f;

  private float currentDelayTime;

  //======================================

  public bool IsSpikeActivated { get; private set; }

  //======================================

  /*private void Update()
  {
    if (IsSpikeActivated)
      return;

    if (!isPlayerStandsSpikes)
      return;

    currentDelayTime += Time.deltaTime;

    if (currentDelayTime >= delayTime)
    {
      ActivateSpike();
    }
  }*/

  //======================================

  private void OnTriggerEnter(Collider other)
  {
    if (other.GetComponent<Block>().GetTypeObject() != TypeObject.playerObject)
      return;

    if (IsSpikeActivated)
    {
      if (other.TryGetComponent(out PlayerObjects playerObjects))
      {
        playerObjects.OnPlayerDeathEvent?.Invoke();
        return;
      }
    }

    isPlayerStandsSpikes = true;
    currentDelayTime = 0;
  }

  private void OnTriggerStay(Collider other)
  {
    if (other.GetComponent<Block>().GetTypeObject() != TypeObject.playerObject)
      return;

    if (IsSpikeActivated)
    {
      if (other.TryGetComponent(out PlayerObjects playerObjects))
      {
        playerObjects.OnPlayerDeathEvent?.Invoke();
        return;
      }
    }
  }

  private void OnTriggerExit(Collider other)
  {
    if (other.GetComponent<Block>().GetTypeObject() != TypeObject.playerObject)
      return;

    ActivateSpike();
  }

  //======================================

  private void ActivateSpike()
  {
    isPlayerStandsSpikes = false;
    IsSpikeActivated = true;
    _normalState.SetActive(false);
    _activatedState.SetActive(true);
  }

  //======================================
}