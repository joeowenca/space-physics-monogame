using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Scenes.Start;
using SpacePhysics.HUD;
using static SpacePhysics.GameState;

namespace SpacePhysics.Menu;

public class SettingsMenu : CustomGameComponent
{
  private Vector2 offset;

  private float opacity;

  private int menuItemsLength;
  private int activeMenu;

  public static bool quit;

  public SettingsMenu(
    bool allowInput,
    Alignment alignment,
    int layerIndex) : base(
      allowInput,
      alignment,
      layerIndex
    )
  {
    float padding = 0.17f;
    float menuSize = 1000f * padding;
    offset = new Vector2(-2600f, -200f);

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
        () => new Vector2(0f, 0f) + offset,
        () => opacity,
        11
      ));

    components.Add(new MenuItem(
      "Graphics",
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSize) + offset,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Gameplay",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSize * 2f) + offset,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Controls",
      () => activeMenu == 4,
      alignment,
      () => new Vector2(0f, menuSize * 3f) + offset,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Back",
      () => activeMenu == 5,
      alignment,
      () => new Vector2(0f, menuSize * 4.5f) + offset,
      () => opacity,
      11
    ));
  }

  public override void Initialize()
  {
    menuItemsLength = 5;
    activeMenu = 1;
    quit = false;

    base.Initialize();
  }

  public override void Update()
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

      if (input.OnFirstFramePress(Keys.Down))
        activeMenu++;

      if (input.OnFirstFramePress(Keys.Up))
        activeMenu--;

      if (activeMenu == 5 && input.OnFirstFramePress(Keys.Enter))
        state = State.MainMenu;
    }

    activeMenu = Math.Clamp(activeMenu, 1, menuItemsLength);

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
