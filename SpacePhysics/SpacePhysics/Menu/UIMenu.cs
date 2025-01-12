using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.HUD;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Menu;

public class UIMenu : CustomGameComponent
{
  private Vector2 offset;
  private Vector2 baseOffset;
  private Vector2 menuOffsetOverride;
  private Vector2 entireOffsetOverride;

  private float opacity;
  private float controlItemDistance;

  private int menuItemsLength;
  private int activeMenu;

  public UIMenu(
    bool allowInput,
    Alignment alignment,
    int layerIndex) : base(
      allowInput,
      alignment,
      layerIndex
    )
  {
    entireOffsetOverride = new Vector2(-600f, -50f);
    offset = new Vector2(menuOffsetXRight, 0f);
    baseOffset = offset;
    controlItemDistance = 1750f;

    components.Add(new HudText(
      "Fonts/title-font",
      () => "UI",
      alignment,
      TextAlign.Left,
      () => new Vector2(-100, -400) + offset + entireOffsetOverride,
      () => Color.White * opacity,
      1.75f,
      11
    ));

    components.Add(new ControlItem(
      "Scale",
      () => "100%",
      () => activeMenu == 1,
      alignment,
      () => new Vector2(0f, 0f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Color",
      () => "Yellow",
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Safe zone",
      () => "0.0",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSizeY * 2f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Back",
      () => activeMenu == 4,
      alignment,
      () => new Vector2(0f, menuSizeY * 3.5f) + menuOffsetOverride + entireOffsetOverride,
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
    if (state != State.UI)
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
    if (state == State.UI)
    {
      if (input.MenuDown())
        activeMenu++;

      if (input.MenuUp())
        activeMenu--;

      if ((activeMenu == 4 && input.MenuSelect()) || input.MenuBack())
        state = State.Settings;
    }

    activeMenu = Math.Clamp(activeMenu, 1, menuItemsLength);
  }

  private void UpdateOffset()
  {
    offset.X = baseOffset.X + menuOffset.X * 3f;
    menuOffsetOverride.X = baseOffset.X - 150 + menuOffsetFactor;
  }
}
