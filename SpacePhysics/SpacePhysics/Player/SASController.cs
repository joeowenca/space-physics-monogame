using System;
using Microsoft.Xna.Framework;
using static SpacePhysics.GameState;
using static SpacePhysics.Player.Ship;

namespace SpacePhysics.Player;

public static class SASController
{
  public enum SASTarget
  {
    Stability,
    Prograde,
    Retrograde,
    RadialLeft,
    RadialRight
  }

  private static SASTarget sasTarget = SASTarget.Stability;

  private static float stabilityThreshold = 0.002f;

  private static float Kp = 10.0f;
  private static float Kv = 15.0f;

  private static float targetAngle;

  private static bool stabilityMode = true;

  public static string sasModeString = "Stability";

  public static void ToggleSAS(InputManager input)
  {
    if (input.ToggleSAS())
    {
      sas = !sas;
      electricity -= deltaTime;
    }

    if (electricity <= 0) sas = false;
  }

  public static void SetSASMode(InputManager input)
  {
    if
    (
      input.SetSASTargetProgradeOrStability()
      && sasTarget != SASTarget.Prograde
      && sasTarget != SASTarget.Stability
    )
    {
      sasTarget = SASTarget.Prograde;
      stabilityMode = true;
    }

    if
    (
      input.SetSASTargetProgradeOrStability()
      && (sasTarget == SASTarget.Prograde
      || sasTarget == SASTarget.Stability)
    )
    {
      stabilityMode = !stabilityMode;

      sasTarget = SASTarget.Prograde;
      sasModeString = "Prograde";

      if (stabilityMode)
      {
        sasTarget = SASTarget.Stability;
        sasModeString = "Stability";
      }
    }

    if (input.SetSASTargetRetrograde())
    {
      sasTarget = SASTarget.Retrograde;
      stabilityMode = false;
      sasModeString = "Retrograde";
    }
    if (input.SetSASTargetRadialLeft())
    {
      sasTarget = SASTarget.RadialLeft;
      stabilityMode = false;
      sasModeString = "Radial Left";
    }
    if (input.SetSASTargetRadialRight())
    {
      sasTarget = SASTarget.RadialRight;
      stabilityMode = false;
      sasModeString = "Radial Right";
    }
  }

  public static void Stabilize(InputManager input)
  {
    if (sas
        && (!maneuverMode || !(Math.Abs(input.AdjustPitch()) > 0f))
        && stabilityMode
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
    if (!stabilityMode)
    {
      if (sasTarget == SASTarget.Prograde)
      {
        targetAngle = progradeRadians;
      }
      else if (sasTarget == SASTarget.Retrograde)
      {
        targetAngle = retrogradeRadians;
      }
      else if (sasTarget == SASTarget.RadialLeft)
      {
        targetAngle = radialLeftRadians;
      }
      else if (sasTarget == SASTarget.RadialRight)
      {
        targetAngle = radialRightRadians;
      }

      float target = targetAngle + (progradeAngularVelocity * 1.53f);

      float angleError = MathHelper.WrapAngle(target - direction);

      float anglePitch = Kp * angleError;

      float dampingPitch = -Kv * angularVelocity;

      if (sas &&
          (!maneuverMode || !(Math.Abs(input.AdjustPitch()) > 0f))
        )
      {
        targetPitch = anglePitch + dampingPitch;
        targetPitch = Math.Clamp(targetPitch, -1.0f, 1.0f);
      }
    }
  }
}
