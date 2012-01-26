using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SamuraiServer.Data;

namespace Samurai.Client.Wp7.Graphics
{
    public class Renderer
    {
        private const int CellWidth = 32;
        private readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private Rectangle drawRect = new Rectangle(0, 0, CellWidth, CellWidth);

        public void LoadContent(ContentManager content)
        {
        }

        public void DrawMap(GraphicsDevice device, SpriteBatch sb, Map map, int xOffset, int yOffset)
        {
            if (sb == null || map == null)
                return;

            int xStart = xOffset % CellWidth;
            int yStart = yOffset % CellWidth;
            int width = Math.Min((xOffset / CellWidth) + (device.Viewport.Width / CellWidth), map.Tiles.GetLength(0));
            int height = Math.Min((yOffset / CellWidth) + (device.Viewport.Height / CellWidth), map.Tiles.GetLength(1));

            drawRect.X = -xStart;
            drawRect.Y = -yStart;

            for (int xIndex = xOffset / CellWidth; xIndex < width; ++xIndex)
            {
                for (int yIndex = yOffset / CellWidth; yIndex < height; ++yIndex)
                {
                    Texture2D tex;
                    if (textures.TryGetValue(map.Tiles[xIndex][yIndex].ImageSpriteResource, out tex))
                        sb.Draw(tex, drawRect, Color.White);
                    drawRect.Y += CellWidth;
                }
                drawRect.X += CellWidth;
            }
        }
    }
}
