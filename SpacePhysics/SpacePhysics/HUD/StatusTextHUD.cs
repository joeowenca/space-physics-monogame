using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Player;
using static SpacePhysics.GameState;

namespace SpacePhysics.HUD;

public class StatusTextHUD : CustomGameComponent
{
    private Vector2 offset;

    private float textOpacity;
    private float fadeOutTimer;

    private string labelText;
    private string valueText;

    public StatusTextHUD(Func<float> opacity) : base(true, Alignment.TopCenter, 11)
    {
        offset = new Vector2(0, 350f);

        labelText = "Camera";
        valueText = "Horizon";

        components.Add(new HudText(
            "Fonts/text-font",
            () => labelText + ": ",
            Alignment.TopCenter,
            TextAlign.Left,
            () => new Vector2(0f, 0f) + offset,
            () => defaultColor * textOpacity * opacity(),
            hudTextScale,
            11
        ));

        components.Add(new HudText(
            "Fonts/text-font",
            () => valueText,
            Alignment.TopCenter,
            TextAlign.Left,
            () => new Vector2(components[0].width, 0f) + offset,
            () => highlightColor * textOpacity * opacity(),
            hudTextScale,
            11
        ));
    }

    public override void Update()
    {
        offset.X = CenterText(components[0].width, components[1].width);

        HandleCameraChange();

        HandleSASModeChange();

        HandleRCSModeChange();

        if (elapsedTime > fadeOutTimer + 2f)
        {
            textOpacity = ColorHelper.FadeOpacity(
                textOpacity,
                1f,
                0f,
                opacityTransitionSpeed
            );
        }

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

    private void HandleCameraChange()
    {
        if (input.ToggleCameraAngle())
        {
            labelText = "Camera";
            valueText = changeCamera ? "Ship" : "Horizon";

            textOpacity = 1f;
            fadeOutTimer = elapsedTime;
        }

        if (input.ToggleCameraMode())
        {
            labelText = "Camera Mode";
            valueText = cameraZoomMode ? "Zoom" : "Move";

            textOpacity = 1f;
            fadeOutTimer = elapsedTime;
        }
    }

    private void HandleSASModeChange()
    {
        if
        (
            input.SetSASTargetProgradeOrStability()
            || input.SetSASTargetRetrograde()
            || input.SetSASTargetRadialLeft()
            || input.SetSASTargetRadialRight()
        )
        {
            labelText = "SAS Mode";
            valueText = SASController.sasModeString;

            textOpacity = 1f;
            fadeOutTimer = elapsedTime;
        }
    }

    private void HandleRCSModeChange()
    {
        if (input.ToggleRCSMode())
        {
            labelText = "RCS Mode";
            valueText = maneuverMode ? "Maneuver" : "Docking";

            textOpacity = 1f;
            fadeOutTimer = elapsedTime;
        }
    }
}
