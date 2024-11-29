using System;
using Microsoft.Xna.Framework.Input;

namespace SpacePhysics;

public class InputManager
{
  private KeyboardState previousKeyboardState;
  private KeyboardState currentKeyboardstate;
  public bool allowInput;

  public InputManager(bool allowInput = true)
  {
    this.allowInput = allowInput;

    previousKeyboardState = Keyboard.GetState();
    currentKeyboardstate = Keyboard.GetState();
  }

  public void Update()
  {
    previousKeyboardState = currentKeyboardstate;
    currentKeyboardstate = Keyboard.GetState();
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
}
