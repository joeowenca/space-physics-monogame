using System;
using Microsoft.Xna.Framework;
using SpacePhysics.Menu;
using SpacePhysics.Player;
using SpacePhysics.Sprites;
using SpacePhysics.Debugging;
using static SpacePhysics.Menu.MenuContainer;

namespace SpacePhysics.Scenes.Start;

public class StartScene : CustomGameComponent
{
  private SceneManager sceneManager;

  private float opacity;

  public StartScene(
    SceneManager sceneManager
  ) : base(true, Alignment.TopLeft, 7)
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

    components.Add(new Title(
      true,
      Alignment.Left,
      11
    ));

    components.Add(new TitleMenu(
      true,
      Alignment.Left,
      11
    ));

    components.Add(new MainMenu(
      true,
      Alignment.Left,
      11
    ));

    components.Add(new SettingsMenu(
      true,
      Alignment.Right,
      11
    ));

    components.Add(new Ship(
      () => opacity,
      false,
      Alignment.TopLeft,
      7
    ));

    components.Add(new DebugView());
  }

  public override void Initialize()
  {
    GameState.Intro();

    Camera.Camera.allowInput = false;

    cameraOffset = cameraOffsetLeft;

    base.Initialize();
  }

  public override void Update()
  {
    TransitionState();

    base.Update();
  }

  private void TransitionState()
  {
    if (GameState.state == GameState.State.Settings)
    {
      targetCameraOffset = cameraOffsetRight;
      targetMenuOffset = new Vector2(-menuOffsetAmount, 0);
    }
    else if (GameState.state == GameState.State.Play)
    {
      opacity = ColorHelper.FadeOpacity(opacity, 1f, 0f, 2f);

      Camera.Camera.targetZoomOverride = 20f;
    }
    else
    {
      opacity = ColorHelper.FadeOpacity(opacity, -0.25f, 1f, 4f);
      targetCameraOffset = cameraOffsetLeft;
      targetMenuOffset = new Vector2(menuOffsetAmount, 0);

      Camera.Camera.zoomOverride = 1f;
      Camera.Camera.targetZoomOverride = 1f;
    }

    if (Camera.Camera.zoomOverride > 10 && opacity <= 0f)
    {
      sceneManager.RemoveScene();
      sceneManager.AddScene(new Space.SpaceScene(sceneManager));

      Camera.Camera.zoomOverride = 0f;
      Camera.Camera.targetZoomOverride = 1f;

      return;
    }
  }
}
