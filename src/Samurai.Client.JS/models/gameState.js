function GameState(opts) {
  this.initialize(opts);
}

GameState.prototype.initialize = function (opt) {
  var key = null,
      default_args = {
        //public Guid Id 
        'id' : '',
        'name' : '',  // NOTE: what does this represent? <csainty: Lets the user name a game so they can tell a friend which to join. Could use invites instead>
        //List<GamePlayer>
        'players' : [],
        'turn' : 0,
        'started' : false,
        //public Guid
        'mapId' : '',
        //List<Guid>
        'playerOrder' : [], 
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