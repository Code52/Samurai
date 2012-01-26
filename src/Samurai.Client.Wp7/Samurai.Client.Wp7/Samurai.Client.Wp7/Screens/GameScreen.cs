using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Samurai.Client.Wp7.Graphics;
using SamuraiServer.Data;
using SamuraiServer.Data.Tiles;
using Microsoft.Xna.Framework.Input.Touch;

namespace Samurai.Client.Wp7.Screens
{
    public class GameScreen : BaseScreen
    {
        private ContentManager content;
        private SpriteBatch sb;
        private Renderer renderer;
        // For map scrolling
        private int xOffset = 0;
        private int yOffset = 0;
        private Vector2 prevPos = Vector2.Zero;

        // TESTING
        private Map fakemap;

        public GameScreen()
            : base()
        {
            renderer = new Renderer();
        }

        public override void LoadContent()
        {
            if (IsReady)
                return;

            content = new ContentManager(Manager.Game.Services, "Content");
            sb = new SpriteBatch(Manager.GraphicsDevice);

            Manager.Jobs.CreateJob(
                () =>
                {
                    renderer.LoadContent(content);

                    // TESTING
                    fakemap = new Map();
                    fakemap.Tiles = new TileType[50][];
                    for (int x = 0; x < fakemap.Tiles.Length; x++)
                    {
                        fakemap.Tiles[x] = new TileType[50];
                        for (int y = 0; y < fakemap.Tiles[x].Length; y++)
                        {
                            fakemap.Tiles[x][y] = new Grass();
                        }
                    }

                    // This indicates that the screen has finished loading and can be displayed without issues
                    IsReady = true;
                });
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            content.Unload();
            content.Dispose();
            sb.Dispose();
            base.UnloadContent();
        }

        public override void Update(double elapsedSeconds)
        {
            // TODO: Replace with proper logic once implemented
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
                Manager.ExitGame();

            // TESTING
            var touches = TouchPanel.GetState();
            if (touches.Count > 0)
            {
                var mapSize = renderer.GetMapSize(fakemap);
                if (touches[0].State == TouchLocationState.Pressed)
                    prevPos = touches[0].Position;
                else if (touches[0].State == TouchLocationState.Moved)
                {
                    xOffset -= (int)(touches[0].Position.X - prevPos.X);
                    if (xOffset < 0)
                        xOffset = 0;
                    else if (xOffset >= (mapSize.X - Manager.GraphicsDevice.Viewport.Width))
                        xOffset = mapSize.X - Manager.GraphicsDevice.Viewport.Width;

                    yOffset -= (int)(touches[0].Position.Y - prevPos.Y);
                    if (yOffset < 0)
                        yOffset = 0;
                    else if (yOffset >= (mapSize.Y - Manager.GraphicsDevice.Viewport.Height))
                        yOffset = mapSize.Y - Manager.GraphicsDevice.Viewport.Height;
                    prevPos = touches[0].Position;
                }
            }

            base.Update(elapsedSeconds);
        }

        public override void Draw(double elapsedSeconds, GraphicsDevice device)
        {
            if (renderer == null)
                return;

            sb.Begin();
            renderer.DrawMap(device, sb, fakemap, xOffset, yOffset);
            sb.End();

            base.Draw(elapsedSeconds, device);
        }

        public override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
        }

        public override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
        }
    }
}
