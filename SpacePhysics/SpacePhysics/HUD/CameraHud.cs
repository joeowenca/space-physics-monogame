using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static SpacePhysics.GameState;

namespace SpacePhysics.HUD;

public class CameraHud : CustomGameComponent
{
    private Vector2 offset;

    public CameraHud(Func<float> opacity) : base(false, Alignment.TopCenter, 11)
    {
        offset = new Vector2(0, 350f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }
}
