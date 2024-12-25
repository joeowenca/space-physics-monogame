using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Scenes.Start;
using SpacePhysics.HUD;
using static SpacePhysics.GameState;

namespace SpacePhysics.Menu;

public class PauseMenu : CustomGameComponent
{
  private Vector2 offset;
  private Vector2 baseOffset;

  private float opacity;

  private int menuItemsLength;
  private int activeMenu;

  public static bool quit;

  public PauseMenu(
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
    offset = new Vector2(1050f + StartScene.menuOffsetX, 0f);
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
      () => new Vector2(0f, menuSize) + offset,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Main Menu",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSize * 2f) + offset,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Quit",
      () => activeMenu == 4,
      alignment,
      () => new Vector2(0f, menuSize * 3f) + offset,
      () => opacity,
      11
    ));
  }

  public override void Initialize()
  {
    menuItemsLength = 4;
    activeMenu = 1;
    quit = false;

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

      if (input.OnFirstFramePress(Keys.Down))
        activeMenu++;

      if (input.OnFirstFramePress(Keys.Up))
        activeMenu--;

      if (activeMenu == 1 && input.OnFirstFramePress(Keys.Enter))
        state = State.Play;

      if (activeMenu == 2 && input.OnFirstFramePress(Keys.Enter))
        state = State.Play;

      if (activeMenu == 3 && input.OnFirstFramePress(Keys.Enter))
        state = State.Play;

      if (activeMenu == 4 && input.OnFirstFramePress(Keys.Enter))
        quit = true;
    }

    activeMenu = Math.Clamp(activeMenu, 1, menuItemsLength);

    offset.X = baseOffset.X + (StartScene.menuOffset.X * 0.85f * 3f);

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
