﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Samurai.Client.Api;

namespace Samurai.Client.ConsoleClient
{
    internal class Program
    {
        static ServerApi api = new ServerApi("http://localhost:49706/");

        private static void Main(string[] args) {
            Welcome();
        }

        private static void Welcome() {
            Console.Clear();
            Console.WriteLine("Welcome to Samurai");
            Console.WriteLine("----");
            Console.WriteLine("[L] List games");
            Console.WriteLine("[X] Exit");
            Choose(new Dictionary<char, Action> {
                { 'L', ListGames },
                { 'X', Exit }
            });
        }

        private static void Exit() { }

        private static void ListGames() {
            api.GetOpenGames((data, e) => {
                if (e != null) { Error(e); return; }
                if (!data.Ok) { BadResponse(); return; }

                Dictionary<char, Action> actions = new Dictionary<char, Action>();
                actions.Add('C', CreateGame);
                actions.Add('X', Welcome);

                Console.Clear();
                Console.WriteLine("Choose a game to join");
                Console.WriteLine("---");
                for (int i = 0; i < data.Games.Length; i++) {
                    Console.WriteLine(String.Format("[{0}] {1}", i + 1, data.Games[i].Name));
                    actions.Add((i + 1).ToString()[0], () => { JoinGame(data.Games[i].Id); });
                }
                Console.WriteLine("---");
                Console.WriteLine("[C] Create a new game");
                Console.WriteLine("[X] Exit");
                Choose(actions);
            });
        }

        private static void CreateGame() {
            Console.Clear();
            string name = GetText("Enter the name of your game", s => !String.IsNullOrWhiteSpace(s));
            api.CreateGame(name, (data, e) => {
                if (e != null) { Error(e); return; }
                if (!data.Ok) { BadResponse(); return; }

                ViewGame(data.Game.Id);
            });
        }

        private static void JoinGame(Guid id) {
            Console.WriteLine("Joining " + id);
            Console.ReadLine();
        }

        private static void ViewGame(Guid id) {
            Console.WriteLine("Viewing game " + id);
            Console.ReadLine();
        }

        private static void Choose(Dictionary<char, Action> actions) {
            Console.WriteLine();
            Console.Write("?");
            Char c = Char.MinValue;
            while (!actions.ContainsKey(c)) {
                c = Char.ToUpper(Console.ReadKey(true).KeyChar);
            }
            actions[c].Invoke();
        }

        private static string GetText(string message, Func<string, bool> valid) {
            Console.WriteLine(message);
            string s = null;
            while (s == null || !valid(s)) {
                s = Console.ReadLine();
            }
            return s;
        }

        private static void Error(Exception e) {
            Console.Clear();
            Console.WriteLine("Error occured");
            Console.WriteLine(e.ToString());
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void BadResponse(string message = "") {
            Console.Clear();
            Console.WriteLine("Received a bad response from the server");
            if (!String.IsNullOrEmpty(message))
                Console.WriteLine(message);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}