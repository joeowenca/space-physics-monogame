using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePhysics.HUD;

public class HudText : CustomGameComponent
{
  private SpriteFont font;
  private Vector2 initialPosition;

  private string fontName;
  private readonly Func<string> value;
  private TextAlign textAlign;
  private readonly Func<Vector2> offset;
  private readonly Func<Color> color;
  private float scale;

  public HudText(
    string fontName,
    Func<string> value,
    Alignment alignment,
    TextAlign textAlign,
    Func<Vector2> offset,
    Func<Color> color,
    float scale,
    int layerIndex
  ) : base(alignment, layerIndex)
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
  }

  public override void Update(GameTime gameTime)
  {
    width = (int)(font.MeasureString(value()).X * scale);
    height = (int)(font.MeasureString(value()).Y * scale);
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
        scale,
        SpriteEffects.None,
        0f
      );
    }
  }
}
