function Unit(opts) {
  this.initialize(opts);
  this.currentHitPoints = this.hitPoints;
}

Unit.prototype.initialize = function (opt) {
  var key = null,
      default_args = {
        //Guid Id
        'id' : null,
        'name' : '',
        'imageSpriteResource' : '',

        'moves' : 0,
        //double
        'attack' : 0,
        'defence' : 0,
        'range' : 0,

        'hitPoints' : 0,

        'x' : 0,
        'y' : 0,
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