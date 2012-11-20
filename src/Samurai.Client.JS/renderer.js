define(['coord', 'map', 'contentmanager'], function (Coord, Map, ContentManager) {

  function Renderer() {
    var CELL_WIDTH = 64,
  //    private readonly List<Texture2D> textures = new List<Texture2D>();
        textures = [],
        drawRect = {
            x: 0,
            y: 0,
            width: CELL_WIDTH,
            height: CELL_WIDTH
        };

  //    public void LoadContent(ContentManager content) {
    function loadContent(content) {
      var tmpImg = new Image();

      textures = [];

      textures.push(content.loadTexture2D('./textures/grass.png'));
      textures.push(content.loadTexture2D('./textures/rock.png'));
      textures.push(content.loadTexture2D('./textures/trees.png'));
      textures.push(content.loadTexture2D('./textures/water.png'));
    }

    /**
     * Draw the supplied map using context at the specified x,y offset.
     * @param {drawingContext2d} context
     * @param {Map} map
     * @param {Number} xOffset
     * @param {Number} yOffset
     * @returns
     */
    function drawMap(context, /*sb,*/ map, xOffset, yOffset) {
  //        if (sb == null || map == null)
      if(/*!sb ||*/ !map) {
        console.log('map', map);
        return;
      }

      var xIndex = xOffset / CELL_WIDTH,
          yIndex = yOffset / CELL_WIDTH,
          xStart = xOffset % CELL_WIDTH,
          yStart = yOffset % CELL_WIDTH,
          width = Math.min((xOffset / CELL_WIDTH) + ($('#viewport').width() / CELL_WIDTH) + 2, map.Tiles.length),
          //All columns are of equal height.
          height = Math.min((yOffset / CELL_WIDTH) + ($('#viewport').height() / CELL_WIDTH) + 2, map.Tiles[0].length),
          tex = null;

      drawRect.y = -yStart;

      for( ; xIndex < width && xIndex >= 0; ++xIndex) {
        drawRect.x = -xStart;

        for(yIndex = yOffset / CELL_WIDTH; yIndex < height && yIndex >= 0; ++yIndex) {
          tex = getTex(map.Tiles[xIndex][yIndex]);

          if(tex) {
              //sb.Draw(tex, drawRect, Color.White);
            context.drawImage(tex, drawRect.x, drawRect.y, drawRect.width, drawRect.height);
          }

          drawRect.x += CELL_WIDTH;
        }
        drawRect.y += CELL_WIDTH;
      }
    }

  //    public Point GetMapSize(Map map) {
    function getMapSize(map) {
      var x = map.Tiles.Length;
          y = map.Tiles[0].Length;

        return new Coord({x : x * CELL_WIDTH, y : y * CELL_WIDTH});
    }

  //    protected Texture2D GetTex(TileType cell) {
    function getTex(cell) {
      switch (cell.Name) {
          case "Grass":
              return textures[0];
          case "Rock":
              return textures[1];
          case "Tree":
              return textures[2];
          case "Water":
              return textures[3];

          default:
              return null;
      }
    }

    return {
      loadContent : loadContent,
      drawMap : drawMap,
      getMapSize : getMapSize
    };
  }

  return Renderer;
});