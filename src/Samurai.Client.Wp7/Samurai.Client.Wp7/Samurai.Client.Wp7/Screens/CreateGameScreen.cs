using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Samurai.Client.Wp7.Api;
using XNInterface.Controls;
using XNInterface.Input;

namespace Samurai.Client.Wp7.Screens
{
    public class CreateGameScreen : BaseScreen
    {
        private ContentManager content;
        private SpriteBatch sb;
        private Window gui;
        private WP7Touch touch;

        public ServerApi API;

        public override void LoadContent()
        {
            if (IsReady)
                return;

            Manager.Jobs.CreateJob(
                () =>
                {
                    content = new ContentManager(Manager.Game.Services, "Content");
                    sb = new SpriteBatch(Manager.GraphicsDevice);
                    gui = content.Load<Window>("GUI\\CreateGame");
                    gui.Initialise(null);
                    gui.LoadGraphics(Manager.GraphicsDevice, content);
                    touch = new WP7Touch(gui);
                    touch.EnableTap();
                    BindInput();

                    IsReady = true;
                });
            base.LoadContent();
        }

        private void BindInput()
        {
        }

        public override void Update(double elapsedSeconds)
        {
            touch.HandleGestures();
            gui.PerformLayout(Manager.GraphicsDevice.Viewport.Width, Manager.GraphicsDevice.Viewport.Height);
            gui.Update(elapsedSeconds);

            base.Update(elapsedSeconds);
        }

        public override void Draw(double elapsedSeconds, GraphicsDevice device)
        {
            sb.Begin();
            gui.Draw(device, sb, elapsedSeconds);
            sb.End();
            base.Draw(elapsedSeconds, device);
        }

        public override void OnNavigatedTo()
        {
            // TODO: Reset all fields
            base.OnNavigatedTo();
        }
    }
}
