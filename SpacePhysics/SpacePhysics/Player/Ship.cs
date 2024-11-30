using System;
using Microsoft.Xna.Framework;
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
  public static float altitude;
  public static float fuelPercent;

  private float dryMass;
  private float maxThrust;
  private float minThrust;
  private float thrustTransitionSpeed;
  private float maxFuel;
  private float angularThrustFactor;

  public readonly Func<float> opacity;

  private bool throttleTransition;

  public Ship(Func<float> opacity, bool allowInput, Alignment alignment, int layerIndex) : base(alignment, layerIndex)
  {
    input = new(allowInput);
    this.opacity = opacity;
  }
}
