using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.HUD;
using SpacePhysics.Player;
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

      components.Add(new HudSprite(
        "HUD/hud-shadow-bottom-right",
        Alignment.BottomRight,
        Alignment.BottomRight,
        () => new Vector2(0f, 0f),
        () => 0f,
        () => Color.White * 0.75f * opacity(),
        () => hudScale * 2.5f,
        11
    ));

      components.Add(new HudText(
        "Fonts/text-font",
        () => "Status",
        Alignment.TopLeft,
        TextAlign.Left,
        () => offset,
        () => defaultColor * opacity(),
        () => hudTextScale * 1.3f,
        11
      ));

      statusItems.Add(new DebugItem("Mass", () => Ship.mass.ToString("0") + " kg"));
      statusItems.Add(new DebugItem("Liquid Fuel", () => fuel.ToString("0") + " L"));
      statusItems.Add(new DebugItem("Mono Propellant", () => mono.ToString("0") + " L"));
      statusItems.Add(new DebugItem("Electricity", () => electricity.ToString("0") + " kWh"));

      UpdateOffset();
    }

    public override void Load(ContentManager contentManager)
    {
      font = contentManager.Load<SpriteFont>("Fonts/text-font");

      base.Load(contentManager);
    }

    public override void Update()
    {
      if (Main.graphicsApplied)
      {
        UpdateOffset();
      }

      base.Update();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      foreach (var component in components)
      {
        component.Draw(spriteBatch);
      }

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

    private void UpdateOffset()
    {
      offset = new Vector2(screenSize.X - 900f, screenSize.Y - 600f);

      for (int i = 0; i < statusItems.Count; i++)
      {
        statusItems[i].position = new Vector2(3f, i * 140f * hudTextScale + 110f) + offset;
      }
    }
  }
}