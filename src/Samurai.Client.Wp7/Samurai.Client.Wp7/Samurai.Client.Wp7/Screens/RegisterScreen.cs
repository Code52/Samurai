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
    public class RegisterScreen : BaseScreen
    {
        private ContentManager content;
        private SpriteBatch sb;
        private Window gui;
        private WP7Touch touch;

        private string userName = "";
        private string defaultUsernameLabel = "";

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
                    gui = content.Load<Window>("GUI\\Register");
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
            var txtUsername = gui.GetChild<TextBlock>("txtUsername");
            var btnRegister = gui.GetChild<Button>("btnRegister");
            var busyMessage = gui.GetChild<TextBlock>("busyMessage");
            if (busyMessage != null)
                busyMessage.Enabled = false;

            if (txtUsername != null)
            {
                defaultUsernameLabel = txtUsername.Text;
                txtUsername.Triggered +=
                    (b) =>
                    {
                        Guide.BeginShowKeyboardInput(PlayerIndex.One, "Username", "Please enter your desired username.", userName, KeyboardCallback, null);
                    };
            }

            if (btnRegister != null)
            {
                btnRegister.Triggered +=
                    (b) =>
                    {
                        if (!string.IsNullOrWhiteSpace(userName))
                        {
                            // Disable button while performing network operations
                            btnRegister.Enabled = false;
                            if (busyMessage != null)
                                busyMessage.Enabled = true;

                            API.CreatePlayer(userName,
                                (p, e) =>
                                {
                                    // Were there Connection issues?
                                    if (e == null)
                                    {
                                        // Is the Username valid and the player has been created?
                                        if (p.Ok)
                                        {
                                            Manager.GetOrCreateScreen<MainMenuScreen>().Player = p.Player;
                                            Manager.TransitionTo<MainMenuScreen>();
                                        }
                                        else
                                        {
                                            Guide.BeginShowMessageBox("Username Taken", "This username has already been taken, please choose another.", new string[] { "Ok" }, 0, MessageBoxIcon.Alert, null, null);
                                        }
                                    }
                                    else
                                    {
                                        Guide.BeginShowMessageBox("Error", e.Message, new string[] { "Ok" }, 0, MessageBoxIcon.Error, null, null);
                                    }
                                    // Re-enable button now that network ops are complete
                                    btnRegister.Enabled = true;
                                    if (busyMessage != null)
                                        busyMessage.Enabled = false;
                                });
                        }
                    };
            }
        }

        private void KeyboardCallback(IAsyncResult iar)
        {
            if (iar.IsCompleted)
            {
                var name = Guide.EndShowKeyboardInput(iar);
                var txtUsername = gui.GetChild<TextBlock>("txtUsername");
                if (string.IsNullOrWhiteSpace(name))
                {
                    txtUsername.Text = defaultUsernameLabel;
                    userName = "";
                }
                else
                {
                    userName = name;
                    txtUsername.Text = userName;
                }
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
