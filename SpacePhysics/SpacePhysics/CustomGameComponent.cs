using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePhysics;

public class CustomGameComponent
{
  public List<CustomGameComponent> components = [];

  private int layerIndex;
  public CustomGameComponent(int layerIndex = 0)
  {
    this.layerIndex = layerIndex;
  }

  public virtual void Initialize()
  {
    foreach (var component in components)
    {
      component.Initialize();
    }
  }

  public virtual void Load(ContentManager contentManager)
  {
    foreach (var component in components)
    {
      component.Load(contentManager);
    }
  }

  public virtual void Update(GameTime gameTime)
  {
    foreach (var component in components)
    {
      component.Update(gameTime);
    }
  }

  public virtual void Draw(SpriteBatch spriteBatch)
  {
    foreach (var component in components)
    {
      component.Draw(spriteBatch);
    }
  }
}
