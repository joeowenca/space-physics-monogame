using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.Menu;
using SpacePhysics.Scenes;

namespace SpacePhysics;

public class Main : Game
{
    public static GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    public static bool applyGraphics;

    public Main()
    {
        IsFixedTimeStep = false;
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        GraphicsDevice.PresentationParameters.MultiSampleCount = 4;

        ApplyGraphics();

        applyGraphics = false;

        SceneManager.Initialize(Content);

        SettingsState.Initialize();
        GameState.Initialize();
        Camera.Camera.Initialize();
        MenuContainer.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        SceneManager.AddScene(new Scenes.Start.StartScene());
    }

    protected override void Update(GameTime gameTime)
    {
        if (GameState.quit)
            Exit();

        if (applyGraphics)
        {
            ApplyGraphics();
            applyGraphics = false;
        }

        SceneManager.GetCurrentScene().Update();

        MenuContainer.Update();
        Camera.Camera.Update();
        GameState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        SceneManager.GetCurrentScene().Draw(spriteBatch);

        base.Draw(gameTime);
    }

    private void ApplyGraphics()
    {
        GameState.UpdateScale();

        int width = (int)SettingsState.GetResolutionVector().X;
        int height = (int)SettingsState.GetResolutionVector().Y;
        bool vsync = SettingsState.vsync;
        bool fullscreen = true;

        graphics.PreferredBackBufferWidth = width;
        graphics.PreferredBackBufferHeight = height;
        graphics.SynchronizeWithVerticalRetrace = vsync;
        graphics.IsFullScreen = fullscreen;

        GraphicsDevice.Viewport = new Viewport(0, 0, width, height);

        graphics.ApplyChanges();
    }
}
