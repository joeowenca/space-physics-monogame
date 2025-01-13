using System;
using Microsoft.Xna.Framework;
using SpacePhysics.Menu.MenuItems;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Menu.SubMenus;

public class SoundMenu : SubMenu
{

  public SoundMenu() : base(
    "Sound",
    new Vector2(0f, -150f),
    State.Sound,
    State.Settings
  )
  { }

  public override void AddMenuItems()
  {
    menuItems.Add(new MenuSelectorItem(
      "Master",
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
      "Music",
      () => "100%",
      false,
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuSelectorItem(
      "Sound effects",
      () => "100%",
      false,
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSizeY * 2f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuSelectorItem(
      "Menu sound effects",
      () => "100%",
      false,
      () => activeMenu == 4,
      alignment,
      () => new Vector2(0f, menuSizeY * 3f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    base.AddMenuItems();
  }
}
