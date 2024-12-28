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

  public readonly bool gamePadConnected;

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
  }

  public bool OnFirstFramePress(Keys key)
  {
    if (!allowInput) return false;

    return currentKeyboardstate.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
  }

  public bool ContinuousPress(Keys key)
  {
    if (!allowInput) return false;

    return currentKeyboardstate.IsKeyDown(key);
  }

  public void ControllerRumble(float intensity)
  {
    GamePad.SetVibration(PlayerIndex.One, intensity, intensity);
  }

  public (Vector2 Left, Vector2 Right) AnalogStick()
  {
    return
    (
      currentGamePadState.ThumbSticks.Left,
      currentGamePadState.ThumbSticks.Right
    );
  }

  public (float Left, float Right) Trigger()
  {
    return
    (
      currentGamePadState.Triggers.Left,
      currentGamePadState.Triggers.Right
    );
  }

  public (ButtonState left, ButtonState right) Bumper()
  {
    return
      (
        currentGamePadState.Buttons.LeftShoulder,
        currentGamePadState.Buttons.LeftShoulder
      );
  }

  public
  (
    ButtonState A,
    ButtonState B,
    ButtonState X,
    ButtonState Y
  )
    FaceButton()
  {
    return
    (
      currentGamePadState.Buttons.A,
      currentGamePadState.Buttons.B,
      currentGamePadState.Buttons.X,
      currentGamePadState.Buttons.Y
    );
  }

  public
  (
    ButtonState Left,
    ButtonState Right,
    ButtonState Up,
    ButtonState Down
  )
    DPad()
  {
    return
    (
      currentGamePadState.DPad.Left,
      currentGamePadState.DPad.Right,
      currentGamePadState.DPad.Up,
      currentGamePadState.DPad.Down
    );
  }

  public (ButtonState start, ButtonState select) Menu()
  {
    return
      (
        currentGamePadState.Buttons.Start,
        currentGamePadState.Buttons.Back
      );
  }
}
