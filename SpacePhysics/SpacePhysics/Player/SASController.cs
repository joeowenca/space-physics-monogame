using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static SpacePhysics.GameState;
using static SpacePhysics.Player.Ship;

namespace SpacePhysics.Player;

public static class SASController
{
  private static float stabilityThreshold = 0.002f;

  public static void ToggleSAS(InputManager input)
  {
    if (input.OnFirstFrameKeyPress(Keys.T))
    {
      sas = !sas;
      electricity -= deltaTime;
    }

    if (input.OnFirstFrameButtonPress(Buttons.Y))
    {
      sas = !sas;
      electricity -= deltaTime;
    }

    if (electricity <= 0) sas = false;
  }

  public static void Stabilize(InputManager input)
  {
    if (sas &&
        (!maneuverMode || (!input.ContinuousKeyPress(Keys.Right) &&
        !input.ContinuousKeyPress(Keys.Left) &&
        !input.ContinuousKeyPress(Keys.D) &&
        !input.ContinuousKeyPress(Keys.A) &&
        Math.Abs(input.AnalogStick().Left.X) <= 0f)
        )
      )
    {
      if (angularVelocity > stabilityThreshold)
      {
        targetPitch = -1f;
        electricity -= deltaTime;
      }

      if (angularVelocity < -stabilityThreshold)
      {
        targetPitch = 1f;
        electricity -= deltaTime;
      }

      if (Math.Abs(angularVelocity) < stabilityThreshold)
      {
        targetPitch = 0f;
        angularVelocity = 0f;
      }
    }
  }
}
