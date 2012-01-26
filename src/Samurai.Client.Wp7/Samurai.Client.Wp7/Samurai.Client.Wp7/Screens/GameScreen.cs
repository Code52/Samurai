using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Samurai.Client.Wp7.Graphics;
using SamuraiServer.Data;

namespace Samurai.Client.Wp7.Screens
{
    public class GameScreen : BaseScreen
    {
        private ContentManager content;
        private SpriteBatch sb;
        private Renderer renderer;

        private Map fakemap;

        public GameScreen()
            : base()
        {
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
            base.UnloadContent();
        }

        public override void Update(double elapsedSeconds)
        {
            // TODO: Replace with proper logic once implemented
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
                Manager.ExitGame();

            base.Update(elapsedSeconds);
        }

        public override void Draw(double elapsedSeconds, GraphicsDevice device)
        {
            if (renderer == null)
                return;

            sb.Begin();
            renderer.DrawMap(device, sb, null, 0, 0);
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
