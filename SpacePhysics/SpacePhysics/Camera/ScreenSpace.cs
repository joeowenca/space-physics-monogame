using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpacePhysics.CustomGameComponent;

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
          (GameState.screenSize.X / 2) - (GameState.screenSize.X * GameState.scale / 2),
          GameState.screenSize.Y - (GameState.screenSize.Y * GameState.scale)),
          GameState.scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.TopCenter:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          (GameState.screenSize.X / 2) - (GameState.screenSize.X * GameState.scale / 2),
          0),
          GameState.scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.Left:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          0,
          (GameState.screenSize.Y / 2) - (GameState.screenSize.Y * GameState.scale / 2)),
          GameState.scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.Right:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          GameState.screenSize.X - (GameState.screenSize.X * GameState.scale),
          (GameState.screenSize.Y / 2) - (GameState.screenSize.Y * GameState.scale / 2)),
          GameState.scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.TopRight:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          GameState.screenSize.X - (GameState.screenSize.X * GameState.scale),
          0),
          GameState.scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.BottomRight:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          GameState.screenSize.X - (GameState.screenSize.X * GameState.scale),
          GameState.screenSize.Y - (GameState.screenSize.Y * GameState.scale)),
          GameState.scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.BottomLeft:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          0,
          GameState.screenSize.Y - (GameState.screenSize.Y * GameState.scale)),
          GameState.scale,
          Vector2.Zero),
          component
        );
        break;

      case Alignment.Center:
        DrawSpriteBatch(spriteBatch, CreateMatrix(new Vector2(
          (GameState.screenSize.X / 2) - (GameState.screenSize.X * GameState.scale / 2),
          (GameState.screenSize.Y / 2) - (GameState.screenSize.Y * GameState.scale / 2)),
          GameState.scale,
          Vector2.Zero),
          component
        );
        break;

      default:
        DrawSpriteBatch(spriteBatch, CreateMatrix(Vector2.Zero, GameState.scale, Vector2.Zero), component);
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
