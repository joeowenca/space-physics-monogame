using System;
using Microsoft.Xna.Framework;

namespace SpacePhysics;

public class ColorHelper
{
  public static Color Lerp(Color from, Color to, float speed)
  {
    speed = Math.Clamp(speed, 0f, 1f);

    return new Color(
      (byte)MathHelper.Lerp(from.R, to.R, speed),
      (byte)MathHelper.Lerp(from.G, to.G, speed),
      (byte)MathHelper.Lerp(from.B, to.B, speed),
      (byte)MathHelper.Lerp(from.A, to.A, speed)
    );
  }

  public static float FadeOpacity(
    float opacity,
    float start,
    float end,
    float durationInSeconds)
  {
    bool fadeIn = (start - end) < 0;
    bool fadeOut = (start - end) > 0;

    if (GameState.deltaTime > 0f)
    {
      float increment = GameState.deltaTime * Math.Abs(start - end) / durationInSeconds;

      if (fadeIn)
      {
        opacity += increment;

        opacity = Math.Clamp(opacity, start, end);
      }

      if (fadeOut)
      {
        opacity -= increment;

        opacity = Math.Clamp(opacity, end, start);
      }

      return opacity;
    }

    return start;
  }
}
