using System;
using System.Drawing;
using System.Numerics;
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
}
