using System;
using Microsoft.Xna.Framework;
using static SpacePhysics.GameState;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Menu;

public class SoundMenu : SubMenu
{

  public SoundMenu() : base(
    "Sound",
    new Vector2(-600f, 0),
    State.Sound,
    State.Settings
  )
  { }

  public override void AddMenuItems()
  {
    components.Add(new ControlItem(
      "Master",
      () => "100%",
      () => activeMenu == 1,
      alignment,
      () => new Vector2(0f, 0f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Music",
      () => "100%",
      () => activeMenu == 2,
      alignment,
      () => new Vector2(0f, menuSizeY) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Sound effects",
      () => "100%",
      () => activeMenu == 3,
      alignment,
      () => new Vector2(0f, menuSizeY * 2f) + menuOffsetOverride + entireOffsetOverride,
      controlItemDistance,
      () => opacity,
      11
    ));

    components.Add(new ControlItem(
      "Menu sound effects",
      () => "100%",
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
