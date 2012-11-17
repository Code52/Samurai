function GamePlayer(opt) {            
  //Units = new List<Unit>();
  this.initialize(opt);
}

GamePlayer.prototype.initialize = function (opt) {
  var key = null,
      default_args = {
        //Guid 
        id  : '',
        //Player 
        player  : null,
        //IList<Unit> 
        units  : [],
        score : 0,
        isAlive : true,
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