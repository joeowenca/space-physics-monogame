using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Sprites;
using static SpacePhysics.GameState;

namespace SpacePhysics.Player;

public class Ship : CustomGameComponent
{
  private AnimatedSprite thrustSprite;
  private AnimatedSprite rcsSprite;
  private Texture2D thrustOverlay;
  private Texture2D thrustLensFlare;

  private Vector2 acceleration;
  private Vector2 force;
  private Vector2 rcsForce;

  public static Vector2 lensFlareRotatedOffset;

  public static float mass;
  public static float thrust;
  public static float thrustAmount;
  public static float rcsThrust;
  public static float rcsThrustAmount;
  public static float rcsDirection;
  public static float rcsLerpSpeed;
  public static float altitude;
  public static float dryMass;

  private float maxThrust;
  private float engineEfficiency;

  private float[] rcsAmount = { 0f, 0f, 0f, 0f, 0f, 0f };
  private float[] rcsAmountTarget = { 0f, 0f, 0f, 0f, 0f, 0f };

  public readonly Func<float> opacity;

  private bool throttleTransition;
  private bool rcsRotateLeft;
  private bool rcsRotateRight;
  private bool rcsLeft;
  private bool rcsRight;
  private bool rcsUp;
  private bool rcsDown;

  public Ship(Func<float> opacity, bool allowInput, Alignment alignment, int layerIndex) : base(allowInput, alignment, layerIndex)
  {
    this.opacity = opacity;
  }

  public override void Initialize()
  {
    acceleration = Vector2.Zero;
    force = Vector2.Zero;
    rcsForce = Vector2.Zero;
    rcsThrustAmount = 100000f;
    dryMass = 2500;
    thrust = 0f;
    maxThrust = 600000f;
    engineEfficiency = 0.00000001f;
    rcsLerpSpeed = 30f;
  }

  public override void Load(ContentManager contentManager)
  {
    texture = contentManager.Load<Texture2D>("Player/ship");

    thrustOverlay = contentManager.Load<Texture2D>("Player/thrust-overlay");

    thrustLensFlare = contentManager.Load<Texture2D>("Player/thrust-lens-flare");

    thrustSprite = new AnimatedSprite(
      contentManager.Load<Texture2D>("Player/thrust-sheet"),
      4,
      1,
      1f / 15f
    );

    rcsSprite = new AnimatedSprite(
      contentManager.Load<Texture2D>("Player/rcs-sheet"),
      4,
      1,
      1f / 24f
    );
  }

  public override void Update()
  {
    if (state != State.Pause)
    {
      thrustSprite.Animate();
      rcsSprite.Animate();

      Physics();
      Throttle();
      Thrust();
      Stability();
      Docking();
      RCS();
    }

    base.Update();
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    spriteBatch.Draw(
      texture,
      GameState.position,
      null,
      Color.White * opacity(),
      direction,
      new Vector2(texture.Width / 2, texture.Height / 2),
      scale,
      SpriteEffects.None,
      0f
    );

    spriteBatch.Draw(
      thrustOverlay,
      GameState.position,
      null,
      Color.White * thrustAmount,
      direction,
      new Vector2(thrustOverlay.Width / 2, thrustOverlay.Height / 2),
      scale,
      SpriteEffects.None,
      0f
    );

    DrawThrust(spriteBatch);

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

    float thrustLensFlareScale = scale * thrustAmount;

    float lensFlareOffset = (Camera.Camera.changeCamera ? 195f : 220f) * scale;

    lensFlareRotatedOffset = new Vector2(
      -(float)Math.Sin(direction) * lensFlareOffset,
      (float)Math.Cos(direction) * lensFlareOffset
    );

    spriteBatch.Draw(
      thrustLensFlare,
      GameState.position + lensFlareRotatedOffset + new Vector2(0f, Camera.Camera.changeCamera ? 0f : -10f),
      null,
      new Color(255, 255, 255, 0) * thrustAmount * 0.25f * opacity(),
      Camera.Camera.changeCamera ? direction : 0f,
      new Vector2(thrustLensFlare.Width / 2, thrustLensFlare.Height / 2),
      thrustLensFlareScale,
      SpriteEffects.None,
      0f
    );
  }

