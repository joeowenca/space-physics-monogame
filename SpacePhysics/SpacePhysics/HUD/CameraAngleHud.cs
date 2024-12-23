using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpacePhysics.GameState;

namespace SpacePhysics.HUD;

public class CameraAngleHud : CustomGameComponent
{
    private Vector2 offset;

    public CameraAngleHud(Func<float> opacity) : base(false, Alignment.TopCenter, 11)
    {
        offset = new Vector2(0, 100f);

        components.Add(new HudText(
            "Fonts/text-font",
            () => "Camera: " + (Camera.Camera.changeCamera ? "Ship" : "Horizon"),
            Alignment.TopCenter,
            TextAlign.Center,
            () => new Vector2(0f, 0f) + offset,
            () => defaultColor * opacity(),
            hudTextScale,
            11
        ));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }
}
