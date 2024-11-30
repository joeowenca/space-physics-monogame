using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.Camera;

namespace SpacePhysics;

public class CustomGameComponent
{
  public enum Alignment
  {
    Left,
    Right,
    Center,
    BottomLeft,
    BottomRight,
    BottomCenter,
    TopLeft,
    TopRight,
    TopCenter
  }

  public List<CustomGameComponent> components = [];

  public Texture2D texture;
  public Vector2 position;
  public int layerIndex;
  public Alignment alignment;

  public CustomGameComponent(Alignment alignment = Alignment.TopLeft, int layerIndex = 0)
  {
    this.alignment = alignment;
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
      if (component.layerIndex >= 1 || component.layerIndex <= 10)
      {
        ScreenSpace.DrawSpriteBatch(
          spriteBatch,
          Camera.Camera.GetViewMatrix((float)component.layerIndex / 7),
          component
        );
      }

      if (component.layerIndex == 0)
      {
        ScreenSpace.DrawScreenSpace(spriteBatch, component);
      }

      if (component.layerIndex == 11)
      {
        ScreenSpace.DrawHudSpace(spriteBatch, component);
      }
    }
  }
}
