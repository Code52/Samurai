define(['contentmanager', 'map', 'renderer', 'serverApi'], function (ContentManager, Map, Renderer, ServerApi) {

  function Program() {
    var api = new ServerApi("http://samuraitest.apphb.com/"),


  //  static Player CurrentPlayer = null;
       currentPlayer = null,
       currentGames = {},
       currentMaps = {},
    //  static GameState CurrentGame = null;
       currentGame = null,

       userList = [],

       //Canvas vars
       $cvsFg = $('#cvsFg'),
       $cvsBg = $('#cvsBg'),
       ctxFg = $cvsFg[0].getContext("2d"),
       ctxBg = $cvsBg[0].getContext("2d"),
       cX,
       cY,

       //HTMLElements
       $chatbox = $('#chatbox'),
       $status = $('#status'),
       $btnChat = $('#btnChat'),
       $divList = $('#divList'),
       $menu = $('#menu'),

       //
       content = new ContentManager(),
       renderer = new Renderer();


    renderer.loadContent(content);
    //Size the canvases.
    $cvsBg[0].width = $cvsFg[0].width = $cvsFg.parent().width();
    $cvsBg[0].height = $cvsFg[0].height = $cvsFg.parent().height();
    cX = cvsFg.width;
    cY = cvsFg.height;

    ctxFg.font = '30px samurai';
    ctxFg.textBaseline = 'middle';

    //Event hookups!
    $('#btnPlay').click(listGames);
    $('#btnLogin').click(login);
    $('#btnLogout').click(logout);

    function run() {
      //Load main menu screen.
      welcome('Welcome');

      //Saved user test
      userList.push({name: 'shiftkey', key: 'someValue'});
      saveUsers();
      //loadUsers();
    }

  //  private static void Welcome(string message = "")
    function welcome(message) {
      var imgBg = content.loadTexture2D('./textures/samurai_background_800x480.png');

      //Draw menu background.
      $(imgBg).one('load', function () {
        ctxBg.drawImage(imgBg, 0, 0, imgBg.width, imgBg.height);
      });

      //Clear foreground canvas.
      $cvsFg[0].width = $cvsFg[0].width;
      $status.empty();

  //    ctxFg.fillText(message, cX / 2 - ctxFg.measureText(message).width / 2, cY / 2);
      if(message) {
        $status.html(message + '<br />')
            .show();
      }

      if(currentPlayer != null) {
        $status.append('Logged in as </br>'+ currentPlayer.Name + " " + currentPlayer.Id);
      }

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
      var name = '';

      $status.text('New player!');
      $chatbox.attr('placeholder', 'Please enter your username');
      $chatbox.focus();

      $btnChat.on('click', function gotName(data) {
        if(/\s/.test($chatbox.val()) || $chatbox.val() == '') {
          $status.text('Please enter your username. No whitespaces!');
          $chatbox.focus();
        } else {
          name = $chatbox.val();
          $chatbox.attr('placeholder', '');
          $btnChat.off('click', gotName);

          api.createPlayer(name, function (data, e) {
            if(e != null) {
  //            Error(e);
              $status.html('An error has occured:</br>' + e);
              console.log(e);
              return;
            }
            if(!data.ok) {
              welcome(data.Message);
              return;
            }

            currentPlayer = data.Player;
            console.log(currentPlayer);
            userList.push({name : currentPlayer.Name, key : currentPlayer.ApiKey});
            saveUsers();
  //          Console.WriteLine("Your generated key: {0}", CurrentPlayer.ApiKey);
  //          Console.ReadKey();
            $chatbox.val('');

            welcome();
          });
        }
      });
    }

    function login() {
      var name/* = currentPlayer.Name*/,
          key/* = currentPlayer.ApiKey*/;

      $status.text('');
      $('#tblList tbody').empty();

      //Load saved users.
      loadUsers();

      //Display user table and allow selection
      $.each(userList, function (index, user) {
        //TODO: Clientside templating, amiright?
        $('#tblList thead').html('<tr><th>Name</th><th>Key</th></tr>');
        $('#tblList tbody').append('<tr><td>'+user.name+'</td><td>'+user.key+'</td></tr>');
      });
      $('#tblList tbody').append('<tr><td>New user</td><td></td></tr>');

      $divList.show();

      $('#tblList tbody').one('click', 'tr', function selectUser() {
        name = $(this).children().first().text();
        key = $(this).children().last().text();

        $divList.hide();

        if(name == 'New user') {
          createUser();
          return;
        }

        //Attempt login.
        $status.html('Logging in as </br>' + name + ' ' + key);

  //    currentPlayer = null;

        api.login(name, key, function (data, e) {
          if(e != null) {
  //            Error(e);
            console.log('e', e);
            return;
          }

          if(!data.ok) {
            welcome(data.Message);
            return;
          }

          currentPlayer = data.Player;
          welcome();
  //           DeleteSave();
  //           Guide.BeginShowMessageBox("Error", "Failed to login: " + (e == null ? p.Message : e.Message), new string[] { "Ok" }, 0, MessageBoxIcon.Error, null, null);
  //              if(status != null)
  //                  status.Enabled = false;
  //              UpdateButtons();
        });
      });
    }

    function listGames() {
      var i = 0;
      if(currentPlayer == null) {
        welcome("Please log in.");
        return;
      }

      api.getOpenGames( function (data, e) {
        if(e != null) {
          //Error(e);
          console.log(e);
          return;
        }
        if(!data.ok) {
  //        BadResponse();
          welcome(data.message);
          return;
        }

        $menu.hide();
        $status.text('');
        $('#tblList tbody').empty();
        $('#tblList thead').html('<tr><th></th><th>Name</th><th>Id</th></tr>');

        //Add each game to the list of options.
        $.each(data.games, function (index, game){
          updateGame(game);
          $('#tblList tbody').append('<tr><td>'+(index+1)+'</td><td>'+game.Name+'</td><td>'+game.Id+'</td></tr>');
        });
        $('#tblList tbody').append('<tr><td></td><td>Create New</td><td></td></tr>');

        $divList.show();

        $('#tblList tbody').one('click', 'tr', function selectGame() {
          name = $(this).children().first().next().text();
          id = $(this).children().last().text();

          $divList.hide();

          if(name == 'Create New') {
            createGame();
            return;
          }

          //Attempt to join existing game.
          $status.html('Joining ' + name);
          joinGame(id);
        });
      });
    }

    /** Create a new game. */
    function createGame() {
      //Get game name.
      $chatbox.attr('placeholder', 'Enter the name of your game.')
          .val('')
          .show();
      $status.text('Enter the name of your game below.');

      $btnChat.show()
          .one('click', function gotGameName() {
            if($chatbox.val() == '') {
              $chatbox.focus();
              $btnChat.one('click', gotGameName);
            } else {
              $btnChat.off('click', gotGameName);

              //Send create request to server
              api.createGameAndJoin($chatbox.val(), currentPlayer.Id, function (data, e) {
                  if(e != null) {
                    //Error(e);
                    console.log(e);
                    return;
                  }
                  if(!data.ok) {
                    welcome(data.Message);
                    return;
                  }

                  $chatbox.attr('placeholder', '');
                  updateGame(data.game);
                  viewGame(data.game.Id);
              });
            }
          });
    }

    /** Retrieve a map from the server.
     * @param {GameState} game
     */
    function getMap(game) {
      var map;

      api.getMap(game.MapId, function (data, e) {
        if(e != null) {
  //        Error(e);
          console.log('data:',data);
          console.log(e);
          return;
        }
        if(!data.ok) {
  //        BadResponse();
          return;
        }

        //Parse string representation and create new map object
        map = new Map().fromStringRepresentation(game.MapId, data.Map.Tiles);

        updateMap(game, map);
      });
    }

    /** Update or create GameState in currentGames list.
     * @param {GameState} game
     */
    function updateGame(game) {
      currentGames[game.Id] = game;
    }

    /** Update a map in client's list of saved maps.
     * @param {GameState} game
     * @param {Map} map
     */
    function updateMap(game, map) {
      currentMaps[game.MapId] = map;
    }

  //  private static void JoinGame(Guid id)
    /** Join the game specified by id. Id should be a GUID...
     * @param {String} id
     */
    function joinGame(id) {
      var game = currentGames[id];

      $.each(game.Players, function (index, player) {
        if(player.Id == currentPlayer.Id) {
          viewGame(id);
          //TODO: Test this. Potential bug.
          return;
        }
      });

      api.joinGame(id, currentPlayer.Id, function (data, e) {
        if(e != null) {
  //        Error(e);
          console.log(e);
          return;
        }
        if(!data.ok) {
          welcome(data.Message);
          return;
        }

        updateGame(data.Game);
        viewGame(data.Game.Id);
      });
    }

  //  private static void StartGame() {
    function startGame() {
      api.startGame(currentGame.id, function (data, e) {
        if(e != null) {
            //Error(e);
          console.log(e);
          return;
        }
        if(!data.ok) {
          welcome(data.Message);
          return;
        }

        updateGame(data.Game);
        viewGame(CurrentGame.Id);
      });
    }

  //  private static void ViewGame(Guid id)
    /**
     * @param {String} id
     */
    function viewGame(id) {
      currentGame = currentGames[id];

      if(!currentMaps[currentGame.MapId])
        getMap(currentGame);

  //      int col2Left = Console.WindowWidth - 20;

      //Hide the buttons.
      $menu.hide();

      $status.hide();
      $chatbox.val('');

      //Clear the background.
      ctxBg.fillStyle = 'rgb(230, 219, 181)';
      ctxBg.fillRect(0, 0, cX, cY);

      //Draw the map.
      renderer.drawMap(ctxBg, currentMaps[currentGame.MapId], 0, 0);

      //Draw the players/units?

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

    /** Clear the current user and return to main menu.*/
    function logout() {
      currentPlayer = null;
      welcome('Welcome');
    }

    /** Loads previous users from localStorage */
    function loadUsers() {
      userList = JSON.parse(localStorage.getItem('userList'));
    }

    /** Save user list to local storage */
    function saveUsers() {
      localStorage.setItem('userList', JSON.stringify(userList));
    }

    //From diveintohtml5.info
    function supports_html5_storage() {
      try {
        return 'localStorage' in window && window['localStorage'] !== null;
      } catch (e) {
        return false;
      }
    }

    return {
      run : run
    };
  }

  return Program;
});