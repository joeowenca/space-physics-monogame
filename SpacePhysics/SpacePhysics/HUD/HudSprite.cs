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

  private Rectangle GetAlignment(Alignment alignment)
  {
    int screenWidth = (int)Main.screenSize.X;
    int screenHeight = (int)Main.screenSize.Y;

    Vector2 alignmentVector = alignment switch
    {
      Alignment.Center => new(
        screenWidth / 2,
        screenHeight / 2
      ),
      Alignment.Left => new(
        0,
        screenHeight / 2
      ),
      Alignment.Right => new(
        screenWidth,
        screenHeight / 2
      ),
      Alignment.BottomLeft => new(
        0,
        screenHeight
      ),
      Alignment.BottomRight => new(
        screenWidth,
        screenHeight
      ),
      Alignment.BottomCenter => new(
        screenWidth / 2,
        screenHeight
      ),
      Alignment.TopLeft => new(
        0,
        0
      ),
      Alignment.TopRight => new(
        screenWidth,
        0
      ),
      Alignment.TopCenter => new(
        screenWidth / 2,
        0
      ),
      _ => throw new ArgumentOutOfRangeException(
        nameof(alignment),
        alignment,
        null
      )
    };

    return new(
      (int)alignmentVector.X,
      (int)alignmentVector.Y,
      (int)(texture.Width * scale),
      (int)(texture.Height * scale)
    );
  }
}
