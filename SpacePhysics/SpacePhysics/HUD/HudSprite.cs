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
  private readonly Func<float> scale;

  public HudSprite(
    string textureName,
    Alignment alignment,
    Alignment origin,
    Func<Vector2> offset,
    Func<float> rotation,
    Func<Color> color,
    Func<float> scale,
    int layerIndex
  ) : base(false, alignment, layerIndex)
  {
    this.textureName = textureName;
    this.alignment = alignment;
    this.origin = origin;
    this.offset = offset;
    this.rotation = rotation;
    this.color = color;
    this.scale = scale;
  }

  public override void Load(ContentManager contentManager)
  {
    texture = contentManager.Load<Texture2D>(textureName);

    AlignRectangle();

    Update();
  }

  public override void Update()
  {
    if (Main.applyGraphics) AlignRectangle();

    rectangle.X = (int)initialPosition.X + (int)offset().X;
    rectangle.Y = (int)initialPosition.Y + (int)offset().Y;
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

  private void AlignRectangle()
  {
    // Need to update scale here as GetAlignmentRectangle needs the updated screenSize
    GameState.UpdateScale();

    rectangle = GetAlignmentRectangle(alignment);

    originVector = GetAlignmentVector(
      new Vector2(texture.Width, texture.Height),
      origin
    );

    initialPosition = new Vector2(
      rectangle.X,
      rectangle.Y
    );
  }

  private Rectangle GetAlignmentRectangle(Alignment alignment)
  {
    Vector2 alignmentVector = GetAlignmentVector(
      new Vector2(GameState.screenSize.X, GameState.screenSize.Y),
      alignment
    );

    return new(
      (int)alignmentVector.X,
      (int)alignmentVector.Y,
      (int)(texture.Width * scale()),
      (int)(texture.Height * scale())
    );
  }

  private Vector2 GetAlignmentVector(Vector2 size, Alignment alignment)
  {
    return alignment switch
    {
      Alignment.Center => new(
        size.X / 2,
        size.Y / 2
      ),
      Alignment.Left => new(
        0,
        size.Y / 2
      ),
      Alignment.Right => new(
        size.X,
        size.Y / 2
      ),
      Alignment.BottomLeft => new(
        0,
        size.Y
      ),
      Alignment.BottomRight => new(
        size.X,
        size.Y
      ),
      Alignment.BottomCenter => new(
        size.X / 2,
        size.Y
      ),
      Alignment.TopLeft => new(
        0,
        0
      ),
      Alignment.TopRight => new(
        size.X,
        0
      ),
      Alignment.TopCenter => new(
        size.X / 2,
        0
      ),
      _ => throw new ArgumentOutOfRangeException(
        nameof(alignment),
        alignment,
        null
      )
    };
  }
}