  private void Physics()
  {
    mass = dryMass + fuel;

    force.X = (float)Math.Cos(direction - (float)(Math.PI * 0.5f)) * thrust;
    force.Y = (float)Math.Sin(direction - (float)(Math.PI * 0.5f)) * thrust;

    rcsForce.X = (float)Math.Cos(direction + rcsDirection - (float)(Math.PI * 0.5f)) * rcsThrust;
    rcsForce.Y = (float)Math.Sin(direction + rcsDirection - (float)(Math.PI * 0.5f)) * rcsThrust;

    acceleration = (force + rcsForce) / mass;

    velocity += acceleration * deltaTime;
    GameState.position += velocity * deltaTime;
  }

  private void Throttle()
  {
    float throttleChangeSpeed = deltaTime * 0.4f;

    if (input.ContinuousPress(Keys.LeftShift))
    {
      throttleTransition = false;
      targetThrottle += throttleChangeSpeed;
    }

    if (input.ContinuousPress(Keys.LeftControl))
    {
      throttleTransition = false;
      targetThrottle -= throttleChangeSpeed;
    }

    if (input.ContinuousPress(Keys.Z))
    {
      targetThrottle = 1;
      throttleTransition = true;
    }

    if (input.ContinuousPress(Keys.X))
    {
      targetThrottle = 0;
      throttleTransition = true;
    }

    throttle = MathHelper.Lerp(throttle, targetThrottle, deltaTime * 10f);

    if (Math.Abs(throttle - targetThrottle) < 0.00001f)
    {
      throttle = targetThrottle;
    }

    if (throttleTransition)
    {
      throttle = MathHelper.Lerp(
        throttle,
        targetThrottle,
        deltaTime * 10f
      );

      if (Math.Abs(throttle - targetThrottle) < 0.00001f)
      {
        throttle = targetThrottle;
        throttleTransition = false;
      }
    }

    throttle = Math.Clamp(throttle, 0f, 1f);
    targetThrottle = Math.Clamp(targetThrottle, 0f, 1f);
  }

  private void Thrust()
  {
    if (fuel > 0)
    {
      thrust = maxThrust * throttle;
    }
    else
    {
      thrust = MathHelper.Lerp(thrust, 0f, deltaTime * 15f);
    }

    fuel -= thrust * engineEfficiency * deltaTime * 5000f;
    fuel = Math.Clamp(fuel, 0f, maxFuel);

    thrustAmount = thrust / maxThrust;
  }

  private void Stability()
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

  private void Docking()
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
        rcsThrust = rcsThrustAmount;
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
        rcsThrust = rcsThrustAmount;
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

  private void RCS()
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

  private void DrawThrust(SpriteBatch spriteBatch)
  {
    float thrustScale = thrustAmount * scale * 0.9f;

    Vector2 origin = new Vector2(thrustSprite.texture.Width / 2, 90);
    Vector2 offset = new Vector2(0f, 220f * scale);

    float rotation = direction;

    Vector2 rotatedOffset = new Vector2(
      offset.X * (float)Math.Cos(rotation) - offset.Y * (float)Math.Sin(rotation),
      offset.X * (float)Math.Sin(rotation) + offset.Y * (float)Math.Cos(rotation)
    );

    Vector2 adjustedPosition = GameState.position + rotatedOffset;

    spriteBatch.Draw(
      thrustSprite.texture,
      adjustedPosition,
      thrustSprite.SourceRectangle,
      Color.White * opacity(),
      rotation,
      origin,
      thrustScale,
      SpriteEffects.None,
      0f
    );
  }

  private void DrawRCS(SpriteBatch spriteBatch, Vector2 offsetOverride, float rotationOverride, float scaleOverride)
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
