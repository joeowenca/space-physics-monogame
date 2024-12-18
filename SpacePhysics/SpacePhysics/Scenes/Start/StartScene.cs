using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SpacePhysics.Menu;
using SpacePhysics.Player;
using SpacePhysics.Sprites;

namespace SpacePhysics.Scenes.Start;

public class StartScene : CustomGameComponent
{
  private SceneManager sceneManager;

  public static Vector2 offset;
  public static Vector2 targetOffset;

  private float opacity;
  private float menuOpacity;

  public StartScene(
    SceneManager sceneManager
  ) : base(true, Alignment.TopLeft, 7)
  {
    this.sceneManager = sceneManager;

    components.Add(new LoopingBackground(
      "Backgrounds/starfield",
      () => new Color(255, 255, 255, 0),
      1
    ));
    components.Add(new LoopingBackground(
      "Backgrounds/purple-background",
      () => new Color(100, 100, 100, 0),
      2
    ));
    components.Add(new LoopingBackground(
      "Backgrounds/purple-background-2",
      () => new Color(100, 100, 100, 0),
      4
    ));

    components.Add(new Ship(
      () => 1f,
      true,
      Alignment.TopLeft,
      7
    ));

    components.Add(new TitleMenu(
      true,
      Alignment.TopLeft,
      7
    ));
  }

  public override void Initialize()
  {
    base.Initialize();
  }

    Camera.Camera.allowInput = false;

    offset = new Vector2(GameState.screenSize.X / 10, -50);
    targetOffset = new Vector2(GameState.screenSize.X / 10, -50);

    base.Initialize();
  }

  public override void Update()
  {
    offset.X = MathHelper.Lerp(offset.X, targetOffset.X, 0.05f);
    offset.Y = MathHelper.Lerp(offset.Y, targetOffset.Y, 0.05f);

    Camera.Camera.offset = offset;

    opacity = ColorHelper.FadeOpacity(opacity, -0.25f, 1f, 4f);
    menuOpacity = ColorHelper.FadeOpacity(menuOpacity, -0.75f, 1f, 4.75f);

    base.Update();
  }
}
