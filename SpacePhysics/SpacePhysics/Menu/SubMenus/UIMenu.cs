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
      () => ["100%"],
      value => SettingsState.uiScale = 1,
      () => activeMenu == 1,
      () => updatable,
      alignment,
      () => new Vector2(0f, 0f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuSelectorItem(
      "Color",
      () => "Yellow",
      () => ["Yellow"],
      value => SettingsState.uiColor = Color.Gold,
      () => activeMenu == 2,
      () => updatable,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuSelectorItem(
      "Safe zone",
      () => "0.0",
      () => ["0.0"],
      value => SettingsState.uiSafeZone = 1f,
      () => activeMenu == 3,
      () => updatable,
      alignment,
      () => new Vector2(0f, menuSizeY * 2) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    base.AddMenuItems();
  }

  public override void Update()
  {
    updatable = state == State.UI;

    if (opacity < 0.1f) activeMenu = 1;

    base.Update();
  }
}
