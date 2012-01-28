using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using SamuraiServer.Data;
#if MONO
using MonoTouch.Foundation;
#endif

namespace Samurai.Client.Wp7.Api
{
    public class ServerApi
    {
        private string serverUrl;

        public ServerApi(string serverUrl)
        {
            this.serverUrl = serverUrl;
        }

        public void CreatePlayer(string name, Action<PlayerResponse, Exception> callback)
        {
            Post("/Api/Players/CreatePlayer", "name=" + System.Uri.EscapeDataString(name), callback);
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

        public void Login(string name, string apiKey, Action<PlayerResponse, Exception> callback)
        {
            Post("/Api/Players/Login", "name=" + System.Uri.EscapeDataString(name) + "&apiKey=" + System.Uri.EscapeDataString(apiKey), callback);
        }

        private void Get<T>(string url, Action<T, Exception> callback)
        {
            try
            {
#if MONO
                var request = HttpWebRequest.Create(Uri(url));
#else
                var request = HttpWebRequest.CreateHttp(Uri(url));
#endif
                request.BeginGetResponse(_ =>
                {
                    try
                    {
                        var response = request.EndGetResponse(_) as HttpWebResponse;
                        if (response.StatusCode != HttpStatusCode.OK) callback(default(T), new Exception("Unexpected server response " + response.StatusCode));

                        string json;
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            json = reader.ReadToEnd();
                            reader.BaseStream.Close();
                        }
                        response.Close();

                        T data = JsonConvert.DeserializeObject<T>(json);
                        callback(data, null);
                    }
                    catch (Exception e)
                    {
                        callback(default(T), e);
                    }
                }, null);
            }
            catch (Exception e)
            {
                callback(default(T), e);
            }
        }

        private void Post<T>(string url, string data, Action<T, Exception> callback)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                var request = (HttpWebRequest)HttpWebRequest.Create(Uri(url));
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.BeginGetRequestStream(iar =>
                    {
                        using (var stream = request.EndGetRequestStream(iar))
                        {
                            stream.Write(bytes, 0, bytes.Length);
                        }
                        request.BeginGetResponse(_ =>
                        {
                            try
                            {
                                var response = request.EndGetResponse(_) as HttpWebResponse;
                                if (response.StatusCode != HttpStatusCode.OK) callback(default(T), new Exception("Unexpected server response " + response.StatusCode));

                                string json;
                                using (var reader = new StreamReader(response.GetResponseStream()))
                                {
                                    json = reader.ReadToEnd();
                                    reader.BaseStream.Close();
                                }
                                response.Close();

                                T convertedData = JsonConvert.DeserializeObject<T>(json);
                                callback(convertedData, null);
                            }
                            catch (Exception e)
                            {
                                callback(default(T), e);
                            }
                        }, null);
                    }, null);
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

    public class PlayerResponse : ServerResponse
    {
        public Player Player { get; set; }
    }

    public class GetMapResponse : ServerResponse
    {
        public string[] Map { get; set; }
    }
}
