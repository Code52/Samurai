using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private string gameName = "";
        private string defaultText;

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
            var txtGameName = gui.GetChild<TextBlock>("txtGameName");
            var btnCreate = gui.GetChild<Button>("btnCreate");

            if (txtGameName != null)
            {
                defaultText = txtGameName.Text;
                txtGameName.Triggered += (b) => Guide.BeginShowKeyboardInput(PlayerIndex.One, "Game Name", "Enter the name for the game.", gameName, KBCallback, null);
            }
        }

        private void KBCallback(IAsyncResult iar)
        {
            if (iar.IsCompleted)
            {
                var txtGameName = gui.GetChild<TextBlock>("txtGameName");
                gameName = Guide.EndShowKeyboardInput(iar);
                if (string.IsNullOrWhiteSpace(gameName))
                    txtGameName.Text = defaultText;
                else
                    txtGameName.Text = gameName;
            }
        }

        public override void Update(double elapsedSeconds)
        {
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back))
                Manager.TransitionTo<LobbyScreen>();

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
