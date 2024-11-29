using System;
using System.Numerics;

namespace SpacePhysics;

public class GameState
{
  public static Vector2 position;
  public static Vector2 velocity;

  public static float angularVelocity;
  public static float direction;
  public static float throttle;
  public static float targetThrottle;
  public static float fuel;
  public static float cameraZoom;
  public static float targetCameraZoom;

  public static bool sas;
  public static bool debug;

  public static void Initialize()
  {
    position = Vector2.Zero;
    velocity = Vector2.Zero;
    angularVelocity = 0f;
    direction = (float)-(Math.PI / 2);
    throttle = 0f;
    targetThrottle = 0f;
    fuel = 7500;
    cameraZoom = 1.27f;
    targetCameraZoom = cameraZoom;
    sas = false;
  }
}
