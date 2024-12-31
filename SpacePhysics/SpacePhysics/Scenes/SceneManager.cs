using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace SpacePhysics.Scenes;

public static class SceneManager
{
  public static readonly Stack<CustomGameComponent> scenes;

  private static ContentManager contentManager;

  static SceneManager()
  {
    scenes = new();
  }

  public static void Initialize(ContentManager contentManager)
  {
    SceneManager.contentManager = contentManager;
  }

  public static void AddScene(CustomGameComponent scene)
  {
    scene.Initialize();
    scene.Load(contentManager);

    scenes.Push(scene);
  }

  public static void RemoveScene()
  {
    contentManager.Unload();
    scenes.Pop();
  }

  public static CustomGameComponent GetCurrentScene()
  {
    return scenes.Peek();
  }
}
