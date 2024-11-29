using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpacePhysics.Camera;

public class Camera
{
  private static InputManager input;
  public static Vector2 position;
  public static Vector2 offset;

  private static Vector2 initialPosition;
  private static Vector2 shakeDirection;
  private static Vector2 shakeOffset;

  private static int counter;

  public static float zoomOverride;
  public static float targetZoomOverride;
  public static float zoomOverrideLerpSpeed;
  public static float zoomPercent;
  private static float minZoom;
  private static float maxZoom;
  private static float rotation;

  public static bool changeCamera;
  public static bool allowInput;

  public Camera() { }

  public static void Initialize()
  {
    input = new();
    counter = 0;
    offset = Vector2.Zero;
    initialPosition = Vector2.Zero;
    shakeDirection = Vector2.Zero;
    zoomOverride = 1f;
    targetZoomOverride = 1f;
    zoomOverrideLerpSpeed = 0.0005f;
    minZoom = 0.4f;
    maxZoom = 4f;
    changeCamera = false;
    allowInput = true;
  }

  public static void Update()
  {
    input.Update();
    input.allowInput = allowInput;
  }

  public static Vector2 Shake(float intensity)
  {
    counter++;

    if (counter % 4 == 0)
    {
      Random random = new Random();
      float angle = (float)(random.NextDouble() * Math.PI * 2);
      shakeDirection = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
    }

    float shakeMagnitude = intensity * (float)Math.Cos(Math.PI * (counter % 4) / 4);

    Vector2 shakeOffset = shakeDirection * shakeMagnitude;

    return initialPosition + shakeOffset;
  }

  private static void AdjustCamera()
  {
    position = GameState.position - new Vector2(Main.screenSize.X / 2, Main.screenSize.Y / 2);

    if (input.OnFirstFramePress(Keys.V))
    {
      changeCamera = !changeCamera;
    }

    if (changeCamera)
    {
      rotation = -GameState.direction - (float)(Math.PI / 2);
    }
    else
    {
      rotation = 0f;
    }

    shakeOffset = Shake(GameState.throttle * 0.01f);
  }

  private static float CalculateZoom(float parallaxFactor)
  {
    parallaxFactor *= 7;

    if (input.ContinuousPress(Keys.OemPlus))
      GameState.targetCameraZoom *= 1.005f;
    if (input.ContinuousPress(Keys.OemMinus))
      GameState.targetCameraZoom *= 0.995f;

    if (parallaxFactor == 1) return 1f;

    GameState.cameraZoom = MathHelper.Lerp(GameState.cameraZoom, GameState.targetCameraZoom, 0.03f);
    GameState.cameraZoom = Math.Clamp(GameState.cameraZoom, minZoom, maxZoom);
    GameState.targetCameraZoom = Math.Clamp(GameState.targetCameraZoom, minZoom, maxZoom);

    zoomPercent =
      ((float)Math.Log10(GameState.cameraZoom) -
      ((float)Math.Log10(minZoom)) /
      (float)Math.Log10(maxZoom) -
      (float)Math.Log10(minZoom)) *
      100;

    return MathHelper.Lerp(1f, GameState.cameraZoom, GetZoomFactor(parallaxFactor));
  }

  private static float CalculateZoomOverride(float parallaxFactor)
  {
    parallaxFactor *= 7;

    if (parallaxFactor == 1) return 1f;

    zoomOverride = ExponentialLerp(zoomOverride, targetZoomOverride, zoomOverrideLerpSpeed);
    zoomOverride = Math.Clamp(zoomOverride, 0f, 20f);

    return MathHelper.Lerp(1f, zoomOverride, GetZoomFactor(parallaxFactor));
  }

  private static float GetZoomFactor(float parallaxFactor)
  {
    return (float)Math.Pow(parallaxFactor / 7f, 3f);
  }

  private static float ExponentialLerp(float start, float end, float t, float k = 10f)
  {
    t = MathHelper.Clamp(t, 0f, 1f);
    float factor = 1f - (float)Math.Exp(-t * k);
    return MathHelper.Lerp(start, end, factor);
  }

  public static Matrix GetViewMatrix(float parallaxFactor)
  {
    float zoom = CalculateZoom(parallaxFactor);
    float extremeZoom = CalculateZoomOverride(parallaxFactor);

    return
        Matrix.CreateTranslation(new Vector3((-GameState.position.X) * parallaxFactor + offset.X, (-GameState.position.Y) * parallaxFactor + offset.Y, 0)) *
        Matrix.CreateRotationZ(rotation) *
        Matrix.CreateTranslation(new Vector3(parallaxFactor == 0 ? 0 : shakeOffset.X, parallaxFactor == 0 ? 0 : shakeOffset.Y, 0)) *
        Matrix.CreateScale(zoom * extremeZoom) *
        Matrix.CreateTranslation(new Vector3(Main.screenSize.X / 2f, Main.screenSize.Y / 2f, 0));
  }
}
