using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePhysics.Sprites;

public class AnimatedSprite : CustomGameComponent
{
  public Rectangle SourceRectangle => new Rectangle(
      currentFrame % columns * frameWidth,
      currentFrame / rows * frameHeight,
      frameWidth,
      frameHeight
  );

  private int rows;
  private int columns;
  private int animationSpeed;
  private int currentFrame;
  private int totalFrames;
  private int frameWidth;
  private int frameHeight;
  private int elapsedTime;
  public AnimatedSprite(Texture2D texture, int rows, int columns, int animationSpeed) : base()
  {
    this.texture = texture;
    this.rows = rows;
    this.columns = columns;
    this.animationSpeed = animationSpeed;
    frameWidth = texture.Width / columns;
    frameHeight = texture.Height / rows;
    totalFrames = rows * columns;
  }

  public void Update(Vector2 position)
  {
    this.position = position;

    elapsedTime += (int)GameState.deltaTime * 1000;

    if (elapsedTime >= animationSpeed)
    {
      currentFrame = (currentFrame + 1) % totalFrames;
      elapsedTime = 0;
    }
  }
}
