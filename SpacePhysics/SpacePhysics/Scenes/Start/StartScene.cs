using System;
using Microsoft.Xna.Framework;
using SpacePhysics.Menu;
using SpacePhysics.Player;
using SpacePhysics.Sprites;

namespace SpacePhysics.Scenes.Start;

public class StartScene : CustomGameComponent
{
  private SceneManager sceneManager;

  public static Vector2 offset;
  public static Vector2 targetOffset;

  public static float transitionSpeed;

  private float opacity;

  public StartScene(
    SceneManager sceneManager
  ) : base(true, Alignment.TopLeft, 7)
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
      false,
      Alignment.TopLeft,
      7
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
  }

  public override void Initialize()
  {
    GameState.Intro();

    Camera.Camera.allowInput = false;

    offset = new Vector2(GameState.screenSize.X * 0.12f, -GameState.screenSize.Y * 0.05f);
    targetOffset = new Vector2(GameState.screenSize.X * 0.12f, -GameState.screenSize.Y * 0.05f);

    transitionSpeed = 0.33f;

    base.Initialize();
  }

  public override void Update()
  {
    if (GameState.state == GameState.State.Settings)
    {
      targetOffset = new Vector2(-GameState.screenSize.X * 0.12f, -GameState.screenSize.Y * 0.05f);
    }
    else
    {
      targetOffset = new Vector2(GameState.screenSize.X * 0.12f, -GameState.screenSize.Y * 0.05f);
    }

    offset.X = MathHelper.Lerp(offset.X, targetOffset.X, GameState.deltaTime * 4f);
    offset.Y = MathHelper.Lerp(offset.Y, targetOffset.Y, GameState.deltaTime * 4f);

    Camera.Camera.offset = offset;

    opacity = ColorHelper.FadeOpacity(opacity, -0.25f, 1f, 4f);

    base.Update();
  }
}
