using System;

public class GetLocation
{
  public static Location[] GetNamesAllLocation()
  {
    return (Location[])Enum.GetValues(typeof(Location));
  }
}

[Serializable]
public enum Location
{
  Chapter_1,
  Chapter_2
}