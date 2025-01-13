using System;
using Microsoft.Xna.Framework;
using SpacePhysics.Menu.MenuItems;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Menu.SubMenus;

public class UIMenu : SubMenu
{

  public UIMenu() : base(
    "UI",
    new Vector2(0f, -50f),
    State.UI,
    State.Settings
  )
  { }

  public override void AddMenuItems()
  {
    menuItems.Add(new MenuSelectorItem(
      "Scale",
      () => "100%",
      false,
      () => activeMenu == 1,
      alignment,
      () => new Vector2(0f, 0f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuSelectorItem(
      "Color",
      () => "Yellow",
      true,
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuSelectorItem(
      "Safe zone",
      () => "0.0",
      false,
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
