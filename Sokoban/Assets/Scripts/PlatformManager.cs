using UnityEngine;

public class PlatformManager
{
  /*public IMain Main { get; private set; }

  public ILocalUserProfiles LocalUserProfiles { get; private set; }

  public IAchievements Achievements { get; private set; }

  public ISaveLoad SaveLoad { get; private set; }

  //======================================

  public event InitializedEventHandler Initialized;
  public event ShutdownedEventHandler Shutdowned;

  //======================================

  public bool Initialize()
  {
    Alekrus.UnivarsalPlatform.Utilities.PlatformDebugging.OnLog += Debug.Log;
    Alekrus.UnivarsalPlatform.Utilities.PlatformDebugging.OnLogError += Debug.LogError;
    Alekrus.UnivarsalPlatform.Utilities.PlatformDebugging.OnLogWarning += Debug.LogWarning;

    Main = MainProvider.Create();

    Main.Initialized += Main_Initialized;

    return Main.Initialize();
  }

  public void Update()
  {
    Main?.Update();
    LocalUserProfiles?.Update();
    SaveLoad?.Update();
  }

  public bool Shutdown() => Main.Shutdown();

  //======================================

  private void Main_Initialized()
  {
    Main.Initialized -= Main_Initialized;

    LocalUserProfiles = Main.GetSubInterface<ILocalUserProfiles>();

    LocalUserProfiles.Initialized += LocalUserProfiles_Initialized;

    LocalUserProfiles.Initialize();
  }

  private void LocalUserProfiles_Initialized()
  {
    LocalUserProfiles.Initialized -= LocalUserProfiles_Initialized;

    Achievements = Main.GetSubInterface<IAchievements>();
    SaveLoad = Main.GetSubInterface<ISaveLoad>();

    Achievements.Initialized += Achievements_Initialized;
    SaveLoad.Initialized += SaveLoad_Initialized;

    Achievements.Initialize();
    SaveLoad.Initialize();
  }

  private void Achievements_Initialized()
  {
    Achievements.Initialized -= Achievements_Initialized;

    Achievements.RequestAchievementsInfo(LocalUserProfiles.GetPrimaryLocalUserId());
  }

  private void SaveLoad_Initialized()
  {
    SaveLoad.Initialized -= SaveLoad_Initialized;
  }*/
}