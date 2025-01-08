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

    degrees %= 360f;

    if (degrees < 0) degrees += 360f;

    return (float)Math.Round(degrees);
  }

  public static Vector2 RotateVector2(Vector2 vector, float radians)
  {
    float cosTheta = (float)Math.Cos(radians);
    float sinTheta = (float)Math.Sin(radians);

    float x = vector.X * cosTheta - vector.Y * sinTheta;
    float y = vector.X * sinTheta + vector.Y * cosTheta;

    return new Vector2(x, y);
  }
}
