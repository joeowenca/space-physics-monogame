using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Sprites;
using static SpacePhysics.GameState;
using static SpacePhysics.Player.Ship;

namespace SpacePhysics.Player;

public class RCSController : CustomGameComponent
{
  private static AnimatedSprite rcsSprite;

  public static Vector2 rcsForce;
  public static Vector2 rcsThrottle;
  public static Vector2 rcsTargetThrottle;
  public static Vector2 rcsThrustVector;

  private static Func<float> opacity;

  public static float rcsThrust;
  private static float rcsThrustAmount;
  public static float rcsDirection;
  private static float[] rcsAmount = { 0f, 0f, 0f, 0f, 0f, 0f };

  public static float rcsLerpSpeed;

  public RCSController(Func<float> opacity) : base()
  {
    RCSController.opacity = opacity;
  }

  public override void Initialize()
  {
    rcsForce = Vector2.Zero;
    rcsLerpSpeed = 30f;
    rcsThrustAmount = 50000f;

    base.Initialize();
  }

  public override void Load(ContentManager contentManager)
  {
    rcsSprite = new AnimatedSprite(
      contentManager.Load<Texture2D>("Player/rcs-sheet"),
      4,
      1,
      1f / 24f
    );

    base.Load(contentManager);
  }

  public override void Update()
  {
    rcsSprite.Animate();

    base.Update();
  }

  public static void ToggleRCS(InputManager input)
  {
    if (input.OnFirstFramePress(Keys.R))
    {
      rcs = !rcs;
      electricity -= deltaTime;
    }

    if (electricity <= 0) rcs = false;
  }

  public static void ToggleRCSMode(InputManager input)
  {
    if (input.OnFirstFramePress(Keys.B) && electricity > 0)
    {
      maneuverMode = !maneuverMode;
      electricity -= deltaTime;
    }
  }

  public static void RotateWithRCS()
  {
    float rcsAngularThrust = 1 / mass * 4f * deltaTime * 250f * pitch;
    float targetAmountLeft;
    float targetAmountRight;

    if ((maneuverMode || sas) && rcs)
    {
      angularVelocity += rcsAngularThrust;

      if (pitch <= 0)
      {
        targetAmountLeft = Math.Abs(pitch);
      }
      else
      {
        targetAmountLeft = 0f;
      }

      if (pitch >= 0)
      {
        targetAmountRight = Math.Abs(pitch);
      }
      else
      {
        targetAmountRight = 0f;
      }
    }
    else
    {
      targetAmountLeft = 0f;
      targetAmountRight = 0f;
    }

    rcsAmount[0] = MathHelper.Lerp(rcsAmount[0], targetAmountLeft, deltaTime * rcsLerpSpeed);
    rcsAmount[1] = MathHelper.Lerp(rcsAmount[1], targetAmountRight, deltaTime * rcsLerpSpeed);
  }

  public static void MoveWithRCS(InputManager input)
  {
    if (rcs)
    {
      if (!maneuverMode)
      {
        if (input.ContinuousPress(Keys.Left) || input.ContinuousPress(Keys.A))
        {
          rcsTargetThrottle.X = -1f;
          electricity -= deltaTime;
        }
        else if (input.ContinuousPress(Keys.Right) || input.ContinuousPress(Keys.D))
        {
          rcsTargetThrottle.X = 1f;
          electricity -= deltaTime;
        }
        else
        {
          rcsTargetThrottle.X = 0f;
        }
      }

      if (input.ContinuousPress(Keys.Up) || input.ContinuousPress(Keys.W))
      {
        rcsTargetThrottle.Y = -1f;
        electricity -= deltaTime * 2f;
      }
      else if (input.ContinuousPress(Keys.Down) || input.ContinuousPress(Keys.S))
      {
        rcsTargetThrottle.Y = 1f;
        electricity -= deltaTime * 2f;
      }
      else
      {
        rcsTargetThrottle.Y = 0f;
      }
    }
    else
    {
      rcsTargetThrottle = Vector2.Zero;
    }

    rcsThrottle.X = MathHelper.Lerp(rcsThrottle.X, rcsTargetThrottle.X, deltaTime * rcsLerpSpeed);
    rcsThrottle.Y = MathHelper.Lerp(rcsThrottle.Y, rcsTargetThrottle.Y, deltaTime * rcsLerpSpeed);

    rcsThrottle.X = Math.Clamp(rcsThrottle.X, -1f, 1f);
    rcsThrottle.Y = Math.Clamp(rcsThrottle.Y, -1f, 1f);

    rcsThrustVector = new Vector2(rcsThrottle.X, rcsThrottle.Y * 2f);

    rcsDirection = MathF.Atan2(rcsThrustVector.Y, rcsThrustVector.X) + ((float)Math.PI * 0.5f);

    rcsThrust = rcsThrustAmount * Math.Abs(rcsThrustVector.Length());

    rcsAmount[2] = rcsThrottle.Y <= 0 ? Math.Abs(rcsThrottle.Y) : 0f; // Up
    rcsAmount[3] = rcsThrottle.Y >= 0 ? Math.Abs(rcsThrottle.Y) : 0f; // Down
    rcsAmount[4] = rcsThrottle.X <= 0 ? Math.Abs(rcsThrottle.X) : 0f; // Left
    rcsAmount[5] = rcsThrottle.X >= 0 ? Math.Abs(rcsThrottle.X) : 0f; // Right
  }

