using System;
using Microsoft.Xna.Framework;

namespace SpacePhysics.Menu;

public class MenuItem : CustomGameComponent
{
  private readonly Func<bool> active;

  private Color color;
  private Color targetColor;

  public MenuItem(
    string label,
    Func<bool> active,
    Alignment alignment,
    Func<Vector2> offset,
    Func<float> opacity,
    int layerIndex
  ) : base(alignment, layerIndex)
  {
    this.active = active;
  }
}
