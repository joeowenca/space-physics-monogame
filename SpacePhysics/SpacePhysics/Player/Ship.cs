using System;
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

  private Vector2 acceleration;
  private Vector2 force;

  public static float mass;
  public static float thrust;
  public static float thrustAmount;
  public static float altitude;
  public static float fuelPercent;

  private float dryMass;
  private float maxThrust;
  private float maxFuel;
  private float engineEfficiency;

  public readonly Func<float> opacity;

  private bool throttleTransition;

  public Ship(Func<float> opacity, bool allowInput, Alignment alignment, int layerIndex) : base(allowInput, alignment, layerIndex)
  {
    this.opacity = opacity;
  }

  public override void Initialize()
  {
    acceleration = Vector2.Zero;
    force = Vector2.Zero;
    dryMass = 2500f;
    thrust = 0f;
    maxThrust = 115800;
    maxFuel = fuel;
    engineEfficiency = 0.0000001f;
  }

  public override void Load(ContentManager contentManager)
  {
    texture = contentManager.Load<Texture2D>("Player/ship");

    thrustOverlay = contentManager.Load<Texture2D>("Player/thrust-overlay");

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
      1f / 15f
    );
  }

  public override void Update()
  {
    if (state != State.Pause)
    {
      thrustSprite.Update(GameState.position);
      rcsSprite.Update(GameState.position);

      Physics();
      Throttle();
      Thrust();
      Stability();
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

    DrawRCS(spriteBatch, new Vector2(32, -30), (float)Math.PI * 0.5f);
    DrawRCS(spriteBatch, new Vector2(-32, -30), (float)-Math.PI * 0.5f);

    DrawRCS(spriteBatch, new Vector2(-45, -30), (float)Math.PI * 0.5f);
    DrawRCS(spriteBatch, new Vector2(45, -30), (float)-Math.PI * 0.5f);
  }

  private void Physics()
  {
    mass = dryMass + fuel;

    force.X = (float)Math.Cos(direction - (float)(Math.PI * 0.5f)) * thrust;
    force.Y = (float)Math.Sin(direction - (float)(Math.PI * 0.5f)) * thrust;

    acceleration = force / mass;

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
    fuelPercent = fuel / maxFuel * 100;
    fuel = Math.Clamp(fuel, 0f, maxFuel);

    thrustAmount = thrust / maxThrust;
  }

  private void Stability()
  {
    float angularThrust = thrustAmount / mass * deltaTime * 250f;

    float rcsThrust = 1 / mass * 4f * deltaTime * 250f;

    if (input.ContinuousPress(Keys.Right) || input.ContinuousPress(Keys.D))
    {
      angularVelocity += angularThrust;
      if (rcs) angularVelocity += rcsThrust;
    }

    if (input.ContinuousPress(Keys.Left) || input.ContinuousPress(Keys.A))
    {
      angularVelocity -= angularThrust;
      if (rcs) angularVelocity -= rcsThrust;
    }

    if (sas &&
        !input.ContinuousPress(Keys.Right) &&
        !input.ContinuousPress(Keys.Left) &&
        !input.ContinuousPress(Keys.D) &&
        !input.ContinuousPress(Keys.A)
      )
    {
      if (angularVelocity > 0f)
      {
        angularVelocity -= angularThrust;
        if (rcs) angularVelocity -= rcsThrust;
      }

      if (angularVelocity < 0f)
      {
        angularVelocity += angularThrust;
        if (rcs) angularVelocity += rcsThrust;
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
  }

  private void DrawThrust(SpriteBatch spriteBatch)
  {
    float thrustScale = thrustAmount * scale * 0.9f;

    Vector2 origin = new Vector2(thrustSprite.texture.Width / 2, 90);
    Vector2 offset = new Vector2(2, 220f * scale);

    float rotation = direction;

    Vector2 rotatedOffset = new Vector2(
      offset.X * (float)Math.Cos(rotation) - offset.Y * (float)Math.Sin(rotation),
      offset.X * (float)Math.Sin(rotation) + offset.Y * (float)Math.Cos(rotation)
    );

    Vector2 adjustedPosition = thrustSprite.position + rotatedOffset;

    spriteBatch.Draw(
      thrustSprite.texture,
      adjustedPosition,
      thrustSprite.SourceRectangle,
      Color.White,
      rotation,
      origin,
      thrustScale,
      SpriteEffects.None,
      0f
    );
  }

  private void DrawRCS(SpriteBatch spriteBatch, Vector2 offsetOverride, float rotationOverride)
  {
    float thrustScale = thrustAmount * scale * 0.3f;

    Vector2 offset = new Vector2(0, scale) + offsetOverride;
    Vector2 origin = new Vector2(rcsSprite.texture.Width / 2, rcsSprite.texture.Width);

    float rotation = direction + rotationOverride;

    Vector2 rotatedOffset = new Vector2(
      offset.X * (float)Math.Cos(rotation) - offset.Y * (float)Math.Sin(rotation),
      offset.X * (float)Math.Sin(rotation) + offset.Y * (float)Math.Cos(rotation)
    );

    Vector2 adjustedPosition = rcsSprite.position + rotatedOffset;

    spriteBatch.Draw(
      rcsSprite.texture,
      adjustedPosition,
      rcsSprite.SourceRectangle,
      Color.White,
      rotation,
      origin,
      thrustScale,
      SpriteEffects.None,
      0f
    );
  }
}
