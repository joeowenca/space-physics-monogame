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
    offset = new Vector2(screenSize.X * 0.33f, 0);

    components.Add(new HudSprite(
      "Menu/icon",
      Alignment.Left,
      Alignment.Left,
      () => new Vector2(0, -screenSize.Y * 0.25f) + offset,
      () => 0f,
      () => Color.White * titleOpacity,
      scale * 6f,
      11
    ));

    components.Add(new HudText(
      "Fonts/title-font",
      () => "Space Physics",
      Alignment.Left,
      TextAlign.Left,
      () => new Vector2(screenSize.X * 0.25f, -screenSize.Y * 0.25f) + offset,
      () => Color.White * titleOpacity,
      scale * 4f,
      11
    ));

    components.Add(new HudText(
      "Fonts/light-font",
      () => "PRESS ANY KEY",
      Alignment.Left,
      TextAlign.Left,
      () => new Vector2(screenSize.X * 0.4f, screenSize.Y * 0.25f) + offset,
      () => Color.White * textOpacity,
      scale * 3f,
      11
    ));
  }

  public override void Update()
  {
    titleOpacity = ColorHelper.FadeOpacity(titleOpacity, -1f, 1f, 5f);
    textOpacity = ColorHelper.FadeOpacity(textOpacity, -2f, 0.9f, 5.5f);

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
