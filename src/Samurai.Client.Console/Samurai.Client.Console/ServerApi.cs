using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using SamuraiServer.Data;

namespace Samurai.Client.ConsoleClient
{
    public class ServerApi
    {
        private readonly string serverUrl;

        public ServerApi(string serverUrl)
        {
            this.serverUrl = serverUrl;
        }

        public void CreatePlayer(string name, Action<CreatePlayerResponse, Exception> callback)
        {
            Post("/Api/Players/CreatePlayer", "name=" + System.Uri.EscapeDataString(name), callback);
        }

        public void Login(string name, string key, Action<CreatePlayerResponse, Exception> callback)
        {
            Post("/Api/Players/Login", "name=" + System.Uri.EscapeDataString(name) + "&token=" + System.Uri.EscapeDataString(key), callback);
        }

        public void GetOpenGames(Action<GetOpenGamesResponse, Exception> callback)
        {
            Get("/Api/Games/GetOpenGames", callback);
        }

        public void CreateGameAndJoin(string name, Guid playerId, Action<CreateGameAndJoinResponse, Exception> callback)
        {
            Post("/Api/Games/CreateGameAndJoin", "name=" + System.Uri.EscapeDataString(name) + "&playerid=" + playerId, callback);
        }

        public void JoinGame(Guid gameId, Guid playerId, Action<CreateGameAndJoinResponse, Exception> callback)
        {
            Post("/Api/Games/JoinGame", "gameid=" + gameId + "&playerid=" + playerId, callback);
        }

        public void GetMap(Guid mapId, Action<GetMapResponse, Exception> callback)
        {
            Post("/Api/Games/GetMap", "mapid=" + mapId, callback);
        }

        public void StartGame(Guid gameId, Action<StartGameResponse, Exception> callback) {
            Get("/Api/Games/StartGame?gameid=" + gameId, callback);
        }

        // A sync method in an async pattern. Easier to work with for the console app.
        private void Get<T>(string url, Action<T, Exception> callback)
        {
            try
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(Uri(url));
                request.Accept = "application/json";
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) callback(default(T), new Exception("Unexpected server response " + response.StatusCode));

                string json;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    json = reader.ReadToEnd();
                    reader.BaseStream.Close();
                }
                response.Close();

                var data = JsonConvert.DeserializeObject<T>(json);
                callback(data, null);
            }
            catch (Exception e)
            {
                callback(default(T), e);
            }
        }

        // A sync method in an async pattern. Easier to work with for the console app.
        private void Post<T>(string url, string data, Action<T, Exception> callback)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                var request = (HttpWebRequest)HttpWebRequest.Create(Uri(url));
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK) callback(default(T), new Exception("Unexpected server response " + response.StatusCode));

                string json;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    json = reader.ReadToEnd();
                    reader.BaseStream.Close();
                }
                response.Close();

                var responseData = JsonConvert.DeserializeObject<T>(json);
                Debug.WriteLine(String.Format("Request: {0} at {1}", url, DateTime.Now));
                Debug.WriteLine(String.Format("Data: {0}", data));
                Debug.WriteLine("Response:");
                Debug.WriteLine(json);
                Debug.WriteLine("=========================================================");
                callback(responseData, null);
            }
            catch (Exception e)
            {
                callback(default(T), e);
            }
        }

        private Uri Uri(string relativePath)
        {
            return new Uri(new Uri(serverUrl), relativePath);
        }
    }

    public class ServerResponse
    {
        public bool Ok { get; set; }
        public string Message { get; set; }
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

    public class LoginResponse : ServerResponse
    {
        public Player Player { get; set; }
    }

    public class GetMapResponse : ServerResponse
    {
        public string[] Map { get; set; }
    }

    public class StartGameResponse : ServerResponse
    {
        public GameState Game { get; set; }
    }
}