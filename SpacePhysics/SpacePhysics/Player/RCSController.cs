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

  private static Func<float> opacity;

  public static float rcsThrust;
  public static float rcsDirection;

  public static float rcsLerpSpeed;

  private static float rcsThrustAmount;

  private static float[] rcsAmount = { 0f, 0f, 0f, 0f, 0f, 0f };
  private static float[] rcsAmountTarget = { 0f, 0f, 0f, 0f, 0f, 0f };

  private static bool rcsLeft;
  private static bool rcsRight;
  private static bool rcsUp;
  private static bool rcsDown;

  public RCSController(Func<float> opacity) : base()
  {
    RCSController.opacity = opacity;
  }

  public override void Initialize()
  {
    rcsForce = Vector2.Zero;
    rcsLerpSpeed = 30f;
    rcsLeft = false;
    rcsRight = false;
    rcsUp = false;
    rcsDown = false;
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

  public static void RotateRCS()
  {
    float rcsAngularThrust = 1 / mass * 4f * deltaTime * 250f * pitch;

    if ((maneuverMode || sas) && rcs)
    {
      angularVelocity += rcsAngularThrust;

      if (pitch <= 0)
      {
        rcsAmount[0] = Math.Abs(pitch);
      }
      else
      {
        rcsAmount[0] = 0f;
      }

      if (pitch >= 0)
      {
        rcsAmount[1] = Math.Abs(pitch);
      }
      else
      {
        rcsAmount[1] = 0f;
      }
    }
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

    rcsAmountTarget[2] = (rcsUp && mono > 0f) ? 1f : 0f;
    rcsAmountTarget[3] = (rcsDown && mono > 0f) ? 1f : 0f;

    rcsAmount[2] = MathHelper.Lerp(rcsAmount[2], rcsAmountTarget[2], deltaTime * rcsLerpSpeed);
    rcsAmount[3] = MathHelper.Lerp(rcsAmount[3], rcsAmountTarget[3], deltaTime * rcsLerpSpeed);

    rcsAmountTarget[4] = (rcsLeft && mono > 0f) ? 1f : 0f;
    rcsAmountTarget[5] = (rcsRight && mono > 0f) ? 1f : 0f;

    rcsAmount[4] = MathHelper.Lerp(rcsAmount[4], rcsAmountTarget[4], deltaTime * rcsLerpSpeed);
    rcsAmount[5] = MathHelper.Lerp(rcsAmount[5], rcsAmountTarget[5], deltaTime * rcsLerpSpeed);
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

    mono = Math.Clamp(mono, 0f, maxMono);
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

    Vector2 offset = new Vector2(0, scale) + offsetOverride;
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
