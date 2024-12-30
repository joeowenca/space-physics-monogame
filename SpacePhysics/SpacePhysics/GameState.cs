using System;
using Microsoft.Xna.Framework;

namespace SpacePhysics;

public class GameState
{
  public enum State
  {
    TitleScreen,
    MainMenu,
    Settings,
    Play,
    Pause
  }

  public static State state;

  public static Vector2 screenSize = new Vector2(2560, 1440);
  public static float FPS;

  public static Vector2 position;
  public static Vector2 velocity;

  public static Color defaultColor;
  public static Color highlightColor;

  public static float angularVelocity;
  public static float progradeRadians;
  public static float retrogradeRadians;
  public static float radialLeftRadians;
  public static float radialRightRadians;
  public static float direction;
  public static float throttle;
  public static float targetThrottle;
  public static float fuel;
  public static float maxFuel;
  public static float fuelPercent;
  public static float mono;
  public static float maxMono;
  public static float monoPercent;
  public static float electricity;
  public static float maxElectricity;
  public static float electricityPercent;
  public static float zoom;
  public static float targetZoom;
  public static float zoomPercent;
  public static float scale;
  public static float scaleOverride;
  public static float hudScale;
  public static float hudTextScale;
  public static float hudScaleOverride;
  public static float hudScaleOverrideFactor;
  public static float opacityTransitionSpeed;
  public static float units;
  public static float elapsedTime;
  public static float deltaTime;

  public static bool sas;
  public static bool rcs;
  public static bool maneuverMode;
  public static bool debug;
  public static bool quit;

  private static DateTime lastFPSCheck;

  public static void Initialize()
  {
    position = Vector2.Zero;
    velocity = Vector2.Zero;
    defaultColor = Color.White * 0.75f;
    highlightColor = Color.Gold;
    angularVelocity = 0f;
    progradeRadians = 0f;
    retrogradeRadians = 0f;
    radialLeftRadians = 0f;
    radialRightRadians = 0f;
    direction = 0f;
    throttle = 0f;
    targetThrottle = 0f;
    fuel = 7500f;
    maxFuel = fuel;
    mono = 300f;
    maxMono = mono;
    electricity = 1200f;
    maxElectricity = electricity;
    zoom = 1.26f;
    targetZoom = zoom;
    scaleOverride = 0.3f;
    scale = screenSize.Y / 1080 * scaleOverride;
    hudScale = 1.4f;
    hudTextScale = hudScale * 0.4f;
    hudScaleOverrideFactor = 0.75f;
    hudScaleOverride = scale * hudScaleOverrideFactor;
    opacityTransitionSpeed = 0.6f;
    units = 5f;
    elapsedTime = 0f;
    deltaTime = 0f;
    sas = false;
    rcs = false;
    maneuverMode = true;
    quit = false;
  }

  public static void Intro()
  {
    state = State.TitleScreen;
    position = new Vector2(400, 400);
    velocity = new Vector2(200, -80);
    angularVelocity = 0.15f;
    direction = -0.4f;
    zoom = 1.75f;
    targetZoom = zoom;
  }

  public static void Update(GameTime gameTime)
  {
    progradeRadians = MathF.Atan2(velocity.Y, velocity.X) + (float)(Math.PI * 0.5f);

    if (progradeRadians == (float)(Math.PI * 0.5f) && velocity == Vector2.Zero)
    {
      progradeRadians = 0f;
    }

    retrogradeRadians = progradeRadians + (float)Math.PI;
    radialLeftRadians = progradeRadians - (float)Math.PI * 0.5f;
    radialRightRadians = progradeRadians + (float)Math.PI * 0.5f;

    fuelPercent = fuel / maxFuel * 100f;
    fuel = Math.Clamp(fuel, 0f, maxFuel);

    monoPercent = mono / maxMono * 100f;
    mono = Math.Clamp(mono, 0f, maxMono);

    electricityPercent = electricity / maxElectricity * 100f;
    electricity = Math.Clamp(electricity, 0f, maxElectricity);

    deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    elapsedTime += deltaTime;

    if ((DateTime.Now - lastFPSCheck).TotalMilliseconds >= 1000 && deltaTime > 0)
    {
      FPS = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
      lastFPSCheck = DateTime.Now;
    }
  }
}
