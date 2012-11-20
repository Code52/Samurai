//using SamuraiServer.Data.Tiles;
define(['tiles'], function (Tiles) {

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
      Id : '',
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
          Tiles : new Array(rows.length)
        }),
        x = 0, y = 0,
        t;          //TileType t;

    for( ; x < m.Tiles.length; x++) {
      m.Tiles[x] = new Array(rows[0].length);
        for(y = 0; y < rows[0].length; y++) {
        switch(rows[x][y]) {
          case '.':
              t = new Tiles.Grass();
              break;

          case '@':
              t = new Tiles.Rock();
              break;

          case 'T':
              t = new Tiles.Tree();
              break;

          case '~':
                t = new Tiles.Water();
                break;

            default:
                continue;
          }

          m.Tiles[x][y] = t;
        }
      }

    return m;
  };

  return Map;
});