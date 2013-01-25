define(function () {
  function Player(opt) {
    this.initialize(opt);
  }

  Player.prototype.initialize = function (opt) {
    var key = null,
    default_args = {
      'Id' : null,
      'Name' : '',
      'ApiKey' : '',
      'Wins' : 0,
      'GamesPlayed' : 0,
      'LastSeen' : new Date(),
      'IsOnline' : false,
      'IsActive' : false
    };

    opt || (opt = default_args);

    for(key in default_args) {
      if(typeof opt[key] == "undefined")
        opt[key] = default_args[key];
    }

    // opt[] has all the data - user provided and optional.
    for(key in opt) {
      this[key] = opt[key];
    }
  };

  return Player;
});