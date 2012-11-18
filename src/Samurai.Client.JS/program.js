function Program() {
  //static api = new ServerApi("http://samuraitest.apphb.com/");
  var api = new ServerApi("http://samuraitest.apphb.com/"),

//  static Player CurrentPlayer = null;
   currentPlayer = null,
//  static Dictionary<Guid, GameState> CurrentGames = new Dictionary<Guid, GameState>();
   currentGames = {},
//  static Dictionary<Guid, string[]> CurrentMaps = new Dictionary<Guid, string[]>();
   currentMaps = {},
//  static GameState CurrentGame = null;
   currentGame = null;

  function run() {
    //Load main menu screen.

  }

//  private static void Welcome(string message = "")
  function welcome(message) {
//    Console.Clear();
//    Console.WriteLine("Welcome to Samurai");
//    if (CurrentPlayer != null) {
//        Console.WriteLine("Logged in as " + CurrentPlayer.Name + " " + CurrentPlayer.Id);
//    }
//    if (!String.IsNullOrEmpty(message)) {
//        Console.WriteLine("----");
//        Console.WriteLine(message);
//    }
//    Console.WriteLine("----");
//    Console.WriteLine("[C] Create user");
//    Console.WriteLine("[O] Login");
//    Console.WriteLine("[L] List games");
//    Console.WriteLine("[X] Exit");
//    Choose(new Dictionary<char, Action> {
//        { 'C', CreateUser },
//        { 'O', Login },
//        { 'L', ListGames },
//        { 'X', Exit }
//    });
  }

  function createUser() {
//    Console.Clear();
//    var name = GetText("Please enter your username", s => !String.IsNullOrWhiteSpace(s));
//    api.CreatePlayer(name, (data, e) =>
//    {
//        if (e != null)
//        {
//            Error(e);
//            return;
//        }
//
//        if (!data.Ok)
//        {
//            Welcome(data.Message);
//            return;
//        }
//
//        CurrentPlayer = data.Player;
//        Console.WriteLine("Your generated key: {0}", CurrentPlayer.ApiKey);
//        Console.ReadKey();
//        Welcome();
//    });
  }

  function login() {
//    var status = window.GetChild<TextBlock>("status");
//    if (status != null)
//        status.Enabled = true;
    var name = Player.Name,
        key = Player.ApiKey;
    currentPlayer = null;

//    api.Login(name, key, new Action<PlayerResponse, Exception>(
    api.login(name, key, function (response, e) {
//        (p, e) => {
//            Thread.Sleep(1000);
            if (e == null && p.Ok) {
              currentPlayer = response.Player;
            } else {
//                DeleteSave();
//                Guide.BeginShowMessageBox("Error", "Failed to login: " + (e == null ? p.Message : e.Message), new string[] { "Ok" }, 0, MessageBoxIcon.Error, null, null);
            }
//            if (status != null)
//                status.Enabled = false;
//            UpdateButtons();
    });
  }

  function listGames() {
    if(currentPlayer == null) {
//          Welcome("Please log in");
//          return;
    }

    api.getOpenGames( function (data, e) {
//          if (e != null) { Error(e); return; }
//          if (!data.Ok) { BadResponse(); return; }

//          Dictionary<string, Action> actions = new Dictionary<string, Action>();
//          actions.Add("C", CreateGame);
//          actions.Add("X", () => Welcome());

//          Console.Clear();
//          Console.WriteLine("Choose a game to join");
//          Console.WriteLine("---");
//          for (int i = 0; i < data.Games.Length; i++) {
//              UpdateGame(data.Games[i]);
//              Console.WriteLine(String.Format("[{0}] {1} - {2}", i + 1, data.Games[i].Name, data.Games[i].Id));
//              var id = data.Games[i].Id;  // This copy of the variable is for the lambda below. Passing a reference to [i] into it is broken by the loop.
//              actions.Add((i + 1).ToString(), () => JoinGame(id));
//          }
//          Console.WriteLine("---");
//          Console.WriteLine("[C] Create a new game");
//          Console.WriteLine("[X] Exit");
//          Choose(actions);
    });
  }

//  private static void CreateGame()
  function createGame() {
    //Get game name.
//      string name = GetText("Enter the name of your game", s => !String.IsNullOrWhiteSpace(s));

    //Send create request to server
    api.CreateGameAndJoin(name, currentPlayer.id, function (data, e) {
//          if (e != null) { Error(e); return; }
//          if (!data.Ok) { Welcome(data.Message); return; }
//          UpdateGame(data.Game);
//          ViewGame(data.Game.Id);
    });
  }

//  private static void GetMap(GameState game)
  function getMap(game) {
    api.getMap(game.mapId, function (data, e) {
//          if (ex != null) { Error(ex); return; }
//          if (!data.Ok) { BadResponse(); return; }
//          UpdateMap(game, data.Map);
    });
  }

//  private static void UpdateGame(GameState game)
  function updateGame(game) {
    //Update or create GameState in currentGames list.
    currentGames[game.id] = game;
  }

//  private static void UpdateMap(GameState game, string[] map) {
  function updateMap(game, map) {
    currentMaps[game.mapId] = map;
  }

//  private static void JoinGame(Guid id)
  function joinGame(id) {
//      Console.Clear();
    var game = currentGames[id];

//      if(game.Players.Any(d => d.Player.Id == CurrentPlayer.Id)) {
      for(player in game.players) {
        if(player.id == currentPlayer.id) {
//          ViewGame(id);
//          return;
        }
      }

    api.joinGame(id, currentPlayer.id, function (data, e) {
//          if (e != null) { Error(e); return; }
//          if (!data.Ok) { Welcome(data.Message); return; }

//          UpdateGame(data.Game);
//          ViewGame(data.Game.Id);
    });
  }

//  private static void StartGame() {
  function startGame() {
    api.startGame(currentGame.id, function (data, e) {
      if(e != null) {
          //Error(e);
        return;
      }
      if(!data.Ok) {
        //Welcome(data.Message);
        return;
      }

      UpdateGame(data.Game);
      ViewGame(CurrentGame.Id);
    });
  }

//  private static void ViewGame(Guid id)
  function viewGame(id) {
//      Console.Clear();
    currentGame = currentGames[id];

//      if (!CurrentMaps.ContainsKey(CurrentGame.MapId))
//          GetMap(CurrentGame);

//      int col2Left = Console.WindowWidth - 20;

//      // Draw the map
//      Console.SetCursorPosition(0, 0);
//      Console.Write("Map");
//      Console.SetCursorPosition(col2Left, 0);
//      Console.Write("Players");
//      Console.SetCursorPosition(0, 1);
//      Console.WriteLine(new String('-', Console.WindowWidth));
//      var currentMap = CurrentMaps[CurrentGame.MapId];
//      for (int i = 0; i < currentMap.Length; i++) {
//          Console.SetCursorPosition(0, 2 + i);
//          var row = currentMap[i];
//          Console.Write(String.Join("", row));
//      }

      // Write out the players
//      for (int i = 0; i < CurrentGame.Players.Count; i++) {
//          Console.SetCursorPosition(col2Left, 2 + i);
//          Console.Write(String.Format("{2}{0} - {3}pts", CurrentGame.Players[i].Player.Name, CurrentGame.Players[i].Id, CurrentGame.Players[i].IsAlive ? "" : "x", CurrentGame.Players[i].Score));
//      }

//      Console.SetCursorPosition(0, Console.WindowHeight - 4);

//      var options = new Dictionary<char, Action> {
//          { 'R', Refresh },
//          { 'X', ListGames },
//      };
//      if (CurrentGame.Started == false) {
//          options.Add('S', StartGame);
//          Console.WriteLine("[S] Start");
//      }
//      Console.WriteLine("[R] Refresh");
//      Console.WriteLine("[X] Exit");

//      Choose(options);
  }

  return {
    run : run
  };
};