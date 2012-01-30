using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Samurai.Client.Wp7.Api;
using SamuraiServer.Data;
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

        private ServerApi api;
        private Player player;
        private string gameName = "";
        private string defaultText;

        public void Init(ServerApi api, Player player)
        {
            this.api = api;
            this.player = player;
        }

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
            var creatingMessage = gui.GetChild<TextBlock>("creatingMessage");

            if (creatingMessage != null)
                creatingMessage.Enabled = false;

            if (txtGameName != null)
            {
                defaultText = txtGameName.Text;
                txtGameName.Triggered += (b) => Guide.BeginShowKeyboardInput(PlayerIndex.One, "Game Name", "Enter the name for the game.", gameName, KBCallback, null);
            }

            if (btnCreate != null)
            {
                btnCreate.Triggered +=
                    (b) =>
                    {
                        if (!string.IsNullOrWhiteSpace(gameName))
                        {
                            if (creatingMessage != null)
                                creatingMessage.Enabled = true;
                            btnCreate.Enabled = false;
                            api.CreateGameAndJoin(gameName, player.Id, new Action<CreateGameAndJoinResponse, Exception>(
                                (r, e) =>
                                {
                                    if (e == null && r.Ok)
                                    {
                                        api.GetMap(r.Game.MapId, new Action<GetMapResponse, Exception>(
                                            (mr, me) =>
                                            {
                                                if (me == null && mr.Ok)
                                                {
                                                    var scr = Manager.GetOrCreateScreen<GameScreen>();
                                                    scr.Init(api, player, r.Game, Map.FromStringRepresentation(r.Game.MapId, mr.Map));
                                                    Manager.TransitionTo<GameScreen>();
                                                }
                                                else
                                                {
                                                    Guide.BeginShowMessageBox("Error", me == null ? mr.Message : me.Message, new string[] { "Ok" }, 0, MessageBoxIcon.Error, null, null);
                                                }
                                                btnCreate.Enabled = true;
                                                if (creatingMessage != null)
                                                    creatingMessage.Enabled = false;
                                            }));
                                    }
                                    else
                                    {
                                        Guide.BeginShowMessageBox("Error", e == null ? r.Message : e.Message, new string[] { "Ok" }, 0, MessageBoxIcon.Error, null, null);
                                        btnCreate.Enabled = true;
                                        if (creatingMessage != null)
                                            creatingMessage.Enabled = false;
                                    }
                                }));
                        }
                    };
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

        public override void OnNavigatedTo()
        {
            // Reset all fields
            gameName = "";
            base.OnNavigatedTo();
        }
    }
}
