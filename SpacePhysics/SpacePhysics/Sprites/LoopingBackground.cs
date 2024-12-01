using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePhysics.Sprites;

public class LoopingBackground : CustomGameComponent
{
  private readonly Func<Color> color;
  private float parallaxFactor;

  public LoopingBackground(Texture2D texture, Func<Color> color, int layerIndex) : base(false, Alignment.TopLeft, layerIndex)
  {
    this.texture = texture;
    this.color = color;
    parallaxFactor = layerIndex * 0.071375f;
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    spriteBatch.Draw(texture, new Rectangle(0, 0, 0, 0), color());
  }
}