using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static SpacePhysics.GameState;
using static SpacePhysics.Player.Ship;

namespace SpacePhysics.Player;

public class RCSController : CustomGameComponent
{
  private static float rcsThrustAmount;

  private static bool rcsLeft;
  private static bool rcsRight;
  private static bool rcsUp;
  private static bool rcsDown;

  public RCSController() : base()
  {
    rcsLeft = false;
    rcsRight = false;
    rcsUp = false;
    rcsDown = false;
    rcsThrustAmount = 50000f;
  }

  public static void Docking(InputManager input)
  {
    rcsThrust = 0f;

    if (rcs)
    {
      if (!maneuverMode)
      {
        if (input.ContinuousPress(Keys.Left) || input.ContinuousPress(Keys.A))
        {
          rcsThrust = rcsThrustAmount;
          electricity -= deltaTime;
          rcsDirection = (float)Math.PI * -0.5f;
          rcsLeft = true;
        }
        else
        {
          rcsLeft = false;
        }

        if (input.ContinuousPress(Keys.Right) || input.ContinuousPress(Keys.D))
        {
          rcsThrust = rcsThrustAmount;
          electricity -= deltaTime;
          rcsDirection = (float)Math.PI * 0.5f;
          rcsRight = true;
        }
        else
        {
          rcsRight = false;
        }
      }
      else
      {
        rcsLeft = false;
        rcsRight = false;
      }

      if (input.ContinuousPress(Keys.Up) || input.ContinuousPress(Keys.W))
      {
        rcsThrust = rcsThrustAmount * 2f;
        electricity -= deltaTime;
        rcsDirection = 0f;
        rcsUp = true;
      }
      else
      {
        rcsUp = false;
      }

      if (input.ContinuousPress(Keys.Down) || input.ContinuousPress(Keys.S))
      {
        rcsThrust = rcsThrustAmount * 2f;
        electricity -= deltaTime;
        rcsDirection = (float)Math.PI;
        rcsDown = true;
      }
      else
      {
        rcsDown = false;
      }
    }

    if (input.OnFirstFramePress(Keys.B))
    {
      maneuverMode = !maneuverMode;
      electricity -= deltaTime;
    }

    rcsAmountTarget[2] = (rcsUp && mono > 0f) ? 1f : 0f;
    rcsAmountTarget[3] = (rcsDown && mono > 0f) ? 1f : 0f;

    rcsAmount[2] = MathHelper.Lerp(rcsAmount[2], rcsAmountTarget[2], deltaTime * rcsLerpSpeed);
    rcsAmount[3] = MathHelper.Lerp(rcsAmount[3], rcsAmountTarget[3], deltaTime * rcsLerpSpeed);

    rcsAmountTarget[4] = (rcsLeft && mono > 0f) ? 1f : 0f;
    rcsAmountTarget[5] = (rcsRight && mono > 0f) ? 1f : 0f;

    rcsAmount[4] = MathHelper.Lerp(rcsAmount[4], rcsAmountTarget[4], deltaTime * rcsLerpSpeed);
    rcsAmount[5] = MathHelper.Lerp(rcsAmount[5], rcsAmountTarget[5], deltaTime * rcsLerpSpeed);
  }

  public static void RCS()
  {
    for (int i = 0; i < rcsAmount.Length; i++)
    {
      if (mono > 0f)
      {
        if (i == 2 || i == 3)
        {
          mono -= rcsAmount[i] * deltaTime * 2f;
        }
        else
        {
          mono -= rcsAmount[i] * deltaTime;
        }
      }
      else
      {
        mono = 0f;
      }
    }

    mono = Math.Clamp(mono, 0f, maxMono);
  }
}
