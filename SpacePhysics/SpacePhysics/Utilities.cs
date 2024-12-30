using System;
using Microsoft.Xna.Framework;

namespace SpacePhysics;

public class Utilities
{
  public static float ExponentialLerp(float start, float end, float t, float k = 10f)
  {
    t = MathHelper.Clamp(t, 0f, 1f);
    float factor = 1f - (float)Math.Exp(-t * k);
    return MathHelper.Lerp(start, end, factor);
  }

  public static float RadiansToDegrees(float radians)
  {
    float degrees = radians * (180f / (float)Math.PI);

    degrees = degrees % 360f;

    if (degrees < 0) degrees += 360f;

    return (float)Math.Round(degrees);
  }
}
