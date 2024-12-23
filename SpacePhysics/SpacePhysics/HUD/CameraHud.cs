using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Scenes.Start;
using static SpacePhysics.GameState;

namespace SpacePhysics.HUD;

public class CameraHud : CustomGameComponent
{
    private Vector2 offset;

    public CameraHud(Func<float> opacity) : base(false, Alignment.TopCenter, 11)
    {
        offset = new Vector2(0, 400f);

        HudSprite meter = new(
            "HUD/meter-left",
            Alignment.TopCenter,
            Alignment.Center,
            () => offset,
            () => -(float)Math.PI * 0.5f,
            () => defaultColor * opacity(),
            hudScale,
            11
        );

        HudSprite indicator = new(
            "HUD/meter-indicator-left",
            Alignment.TopCenter,
            Alignment.Center,
            () => new Vector2((zoomPercent * 6.28f) - 628f, 0) + offset,
            () => -(float)Math.PI * 0.5f,
            () => highlightColor * opacity(),
            hudScale,
            11
        );

        HudText percent = new(
            "Fonts/text-font",
            () => zoomPercent.ToString("0") + "%",
            Alignment.TopCenter,
            TextAlign.Center,
            () => new Vector2(0, 120f) + offset,
            () => highlightColor * opacity(),
            hudTextScale,
            11
        );

        components.Add(meter);
        components.Add(indicator);
        components.Add(percent);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }
}
