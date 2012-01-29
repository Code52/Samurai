using System;
using System.Collections.Generic;
using System.Linq;
using SamuraiServer.Data;

namespace Samurai.Client.ConsoleClient
{
    internal class Program
    {
        static ServerApi api = new ServerApi("http://samuraitest.apphb.com/");

        static Player CurrentPlayer = null;
        static Dictionary<Guid, GameState> CurrentGames = new Dictionary<Guid, GameState>();
        static Dictionary<Guid, string[]> CurrentMaps = new Dictionary<Guid, string[]>();
        static GameState CurrentGame = null;

        private static void Main(string[] args)
        {
            Welcome();
        }

        private static void Welcome(string message = "")
        {
            Console.Clear();
            Console.WriteLine("Welcome to Samurai");
            if (CurrentPlayer != null)
            {
                Console.WriteLine("Logged in as " + CurrentPlayer.Name + " " + CurrentPlayer.Id);
            }
            if (!String.IsNullOrEmpty(message))
            {
                Console.WriteLine("----");
                Console.WriteLine(message);
            }
            Console.WriteLine("----");
            Console.WriteLine("[C] Create user");
            Console.WriteLine("[O] Login");
            Console.WriteLine("[L] List games");
            Console.WriteLine("[X] Exit");
            Choose(new Dictionary<char, Action> {
                { 'C', CreateUser },
                { 'O', Login },
                { 'L', ListGames },
                { 'X', Exit }
            });
        }

        private static void Exit() { }

        private static void CreateUser()
        {
            Console.Clear();
            var name = GetText("Please enter your username", s => !String.IsNullOrWhiteSpace(s));
            api.CreatePlayer(name, (data, e) =>
            {
                if (e != null)
                {
                    Error(e);
                    return;
                }

                if (!data.Ok)
                {
                    Welcome(data.Message);
                    return;
                }

                CurrentPlayer = data.Player;
                Console.WriteLine("Your generated key: {0}", CurrentPlayer.ApiKey);
                Console.ReadKey();
                Welcome();
            });
        }

        private static void Login()
        {
            var name = GetText("Please enter your username", s => !String.IsNullOrWhiteSpace(s));
            var key = GetText("Please enter your key", s => !String.IsNullOrWhiteSpace(s));

            api.Login(name, key, (data, e) =>
            {
                if (e != null)
                {
                    Error(e);
                    return;
                }

                if (!data.Ok)
                {
                    BadResponse();
                    return;
                }

                CurrentPlayer = data.Player;
                Welcome();
            });
        }

        private static void ListGames()
        {
            if (CurrentPlayer == null)
            {
                Welcome("Please log in");
                return;
            }

            api.GetOpenGames((data, e) =>
            {
                if (e != null) { Error(e); return; }
                if (!data.Ok) { BadResponse(); return; }

                Dictionary<char, Action> actions = new Dictionary<char, Action>();
                actions.Add('C', CreateGame);
                actions.Add('X', () => Welcome());

                Console.Clear();
                Console.WriteLine("Choose a game to join");
                Console.WriteLine("---");
                for (int i = 0; i < data.Games.Length; i++)
                {
                    UpdateGame(data.Games[i]);
                    Console.WriteLine(String.Format("[{0}] {1} - {2}", i + 1, data.Games[i].Name, data.Games[i].Id));
                    var id = data.Games[i].Id;  // This copy of the variable is for the lambda below. Passing a reference to [i] into it is broken by the loop.
                    actions.Add((i + 1).ToString()[0], () => JoinGame(id));
                }
                Console.WriteLine("---");
                Console.WriteLine("[C] Create a new game");
                Console.WriteLine("[X] Exit");
                Choose(actions);
            });
        }

