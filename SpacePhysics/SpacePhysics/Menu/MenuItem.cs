using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.HUD;

namespace SpacePhysics.Menu;

public class MenuItem : CustomGameComponent
{
  private readonly Func<bool> active;

  private Color color;
  private Color targetColor;
  private Color defaultColor;
  private Color highlightColor;

  private CustomGameComponent component;

  public MenuItem(
    string label,
    Func<bool> active,
    Alignment alignment,
    Func<Vector2> offset,
    Func<float> opacity,
    int layerIndex
  ) : base(false, alignment, layerIndex)
  {
    this.active = active;

    component = new HudText(
      "Fonts/text-font",
      () => label,
      alignment,
      TextAlign.Left,
      () => offset(),
      () => color * opacity(),
      1f,
      11
    );
  }

  public override void Initialize()
  {
    defaultColor = Color.White * 0.75f;
    highlightColor = Color.Gold;
    color = defaultColor;
    targetColor = color;

    component.Initialize();
  }

  public override void Load(ContentManager contentManager)
  {
    component.Load(contentManager);
  }

  public override void Update()
  {
    position = component.position;
    width = component.width;
    height = component.height;

    targetColor = defaultColor;

    if (active()) targetColor = highlightColor;

    color = ColorHelper.Lerp(color, targetColor, 0.3f);

    component.Update();
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    component.Draw(spriteBatch);
  }
}
