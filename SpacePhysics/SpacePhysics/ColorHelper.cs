using System;
using Microsoft.Xna.Framework;

namespace SpacePhysics;

public class ColorHelper
{
  public static Color Lerp(Color from, Color to, float duration)
  {
    duration = Math.Clamp(duration, 0f, 1f);

    return new Color(
      (byte)MathHelper.Lerp(from.R, to.R, duration),
      (byte)MathHelper.Lerp(from.G, to.G, duration),
      (byte)MathHelper.Lerp(from.B, to.B, duration),
      (byte)MathHelper.Lerp(from.A, to.A, duration)
    );
  }
}
