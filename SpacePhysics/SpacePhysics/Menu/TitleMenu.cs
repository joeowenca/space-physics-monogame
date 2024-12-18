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

  private float titleOpacity;
  private float textOpacity;

  public TitleMenu(
    bool allowInput,
    Alignment alignment,
    int layerIndex) : base(
      allowInput,
      alignment,
      layerIndex
    )
  {
    components.Add(new HudSprite(
      "Menu/icon",
      Alignment.TopLeft,
      Alignment.TopLeft,
      () => new Vector2(screenSize.X - (screenSize.X * 0.5f) - (screenSize.X * 0.22f), 0f) + offset,
      () => 0f,
      () => Color.White * titleOpacity,
      2f,
      11
    ));

    components.Add(new HudText(
      "Fonts/title-font",
      () => "Space Physics",
      Alignment.TopLeft,
      TextAlign.Left,
      () => new Vector2(screenSize.X - (screenSize.X * 0.5f), 0f) + offset,
      () => Color.White * titleOpacity,
      scale * 5f,
      11
    ));

    components.Add(new HudText(
      "Fonts/light-font",
      () => "PRESS ANY KEY",
      Alignment.TopLeft,
      TextAlign.Left,
      () => new Vector2(screenSize.X * 0.75f, screenSize.Y * 0.6f) + offset,
      () => Color.White * textOpacity,
      scale * 3f,
      11
    ));
  }

  public override void Update()
  {
    offset = MenuContainer.CenterMenu(components);

    titleOpacity = ColorHelper.FadeOpacity(titleOpacity, -0.75f, 1f, 4.5f);
    textOpacity = ColorHelper.FadeOpacity(textOpacity, -2f, 0.9f, 5f);

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
