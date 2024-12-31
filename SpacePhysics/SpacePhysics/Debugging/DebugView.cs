using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Player;
using static SpacePhysics.GameState;

namespace SpacePhysics.Debugging
{
  internal class DebugView : CustomGameComponent
  {
    private List<DebugItem> debugItems = new List<DebugItem>();

    private SpriteFont font;

    private SystemUsage systemUsage = new SystemUsage();

    private float debugItemScale = hudTextScale * 1.9f;

    public DebugView() : base(true, Alignment.TopLeft, 0)
    {
      debugItems.Add(new DebugItem("System", () => ""));
      debugItems.Add(new DebugItem("FPS", () => FPS.ToString()));

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        debugItems.Add(new DebugItem("CPU", () => systemUsage.GetCpuUsage() + " %"));
        debugItems.Add(new DebugItem("Memory", () => systemUsage.GetRamUsage() / (1024 * 1024) + " MB"));
      }

      debugItems.Add(new DebugItem("", () => ""));
      debugItems.Add(new DebugItem("Player", () => ""));
      debugItems.Add(new DebugItem("X", () => GameState.position.X.ToString()));
      debugItems.Add(new DebugItem("Y", () => GameState.position.Y.ToString()));
      debugItems.Add(new DebugItem("Mass", () => Ship.mass.ToString()));
      debugItems.Add(new DebugItem("Velocity X", () => velocity.X.ToString()));
      debugItems.Add(new DebugItem("Velocity Y", () => velocity.Y.ToString()));
      debugItems.Add(new DebugItem("Velocity Magnitude", () => velocity.Length().ToString()));
      debugItems.Add(new DebugItem("Acceleration Magnitude", () => Ship.acceleration.Length().ToString()));
      debugItems.Add(new DebugItem("Forward Thrust", () => Ship.forwardThrust.ToString()));
      debugItems.Add(new DebugItem("Angular Velocity", () => angularVelocity.ToString()));
      debugItems.Add(new DebugItem("Prograde Angular Velocity", () => progradeAngularVelocity.ToString()));
      debugItems.Add(new DebugItem("Direction", () => direction.ToString()));
      debugItems.Add(new DebugItem("Pitch", () => Ship.pitch.ToString()));

      debugItems.Add(new DebugItem("", () => ""));
      debugItems.Add(new DebugItem("Camera", () => ""));
      debugItems.Add(new DebugItem("Camera X", () => Camera.Camera.position.X.ToString()));
      debugItems.Add(new DebugItem("Camera Y", () => Camera.Camera.position.Y.ToString()));
      debugItems.Add(new DebugItem("Camera Offset", () => Camera.Camera.offset.ToString()));
      debugItems.Add(new DebugItem("Camera Target Offset", () => Camera.Camera.targetOffset.ToString()));
      debugItems.Add(new DebugItem("Camera Zoom Speed", () => Camera.Camera.zoomSpeed.ToString()));
      debugItems.Add(new DebugItem("Zoom", () => zoom.ToString()));
      debugItems.Add(new DebugItem("Target Zoom", () => targetZoom.ToString()));
      debugItems.Add(new DebugItem("Zoom Override", () => Camera.Camera.zoomOverride.ToString()));
      debugItems.Add(new DebugItem("Target Zoom Override", () => Camera.Camera.targetZoomOverride.ToString()));

      debugItems.Add(new DebugItem("", () => ""));
      debugItems.Add(new DebugItem("Game State", () => ""));
      debugItems.Add(new DebugItem("State", () => state.ToString()));
      debugItems.Add(new DebugItem("Scene", () => sceneString));

      debugItems.Add(new DebugItem("", () => ""));
      debugItems.Add(new DebugItem("Controls", () => ""));
      debugItems.Add(new DebugItem("GamePad Connected", () => input.gamePadConnected.ToString()));
      debugItems.Add(new DebugItem("GamePad Left Stick X", () => input.AnalogStick().Left.X.ToString()));
      debugItems.Add(new DebugItem("GamePad Left Stick Y", () => input.AnalogStick().Left.Y.ToString()));
      debugItems.Add(new DebugItem("GamePad Right Stick X", () => input.AnalogStick().Right.X.ToString()));
      debugItems.Add(new DebugItem("GamePad Right Stick Y", () => input.AnalogStick().Right.Y.ToString()));

      for (int i = 0; i < debugItems.Count; i++)
      {
        debugItems[i].position = new Vector2(10, 5 + i * 18 * debugItemScale);
      }
    }

    public override void Load(ContentManager contentManager)
    {
      font = contentManager.Load<SpriteFont>("Fonts/regular-font");

      base.Load(contentManager);
    }

    public override void Update()
    {
      if (input.OnFirstFrameKeyPress(Keys.F3))
        debug = !debug;

      base.Update();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (debug)
      {
        foreach (var item in debugItems)
        {
          spriteBatch.DrawString(
            font,
            item.Label + (item.GetValue().Length > 0 ? ": " : ""),
            item.position,
            defaultColor,
            0f,
            Vector2.Zero,
            debugItemScale,
            SpriteEffects.None,
            0f
          );

          spriteBatch.DrawString(
            font,
            item.ValueGetter(),
            item.position + new Vector2(font.MeasureString(item.Label).X * debugItemScale + 10, 0),
            highlightColor,
            0f,
            Vector2.Zero,
            debugItemScale,
            SpriteEffects.None,
            0f
          );
        }
      }
    }
  }
}