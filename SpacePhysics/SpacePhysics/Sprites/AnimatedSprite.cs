using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpacePhysics.Sprites;

public class AnimatedSprite : CustomGameComponent
{
  public Rectangle SourceRectangle => new Rectangle(
      currentFrame % columns * frameWidth,
      currentFrame / columns * frameHeight,
      frameWidth,
      frameHeight
  );

  private int rows;
  private int columns;
  private float animationSpeed;
  private int currentFrame;
  private int totalFrames;
  private int frameWidth;
  private int frameHeight;
  private float elapsedTime;
  public AnimatedSprite(Texture2D texture, int rows, int columns, float animationSpeed) : base()
  {
    this.texture = texture;
    this.rows = rows;
    this.columns = columns;
    this.animationSpeed = animationSpeed;
    frameWidth = texture.Width / columns;
    frameHeight = texture.Height / rows;
    totalFrames = rows * columns;
  }

  public void Animate()
  {
    elapsedTime += GameState.deltaTime;

    if (elapsedTime >= animationSpeed)
    {
      currentFrame = (currentFrame + 1) % totalFrames;
      elapsedTime -= animationSpeed;
    }
  }
}
