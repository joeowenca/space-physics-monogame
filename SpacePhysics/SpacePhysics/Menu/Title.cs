using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.HUD;
using SpacePhysics.Scenes.Start;
using static SpacePhysics.GameState;

namespace SpacePhysics.Menu;

public class Title : CustomGameComponent
{
  private Vector2 offset;

  private float opacity;

  public Title(
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
      () => Color.White * opacity,
      scale * 6f,
      11
    ));

    components.Add(new HudText(
      "Fonts/title-font",
      () => "Space Physics",
      Alignment.Left,
      TextAlign.Left,
      () => new Vector2(screenSize.X * 0.25f, -screenSize.Y * 0.25f) + offset,
      () => Color.White * opacity,
      scale * 4f,
      11
    ));
  }

  public override void Update()
  {
    if (state != State.TitleScreen && state != State.MainMenu)
    {
      opacity = ColorHelper.FadeOpacity(opacity, 1f, 0f, StartScene.transitionSpeed);
    }
    else
    {
      opacity = ColorHelper.FadeOpacity(opacity, -1f, 1f, 5f);
    }

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
