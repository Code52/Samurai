using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Samurai.Client.Wp7.Api;
using XNInterface.Controls;
using XNInterface.Input;
using SamuraiServer.Data;

namespace Samurai.Client.Wp7.Screens
{
    public class LobbyScreen : BaseScreen
    {
        public ServerApi API;
        public Player Player;

        private ContentManager content;
        private SpriteBatch sb;
        private Window gui;
        private WP7Touch touch;

        public override void LoadContent()
        {
            if (IsReady)
                return;

            Manager.Jobs.CreateJob(
                () =>
                {
                    content = new ContentManager(Manager.Game.Services, "Content");
                    sb = new SpriteBatch(Manager.GraphicsDevice);
                    gui = content.Load<Window>("GUI\\Lobby");
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
            var btnCreateGame = gui.GetChild<Button>("btnCreateGame");

            if (btnCreateGame != null)
            {
                btnCreateGame.Triggered +=
                    (b) =>
                    {
                        var scr = Manager.GetOrCreateScreen<CreateGameScreen>();
                        scr.API = API;
                        scr.Player = Player;
                        Manager.TransitionTo<CreateGameScreen>();
                    };
            }
        }

        public override void Update(double elapsedSeconds)
        {
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
                Manager.TransitionTo<MainMenuScreen>();

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
    }
}
