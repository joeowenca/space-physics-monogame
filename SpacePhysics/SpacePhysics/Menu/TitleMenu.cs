using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.HUD;
using SpacePhysics.Scenes.Start;
using static SpacePhysics.GameState;

namespace SpacePhysics.Menu;

public class TitleMenu : CustomGameComponent
{
  private Vector2 offset;

  private float opacity;

  public TitleMenu(
    bool allowInput,
    Alignment alignment,
    int layerIndex) : base(
      allowInput,
      alignment,
      layerIndex
    )
  {
    offset = new Vector2(800 + MenuContainer.menuOffsetX, 0);

    components.Add(new HudText(
      "Fonts/light-font",
      () => "PRESS ANY KEY",
      alignment,
      TextAlign.Left,
      () => new Vector2(1250, 300) + offset,
      () => Color.White * opacity,
      1.4f,
      11
    ));
  }

  public override void Initialize()
  {
    opacity = -2f;

    base.Initialize();
  }

  public override void Update()
  {
    TransitionState();

    base.Update();
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    foreach (var component in components)
    {
      component.Draw(spriteBatch);
    }
  }

  private void TransitionState()
  {
    if (state != State.TitleScreen)
    {
      opacity = ColorHelper.FadeOpacity(opacity, 0.9f, 0f, StartScene.transitionSpeed);
    }
    else
    {
      opacity = ColorHelper.FadeOpacity(opacity, -2f, 0.9f, 5.5f);
    }

    if (Keyboard.GetState().GetPressedKeys().Length > 0 && opacity >= 0.5f)
    {
      state = State.MainMenu;
    }
  }
}
