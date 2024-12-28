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
    // ------------ To be moved to Ship.AdjustPitch ----------------------------

    float angularThrust = thrustAmount / mass * deltaTime * 250f;

    if (maneuverMode && angularThrust > 0)
    {
      if (input.ContinuousPress(Keys.Right) || input.ContinuousPress(Keys.D))
      {
        angularVelocity += angularThrust;
        electricity -= deltaTime;
      }

      if (input.ContinuousPress(Keys.Left) || input.ContinuousPress(Keys.A))
      {
        angularVelocity -= angularThrust;
        electricity -= deltaTime;
      }
    }

    // -------------------------------------------------------------------------

    if (sas && angularThrust > 0 &&
        !input.ContinuousPress(Keys.Right) &&
        !input.ContinuousPress(Keys.Left) &&
        !input.ContinuousPress(Keys.D) &&
        !input.ContinuousPress(Keys.A)
      )
    {
      if (angularVelocity > 0.001f)
      {
        angularVelocity -= angularThrust;
        electricity -= deltaTime;
      }

      if (angularVelocity < -0.001f)
      {
        angularVelocity += angularThrust;
        electricity -= deltaTime;
      }

      if (Math.Abs(angularVelocity) < 0.001f)
      {
        angularVelocity = 0f;
      }
    }
  }
}
