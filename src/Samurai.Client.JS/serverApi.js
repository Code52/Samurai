/** Server API module.
 * Ported from C# implementation.
 */
function ServerApi(serverUrl) {
  var serverUrl = serverUrl;

//  function createPlayer(string name, Action<CreatePlayerResponse, Exception> callback) {
  function createPlayer(name, callback) {
      //post("/Api/Players/CreatePlayer", "name=" + System.Uri.EscapeDataString(name), callback);
    post("/Api/Players/CreatePlayer", {name : name}, callback);
  }

//  function login(string name, string key, Action<CreatePlayerResponse, Exception> callback) {
  function login(name, key, callback) {
//      post("/Api/Players/Login", "name=" + System.Uri.EscapeDataString(name) + "&token=" + System.Uri.EscapeDataString(key), callback);
    post("/Api/Players/Login", {name : name, token : key}, callback);
  }

//  function getOpenGames(Action<GetOpenGamesResponse, Exception> callback) {
  function getOpenGames(callback) {
    get("/Api/Games/GetOpenGames", callback);
  }

//  function createGameAndJoin(string name, Guid playerId, Action<CreateGameAndJoinResponse, Exception> callback) {
  function createGameAndJoin(name, playerId, callback) {
//      post("/Api/Games/CreateGameAndJoin", "name=" + System.Uri.EscapeDataString(name) + "&playerid=" + playerId, callback);
    post("/Api/Games/CreateGameAndJoin", {name : name, playerid : playerId}, callback);
  }

//  function joinGame(Guid gameId, Guid playerId, Action<CreateGameAndJoinResponse, Exception> callback) {
  function joinGame(gameId, playerId, callback) {
//      post("/Api/Games/JoinGame", "gameid=" + gameId + "&playerid=" + playerId, callback);
    post("/Api/Games/JoinGame", {gameid : gameId, playerid : playerId}, callback);
  }

//  function getMap(Guid mapId, Action<GetMapResponse, Exception> callback) {
  function getMap(mapId, callback) {
//    post("/Api/Games/GetMap", "mapid=" + mapId, callback);
    post("/Api/Games/GetMap", {mapid : mapId}, callback);
  }

//  function startGame(Guid gameId, Action<StartGameResponse, Exception> callback) {
  function startGame(gameId, callback) {
//    get("/Api/Games/StartGame?gameid=" + gameId, callback);
    get("/Api/Games/StartGame", callback, {gameid: gameId});
  }

  //function getGame(Guid gameId, Action<GetGameResponse, Exception> callback) {
  function getGame(gameId, callback) {
    //get("/Api/Games/GetGame?gameid=" + gameId, callback);
    get("/Api/Games/GetGame", callback, {gameid: gameId});
  }

//A sync method in an async pattern. Easier to work with for the console app.
//  private void Get<T>(string url, Action<T, Exception> callback) {
  function get(url, callback, data) {
      try {
/******************/
        $.ajax(serverUrl + url, {
          async: false,
          dataType: json,
          data: data,
          success: function (data) {
            callback(data, null);
          },
          error: function (jqXHR, textStatus) {
            //'Unexpected server response ' + jqXHR.status
            //callback(null, );
          },
        });
/******************/
//          var request = (HttpWebRequest)HttpWebRequest.Create(Uri(url));
//          request.Accept = "application/json";
//          var response = (HttpWebResponse)request.GetResponse();
//          if (response.StatusCode != HttpStatusCode.OK) callback(default(T), new Exception("Unexpected server response " + response.StatusCode));
//
//          string json;
//          using (var reader = new StreamReader(response.GetResponseStream()))
//          {
//              json = reader.ReadToEnd();
//              reader.BaseStream.Close();
//          }
//          response.Close();
//
//          var data = JsonConvert.DeserializeObject<T>(json);
//          callback(data, null);
      }
//      catch (Exception e) {
      catch(e) {
//          callback(default(T), e);
        //callback(default(T), e);
      }
  }

  // A sync method in an async pattern. Easier to work with for the console app.
//  private void Post<T>(string url, string data, Action<T, Exception> callback) {
    function post(url, data, callback) {
      try {
/******************/
        $.ajax(serverUrl + url, {
          type: "POST",
          async: false,
          dataType: json,
          data: data,
          success: function (data) {
              callback(data, null);
           },
          error: function (jqXHR, textStatus) {
            //'Unexpected server response ' + jqXHR.status
          //callback(null, );
          }
        });
/******************/
//          byte[] bytes = Encoding.UTF8.GetBytes(data);
//          var request = (HttpWebRequest)HttpWebRequest.Create(Uri(url));
//          request.Method = "POST";
//          request.Accept = "application/json";
//          request.ContentType = "application/x-www-form-urlencoded";
//          using (var stream = request.GetRequestStream())
//          {
//              stream.Write(bytes, 0, bytes.Length);
//          }
//          var response = (HttpWebResponse)request.GetResponse();
//          if (response.StatusCode != HttpStatusCode.OK) callback(default(T), new Exception("Unexpected server response " + response.StatusCode));
//
//          string json;
//          using (var reader = new StreamReader(response.GetResponseStream()))
//          {
//              json = reader.ReadToEnd();
//              reader.BaseStream.Close();
//          }
//          response.Close();
//
//          var responseData = JsonConvert.DeserializeObject<T>(json);
//          Debug.WriteLine(String.Format("Request: {0} at {1}", url, DateTime.Now));
//          Debug.WriteLine(String.Format("Data: {0}", data));
//          Debug.WriteLine("Response:");
//          Debug.WriteLine(json);
//          Debug.WriteLine("=========================================================");
//          callback(responseData, null);
      }
//      catch (Exception e) {
      catch(e) {
//          callback(default(T), e);
      }
  }

//  private Uri Uri(string relativePath) {
//  function Uri(relativePath) {
//      return new Uri(new Uri(serverUrl), relativePath);
//  }

  return {
     createPlayer : createPlayer,
     login : login,
     getOpenGames : getOpenGames,
     createGameAndJoin : createGameAndJoin,
     joinGame : joinGame,
     getMap : getMap,
     startGame : startGame,
     getGame : getGame,
  };
}

