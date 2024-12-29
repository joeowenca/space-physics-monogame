using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Scenes.Space;
using SpacePhysics.Scenes.Start;
using static SpacePhysics.GameState;

namespace SpacePhysics.HUD;

public class CameraHud : CustomGameComponent
{
    private Vector2 offset;

    private float cameraAngleTextOpacity;
    private float cameraAngleTextOpacityTimer;

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
            () => defaultColor * cameraAngleTextOpacity * opacity(),
            hudTextScale,
            11
        ));

        components.Add(new HudText(
            "Fonts/text-font",
            () => cameraAngleText,
            Alignment.TopCenter,
            TextAlign.Left,
            () => new Vector2(components[0].width, 0f) + offset,
            () => highlightColor * cameraAngleTextOpacity * opacity(),
            hudTextScale,
            11
        ));
    }

    public override void Update()
    {
        CenterCameraAngleText();

        HandleCameraAngleChange();

        base.Update();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }

    private void CenterCameraAngleText()
    {
        offset.X = (components[0].width + components[1].width) * -0.5f;
    }

    private void HandleCameraAngleChange()
    {
        cameraAngleText = Camera.Camera.changeCamera ? "Ship" : "Horizon";

        if
        (
            input.OnFirstFrameKeyPress(Keys.V)
            || input.OnFirstFrameButtonPress(Buttons.Back)
        )
        {
            cameraAngleTextOpacity = 1f;
            cameraAngleTextOpacityTimer = elapsedTime;
        }

        if (elapsedTime > cameraAngleTextOpacityTimer + 2f)
        {
            cameraAngleTextOpacity = ColorHelper.FadeOpacity
            (
                cameraAngleTextOpacity,
                1f,
                0f,
                0.6f
            );
        }
    }
}
