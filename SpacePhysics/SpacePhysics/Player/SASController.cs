using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static SpacePhysics.GameState;
using static SpacePhysics.Player.Ship;

namespace SpacePhysics.Player;

public static class SASController
{
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
      if (angularVelocity > 0.001f)
      {
        targetPitch = -1f;
        electricity -= deltaTime;
      }

      if (angularVelocity < -0.001f)
      {
        targetPitch = 1f;
        electricity -= deltaTime;
      }

      if (Math.Abs(angularVelocity) < 0.001f)
      {
        targetPitch = 0f;
        angularVelocity = 0f;
      }
    }
  }
}
