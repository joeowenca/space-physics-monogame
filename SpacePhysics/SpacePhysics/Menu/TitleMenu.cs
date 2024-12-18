using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.HUD;
using static SpacePhysics.GameState;

namespace SpacePhysics.Menu;

public class TitleMenu : CustomGameComponent
{
  private Vector2 offset;

  public TitleMenu(
    bool allowInput,
    Alignment alignment,
    Func<float> opacity,
    int layerIndex) : base(
      allowInput,
      alignment,
      layerIndex
    )
  {
    components.Add(new HudText(
      "Fonts/title-font",
      () => "Space Physics",
      Alignment.TopLeft,
      TextAlign.Center,
      () => new Vector2(screenSize.X, 0f) + offset,
      () => Color.White * opacity(),
      scale * 5f,
      11
    ));

    components.Add(new HudText(
      "Fonts/light-font",
      () => "PRESS ANY KEY",
      Alignment.TopLeft,
      TextAlign.Center,
      () => new Vector2(screenSize.X, screenSize.Y * 0.5f) + offset,
      () => Color.White * opacity(),
      scale * 3f,
      11
    ));
  }

  public override void Update()
  {
    offset = MenuContainer.CenterMenu(components);

    base.Update();
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    foreach (var component in components)
    {
      component.Draw(spriteBatch);
    }
  }
}
