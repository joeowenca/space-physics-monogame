using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace SpacePhysics.Scenes;

public class SceneManager
{
  private readonly Stack<CustomGameComponent> scenes;
  private ContentManager contentManager;

  public SceneManager(ContentManager contentManager)
  {
    this.contentManager = contentManager;
    scenes = new();
  }

  public void AddScene(CustomGameComponent scene)
  {
    scene.Initialize();
    scene.Load(contentManager);

    scenes.Push(scene);
  }

  public void RemoveScene()
  {
    contentManager.Unload();
    scenes.Pop();
  }

  public CustomGameComponent GetCurrentScene()
  {
    return scenes.Peek();
  }
}
