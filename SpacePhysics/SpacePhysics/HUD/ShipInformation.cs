using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpacePhysics.GameState;

namespace SpacePhysics.HUD;

public class ShipInformation : CustomGameComponent
{
  private Vector2 offset;

  public ShipInformation(Func<float> opacity) : base(false, Alignment.BottomRight, 11)
  {
    offset = new Vector2(-1000f, -700f);

    components.Add(new HudText(
      "Fonts/text-font",
      () => "Ship status",
      Alignment.BottomRight,
      TextAlign.Left,
      () => offset,
      () => defaultColor * opacity(),
      hudTextScale,
      11
    ));
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    foreach (var component in components)
    {
      component.Draw(spriteBatch);
    }
  }
}
