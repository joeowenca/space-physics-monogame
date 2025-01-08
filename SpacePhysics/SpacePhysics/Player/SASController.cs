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

  private static float stabilityThreshold = 0.4f;

  private static float Kp = 10.0f;
  private static float Kv = 15.0f;

  private static float targetAngle;

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

      if (stabilityMode)
      {
        sasTarget = SASTarget.Stability;
      }
    }

    if (input.SetSASTargetRetrograde())
    {
      sasTarget = SASTarget.Retrograde;
      stabilityMode = false;
    }
    if (input.SetSASTargetRadialLeft())
    {
      sasTarget = SASTarget.RadialLeft;
      stabilityMode = false;
    }
    if (input.SetSASTargetRadialRight())
    {
      sasTarget = SASTarget.RadialRight;
      stabilityMode = false;
    }

    if (sasTarget == SASTarget.Stability) sasModeString = "Stability";
    if (sasTarget == SASTarget.Prograde) sasModeString = "Prograde";
    if (sasTarget == SASTarget.Retrograde) sasModeString = "Retrograde";
    if (sasTarget == SASTarget.RadialLeft) sasModeString = "Radial Left";
    if (sasTarget == SASTarget.RadialRight) sasModeString = "Radial Right";
  }

  public static void Stabilize(InputManager input)
  {
    if (sas
        && (!maneuverMode || !(Math.Abs(input.AdjustPitch()) > 0f))
        && stabilityMode
      )
    {
      if (angularVelocity > stabilityThreshold * deltaTime)
      {
        targetPitch = -1f;
        electricity -= deltaTime;
      }

      if (angularVelocity < -stabilityThreshold * deltaTime)
      {
        targetPitch = 1f;
        electricity -= deltaTime;
      }

      if (Math.Abs(angularVelocity) < stabilityThreshold * deltaTime)
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

  public static bool isSasMode(SASTarget sasMode)
  {
    return sasMode == sasTarget && sas;
  }
}
