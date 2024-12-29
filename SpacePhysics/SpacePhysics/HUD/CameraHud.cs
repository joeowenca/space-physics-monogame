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
    private Vector2 cameraAngleOffset;
    private Vector2 cameraModeOffset;

    private float cameraAngleTextOpacity;
    private float cameraAngleTextOpacityTimer;

    private string cameraAngleText;
    private string cameraModeText;

    public CameraHud(Func<float> opacity) : base(true, Alignment.TopCenter, 11)
    {
        cameraAngleOffset = new Vector2(0, 350f);
        cameraModeOffset = new Vector2(0, 175f);

        cameraAngleText = "Horizon";
        cameraModeText = "Move";

        components.Add(new HudText(
            "Fonts/text-font",
            () => "Camera: ",
            Alignment.TopCenter,
            TextAlign.Left,
            () => new Vector2(0f, 0f) + cameraAngleOffset,
            () => defaultColor * cameraAngleTextOpacity * opacity(),
            hudTextScale,
            11
        ));

        components.Add(new HudText(
            "Fonts/text-font",
            () => cameraAngleText,
            Alignment.TopCenter,
            TextAlign.Left,
            () => new Vector2(components[0].width, 0f) + cameraAngleOffset,
            () => highlightColor * cameraAngleTextOpacity * opacity(),
            hudTextScale,
            11
        ));

        components.Add(new HudText(
            "Fonts/text-font",
            () => "Camera Mode: ",
            Alignment.TopCenter,
            TextAlign.Left,
            () => new Vector2(0f, 0f) + cameraModeOffset,
            () => defaultColor * cameraModeTextOpacity * opacity(),
            hudTextScale,
            11
        ));

        components.Add(new HudText(
            "Fonts/text-font",
            () => cameraModeText,
            Alignment.TopCenter,
            TextAlign.Left,
            () => new Vector2(components[2].width, 0f) + cameraModeOffset,
            () => highlightColor * cameraModeTextOpacity * opacity(),
            hudTextScale,
            11
        ));
    }

    public override void Update()
    {
        cameraAngleOffset.X = CenterText(components[0].width, components[1].width);
        cameraModeOffset.X = CenterText(components[2].width, components[3].width);

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

    private float CenterText(float width1, float width2)
    {
        return (width1 + width2) * -0.5f;
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
                opacityTransitionSpeed
            );
        }
    }
}
