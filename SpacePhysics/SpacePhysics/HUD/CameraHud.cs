using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpacePhysics.GameState;

namespace SpacePhysics.HUD;

public class CameraHud : CustomGameComponent
{
    private Vector2 offset;

    private string cameraAngleText;

    public CameraHud(Func<float> opacity) : base(true, Alignment.TopCenter, 11)
    {
        offset = new Vector2(0, 350f);

        cameraAngleText = "Horizon";

        components.Add(new HudText(
            "Fonts/text-font",
            () => "Camera: ",
            Alignment.TopCenter,
            TextAlign.Left,
            () => new Vector2(0f, 0f) + offset,
            () => defaultColor * opacity(),
            hudTextScale,
            11
        ));

        components.Add(new HudText(
            "Fonts/text-font",
            () => cameraAngleText,
            Alignment.TopCenter,
            TextAlign.Left,
            () => new Vector2(components[0].width, 0f) + offset,
            () => highlightColor * opacity(),
            hudTextScale,
            11
        ));
    }

    public override void Update()
    {
        cameraAngleText = Camera.Camera.changeCamera ? "Ship" : "Horizon";

        base.Update();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }
}
