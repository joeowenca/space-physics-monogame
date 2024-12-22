using System;
using Microsoft.Xna.Framework;
using SpacePhysics.HUD;
using SpacePhysics.Player;
using SpacePhysics.Scenes.Start;
using SpacePhysics.Sprites;

namespace SpacePhysics.Scenes.Space;

public class SpaceScene : CustomGameComponent
{
  SceneManager sceneManager;

  private float opacity;

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

    components.Add(new CameraHud(
      () => opacity
    ));
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

    base.Update();
  }
}
