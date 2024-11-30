using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePhysics.HUD;

public class HudSprite : CustomGameComponent
{
  private Rectangle rectangle;
  private Vector2 initialPosition;
  private Vector2 originVector;

  private string textureName;
  private readonly Alignment origin;
  private readonly Func<Vector2> offset;
  private readonly Func<float> rotation;
  private readonly Func<Color> color;
  private float scale;

  public HudSprite(
    string textureName,
    Alignment alignment,
    Alignment origin,
    Func<Vector2> offset,
    Func<float> rotation,
    Func<Color> color,
    float scale,
    int layerIndex
  ) : base(alignment, layerIndex)
  {
    this.textureName = textureName;
    this.alignment = alignment;
    this.origin = origin;
    this.offset = offset;
    this.rotation = rotation;
    this.color = color;
  }

  public override void Load(ContentManager contentManager)
  {
    texture = contentManager.Load<Texture2D>(textureName);
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    bool hidden = color().R < 1 && color().G < 1 && color().B < 1 && color().A < 1;

    if (!hidden)
    {
      spriteBatch.Draw(
        texture,
        rectangle,
        null,
        color(),
        rotation(),
        originVector,
        SpriteEffects.None,
        0f
      );
    }
  }
}
