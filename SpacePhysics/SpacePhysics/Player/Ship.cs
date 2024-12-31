using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Sprites;
using static SpacePhysics.GameState;
using static SpacePhysics.Player.SASController;
using static SpacePhysics.Player.RCSController;

namespace SpacePhysics.Player;

public class Ship : CustomGameComponent
{
  private AnimatedSprite thrustSprite;
  private Texture2D thrustOverlay;
  private Texture2D thrustLensFlare;

  public static Vector2 acceleration;
  private Vector2 force;

  public static Vector2 lensFlareRotatedOffset;

  public static float mass;
  public static float thrust;
  public static float forwardThrust;
  public static float thrustAmount;
  public static float thrustDirection;
  public static float altitude;
  public static float dryMass;
  public static float pitch;
  public static float targetPitch;

  private float maxThrust;
  private float engineEfficiency;

  public readonly Func<float> opacity;

  public Ship(Func<float> opacity, bool allowInput, Alignment alignment, int layerIndex) : base(allowInput, alignment, layerIndex)
  {
    this.opacity = opacity;

    components.Add(new RCSController(opacity));
  }

  public override void Initialize()
  {
    acceleration = Vector2.Zero;
    force = Vector2.Zero;
    thrust = 0f;
    forwardThrust = 0f;
    dryMass = 2200;
    pitch = 0f;
    maxThrust = 600000f;
    engineEfficiency = 0.00005f;

    base.Initialize();
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

    base.Load(contentManager);
  }

  public override void Update()
  {
    if (state != State.Pause)
    {
      thrustSprite.Animate();

      Physics();
      Thrust();

      if (electricity > 0)
      {
        Throttle();
        AdjustPitch();
        ToggleSAS(input);
        Stabilize(input);
        LockOnTarget(input);
        SetSASMode(input);
        ToggleRCS(input);
        ToggleRCSMode(input);

        if (mono > 0)
        {
          RotateWithRCS();
          MoveWithRCS(input);
        }
      }

      pitch = MathHelper.Lerp(pitch, targetPitch, deltaTime * 20f);

      if (pitch > 0.999f) pitch = 1f;
      if (pitch < -0.999f) pitch = -1f;

      pitch = Math.Clamp(pitch, -1f, 1f);

      base.Update();
    }

    input.ControllerRumble(state != State.Play ? 0f : thrustAmount * 0.5f);
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    DrawThrust(spriteBatch);

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

    DrawAllRCS(spriteBatch);

    DrawLensFlare(spriteBatch);
  }

  private void Physics()
  {
    mass = dryMass + fuel + mono;

    force.X = (float)Math.Cos(direction - (float)(Math.PI * 0.5f)) * forwardThrust;
    force.Y = (float)Math.Sin(direction - (float)(Math.PI * 0.5f)) * forwardThrust;

    rcsForce.X = (float)Math.Cos(direction + rcsDirection - (float)(Math.PI * 0.5f)) * rcsThrust;
    rcsForce.Y = (float)Math.Sin(direction + rcsDirection - (float)(Math.PI * 0.5f)) * rcsThrust;

    acceleration = (force + rcsForce) / mass;

    velocity += acceleration * deltaTime;
    GameState.position += velocity * deltaTime;

    direction += angularVelocity * deltaTime;
  }

  private void Thrust()
  {
    if (fuel > 0)
    {
      thrust = maxThrust * throttle;
    }
    else
    {
      thrust = MathHelper.Lerp(thrust, 0f, deltaTime * 30f);
    }


    thrustDirection = pitch * -0.2f;

    forwardThrust = thrust * (float)Math.Cos(thrustDirection);

    thrustAmount = thrust / maxThrust;

    angularVelocity += -thrustDirection * thrustAmount * deltaTime * 0.25f;

    fuel -= thrust * engineEfficiency * deltaTime;
  }

  private void Throttle()
  {
    targetThrottle += input.AdjustThrottle();

    if
    (
      Math.Abs(input.AdjustThrottle()) > 0
      && throttle != 0f
      && throttle != 1f
    )
    {
      electricity -= deltaTime * input.AdjustThrottle();
    }

    throttle = Math.Clamp(throttle, 0f, 1f);
    targetThrottle = Math.Clamp(targetThrottle, 0f, 1f);
    throttle = MathHelper.Lerp(throttle, targetThrottle, deltaTime * 10f);

    if (Math.Abs(throttle - targetThrottle) < 0.00001f)
    {
      throttle = targetThrottle;
    }
  }

  private void AdjustPitch()
  {
    if (maneuverMode)
    {
      targetPitch = 0f;

      if (Math.Abs(pitch - targetPitch) < 0.01f) pitch = 0f;

      if (pitch > 0.99f) pitch = 1f;

      if (pitch < -0.99f) pitch = -1f;

      targetPitch = input.AdjustPitch();
      electricity -= deltaTime * Math.Abs(input.AdjustPitch());
    }
  }

  private void DrawThrust(SpriteBatch spriteBatch)
  {
    float thrustScale = thrustAmount * scale * 0.8f;

    Vector2 origin = new Vector2(thrustSprite.texture.Width / 2, 90);
    Vector2 offset = new Vector2(0f + (pitch * 7.5f), 220f * scale);

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
      rotation + thrustDirection,
      origin,
      thrustScale,
      SpriteEffects.None,
      0f
    );
  }

  private void DrawLensFlare(SpriteBatch spriteBatch)
  {
    float thrustLensFlareScale = scale * thrustAmount;

    float lensFlareOffset = (Camera.Camera.changeCamera ? 210f : 235f) * scale;

    lensFlareRotatedOffset = new Vector2(
      -(float)Math.Sin(direction + (pitch * -0.1f)) * lensFlareOffset,
      (float)Math.Cos(direction + (pitch * -0.1f)) * lensFlareOffset
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
}
