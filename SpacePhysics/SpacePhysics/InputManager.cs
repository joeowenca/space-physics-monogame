using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpacePhysics;

public class InputManager
{
  private KeyboardState previousKeyboardState;
  private KeyboardState currentKeyboardstate;

  private GamePadState previousGamePadState;
  private GamePadState currentGamePadState;

  public bool allowInput;

  public bool gamePadConnected;

  public InputManager(bool allowInput = true)
  {
    this.allowInput = allowInput;

    gamePadConnected = false;

    previousKeyboardState = Keyboard.GetState();
    currentKeyboardstate = Keyboard.GetState();

    previousGamePadState = GamePad.GetState(PlayerIndex.One);
    currentGamePadState = GamePad.GetState(PlayerIndex.One);
  }

  public void Update()
  {
    previousKeyboardState = currentKeyboardstate;
    currentKeyboardstate = Keyboard.GetState();

    previousGamePadState = currentGamePadState;
    currentGamePadState = GamePad.GetState(PlayerIndex.One);

    gamePadConnected = GamePad.GetState(PlayerIndex.One).IsConnected;
  }

  public bool OnFirstFrameKeyPress(Keys key)
  {
    if (!allowInput) return false;

    return currentKeyboardstate.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
  }

  public bool ContinuousKeyPress(Keys key)
  {
    if (!allowInput) return false;

    return currentKeyboardstate.IsKeyDown(key);
  }

  public bool OnFirstFrameButtonPress(Buttons button)
  {
    if (!allowInput || !gamePadConnected) return false;

    return currentGamePadState.IsButtonDown(button) && !previousGamePadState.IsButtonDown(button);
  }

  public bool ContinuousButtonPress(Buttons button)
  {
    if (!allowInput || !gamePadConnected) return false;

    return currentGamePadState.IsButtonDown(button);
  }

  public void ControllerRumble(float intensity)
  {
    if (!allowInput || !gamePadConnected) return;

    GamePad.SetVibration(PlayerIndex.One, intensity, intensity);
  }

  public (Vector2 Left, Vector2 Right) AnalogStick()
  {
    if (!allowInput || !gamePadConnected) return (Vector2.Zero, Vector2.Zero);

    return
    (
      currentGamePadState.ThumbSticks.Left,
      currentGamePadState.ThumbSticks.Right
    );
  }

  public (float Left, float Right) Trigger()
  {
    if (!allowInput || !gamePadConnected) return (0f, 0f);

    return
    (
      currentGamePadState.Triggers.Left,
      currentGamePadState.Triggers.Right
    );
  }

  public Vector2 AdjustRCS()
  {
    Vector2 rcsAmount = Vector2.Zero;

    if (ContinuousKeyPress(Keys.Left) || ContinuousKeyPress(Keys.A))
      rcsAmount.X = -1f;
    if (ContinuousKeyPress(Keys.Right) || ContinuousKeyPress(Keys.D))
      rcsAmount.X = 1f;
    if (ContinuousKeyPress(Keys.Up) || ContinuousKeyPress(Keys.W))
      rcsAmount.Y = -1f;
    if (ContinuousKeyPress(Keys.Down) || ContinuousKeyPress(Keys.S))
      rcsAmount.Y = 1f;

    if (gamePadConnected)
    {
      rcsAmount.X = AnalogStick().Left.X
        + (
          !Camera.Camera.cameraZoomMode
          && !GameState.maneuverMode ?
          AnalogStick().Right.X
          : 0f
        );

      rcsAmount.Y = -AnalogStick().Left.Y
      + (
        !Camera.Camera.cameraZoomMode
        && !GameState.maneuverMode ?
        -AnalogStick().Right.Y
        : 0f
      );
    }

    return rcsAmount;
  }

  public Vector2 MoveCamera()
  {
    Vector2 moveAmount = Vector2.Zero;

    if (gamePadConnected && !Camera.Camera.cameraZoomMode)
    {
      return new Vector2(-AnalogStick().Right.X, AnalogStick().Right.Y);
    }

    return moveAmount;
  }

  public float AdjustPitch()
  {
    if (ContinuousKeyPress(Keys.Right) || ContinuousKeyPress(Keys.D)) return 1f;
    if (ContinuousKeyPress(Keys.Left) || ContinuousKeyPress(Keys.A)) return -1f;

    if (gamePadConnected)
    {
      return AnalogStick().Left.X;
    }

    return 0f;
  }

  public float AdjustThrottle()
  {
    float throttleChangeSpeed = GameState.deltaTime * 0.4f;
    float throttleChangeAmount = 0f;

    if (gamePadConnected)
    {
      throttleChangeAmount = (Trigger().Right + -Trigger().Left) * 10f;
    }

    if (ContinuousKeyPress(Keys.LeftShift)) throttleChangeAmount = 1f;
    if (ContinuousKeyPress(Keys.LeftControl)) throttleChangeAmount = -1f;

    if (OnFirstFrameKeyPress(Keys.Z)) throttleChangeAmount = 1000f;
    if (OnFirstFrameKeyPress(Keys.X)) throttleChangeAmount = -1000f;

    return throttleChangeSpeed * throttleChangeAmount;
  }

  public float AdjustCameraZoom()
  {
    if (ContinuousKeyPress(Keys.OemPlus)) return 1f;
    if (ContinuousKeyPress(Keys.OemMinus)) return -1f;

    if (gamePadConnected && Camera.Camera.cameraZoomMode)
    {
      return AnalogStick().Right.Y;
    }
    else
    {
      return 0f;
    }
  }

  public bool ToggleSAS()
  {
    return OnFirstFrameKeyPress(Keys.T) || OnFirstFrameButtonPress(Buttons.Y);
  }

  public bool ToggleRCS()
  {
    return OnFirstFrameKeyPress(Keys.R) || OnFirstFrameButtonPress(Buttons.X);
  }

  public bool ToggleRCSMode()
  {
    return OnFirstFrameKeyPress(Keys.B) || OnFirstFrameButtonPress(Buttons.LeftStick);
  }

  public bool ToggleCameraAngle()
  {
    return OnFirstFrameKeyPress(Keys.V) || OnFirstFrameButtonPress(Buttons.Back);
  }

  public bool ToggleCameraMode()
  {
    return OnFirstFrameButtonPress(Buttons.RightStick);
  }
}
