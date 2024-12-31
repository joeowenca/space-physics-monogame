using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.HUD;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Menu;

public class ControlsMenu : CustomGameComponent
{
  private Vector2 offset;
  private Vector2 baseOffset;
  private Vector2 menuOffsetOverride;
  private Vector2 entireOffsetOverride;

  private float opacity;
  private float controlItemDistance;

  private int menuItemsLength;
  private int activeMenu;

  public ControlsMenu(
    bool allowInput,
    Alignment alignment,
    int layerIndex) : base(
      allowInput,
      alignment,
      layerIndex
    )
  {
    entireOffsetOverride = new Vector2(-600f, -950f);
    offset = new Vector2(menuOffsetXRight, 0f);
    baseOffset = offset;
    controlItemDistance = 1750f;

    components.Add(new HudText(
      "Fonts/title-font",
      () => "Controls",
      alignment,
      TextAlign.Left,
      () => new Vector2(-100, -400) + offset + entireOffsetOverride,
      () => Color.White * opacity,
      1.75f,
      11
    ));

    components.Add(new ControlItem(
      "Adjust Pitch",
      () => "Left Stick",
      () => activeMenu == 1,
      alignment,
      () => new Vector2(0f, 0f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Increase Throttle",
      () => "Right Trigger",
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Decrease Throttle",
      () => "Left Trigger",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSizeY * 2f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Adjust RCS",
      () => "Left Stick",
      () => activeMenu == 4,
      alignment,
      () => new Vector2(0f, menuSizeY * 3f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Toggle RCS",
      () => "X",
      () => activeMenu == 5,
      alignment,
      () => new Vector2(0f, menuSizeY * 4f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Toggle RCS Mode",
      () => "Left Stick Click",
      () => activeMenu == 6,
      alignment,
      () => new Vector2(0f, menuSizeY * 5f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Toggle SAS",
      () => "Y",
      () => activeMenu == 7,
      alignment,
      () => new Vector2(0f, menuSizeY * 6f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Set SAS Mode",
      () => "Directional Pad",
      () => activeMenu == 8,
      alignment,
      () => new Vector2(0f, menuSizeY * 7f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Move Camera",
      () => "Right Stick",
      () => activeMenu == 9,
      alignment,
      () => new Vector2(0f, menuSizeY * 8f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Adjust Camera Zoom",
      () => "Right Stick",
      () => activeMenu == 10,
      alignment,
      () => new Vector2(0f, menuSizeY * 9f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Toggle Camera",
      () => "Back",
      () => activeMenu == 11,
      alignment,
      () => new Vector2(0f, menuSizeY * 10f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Toggle Camera Mode",
      () => "Right Stick Click",
      () => activeMenu == 12,
      alignment,
      () => new Vector2(0f, menuSizeY * 11f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Toggle Debug View",
      () => "F3",
      () => activeMenu == 13,
      alignment,
      () => new Vector2(0f, menuSizeY * 12f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new MenuItem(
      "Back",
      () => activeMenu == 14,
      alignment,
      () => new Vector2(0f, menuSizeY * 13.5f) + menuOffsetOverride + entireOffsetOverride,
      () => opacity,
      11
    ));
  }

  public override void Initialize()
  {
    menuItemsLength = 14;
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
    if (state != State.Controls)
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
    if (state == State.Controls)
    {
      if (input.MenuDown())
        activeMenu++;

      if (input.MenuUp())
        activeMenu--;

      if ((activeMenu == 14 && input.MenuSelect()) || input.MenuBack())
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
