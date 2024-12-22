using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePhysics.HUD;

public class Gauge : CustomGameComponent
{
    private Vector2 offset;

    public Gauge(Func<float> opacity) : base(false, Alignment.BottomCenter, 11) {
        offset = new Vector2(0, -400f);

        float guageScale = 1.4f;

        HudSprite guage = new(
            "HUD/gauge",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => 0f,
            () => GameState.defaultColor * opacity(),
            guageScale,
            11
        );

        HudSprite directionIndicator = new(
            "HUD/gauge-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => GameState.direction,
            () => GameState.defaultColor * opacity(),
            guageScale,
            11
        );

        HudSprite progradeIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => GameState.velocityAngle,
            () => new Color(0, 255, 0) * opacity(),
            guageScale,
            11
        );

        HudSprite retrogradeIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => GameState.velocityAngle + (float)Math.PI,
            () => Color.Red * opacity(),
            guageScale,
            11
        );

        HudSprite radialLeftIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => GameState.velocityAngle - (float)(Math.PI * 0.5f),
            () => Color.Cyan * opacity(),
            guageScale,
            11
        );

        HudSprite radialRightIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => GameState.velocityAngle + (float)(Math.PI * 0.5f),
            () => Color.Cyan * opacity(),
            guageScale,
            11
        );

        components.Add(guage);
        components.Add(directionIndicator);
        components.Add(progradeIndicator);
        components.Add(retrogradeIndicator);
        components.Add(radialLeftIndicator);
        components.Add(radialRightIndicator);

        components.Add(new HudText(
            "Fonts/text-font",
            () => (GameState.velocity.Length() / GameState.units).ToString("0.0") + "m/s",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => offset + new Vector2(0, -120f),
            () => GameState.defaultColor * opacity(),
            0.6f,
            11
        ));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components) {
            component.Draw(spriteBatch);
        }
    }
}
