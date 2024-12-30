using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static SpacePhysics.GameState;
using static SpacePhysics.Player.Ship;

namespace SpacePhysics.Player;

public static class SASController
{
  private static float stabilityThreshold = 0.002f;

  private static float Kp = 20.0f;
  private static float Kv = 40.0f;

  private static bool lockOnRadialRight = true;

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
        (!maneuverMode || !(Math.Abs(input.AdjustPitch()) > 0f) && !lockOnRadialRight)
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

  public static void LockOnTarget(InputManager input)
  {
    float targetAngle = velocityAngle;

    float angleError = MathHelper.WrapAngle(targetAngle - direction);

    float dampingPitch = -Kv * angularVelocity;

    float anglePitch = Kp * angleError;

    if (sas &&
        (!maneuverMode || !(Math.Abs(input.AdjustPitch()) > 0f) && lockOnRadialRight)
      )
    {
      targetPitch = anglePitch + dampingPitch;
      targetPitch = Math.Clamp(targetPitch, -1.0f, 1.0f);
    }
  }
}
