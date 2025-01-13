using System;
using Microsoft.Xna.Framework;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Menu;

public class DisplayMenu : SubMenu
{

  public DisplayMenu() : base(
    "Display",
    new Vector2(0f, -50f),
    State.Display,
    State.Settings
  )
  { }

  public override void AddMenuItems()
  {
    menuItems.Add(new ControlItem(
      "Aspect ratio",
      () => "16:9",
      () => activeMenu == 1,
      alignment,
      () => new Vector2(0f, 0f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Resolution",
      () => "2560x1440",
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new ControlItem(
      "Vsync",
      () => "Off",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSizeY * 2f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    base.AddMenuItems();
  }
}
