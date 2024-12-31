using System;
using Microsoft.Xna.Framework;
using SpacePhysics.HUD;
using SpacePhysics.Player;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Sprites;
using SpacePhysics.Scenes.Start;
using SpacePhysics.Debugging;
using SpacePhysics.Menu;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Scenes.Space;

public class SpaceScene : CustomGameComponent
{
  private float opacity;
  private float hudOpacity;
  private float previousTargetZoom;

  public SpaceScene() : base(true, Alignment.TopLeft, 7)
  {
    components.Add(new LoopingBackground(
      "Backgrounds/starfield",
      () => new Color(255, 255, 255, 0) * opacity,
      2f,
      1
    ));

    components.Add(new LoopingBackground(
      "Backgrounds/purple-background",
      () => new Color(100, 100, 100, 0) * opacity,
      3f,
      2
    ));

    components.Add(new LoopingBackground(
      "Backgrounds/purple-background-2",
      () => new Color(100, 100, 100, 0) * opacity,
      3f,
      4
    ));

    components.Add(new Ship(
      () => opacity,
      true,
      Alignment.TopLeft,
      7
    ));

    components.Add(new HudSprite(
        "Backgrounds/dirty-lens",
        Alignment.Center,
        Alignment.Center,
        () => new Vector2(0f, 0f),
        () => 0f,
        () => new Color(255, 255, 255, 0) * ((0.05f * Ship.thrustAmount * (GameState.zoomPercent / 100f)) + 0f) * opacity,
        6.5f * GameState.scale,
        0
    ));

    components.Add(new Gauge(
      () => opacity * hudOpacity
    ));

    components.Add(new Meter(
      () => opacity * hudOpacity
    ));

    components.Add(new ShipInformation(
      () => opacity * hudOpacity
    ));

    components.Add(new ManeuverInformation(
      () => opacity * hudOpacity
    ));

    components.Add(new StatusTextHUD(
      () => opacity * hudOpacity
    ));

    components.Add(new PauseMenu(
      true,
      Alignment.Left,
      11
    ));

    components.Add(new DebugView());
  }

  public override void Initialize()
  {
    GameState.Initialize();

    GameState.state = GameState.State.Play;

    Camera.Camera.allowInput = true;
    Camera.Camera.zoomOverrideLerpSpeedFactor = 0.025f;

    Camera.Camera.offset = Vector2.Zero;
    Camera.Camera.cameraOffsetLerpSpeed = 5f;

    previousTargetZoom = GameState.targetZoom;

    base.Initialize();
  }

  public override void Update()
  {
    TransitionState();

    if (input.MenuPause() && GameState.state == GameState.State.Play)
    {
      GameState.state = GameState.State.Pause;
    }

    base.Update();
  }

  private void TransitionState()
  {
    if (GameState.state == GameState.State.Play)
    {
      opacity = ColorHelper.FadeOpacity(opacity, 0f, 1f, 2f);

      Camera.Camera.targetZoomOverride = 1f;

      if (input.AdjustCameraZoom() == 0)
      {
        GameState.targetZoom = previousTargetZoom;
      }

      previousTargetZoom = GameState.targetZoom;
      hudOpacity = ColorHelper.FadeOpacity(hudOpacity, 0f, 1f, 0.2f);
      Camera.Camera.targetOffset = Vector2.Zero;
    }

    if (GameState.state == GameState.State.Pause)
    {
      Camera.Camera.zoomOverrideLerpSpeedFactor = 0.5f;
      Camera.Camera.targetZoomOverride = 1.5f;
      GameState.targetZoom = 1.26f;

      hudOpacity = ColorHelper.FadeOpacity(hudOpacity, 1f, 0f, 0.2f);

      Camera.Camera.targetOffset = cameraOffsetLeft;
    }

    if (GameState.state == GameState.State.TitleScreen)
    {
      opacity = ColorHelper.FadeOpacity(opacity, 1f, 0f, 2f);

      Camera.Camera.cameraOffsetLerpSpeed = 3f;

      Camera.Camera.zoomOverrideLerpSpeedFactor = 0.005f;
      Camera.Camera.targetZoomOverride = 20f;
    }

    if (Camera.Camera.zoomOverride > 10 && opacity <= 0f)
    {
      GameState.Initialize();

      SceneManager.RemoveScene();
      SceneManager.AddScene(new StartScene());

      Camera.Camera.zoomOverride = 1f;
      Camera.Camera.targetZoomOverride = 1f;

      return;
    }
  }
}
