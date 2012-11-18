function GameState(opts) {
  this.initialize(opts);
}

GameState.prototype.initialize = function (opt) {
  var key = null,
      default_args = {
        //public Guid Id
        Id : '',
        Name : '',  // NOTE: what does this represent? <csainty: Lets the user name a game so they can tell a friend which to join. Could use invites instead>
        //List<GamePlayer>
        Players : [],
        Turn : 0,
        Started : false,
        //public Guid
        MapId : '',
        //List<Guid>
        PlayerOrder : [],
      };

  opt || (opt = default_args);

  for(key in default_args) {
    if(typeof opt[key] == "undefined") opt[key] = default_args[key];
  }

  // opt[] has all the data - user provided and optional.
  for(key in opt) {
    this[key] = opt[key];
  }
};