using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpacePhysics.Menu;
using SpacePhysics.Scenes;

namespace SpacePhysics;

public class Main : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private SceneManager sceneManager;
    private InputManager input;

    public Main()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = (int)GameState.screenSize.X;
        graphics.PreferredBackBufferHeight = (int)GameState.screenSize.Y;
        graphics.IsFullScreen = true;
        IsFixedTimeStep = false;
        graphics.SynchronizeWithVerticalRetrace = true;
        graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        GraphicsDevice.PresentationParameters.MultiSampleCount = 4;

        GameState.Initialize();
        Camera.Camera.Initialize();
        MenuContainer.Initialize();

        input = new InputManager();

        sceneManager = new(Content);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        sceneManager.AddScene(new Scenes.Space.SpaceScene(sceneManager));
    }

    protected override void Update(GameTime gameTime)
    {
        input.Update();

        if (GameState.quit)
            Exit();

        sceneManager.GetCurrentScene().Update();

        MenuContainer.Update();
        Camera.Camera.Update();
        GameState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        sceneManager.GetCurrentScene().Draw(spriteBatch);

        base.Draw(gameTime);
    }
}
