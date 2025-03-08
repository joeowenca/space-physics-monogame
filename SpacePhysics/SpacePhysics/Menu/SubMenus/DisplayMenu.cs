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
      () => SettingsState.aspectRatio,
      () => SettingsState.aspectRatioOptions,
      value => SettingsState.aspectRatio = value,
      () => activeMenu == 1,
      () => updatable,
      alignment,
      () => new Vector2(0f, 0f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuSelectorItem(
      "Resolution",
      () => SettingsState.resolution,
      () => SettingsState.GetReslutionOptionsFromAspectRatio(SettingsState.aspectRatio),
      value => SettingsState.resolution = value,
      () => activeMenu == 2,
      () => updatable,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuSelectorItem(
      "Vsync",
      () => SettingsState.vsync.ToString(),
      () => ["On", "Off"],
      value => SettingsState.vsync = value == "On",
      () => activeMenu == 3,
      () => updatable,
      alignment,
      () => new Vector2(0f, menuSizeY * 2) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    menuItems.Add(new MenuItem(
      "Apply",
      () => activeMenu == 4,
      alignment,
      () => new Vector2(0f, menuSizeY * 3.5f) + menuOffsetOverride + entireOffsetOverride,
      () => opacity,
      11
    ));

    base.AddMenuItems();
  }

  public override void Update()
  {
    updatable = state == State.Display;

    if (activeMenu == 4 && input.MenuSelect())
    {
      Main.applyGraphics = true;
      state = previousState;
    }

    if (opacity < 0.1f) activeMenu = 1;

    base.Update();
  }
}
