using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpacePhysics;

public class Main : Game
{
    public static Vector2 screenSize = new Vector2(1560, 1440);
    public static float FPS;

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    public Main()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = (int)screenSize.X;
        graphics.PreferredBackBufferHeight = (int)screenSize.Y;
        graphics.IsFullScreen = true;
        graphics.PreferMultiSampling = true;
        graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        GraphicsDevice.PresentationParameters.MultiSampleCount = 4;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (gameTime.ElapsedGameTime.TotalSeconds > 0)
        {
            FPS = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);


        base.Draw(gameTime);
    }
}
