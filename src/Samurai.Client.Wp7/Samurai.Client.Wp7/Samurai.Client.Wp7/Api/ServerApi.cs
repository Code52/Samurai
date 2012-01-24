using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using SamuraiServer.Data;

namespace Samurai.Client.Wp7.Api
{
    public class ServerApi
    {
        private string serverUrl;

        public ServerApi(string serverUrl) {
            this.serverUrl = serverUrl;
        }

        public void GetListOfGames(Action<IEnumerable<GameState>, Exception> callback) {
            Get("/api/ListGames", callback);
        }

        private void Get<T>(string url, Action<T, Exception> callback) {
            try {
                var request = HttpWebRequest.CreateHttp(Uri(url));
                request.BeginGetResponse(_ => {
                    try {
                        var response = request.EndGetResponse(_) as HttpWebResponse;
                        if (response.StatusCode != HttpStatusCode.OK) callback(default(T), new Exception("Unexpected server response " + response.StatusCode));

                        string json;
                        using (var reader = new StreamReader(response.GetResponseStream())) {
                            json= reader.ReadToEnd();
                            reader.BaseStream.Close();
                        }
                        response.Close();

                        T data = JsonConvert.DeserializeObject<T>(json);
                        callback(data, null);
                    } catch (Exception e) {
                        callback(default(T), e);
                    }
                }, null);
            } catch (Exception e) {
                callback(default(T), e);
            }
        }

        private Uri Uri(string relativePath) {
            return new Uri(new Uri(serverUrl), relativePath);
        }
    }
}
