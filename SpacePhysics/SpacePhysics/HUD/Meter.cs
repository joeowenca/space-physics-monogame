using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpacePhysics.GameState;

namespace SpacePhysics.HUD;

public class Meter : CustomGameComponent
{
    private Vector2 offset;

    public Meter(Func<float> opacity) : base(false, Alignment.BottomCenter, 11)
    {
        offset = new Vector2(0, -450f);

        HudSprite throttleMeter = new(
            "HUD/meter-left",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(-500, 0) + offset,
            () => 0f,
            () => defaultColor * opacity(),
            hudScale,
            11
        );

        HudSprite throttleMeterIndicator = new(
            "HUD/meter-indicator-left",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(-500, -throttle * 628) + offset,
            () => 0f,
            () => highlightColor * opacity(),
            hudScale,
            11
        );

        HudText throttlePercent = new(
            "Fonts/text-font",
            () => (throttle * 100).ToString("0") + "%",
            Alignment.BottomCenter,
            TextAlign.Right,
            () => new Vector2(-650, 50f) + offset,
            () => highlightColor * opacity(),
            hudTextScale,
            11
        );

        HudText throttleLabel = new(
            "Fonts/text-font",
            () => "Throttle",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(-500, -360f) + offset,
            () => defaultColor * opacity(),
            hudTextScale,
            11
        );

        components.Add(throttleMeter);
        components.Add(throttleMeterIndicator);
        components.Add(throttleLabel);
        components.Add(throttlePercent);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }
}
