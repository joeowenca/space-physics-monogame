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
    if (input.OnFirstFramePress(Keys.T))
    {
      sas = !sas;
      electricity -= deltaTime;
    }

    if (electricity <= 0) sas = false;
  }

  public static void Stability(InputManager input)
  {
    if (sas &&
        !input.ContinuousPress(Keys.Right) &&
        !input.ContinuousPress(Keys.Left) &&
        !input.ContinuousPress(Keys.D) &&
        !input.ContinuousPress(Keys.A)
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
