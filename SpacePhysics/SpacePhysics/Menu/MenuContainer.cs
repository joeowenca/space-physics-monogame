using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using static SpacePhysics.Camera.Camera;

namespace SpacePhysics.Menu;

public class MenuContainer
{
  public static Vector2 cameraOffsetLeft;
  public static Vector2 cameraOffsetRight;

  public static Vector2 menuOffset;
  public static Vector2 targetMenuOffset;

  public static float menuOffsetXLeft;
  public static float menuOffsetXRight;

  public static float menuOffsetFactor;

  public static int menuOffsetAmount;

  public static float padding = 0.17f;
  public static float menuSizeY = 1000f * padding;
  public static float menuOffsetX;

  public static void Initialize()
  {
    cameraOffsetLeft = new Vector2(GameState.screenSize.X * 0.12f, -GameState.screenSize.Y * 0.05f);
    cameraOffsetRight = new Vector2(-GameState.screenSize.X * 0.12f, -GameState.screenSize.Y * 0.05f);

    cameraOffsetLerpSpeed = 3f;

    menuOffsetAmount = 300;
    menuOffset = new Vector2(menuOffsetAmount, 0);
    targetMenuOffset = new Vector2(menuOffsetAmount, 0);

    float start = 1250f;
    float end = 1f;
    menuOffsetX = start - (GameState.hudScaleOverrideFactor - 0.1f) * Math.Abs((end - start) / 0.9f);

    menuOffsetXLeft = 1050f + menuOffsetX;
    menuOffsetXRight = -1700f - menuOffsetX;
  }

  public static void Update()
  {
    menuOffset.X = MathHelper.Lerp(menuOffset.X, targetMenuOffset.X, GameState.deltaTime * 3f);
    menuOffset.Y = MathHelper.Lerp(menuOffset.Y, targetMenuOffset.Y, GameState.deltaTime * 3f);

    menuOffsetFactor = menuOffset.X * 0.85f * 3f;
  }

  public static float CalculateMenuHeight(List<CustomGameComponent> menuItems)
  {
    float totalHeight = 0;
    float previousPositionY = 0;
    float previousHeight = 0;

    foreach (var menuItem in menuItems)
    {
      totalHeight += menuItem.height;

      if (Math.Abs(previousPositionY) > 0 || previousHeight > 0)
      {
        totalHeight += menuItem.position.Y - previousPositionY - previousHeight;
      }

      previousPositionY = menuItem.position.Y;
      previousHeight = menuItem.height;
    }

    return totalHeight;
  }

  public static Vector2 CenterMenu(List<CustomGameComponent> menuItems)
  {
    return new Vector2(
      0f,
      GameState.screenSize.Y * GameState.scale - CalculateMenuHeight(menuItems) / 2
    );
  }
}
