using System;
using Microsoft.Xna.Framework;
using SpacePhysics.Menu.MenuItems;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Menu.SubMenus;

public class ControlsMenu : SubMenu
{

  public ControlsMenu() : base(
    "Controls",
    new Vector2(0f, -950f),
    State.Controls,
    State.Settings
  )
  { }

  public override void AddMenuItems()
  {
    menuItems.Add(new ControlItem(
      "Adjust Pitch",
      () => "Left Stick",
      () => activeMenu == 1,
      alignment,
      () => new Vector2(0f, 0f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Increase Throttle",
      () => "Right Trigger",
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Decrease Throttle",
      () => "Left Trigger",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSizeY * 2f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Adjust RCS",
      () => "Left Stick",
      () => activeMenu == 4,
      alignment,
      () => new Vector2(0f, menuSizeY * 3f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Toggle RCS",
      () => "X",
      () => activeMenu == 5,
      alignment,
      () => new Vector2(0f, menuSizeY * 4f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Toggle RCS Mode",
      () => "Left Stick Click",
      () => activeMenu == 6,
      alignment,
      () => new Vector2(0f, menuSizeY * 5f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Toggle SAS",
      () => "Y",
      () => activeMenu == 7,
      alignment,
      () => new Vector2(0f, menuSizeY * 6f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Set SAS Mode",
      () => "Directional Pad",
      () => activeMenu == 8,
      alignment,
      () => new Vector2(0f, menuSizeY * 7f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Move Camera",
      () => "Right Stick",
      () => activeMenu == 9,
      alignment,
      () => new Vector2(0f, menuSizeY * 8f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Adjust Camera Zoom",
      () => "Right Stick",
      () => activeMenu == 10,
      alignment,
      () => new Vector2(0f, menuSizeY * 9f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Toggle Camera",
      () => "Back",
      () => activeMenu == 11,
      alignment,
      () => new Vector2(0f, menuSizeY * 10f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Toggle Camera Mode",
      () => "Right Stick Click",
      () => activeMenu == 12,
      alignment,
      () => new Vector2(0f, menuSizeY * 11f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Toggle Debug View",
      () => "F3",
      () => activeMenu == 13,
      alignment,
      () => new Vector2(0f, menuSizeY * 12f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    base.AddMenuItems();
  }
}
