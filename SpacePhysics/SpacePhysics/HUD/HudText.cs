using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePhysics.HUD;

public class HudText : CustomGameComponent
{
  private SpriteFont font;

  private string fontName;
  private readonly Func<string> value;
  private TextAlign textAlign;
  private readonly Func<Vector2> offset;
  private readonly Func<Color> color;
  private readonly Func<float> scale;

  public HudText(
    string fontName,
    Func<string> value,
    Alignment alignment,
    TextAlign textAlign,
    Func<Vector2> offset,
    Func<Color> color,
    Func<float> scale,
    int layerIndex
  ) : base(false, alignment, layerIndex)
  {
    this.fontName = fontName;
    this.value = value;
    this.alignment = alignment;
    this.textAlign = textAlign;
    this.offset = offset;
    this.color = color;
    this.scale = scale;
  }

  public override void Load(ContentManager contentManager)
  {
    font = contentManager.Load<SpriteFont>(fontName);

    Update();
  }

  public override void Update()
  {
    width = font.MeasureString(value()).X * scale();
    height = font.MeasureString(value()).Y * scale();

    position = offset();

    position += GetAlignment(alignment);
    position += GetTextAlign(textAlign);
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    bool hidden = color().R < 1 && color().G < 1 && color().B < 1 && color().A < 1;

    if (!hidden)
    {
      spriteBatch.DrawString(
        font,
        value(),
        position,
        color(),
        0f,
        Vector2.Zero,
        scale(),
        SpriteEffects.None,
        0f
      );
    }
  }

  private Vector2 GetAlignment(Alignment alignment)
  {
    float screenWidth = GameState.screenSize.X;
    float screenHeight = GameState.screenSize.Y;

    return alignment switch
    {
      Alignment.Center => new Vector2(
          screenWidth / 2,
          (screenHeight - height) / 2
      ),
      Alignment.Left => new Vector2(
          0,
          (screenHeight - height) / 2
      ),
      Alignment.Right => new Vector2(
          screenWidth,
          (screenHeight - height) / 2
      ),
      Alignment.BottomLeft => new Vector2(
          0,
          screenHeight - height
      ),
      Alignment.BottomRight => new Vector2(
          screenWidth,
          screenHeight - height
      ),
      Alignment.BottomCenter => new Vector2(
          screenWidth / 2,
          screenHeight - height
      ),
      Alignment.TopLeft => new Vector2(
          0,
          0
      ),
      Alignment.TopRight => new Vector2(
          screenWidth,
          0
      ),
      Alignment.TopCenter => new Vector2(
          screenWidth / 2,
          0
      ),
      _ => throw new ArgumentOutOfRangeException(
          nameof(alignment),
          alignment,
          null
      )
    };
  }

  private Vector2 GetTextAlign(TextAlign textAlign)
  {
    return textAlign switch
    {
      TextAlign.Center => new Vector2(
        -width / 2,
        0
      ),
      TextAlign.Left => new Vector2(
        0,
        0
      ),
      TextAlign.Right => new Vector2(
        -width,
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
