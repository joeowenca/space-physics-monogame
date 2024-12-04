using System;
using System.Collections.Generic;

namespace SpacePhysics.Menu;

public class MenuContainer
{
  public static float CalculateMenuHeight(List<CustomGameComponent> menuItems)
  {
    float totalHeight = 0;
    float previousPositionY = 0;
    float previousHeight = 0;

    foreach (var menuItem in menuItems)
    {
      totalHeight += menuItem.height;

      if (previousPositionY > 0 && previousHeight > 0)
      {
        totalHeight += menuItem.position.Y - previousPositionY - previousHeight;
      }

      previousPositionY = menuItem.position.Y;
      previousHeight = menuItem.height;
    }

    return totalHeight;
  }
}
