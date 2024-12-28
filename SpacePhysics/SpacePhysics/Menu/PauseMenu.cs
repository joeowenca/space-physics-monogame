using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Scenes.Start;
using SpacePhysics.HUD;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Menu;

public class PauseMenu : CustomGameComponent
{
  private Vector2 offset;
  private Vector2 baseOffset;

  private float opacity;

  private int menuItemsLength;
  private int activeMenu;

  public PauseMenu(
    bool allowInput,
    Alignment alignment,
    int layerIndex) : base(
      allowInput,
      alignment,
      layerIndex
    )
  {
    offset = new Vector2(menuOffsetXLeft, 0f);
    baseOffset = offset;

    components.Add(new HudText(
      "Fonts/title-font",
      () => "Paused",
      alignment,
      TextAlign.Left,
      () => new Vector2(-100, -400) + offset,
      () => Color.White * opacity,
      1.75f,
      11
    ));

    components.Add(new MenuItem(
        "Resume",
        () => activeMenu == 1,
        alignment,
        () => new Vector2(0f, 0f) + offset,
        () => opacity,
        11
      ));

    components.Add(new MenuItem(
      "Settings",
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSizeY) + offset,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Main Menu",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSizeY * 2f) + offset,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Quit",
      () => activeMenu == 4,
      alignment,
      () => new Vector2(0f, menuSizeY * 3f) + offset,
      () => opacity,
      11
    ));
  }

  public override void Initialize()
  {
    menuItemsLength = 4;
    activeMenu = 1;

    base.Initialize();
  }

  public override void Update()
  {
    if (state != State.Pause)
    {
      if (opacity > 0)
        opacity = ColorHelper.FadeOpacity(opacity, 1f, 0f, 0.2f);

      if (opacity <= 0.1f)
        activeMenu = 1;
    }
    else
    {
      opacity = ColorHelper.FadeOpacity(opacity, 0f, 1f, 0.2f);

      if (input.OnFirstFrameKeyPress(Keys.Down))
        activeMenu++;

      if (input.OnFirstFrameKeyPress(Keys.Up))
        activeMenu--;

      if (activeMenu == 1 && input.OnFirstFrameKeyPress(Keys.Enter))
      {
        state = State.Play;
      }

      if (activeMenu == 2 && input.OnFirstFrameKeyPress(Keys.Enter))
      {
        state = State.Play;
      }

      if (activeMenu == 3 && input.OnFirstFrameKeyPress(Keys.Enter))
      {
        state = State.TitleScreen;
      }

      if (activeMenu == 4 && input.OnFirstFrameKeyPress(Keys.Enter))
        quit = true;
    }

    activeMenu = Math.Clamp(activeMenu, 1, menuItemsLength);

    offset.X = baseOffset.X + menuOffsetFactor;

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
