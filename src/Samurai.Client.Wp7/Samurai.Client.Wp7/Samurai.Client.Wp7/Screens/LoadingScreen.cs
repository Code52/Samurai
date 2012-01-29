using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNInterface.Controls;

namespace Samurai.Client.Wp7.Screens
{
    public class LoadingScreen : BaseScreen
    {
        private ContentManager content;
        private SpriteBatch sb;
        private Window gui;

        public override void LoadContent()
        {
            if (IsReady)
                return;

            content = new ContentManager(Manager.Game.Services, "Content");
            sb = new SpriteBatch(Manager.GraphicsDevice);
            gui = content.Load<Window>("GUI\\Loading");
            gui.Initialise(null);
            gui.LoadGraphics(Manager.GraphicsDevice, content);
            IsReady = true;
            base.LoadContent();
        }

        public override void Update(double elapsedSeconds)
        {
            gui.PerformLayout(Manager.GraphicsDevice.Viewport.Width, Manager.GraphicsDevice.Viewport.Height);
            gui.Update(elapsedSeconds);
            base.Update(elapsedSeconds);
        }

        public override void Draw(double elapsedSeconds, GraphicsDevice device)
        {
            sb.Begin();
            gui.Draw(device, sb, elapsedSeconds);
            sb.End();
            base.Draw(elapsedSeconds, device);
        }
    }
}
