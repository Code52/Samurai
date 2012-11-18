//using SamuraiServer.Data.Tiles;

function Map(opt) {
//  public Map()
//  {
//      Id = Guid.NewGuid();
//  }

  this.initialize(opt);
}

Map.prototype.toString = function () {
//  return Tiles.Select(t => new string(t.Select(t2 => t2.StringRepresentation).ToArray())).ToArray();
};

Map.prototype.stringRepresentation = function () {
  var strMap = [],
      r = c = 0;

  for( ; r < this.tiles.length; r++) {
    for(; c < this.tiles[0].length; c++) {
      strMap[r][c] = this.tiles[r][c].toString();
    }
  }

  return strMap;
};

Map.prototype.initialize = function (opt) {
  var key = null,
  default_args = {
//      public Guid Id { get; private set; }
      'id' : null,
      name : '',
      imageResource : '',
      minPlayers : 0,
      maxPlayers : 0,
//      public Dictionary<int, List<Unit>> StartingUnits { get; set; }
      startingUnits : {},
//      public TileType[][] Tiles
      'tiles' : []
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

Map.prototype.fromStringRepresentation = function fromString(id, rows) {
  var m = new Map({
        id : id,
        tiles : new Array(rows[0].length)
      }),
      x = y = 0,
      t;          //TileType t;

  for( ; x < m.tiles.length; x++) {
    m.tiles[x] = new Array(rows.length);
      for( ; y < rows.length; y++) {
        switch (rows[y][x]) {
          case '.':
              t = new Grass();
              break;

          case '@':
              t = new Rock();
              break;

          case 'T':
              t = new Tree();
              break;

          case '~':
              t = new Water();
              break;

          default:
              continue;
        }

        m.tiles[x][y] = t;
      }
  }

  return m;
};