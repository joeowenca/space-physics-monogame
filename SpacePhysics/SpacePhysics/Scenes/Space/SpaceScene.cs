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
  SceneManager sceneManager;

  private float opacity;
  private float hudOpacity;
  private float cameraHudOpacity;
  private float cameraHudShadowOpacity;
  private float cameraAngleHudOpacity;
  private float cameraHudTime;
  private float cameraAngleHudTime;
  private float previousTargetZoom;

  public SpaceScene(SceneManager sceneManager) : base(true, Alignment.TopLeft, 7)
  {
    this.sceneManager = sceneManager;

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

    components.Add(new Gauge(
      () => opacity * hudOpacity
    ));

    components.Add(new Meter(
      () => opacity * hudOpacity
    ));

    components.Add(new HudSprite(
        "HUD/hud-shadow",
        Alignment.TopCenter,
        Alignment.Center,
        () => new Vector2(0f, 500f),
        () => (float)Math.PI,
        () => Color.White * 0.75f * cameraHudShadowOpacity * hudOpacity,
        GameState.hudScale * 0.5f,
        11
    ));

    components.Add(new CameraHud(
      () => opacity * cameraHudOpacity * hudOpacity
    ));

    components.Add(new CameraAngleHud(
      () => opacity * cameraAngleHudOpacity * hudOpacity
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

    Camera.Camera.allowInput = true;
    Camera.Camera.offset = Vector2.Zero;
    Camera.Camera.zoomOverrideLerpSpeedFactor = 0.025f;

    cameraOffset = Vector2.Zero;

    previousTargetZoom = GameState.targetZoom;

    base.Initialize();
  }

  public override void Update()
  {
    if (GameState.state == GameState.State.Play)
    {
      opacity = ColorHelper.FadeOpacity(opacity, 0f, 1f, 2f);
    }

    if (input.ContinuousPress(Keys.OemMinus) || input.ContinuousPress(Keys.OemPlus))
    {
      cameraHudOpacity = 1f;
      cameraHudTime = GameState.elapsedTime;
    }
    else
    {
      if (GameState.elapsedTime > cameraHudTime + 2)
      {
        cameraHudOpacity = ColorHelper.FadeOpacity(cameraHudOpacity, 1f, 0f, StartScene.transitionSpeed);
      }
    }

    if (input.ContinuousPress(Keys.V))
    {
      cameraAngleHudOpacity = 1f;
      cameraAngleHudTime = GameState.elapsedTime;
    }
    else
    {
      if (GameState.elapsedTime > cameraAngleHudTime + 2)
      {
        cameraAngleHudOpacity = ColorHelper.FadeOpacity(cameraAngleHudOpacity, 1f, 0f, StartScene.transitionSpeed);
      }
    }

    if (cameraAngleHudOpacity > 0 && cameraAngleHudOpacity > cameraHudOpacity)
    {
      cameraHudShadowOpacity = cameraAngleHudOpacity;
    }

    if (cameraHudOpacity > 0 && cameraHudOpacity > cameraAngleHudOpacity)
    {
      cameraHudShadowOpacity = cameraHudOpacity;
    }

    if (input.OnFirstFramePress(Keys.Escape))
    {
      GameState.state = GameState.State.Pause;
    }

    if (GameState.state == GameState.State.Pause)
    {
      Camera.Camera.zoomOverrideLerpSpeedFactor = 0.5f;
      Camera.Camera.targetZoomOverride = 1.5f;
      GameState.targetZoom = 1.26f;

      hudOpacity = ColorHelper.FadeOpacity(hudOpacity, 1f, 0f, 0.2f);

      targetCameraOffset = cameraOffsetLeft;
    }
    else
    {
      Camera.Camera.targetZoomOverride = 1f;

      if (!input.ContinuousPress(Keys.OemMinus) && !input.ContinuousPress(Keys.OemPlus))
      {
        GameState.targetZoom = previousTargetZoom;
      }

      previousTargetZoom = GameState.targetZoom;
      hudOpacity = ColorHelper.FadeOpacity(hudOpacity, 0f, 1f, 0.2f);
      targetCameraOffset = Vector2.Zero;
    }

    cameraOffset.X = MathHelper.Lerp(cameraOffset.X, targetCameraOffset.X, GameState.deltaTime * 5f);
    cameraOffset.Y = MathHelper.Lerp(cameraOffset.Y, targetCameraOffset.Y, GameState.deltaTime * 5f);

    Camera.Camera.offset = cameraOffset;

    base.Update();
  }
}
