using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Scenes.Start;
using SpacePhysics.HUD;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Menu;

public class SettingsMenu : CustomGameComponent
{
  private Vector2 offset;
  private Vector2 baseOffset;
  private Vector2 menuOffsetOverride;

  private float opacity;

  private int menuItemsLength;
  private int activeMenu;

  public SettingsMenu(
    bool allowInput,
    Alignment alignment,
    int layerIndex) : base(
      allowInput,
      alignment,
      layerIndex
    )
  {
    offset = new Vector2(menuOffsetXRight, -200f);
    baseOffset = offset;

    components.Add(new HudText(
      "Fonts/title-font",
      () => "Settings",
      alignment,
      TextAlign.Left,
      () => new Vector2(-100, -400) + offset,
      () => Color.White * opacity,
      1.75f,
      11
    ));

    components.Add(new MenuItem(
        "Audio",
        () => activeMenu == 1,
        alignment,
        () => new Vector2(0f, 0f) + menuOffsetOverride,
        () => opacity,
        11
      ));

    components.Add(new MenuItem(
      "Graphics",
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Gameplay",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSizeY * 2f) + menuOffsetOverride,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Controls",
      () => activeMenu == 4,
      alignment,
      () => new Vector2(0f, menuSizeY * 3f) + menuOffsetOverride,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Back",
      () => activeMenu == 5,
      alignment,
      () => new Vector2(0f, menuSizeY * 4.5f) + menuOffsetOverride,
      () => opacity,
      11
    ));
  }

  public override void Initialize()
  {
    menuItemsLength = 5;
    activeMenu = 1;

    base.Initialize();
  }

  public override void Update()
  {
    TransitionState();

    UpdateMenu();

    UpdateOffset();

    base.Update();
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    foreach (var component in components)
    {
      component.Draw(spriteBatch);
    }
  }

  private void TransitionState()
  {
    if (state != State.Settings)
    {
      if (opacity > 0)
        opacity = ColorHelper.FadeOpacity(opacity, 1f, 0f, StartScene.transitionSpeed);

      if (opacity <= 0.1f)
        activeMenu = 1;
    }
    else
    {
      opacity = ColorHelper.FadeOpacity(opacity, 0f, 1f, StartScene.transitionSpeed);
    }
  }

  private void UpdateMenu()
  {
    if (state == State.Settings)
    {
      if (input.OnFirstFrameKeyPress(Keys.Down))
        activeMenu++;

      if (input.OnFirstFrameKeyPress(Keys.Up))
        activeMenu--;

      if (activeMenu == 5 && input.OnFirstFrameKeyPress(Keys.Enter))
        state = State.MainMenu;
    }

    activeMenu = Math.Clamp(activeMenu, 1, menuItemsLength);
  }

  private void UpdateOffset()
  {
    offset.X = baseOffset.X + menuOffset.X * 3f;
    menuOffsetOverride.X = baseOffset.X - 150 + menuOffsetFactor;
    menuOffsetOverride.Y = -200f;
  }
}
