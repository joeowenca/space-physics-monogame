using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpacePhysics.CustomGameComponent;
using static SpacePhysics.GameState;

namespace SpacePhysics.Camera;

public class ScreenSpace
{
  public static void DrawSpriteBatch(SpriteBatch spriteBatch, Matrix matrix, CustomGameComponent component)
  {
    spriteBatch.Begin(
      SpriteSortMode.Deferred,
      BlendState.AlphaBlend,
      SamplerState.AnisotropicClamp,
      DepthStencilState.Default,
      RasterizerState.CullNone,
      null,
      transformMatrix: matrix
    );

    component.Draw(spriteBatch);

    spriteBatch.End();
  }

  public static void DrawScreenSpace(SpriteBatch spriteBatch, CustomGameComponent component)
  {
    DrawSpriteBatch(spriteBatch, CreateMatrix(Vector2.Zero, 1f, Vector2.Zero), component);
  }

  public static void DrawHudSpace(SpriteBatch spriteBatch, CustomGameComponent component)
  {
    switch (component.alignment)
    {
      case Alignment.BottomCenter:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          (screenSize.X / 2) - (screenSize.X * scale / 2),
          screenSize.Y - (screenSize.Y * scale)),
          scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.TopCenter:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          (screenSize.X / 2) - (screenSize.X * scale / 2),
          0),
          scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.Left:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          0,
          (screenSize.Y / 2) - (screenSize.Y * scale / 2)),
          scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.Right:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          screenSize.X - (screenSize.X * scale),
          (screenSize.Y / 2) - (screenSize.Y * scale / 2)),
          scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.TopRight:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          screenSize.X - (screenSize.X * scale),
          0),
          scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.BottomRight:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          screenSize.X - (screenSize.X * scale),
          screenSize.Y - (screenSize.Y * scale)),
          scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.BottomLeft:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          0,
          screenSize.Y - (screenSize.Y * scale)),
          scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.Center:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          (screenSize.X / 2) - (screenSize.X * scale / 2),
          (screenSize.Y / 2) - (screenSize.Y * scale / 2)),
          scale,
          Vector2.Zero),
          component
        );
        break;

      default:
        DrawSpriteBatch(spriteBatch, CreateMatrix(Vector2.Zero, scale, Vector2.Zero), component);
        break;
    }
  }

  private static Matrix CreateMatrix(Vector2 position, float scale, Vector2 offset)
  {
    return
      Matrix.CreateTranslation(new Vector3(offset.X / scale, offset.Y / scale, 0)) *
      Matrix.CreateScale(scale) *
      Matrix.CreateTranslation(new Vector3(
        position.X,
        position.Y,
        0
      ));
  }
}
