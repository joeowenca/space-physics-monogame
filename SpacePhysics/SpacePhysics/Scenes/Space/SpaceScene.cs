using System;
using Microsoft.Xna.Framework;
using SpacePhysics.HUD;
using SpacePhysics.Player;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Sprites;
using SpacePhysics.Scenes.Start;
using SpacePhysics.Debugging;

namespace SpacePhysics.Scenes.Space;

public class SpaceScene : CustomGameComponent
{
  SceneManager sceneManager;

  private float opacity;
  private float cameraHudOpacity;
  private float cameraHudShadowOpacity;
  private float cameraAngleHudOpacity;
  private float cameraHudTime;
  private float cameraAngleHudTime;

  public SpaceScene(SceneManager sceneManager) : base(true, Alignment.TopLeft, 7)
  {
    this.sceneManager = sceneManager;

    components.Add(new LoopingBackground(
      "Backgrounds/starfield",
      () => new Color(255, 255, 255, 0) * opacity,
      1
    ));
    components.Add(new LoopingBackground(
      "Backgrounds/purple-background",
      () => new Color(100, 100, 100, 0) * opacity,
      2
    ));
    components.Add(new LoopingBackground(
      "Backgrounds/purple-background-2",
      () => new Color(100, 100, 100, 0) * opacity,
      4
    ));

    components.Add(new Ship(
      () => opacity,
      true,
      Alignment.TopLeft,
      7
    ));

    components.Add(new Gauge(
      () => opacity
    ));

    components.Add(new Meter(
      () => opacity
    ));

    components.Add(new HudSprite(
        "HUD/hud-shadow",
        Alignment.TopCenter,
        Alignment.Center,
        () => new Vector2(0f, 500f),
        () => (float)Math.PI,
        () => Color.White * 0.75f * cameraHudShadowOpacity,
        GameState.hudScale * 0.5f,
        11
    ));

    components.Add(new CameraHud(
      () => opacity * cameraHudOpacity
    ));

    components.Add(new CameraAngleHud(
      () => opacity * cameraAngleHudOpacity
    ));

    components.Add(new DebugView());
  }

  public override void Initialize()
  {
    GameState.Initialize();

    Camera.Camera.allowInput = true;
    Camera.Camera.offset = Vector2.Zero;
    Camera.Camera.zoomOverrideLerpSpeedFactor = 0.025f;

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

    base.Update();
  }
}
