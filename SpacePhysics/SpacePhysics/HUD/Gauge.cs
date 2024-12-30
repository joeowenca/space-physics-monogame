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
            () => progradeRadians,
            () => new Color(0, 255, 0) * opacity(),
            hudScale,
            11
        );

        HudSprite retrogradeIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => retrogradeRadians,
            () => Color.Red * opacity(),
            hudScale,
            11
        );

        HudSprite radialLeftIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => radialLeftRadians,
            () => Color.Cyan * opacity(),
            hudScale,
            11
        );

        HudSprite radialRightIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => radialRightRadians,
            () => Color.Cyan * opacity(),
            hudScale,
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
            () => Utilities.RadiansToDegrees(direction).ToString() + "°",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(0, 240f) + offset,
            () => sas ? highlightColor * opacity() : defaultColor * opacity(),
            hudTextScale,
            11
        );

        HudText sasStatus = new(
            "Fonts/text-font",
            () => "SAS",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(200f, 50f) + offset,
            () => (sas ? highlightColor : defaultColor) * opacity(),
            hudTextScale,
            11
        );

        HudText rcsStatus = new(
            "Fonts/text-font",
            () => "RCS",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(-200f, 50f) + offset,
            () => (rcs ? Color.Cyan : defaultColor) * opacity(),
            hudTextScale,
            11
        );

        HudSprite hudShadow = new(
            "HUD/hud-shadow",
            Alignment.BottomCenter,
            Alignment.BottomCenter,
            () => Vector2.Zero,
            () => 0f,
            () => Color.White * 0.75f * opacity(),
            hudScale,
            11
        );

        components.Add(hudShadow);
        components.Add(guage);
        components.Add(directionIndicator);
        components.Add(progradeIndicator);
        components.Add(retrogradeIndicator);
        components.Add(radialLeftIndicator);
        components.Add(radialRightIndicator);
        components.Add(velocity);
        components.Add(heading);
        components.Add(sasStatus);
        components.Add(rcsStatus);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }
}
