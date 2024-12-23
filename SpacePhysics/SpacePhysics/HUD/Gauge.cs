using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpacePhysics.GameState;

namespace SpacePhysics.HUD;

public class Gauge : CustomGameComponent
{
    private Vector2 offset;

    public Gauge(Func<float> opacity) : base(false, Alignment.BottomCenter, 11)
    {
        offset = new Vector2(0, -450f);

        HudSprite guage = new(
            "HUD/gauge",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => 0f,
            () => defaultColor * opacity(),
            hudScale,
            11
        );

        HudSprite directionIndicator = new(
            "HUD/gauge-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => direction,
            () => sas ? highlightColor * opacity() : defaultColor * opacity(),
            hudScale,
            11
        );

        HudSprite progradeIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => velocityAngle,
            () => new Color(0, 255, 0) * opacity(),
            hudScale,
            11
        );

        HudSprite retrogradeIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => velocityAngle + (float)Math.PI,
            () => Color.Red * opacity(),
            hudScale,
            11
        );

        HudSprite radialLeftIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => velocityAngle - (float)(Math.PI * 0.5f),
            () => Color.Cyan * opacity(),
            hudScale,
            11
        );

        HudSprite radialRightIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => velocityAngle + (float)(Math.PI * 0.5f),
            () => Color.Cyan * opacity(),
            hudScale,
            11
        );

        HudText altitude = new(
            "Fonts/text-font",
            () => (Math.Abs(GameState.position.Y) / units).ToString("0") + " m",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(0, -440f) + offset,
            () => highlightColor * opacity(),
            hudTextScale,
            11
        );

        HudText velocity = new(
            "Fonts/text-font",
            () => (GameState.velocity.Length() / units).ToString("0.0") + " m/s",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(0, -120f) + offset,
            () => highlightColor * opacity(),
            hudTextScale,
            11
        );

        HudText heading = new(
            "Fonts/text-font",
            () => Math.Round(Math.Abs(direction * (180 / Math.PI) + 90) % 360).ToString() + "°",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(0, 240f) + offset,
            () => sas ? highlightColor * opacity() : defaultColor * opacity(),
            hudTextScale,
            11
        );

        components.Add(guage);
        components.Add(directionIndicator);
        components.Add(progradeIndicator);
        components.Add(retrogradeIndicator);
        components.Add(radialLeftIndicator);
        components.Add(radialRightIndicator);
        components.Add(altitude);
        components.Add(velocity);
        components.Add(heading);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }
}
