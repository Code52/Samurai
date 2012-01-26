using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using SamuraiServer.Data;

namespace Samurai.Client.Api
{
    public class ServerApi
    {
        private string serverUrl;

        public ServerApi(string serverUrl) {
            this.serverUrl = serverUrl;
        }

        public void CreatePlayer(string name, Action<CreatePlayerResponse, Exception> callback) {
            Post("/Api/Players/CreatePlayer", "name=" + System.Uri.EscapeDataString(name), callback);
        }

        public void GetOpenGames(Action<GetOpenGamesResponse, Exception> callback) {
            Get("/Api/Games/GetOpenGames", callback);
        }

        public void CreateGameAndJoin(string name, Guid playerId, Action<CreateGameAndJoinResponse, Exception> callback) {
            Post("/Api/Games/CreateGameAndJoin", "name=" + System.Uri.EscapeDataString(name) + "&playerid=" + playerId, callback);
        }

        public void JoinGame(Guid gameId, Guid playerId, Action<CreateGameAndJoinResponse, Exception> callback) {
            Post("/Api/Games/JoinGame", "gameid=" + gameId + "&playerid=" + playerId, callback);
        }

        // A sync method in an async pattern. Easier to work with for the console app.
        private void Get<T>(string url, Action<T, Exception> callback) {
            try {
                var request = (HttpWebRequest)HttpWebRequest.Create(Uri(url));
                request.Accept = "application/json";
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) callback(default(T), new Exception("Unexpected server response " + response.StatusCode));

                string json;
                using (var reader = new StreamReader(response.GetResponseStream())) {
                    json = reader.ReadToEnd();
                    reader.BaseStream.Close();
                }
                response.Close();

                var data = JsonConvert.DeserializeObject<T>(json);
                callback(data, null);
            } catch (Exception e) {
                callback(default(T), e);
            }
        }

        // A sync method in an async pattern. Easier to work with for the console app.
        private void Post<T>(string url, string data, Action<T, Exception> callback) {
            try {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                var request = (HttpWebRequest)HttpWebRequest.Create(Uri(url));
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                using (var stream = request.GetRequestStream()) {
                    stream.Write(bytes, 0, bytes.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) callback(default(T), new Exception("Unexpected server response " + response.StatusCode));

                string json;
                using (var reader = new StreamReader(response.GetResponseStream())) {
                    json = reader.ReadToEnd();
                    reader.BaseStream.Close();
                }
                response.Close();

                var responseData = JsonConvert.DeserializeObject<T>(json);
                callback(responseData, null);
            } catch (Exception e) {
                callback(default(T), e);
            }
        }

        private Uri Uri(string relativePath) {
            return new Uri(new Uri(serverUrl), relativePath);
        }
    }

    public class ServerResponse
    {
        public bool Ok { get; set; }
    }

    public class GetOpenGamesResponse : ServerResponse
    {
        public GameState[] Games { get; set; }
    }

    public class CreateGameAndJoinResponse : ServerResponse
    {
        public GameState Game { get; set; }
    }

    public class JoinGameResponse : ServerResponse
    {
        public GameState Game { get; set; }
    }

    public class CreatePlayerResponse : ServerResponse
    {
        public Player Player { get; set; }
    }
}