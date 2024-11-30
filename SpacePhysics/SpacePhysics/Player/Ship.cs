using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.Sprites;

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
  public static float maxThrust;
  public static float altitude;

  private float dryMass;
  private float throttleTransitionSpeed;
  private float angularThrustFactor;

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
    angularThrustFactor = 0.001f;
    throttleTransition = false;
    throttleTransitionSpeed = 10f;
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

    Physics(gameTime);
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    spriteBatch.Draw(
      texture,
      GameState.position,
      null,
      Color.White * opacity(),
      GameState.direction,
      new Vector2(texture.Width / 2, texture.Height / 2),
      GameState.scale,
      SpriteEffects.None,
      0f
    );

    spriteBatch.Draw(
      thrustOverlay,
      GameState.position,
      null,
      Color.White * (GameState.throttle / 100 * opacity()),
      GameState.direction,
      new Vector2(thrustOverlay.Width / 2, thrustOverlay.Height / 2),
      GameState.scale,
      SpriteEffects.None,
      0f
    );
  }

  private void Physics(GameTime gameTime)
  {
    mass = dryMass + GameState.fuel;

    force.X = (float)Math.Cos(GameState.direction) * thrust;
    force.Y = (float)Math.Sin(GameState.direction) * thrust;

    acceleration = force / mass;

    GameState.velocity += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
    GameState.position += GameState.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
  }
}
