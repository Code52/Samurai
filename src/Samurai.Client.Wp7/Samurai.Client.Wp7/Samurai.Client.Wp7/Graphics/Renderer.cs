using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SamuraiServer.Data;
using SamuraiServer.Data.Tiles;

namespace Samurai.Client.Wp7.Graphics
{
    public class Renderer
    {
        private const int CellWidth = 32;
        private readonly List<Texture2D> textures = new List<Texture2D>();
        private Rectangle drawRect = new Rectangle(0, 0, CellWidth, CellWidth);

        public void LoadContent(ContentManager content)
        {
            textures.Clear();
            textures.Add(content.Load<Texture2D>("Textures\\grass"));
            // TODO: Uncomment when other tile type images added
            //textures.Add(content.Load<Texture2D>("Textures\\rock"));
            //textures.Add(content.Load<Texture2D>("Textures\\tree"));
            //textures.Add(content.Load<Texture2D>("Textures\\water"));
        }

        public void DrawMap(GraphicsDevice device, SpriteBatch sb, Map map, int xOffset, int yOffset)
        {
            if (sb == null || map == null)
                return;

            int xStart = xOffset % CellWidth;
            int yStart = yOffset % CellWidth;
            int width = Math.Min((xOffset / CellWidth) + (device.Viewport.Width / CellWidth), map.Tiles.Length);
            int height = Math.Min((yOffset / CellWidth) + (device.Viewport.Height / CellWidth), map.Tiles[0].Length); // All columns are of equal height

            drawRect.X = -xStart;
            for (int xIndex = xOffset / CellWidth; xIndex < width && xIndex >= 0; ++xIndex)
            {
                drawRect.Y = -yStart;
                for (int yIndex = yOffset / CellWidth; yIndex < height && yIndex >= 0; ++yIndex)
                {
                    var tex = GetTex(map.Tiles[xIndex][yIndex]);
                    if (tex != null)
                        sb.Draw(tex, drawRect, Color.White);
                    drawRect.Y += CellWidth;
                }
                drawRect.X += CellWidth;
            }
        }

        protected Texture2D GetTex(TileType cell)
        {
            if (cell is Grass) return textures[0];
            // TODO: Uncomment when other tile type images added
            //if (cell is Rock) return textures[1];
            //if (cell is Tree) return textures[2];
            //if (cell is Water) return textures[3];

            return null;
        }
    }
}
