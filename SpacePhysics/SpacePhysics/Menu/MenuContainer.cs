using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpacePhysics.Menu;

public class MenuContainer
{
  public static float menuOffsetX;

  public static float padding = 0.17f;
  public static float menuSizeY = 1000f * padding;

  public MenuContainer()
  {
    float start = 1250f;
    float end = 1f;

    menuOffsetX = start - (GameState.hudScaleOverrideFactor - 0.1f) * Math.Abs((end - start) / 0.9f);
  }

  public static float CalculateMenuHeight(List<CustomGameComponent> menuItems)
  {
    float totalHeight = 0;
    float previousPositionY = 0;
    float previousHeight = 0;

    foreach (var menuItem in menuItems)
    {
      totalHeight += menuItem.height;

      if (Math.Abs(previousPositionY) > 0 || previousHeight > 0)
      {
        totalHeight += menuItem.position.Y - previousPositionY - previousHeight;
      }

      previousPositionY = menuItem.position.Y;
      previousHeight = menuItem.height;
    }

    return totalHeight;
  }

  public static Vector2 CenterMenu(List<CustomGameComponent> menuItems)
  {
    return new Vector2(
      0f,
      GameState.screenSize.Y * GameState.scale - CalculateMenuHeight(menuItems) / 2
    );
  }
}
