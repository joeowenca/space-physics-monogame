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
  public static float direction;
  public static float throttle;
  public static float targetThrottle;
  public static float fuel;
  public static float zoom;
  public static float targetZoom;
  public static float scale;
  public static float scaleOverride;
  public static float units;
  public static float elapsedTime;
  public static float deltaTime;

  public static bool sas;
  public static bool debug;

  public static void Initialize()
  {
    state = State.TitleScreen;
    position = Vector2.Zero;
    velocity = Vector2.Zero;
    defaultColor = Color.White * 0.75f;
    highlightColor = Color.Gold;
    angularVelocity = 0f;
    direction = (float)-(Math.PI * 0.5f);
    throttle = 0f;
    targetThrottle = 0f;
    fuel = 7500f;
    zoom = 1.27f;
    targetZoom = zoom;
    scale = 0.4f;
    units = 5f;
    elapsedTime = 0f;
    deltaTime = 0f;
    sas = false;
  }

  public static void Update(GameTime gameTime)
  {
    deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    elapsedTime += deltaTime;

    if (deltaTime > 0)
    {
      FPS = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
  }
}
