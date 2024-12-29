using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Scenes.Start;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

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
    offset = new Vector2(menuOffsetXLeft, 50f);
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
      () => new Vector2(0f, menuSizeY) + offset,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Quit",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSizeY * 2f) + offset,
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
        opacity = ColorHelper.FadeOpacity(opacity, 1f, 0f, opacityTransitionSpeed);

      if (opacity <= 0.1f)
        activeMenu = 1;
    }
    else
    {
      opacity = ColorHelper.FadeOpacity(opacity, 0f, 1f, opacityTransitionSpeed);
    }
  }

  private void UpdateMenu()
  {
    if (state == State.MainMenu)
    {
      if (input.OnFirstFrameKeyPress(Keys.Down)
        || input.OnFirstFrameButtonPress(Buttons.DPadDown)
        || input.OnFirstFrameButtonPress(Buttons.LeftThumbstickDown)
      )
        activeMenu++;

      if (input.OnFirstFrameKeyPress(Keys.Up)
        || input.OnFirstFrameButtonPress(Buttons.DPadUp)
        || input.OnFirstFrameButtonPress(Buttons.LeftThumbstickUp)
      )
        activeMenu--;

      if (activeMenu == 1 &&
        (input.OnFirstFrameKeyPress(Keys.Enter)
        || input.OnFirstFrameButtonPress(Buttons.A))
      )
        state = State.Play;

      if (activeMenu == 2 &&
        (input.OnFirstFrameKeyPress(Keys.Enter)
        || input.OnFirstFrameButtonPress(Buttons.A))
      )
        state = State.Settings;

      if (activeMenu == 3 &&
        (input.OnFirstFrameKeyPress(Keys.Enter)
        || input.OnFirstFrameButtonPress(Buttons.A))
      )
        quit = true;
    }

    activeMenu = Math.Clamp(activeMenu, 1, menuItemsLength);
  }

  private void UpdateOffset()
  {
    offset.X = baseOffset.X + menuOffsetFactor;
  }
}
