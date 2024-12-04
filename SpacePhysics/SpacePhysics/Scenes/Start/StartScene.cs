using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SpacePhysics.Player;
using SpacePhysics.Sprites;

namespace SpacePhysics.Scenes.Start;

public class StartScene : CustomGameComponent
{
  private SceneManager sceneManager;

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

    components.Add(new Ship(() => 1f, true, Alignment.TopLeft, 7));
  }

  public override void Initialize()
  {
    base.Initialize();
  }

  public override void Load(ContentManager contentManager)
  {
    base.Load(contentManager);
  }

  public override void Update()
  {
    base.Update();
  }
}
