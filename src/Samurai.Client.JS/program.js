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


  return {
    run : run
  };
};