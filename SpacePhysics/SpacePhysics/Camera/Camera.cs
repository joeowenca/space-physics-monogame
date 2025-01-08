using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Menu;
using SpacePhysics.Player;
using SpacePhysics.Scenes;
using static SpacePhysics.GameState;

namespace SpacePhysics.Camera;

public class Camera
{
  private static InputManager input;
  public static Vector2 position;
  public static Vector2 offset;
  public static Vector2 targetOffset;
  public static Vector2 rotatedOffset;

  private static Vector2 initialPosition;
  private static Vector2 shakeDirection;
  private static Vector2 shakeOffset;

  private static float counter;

  public static float cameraOffsetLerpSpeed;

  public static float zoomOverride;
  public static float targetZoomOverride;
  public static float zoomOverrideLerpSpeed;
  public static float zoomOverrideLerpSpeedFactor;
  public static float zoomSpeed;


  private static float minZoom;
  private static float maxZoom;
  public static float rotation;

  public static bool changeCamera;
  public static bool cameraZoomMode;

  public static bool allowInput;

  public Camera() { }

  public static void Initialize()
  {
    input = new();
    counter = 0;
    offset = Vector2.Zero;
    targetOffset = Vector2.Zero;
    rotatedOffset = Vector2.Zero;
    initialPosition = Vector2.Zero;
    shakeDirection = Vector2.Zero;
    zoomOverride = 1f;
    targetZoomOverride = 1f;
    zoomOverrideLerpSpeedFactor = 0.005f;
    zoomSpeed = 1.3f;
    minZoom = 0.4f;
    maxZoom = 4f;
    changeCamera = false;
    cameraZoomMode = false;
    allowInput = true;
  }

  public static void Update()
  {
    input.Update();
    input.allowInput = allowInput;

    zoomOverrideLerpSpeed = deltaTime * zoomOverrideLerpSpeedFactor;

    AdjustCamera();
  }

  public static Vector2 Shake(float intensity)
  {
    counter += deltaTime;

    if (counter >= 1f / 60f)
    {
      counter = 0f;
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
    position = GameState.position - new Vector2(screenSize.X / 2, screenSize.Y / 2);

    if (input.ToggleCameraAngle())
    {
      changeCamera = !changeCamera;
    }

    rotation = 0f;

    if (changeCamera)
    {
      rotation = -direction;
    }

    if (input.ToggleCameraMode())
    {
      cameraZoomMode = !cameraZoomMode;
    }

    if
    (
      (state == State.Play && SceneManager.GetCurrentScene() is Scenes.Space.SpaceScene)
      || SceneManager.GetCurrentScene() is Scenes.Start.StartScene
    ) shakeOffset = Shake(Ship.thrustAmount);

    if (state == State.Play
      && SceneManager.GetCurrentScene() is Scenes.Space.SpaceScene
      && !cameraZoomMode
      && maneuverMode
    )
    {
      targetOffset = new Vector2
      (
        -input.AnalogStick().Right.X,
        input.AnalogStick().Right.Y
      ) * screenSize.Y * 0.2f;
    }

    offset.X = MathHelper.Lerp(offset.X, targetOffset.X, deltaTime * cameraOffsetLerpSpeed);
    offset.Y = MathHelper.Lerp(offset.Y, targetOffset.Y, deltaTime * cameraOffsetLerpSpeed);

    rotatedOffset = Utilities.RotateVector2(offset, -rotation);

    if (Math.Abs(offset.X - targetOffset.X) < 0.01f) offset.X = targetOffset.X;
    if (Math.Abs(offset.Y - targetOffset.Y) < 0.01f) offset.Y = targetOffset.Y;
  }

  private static float CalculateZoom(float parallaxFactor)
  {
    parallaxFactor *= 7;

    if (parallaxFactor == 1) return 1f;

    zoomSpeed = 1f + (Math.Abs(input.AdjustCameraZoom()) * 0.3f);

    if (input.AdjustCameraZoom() > 0)
    {
      targetZoom *= (float)Math.Pow(zoomSpeed, deltaTime);
    }

    if (input.AdjustCameraZoom() < 0)
    {
      targetZoom /= (float)Math.Pow(zoomSpeed, deltaTime);
    }

    zoom = MathHelper.Lerp(zoom, targetZoom, deltaTime * 2f);

    if (Math.Abs(zoom - targetZoom) < 0.001f) zoom = targetZoom;

    zoom = Math.Clamp(zoom, minZoom, maxZoom);
    targetZoom = Math.Clamp(targetZoom, minZoom, maxZoom);

    zoomPercent =
      (((float)Math.Log10(zoom) -
      ((float)Math.Log10(minZoom)) /
      (float)Math.Log10(maxZoom) -
      (float)Math.Log10(minZoom)) *
      100) - 66; // TODO: Why do I need to subtract by 66??

    return MathHelper.Lerp(1f, zoom, GetZoomFactor(parallaxFactor));
  }

  private static float CalculateZoomOverride(float parallaxFactor)
  {
    parallaxFactor *= 7;

    if (parallaxFactor == 1) return 1f;

    zoomOverride = Utilities.ExponentialLerp(zoomOverride, targetZoomOverride, zoomOverrideLerpSpeed);

    if (Math.Abs(zoomOverride - targetZoomOverride) < 0.01f) zoomOverride = targetZoomOverride;

    zoomOverride = Math.Clamp(zoomOverride, 0f, 20f);

    return MathHelper.Lerp(1f, zoomOverride, GetZoomFactor(parallaxFactor));
  }

  private static float GetZoomFactor(float parallaxFactor)
  {
    return (float)Math.Pow(parallaxFactor / 7f, 3f);
  }

  public static Matrix GetViewMatrix(float parallaxFactor)
  {
    float zoom = CalculateZoom(parallaxFactor);
    float zoomOverride = CalculateZoomOverride(parallaxFactor);

    return
        Matrix.CreateTranslation(new Vector3((-GameState.position.X) * parallaxFactor + rotatedOffset.X, (-GameState.position.Y) * parallaxFactor + rotatedOffset.Y, 0)) *
        Matrix.CreateRotationZ(rotation) *
        Matrix.CreateTranslation(new Vector3(parallaxFactor == 0 ? 0 : shakeOffset.X, parallaxFactor == 0 ? 0 : shakeOffset.Y, 0)) *
        Matrix.CreateScale(zoom * zoomOverride) *
        Matrix.CreateTranslation(new Vector3(screenSize.X / 2f, screenSize.Y / 2f, 0));
  }
}
