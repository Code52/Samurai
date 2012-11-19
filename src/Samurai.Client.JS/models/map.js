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
      r = 0, c = 0;

  for( ; r < this.Tiles.length; r++) {
    strMap[r] = [];
    for(c = 0; c < this.Tiles[0].length; c++) {
      strMap[r][c] = this.Tiles[r][c].toString();
    }
  }

  return strMap;
};

Map.prototype.initialize = function (opt) {
  var key = null,
  default_args = {
//      public Guid Id { get; private set; }
      Id : null,
      Name : '',
      ImageResource : '',
      MinPlayers : 0,
      MaxPlayers : 0,
//      public Dictionary<int, List<Unit>> StartingUnits { get; set; }
      StartingUnits : {},
//      public TileType[][] Tiles
      Tiles : []
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
        Id : id,
        Tiles : new Array(rows[0].length)
      }),
      x = 0, y = 0,
      t;          //TileType t;

  for( ; x < m.Tiles.length; x++) {
    m.Tiles[x] = new Array(rows.length);
      for(y = 0; y < rows.length; y++) {
        switch(rows[x][y]) {
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

        m.Tiles[x][y] = t;
      }
    }

  return m;
};