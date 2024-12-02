using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SpacePhysics.Sprites;
using static SpacePhysics.GameState;

namespace SpacePhysics.Scenes.Start;

public class StartScene : CustomGameComponent
{
  private SceneManager sceneManager;

  public static Vector2 offset;
  private Vector2 targetOffset;

  private float opacity;
  private float backgroundOpacity;

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
      1
    ));
    components.Add(new LoopingBackground(
      "Backgrounds/starfield",
      () => new Color(160, 160, 160, 0),
      1
    ));
  }

  public override void Initialize()
  {
    offset = new Vector2(screenSize.X / 10, -50);
    targetOffset = new Vector2(screenSize.X / 10, -50);

    opacity = 0f;
    backgroundOpacity = 1f;

    Camera.Camera.allowInput = false;
    Camera.Camera.zoomOverrideLerpSpeed = 0.0001f;

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

  public override void Update()
  {
    if (state == State.TitleScreen || state == State.MainMenu)
    {
      targetOffset = new Vector2(screenSize.X / 10, -50);
    }

    if (state == State.Settings)
    {
      targetOffset = new Vector2(-screenSize.X / 10, -50);
    }

    if (state == State.Play)
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
      component.Update();
    }
  }
}
