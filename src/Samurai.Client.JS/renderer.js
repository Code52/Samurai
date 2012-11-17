//public class Renderer {
function Renderer() {
//    private const int CELL_WIDTH = 64;
  /*const*/ var CELL_WIDTH = 64;
//    private readonly List<Texture2D> textures = new List<Texture2D>();
  var textures = [],
//    private Rectangle drawRect = new Rectangle(0, 0, CELL_WIDTH, CELL_WIDTH);
      drawRect = new Rectangle(0, 0, CELL_WIDTH, CELL_WIDTH);

//    public void LoadContent(ContentManager content) {
  function loadContent(content) {
//        textures.Clear();
    textures = [];
//    textures.Add(content.Load<Texture2D>("Textures\\grass"));
    //textures.push();
//    textures.Add(content.Load<Texture2D>("Textures\\rock"));
//    textures.Add(content.Load<Texture2D>("Textures\\trees"));
//    textures.Add(content.Load<Texture2D>("Textures\\water"));
  }

//    public void DrawMap(GraphicsDevice device, SpriteBatch sb, Map map, int xOffset, int yOffset) {
  function drawMap(context, sb, map, xOffset, yOffset) {
//        if (sb == null || map == null)
//            return;
    var xIndex = xOffset,
        xStart = xOffset % CELL_WIDTH,
        yStart = yOffset % CELL_WIDTH//,
        //width = Math.Min((xOffset / CELL_WIDTH) + (device.Viewport.Width / CELL_WIDTH) + 2, map.Tiles.Length),
        //All columns are of equal height.
        //height = Math.Min((yOffset / CELL_WIDTH) + (device.Viewport.Height / CELL_WIDTH) + 2, map.Tiles[0].Length)
        ;

//        int xStart = xOffset % CELL_WIDTH;
//        int yStart = yOffset % CELL_WIDTH;
//        int width = Math.Min((xOffset / CELL_WIDTH) + (device.Viewport.Width / CELL_WIDTH) + 2, map.Tiles.Length);
//        int height = Math.Min((yOffset / CELL_WIDTH) + (device.Viewport.Height / CELL_WIDTH) + 2, map.Tiles[0].Length); // All columns are of equal height

//        drawRect.X = -xStart;
//        for (int xIndex = xOffset / CELL_WIDTH; xIndex < width && xIndex >= 0; ++xIndex) {
//            drawRect.Y = -yStart;
//            for (int yIndex = yOffset / CELL_WIDTH; yIndex < height && yIndex >= 0; ++yIndex) {
//                var tex = GetTex(map.Tiles[xIndex][yIndex]);
//                if (tex != null)
//                    sb.Draw(tex, drawRect, Color.White);
//                drawRect.Y += CELL_WIDTH;
//            }
//            drawRect.X += CELL_WIDTH;
//        }
    }

//    public Point GetMapSize(Map map) {
  function getMapSize(map) {
    var x = map.Tiles.Length;
        y = map.Tiles[0].Length;
//        return new Point(x * CELL_WIDTH, y * CELL_WIDTH);
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