  public static void DepleteMono()
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
  }

  public static void DrawAllRCS(SpriteBatch spriteBatch)
  {
    // [0]: rotate left
    // [1]: rotate right
    // [2]: up
    // [3]: down
    // [4]: left
    // [5]: right

    DrawRCS(spriteBatch, new Vector2(30, -30), (float)Math.PI * 0.5f, rcsAmount[1] + rcsAmount[4]); // Bottom right
    DrawRCS(spriteBatch, new Vector2(-33, -30), (float)-Math.PI * 0.5f, rcsAmount[0] + rcsAmount[5]); // Bottom left

    DrawRCS(spriteBatch, new Vector2(-47, -30), (float)Math.PI * 0.5f, rcsAmount[0] + rcsAmount[4]); // Top right
    DrawRCS(spriteBatch, new Vector2(44, -30), (float)-Math.PI * 0.5f, rcsAmount[1] + rcsAmount[5]); // Top left

    // Docking mode
    DrawRCS(spriteBatch, new Vector2(28, -32), (float)Math.PI, rcsAmount[2]); // Bottom left
    DrawRCS(spriteBatch, new Vector2(-31, -32), (float)Math.PI, rcsAmount[2]); // Bottom right

    DrawRCS(spriteBatch, new Vector2(28, 44), (float)Math.PI, rcsAmount[2]); // Top left
    DrawRCS(spriteBatch, new Vector2(-31, 44), (float)Math.PI, rcsAmount[2]); // Top right

    DrawRCS(spriteBatch, new Vector2(28, -47), 0f, rcsAmount[3]); // Top right retro
    DrawRCS(spriteBatch, new Vector2(-32, -47), 0f, rcsAmount[3]); // Top left retro

    DrawRCS(spriteBatch, new Vector2(28, 32), 0f, rcsAmount[3]); // Bottom right retro
    DrawRCS(spriteBatch, new Vector2(-32, 32), 0f, rcsAmount[3]); // Bottom left retro
  }

  private static void DrawRCS(SpriteBatch spriteBatch, Vector2 offsetOverride, float rotationOverride, float scaleOverride)
  {
    scaleOverride = Math.Clamp(scaleOverride, 0f, 1f);

    float thrustScale = scaleOverride * scale * 0.2f;

    Vector2 offset = new Vector2(0, scale) + (offsetOverride * scale * 2.5f);
    Vector2 origin = new Vector2(rcsSprite.texture.Width / 2, rcsSprite.texture.Width);

    float rotation = direction + rotationOverride;

    Vector2 rotatedOffset = new Vector2(
      offset.X * (float)Math.Cos(rotation) - offset.Y * (float)Math.Sin(rotation),
      offset.X * (float)Math.Sin(rotation) + offset.Y * (float)Math.Cos(rotation)
    );

    Vector2 adjustedPosition = GameState.position + rotatedOffset;

    spriteBatch.Draw(
      rcsSprite.texture,
      adjustedPosition,
      rcsSprite.SourceRectangle,
      Color.White * opacity(),
      rotation,
      origin,
      thrustScale,
      SpriteEffects.None,
      0f
    );
  }
}
