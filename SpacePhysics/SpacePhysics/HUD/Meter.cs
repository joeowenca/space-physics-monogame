using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.Player;
using static SpacePhysics.GameState;

namespace SpacePhysics.HUD;

public class Meter : CustomGameComponent
{
    private Vector2 offset;

    private float padding;

    public Meter(Func<float> opacity) : base(false, Alignment.BottomCenter, 11)
    {
        offset = new Vector2(0, -450f);

        padding = 600f;

        HudSprite throttleMeter = new(
            "HUD/meter-left",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(-padding, 0) + offset,
            () => 0f,
            () => defaultColor * opacity(),
            () => hudScale,
            11
        );

        HudSprite throttleMeterIndicator = new(
            "HUD/meter-indicator-left",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(-padding, -throttle * 628) + offset,
            () => 0f,
            () => highlightColor * opacity(),
            () => hudScale,
            11
        );

        HudText throttlePercent = new(
            "Fonts/text-font",
            () => (throttle * 100).ToString("0") + "%",
            Alignment.BottomCenter,
            TextAlign.Right,
            () => new Vector2(-padding - 150, 50f) + offset,
            () => highlightColor * opacity(),
            hudTextScale,
            11
        );

        HudText throttleLabel = new(
            "Fonts/text-font",
            () => "Throttle",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(-padding, -360f) + offset,
            () => defaultColor * opacity(),
            hudTextScale,
            11
        );

        HudSprite electricityMeter = new(
            "HUD/meter-left",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(-padding * 2f, 0) + offset,
            () => 0f,
            () => defaultColor * opacity(),
            () => hudScale,
            11
        );

        HudSprite electricityMeterIndicator = new(
            "HUD/meter-indicator-left",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(-padding * 2f, GameState.electricityPercent * -6.28f) + offset,
            () => 0f,
            () => highlightColor * opacity(),
            () => hudScale,
            11
        );

        HudText electricityPercent = new(
            "Fonts/text-font",
            () => GameState.electricityPercent.ToString("0") + "%",
            Alignment.BottomCenter,
            TextAlign.Right,
            () => new Vector2(-padding * 2f - 150, 50f) + offset,
            () => highlightColor * opacity(),
            hudTextScale,
            11
        );

        HudText electricityLabel = new(
            "Fonts/text-font",
            () => "Electricity",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(-padding * 2f, -360f) + offset,
            () => defaultColor * opacity(),
            hudTextScale,
            11
        );

        HudSprite fuelMeter = new(
            "HUD/meter-right",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(padding, 0) + offset,
            () => 0f,
            () => defaultColor * opacity(),
            () => hudScale,
            11
        );

        HudSprite fuelMeterIndicator = new(
            "HUD/meter-indicator-right",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(padding, -GameState.fuelPercent * 6.28f) + offset,
            () => 0f,
            () => highlightColor * opacity(),
            () => hudScale,
            11
        );

        HudText fuelPercent = new(
            "Fonts/text-font",
            () => GameState.fuelPercent.ToString("0") + "%",
            Alignment.BottomCenter,
            TextAlign.Left,
            () => new Vector2(padding + 150, 50f) + offset,
            () => highlightColor * opacity(),
            hudTextScale,
            11
        );

        HudText fuelLabel = new(
            "Fonts/text-font",
            () => "Fuel",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(padding, -360f) + offset,
            () => defaultColor * opacity(),
            hudTextScale,
            11
        );

        HudSprite monoMeter = new(
            "HUD/meter-right",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(padding * 2f, 0) + offset,
            () => 0f,
            () => defaultColor * opacity(),
            () => hudScale,
            11
        );

        HudSprite monoMeterIndicator = new(
            "HUD/meter-indicator-right",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(padding * 2f, -GameState.monoPercent * 6.28f) + offset,
            () => 0f,
            () => highlightColor * opacity(),
            () => hudScale,
            11
        );

        HudText monoPercent = new(
            "Fonts/text-font",
            () => GameState.monoPercent.ToString("0") + "%",
            Alignment.BottomCenter,
            TextAlign.Left,
            () => new Vector2(padding * 2f + 150, 50f) + offset,
            () => highlightColor * opacity(),
            hudTextScale,
            11
        );

        HudText monoLabel = new(
            "Fonts/text-font",
            () => "Mono",
            Alignment.BottomCenter,
            TextAlign.Center,
            () => new Vector2(padding * 2f, -360f) + offset,
            () => defaultColor * opacity(),
            hudTextScale,
            11
        );

        HudSprite pitchMeter = new(
            "HUD/meter-blank",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2(0f, -padding) + offset,
            () => (float)Math.PI * 0.5f,
            () => defaultColor * opacity(),
            () => hudScale,
            11
        );

        HudSprite pitchIndicator = new(
            "HUD/meter-indicator-left",
            Alignment.BottomCenter,
            Alignment.Center,
            () => new Vector2((Ship.pitch * 314f) + 315f, -padding) + offset,
            () => (float)Math.PI * 0.5f,
            () => highlightColor * opacity(),
            () => hudScale,
            11
        );

        components.Add(throttleMeter);
        components.Add(throttleMeterIndicator);
        components.Add(throttlePercent);
        components.Add(throttleLabel);
        components.Add(electricityMeter);
        components.Add(electricityMeterIndicator);
        components.Add(electricityPercent);
        components.Add(electricityLabel);
        components.Add(fuelMeter);
        components.Add(fuelMeterIndicator);
        components.Add(fuelPercent);
        components.Add(fuelLabel);
        components.Add(monoMeter);
        components.Add(monoMeterIndicator);
        components.Add(monoPercent);
        components.Add(monoLabel);
        components.Add(pitchMeter);
        components.Add(pitchIndicator);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }
}
