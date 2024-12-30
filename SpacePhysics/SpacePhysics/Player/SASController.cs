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
    if (input.ToggleSAS())
    {
      sas = !sas;
      electricity -= deltaTime;
    }

    if (electricity <= 0) sas = false;
  }

  public static void Stabilize(InputManager input)
  {
    if (sas &&
        (!maneuverMode || !(Math.Abs(input.AdjustPitch()) > 0f))
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
