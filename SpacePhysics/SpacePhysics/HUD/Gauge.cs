using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePhysics.HUD;

public class Gauge : CustomGameComponent
{
    private Vector2 offset;

    public Gauge(Func<float> opacity) : base(false, Alignment.BottomCenter, 11) {
        offset = new Vector2(0, -400f);

        HudSprite guage = new(
            "HUD/gauge",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => 0f,
            () => GameState.defaultColor * opacity(),
            1.25f,
            11
        );

        HudSprite directionIndicator = new(
            "HUD/gauge-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => GameState.direction,
            () => GameState.defaultColor * opacity(),
            1.25f,
            11
        );

        HudSprite progradeIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => GameState.velocityAngle,
            () => new Color(0, 255, 0) * opacity(),
            1.25f,
            11
        );

        HudSprite retrogradeIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => GameState.velocityAngle + (float)Math.PI,
            () => Color.Red * opacity(),
            1.25f,
            11
        );

        HudSprite radialLeftIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => GameState.velocityAngle - (float)(Math.PI * 0.5f),
            () => Color.Cyan * opacity(),
            1.25f,
            11
        );

        HudSprite radialRightIndicator = new(
            "HUD/direction-indicator",
            Alignment.BottomCenter,
            Alignment.Center,
            () => offset,
            () => GameState.velocityAngle + (float)(Math.PI * 0.5f),
            () => Color.Cyan * opacity(),
            1.25f,
            11
        );

        components.Add(guage);
        components.Add(directionIndicator);
        components.Add(progradeIndicator);
        components.Add(retrogradeIndicator);
        components.Add(radialLeftIndicator);
        components.Add(radialRightIndicator);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components) {
            component.Draw(spriteBatch);
        }
    }
}
