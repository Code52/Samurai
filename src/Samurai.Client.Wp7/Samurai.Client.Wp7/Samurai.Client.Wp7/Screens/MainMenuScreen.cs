using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNInterface.Controls;
using XNInterface.Input;

namespace Samurai.Client.Wp7.Screens
{
    public class MainMenuScreen : BaseScreen
    {
        private Window window;
        private ContentManager content;
        private SpriteBatch sb;
        private WP7Touch touchInput;

        public MainMenuScreen()
            : base()
        {
            // No-param constructor required for ScreenManager.GetOrCreateScreen()
        }

        public override void LoadContent()
        {
            if (IsReady)
                return;

            sb = new SpriteBatch(Manager.GraphicsDevice);
            content = new ContentManager(Manager.Game.Services, "Content");

            Manager.Jobs.CreateJob(
                () =>
                {
                    window = content.Load<Window>("GUI\\MainMenu");
                    window.Initialise(null);
                    window.LoadGraphics(Manager.GraphicsDevice, content);
                    touchInput = new WP7Touch(window);
                    touchInput.EnableTap();

                    BindMenuItems();

                    IsReady = true;
                });

            base.LoadContent();
        }

        private void BindMenuItems()
        {
            var playBtn = window.GetChild<Button>("btnPlay");
            if (playBtn != null)
            {
                playBtn.Triggered +=
                    (b) =>
                    {
                        var game = Manager.GetOrCreateScreen<GameScreen>();
                        // TODO: Replace with lobby - TESTING
                        Manager.TransitionTo<GameScreen>();
                    };
            }
        }

        public override void UnloadContent()
        {
            content.Unload();
            content.Dispose();
            sb.Dispose();
            IsReady = false;
            base.UnloadContent();
        }

        public override void Update(double elapsedSeconds)
        {
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
                Manager.ExitGame();

            touchInput.HandleGestures();
            window.Update(elapsedSeconds);
            window.PerformLayout(Manager.GraphicsDevice.Viewport.Width, Manager.GraphicsDevice.Viewport.Height);

            base.Update(elapsedSeconds);
        }

        public override void Draw(double elapsedSeconds, GraphicsDevice device)
        {
            sb.Begin();
            window.Draw(device, sb, elapsedSeconds);
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
