using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.HUD;

namespace SpacePhysics.Menu;

public class TitleMenu : CustomGameComponent
{
  public TitleMenu(
    bool allowInput,
    Alignment alignment,
    int layerIndex) : base(
      allowInput,
      alignment,
      layerIndex
    )
  {
    components.Add(new HudText(
      "Fonts/title-font",
      () => "Space Physics",
      Alignment.TopLeft,
      TextAlign.Left,
      () => position,
      () => Color.White,
      1f,
      11
    ));
  }

  public override void Initialize()
  {
    base.Initialize();
  }

  public override void Load(ContentManager contentManager)
  {
    base.Load(contentManager);
  }

  public override void Update()
  {
    height = MenuContainer.CalculateMenuHeight(components);

    base.Update();
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    foreach (var component in components)
    {
      component.Draw(spriteBatch);
    }
  }
}
