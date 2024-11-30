using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SpacePhysics.Scenes.Start;

public class StartScene : CustomGameComponent
{
  private SceneManager sceneManager;
  private InputManager input;

  public static Vector2 offset;
  private Vector2 targetOffset;

  private float opacity;
  private float backgroundOpacity;

  public StartScene(
    SceneManager sceneManager
  ) : base(Alignment.TopLeft, 7)
  {
    this.sceneManager = sceneManager;
  }

  public override void Initialize()
  {
    input = new();
    offset = new Vector2(GameState.screenSize.X / 10, -50);
    targetOffset = new Vector2(GameState.screenSize.X / 10, -50);

    opacity = 0f;
    backgroundOpacity = 1f;

    Camera.Camera.allowInput = false;
    Camera.Camera.zoomOverrideLerpSpeed = 0.0001f;

    GameState.Initialize();

    foreach (var component in components)
    {
      component.Initialize();
    }
  }

  public override void Load(ContentManager contentManager)
  {
    foreach (var component in components)
    {
      component.Load(contentManager);
    }
  }

  public override void Update(GameTime gameTime)
  {
    GameState.Update(gameTime);
    input.Update();

    if (GameState.state == GameState.State.TitleScreen || GameState.state == GameState.State.MainMenu)
    {
      targetOffset = new Vector2(GameState.screenSize.X / 10, -50);
    }

    if (GameState.state == GameState.State.Settings)
    {
      targetOffset = new Vector2(-GameState.screenSize.X / 10, -50);
    }

    if (GameState.state == GameState.State.Play)
    {
      Camera.Camera.targetZoomOverride = 20;
      opacity = ColorHelper.FadeOpacity(opacity, 1f, 0f, 0f, 2f);
      backgroundOpacity = ColorHelper.FadeOpacity(backgroundOpacity, 1f, 0f, 0f, 2f);
    }

    opacity = ColorHelper.FadeOpacity(opacity, 0f, 1f, 0.5f, 2f);

    offset.X = MathHelper.Lerp(offset.X, targetOffset.X, 0.05f);
    offset.Y = MathHelper.Lerp(offset.Y, targetOffset.Y, 0.05f);

    Camera.Camera.offset = offset;

    foreach (var component in components)
    {
      component.Update(gameTime);
    }
  }
}
