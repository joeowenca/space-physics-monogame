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

      Console.WriteLine(opacity);

      return opacity;
    }

    return start;
  }
}
