using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static SpacePhysics.GameState;
using static SpacePhysics.Player.Ship;

namespace SpacePhysics.Player;

public class SAS
{
  public static void Stability(InputManager input)
  {
    float angularThrust = thrustAmount / mass * deltaTime * 250f;

    float rcsAngularThrust = 1 / mass * 4f * deltaTime * 250f;

    if (maneuverMode && (angularThrust > 0 || rcs))
    {
      if (input.ContinuousPress(Keys.Right) || input.ContinuousPress(Keys.D))
      {
        angularVelocity += angularThrust;
        electricity -= deltaTime;

        if (rcs)
        {
          angularVelocity += rcsAngularThrust;
          electricity -= deltaTime;
          rcsRotateRight = true;
        }
      }
      else
      {
        rcsRotateRight = false;
      }

      if (input.ContinuousPress(Keys.Left) || input.ContinuousPress(Keys.A))
      {
        angularVelocity -= angularThrust;
        electricity -= deltaTime;

        if (rcs)
        {
          angularVelocity -= rcsAngularThrust;
          electricity -= deltaTime;
          rcsRotateLeft = true;
        }
      }
      else
      {
        rcsRotateLeft = false;
      }
    }
    else
    {
      rcsRotateLeft = false;
      rcsRotateRight = false;
    }

    if (sas && (angularThrust > 0 || rcs) &&
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

        if (rcs)
        {
          angularVelocity -= rcsAngularThrust;
          electricity -= deltaTime;
          rcsRotateLeft = true;
        }
      }
      else
      {
        rcsRotateLeft = false;
      }

      if (angularVelocity < -0.001f)
      {
        angularVelocity += angularThrust;
        electricity -= deltaTime;

        if (rcs)
        {
          angularVelocity += rcsAngularThrust;
          electricity -= deltaTime;
          rcsRotateRight = true;
        }
      }
      else
      {
        rcsRotateRight = false;
      }

      if (Math.Abs(angularVelocity) < 0.001f)
      {
        angularVelocity = 0f;
      }
    }

    direction += angularVelocity * deltaTime;

    if (input.OnFirstFramePress(Keys.T))
    {
      sas = !sas;
    }

    if (input.OnFirstFramePress(Keys.R))
    {
      rcs = !rcs;
    }

    rcsAmountTarget[0] = (rcsRotateLeft && mono > 0f) ? 1f : 0f;
    rcsAmountTarget[1] = (rcsRotateRight && mono > 0f) ? 1f : 0f;

    rcsAmount[0] = MathHelper.Lerp(rcsAmount[0], rcsAmountTarget[0], deltaTime * rcsLerpSpeed);
    rcsAmount[1] = MathHelper.Lerp(rcsAmount[1], rcsAmountTarget[1], deltaTime * rcsLerpSpeed);
  }
}
