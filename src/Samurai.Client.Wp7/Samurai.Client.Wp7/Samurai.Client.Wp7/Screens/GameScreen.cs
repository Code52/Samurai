using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Samurai.Client.Wp7.Api;
using Samurai.Client.Wp7.Graphics;
using SamuraiServer.Data;

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

        private Map map;
        private Player player;
        private GameState game;
        private ServerApi api;

        public GameScreen()
            : base()
        {
            renderer = new Renderer();
        }

        public void Init(ServerApi api, Player player, GameState game, Map map)
        {
            this.api = api;
            this.player = player;
            this.game = game;
            this.map = map;
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

            var touches = TouchPanel.GetState();
            if (touches.Count > 0)
            {
                var mapSize = renderer.GetMapSize(map);
                if (touches[0].State == TouchLocationState.Pressed)
                    prevPos = touches[0].Position;
                else if (touches[0].State == TouchLocationState.Moved)
                {
                    if (mapSize.X > Manager.GraphicsDevice.Viewport.Width)
                    {
                        xOffset -= (int)(touches[0].Position.X - prevPos.X);
                        if (xOffset < 0)
                            xOffset = 0;
                        else if (xOffset >= (mapSize.X - Manager.GraphicsDevice.Viewport.Width))
                            xOffset = mapSize.X - Manager.GraphicsDevice.Viewport.Width;
                    }

                    if (mapSize.Y > Manager.GraphicsDevice.Viewport.Height)
                    {
                        yOffset -= (int)(touches[0].Position.Y - prevPos.Y);
                        if (yOffset < 0)
                            yOffset = 0;
                        else if (yOffset >= (mapSize.Y - Manager.GraphicsDevice.Viewport.Height))
                            yOffset = mapSize.Y - Manager.GraphicsDevice.Viewport.Height;
                    }
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
            renderer.DrawMap(device, sb, map, xOffset, yOffset);
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
