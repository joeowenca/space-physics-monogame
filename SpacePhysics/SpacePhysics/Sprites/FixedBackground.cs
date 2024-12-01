using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePhysics.Sprites;

public class FixedBackground : CustomGameComponent
{
  private Color color;

  public FixedBackground(Texture2D texture, Vector2 position, Color color, Alignment alignment, int layerIndex) : base(false, alignment, layerIndex)
  {
    this.texture = texture;
    this.position = position;
    this.color = color;
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    spriteBatch.Draw(texture, position, color);
  }
}