        private static void CreateGame()
        {
            Console.Clear();
            string name = GetText("Enter the name of your game", s => !String.IsNullOrWhiteSpace(s));
            api.CreateGameAndJoin(name, CurrentPlayer.Id, (data, e) =>
            {
                if (e != null) { Error(e); return; }
                if (!data.Ok) { BadResponse(); return; }
                api.GetMap(data.Game.MapId, (mapResponse, ex) =>
                {
                    if (ex != null) { Error(ex); return; }
                    if (!mapResponse.Ok) { BadResponse(); return; }
                    CurrentMaps[data.Game.MapId] = mapResponse.Map;
                    UpdateGame(data.Game);
                    ViewGame(data.Game.Id);
                });
            });
        }

        private static void UpdateGame(GameState game)
        {
            if (CurrentGames.ContainsKey(game.Id))
            {
                CurrentGames[game.Id] = game;
            }
            else
            {
                CurrentGames.Add(game.Id, game);
            }
        }

        private static void JoinGame(Guid id)
        {
            Console.Clear();
            var game = CurrentGames[id];

            if (game.Players.Any(d => d.Id == CurrentPlayer.Id))
            {
                ViewGame(id);
                return;
            }

            api.JoinGame(id, CurrentPlayer.Id, (data, e) =>
            {
                if (e != null) { Error(e); return; }
                if (!data.Ok) { BadResponse(); return; }

                UpdateGame(data.Game);
                ViewGame(data.Game.Id);
            });
        }

        private static void ViewGame(Guid id)
        {
            Console.Clear();
            CurrentGame = CurrentGames[id];

            int col2Left = Console.WindowWidth - 20;

            // Draw the map
            Console.SetCursorPosition(0, 0);
            Console.Write("Map");
            Console.SetCursorPosition(col2Left, 0);
            Console.Write("Players");
            Console.SetCursorPosition(0, 1);
            Console.WriteLine(new String('-', Console.WindowWidth));
            var currentMap = CurrentMaps[CurrentGame.MapId];
            for (int i = 0; i < currentMap.Length; i++)
            {
                Console.SetCursorPosition(0, 2 + i);
                var row = currentMap[i];
                Console.Write(String.Join("", row));
            }

            // Write out the players
            for (int i = 0; i < CurrentGame.Players.Count; i++)
            {
                Console.SetCursorPosition(col2Left, 2 + i);
                Console.Write(String.Format("{2}{0} - {3}pts", CurrentGame.Players[i].Player.Name, CurrentGame.Players[i].Id, CurrentGame.Players[i].IsAlive ? "" : "x", CurrentGame.Players[i].Score));
            }

            Console.SetCursorPosition(0, Console.WindowHeight - 4);
            Console.WriteLine("[R] Refresh");
            Console.WriteLine("[X] Exit");

            Choose(new Dictionary<char, Action> {
                { 'R', Refresh },
                { 'X', ListGames },
            });
        }

        private static string GetTileView(TileType tile)
        {
            switch (tile.Name)
            {
                case "Grass":
                    return ".";
                case "Rock":
                    return "@";
                case "Tree":
                    return "T";
                case "Water":
                    return "~";
                default:
                    return "?";
            }
        }

        private static void Refresh()
        {
            ViewGame(CurrentGame.Id);
        }

        private static void Choose(Dictionary<char, Action> actions)
        {
            Console.Write("?");
            Char c = Char.MinValue;
            while (!actions.ContainsKey(c))
            {
                c = Char.ToUpper(Console.ReadKey(true).KeyChar);
            }
            actions[c].Invoke();
        }

        private static string GetText(string message, Func<string, bool> valid)
        {
            Console.WriteLine(message);
            string s = null;
            while (s == null || !valid(s))
            {
                s = Console.ReadLine();
            }
            return s;
        }

        private static void Error(Exception e)
        {
            Console.Clear();
            Console.WriteLine("Error occured");
            Console.WriteLine(e.ToString());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void BadResponse(string message = "")
        {
            Console.Clear();
            Console.WriteLine("Received a bad response from the server");
            if (!String.IsNullOrEmpty(message))
                Console.WriteLine(message);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}