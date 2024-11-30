using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Sprites;
using static SpacePhysics.GameState;

namespace SpacePhysics.Player;

public class Ship : CustomGameComponent
{
  private InputManager input;
  private AnimatedSprite thrustSprite;
  private Texture2D thrustOverlay;

  private Vector2 acceleration;
  private Vector2 force;

  public static float mass;
  public static float thrust;
  public static float altitude;
  public static float fuelPercent;

  private float dryMass;
  private float maxThrust;
  private float maxFuel;
  private float engineEfficiency;
  private float deltaTime;

  public readonly Func<float> opacity;

  private bool throttleTransition;

  public Ship(Func<float> opacity, bool allowInput, Alignment alignment, int layerIndex) : base(alignment, layerIndex)
  {
    input = new(allowInput);
    this.opacity = opacity;
  }

  public override void Initialize()
  {
    acceleration = Vector2.Zero;
    force = Vector2.Zero;
    dryMass = 2500f;
    thrust = 0f;
    maxThrust = 115800f;
    maxFuel = fuel;
    engineEfficiency = 0.001f;
    deltaTime = 0;
  }

  public override void Load(ContentManager contentManager)
  {
    texture = contentManager.Load<Texture2D>("Ship");

    thrustOverlay = contentManager.Load<Texture2D>("thrustOverlay");

    thrustSprite = new AnimatedSprite(
      contentManager.Load<Texture2D>("thrustSheet"),
      4,
      1,
      60
    );
  }

  public override void Update(GameTime gameTime)
  {
    input.Update();
    thrustSprite.Update(gameTime, GameState.position);

    deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

    Physics();
    Throttle();
    Thrust();
    Stability();
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
      Color.White * (throttle / 100 * opacity()),
      direction,
      new Vector2(thrustOverlay.Width / 2, thrustOverlay.Height / 2),
      scale,
      SpriteEffects.None,
      0f
    );
  }

  private void Physics()
  {
    mass = dryMass + fuel;

    force.X = (float)Math.Cos(direction) * thrust;
    force.Y = (float)Math.Sin(direction) * thrust;

    acceleration = force / mass;

    velocity += acceleration * deltaTime;
    GameState.position += velocity * deltaTime;
  }

  private void Throttle()
  {
    if (input.ContinuousPress(Keys.LeftShift))
    {
      throttleTransition = false;
      targetThrottle += 0.01f * deltaTime;
    }

    if (input.ContinuousPress(Keys.LeftControl))
    {
      throttleTransition = false;
      targetThrottle -= 0.01f * deltaTime;
    }

    if (input.ContinuousPress(Keys.Z) && thrust != maxThrust)
    {
      targetThrottle = 1;
      throttleTransition = true;
    }

    if (input.ContinuousPress(Keys.X) && thrust != 0)
    {
      targetThrottle = 0;
      throttleTransition = true;
    }

    throttle = MathHelper.Lerp(throttle, targetThrottle, 0.1f);

    if (Math.Abs(throttle - targetThrottle) < 0.01f)
    {
      throttle = targetThrottle;
    }

    if (throttleTransition)
    {
      throttle = MathHelper.Lerp(
        throttle,
        targetThrottle,
        10f * deltaTime
      );

      if (Math.Abs(throttle - targetThrottle) < 0.01f)
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
      thrust = 0f;
    }

    fuel -= thrust * engineEfficiency;
    fuelPercent = fuel / maxFuel * 100;
  }

  private void Stability()
  {
    float angularThrust = throttle / mass;

    if (input.ContinuousPress(Keys.Right) || input.ContinuousPress(Keys.D))
    {
      angularVelocity += angularThrust * deltaTime;
    }

    if (input.ContinuousPress(Keys.Left) || input.ContinuousPress(Keys.A))
    {
      angularVelocity -= angularThrust * deltaTime;
    }

    if (sas &&
        !input.ContinuousPress(Keys.Right) &&
        !input.ContinuousPress(Keys.Left) &&
        !input.ContinuousPress(Keys.D) &&
        !input.ContinuousPress(Keys.A)
      )
    {
      angularVelocity = 0f;

      if (angularVelocity > 0.0001f)
      {
        angularVelocity -= angularThrust * deltaTime;
      }

      if (angularVelocity < 0.0001f)
      {
        angularVelocity += angularThrust * deltaTime;
      }
    }

    direction += angularVelocity;

    if (input.OnFirstFramePress(Keys.T))
    {
      sas = !sas;
    }
  }
}
