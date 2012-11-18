function Unit(opts) {
  this.initialize(opts);
  this.currentHitPoints = this.hitPoints;
}

Unit.prototype.initialize = function (opt) {
  var key = null,
      default_args = {
        //Guid Id
        'Id' : null,
        'Name' : '',
        'ImageSpriteResource' : '',

        'Moves' : 0,
        //double
        'Attack' : 0,
        'Defence' : 0,
        'Range' : 0,

        'HitPoints' : 0,

        'X' : 0,
        'Y' : 0
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