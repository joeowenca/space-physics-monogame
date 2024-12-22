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

  public static Vector2 screenSize = new Vector2(1920, 1080);
  public static float FPS;

  public static Vector2 position;
  public static Vector2 velocity;

  public static Color defaultColor;
  public static Color highlightColor;

  public static float angularVelocity;
  public static float velocityAngle;
  public static float direction;
  public static float throttle;
  public static float targetThrottle;
  public static float fuel;
  public static float maxFuel;
  public static float fuelPercent;
  public static float zoom;
  public static float targetZoom;
  public static float scale;
  public static float scaleOverride;
  public static float hudScale;
  public static float hudTextScale;
  public static float units;
  public static float elapsedTime;
  public static float deltaTime;

  public static bool sas;
  public static bool debug;

  public static void Initialize()
  {
    position = Vector2.Zero;
    velocity = Vector2.Zero;
    defaultColor = Color.White * 0.75f;
    highlightColor = Color.Gold;
    angularVelocity = 0f;
    velocityAngle = 0f;
    direction = 0f;
    throttle = 0f;
    targetThrottle = 0f;
    fuel = 7500f;
    maxFuel = fuel;
    zoom = 1.26f;
    targetZoom = zoom;
    scaleOverride = 0.3f;
    scale = screenSize.Y / 1080 * scaleOverride;
    hudScale = 1.4f;
    hudTextScale = hudScale * 0.4f;
    units = 5f;
    elapsedTime = 0f;
    deltaTime = 0f;
    sas = false;
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
    velocityAngle = MathF.Atan2(velocity.Y, velocity.X) + (float)(Math.PI * 0.5f);
    fuelPercent = fuel / maxFuel * 100f;

    if (velocityAngle == (float)(Math.PI * 0.5f) && velocity == Vector2.Zero)
    {
      velocityAngle = 0f;
    }

    deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    elapsedTime += deltaTime;

    if (deltaTime > 0)
    {
      FPS = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
    }
  }
}
