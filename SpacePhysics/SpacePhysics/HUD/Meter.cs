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

        components.Add(throttleMeter);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }
}
