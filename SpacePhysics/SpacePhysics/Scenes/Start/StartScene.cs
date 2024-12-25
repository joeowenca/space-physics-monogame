using System;
using Microsoft.Xna.Framework;
using SpacePhysics.Menu;
using SpacePhysics.Player;
using SpacePhysics.Sprites;
using SpacePhysics.Debugging;

namespace SpacePhysics.Scenes.Start;

public class StartScene : CustomGameComponent
{
  private SceneManager sceneManager;

  private Vector2 offset;
  private Vector2 targetOffset;

  public static Vector2 menuOffset;
  private Vector2 targetMenuOffset;
  private int menuOffsetAmount;

  public static float transitionSpeed;

  public static float menuOffsetX;

  private float opacity;

  public StartScene(
    SceneManager sceneManager
  ) : base(true, Alignment.TopLeft, 7)
  {
    this.sceneManager = sceneManager;

    float start = 1250f;
    float end = 1f;

    menuOffsetX = start - (GameState.hudScaleOverrideFactor - 0.1f) * Math.Abs((end - start) / 0.9f);

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

    offset = new Vector2(GameState.screenSize.X * 0.12f, -GameState.screenSize.Y * 0.05f);
    targetOffset = new Vector2(GameState.screenSize.X * 0.12f, -GameState.screenSize.Y * 0.05f);

    menuOffsetAmount = 300;
    menuOffset = new Vector2(menuOffsetAmount, 0);
    targetMenuOffset = new Vector2(menuOffsetAmount, 0);

    transitionSpeed = 0.6f;

    base.Initialize();
  }

  public override void Update()
  {
    TransitionState();

    offset.X = MathHelper.Lerp(offset.X, targetOffset.X, GameState.deltaTime * 3f);
    offset.Y = MathHelper.Lerp(offset.Y, targetOffset.Y, GameState.deltaTime * 3f);

    menuOffset.X = MathHelper.Lerp(menuOffset.X, targetMenuOffset.X, GameState.deltaTime * 3f);
    menuOffset.Y = MathHelper.Lerp(menuOffset.Y, targetMenuOffset.Y, GameState.deltaTime * 3f);

    Camera.Camera.offset = offset;

    base.Update();
  }

  private void TransitionState()
  {
    if (GameState.state == GameState.State.Settings)
    {
      targetOffset = new Vector2(-GameState.screenSize.X * 0.12f, -GameState.screenSize.Y * 0.05f);
      targetMenuOffset = new Vector2(-menuOffsetAmount, 0);
    }
    else if (GameState.state == GameState.State.Play)
    {
      opacity = ColorHelper.FadeOpacity(opacity, 1f, 0f, 2f);
    }
    else
    {
      opacity = ColorHelper.FadeOpacity(opacity, -0.25f, 1f, 4f);
      targetOffset = new Vector2(GameState.screenSize.X * 0.12f, -GameState.screenSize.Y * 0.05f);
      targetMenuOffset = new Vector2(menuOffsetAmount, 0);
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