/*****************************************************************************/
function ServerResponse() {
//    public bool Ok { get; set; }
//    public string Message { get; set; }
  this.ok = false;
  this.message = '';
}

function GetOpenGamesResponse() {
//    public GameState[] Games { get; set; }
  ServerResponse.call(this);
  this.games = [];
}
GetOpenGamesResponse.prototype = new ServerResponse();
GetOpenGamesResponse.prototype.constructor = GetOpenGamesResponse();

function CreateGameAndJoinResponse() {
//    public GameState Game { get; set; }
  ServerResponse.call(this);
  this.game = new GameState();
}
CreateGameAndJoinResponse.prototype = new ServerResponse();
CreateGameAndJoinResponse.prototype.constructor = CreateGameAndJoinResponse();

function JoinGameResponse() {
//    public GameState Game { get; set; }
  ServerResponse.call(this);
  this.game = new GameState();
}
JoinGameResponse.prototype = new ServerResponse();
JoinGameResponse.prototype.constructor = JoinGameResponse();

function CreatePlayerResponse() {
//    public Player Player { get; set; }
  ServerResponse.call(this);
  this.Player = new Player();
}
CreatePlayerResponse.prototype = new ServerResponse();
CreatePlayerResponse.prototype.constructor = CreatePlayerResponse();

function LoginResponse() {
//    public Player Player { get; set; }
  ServerResponse.call(this);
  this.Player = new Player();
}
LoginResponse.prototype = new ServerResponse();
LoginResponse.prototype.constructor = LoginResponse();

function GetMapResponse() {
//    public string[] Map { get; set; }
  ServerResponse.call(this);
  this.map = [];
}
GetMapResponse.prototype = new ServerResponse();
GetMapResponse.prototype.constructor = GetMapResponse();

function StartGameResponse() {
//    public GameState Game { get; set; }
  ServerResponse.call(this);
  this.game = new GameState();
}
StartGameResponse.prototype = new ServerResponse();
StartGameResponse.prototype.constructor = StartGameResponse();

function GetGameResponse() {
//    public GameState Game { get; set; }
  ServerResponse.call(this);
  this.game = new GameState();
}
GetGameResponse.prototype = new ServerResponse();
GetGameResponse.prototype.constructor = GetGameResponse();
/*****************************************************************************/