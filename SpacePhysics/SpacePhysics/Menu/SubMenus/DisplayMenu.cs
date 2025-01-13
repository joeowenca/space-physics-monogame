using System;
using Microsoft.Xna.Framework;
using SpacePhysics.Menu.MenuItems;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Menu.SubMenus;

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
    menuItems.Add(new MenuSelectorItem(
      "Aspect ratio",
      () => "16:9",
      true,
      () => activeMenu == 1,
      alignment,
      () => new Vector2(0f, 0f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuSelectorItem(
      "Resolution",
      () => "2560x1440",
      true,
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuSelectorItem(
      "Vsync",
      () => "Off",
      true,
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
