function Move(opt) {
  //public Player Player { get; set; }
  //public List<Unit> PlayersUnits { get; set; }
  //public Dictionary<Player, List<Unit>> AffectedUnits { get; set; }
  this.initialize(opt);
}

Move.prototype.initialize = function (opt) {
  var key = null,
      default_args = {
        Player : null,
        PlayersUnits : [],
        AffectedUnits : {}
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