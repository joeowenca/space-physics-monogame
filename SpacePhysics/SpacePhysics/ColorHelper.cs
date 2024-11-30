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
    float preDelayInSeconds,
    float durationInSeconds)
  {
    bool fadeIn = start - end < 0;
    bool fadeOut = start - end > 0;

    if (GameState.deltaTime > 0f)
    {
      float increment = GameState.deltaTime * Math.Abs(start - end) / (durationInSeconds + preDelayInSeconds);
      float preDelayFrames = preDelayInSeconds * Main.FPS;
      float preDelay = preDelayFrames * increment;

      if (fadeIn)
      {
        opacity -= preDelay;
        opacity += increment;

        opacity = Math.Clamp(opacity, start - preDelay, end);
      }

      if (fadeOut)
      {
        opacity += preDelay;
        opacity -= increment;

        opacity = Math.Clamp(opacity, start, end + preDelay);
      }

      return opacity;
    }

    if (fadeIn) return start;
    if (fadeOut) return end;

    return 0f;
  }
}
