using System;
using System.Collections.Generic;

namespace SpacePhysics.Menu;

public class MenuContainer
{
  public static float CalculateMenuHeight(List<CustomGameComponent> components)
  {
    float totalHeight = 0;
    float previousPositionY = 0;
    float previousHeight = 0;

    foreach (var component in components)
    {
      totalHeight += component.height;

      if (previousPositionY > 0 && previousHeight > 0)
      {
        totalHeight += component.position.Y - previousPositionY - previousHeight;
      }

      previousPositionY = component.position.Y;
      previousHeight = component.height;
    }

    return totalHeight;
  }
}
