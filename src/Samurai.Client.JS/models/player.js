function Player(opt) {
  if(opt)
    this.initialize(opt);
  else {
  //Guid
    this.id = null;

    this.name = '';
    this. apiKey = '';

    this.wins = 0;
    this.gamesPlayed = 0;

  //DateTime LastSeen { get; set; }
    this.lastSeen = new Date();

    this.isOnline = false;
    this.isActive = false;
  }
}

Player.prototype.initialize = function (opt) {
  var key = null,
  default_args = {
    'id' : null,
    'name' : '',
    'apiKey' : '',
    'wins' : 0,
    'gamesPlayed' : 0,
    'lastSeen' : new Date(),
    'isOnline' : false,
    'isActive' : false
  };

  opt = (opt || default_args);

  for(key in default_args) {
    if(typeof opt[key] == "undefined")
      opt[key] = default_args[key];
  }

  // opt[] has all the data - user provided and optional.
  for(key in opt) {
    this[key] = opt[key];
  }
};