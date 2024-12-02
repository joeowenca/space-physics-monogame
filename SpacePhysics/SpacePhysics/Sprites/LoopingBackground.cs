using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
        (int)GameState.screenSize.X,
        (int)GameState.screenSize.Y
    );

    int startX = (int)Math.Floor((float)(viewport.Left * parallaxFactor) / texture.Width) - 2;
    int startY = (int)Math.Floor((float)(viewport.Top * parallaxFactor) / texture.Height) - 2;
    int endX = (int)Math.Ceiling((float)(viewport.Right * parallaxFactor) / texture.Width) + 2;
    int endY = (int)Math.Ceiling((float)(viewport.Bottom * parallaxFactor) / texture.Height) + 2;

    for (int x = startX; x <= endX; x++)
    {
      for (int y = startY; y <= endY; y++)
      {
        float tilePositionX = x * texture.Width + viewport.X * parallaxFactor;
        float tilePositionY = y * texture.Height + viewport.Y * parallaxFactor;


        Rectangle tileRectangle = new Rectangle(
          (int)tilePositionX,
          (int)tilePositionY,
          texture.Width,
          texture.Height
        );

        spriteBatch.Draw(texture, tileRectangle, color());
      }
    }
  }
}