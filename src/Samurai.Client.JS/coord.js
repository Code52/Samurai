/** Coordinate object. Represents a point in 2-space.
 */
function Coord(opt) {
  this.initialize(opt);
}


Coord.prototype.initialize = function (opt) {
  var key = null,
      default_args = {
        'x' : 0,
        'y' : 0
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

/** Move this Coord to a new (x, y) location.
 * @param {Number} x
 * @param {Number} y
 */
Coord.prototype.move = function (x, y) {
  this.x = x;
  this.y = y;

  return this;
};

/** Add [Coord] other to this and return the result. */
Coord.prototype.plus = function (other) {
  return new Coord({x : this.x + other.x, y : this.y + other.y});
};

/** Subtracts [Coord] other from this and return the result.
 * @param {Coord} other
 */
Coord.prototype.minus = function (other) {
//    return new Coord.init(this.x - other.x, this.y - other.y);
  return new Coord({x : this.x - other.x, y : this.y - other.y});
};

/** Scales this [Coord] by the factor specified.
 * @param {Number} scale
 */
Coord.prototype.scale = function (scale) {
  return new Coord({x : this.x * scale, y : this.y * scale});
};

/** Returns distance from this [Coord] to [Coord] b.
 * @param {Coord} b
 */
Coord.prototype.dist = function (b) {
  return Math.sqrt((this.x - b.x)*(this.x - b.x) + (this.y - b.y)*(this.y - b.y));
};

/** Returns the midpoint of this [Coord] and [Coord] b.
 * @param {Coord} b
 */
Coord.prototype.midpoint = function (b) {
  return new Coord({
      x : (this.x + b.x) / 2,
      y : (this.y + b.y) / 2
  });
};