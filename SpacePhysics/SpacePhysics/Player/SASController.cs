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

  public static SASTarget sasTarget = SASTarget.Stability;

  private static float stabilityThreshold = 0.002f;

  private static float Kp = 20.0f;
  private static float Kv = 40.0f;

  private static float targetAngle;

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
    }

    if
    (
      input.SetSASTargetProgradeOrStability()
      && sasTarget == SASTarget.Prograde
    )
    {
      sasTarget = SASTarget.Stability;
    }

    if
    (
      input.SetSASTargetProgradeOrStability()
      && sasTarget == SASTarget.Stability
    )
    {
      sasTarget = SASTarget.Prograde;
    }

    if (input.SetSASTargetRetrograde()) sasTarget = SASTarget.Retrograde;
    if (input.SetSASTargetRadialLeft()) sasTarget = SASTarget.RadialLeft;
    if (input.SetSASTargetRadialRight()) sasTarget = SASTarget.RadialRight;
  }

  public static void Stabilize(InputManager input)
  {
    if (sas
        && (!maneuverMode || !(Math.Abs(input.AdjustPitch()) > 0f))
        && sasTarget == SASTarget.Stability
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
    if (sasTarget != SASTarget.Stability)
    {
      if (sasTarget == SASTarget.Prograde)
      {
        targetAngle = velocityAngle;
      }
      else if (sasTarget == SASTarget.Retrograde)
      {
        targetAngle = velocityAngle + (float)Math.PI;
      }
      else if (sasTarget == SASTarget.RadialLeft)
      {
        targetAngle = velocityAngle - (float)(Math.PI * 0.5f) - (130 / velocity.Length());
      }
      else if (sasTarget == SASTarget.RadialRight)
      {
        targetAngle = velocityAngle + (float)(Math.PI * 0.5f) + (130 / velocity.Length());
      }

      float angleError = MathHelper.WrapAngle(targetAngle - direction);

      float dampingPitch = -Kv * angularVelocity;

      float anglePitch = Kp * angleError;

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
