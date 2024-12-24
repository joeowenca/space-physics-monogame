using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static SpacePhysics.GameState;

namespace SpacePhysics.Sprites;

public class LoopingBackground : CustomGameComponent
{
  private readonly string textureName;
  private readonly Func<Color> color;

  private float parallaxFactor;

  private float textureScale;

  public LoopingBackground(string textureName, Func<Color> color, float textureScale, int layerIndex) : base(false, Alignment.TopLeft, layerIndex)
  {
    this.textureName = textureName;
    this.color = color;
    this.textureScale = textureScale;

    parallaxFactor = layerIndex / 14f;
  }

  public override void Load(ContentManager contentManager)
  {
    texture = contentManager.Load<Texture2D>(textureName);
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    Vector4 viewport = new(
      Camera.Camera.position.X,
      Camera.Camera.position.Y,
      screenSize.X,
      screenSize.Y
    );

    float viewportLeft = viewport.X;
    float viewportRight = viewport.X + viewport.Z;
    float viewportTop = viewport.Y;
    float viewportBottom = viewport.W + viewport.Y;

    width = (int)(texture.Width * scale * textureScale);
    height = (int)(texture.Height * scale * textureScale);

    float startX = (float)Math.Floor(viewportLeft * parallaxFactor / width) - 2;
    float startY = (float)Math.Floor(viewportTop * parallaxFactor / height) - 2;
    float endX = (float)Math.Ceiling(viewportRight * parallaxFactor / width) + 2;
    float endY = (float)Math.Ceiling(viewportBottom * parallaxFactor / height) + 2;

    for (int x = (int)startX; x <= endX; x++)
    {
      for (int y = (int)startY; y <= endY; y++)
      {
        float tilePositionX = x * width + viewport.X * parallaxFactor;
        float tilePositionY = y * height + viewport.Y * parallaxFactor;


        Rectangle tileRectangle = new Rectangle(
          (int)tilePositionX,
          (int)tilePositionY,
          (int)width,
          (int)height
        );

        spriteBatch.Draw(texture, tileRectangle, color());
      }
    }
  }
}