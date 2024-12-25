using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Scenes.Start;
using static SpacePhysics.GameState;

namespace SpacePhysics.Menu;

public class MainMenu : CustomGameComponent
{
  private Vector2 offset;
  private Vector2 baseOffset;

  private float opacity;

  private int menuItemsLength;
  private int activeMenu;

  public MainMenu(
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
    offset = new Vector2(1050f + MenuContainer.menuOffsetX, 50f);
    baseOffset = offset;

    components.Add(new MenuItem(
        "Play",
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
      "Quit",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSize * 2f) + offset,
      () => opacity,
      11
    ));
  }

  public override void Initialize()
  {
    menuItemsLength = 3;
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
    if (state != State.MainMenu)
    {
      if (opacity > 0)
        opacity = ColorHelper.FadeOpacity(opacity, 1f, 0f, StartScene.transitionSpeed);

      if (opacity <= 0.1f)
        activeMenu = 1;

      if (state == State.Play)
      {
        Camera.Camera.targetZoomOverride = 20f;
      }
      else
      {
        Camera.Camera.zoomOverride = 1f;
        Camera.Camera.targetZoomOverride = 1f;
      }
    }
    else
    {
      opacity = ColorHelper.FadeOpacity(opacity, 0f, 1f, StartScene.transitionSpeed);

      Camera.Camera.zoomOverride = 1f;
      Camera.Camera.targetZoomOverride = 1f;
    }
  }

  private void UpdateMenu()
  {
    if (state == State.MainMenu)
    {
      if (input.OnFirstFramePress(Keys.Down))
        activeMenu++;

      if (input.OnFirstFramePress(Keys.Up))
        activeMenu--;

      if (activeMenu == 1 && input.OnFirstFramePress(Keys.Enter))
        state = State.Play;

      if (activeMenu == 2 && input.OnFirstFramePress(Keys.Enter))
        state = State.Settings;

      if (activeMenu == 3 && input.OnFirstFramePress(Keys.Enter))
        quit = true;
    }

    activeMenu = Math.Clamp(activeMenu, 1, menuItemsLength);
  }

  private void UpdateOffset()
  {
    offset.X = baseOffset.X + (StartScene.menuOffset.X * 0.85f * 3f);
  }
}
