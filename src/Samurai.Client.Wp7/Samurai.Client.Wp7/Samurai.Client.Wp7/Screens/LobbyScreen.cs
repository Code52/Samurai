using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Samurai.Client.Wp7.Api;
using SamuraiServer.Data;

namespace Samurai.Client.Wp7.Screens
{
    public class LobbyScreen : BaseScreen
    {
        private ServerApi api = new ServerApi("http://samuraitest.apphb.com/");
        private Player player;

        public override void LoadContent()
        {
            IsReady = true;
            base.LoadContent();
        }

        public override void OnNavigatedTo()
        {
            Guide.BeginShowKeyboardInput(PlayerIndex.One, "", "Please enter the player name.", "player", Callback, "name");
            base.OnNavigatedTo();
        }

        private void Callback(IAsyncResult iar)
        {
            if (iar.IsCompleted)
            {
                switch (iar.AsyncState as string)
                {
                    case "name":
                        var name = Guide.EndShowKeyboardInput(iar);
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            api.CreatePlayer(name, new Action<CreatePlayerResponse, Exception>((r, e) =>
                                {
                                    if (e == null)
                                    {
                                        if (r.Ok)
                                        {
                                            player = r.Player;
                                            Guide.BeginShowKeyboardInput(PlayerIndex.One, "", "Please enter a game name.", "game", Callback, "game");
                                        }
                                        else
                                            Manager.ExitGame();
                                    }
                                    else
                                    {
                                        Guide.BeginShowMessageBox("Error", e.Message, new string[] { "ok" }, 0, MessageBoxIcon.Error, Callback, "error");
                                    }
                                }));
                        }
                        else
                        {
                            Manager.TransitionTo<MainMenuScreen>();
                        }
                        break;

                    case "game":
                        var game = Guide.EndShowKeyboardInput(iar);
                        if (!string.IsNullOrWhiteSpace(game))
                        {
                            api.CreateGameAndJoin(game, player.Id, new Action<CreateGameAndJoinResponse, Exception>((r, e) =>
                            {
                                if (e == null)
                                {
                                    if (r.Ok)
                                    {
                                        var scr = Manager.GetOrCreateScreen<GameScreen>();
                                        scr.Init(api, player, r.Game);
                                        Manager.TransitionTo<GameScreen>();
                                    }
                                    else
                                        Manager.ExitGame();
                                }
                                else
                                {
                                    Guide.BeginShowMessageBox("Error", e.Message, new string[] { "ok" }, 0, MessageBoxIcon.Error, Callback, "error");
                                }
                            }));
                        }
                        else
                        {
                            Manager.ExitGame();
                        }
                        break;

                    case "error":
                        Manager.ExitGame();
                        break;
                }
            }
        }
    }
}
