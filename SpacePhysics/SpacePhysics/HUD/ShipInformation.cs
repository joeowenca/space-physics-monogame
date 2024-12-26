using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using static SpacePhysics.GameState;

namespace SpacePhysics.Debugging
{
  internal class ShipInformation : CustomGameComponent
  {
    private List<DebugItem> statusItems = new List<DebugItem>();

    private SpriteFont font;

    private Vector2 offset;

    private Func<float> opacity;

    public ShipInformation(Func<float> opacity) : base(false, Alignment.BottomRight, 11)
    {
      this.opacity = opacity;

      offset = new Vector2(screenSize.X - 1000f, screenSize.Y - 800f);

      statusItems.Add(new DebugItem("X", () => GameState.position.X.ToString()));
      statusItems.Add(new DebugItem("Y", () => GameState.position.Y.ToString()));

      for (int i = 0; i < statusItems.Count; i++)
      {
        statusItems[i].position = new Vector2(20, i * 140 * hudTextScale) + offset;
      }
    }

    public override void Load(ContentManager contentManager)
    {
      font = contentManager.Load<SpriteFont>("Fonts/text-font");

      base.Load(contentManager);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      foreach (var item in statusItems)
      {
        spriteBatch.DrawString(
          font,
          item.Label + ": ",
          item.position,
          defaultColor * opacity(),
          0f,
          Vector2.Zero,
          hudTextScale,
          SpriteEffects.None,
          0f
        );

        spriteBatch.DrawString(
          font,
          item.ValueGetter(),
          item.position + new Vector2(font.MeasureString(item.Label).X * hudTextScale + 30, 0),
          highlightColor * opacity(),
          0f,
          Vector2.Zero,
          hudTextScale,
          SpriteEffects.None,
          0f
        );
      }
    }
  }
}