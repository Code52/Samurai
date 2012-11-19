//public class Renderer {
function Renderer() {
//    private const int CELL_WIDTH = 64;
  /*const*/ var CELL_WIDTH = 64;
//    private readonly List<Texture2D> textures = new List<Texture2D>();
  var textures = [],
//    private Rectangle drawRect = new Rectangle(0, 0, CELL_WIDTH, CELL_WIDTH);
      drawRect = {x: 0, y: 0, width: CELL_WIDTH, height: CELL_WIDTH};

//    public void LoadContent(ContentManager content) {
  function loadContent(content) {
    var tmpImg = new Image();
//        textures.Clear();
    textures = [];

//    textures.Add(content.Load<Texture2D>("Textures\\grass"));
    //textures.push(content.loadTexture2D('../textures/grass.png');
    tmpImg.src = './textures/grass.png';
    textures.push(tmpImg);

//    textures.Add(content.Load<Texture2D>("Textures\\rock"));
    tmpImg = new Image();
    tmpImg.src = './textures/rock.png';
    textures.push(tmpImg);

//    textures.Add(content.Load<Texture2D>("Textures\\trees"));
    tmpImg = new Image();
    tmpImg.src = './textures/trees.png';
    textures.push(tmpImg);

//    textures.Add(content.Load<Texture2D>("Textures\\water"));
    tmpImg = new Image();
    tmpImg.src = './textures/water.png';
    textures.push(tmpImg);
  }

//    public void DrawMap(GraphicsDevice device, SpriteBatch sb, Map map, int xOffset, int yOffset) {
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