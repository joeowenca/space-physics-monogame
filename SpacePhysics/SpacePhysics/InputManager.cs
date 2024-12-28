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
    if (!allowInput || gamePadConnected) return false;

    return currentKeyboardstate.IsKeyDown(key) && !previousKeyboardState.IsKeyDown(key);
  }

  public bool ContinuousKeyPress(Keys key)
  {
    if (!allowInput || gamePadConnected) return false;

    return currentKeyboardstate.IsKeyDown(key);
  }

  public bool OnFirstFrameButtonPress(Buttons button)
  {
    if (!allowInput || !gamePadConnected) return false;

    return currentGamePadState.IsButtonDown(button) && !previousGamePadState.IsButtonDown(button);
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
}
