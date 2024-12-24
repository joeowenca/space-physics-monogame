using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpacePhysics.Menu;
using SpacePhysics.Scenes;

namespace SpacePhysics;

public class Main : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private SceneManager sceneManager;
    private InputManager input;

    private GameState.State previousState;

    public Main()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = (int)GameState.screenSize.X;
        graphics.PreferredBackBufferHeight = (int)GameState.screenSize.Y;
        graphics.IsFullScreen = true;
        IsFixedTimeStep = false;
        graphics.SynchronizeWithVerticalRetrace = false;
        graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        GraphicsDevice.PresentationParameters.MultiSampleCount = 4;

        GameState.Initialize();
        Camera.Camera.Initialize();

        previousState = GameState.State.TitleScreen;

        input = new InputManager();

        sceneManager = new(Content);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        sceneManager.AddScene(new Scenes.Start.StartScene(sceneManager));
    }

    protected override void Update(GameTime gameTime)
    {
        input.Update();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || MainMenu.quit)
            Exit();

        GameState.Update(gameTime);

        Camera.Camera.Update();

        sceneManager.GetCurrentScene().Update();

        if (!GameState.paused)
        {
            previousState = GameState.state;
        }

        if (input.OnFirstFramePress(Keys.Q)
            && (GameState.state == GameState.State.Play
            || GameState.state == GameState.State.Pause))
        {
            GameState.paused = !GameState.paused;
        }

        GameState.state = previousState;

        if (GameState.paused)
        {
            GameState.state = GameState.State.Pause;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        sceneManager.GetCurrentScene().Draw(spriteBatch);

        base.Draw(gameTime);
    }
}
