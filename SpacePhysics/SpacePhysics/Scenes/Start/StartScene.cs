using System;
using Microsoft.Xna.Framework;

namespace SpacePhysics.Scenes.Start;

public class StartScene : CustomGameComponent
{
  private SceneManager sceneManager;
  private InputManager input;

  public static Vector2 offset;
  private Vector2 targetOffset;

  private float opacity;
  private float backgroundOpacity;
  private float targetBackgroundOpacity;

  public StartScene(
    SceneManager sceneManager,
    int layerIndex
  ) : base(Alignment.TopLeft, layerIndex)
  {
    this.sceneManager = sceneManager;
  }

  public override void Initialize()
  {
    input = new();
    offset = new Vector2(Main.screenSize.X / 10, -50);
    targetOffset = new Vector2(Main.screenSize.X / 10, -50);

    Camera.Camera.allowInput = false;
    Camera.Camera.zoomOverrideLerpSpeed = 0.0001f;

    GameState.Initialize();
  }
}
