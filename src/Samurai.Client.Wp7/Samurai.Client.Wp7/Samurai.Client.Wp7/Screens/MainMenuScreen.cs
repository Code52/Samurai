using System;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Samurai.Client.Wp7.Api;
using SamuraiServer.Data;
using XNInterface.Controls;
using XNInterface.Input;
using System.Threading;
using Samurai.Client.Services;

namespace Samurai.Client.Wp7.Screens
{
    public class MainMenuScreen : BaseScreen
    {
        private Window window;
        private ContentManager content;
        private SpriteBatch sb;
        private WP7Touch touchInput;

        private ServerApi api = new ServerApi("http://samuraitest.apphb.com");
        public Player Player;

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
            var loginBtn = window.GetChild<Button>("btnLogin");
            var logoutBtn = window.GetChild<Button>("btnLogout");
            var registerBtn = window.GetChild<Button>("btnRegister");
            var settingsBtn = window.GetChild<Button>("btnSettings");
            var status = window.GetChild<TextBlock>("status");
            if (status != null)
                status.Enabled = false;

            if (playBtn != null)
            {
                playBtn.Triggered +=
                    (b) =>
                    {
                        Manager.GetOrCreateScreen<CreateGameScreen>().Init(api, Player);
                        Manager.TransitionTo<CreateGameScreen>();
                    };
            }

            if (registerBtn != null)
            {
                registerBtn.Triggered +=
                    (b) =>
                    {
                        Manager.GetOrCreateScreen<RegisterScreen>().API = api;
                        Manager.TransitionTo<RegisterScreen>();
                    };
            }

            if (settingsBtn != null)
            {
                settingsBtn.Triggered +=
                    (b) =>
                    {
                        Manager.TransitionTo<SettingsScreen>();
                    };
            }

            var exitBtn = window.GetChild<Button>("btnExit");
            if (exitBtn != null)
                exitBtn.Triggered += (b) => Manager.ExitGame();
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
            SavePlayer();
            base.OnNavigatedFrom();
        }

        public override void OnNavigatedTo()
        {
            LoadPlayer();
            UpdateButtons();
            base.OnNavigatedTo();
        }

        private void SavePlayer()
        {
            if (Player == null)
                return;

            Setting.Instance.SetPlayer(Player);
        }

        private void LoadPlayer()
        {
            if (Player != null)
                return;

            if (Setting.Instance.CurrentPlayer != null)
                Player = Setting.Instance.CurrentPlayer;
        }

        private void Login()
        {
            var status = window.GetChild<TextBlock>("status");
            if (status != null)
                status.Enabled = true;
            var name = Player.Name;
            var key = Player.ApiKey;
            Player = null;
            api.Login(name, key, new Action<PlayerResponse, Exception>(
                (p, e) =>
                {
                    Thread.Sleep(1000);
                    if (e == null && p.Ok)
                        Player = p.Player;
                    else
                    {
                        DeleteSave();
                        Guide.BeginShowMessageBox("Error", "Failed to login: " + (e == null ? p.Message : e.Message), new string[] { "Ok" }, 0, MessageBoxIcon.Error, null, null);
                    }
                    if (status != null)
                        status.Enabled = false;
                    UpdateButtons();
                }));
        }

        private void DeleteSave()
        {
            using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
                iso.DeleteFile("player.dat");
        }

        private void UpdateButtons()
        {
            bool isLoggedIn = Player != null;

            var playBtn = window.GetChild<Button>("btnPlay");
            var registerBtn = window.GetChild<Button>("btnRegister");

            if (playBtn != null)
                playBtn.Enabled = isLoggedIn;

            if (registerBtn != null)
                registerBtn.Enabled = !isLoggedIn;
        }
    }
}
