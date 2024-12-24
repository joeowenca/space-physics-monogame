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

  public LoopingBackground(string textureName, Func<Color> color, int layerIndex) : base(false, Alignment.TopLeft, layerIndex)
  {
    this.textureName = textureName;
    this.color = color;
    parallaxFactor = layerIndex * 0.071375f;
  }

  public override void Load(ContentManager contentManager)
  {
    texture = contentManager.Load<Texture2D>(textureName);
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    Rectangle viewport = new Rectangle(
        (int)Camera.Camera.position.X,
        (int)Camera.Camera.position.Y,
        (int)screenSize.X,
        (int)screenSize.Y
    );

    width = texture.Width * scale * 2f;
    height = texture.Height * scale * 2f;

    int startX = (int)Math.Floor((float)(viewport.Left * parallaxFactor) / width) - 2;
    int startY = (int)Math.Floor((float)(viewport.Top * parallaxFactor) / height) - 2;
    int endX = (int)Math.Ceiling((float)(viewport.Right * parallaxFactor) / width) + 2;
    int endY = (int)Math.Ceiling((float)(viewport.Bottom * parallaxFactor) / height) + 2;

    for (int x = startX; x <= endX; x++)
    {
      for (int y = startY; y <= endY; y++)
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