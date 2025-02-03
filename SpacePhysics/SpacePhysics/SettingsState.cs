using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace SpacePhysics;

public static class SettingsState
{
  public static string[] aspectRatioOptions = ["4:3", "16:9", "16:10"];

  public static string[] resolutionOptions4x3 =
  [
    "1024x768",
    "1280x960",
    "1400x1050",
    "1440x1080",
    "1600x1200",
    "1856x1392",
    "1920x1440",
    "2048x1536"
  ];

  public static string[] resolutionOptions16x9 =
  [
    "1280x720",
    "1600x900",
    "1920x1080",
    "2560x1440",
    "3840x2160"
  ];

  public static string[] resolutionOptions16x10 =
  [
    "1280x800",
    "1440x900",
    "1680x1050",
    "1920x1200",
    "2560x1600",
    "3840x2400"
  ];

  public static Color uiColor;

  public static float uiSafeZone;

  public static int masterVolume;
  public static int musicVolume;
  public static int soundEffectsVolume;
  public static int menuSoundEffectsVolume;

  public static int volumeIncrementAmount;

  public static int uiScale;

  public static int[] uiScaleOptions = [
    50,
    60,
    70,
    80,
    90,
    100,
    110,
    120,
    130,
    140,
    150
  ];

  public static string resolution = "2560x1440";
  public static string aspectRatio = "16:9";

  public static bool vsync;

  public static void Initialize()
  {
    resolution = "2560x1440";
    aspectRatio = "16:9";
    vsync = true;

    masterVolume = 100;
    musicVolume = 100;
    soundEffectsVolume = 100;
    menuSoundEffectsVolume = 100;

    volumeIncrementAmount = 5;

    uiColor = GameState.highlightColor;
    uiScale = uiScaleOptions[5];
  }

  public static string[] GetReslutionOptionsFromAspectRatio(string aspectRatio)
  {
    if (aspectRatio == "4:3") return resolutionOptions4x3;
    if (aspectRatio == "16:9") return resolutionOptions16x9;
    if (aspectRatio == "16:10") return resolutionOptions16x10;

    return resolutionOptions16x9;
  }

  public static Vector2 GetResolutionVector()
  {
    string[] splitString = resolution.Split('x');

    if (splitString.Length == 2 && int.TryParse(splitString[0], out int width) && int.TryParse(splitString[1], out int height))
    {
      return new Vector2(width, height);
    }
    else
    {
      return new Vector2(1920, 1080);
    }
  }
}
